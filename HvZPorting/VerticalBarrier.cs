using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HumansVsZombies.cs
{
    class VerticalBarrier : Barrier
    {
        /// <summary>
        /// This was made seperately to create a vertically shape barrier (it was just easier)
        /// </summary>
        /// <param name="x">The X location</param>
        /// <param name="y">The Y location</param>
        /// <param name="mt">The maptile this barrier is going to connect to</param>
        /// <param name="p">The player</param>
        /// <param name="d">The direction the player has to go in order to go through the barrier</param>
        public VerticalBarrier(int x, int y)
            : base(x, y, GameVariables.barrierLowerNumber, GameVariables.barrierHigherNumber)
        {

        }
    }
}
