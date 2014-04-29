using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiskWars.parser
{
   struct PlayerData
   {
      public int xPos,
         yPos,
         rot;

      public bool hasDisk,
         hasShield,
         hasSpeed,
         isAlive;

      public PlayerData()
      {
         xPos = -999;
         yPos = -999;
         rot = -999;

         hasDisk = false;
         hasShield = false;
         hasSpeed = false;
         isAlive = false;
      }
   }
}
