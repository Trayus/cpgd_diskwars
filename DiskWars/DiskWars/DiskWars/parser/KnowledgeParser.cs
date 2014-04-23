using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace DiskWars.parser
{
   static class KnowledgeParser
   {
      static string mapPattern = @"^maps/(<mapName>[Arena|Box|Breakout|Ring|WallWorld])";
      static string timePattern = @"^t = (<timeMS>\d+) (\d+) (\d+) (\d+) (\d+)";
      static string playerPattern = @"^p(<playerNum>\d) pos \<(<posX>\d+),(<posY>\d+)\> rot (<rot>\d+) disk \<(<diskX>\d+),(<diskY>\d+)\> holding (<hasDisk>[t|f]) returning (<diskReturning>[t|f]) shield (<hasShield>[t|f]) speed (<hasSpeed>[t|f])";

      Regex mapRE = new Regex(mapPattern);
      Regex timeRE = new Regex(timePattern);
      Regex playerRE = new Regex(playerPattern);


   }
}
