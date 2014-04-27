using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using System.IO;

namespace DiskWars.parser
{
   static class KnowledgeParser
   {
      private static string mapPattern = @"^maps/(<mapName>[Arena|Box|Breakout|Ring|WallWorld])";
      private static string timePattern = @"^t = (<timeMS>\d+) (<p1Score>\d+) (<p2Score>\d+) (<p3Score>\d+) (<p4Score>\d+)";
      private static string playerPattern = @"^p(<playerNum>\d) pos \<(<posX>\d+),(<posY>\d+)\> rot (<rot>\d+) disk \<(<diskX>\d+),(<diskY>\d+)\> holding (<hasDisk>[t|f]) returning (<diskReturning>[t|f]) shield (<hasShield>[t|f]) speed (<hasSpeed>[t|f])";

      private static Regex mapRE = new Regex(mapPattern);
      private static Regex timeRE = new Regex(timePattern);
      private static Regex playerRE = new Regex(playerPattern);

      // This is so gross.
      private static Dictionary<string, Dictionary<int, 
       Dictionary<PositionSelector, Dictionary<PositionSelector, DWFrame>>>> 
        knowledgeTree;

      private static string currentMap;
      private static int currentPlayersAlive;
      private static PositionSelector currentDiskPositions;
      private static PositionSelector currentPlayerPositions;
      private static DWFrame currentFrame;

      public static void ParsePlaytestDirectory(string dirName)
      {
         string[] filePaths = Directory.GetFiles(dirName);

         foreach (string fileName in filePaths)
         {
            ParsePlaytest(fileName);
         }
      }

      public static void ParsePlaytest(string fileName) 
      {
         if (File.Exists(fileName))
         {
            using (StreamReader sr = File.OpenText(fileName))
            {
               string line = "";
               Match mapMatch = null;

               line = sr.ReadLine();

               if (!mapRE.IsMatch(line))
               {
                  throw new Exception("KP Error: Unrecognized map line: " + line);
               }

               mapMatch = mapRE.Match(line);
               currentMap = mapMatch.Groups["mapName"].Value;

               while ((line = sr.ReadLine()) != null)
               {
                  parseTimeAndScore(line, currentFrame);
                  
                  while (sr.Peek() != 't')
                  {
                     line = sr.ReadLine();

                     parsePlayerAndDisk(line, currentFrame);
                  }
               }
            }
         }
      }

      private static void parseTimeAndScore(string line, DWFrame currentFrame)
      {
         if (!timeRE.IsMatch(line))
         {
            throw new Exception("KP Error: Unrecognized time line: " + line);
         }

         Match timeMatch = timeRE.Match(line);
         try
         {
            currentFrame.timeLeft = Convert.ToInt32(timeMatch.Groups["timeMS"].Value);
         }
         catch (FormatException e)
         {
            Console.WriteLine("Input string is not a sequence of digits.");
         }
         catch (OverflowException e)
         {
            Console.WriteLine("The number cannot fit in an Int32.");
         }
      }

      private static void parsePlayerAndDisk(string line, DWFrame currentFrame)
      {
         if (!playerRE.IsMatch(line))
         {
            throw new Exception("KP Error: Unrecognized player line: " + line);
         }

         Match playerMatch = playerRE.Match(line);

         // TODO: The rest of this, lol.
      }

      // The following functions should never be used independently... 
      // Only chained together.      
      /// <summary>
      /// The first of four in the chain of methods to return a list of frames 
      /// for a given set of game conditions. This narrows the focus down by map.
      /// </summary>
      /// <param name="mapName">The name of the map</param>
      /// <returns>Ugly Dictionary. You don't to use this alone.</returns>
      public static Dictionary<int, Dictionary<PositionSelector,
       Dictionary<PositionSelector, DWFrame>>> Map(string mapName) 
      {
         currentMap = mapName;
         return knowledgeTree[currentMap];
      }

      /// <summary>
      /// The second of four in the chain of methods to return a list of frames
      /// for a given set of game conditions. This narrows the focus down by
      /// number of players alive.
      /// </summary>
      /// <param name="playersAlive">How many players are living?</param>
      /// <returns>Ugly Dictionary. You don't to use this alone.</returns>
      public static Dictionary<PositionSelector, Dictionary<PositionSelector, 
       DWFrame>> PlayersAlive(int playersAlive) 
      {
         currentPlayersAlive = playersAlive;
         return knowledgeTree[currentMap][currentPlayersAlive];
      }

      /// <summary>
      /// ** GIVE POSITIONS IN ORDER OF PLAYER **
      /// The third of four in the chain of methods to return a list of frames
      /// for a given set of game conditions. This narrows the focus down by
      /// the set of positions where disks are.
      /// </summary>
      /// <param name="positions">The positions of disks in order of player</param>
      /// <returns>Ugly Dictionary. You don't to use this alone.</returns>
      public static Dictionary<PositionSelector, DWFrame> 
       DisksInRegions(params Vector2[] positions)
      {
         currentDiskPositions = positions;
         return knowledgeTree[currentMap][currentPlayersAlive]
          [currentDiskPositions];
      }

      /// <summary>
      /// ** GIVE POSITIONS IN ORDER OF PLAYER **
      /// The four of four in the chain of methods to return a frame
      /// for a given set of game conditions. This narrows the focus down by
      /// the set of positions where players are.
      /// </summary>
      /// <param name="positions">The positions of players in order of player</param>
      /// <returns>The frame corresponding to the situation described.
      /// Not particularly useful by itself but gives access to the frames
      /// following it and the frames preceding it.</returns>
      public static DWFrame PlayersInRegions(params Vector2[] positions)
      {
         currentPlayerPositions = positions;
         return knowledgeTree[currentMap][currentPlayersAlive]
          [currentDiskPositions][currentPlayerPositions];
      }
   }
}
