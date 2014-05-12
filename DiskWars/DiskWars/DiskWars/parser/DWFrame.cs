using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DiskWars.parser
{
   public class DWFrame
   {
      public bool isHead { set; get; }
      public int timeLeft { set; get; }

      public PlayerData[] players;
      public DiskData[] disks;

      public DWFrame nextFrame { set; get; }
      public DWFrame prevFrame { set; get; }

      public PositionSelector playerQuadrants;
      public PositionSelector diskQuadrants;

      public int[] scores;

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

         playerQuadrants = new PositionSelector();
         diskQuadrants = new PositionSelector();

         scores = new int[] { 0, 0, 0, 0 };
      }

      // TODO: This method is sketchy...
      public bool isPlayerActive(int playerNum)
      {
         return players[playerNum].xPos != 0;
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

      public int getNumPlayersAlive()
      {
         int playersAlive = 0;

         for (int i = 0; i < players.Length; i++)
         {
            if (players[i].isAlive && isPlayerActive(i))
            {
               playersAlive++;
            }
         }

         return playersAlive;
      }

      public void updateQuadrants()
      {
         List<Vector2> positions = new List<Vector2>(), 
            diskPositions = new List<Vector2>();

         for (int i = 0; i < players.Length; i++)
         {
            PlayerData p = players[i];

            if (p.isAlive && isPlayerActive(i))
            {
               positions.Add(new Vector2(p.xPos, p.yPos));
               diskPositions.Add(new Vector2(disks[i].xPos, disks[i].yPos));
            }
         }

         playerQuadrants.update(positions.ToArray());
         diskQuadrants.update(diskPositions.ToArray());
      }
   }
}
