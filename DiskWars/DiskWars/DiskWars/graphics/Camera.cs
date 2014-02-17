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

namespace Graphics2D
{
    /// <summary>
    /// A camera (position, scale) that's capable of 'rumble' and zoom
    /// </summary>
    class Camera
    {
        /// <summary>
        /// The position of the camera
        /// </summary>
        Vector2 position;
        /// <summary>
        /// The position of the camera, taking into account rumbles
        /// </summary>
        Vector2 referencePoint;
        /// <summary>
        /// The rumble and scale values
        /// </summary>
        float rumbleforce = 0, totalrumbleforce = 0, scale = 1f;
        /// <summary>
        /// The time to rumble for
        /// </summary>
        int rumbletime = 0, totalrumbletime = 0;
        /// <summary>
        /// whether or not rumble goes to zero over time
        /// </summary>
        bool rumbleinterp = false;
        /// <summary>
        /// rng
        /// </summary>
        private Random rand = new Random();

        /// <summary>
        /// Creates a new camera with position 0,0
        /// </summary>
        public Camera()
        {
            position = new Vector2(0,0);
        }
        /// <summary>
        /// Creates a new camera at the specified location
        /// </summary>
        /// <param name="position">The camera's position</param>
        public Camera(Vector2 position)
        {
            this.position = position;
        }

        /// <summary>
        /// Returns the reference point (center point w/rumble) of the camera
        /// </summary>
        /// <returns>The reference point</returns>
        public Vector2 getRefPoint()
        {
            return referencePoint;
        }

        /// <summary>
        /// Sets the center point of the camera
        /// </summary>
        /// <param name="position">the new position</param>
        public void setPosition(Vector2 position)
        {
            this.position = position;
        }

        /// <summary>
        /// Updates the camera: selecting a new reference point if rumbling and updating rumble values
        /// </summary>
        /// <param name="gameTime"></param>
        public void update(float gameTime)
        {
            if (rumbletime > 0)
            {
                rumbletime -= (int)gameTime;
            }
            if (rumbletime < 0)
                rumbletime = 0;

            Vector2 randomval = new Vector2(0, 0);
            if (rumbletime != 0)
            {
                randomval.X = (float)rand.NextDouble() * rumbleforce;
                randomval.Y = (float)rand.NextDouble() * rumbleforce;
                if (rumbleinterp)
                {
                    rumbleforce = totalrumbleforce * ((float)rumbletime / (float)totalrumbletime);
                }
            }
            referencePoint = position + randomval;
        }

        /// <summary>
        /// Gets the scale of the camera
        /// </summary>
        /// <returns>current scale</returns>
        public float getScale()
        {
            return scale;
        }
        /// <summary>
        /// Sets the scale of the camera to the specified value
        /// </summary>
        /// <param name="scale">the new scale</param>
        public void setScale(float scale)
        {
            this.scale = scale;
        }
        /// <summary>
        /// Increments the scale by the given value
        /// </summary>
        /// <param name="inc">the amount to increment the scale by</param>
        public void increaseScale(float inc)
        {
            scale += inc;
        }

        /// <summary>
        /// Rumble at a constant value over a given amount of time
        /// </summary>
        /// <param name="intensity">the strength of the rumble</param>
        /// <param name="milliseconds">the duration of the rumble</param>
        public void rumbleConstant(float intensity, int milliseconds)
        {
            rumbleforce = intensity;
            totalrumbleforce = 0;
            rumbletime = milliseconds;
            totalrumbletime = 0;
            rumbleinterp = false;
        }
        /// <summary>
        /// Rumble over a given amount of time, decreasing the total rumble as time goes on, thereby ending at no rumble by the end
        /// </summary>
        /// <param name="intensity">The starting intesity of the rumble</param>
        /// <param name="milliseconds">The duration of the rumble</param>
        public void rumbleInterpolate(float intensity, int milliseconds)
        {
            rumbleforce = intensity;
            totalrumbleforce = intensity;
            rumbletime = milliseconds;
            totalrumbletime = milliseconds;
            rumbleinterp = true;
        }

    }
}
