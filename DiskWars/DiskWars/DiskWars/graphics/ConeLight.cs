using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Graphics2D
{
    class ConeLight : Light
    {
        public Vector2 direction;
        public float angle;
        public bool hardedge;

        public ConeLight(Color color, Vector2 position, float power, int size, Vector2 direction, float angle, bool hardedge)
            : base(color, position, power, size)
        {
            this.direction = direction;
            this.angle = angle;
            this.hardedge = hardedge;
        }
        public ConeLight(Color color, Vector2 position, float power, int size, String id, Vector2 direction, float angle, bool hardedge)
            : base(color, position, power, size, id)
        {
            this.direction = direction;
            this.angle = angle;
            this.hardedge = hardedge;
        }

    }
}
