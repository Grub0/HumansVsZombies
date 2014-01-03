using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HumansVsZombies.cs
{
    class MapTile : GamePiece
    {
        #region Attributes

        List<Zombie> zombies;
        HorizontalBarrier _bUp0;
        VerticalBarrier _bRight1;
        HorizontalBarrier _bDown2;
        VerticalBarrier _bLeft3;

        int gameWidth;
        int gameHeight;
        int spriteWidth;
        int spriteHeight;
        Random rand;

        public bool extraction;

        Texture2D backgroundImage;

        List<DartCollectible> collectibleDarts;


        #endregion

        #region Properties

        /// <summary>
        /// Get and/or set the list of zombies
        /// </summary>
        public List<Zombie> GetSetZombieList { get { return zombies; } set { zombies = value; } }

        /// <summary>
        /// Get and/or set the top barrier
        /// </summary>
        public HorizontalBarrier GetSetBarrierUp0 { get { return _bUp0; } set { _bUp0 = value; } }

        /// <summary>
        /// Get and/or set the right barrier
        /// </summary>
        public VerticalBarrier GetSetBarrierRight1 { get { return _bRight1; } set { _bRight1 = value; } }

        /// <summary>
        /// Get and/or set the bottom barrier
        /// </summary>
        public HorizontalBarrier GetSetBarrierDown2 { get { return _bDown2; } set { _bDown2 = value; } }

        /// <summary>
        /// Get and/or set the left barrier
        /// </summary>
        public VerticalBarrier GetSetBarrierLeft3 { get { return _bLeft3; } set { _bLeft3 = value; } }

        /// <summary>
        /// Get and/or set the background image
        /// </summary>
        public Texture2D GetSetBackgroundImage { get { return backgroundImage; } set { backgroundImage = value; } }

        /// <summary>
        /// Get and/or set the background image
        /// </summary>
        public List<DartCollectible> GetSetDartCollectibles { get { return collectibleDarts; } set { collectibleDarts = value; } }

        #endregion

        #region Constructor

        /// <summary>
        /// This is to set up the background image with both zombies and barriers
        /// </summary>
        /// <param name="numZombies">The # of zombies that will be on this maptile</param>
        /// <param name="width">The width of the background (please use the screen width)</param>
        /// <param name="height">The height of the background (please use the screen height)</param>
        /// <param name="b">The background texture that this is</param>
        public MapTile(int numZombies, int numDarts, int width, int height, Texture2D b)
            : base(0, 0, width, height)
        {
            zombies = new List<Zombie>();
            collectibleDarts = new List<DartCollectible>();
            gameWidth = width;
            gameHeight = height;
            spriteWidth = GameVariables.spriteWidth;
            spriteHeight = GameVariables.spriteHeight;
            rand = new Random();

            // The 4 possible barriers that is going to be within the map tile
            _bUp0 = new HorizontalBarrier(gameWidth / 2 - GameVariables.barrierHigherNumber / 2, 0);
            _bRight1 = new VerticalBarrier(gameWidth - GameVariables.barrierLowerNumber, gameHeight / 2 - GameVariables.barrierHigherNumber / 2);
            _bDown2 = new HorizontalBarrier(gameWidth / 2 - GameVariables.barrierHigherNumber / 2, gameHeight - GameVariables.barrierLowerNumber);
            _bLeft3 = new VerticalBarrier(0, gameHeight / 2 - GameVariables.barrierHigherNumber / 2);

            // Get the list of zombies for this individual maptile
            SpawnZombies(numZombies);

            // Get the list of collectible darts for this individual maptile
            SpawnCollectibleDarts(numDarts);

            backgroundImage = b;
        }

        #endregion

        #region SpawnZombies

        /// <summary>
        /// Creates all of the zombies and where they should go
        /// </summary>
        /// <param name="num">The number of zombies you want to spawn</param>
        public void SpawnZombies(int num)
        {
            for (int i = 0; i < num; i++)
            {
                zombies.Add(new Zombie(rand.Next(100, gameWidth) - 100, rand.Next(100, gameHeight) - 100, spriteWidth, spriteHeight));
                // Where all the zombies will be spawn
            }

            for (int i = 0; i < zombies.Count; i++)
            {
                for (int j = 0; j < zombies.Count; j++)
                {
                    if (i != j)
                    {
                        zombies.ElementAt(i).RecursiveSummoning(zombies.ElementAt(j), gameWidth, gameHeight);       // To make sure none of them are on top
                    }                                                                                       // of one another
                }
            }
        }
        #endregion

        #region SpawnCollectibleDarts

        public void SpawnCollectibleDarts(int num)
        {
            for (int i = 0; i < num; i++)
            {
                // Where all the darts will be spawned
                collectibleDarts.Add(new DartCollectible(rand.Next(100, gameWidth) - 100, rand.Next(100, gameHeight) - 100, spriteWidth, spriteHeight));
            }
        }

        #endregion
        public void ExtractionTrue()
        {
            extraction = true;
        }

    }
}
