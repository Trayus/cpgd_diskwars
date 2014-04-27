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
      public int playersAlive { set; get; }
      public int timeLeft { set; get; }
      public Vector2[] playerPositions;
      public Vector2[] diskPositions;

      DWFrame nextFrame { set; get; }
      DWFrame lastFrame { set; get; }


   }
}
