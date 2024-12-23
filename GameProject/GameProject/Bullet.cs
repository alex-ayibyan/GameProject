using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
    public class Bullet
    {
        public static List<Bullet> bullets = new List<Bullet>();

        private Vector2 position;
        private int speed = 100;
        public int radius = 18;
        private Direction direction;
        private bool collided = false;

        private Texture2D texture;

        public Bullet(Vector2 newPosition, Direction newDirection, Texture2D bulletTexture)
        {
            position = newPosition;
            direction = newDirection;
            texture = bulletTexture;
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
           
        }

        public bool Collided
        {
            get
            {
                return collided;
            }
            set
            {
                collided = value;
            }
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (direction)
            {
                case Direction.Right:
                    position.X += speed * dt;
                    break; 

                case Direction.Left:
                    position.X -= speed * dt;
                    break;

                case Direction.Up:
                    position.Y -= speed * dt;
                    break;

                case Direction.Down:
                    position.Y += speed * dt;
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(position.X - 160, position.Y - 90), Color.White);
        }
    }
}
