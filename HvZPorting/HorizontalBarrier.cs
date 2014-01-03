using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HumansVsZombies.cs
{
    class HorizontalBarrier : Barrier
    {
        /// <summary>
        /// This was made seperately to create a horizontally shape barrier (it was just easier)
        /// </summary>
        /// <param name="x">The X location</param>
        /// <param name="y">The Y location</param>
        /// <param name="mt">The maptile this barrier is going to connect to</param>
        /// <param name="p">The player</param>
        /// <param name="d">The direction the player has to go in order to go through the barrier</param>
        public HorizontalBarrier(int x, int y)
            : base(x, y, GameVariables.barrierHigherNumber, GameVariables.barrierLowerNumber)
        {

        }
    }
}
