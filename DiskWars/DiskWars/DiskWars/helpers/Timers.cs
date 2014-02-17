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

namespace Helpers
{
    /// <summary>
    /// A helper class for timers (since it's a gamecomponent, all timers will be updates automatically!)
    /// </summary>
    class Timers : GameComponent
    {
        /// <summary>
        /// A dictionary mapping strings to ints: each timer is a simple int, and exterior classes can access them by the strings
        /// </summary>
        static Dictionary<String, int> table;

        /// <summary>
        /// Constructor: takes a game since this is a GameComponent. Also initializes the dictionary
        /// </summary>
        /// <param name="game"></param>
        public Timers(Game game)
            :base(game)
        {
            Timers.table = new Dictionary<string, int>();
            game.Components.Add(this);
        }

        /// <summary>
        /// Sets a timer (given by the string name) to the value given in millis. 
        /// If the timer doesn't exist, it is created
        /// </summary>
        /// <param name="name">The timer to set</param>
        /// <param name="millis">What to set the timer to (in milliseconds)</param>
        public static void setTimer(String name, int millis)
        {
            ensureExists(name);

            table[name] = millis;
        }

        /// <summary>
        /// Checks the timer (given by string name) to see if it's reached zero yet (timers decrease in value on their own)
        /// If the timer doesn't exist, it is created
        /// </summary>
        /// <param name="name">the timer to check</param>
        /// <returns>return true iff the timer has reached zero </returns>
        public static bool checkTimer(String name)
        {
            ensureExists(name);

            return table[name] == 0;
        }

        /// <summary>
        /// Gets the time remaining on the timer given by string name (timers decrease in value on their own)
        /// If the timer doesn't exist, it is created
        /// </summary>
        /// <param name="name">the timer to check</param>
        /// <returns>the time remaining of the timer</returns>
        public static int timeRemaining(String name)
        {
            ensureExists(name);

            return table[name];
        }

        /// <summary>
        /// Creates a timer (given by the string name) if it doesn't yet exist
        /// </summary>
        /// <param name="name">the timer to check</param>
        private static void ensureExists(String name)
        {
            if (!table.ContainsKey(name))
                table.Add(name, 0);
        }

        /// <summary>
        /// Updates all timers (if they're non-zero, it will reduce them by the elapsed gametime since the last frame in milliseconds)
        /// </summary>
        /// <param name="gameTime">The elapsed gametime since the last update</param>
        public override void Update(GameTime gameTime)
        {
            List<string> entries = new List<string>(table.Keys);
            for (int i = 0; i < entries.Count; i++)
            {
                if (table[entries[i]] > 0)
                    table[entries[i]] -= gameTime.ElapsedGameTime.Milliseconds;
                if (table[entries[i]] < 0)
                    table[entries[i]] = 0;
            }

            base.Update(gameTime);
        }
    }
}
