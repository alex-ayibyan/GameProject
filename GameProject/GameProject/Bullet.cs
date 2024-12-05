using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
    class Bullet
    {
        public static List<Bullet> bullets = new List<Bullet>();

        private Vector2 position;
        private int speed = 1000;
        public int radius = 18;
        private Direction direction;
        private bool collided = false;

        public Bullet(Vector2 newPosition, Direction newDirection)
        {
            position = newPosition;
            direction = newDirection;
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
    }
}
