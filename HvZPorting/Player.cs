using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HumansVsZombies.cs
{
    class Player : GamePiece
    {
        #region Attributes

        public int levelScore;
        public int totalScore;
        public int health;
        public int moveSpeed;
        public Texture2D _health;
        public Texture2D _twinkie3;
        public Texture2D _twinkie2;
        public Texture2D _twinkie1;

        // Textures
        Texture2D _player0A;
        Texture2D _player0B;
        Texture2D _player1A;
        Texture2D _player1B;
        Texture2D _player2A;
        Texture2D _player2B;
        Texture2D _player3A;
        Texture2D _player3B;
        Texture2D _currentPlayer;
        Texture2D _previousPlayer;
        Direction d;

        #endregion

        #region Properties

        /// <summary>
        /// Get the current direction state
        /// </summary>
        public Direction DState { get { return d; } set { d = value; } }

        /// <summary>
        /// One of the 2 pictures of the zombie moving up
        /// </summary>
        public Texture2D UpA { get { return _player0A; } set { _player0A = value; } }

        /// <summary>
        /// One of the 2 pictures of the zombie moving up
        /// </summary>
        public Texture2D UpB { get { return _player0B; } set { _player0B = value; } }

        /// <summary>
        /// One of the 2 pictures of the zombie moving right
        /// </summary>
        public Texture2D RightA { get { return _player1A; } set { _player1A = value; } }

        /// <summary>
        /// One of the 2 pictures of the zombie moving right
        /// </summary>
        public Texture2D RightB { get { return _player1B; } set { _player1B = value; } }

        /// <summary>
        /// One of the 2 pictures of the zombie moving down
        /// </summary>
        public Texture2D DownA { get { return _player2A; } set { _player2A = value; } }

        /// <summary>
        /// One of the 2 pictures of the zombie moving down
        /// </summary>
        public Texture2D DownB { get { return _player2B; } set { _player2B = value; } }

        /// <summary>
        /// One of the 2 pictures of the zombie moving left
        /// </summary>
        public Texture2D LeftA { get { return _player3A; } set { _player3A = value; } }

        /// <summary>
        /// One of the 2 pictures of the zombie moving left
        /// </summary>
        public Texture2D LeftB { get { return _player3B; } set { _player3B = value; } }

        /// <summary>
        /// The current A or B picture the zombie is
        /// </summary>
        public Texture2D CurrentPlayer { get { return _currentPlayer; } set { _currentPlayer = value; } }

        /// <summary>
        /// The previous A or B picture that the zombie was
        /// </summary>
        public Texture2D PreviousPlayer { get { return _previousPlayer; } set { _previousPlayer = value; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for the player class
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <param name="w">width (in pixels)</param>
        /// <param name="h">height (in pixels)</param>
        public Player(int x, int y, int w, int h)
            : base(x, y, w, h)
        {
            health = GameVariables.humanHealth;
            levelScore = 0;
            totalScore = 0;
            _currentPlayer = _player0A;
            _previousPlayer = _player0B;
            moveSpeed = 10;
        }

        #endregion

        #region TakeHit

        /// <summary>
        /// Simulates the player loosing health
        /// </summary>
        public void TakeHit()
        {
            health -= 1;
            if (health == 3)
            {
                //changes the state of the twinkie picture based on health
                _health = _twinkie3;
            }
            else if (health == 2)
            {
                _health = _twinkie2;
            }
            else if (health == 1)
            {
                _health = _twinkie1;
            }
        }

        #endregion

        #region CheckPlayerDirection

        /// <summary>
        /// Sets the direction, footing, and new texture of a zombie
        /// </summary>
        public void CheckPlayerDirection()
        {
            switch (this.DState)
            {
                case Player.Direction.Up:
                    {
                        if (_currentPlayer == _player1A || _currentPlayer == _player1B || _currentPlayer == _player2A ||
                            _currentPlayer == _player2B || _currentPlayer == _player3A || _currentPlayer == _player3B)

                        //if (_currentPlayer != _player0A || _currentPlayer != _player0B)
                        {
                            _previousPlayer = _player0A;
                            _currentPlayer = _player0B;
                        }

                        Texture2D p = _previousPlayer;
                        _previousPlayer = _currentPlayer;
                        _currentPlayer = p;
                        break;
                    }

                case Player.Direction.Right:
                    {
                        if (_currentPlayer == _player0A || _currentPlayer == _player0B || _currentPlayer == _player2A ||
                            _currentPlayer == _player2B || _currentPlayer == _player3A || _currentPlayer == _player3B)

                        //if (_currentPlayer != _player1A || _currentPlayer != _player1B)
                        {
                            _previousPlayer = _player1A;
                            _currentPlayer = _player1B;
                        }

                        Texture2D p = _previousPlayer;
                        _previousPlayer = _currentPlayer;
                        _currentPlayer = p;
                        break;
                    }

                case Player.Direction.Down:
                    {
                        if (_currentPlayer == _player0A || _currentPlayer == _player0B || _currentPlayer == _player1A ||
                            _currentPlayer == _player1B || _currentPlayer == _player3A || _currentPlayer == _player3B)

                        //if (_currentPlayer != _player2A || _currentPlayer != _player2B)
                        {
                            _previousPlayer = _player2A;
                            _currentPlayer = _player2B;
                        }

                        Texture2D p = _previousPlayer;
                        _previousPlayer = _currentPlayer;
                        _currentPlayer = p;
                        break;
                    }

                case Player.Direction.Left:
                    {
                        if (_currentPlayer == _player0A || _currentPlayer == _player0B || _currentPlayer == _player1A ||
                            _currentPlayer == _player1B || _currentPlayer == _player2A || _currentPlayer == _player2B)

                        //if (_currentPlayer != _player3A || _currentPlayer != _player3B)
                        {
                            _previousPlayer = _player3A;
                            _currentPlayer = _player3B;
                        }

                        Texture2D p = _previousPlayer;
                        _previousPlayer = _currentPlayer;
                        _currentPlayer = p;
                        break;
                    }
            }
        }

        #endregion
    }
}
