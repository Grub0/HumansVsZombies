using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HumansVsZombies.cs
{
    class DartCollectible : GamePiece
    {
        bool collected;

        public Boolean GetSetCollected { get { return collected; } set { collected = value; } }

        public DartCollectible(int x, int y, int width, int height)
            : base(x, y, width, height)
        {
            collected = false;
        }
    }
}
