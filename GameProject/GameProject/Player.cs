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
        private Vector2 position = new Vector2(500, 300);
        private int speed = 300;
        private Direction direction = Direction.Down;
        private bool isMoving = false;

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

            if (isMoving)
            {
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

            animation = animations[(int)direction];

            animation.Position = new Vector2(position.X - 40, position.Y - 40);

            if (isMoving)
            {
                animation.Update(gameTime);
            }
            else
            {
                animation.setFrame(0);
            }

            
        }

    }
}
