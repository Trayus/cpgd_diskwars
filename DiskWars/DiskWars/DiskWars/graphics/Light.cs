using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Graphics2D
{
    /// <summary>
    /// Represents a light to be used in a rendering engine
    /// </summary>
    class Light
    {
        /// <summary>
        /// The position of the light
        /// </summary>
        public Vector2 position;
        /// <summary>
        /// The color of the light
        /// </summary>
        public Color color;
        /// <summary>
        /// The power of the light (1 = total, 0 = nothing)
        /// </summary>
        public float power;
        /// <summary>
        /// The radius of the light
        /// </summary>
        public int size;
        /// <summary>
        /// Whether the light is active
        /// </summary>
        private bool enabled;
        /// <summary>
        /// The id of the light (optional)
        /// </summary>
        private String id;

        /// <summary>
        /// returns whether the light is on
        /// </summary>
        /// <returns>if the light is on</returns>
        public bool isEnabled()
        {
            return enabled;
        }
        /// <summary>
        /// Sets the light on or off
        /// </summary>
        /// <param name="on">whether the light should be on or off</param>
        public void setEnabled(bool on)
        {
            enabled = on;
        }
        /// <summary>
        /// Returns the ID of the light (optional: for using a single light in multiple objects)
        /// </summary>
        /// <returns></returns>
        public String getID()
        {
            return id;
        }
        /// <summary>
        /// Returns the color of the light as a vector4
        /// </summary>
        /// <returns>the color</returns>
        public Vector4 getColor()
        {
            return color.ToVector4();
        }

        /// <summary>
        /// Creates a new light without an ID
        /// </summary>
        /// <param name="color">the color of the light</param>
        /// <param name="position">the position of the light</param>
        /// <param name="power">The intensity of the light</param>
        /// <param name="size">the radius of the light</param>
        public Light(Color color, Vector2 position, float power, int size)
        {
            id = "";
            enabled = true;
            this.color = color;
            this.position = position;
            this.power = power;
            this.size = size;
            RenderingEngine.instance.addLight(this);
        }
        /// <summary>
        /// Creates a new light with an ID
        /// </summary>
        /// <param name="color">the color of the light</param>
        /// <param name="position">the position of the light</param>
        /// <param name="power">The intensity of the light</param>
        /// <param name="size">the radius of the light</param>
        /// <param name="ID">The ID for the light</param>
        public Light(Color color, Vector2 position, float power, int size, String ID)
        {
            id = ID;
            enabled = true;
            this.color = color;
            this.position = position;
            this.power = power;
            this.size = size;
            RenderingEngine.instance.addLight(this);
        }
        /// <summary>
        /// Removes the light from the rendering engine
        /// </summary>
        public void removeFromRenderingEngine()
        {
            RenderingEngine.instance.removeLight(this);
        }

    }
}
