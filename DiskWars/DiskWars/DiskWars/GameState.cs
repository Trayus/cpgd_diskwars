﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using Graphics2D;
using Helpers;


namespace DiskWars
{
    class GameState : State
    {
        public static String mapname = "initialized from menu state";
        private SpriteFont customfont;
        ContentManager content;

        float timer;
        HUDText time;
        Map map;
        Player[] players;
        HUDText[] scores;

        public GameState(ContentManager content)
        {
            this.content = content;
            customfont = content.Load<SpriteFont>("gamefont");
        }

        public override void update(float gameTime)
        {
            updateTimer(gameTime);
            if (timer < -Constants.TIMEFADE)
            {
                Game1.goToState(State.REPLAY);
            }
            else if (timer < Constants.TIMEMILLIS && timer > 0)
            {
                // game logic 
                for (int i = 0; i < 4; i++)
                {
                    players[i].update(gameTime);
                    players[i].collisionDetectionVsMap(map);
                    players[i].disk.update(gameTime);
                    players[i].disk.collisionDetectionVsMap(map);
                    for (int j = i + 1; j < 4; j++)
                    {
                        if (players[i].alive && players[j].alive)
                        {
                            if (!players[i].disk.collide(players[j].disk))
                            {
                                if (!players[j].holdingDisk)
                                    if (players[i].check(players[j].disk))
                                        players[j].score += Constants.SCOREPERKILL;
                                if (!players[i].holdingDisk)
                                    if (players[j].check(players[i].disk))
                                        players[i].score += Constants.SCOREPERKILL;
                            }
                        }
                    }
                    scores[i].setText(players[i].score + " ");
                }
            }
        }

        private void updateTimer(float gameTime)
        {
            timer -= gameTime;
            if (timer > Constants.TIMEMILLIS)
            {
                time.setText("start in " + ((int)(timer / 1000 + 1) - Constants.TIMEMILLIS / 1000) + " s");
                time.setColor(Color.White);
            }
            else if (timer < 0)
            {
                time.setText("Game over!");
                time.setColor(Color.White);
            }
            else
            {
                time.setText((int)(timer / 1000 + 1) + "s");
                time.setColor(new Color((float)Math.Sin(timer / 100 + 1.6) / 2 + 1f,
                                        (float)Math.Sin(timer / 100 + 3.1) / 2 + 1f,
                                        (float)Math.Sin(timer / 100 + 0.0) / 2 + 1f));
            }

            
        }

        public override void enterState()
        {
            new Background("bgs/bgtile", 0, 0);
            RenderingEngine.camera.setScale(Constants.GAMESCALE * (Constants.DEBUG ? 1366f / 1920f : 1));
            RenderingEngine.camera.setPosition(new Vector2(1920 / 2, 1080 / 2));
            RenderingEngine.ambientLight = Color.Gray;

            RenderingEngine.UI.addText(time = new HUDText("Timer", new Vector2(50, 20), customfont, Color.White));
            timer = Constants.TIMEINTRO + Constants.TIMEMILLIS;

            map = new Map(mapname, content);
            players = new Player[4];
            scores = new HUDText[4];
            for (int i = 0; i < 4; i++)
            {
                players[i] = new Player(map.spawns[i], i + 1);
                scores[i] = new HUDText("0", new Vector2(500 + i * 100, 20), customfont, (i == 0 ? Color.Red : (i == 1 ? Color.Yellow : (i == 2 ? Color.LightGreen : Color.Blue))));
                RenderingEngine.UI.addText(scores[i]);
            }
        }
        public override void exitState()
        {
            RenderingEngine.UI.removeAll();
            RenderingEngine.instance.removeAllAnimations();
            RenderingEngine.instance.removeAllBackgrounds();
            RenderingEngine.instance.removeAllLights();
        }

    }
}