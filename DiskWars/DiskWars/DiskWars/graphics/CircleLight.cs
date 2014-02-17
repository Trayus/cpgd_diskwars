using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Graphics2D
{
    class CircleLight : Light
    {
        public int minsize, maxsize;
        public bool hardedge;

        public CircleLight(Color color, Vector2 position, float power, int minsize, int maxsize, bool hardedge)
            : base(color, position, power, maxsize)
        {
            this.hardedge = hardedge;
            this.maxsize = maxsize;
            this.minsize = minsize;
        }
        public CircleLight(Color color, Vector2 position, float power, int minsize, int maxsize, bool hardedge, string id)
            : base(color, position, power, maxsize, id)
        {
            this.hardedge = hardedge;
            this.maxsize = maxsize;
            this.minsize = minsize;
        }

    }
}
