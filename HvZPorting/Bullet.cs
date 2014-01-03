using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HumansVsZombies.cs
{
    class Bullet
    {

        // Image representing the Projectile
        public Texture2D Texture;

        // Position of the Projectile relative to the upper left side of the screen
        public Vector2 Position;

        // State of the Projectile
        public bool Active;

        // The amount of damage the projectile can inflict to an enemy
        public int Damage;

        // Represents the viewable boundary of the game
        Viewport viewport;

        public GamePiece.Direction D;
        //player
        Player p;

        public int Width
        {
            get { return Texture.Width; }
        }


        public int Height
        {
            get { return Texture.Height; }
        }

        // Determines how fast the projectile moves
        float projectileMoveSpeed;


        public void Initialize(Viewport viewport, Texture2D texture, Vector2 position,GamePiece.Direction dir)
        {
            Texture = texture;
            Position = position;
            this.viewport = viewport;
            D = dir;
            Active = true;

            //change the movement speed here
            projectileMoveSpeed = 20f;
        }

        public void Update(Player p1)
        {
            // Projectiles always move to the right
            if (D == GamePiece.Direction.Right)
            {
                Position.X += projectileMoveSpeed;
            }
            else if (D == GamePiece.Direction.Up)
            {
                Position.Y -= projectileMoveSpeed;
            }
            else if (D == GamePiece.Direction.Down)
            {
                Position.Y += projectileMoveSpeed;
            }
            else if (D == GamePiece.Direction.Left)
            {
                Position.X -= projectileMoveSpeed;
            }

            // Deactivate the bullet if it goes out of screen
            if (Position.X + Texture.Width / 2 > viewport.Width)
                Active = false;
            else if (Position.X - Texture.Width / 2 > viewport.Width)
                Active = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, 0f,
            new Vector2(Width / 2 - GameVariables.spriteHeight / 2, Height / 2 - GameVariables.spriteWidth / 2), 1f, SpriteEffects.None, 0f);
        }
    }
}
