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
   }
}
