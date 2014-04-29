using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DiskWars.parser
{
   class DWFrame
   {
      public bool isHead { set; get; }
      public int timeLeft { set; get; }
      public PlayerData[] players;
      public DiskData[] disks;

      public DWFrame nextFrame { set; get; }
      public DWFrame prevFrame { set; get; }

      public DWFrame()
      {
         players = new PlayerData[] {
            new PlayerData(),
            new PlayerData(),
            new PlayerData(),
            new PlayerData()
         };

         disks = new DiskData[] {
            new DiskData(),
            new DiskData(),
            new DiskData(),
            new DiskData()
         };
      }

      public bool isPlayerActive(int playerNum)
      {
         return players[playerNum].xPos != -999;
      }

      public bool isPlayerAlive(int playerNum)
      {
         return players[playerNum].isAlive;
      }

      public Vector2 getPlayerPosition(int playerNum)
      {
         if (!isPlayerActive(playerNum))
         {
            Console.WriteLine("WARNING: Access of a player position for an inactive player: Player " + playerNum);
         }

         if (!isPlayerAlive(playerNum))
         {
            Console.WriteLine("WARNING: Access of a player position for a dead player: Player " + playerNum);
         }

         return new Vector2(players[playerNum].xPos, players[playerNum].yPos);
      }

      public Vector2 getDiskPosition(int playerNum)
      {
         if (!isPlayerActive(playerNum))
         {
            Console.WriteLine("WARNING: Access of a disk position for an " +
               "inactive player: Player " + playerNum);
         }

         if (!isPlayerAlive(playerNum))
         {
            Console.WriteLine("WARNING: Access of a disk position for a " +
             " dead player: Player " + playerNum);
         }

         return new Vector2(disks[playerNum].xPos, disks[playerNum].yPos);
      }
   }
}
