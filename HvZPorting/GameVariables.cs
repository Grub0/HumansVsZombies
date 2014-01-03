using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HumansVsZombies.cs
{
    static class GameVariables
    {
        public static int spriteWidth = 60;
        public static int spriteHeight = 80;

        public static int playerStartX = 100;
        public static int playerStartY = 120;

        public static double stepSpeed = .18;

        // Zombie Variables
        public static int zombieScorePoints = 100;
        public static double zombieDefaultWaitTime = 5.0;
        public static double zombieMovingTimer = 5.0;
        public static int zombieDefaultRange = 500;
        public static int zombieDefaultChaseSpeed = 2;
        public static int zombieHealth = 3;

        // Human Variables
        public static int humanHealth = 3;

        public static int screenWidth = 1280;
        public static int screenHeight = 720;

        public static int barrierHigherNumber = 160;
        public static int barrierLowerNumber = 20;

        // This is to tell the game which maptiles exist and which ones don't
        public static int defaultMapTileReferenced = 100;
        public static int dartCollectibleWorth = 5;
        public static double bTimer = .01;
    }
}
