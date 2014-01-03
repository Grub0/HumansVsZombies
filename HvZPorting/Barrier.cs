using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HumansVsZombies.cs
{
    class Barrier : GamePiece
    {
        int maptileNumber;

        public int GetSetMaptileNumber { get { return maptileNumber; } set { maptileNumber = value; } }

        /// <summary>
        /// The base barrier that all barriers will inherit from
        /// </summary>
        /// <param name="x">The X location</param>
        /// <param name="y">The Y location</param>
        /// <param name="width">The width of the barrier</param>
        /// <param name="height">The height of the barrier</param>
        /// <param name="mt">The maptile this barrier is going to connect to</param>
        /// <param name="p">The player</param>
        /// <param name="d">The direction the player has to go in order to go through the barrier</param>
        public Barrier(int x, int y, int width, int height)//, MapTile mt)//, Player p, Direction d)
            : base(x, y, width, height)
        {
            maptileNumber = GameVariables.defaultMapTileReferenced;
        }
    }
}
