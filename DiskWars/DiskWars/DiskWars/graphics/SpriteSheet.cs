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
    /// A spritesheet data collection: there should ever only be one per .png file
    /// </summary>
    class SpriteSheet
    {
        /// <summary>
        /// How many actions there are in the spritesheet
        /// </summary>
        List<int> Sequencesets;
        /// <summary>
        /// The animation times of each action in the spritesheet
        /// </summary>
        List<int> animtimes;
        /// <summary>
        /// The source rectangles of each image from the spritesheet (where to find the images on the texture)
        /// </summary>
        List<Rectangle> srcrects;
        /// <summary>
        /// The color map
        /// </summary>
        Texture2D full = null;
        /// <summary>
        /// The normal map
        /// </summary>
        Texture2D normals = null;
        /// <summary>
        /// The file name for the spritesheet
        /// </summary>
        String file;

        /// <summary>
        /// Creates a new Spritesheet from the given file name
        /// </summary>
        /// <param name="filename">the texture to load and analyze</param>
        public SpriteSheet(String filename)
        {
            file = filename;

            Texture2D shortt = RenderingEngine.instance.requestTexture(filename + "_short");
            Color[] data = new Color[shortt.Width * shortt.Height];
            shortt.GetData<Color>(data);

            Sequencesets = new List<int>();
            animtimes = new List<int>();
            srcrects = new List<Rectangle>();

            if (!(data[0].A > 0 && data[0].A < 255)) 
            {
                Sequencesets.Add(-1);
                animtimes.Add(1000);
            }
            else
            {
                int prevheight = 1;
                for (int i = 1; data[i].R != 0 || data[i].G != 0 || data[i].B != 0; i += 2)
                {
                    Sequencesets.Add(data[i].R);
                    animtimes.Add(data[i].G);
                    
                    srcrects.Add(new Rectangle(0, prevheight,data[i].B * 100 + data[i+1].R, data[i+1].G * 100 + data[i+1].B));
                    prevheight += data[i + 1].G * 100 + data[i + 1].B;
                }
            }
        }

        /// <summary>
        /// Gets the bounding rectangle of the current sequence
        /// </summary>
        /// <param name="Sequence">the action you want to check in the animation</param>
        /// <returns>the bounding rectangle</returns>
        public Rectangle getBounds(int Sequence)
        {
            return srcrects[Sequence];
        }

        /// <summary>
        /// Return the entire color map
        /// </summary>
        /// <returns>the color map texture</returns>
        public Texture2D getImage()
        {
            if (full == null)
            {
                full = RenderingEngine.instance.requestTexture(file + "_full");
            }
            return full;
        }
        /// <summary>
        /// Return the entire normal map
        /// </summary>
        /// <returns>the color normal texture</returns>
        public Texture2D getNormals()
        {
            if (normals == null)
            {
                normals = RenderingEngine.instance.requestTexture(file + "_normals");
            }
            return normals;
        }
        /// <summary>
        /// Returns the number of images in the given sequence
        /// </summary>
        /// <param name="sequence">the sequence to check</param>
        /// <returns>the number of images in the sequence</returns>
        public int numberOfImagesInSequence(int sequence)
        {
            return Sequencesets[sequence];
        }

        /// <summary>
        /// Returns the number of sequences in this spritesheet
        /// </summary>
        /// <returns>the number of sequences in this spritesheet</returns>
        public int numberOfSequences()
        {
            return Sequencesets.Count;
        }
        /// <summary>
        /// Get the animation time of the given sequence
        /// </summary>
        /// <param name="sequence">the sequence to check</param>
        /// <returns>the animation time</returns>
        public int animationTimeOfSequence(int sequence)
        {
            return animtimes[sequence];
        }
    }
}
