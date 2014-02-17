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
    abstract class State
    {
        public const int MENU = 0, GAME = 1, REPLAY = 2;

        public abstract void update(float gameTime);
        public abstract void enterState();
        public abstract void exitState();
    }
}
