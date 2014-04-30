using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using System.IO;
using System.Globalization;

namespace DiskWars.parser
{
   /// <summary>
   /// Knowledge Parser
   /// Takes in text files and builds an underlying tree that can be queried.
   /// Tree is built like so:
   /// Map Name
   ///   Number of players alive
   ///      The positions of the disks on the map
   ///         The positions of the players on the map
   ///            A list of frames that describe that situation (with links to 
   ///            the frames that preceded and followed the given frame)
   ///            
   /// Recommended workflow:
   ///   1. Call ParsePlaytestDirectory on a directory full of playtest files
   ///   OR
   ///   1. Call ParsePlaytest on individual files, trying to parse as many files as
   ///   possible before step 2
   ///   
   ///   2. Gather the information needed for querying the knowledge base
   ///      - Map name
   ///      - Number of players alive
   ///      - The positions of the disks
   ///      - The positions of the players
   ///      
   ///   3. Call the suggested method chain of:
   ///      LinkedList<DWFrame> = KnowledgeParser
   ///                                    .Map(mapName)
   ///                                    .PlayersAlive(numPlayersAlive)
   ///                                    .DiskPositions(disk1.pos, disk2.pos, ...)
   ///                                    .PlayerPositions(p1.pos, p2.pos, ...);
   ///      Don't put in positions for players that are dead or inactive.
   ///                                    
   ///   4. Do whatever you wanna do with the frames you now have :-).
   ///   
   /// Ok so you have your linked list of DWFrames, what the heck is a DW Frame?!
   /// DWFrame gives you access to:
   ///   - Whether or not this frame is the first frame in the playtest (isHead)
   ///   - How much time is left in the game at this point (timeLeft)
   ///   - Access to the players, each of which contains:
   ///      - player x position (xPos)
   ///      - player y position (yPos)
   ///      - player rotation (rot)
   ///      - if the player has their disk (hasDisk)
   ///      - if the player has the shield power-up (hasShield)
   ///      - if the player has the speed power-up (hasSpeed)
   ///      - ***** if the player is actually alive (isAlive) *****
   ///   - Access to the disks, each of which contains:
   ///      - disk x position (xPos)
   ///      - disk y position (yPos)
   ///      - if the disk is trying to return to the player (isReturning)
   ///   - The next frame from the playtest (nextFrame)
   ///   - The preceding frame from the playtest (prevFrame)
   ///      - prevFrame will be null if this was the first frame of the playtest
   ///   - PositionSelectors for players and disks, these store the quadrant numbers
   ///   and actual positions of the players and disks
   ///      - positions array holds actual positions
   ///      - activeQuadrants holds the quadrant numbers
   ///   - Asking if a given player is active in the game (isPlayerActive)
   ///   - Asking if a given player is alive (isPlayerAlive)
   ///   - Getting the current scores for each player (scores)
   ///   - Getting a player position as a Vector2 (getPlayerPosition)
   ///   - Getting a disk position as a Vector2 (getDiskPosition)
   ///   - Getting the number of players currently alive (getNumPlayersAlive)
   /// </summary>
   static class KnowledgeParser
   {
      private static string mapPattern = @"maps/(?<mapName>\bArena|Box|Breakout|Ring|WallWorld\b)";
      
      // Scores have .+ because of negative numbers, \d doesn't work
      private static string timePattern = @"t = (?<timeMS>\d+) (?<p1Score>.+) (?<p2Score>.+) (?<p3Score>.+) (?<p4Score>.+)";
      
      // Rotation has .+ because of negative numbers, \d doesn't work
      private static string playerPattern = @"p(?<playerNum>\d) pos \<(?<posX>\d+),(?<posY>\d+)\> rot (?<rot>.+) disk \<(?<diskX>\d+),(?<diskY>\d+)\> holding (?<hasDisk>[t|f]) returning (?<diskReturning>[tf]) shield (?<hasShield>[tf]) speed (?<hasSpeed>[tf])";

      private static Regex mapRE = new Regex(mapPattern);
      private static Regex timeRE = new Regex(timePattern);
      private static Regex playerRE = new Regex(playerPattern);

      // This is so gross.
      private static Dictionary<string, Dictionary<int, 
       Dictionary<PositionSelector, Dictionary<PositionSelector, 
       LinkedList<DWFrame>>>>> knowledgeTree = 
         new Dictionary<string, Dictionary<int, Dictionary<PositionSelector, 
          Dictionary<PositionSelector, LinkedList<DWFrame>>>>>();

      private static string currentMap;
      private static int currentPlayersAlive;
      private static PositionSelector currentDiskPositions;
      private static PositionSelector currentPlayerPositions;
      private static DWFrame currentFrame = null;

      /// <summary>
      /// Parse a whole directory of recorded playtests.
      /// </summary>
      /// <param name="dirName">The directory path</param>
      public static void ParsePlaytestDirectory(string dirName)
      {
         string[] filePaths = Directory.GetFiles(dirName);

         foreach (string fileName in filePaths)
         {
            ParsePlaytest(fileName);
         }
      }

      /// <summary>
      /// Parse a single playtest.
      /// </summary>
      /// <param name="fileName">The path to the playtest file</param>
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

               knowledgeTree[currentMap] = 
                  new Dictionary<int, Dictionary<PositionSelector, 
                     Dictionary<PositionSelector, LinkedList<DWFrame>>>>();

