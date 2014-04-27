using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DiskWars.parser
{
   class PositionSelector
   {
      private Vector2[] positions;

      public PositionSelector(Vector2[] positions)
      {
         // TODO: Complete member initialization
         this.positions = positions;
      }

      public static implicit operator PositionSelector(Vector2[] positions)
      {
         return new PositionSelector(positions);
      }
   }
}
