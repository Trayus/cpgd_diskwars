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
      
      // Scores have .+ because of negative numbers, \d doesn't work
      private static string timePattern = @"^t = (<timeMS>\d+) (<p1Score>.+) (<p2Score>.+) (<p3Score>.+) (<p4Score>.+)";
      
      // Rotation has .+ because of negative numbers, \d doesn't work
      private static string playerPattern = @"^p(<playerNum>\d) pos \<(<posX>\d+),(<posY>\d+)\> rot (<rot>.+) disk \<(<diskX>\d+),(<diskY>\d+)\> holding (<hasDisk>[t|f]) returning (<diskReturning>[t|f]) shield (<hasShield>[t|f]) speed (<hasSpeed>[t|f])";

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
      private static DWFrame currentFrame = null;

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
               DWFrame tmpFrame = null;

               line = sr.ReadLine();

               if (!mapRE.IsMatch(line))
               {
                  throw new Exception("KP Error: Unrecognized map line: " + line);
               }

               mapMatch = mapRE.Match(line);
               currentMap = mapMatch.Groups["mapName"].Value;

               while ((line = sr.ReadLine()) != null)
               {
                  tmpFrame = currentFrame;
                  currentFrame = new DWFrame();

                  tmpFrame.nextFrame = currentFrame;
                  currentFrame.prevFrame = tmpFrame;

                  if (tmpFrame == null)
                  {
                     currentFrame.isHead = true;
                  }

                  parseTimeAndScore(line, currentFrame);
                  
                  while (sr.Peek() != 't')
                  {
                     line = sr.ReadLine();

                     parsePlayerAndDisk(line, currentFrame);

                     // TODO: Use DWFrame and place within Dictionary
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
         bool playerDead = false;
         int playerNum, playerRot;
         int playerX, playerY;
         int diskX, diskY;
         bool hasDisk, diskReturning, hasShield, hasSpeed;

         if (!playerRE.IsMatch(line))
         {
            if (line.Contains("dead"))
            {
               playerDead = true;
            }
            else
            {
               throw new Exception("KP Error: Unrecognized player line: " + line);
            }
         }

         if (!playerDead)
         {
            Match playerMatch = playerRE.Match(line);
            playerNum = Convert.ToInt32(playerMatch.Groups["playerNum"].Value);
            playerX = Convert.ToInt32(playerMatch.Groups["posX"].Value);
            playerY = Convert.ToInt32(playerMatch.Groups["posY"].Value);
            playerRot = Convert.ToInt32(playerMatch.Groups["rot"].Value);

            diskX = Convert.ToInt32(playerMatch.Groups["diskX"].Value);
            diskY = Convert.ToInt32(playerMatch.Groups["diskY"].Value);
            diskReturning = playerMatch.Groups["diskReturning"].Value.Equals("t");

            hasDisk = playerMatch.Groups["hasDisk"].Value.Equals("t");
            hasShield = playerMatch.Groups["hasShield"].Value.Equals("t");
            hasSpeed = playerMatch.Groups["hasSpeed"].Value.Equals("t");

            currentFrame.players[playerNum].xPos = playerX;
            currentFrame.players[playerNum].yPos = playerY;
            currentFrame.players[playerNum].rot = playerRot;
            currentFrame.players[playerNum].hasDisk = hasDisk;
            currentFrame.players[playerNum].hasShield = hasShield;
            currentFrame.players[playerNum].hasSpeed = hasSpeed;
            currentFrame.players[playerNum].isAlive = true;

            currentFrame.disks[playerNum].xPos = diskX;
            currentFrame.disks[playerNum].yPos = diskY;
            currentFrame.disks[playerNum].isReturning = diskReturning;
         }
         else
         {
            playerNum = Convert.ToInt32(line.Substring(1, 1));

            currentFrame.players[playerNum].xPos = -1;
            currentFrame.players[playerNum].yPos = -1;
            currentFrame.players[playerNum].isAlive = false;

            currentFrame.disks[playerNum].xPos = -1;
            currentFrame.disks[playerNum].yPos = -1;
         }
      }

      // The following functions should never be used independently... 
      // Only chained together.      
      /// <summary>
      /// The first of four in the chain of methods to return a list of frames 
      /// for a given set of game conditions. This narrows the focus down by map.
      /// </summary>
      /// <param name="mapName">The name of the map</param>
      /// <returns>Ugly Dictionary. You don't want to use this alone.</returns>
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
