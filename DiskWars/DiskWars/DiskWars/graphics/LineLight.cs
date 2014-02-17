using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Graphics2D
{
    class LineLight : Light
    {
        public Vector2 endpoint;
        public float radius;
        public bool hardedge;

        public LineLight(Color color, Vector2 position, float power, Vector2 relativeendpoint, float radius, bool hardedge)
            : base(color, position, power, (int)Vector2.Distance(position, position + relativeendpoint))
        {
            this.endpoint = relativeendpoint;
            this.radius = radius;
            this.hardedge = hardedge;
        }
        public LineLight(Color color, Vector2 position, float power, String id, Vector2 relativeendpoint, float radius, bool hardedge)
            : base(color, position, power, (int)Vector2.Distance(position, position + relativeendpoint), id)
        {
            this.endpoint = relativeendpoint;
            this.radius = radius;
            this.hardedge = hardedge;
        }
    }
}
