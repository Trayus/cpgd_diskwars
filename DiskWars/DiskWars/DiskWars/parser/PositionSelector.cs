using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace DiskWars.parser
{
   public class PositionSelector
   {
      private Vector2[] positions;
      private List<int> activeQuadrants;
      private static int MAP_WIDTH = 1920;
      private static int MAP_HEIGHT = 1080;
      private static int HORIZ_QUADRANTS = 10;
      private static int VERT_QUADRANTS = 5;

      public PositionSelector()
      {
         positions = null;
      }

      public PositionSelector(Vector2[] positions)
      {
         this.positions = positions;
         GenerateQuadrants();
      }

      public static implicit operator PositionSelector(Vector2[] positions)
      {
         return new PositionSelector(positions);
      }

      private void GenerateQuadrants()
      {
         activeQuadrants = new List<int>();

         int horizOffset = MAP_WIDTH / HORIZ_QUADRANTS;
         int vertOffset = MAP_HEIGHT / VERT_QUADRANTS;
         int row, col;

         foreach (Vector2 pos in positions)
         {
            row = pos.Y > 0 ? Convert.ToInt32(pos.Y / vertOffset) : 0;
            col = pos.X > 0 ? Convert.ToInt32(pos.X / horizOffset) : 0;
            activeQuadrants.Add((row > 0 ? row : 0) * HORIZ_QUADRANTS + 
               col);
         }
         
         // Make sure arrays are ordered!
         this.activeQuadrants.Sort();
      }

      public void update(Vector2[] positions)
      {
         this.positions = positions;
         GenerateQuadrants();
      }

      public class PSComparer : IEqualityComparer<PositionSelector>
      {
         public bool Equals(PositionSelector p1, PositionSelector p2)
         {
            return p2.activeQuadrants.Count == p1.activeQuadrants.Count &&
            p2.activeQuadrants.SequenceEqual(p1.activeQuadrants);
         }

         public int GetHashCode(PositionSelector obj)
         {
            int sum = 0;

            foreach (int i in obj.activeQuadrants)
            {
               sum += i;
            }

            return sum;
         }
      }
   }
}


