using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HumansVsZombies.cs
{
    public class GamePiece
    {
        #region Attributes

        Texture2D _texture;
        Rectangle _rectangle;
        public enum Direction { Up, Right, Down, Left };
        Direction direction;

        #endregion

        #region Properties

        public Texture2D _Texture { get { return _texture; } set { _texture = _Texture; } }
        public Rectangle _Rectangle { get { return _rectangle; } set { _rectangle = _Rectangle; } }
        public int XValue { get { return _rectangle.X; } set { _rectangle.X = value; } }
        public int YValue { get { return _rectangle.Y; } set { _rectangle.Y = value; } }
        public Direction GetSetDirection { get { return direction; } set { direction = value; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor for Gamepiece
        /// </summary>
        /// <param name="x">X Coordinate</param>
        /// <param name="y">Y Coordinate</param>
        /// <param name="w">Widtht (in pixels)</param>
        /// <param name="h">Height (in pixels)</param>
        public GamePiece(int x, int y, int w, int h)
        {
            _rectangle = new Rectangle(x, y, w, h);
            _rectangle.X = x;
            _rectangle.Y = y;
            _rectangle.Width = w;
            _rectangle.Height = h;
        }

        #endregion

        #region Draw

        /// <summary>
        /// Draws the gamepiece for the screen
        /// </summary>
        /// <param name="s">Spritebatch to be opened</param>
        public virtual void Draw(SpriteBatch s)
        {
            s.Draw(_texture, _rectangle, Color.White);
        }

        #endregion

        #region CheckCollision

        /// <summary>
        /// The method that checks whether or not the zombie has been hit
        /// </summary>
        /// <param name="gp">The game object in question (probably should be the player)</param>
        /// <returns>True if there is collision; false if there isn't</returns>
        public virtual bool CheckCollision(GamePiece gp)
        {
            if (gp._Rectangle.Intersects(this._Rectangle))
            {
                return true;
            }

            else { return false; }
        }

        #endregion
    }
}