               while ((line = sr.ReadLine()) != null)
               {
                  tmpFrame = currentFrame;
                  currentFrame = new DWFrame();

                  if (tmpFrame == null)
                  {
                     currentFrame.isHead = true;
                  }
                  else
                  {
                     tmpFrame.nextFrame = currentFrame;
                  }

                  currentFrame.prevFrame = tmpFrame;

                  parseTimeAndScore(line);
                  
                  while (sr.Peek() != 't' && sr.Peek() != -1)
                  {
                     line = sr.ReadLine();

                     parsePlayerAndDisk(line);
                  }

                  putFrameInTree();
               }
            }
         }
         else
         {
            throw new FileNotFoundException("File not found! " + Directory.GetCurrentDirectory() + fileName);
         }
      }

      private static void putFrameInTree()
      {
         PositionSelector playerPS, diskPS;
         currentFrame.updateQuadrants();

         currentPlayersAlive = currentFrame.getNumPlayersAlive();
         if (!knowledgeTree[currentMap].ContainsKey(currentPlayersAlive))
         {
            knowledgeTree[currentMap][currentPlayersAlive] = 
               new Dictionary<PositionSelector,
                  Dictionary<PositionSelector, 
                  LinkedList<DWFrame>>>();
         }

         diskPS = currentFrame.playerQuadrants;
         if (!knowledgeTree[currentMap][currentPlayersAlive].ContainsKey(diskPS))
         {
            knowledgeTree[currentMap][currentPlayersAlive][diskPS] = new 
               Dictionary<PositionSelector, LinkedList<DWFrame>>();
         }

         playerPS = currentFrame.playerQuadrants;
         if (!knowledgeTree[currentMap][currentPlayersAlive][diskPS].ContainsKey(playerPS))
         {
            knowledgeTree[currentMap][currentPlayersAlive][diskPS][playerPS] = 
               new LinkedList<DWFrame>();
         }

         knowledgeTree[currentMap][currentPlayersAlive][diskPS][playerPS].AddLast(currentFrame);
      }

      private static void parseTimeAndScore(string line)
      {
         if (!timeRE.IsMatch(line))
         {
            throw new Exception("KP Error: Unrecognized time line: " + line);
         }

         Match timeMatch = timeRE.Match(line);
         try
         {
            currentFrame.timeLeft = Convert.ToInt32(timeMatch.Groups["timeMS"].Value);
            currentFrame.scores[0] = (int)Decimal.Parse(timeMatch.Groups["p1Score"].Value, 
               NumberStyles.Number);
            currentFrame.scores[1] = (int)Decimal.Parse(timeMatch.Groups["p2Score"].Value,
               NumberStyles.Number);
            currentFrame.scores[2] = (int)Decimal.Parse(timeMatch.Groups["p3Score"].Value,
               NumberStyles.Number);
            currentFrame.scores[3] = (int)Decimal.Parse(timeMatch.Groups["p4Score"].Value,
               NumberStyles.Number);
         }
         catch (FormatException e)
         {
            Console.WriteLine("Input string is not a sequence of digits. " + e.Message);
         }
         catch (OverflowException e)
         {
            Console.WriteLine("The number cannot fit in an Int32. " + e.Message);
         }
      }

      private static void parsePlayerAndDisk(string line)
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
            playerNum = Convert.ToInt32(playerMatch.Groups["playerNum"].Value) - 1;
            playerX = Convert.ToInt32(playerMatch.Groups["posX"].Value);
            playerY = Convert.ToInt32(playerMatch.Groups["posY"].Value);
            playerRot = (int)Decimal.Parse(playerMatch.Groups["rot"].Value, NumberStyles.Number);

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
            playerNum = Convert.ToInt32(line.Substring(1, 1)) - 1;

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
       Dictionary<PositionSelector, LinkedList<DWFrame>>>> Map(string mapName) 
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
       LinkedList<DWFrame>>> PlayersAlive(int playersAlive) 
      {
         currentPlayersAlive = playersAlive;
         return knowledgeTree[currentMap][currentPlayersAlive];
      }

      /// <summary>
      /// The third of four in the chain of methods to return a list of frames
      /// for a given set of game conditions. This narrows the focus down by
      /// the set of positions where disks are.
      /// </summary>
      /// <param name="positions">The positions of disks, order doesn't matter</param>
      /// <returns>Ugly Dictionary. You don't to use this alone.</returns>
      public static Dictionary<PositionSelector, LinkedList<DWFrame>> 
       DiskPositions(params Vector2[] positions)
      {
         currentDiskPositions = positions;
         return knowledgeTree[currentMap][currentPlayersAlive]
          [currentDiskPositions];
      }

      /// <summary>
      /// The four of four in the chain of methods to return a frame
      /// for a given set of game conditions. This narrows the focus down by
      /// the set of positions where players are.
      /// </summary>
      /// <param name="positions">The positions of players, order doesn't matter</param>
      /// <returns>The frame corresponding to the situation described.
      /// It can be used to query for more specific information like player positions,
      /// disk positions, and gives access to the frames following it and the 
      /// frames preceding it.</returns>
      public static LinkedList<DWFrame> PlayerPositions(params Vector2[] positions)
      {
         currentPlayerPositions = positions;
         return knowledgeTree[currentMap][currentPlayersAlive]
          [currentDiskPositions][currentPlayerPositions];
      }
   }
}
