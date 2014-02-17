using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiskWars
{
    class Constants
    {
        public static bool DEBUG = false;

        public static int MAPX = 48;
        public static int MAPY = 27;
        public static float GAMESCALE = 1;
        public static float RESPAWN = 3000;
        public static int TIMEMILLIS = 100000;
        public static int TIMEINTRO = 1000;
        public static int TIMEFADE = 3000;
        public static int TIMEREPLAY = 12000;
        public static int TILESIZE = 40;
        public static int PLAYERLIGHTSIZE = 80;
        public static int DISKLIGHTSIZE = 120;
        public static float PLAYERLIGHTPOWER = 0.2f;
        public static float DISKLIGHTPOWER = 0.3f;
        public static float VELOCITY = 0.25f;
        public static float DISKVELOCITY = 1.10f;
        public static float MAXVELOCITY = 3.0f;
        public static float ACCELERATION = 0.03f;
        public static float SLIDE = 1.2f;
        public static int PLAYERRADIUS = 30;
        public static int DISKRADIUS = 24;
        public static int SCOREPERKILL = 3;
        public static int SCOREPERDEATH = -1;
        public static float MINDISKTIME = 100;
        public static String[] mapnames = { "placeholder", "testmap"};
    }
}
