using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DiskWars.parser
{
   class PositionSelector : IEquatable<PositionSelector>
   {
      private Vector2[] positions;
      private int[] activeQuadrants;

      public PositionSelector(Vector2[] positions)
      {
         // TODO: Complete member initialization
         this.positions = positions;
      }

      public static implicit operator PositionSelector(Vector2[] positions)
      {
         return new PositionSelector(positions);
      }

      public bool Equals(PositionSelector p2)
      {
         return p2.activeQuadrants.Length == this.activeQuadrants.Length &&
            p2.activeQuadrants.SequenceEqual(this.activeQuadrants);
      }

      public void GenerateQuadrants()
      {
         // Make sure arrays are ordered!
      }
   }
}
