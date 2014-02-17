using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Helpers
{
    class Rand
    {
        public Random rand = new Random();
        public static Rand instance;

        public Rand()
        {
            instance = this;
        }

        /// <summary>
        /// gives a random float between 0 and 1
        /// </summary>
        /// <returns>random number</returns>
        public static float Float()
        {
            return (float)Rand.instance.rand.NextDouble();
        }
        /// <summary>
        /// gives a random float between 0 and 'max'
        /// </summary>
        /// <param name="max">the maximum possible float value</param>
        /// <returns>random number</returns>
        public static float Float(float max)
        {
            return (float)Rand.instance.rand.NextDouble() * max;
        }
        /// <summary>
        /// gives a random float between 'min' and 'max'
        /// </summary>
        /// <param name="min">the minimum possible outcome</param>
        /// <param name="max">the maximum possible outcome</param>
        /// <returns>random number</returns>
        public static float Float(float min, float max)
        {
            return (float)Rand.instance.rand.NextDouble() * (max - min) + min;
        }

        /// <summary>
        /// gives a random number between 0 and 'max'
        /// </summary>
        /// <param name="max">the maximum possible value</param>
        /// <returns>random number</returns>
        public static int Int(int max)
        {
            return Rand.instance.rand.Next(max);
        }

        /// <summary>
        /// gives a random number between 'min' and 'max'
        /// </summary>
        /// <param name="min">the minimum possible value</param>
        /// <param name="max">the maximum possible value</param>
        /// <returns>random number</returns>
        public static int Int(int min, int max)
        {
            return Rand.instance.rand.Next(min, max);
        }

        /// <summary>
        /// Gives a random vector, with both components between 0 and 1
        /// </summary>
        /// <returns>random vector</returns>
        public static Vector2 Vector2D()
        {
            return new Vector2(Rand.Float(), Rand.Float());
        }

        /// <summary>
        /// Gives a random vector, with both components between 0 and 'max'
        /// </summary>
        /// <param name="bound">the maximum possible component value</param>
        /// <returns>random vector</returns>
        public static Vector2 Vector2D(float max)
        {
            return new Vector2(Rand.Float(max), Rand.Float(max));
        }

        /// <summary>
        /// Gives a random vector, with both components between 0 and 1
        /// </summary>
        /// <param name="min">the minimum possible component value</param>
        /// <param name="max">the maximum possible component value</param>
        /// <returns>random vector</returns>
        public static Vector2 Vector2D(float min, float max)
        {
            return new Vector2(Rand.Float(min, max), Rand.Float(min, max));
        }

        /// <summary>
        /// Gives a random vector, with both components between -0.5 and 0.5
        /// </summary>
        /// <returns>random vector</returns>
        public static Vector2 biVector2D()
        {
            return new Vector2(Rand.Float() - 0.5f, Rand.Float() -0.5f);
        }
        /// <summary>
        /// Gives a random vector, with both components between -'max' and 'max'
        /// </summary>
        /// <param name="max">the maximum negative and positive component values</param>
        /// <returns>random vector</returns>
        public static Vector2 biVector2D(float max)
        {
            return new Vector2((Rand.Float() - 0.5f) * 2 * max, (Rand.Float() - 0.5f) * 2 * max);
        }
    }
}
