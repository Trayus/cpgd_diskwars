﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiskWars.parser
{
   public struct PlayerData
   {
      public int xPos,
         yPos;

      public float rot;

      public bool hasDisk,
         hasShield,
         hasSpeed,
         isAlive;
   }
}
