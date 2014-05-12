using DiskWars.parser;
using System;

namespace DiskWars
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
           // KnowledgeParser kp = new KnowledgeParser();
           // kp.ParsePlaytestDirectory(@"C:\Users\Tag\Documents\Projects\cpgd_diskwars\DiskWars\DiskWars\DiskWars\parser\dw_kb");

            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

