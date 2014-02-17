using System;
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
    class MenuState : State
    {
        List<HUDText> maps;
        HUDText title;
        UniformBackground bg;
        SpriteFont customfont;
        int selection;

        public MenuState(ContentManager content)
        {
            customfont = content.Load<SpriteFont>("menufont");
        }

        public override void update(float gameTime)
        {
            int psel = selection;
            if (OnceInput.DDOWN(1) || OnceInput.DOWN() || OnceInput.LSTICKDOWN(1))
            {
                selection++;
            }
            if (OnceInput.DUP(1) || OnceInput.UP() || OnceInput.LSTICKUP(1))
            {
                selection--;
            }
            if (psel != selection)
            {
                foreach (HUDText ht in maps)
                    ht.setColor(Color.Gray);
                if (selection < 0) selection = Constants.mapnames.Length - 1;
                if (selection >= Constants.mapnames.Length) selection = 0;
                maps[selection].setColor(Color.LightGray);
            }

            if (OnceInput.A(1) || OnceInput.ENTER())
            {
                GameState.mapname = "maps/" + maps[selection].getText();
                Game1.goToState(State.GAME);
            }
        }

        public override void enterState()
        {
            int ndx = 0;
            title = new HUDText("Select a map to play!", new Vector2(50, 50), customfont, Color.White);
            RenderingEngine.UI.addText(title);
            maps = new List<HUDText>();
            foreach (String s in Constants.mapnames)
            {
                maps.Add(new HUDText(s, new Vector2(100, 150 + 100 * ndx), customfont, Color.Gray));
                RenderingEngine.UI.addText(maps[ndx++]);
            }
            maps[0].setColor(Color.LightGray);
            bg = new UniformBackground("bgs/dw", 0, 0);
            RenderingEngine.camera.setScale(Constants.DEBUG? 1366f / 1920f : 1);
            RenderingEngine.ambientLight = Color.White;
            RenderingEngine.camera.setPosition(Vector2.Zero);
        }

        public override void exitState()
        {
            RenderingEngine.UI.removeAll();
            RenderingEngine.instance.removeAllBackgrounds();
        }

    }
}
