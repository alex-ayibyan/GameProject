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
    class Player
    {
        static public Vector2 defaultPosition = new Vector2(640, 360);

        public Vector2 position = defaultPosition;
        private int speed = 300;
        private Direction direction = Direction.Down;
        private bool isMoving = false;
        private KeyboardState kStateOld = Keyboard.GetState();
        public bool dead = false;

        public SpriteAnimation animation;

        public SpriteAnimation[] animations = new SpriteAnimation[4];

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public void setX(float newX)
        {
            position.X = newX;
        }

        public void setY(float newY)
        {
            position.Y = newY;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            isMoving = false;

            if (kState.IsKeyDown(Keys.Right))
            {
                direction = Direction.Right;
                isMoving = true;
            }

            if (kState.IsKeyDown(Keys.Left))
            {
                direction = Direction.Left;
                isMoving = true;
            }

            if (kState.IsKeyDown(Keys.Up))
            {
                direction = Direction.Up;
                isMoving = true;
            }

            if (kState.IsKeyDown(Keys.Down))
            {
                direction = Direction.Down;
                isMoving = true;
            }

            if (dead)
            {
                isMoving = false;
            }

            if (isMoving)
            {
                switch (direction)
                {
                    case Direction.Right:
                        if (position.X < 1100)
                        {
                            position.X += speed * dt;
                        }
                        break;

                    case Direction.Left:
                        if (position.X > 180)
                        {
                            position.X -= speed * dt;
                        }
                        break;

                    case Direction.Up:
                        if (position.Y > 50)
                        {
                            position.Y -= speed * dt;
                        }
                        break;

                    case Direction.Down:
                        if (position.Y < 620)
                        {
                            position.Y += speed * dt;
                        }
                        break;
                }

            }

            animation = animations[(int)direction];

            animation.Position = new Vector2(position.X - 32, position.Y - 32);

            if (kState.IsKeyDown(Keys.Space))
            {
                animation.setFrame(1);
            }
            else if (isMoving)
            {
                animation.Update(gameTime);
            }
            else
            {
                animation.setFrame(0);
            }

            if (kState.IsKeyDown(Keys.Space) && kStateOld.IsKeyUp(Keys.Space))
            {
                Bullet.bullets.Add(new Bullet(position, direction));
            }

            kStateOld = kState;
            
        }

        public void Reset()
        {
            position = defaultPosition;
        }

    }
}
