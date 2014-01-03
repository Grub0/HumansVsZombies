using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace HumansVsZombies.cs
{
    class Zombie : GamePiece
    {
        #region Attributes

        bool isAlive;
        int scorePoints;
        double aiTimer;
        double movingTimer;
        double defaultWaitTime;
        int defaultRange;
        int defaultChaseSpeed;
        int health;

        Direction d;
        int flippedDirection;
        Rectangle previousZombiePosition;
        Rectangle currentZombiePosition;

        //Textures
        Texture2D _zombie0A;
        Texture2D _zombie0B;
        Texture2D _zombie1A;
        Texture2D _zombie1B;
        Texture2D _zombie2A;
        Texture2D _zombie2B;
        Texture2D _zombie3A;
        Texture2D _zombie3B;
        Texture2D _currentZombie;
        Texture2D _previousZombie;

        #endregion

        #region Properties

        /// <summary>
        /// Returns true if zombie is still alive, false if he isn't
        /// </summary>
        public bool IsAlive { get { return isAlive; } set { isAlive = value; } }

        /// <summary>
        /// Health of the Zombie. MAX SETTING SHOULD BE THREE I THINK FOR ADEQUATE BALANCE. MIGHT WANT TO MAKE A CONSTANT OF THIS.
        /// </summary>
        public int Health { get { return health; } set { health = value; } }

        /// <summary>
        /// How many points the zomibe is worth
        /// </summary>
        public int ScorePoints { get { return scorePoints; } set { scorePoints = value; } }

        /// <summary>
        /// The timer that determines what the zombie is doing
        /// </summary>
        public double AITimer { get { return aiTimer; } set { aiTimer = value; } }

        /// <summary>
        /// The time it takes for the zombie to move while waiting
        /// </summary>
        public double MovingTimer { get { return movingTimer; } set { movingTimer = value; } }

        /// <summary>
        /// The default time that all zombies will make before moving on its own
        /// </summary>
        public double DefaultWaitTime { get { return defaultWaitTime; } set { defaultWaitTime = value; } }

        /// <summary>
        /// The Range in which the zombie will pursue the human
        /// </summary>
        public int DefaultRange { get { return defaultRange; } set { defaultRange = value; } }

        /// <summary>
        /// How fast the zombie will chase the human
        /// </summary>
        public int DefaultChaseSpeed { get { return defaultChaseSpeed; } set { defaultChaseSpeed = value; } }

        /// <summary>
        /// Get the current direction state
        /// </summary>
        public Direction DState { get { return d; } set { d = value; } }

        /// <summary>
        /// How many times the zombie will flip when waiting
        /// </summary>
        public int FlippedDirection { get { return flippedDirection; } set { flippedDirection = value; } }

        /// <summary>
        /// One of the 2 pictures of the zombie moving up
        /// </summary>
        public Texture2D UpA { get { return _zombie0A; } set { _zombie0A = value; } }

        /// <summary>
        /// One of the 2 pictures of the zombie moving up
        /// </summary>
        public Texture2D UpB { get { return _zombie0B; } set { _zombie0B = value; } }

        /// <summary>
        /// One of the 2 pictures of the zombie moving right
        /// </summary>
        public Texture2D RightA { get { return _zombie1A; } set { _zombie1A = value; } }

        /// <summary>
        /// One of the 2 pictures of the zombie moving right
        /// </summary>
        public Texture2D RightB { get { return _zombie1B; } set { _zombie1B = value; } }

        /// <summary>
        /// One of the 2 pictures of the zombie moving down
        /// </summary>
        public Texture2D DownA { get { return _zombie2A; } set { _zombie2A = value; } }

        /// <summary>
        /// One of the 2 pictures of the zombie moving down
        /// </summary>
        public Texture2D DownB { get { return _zombie2B; } set { _zombie2B = value; } }

        /// <summary>
        /// One of the 2 pictures of the zombie moving left
        /// </summary>
        public Texture2D LeftA { get { return _zombie3A; } set { _zombie3A = value; } }

        /// <summary>
        /// One of the 2 pictures of the zombie moving left
        /// </summary>
        public Texture2D LeftB { get { return _zombie3B; } set { _zombie3B = value; } }

        /// <summary>
        /// The current A or B picture the zombie is
        /// </summary>
        public Texture2D CurrentZombie { get { return _currentZombie; } set { _currentZombie = value; } }

        /// <summary>
        /// The previous A or B picture that the zombie was
        /// </summary>
        public Texture2D PreviousZombie { get { return _previousZombie; } set { _previousZombie = value; } }

        #endregion

        #region Constructor

        /// <summary>
        /// The actual zombie object
        /// </summary>
        /// <param name="newX">The X value</param>
        /// <param name="newY">The Y value</param>
        /// <param name="width">The width of the zombie box</param>
        /// <param name="height">The height of the zombie box</param>
        /// <param name="f">What direction the zombie starts facing off in</param>
        public Zombie(int newX, int newY, int width, int height)
            : base(newX, newY, width, height)
        {
            base._Texture = UpA;
            scorePoints = GameVariables.zombieScorePoints;
            defaultWaitTime = GameVariables.zombieDefaultWaitTime;
            aiTimer = defaultWaitTime;
            movingTimer = GameVariables.zombieMovingTimer;
            defaultRange = GameVariables.zombieDefaultRange;
            defaultChaseSpeed = GameVariables.zombieDefaultChaseSpeed;
            d = new Direction();
            flippedDirection = 1;
            _currentZombie = _zombie0A;
            _previousZombie = _zombie0B;
            currentZombiePosition = new Rectangle(newX, newY, width, height);
            health = GameVariables.zombieHealth;
        }

        #endregion

        #region Draw

        /// <summary>
        /// The method that draws the zombie object
        /// </summary>
        /// <param name="pict">The necessary spritebatch</param>
        /// <param name="drawing">The picture to draw</param>
        public override void Draw(SpriteBatch pict)
        {
            if (isAlive == true)
            {
                base.Draw(pict);
            }
        }

        #endregion

        #region AI

        /// <summary>
        /// Rudimentary AI For the basic Zombie Class
        /// </summary>
        /// <param name="gp">The Zombie in question</param>
        /// <param name="t"></param>
        public void AI(GamePiece gp, double t)
        {
            aiTimer -= t;   // The passed in time is subtracted from the timer
            previousZombiePosition = currentZombiePosition; // The previous picture now becomes the current

            if (CheckCollision(gp) == true) { } // Once it collides then just stand there and attack

            // Otherwise
            else
            {
                if (this._Rectangle.X <= gp._Rectangle.X && gp._Rectangle.X <= this._Rectangle.X + defaultRange &&      // If in the upper left quadrant
                        this._Rectangle.Y >= gp._Rectangle.Y && gp._Rectangle.Y >= this._Rectangle.Y - defaultRange)    // pursue player there
                {
                    this.YValue -= defaultChaseSpeed;   // By moving up
                    d = Direction.Up;
                    aiTimer = defaultWaitTime;          // Reset the timer
                }

                if (this._Rectangle.X <= gp._Rectangle.X && gp._Rectangle.X <= this._Rectangle.X + defaultRange &&      // If in the upper right quadrant
                    this._Rectangle.Y <= gp._Rectangle.Y && gp._Rectangle.Y <= this._Rectangle.Y + defaultRange)        // pursue player there
                {
                    this.XValue += defaultChaseSpeed;   // By moving right
                    d = Direction.Right;
                    aiTimer = defaultWaitTime;          // Rese the timer
                }

                if (this._Rectangle.X >= gp._Rectangle.X && gp._Rectangle.X >= this._Rectangle.X - defaultRange &&      // If in the lower right quadrant
                    this._Rectangle.Y <= gp._Rectangle.Y && gp._Rectangle.Y <= this._Rectangle.Y + defaultRange)        // pursue player there
                {
                    this.YValue += defaultChaseSpeed;   // By moving down
                    d = Direction.Down;
                    aiTimer = defaultWaitTime;          // Reset the timer
                }

                if (this._Rectangle.X >= gp._Rectangle.X && gp._Rectangle.X >= this._Rectangle.X - defaultRange &&      // If in the lower left quadrant
                    this._Rectangle.Y >= gp._Rectangle.Y && gp._Rectangle.Y >= this._Rectangle.Y - defaultRange)        // pursue player there
                {
                    this.XValue -= defaultChaseSpeed;   // By moving left
                    d = Direction.Left;
                    aiTimer = defaultWaitTime;          // Reset the timer
                }

                currentZombiePosition = this._Rectangle;    // Now make the current zombie position into what it is

                // Now since we're only reseting the timer if the zombie moved then
                // If he hasn't moved for the default wait time, then...
                if (aiTimer < 0)
                {
                    movingTimer -= t;       // The timer for how long the zombie moves
                    if (movingTimer > 0)    // As long as the timer is greater than 0
                    {
                        if (flippedDirection == 1)  // Change direction ONCE
                        {
                            switch (d)
                            {
                                case Direction.Up:
                                    {
                                        d = Direction.Down;
                                        flippedDirection--;
                                        break;
                                    }

                                case Direction.Right:
                                    {
                                        d = Direction.Left;
                                        flippedDirection--;
                                        break;
                                    }

                                case Direction.Down:
                                    {
                                        d = Direction.Up;
                                        flippedDirection--;
                                        break;
                                    }

                                case Direction.Left:
                                    {
                                        d = Direction.Right;
                                        flippedDirection--;
                                        break;
                                    }
                            }
                        }

                        switch (d)  // Then based on what the zombie's currently set direction
                        {
                            case Direction.Down:    // Move down and set the current zombie position to it
                                {
                                    this.YValue++;
                                    currentZombiePosition = this._Rectangle;
                                    break;
                                }

                            case Direction.Right:   // Move right and set the current zombie position to it
                                {
                                    this.XValue++;
                                    currentZombiePosition = this._Rectangle;
                                    break;
                                }

                            case Direction.Up:      // Move up and set the current zombie position to it
                                {
                                    this.YValue--;
                                    currentZombiePosition = this._Rectangle;
                                    break;
                                }

                            case Direction.Left:    // Move left and set the current zombie position to it
                                {
                                    this.XValue--;
                                    currentZombiePosition = this._Rectangle;
                                    break;
                                }
                        }
                    }

                    // Then once it has moved for the set amount of time then
                    // reset both the move and ai timers and the int that allows
                    // the zombie to flip direction once
                    else
                    {
                        aiTimer = defaultWaitTime; 
                        movingTimer = 5.0;
                        flippedDirection = 1;
                    }
                }
            }
        }

        #endregion

        #region CheckZombieDirection

        /// <summary>
        /// Sets the direction, footing, and new texture of a zombie
        /// </summary>
        /// <param name="z"></param>
        public void CheckZombieDirection()
        {
            switch (this.DState)
            {
                //The Zombie in facing upward/going upward
                case Zombie.Direction.Up:
                    {
                        if (_currentZombie == _zombie1A || _currentZombie == _zombie1B || _currentZombie == _zombie2A ||
                            _currentZombie == _zombie2B || _currentZombie == _zombie3A || _currentZombie == _zombie3B)

                        //if (_currentZombie != _zombie0A || _currentZombie != _zombie0B)
                        {
                            _previousZombie = _zombie0A;
                            _currentZombie = _zombie0B;
                        }

                        Texture2D p = _previousZombie;
                        _previousZombie = _currentZombie;
                        _currentZombie = p;
                        break;
                    }
                //The zombie is facing the right/traversing right
                case Zombie.Direction.Right:
                    {
                        if (_currentZombie == _zombie0A || _currentZombie == _zombie0B || _currentZombie == _zombie2A ||
                            _currentZombie == _zombie2B || _currentZombie == _zombie3A || _currentZombie == _zombie3B)

                        //if (_currentZombie != _zombie1A || _currentZombie != _zombie1B)
                        {
                            _previousZombie = _zombie1A;
                            _currentZombie = _zombie1B;
                        }

                        Texture2D p = _previousZombie;
                        _previousZombie = _currentZombie;
                        _currentZombie = p;
                        break;
                    }
                //the Zombie is facing downwards/going downward
                case Zombie.Direction.Down:
                    {
                        if (_currentZombie == _zombie0A || _currentZombie == _zombie0B || _currentZombie == _zombie1A ||
                            _currentZombie == _zombie1B || _currentZombie == _zombie3A || _currentZombie == _zombie3B)

                        //if (_currentZombie != _zombie2A || _currentZombie != _zombie2B)
                        {
                            _previousZombie = _zombie2A;
                            _currentZombie = _zombie2B;
                        }

                        Texture2D p = _previousZombie;
                        _previousZombie = _currentZombie;
                        _currentZombie = p;
                        break;
                    }
                //the zombie is facing left/traversing left
                case Zombie.Direction.Left:
                    {
                        if (_currentZombie == _zombie0A || _currentZombie == _zombie0B || _currentZombie == _zombie1A ||
                            _currentZombie == _zombie1B || _currentZombie == _zombie2A || _currentZombie == _zombie2B)

                        //if (_currentZombie != _zombie3A || _currentZombie != _zombie3B)
                        {
                            _previousZombie = _zombie3A;
                            _currentZombie = _zombie3B;
                        }

                        Texture2D p = _previousZombie;
                        _previousZombie = _currentZombie;
                        _currentZombie = p;
                        break;
                    }
            }
        }

        #endregion

        #region HitOtherZombie

        /// <summary>
        /// What to do if they hit another zombie
        /// </summary>
        /// <param name="z">The zombie it collides with</param>
        public void HitOtherZomibe(Zombie z)
        {
            if (this.CheckCollision(z) == true)
            {
                this.XValue = previousZombiePosition.X;
                this.YValue = previousZombiePosition.Y;
                currentZombiePosition = previousZombiePosition;
            }
        }

        #endregion

        #region RecursiveSummoning

        /// <summary>
        /// Summons Lots of Zombies, Fast.
        /// </summary>
        /// <param name="z">Zombie in question</param>
        /// <param name="graphicsWidth">Graphics Width</param>
        /// <param name="graphicsHeight">Graphics Height</param>
        public void RecursiveSummoning(Zombie z, int graphicsWidth, int graphicsHeight)
        {
            if (this.CheckCollision(z) == true)
            {
                Random rand = new Random();
                this.XValue = rand.Next(rand.Next(100, graphicsWidth) - 100);
                this.YValue = rand.Next(rand.Next(100, graphicsHeight) - 100);
                RecursiveSummoning(z, graphicsWidth, graphicsHeight);
            }
        }

        #endregion
    }
}
