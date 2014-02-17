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
    class ReplayState : State
    {
        HUDText title;
        UniformBackground bg;
        SpriteFont customfont;
        float timer;

        public ReplayState(ContentManager content)
        {
            customfont = content.Load<SpriteFont>("menufont");
        }

        public override void update(float gameTime)
        {
            timer -= gameTime;

            if (timer < 0)
                Game1.goToState(State.MENU);

            if (OnceInput.A(1) || OnceInput.A(2) || OnceInput.A(3) || OnceInput.A(4) || OnceInput.ENTER())
            {
                Game1.goToState(State.GAME);
            }
        }

        public override void enterState()
        {
            title = new HUDText("Press 'A' to replay!", new Vector2(50, 50), customfont, Color.White);
            RenderingEngine.UI.addText(title);

            bg = new UniformBackground("bgs/dw", 0, 0);
            RenderingEngine.camera.setScale(Constants.DEBUG? 1366f / 1920f : 1);
            RenderingEngine.ambientLight = Color.White;
            RenderingEngine.camera.setPosition(Vector2.Zero);

            timer = Constants.TIMEREPLAY;
        }

        public override void exitState()
        {
            RenderingEngine.UI.removeAll();
            RenderingEngine.instance.removeAllBackgrounds();
        }

    }
}
