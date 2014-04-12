using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
        private SpriteFont customfont, customfont2;
        ContentManager content;

        float timer;
        HUDText time;
        Map map;
        Player[] players;
        HUDText[] scores;
        HUDText large;
        bool[] toggles;
        Random random = new Random();
        FileStream fs;

        Vector2[] spawns;

        public GameState(ContentManager content)
        {
            this.content = content;
            customfont = content.Load<SpriteFont>("gamefont");
            customfont2 = content.Load<SpriteFont>("menufont");
        }

        public override void update(float gameTime)
        {
            updateTimer(gameTime);
            checkEnabled();
            if (Keyboard.GetState().IsKeyDown(Keys.F8))
            {
                Game1.goToState(State.MENU);
                return;
            }

            if (timer < -Constants.TIMEFADE)
            {
                Game1.goToState(State.REPLAY);
            }
            else if (timer < Constants.TIMEMILLIS && timer > 0)
            {
                writeString("t = " + timer + " " + players[0].score + " " + players[1].score + " " + players[2].score + " " + players[3].score);

                // game logic 
                for (int i = 0; i < 4; i++)
                {
                    if (players[i].enabled)
                    {
                        writeString(players[i].getData());

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
                // check for respawning destructible tiles
                for (int i = 0; i < Constants.MAPX; i++)
                {
                    for (int j = 0; j < Constants.MAPY; j++)
                    {
                        if (map.tiles[i, j].wall == Map.WALL.destr && map.tiles[i, j].respawn > 0)
                        {
                            map.tiles[i, j].respawn -= gameTime;
                            if (map.tiles[i, j].respawn <= 0)
                            {
                                map.tiles[i, j].respawn = 0;
                                map.tiles[i, j].type = Map.TILE.wall;
                                map.tiles[i, j].anim.removeFromRenderingEngine();
                                map.tiles[i, j].anim = Animation.createSingleFrameAnimation("tiles/breakwall",
                                    new Vector2(map.tiles[i, j].anim.position.X, map.tiles[i, j].anim.position.Y), 0.1f);
                                SoundManager.PlaySound("sound/dw_breakon");
                            }
                        }
                    }
                }

                //POWERUPS
                for (int i = 0; i < map.powerUps.Count; i++)
                {
                    for (int j = 0; j < players.Length; j++)
                    {
                        if (players[j].alive && map.powerUps[i].animation.checkHit(players[j].animation))
                        {
                            players[j].powerUps.Add(map.powerUps[i]);
                            map.powerUps[i].animation.setVisible(false);
                            map.powerUps.Remove(map.powerUps[i--]);
                            SoundManager.PlaySound("sound/dw_pickup");
                            break;
                        }
                    }
                }

                for (int j = 0; j < players.Length; j++)
                {
                    for (int i = 0; i < players[j].powerUps.Count; i++)
                    {
                        if (players[j].powerUps[i].activeTime < 0)
                        {
                            players[j].powerUps[i].respawnTimer--;
                            if (players[j].powerUps[i].respawnTimer < 0)
                            {
                                players[j].powerUps[i].activeTime = Constants.POWERUPTIMER;
                                players[j].powerUps[i].respawnTimer = Constants.POWERUPRESPAWN;
                                map.powerUps.Add(players[j].powerUps[i]);
                                players[j].powerUps[i].animation.setVisible(true);
                                players[j].powerUps.Remove(players[j].powerUps[i]);
                            }
                        }
                    }
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

                large.setEnabled(true);
                large.setText("   " + ((int)(timer / 1000 + 1) - Constants.TIMEMILLIS / 1000) + "...");
            }
            else if (timer < 0)
            {
                time.setText("Game over!");
                time.setColor(Color.White);

                int winner = 0;
                if (players[1].score > players[winner].score) winner = 1;
                if (players[2].score > players[winner].score) winner = 2;
                if (players[3].score > players[winner].score) winner = 3;

                large.setEnabled(true);
                large.setText("Player " + (winner + 1) + " wins!");
                switch (winner)
                {
                    case 0: large.setColor(new Color(1f, 0.2f, 0.2f)); break;
                    case 1: large.setColor(new Color(1f, 1f, 0.2f)); break;
                    case 2: large.setColor(new Color(0.2f, 1f, 0.2f)); break;
                    case 3: large.setColor(new Color(0.2f, 0.2f, 1f)); break;
                }
            }
            else
            {
                time.setText((int)(timer / 1000 + 1) + "s");
                time.setColor(new Color((float)Math.Sin(timer / 100 + 1.6) / 2 + 1f,
                                        (float)Math.Sin(timer / 100 + 3.1) / 2 + 1f,
                                        (float)Math.Sin(timer / 100 + 0.0) / 2 + 1f));

                if (timer + 2000 > Constants.TIMEMILLIS)
                    large.setEnabled(true);
                else
                    large.setEnabled(false);
                large.setText("Begin!");

            }
        }
        private void checkEnabled()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.F1) && !toggles[0])
            {
                if (players[0].enabled)
                {
                    players[0].disable();
                    scores[0].setEnabled(false);
                }
                else
                {
                    players[0].enable();
                    scores[0].setEnabled(true);
                }
                toggles[0] = true;
                players[0].score = 0;
            }
            if (!Keyboard.GetState().IsKeyDown(Keys.F1) && toggles[0]) toggles[0] = false;

            if (Keyboard.GetState().IsKeyDown(Keys.F2) && !toggles[1])
            {
                if (players[1].enabled)
                {
                    players[1].disable();
                    scores[1].setEnabled(false);
                }
                else
                {
                    players[1].enable();
                    scores[1].setEnabled(true);
                }
                toggles[1] = true;
                players[1].score = 0;
            }
            if (!Keyboard.GetState().IsKeyDown(Keys.F2) && toggles[1]) toggles[1] = false;

            if (Keyboard.GetState().IsKeyDown(Keys.F3) && !toggles[2])
            {
                if (players[2].enabled)
                {
                    players[2].disable();
                    scores[2].setEnabled(false);
                }
                else
                {
                    players[2].enable();
                    scores[2].setEnabled(true);
                }
                toggles[2] = true;
                players[2].score = 0;
            }
            if (!Keyboard.GetState().IsKeyDown(Keys.F3) && toggles[2]) toggles[2] = false;

            if (Keyboard.GetState().IsKeyDown(Keys.F4) && !toggles[3])
            {
                if (players[3].enabled)
                {
                    players[3].disable();
                    scores[3].setEnabled(false);
                }
                else
                {
                    players[3].enable();
                    scores[3].setEnabled(true);
                }
                toggles[3] = true;
                players[3].score = 0;
            }
            if (!Keyboard.GetState().IsKeyDown(Keys.F4) && toggles[3]) toggles[3] = false;
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
            large = new HUDText("Starting", new Vector2(880, 500), customfont2, Color.White);
            RenderingEngine.UI.addText(large);
            toggles = new bool[4];

            this.spawns = map.spawns;

            for (int i = 0; i < 4; i++)
            {
                players[i] = new Player(map.spawns, i + 1, this);
                scores[i] = new HUDText("0", new Vector2(500 + i * 100, 20), customfont, (i == 0 ? Color.Red : (i == 1 ? Color.Yellow : (i == 2 ? Color.LightGreen : Color.Blue))));
                RenderingEngine.UI.addText(scores[i]);
                toggles[i] = false;
            }
            SoundManager.PlayMusicLooped("sound/4614(2)");

            fs = File.Open("dw_kb_" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
            writeString(mapname);
        }

        public void writeString(String s)
        {
            int i;
            byte[] chars = new byte[s.Length + 2];
            for (i = 0; i < s.Length; i++)
                chars[i] = (byte)s[i];
            chars[i++] = (byte)13;
            chars[i++] = (byte)10;
            fs.Write(chars, 0, s.Length + 2);
        }

        public override void exitState()
        {
            RenderingEngine.UI.removeAll();
            RenderingEngine.instance.removeAllAnimations();
            RenderingEngine.instance.removeAllBackgrounds();
            RenderingEngine.instance.removeAllLights();
            SoundManager.StopMusic();
            fs.Close();
        }
        public Vector2 getRandomSpawn()
        {
            int randomNumber;
            Boolean spaceEmpty = true;
            int tries = 0;

            do
            {
                randomNumber = random.Next(0, spawns.Length);
                spaceEmpty = true;

                for (int i = 0; i < 4; i++)
                {
                    //Test if any player is within two playerradius's from this spawn point.
                    spaceEmpty &= !((Math.Abs(players[i].animation.position.X - spawns[randomNumber].X) < Constants.PLAYERRADIUS * 2) && (Math.Abs(players[i].animation.position.Y - spawns[randomNumber].Y) < Constants.PLAYERRADIUS * 2));

                    //Test if any disk is within three playerradius's from this spawn point.
                    spaceEmpty &= !((Math.Abs(players[i].disk.getPosition().X - spawns[randomNumber].X) < Constants.PLAYERRADIUS * 3) && (Math.Abs(players[i].disk.getPosition().Y - spawns[randomNumber].Y) < Constants.PLAYERRADIUS * 3));
                }
                //This is used so that it doesn't keep looping forever if it couldn't find a spot.
                tries++;
                if (tries > spawns.Length)
                {
                    Console.Write("Failed to find suitable spawn.");
                    randomNumber = random.Next(0, spawns.Length);
                    spaceEmpty = true;
                }
            } while (!spaceEmpty);

            return spawns[randomNumber];
        }

    }
}
