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
    /// Represents a tiled background that features parallax scrolling
    /// </summary>
    class Background
    {
        /// <summary>
        /// The normals and colors of the background
        /// </summary>
        Texture2D image, normals;
        /// <summary>
        /// The intesity of parallax scrolling (1 = same rate as camera) and layer depth of background
        /// </summary>
        float parallax, depth;

        /// <summary>
        /// Creates a new background and adds it to the rendering engine
        /// </summary>
        /// <param name="filename">The name of the image(s) to use </param>
        /// <param name="parallaxIntensity">how much it parallax scrolls (greater than 1 = faster than camera, less than 1 = slower than camera)</param>
        /// <param name="depth">The layer depth of the background image</param>
        public Background(String filename, float parallaxIntensity, float depth)
        {
            image = RenderingEngine.instance.requestTexture(filename);
            normals = RenderingEngine.instance.requestTexture(filename + "_normals");

            this.parallax = parallaxIntensity;
            this.depth = depth;

            RenderingEngine.instance.addBackground(this);
        }

        /// <summary>
        /// Draws the color map of the background in a tiled fashion, taking into account the parallax scrolling
        /// </summary>
        /// <param name="s">The spritebatch with which to draw</param>
        /// <param name="reference">The camera's position</param>
        /// <param name="scale">The camera's scale</param>
        /// <param name="screensize">The total screen size (for tiling)</param>
        public void Draw(SpriteBatch s, Vector2 reference, float scale, Vector2 screensize)
        {
            Vector2 screenCenter = (reference * parallax + screensize / 2);
            Vector2 currentDrawPos = new Vector2((float)(Math.Floor((screenCenter.X - screensize.X / scale / 2) / image.Width) - 1) * image.Width,
                (float)(Math.Floor((screenCenter.Y - screensize.Y / scale / 2) / image.Height) - 1) * image.Height);

            for (; currentDrawPos.X < (float)(Math.Floor((screenCenter.X + screensize.X / scale / 2) / image.Width) + 1) * image.Width; 
                currentDrawPos.X += image.Width)
            {
                for (currentDrawPos.Y = (float)(Math.Floor((screenCenter.Y - screensize.Y / scale / 2) / image.Height) - 1) * image.Height;
                    currentDrawPos.Y < (float)(Math.Floor((screenCenter.Y + screensize.Y / scale / 2) / image.Height) + 1) * image.Height; 
                    currentDrawPos.Y += image.Height)
                {
                    s.Draw(image, (currentDrawPos - screenCenter) * scale + screensize / 2, null, Color.White, 0f, 
                        Vector2.Zero, scale, SpriteEffects.None, depth);
                }
            }
        }

        /// <summary>
        /// Draws the normal map of the background in a tiled fashion, taking into account the parallax scrolling
        /// </summary>
        /// <param name="s">The spritebatch with which to draw</param>
        /// <param name="reference">The camera's position</param>
        /// <param name="scale">The camera's scale</param>
        /// <param name="screensize">The total screen size (for tiling)</param>
        public void DrawNormals(SpriteBatch s, Vector2 reference, float scale, Vector2 screensize)
        {
            Vector2 screenCenter = (reference * parallax + screensize / 2);
            Vector2 currentDrawPos = new Vector2((float)(Math.Floor((screenCenter.X - screensize.X / scale / 2) / image.Width) - 1) * image.Width,
                (float)(Math.Floor((screenCenter.Y - screensize.Y / scale / 2) / image.Height) - 1) * image.Height);

            for (; currentDrawPos.X < (float)(Math.Floor((screenCenter.X + screensize.X / scale / 2) / image.Width) + 1) * image.Width;
                currentDrawPos.X += image.Width)
            {
                for (currentDrawPos.Y = (float)(Math.Floor((screenCenter.Y - screensize.Y / scale / 2) / image.Height) - 1) * image.Height;
                    currentDrawPos.Y < (float)(Math.Floor((screenCenter.Y + screensize.Y / scale / 2) / image.Height) + 1) * image.Height;
                    currentDrawPos.Y += image.Height)
                {
                    s.Draw(normals, (currentDrawPos - screenCenter) * scale + screensize / 2, null, Color.White, 0f,
                        Vector2.Zero, scale, SpriteEffects.None, depth);
                }
            }
        }

    }
}
