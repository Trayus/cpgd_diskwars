using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiskWars
{
    class Constants
    {
        public static bool DEBUG = false;
        public static bool WRAP = false;
        public static bool BOUNCETOSLOW = false;

        public static int MAPX = 62;
        public static int MAPY = 35;
        public static float GAMESCALE = 1;
        public static float RESPAWN = 3000;
        public static int TIMEMILLIS = 150000;
        public static int TIMEINTRO = 3000;
        public static int TIMEFADE = 3000;
        public static int TIMEREPLAY = 12000;
        public static int TILESIZE = 32;
        public static int PLAYERLIGHTSIZE = 80;
        public static int DISKLIGHTSIZE = 120;
        public static int POWERUPLIGHTSIZE = 50;
        public static float PLAYERLIGHTPOWER = 0.2f;
        public static float DISKLIGHTPOWER = 0.3f;
        public static float POWERUPLIGHTPOWER = 0.2f;
        public static float VELOCITY = 0.25f;
        public static float DISKVELOCITY = 1.1f;
        public static float DISKVELOCITYPU = 1.8f;
        public static float MAXVELOCITY = 3.0f;
        public static float ACCELERATION = 0.03f;
        public static float SLIDE = 1.2f;
        public static int PLAYERRADIUS = 24;
        public static int DISKRADIUS = 18;
        public static int DISKRADIUSPU = 27;
        public static int SCOREPERKILL = 3;
        public static int SCOREPERDEATH = -1;
        public static float MINDISKTIME = 100;
        public static float PLAYERSCALE = 0.8f;
        public static float POWERUPSCALE = 1.5f;
        public static float POWERUPSIZESCALE = 1.5f;
        public static float DESTR_RESPAWN = 5000f;
        public static String[] mapnames = { "Arena", "Box", "Breakout", "Ring", "WallWorld" };
        public static float POWERUPTIMER = 500.0f;
        public static float POWERUPRESPAWN = 800.0f;
    }
}
