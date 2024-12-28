using GameProject.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Graphics;

namespace GameProject.Entities
{
    public class FastEnemy : Enemy
    {
        private Vector2 patrolDirection;
        private float zigzagTimer = 0;
        private bool zigzagLeft = true;
        private Vector2 lastPosition;
        private float stuckTimer = 0;

        public FastEnemy(Vector2 newPosition, Texture2D sprite, MapGenerator gameMap, int speed) : base(newPosition, sprite, gameMap)
        {
            Speed = speed;
            Radius = 20;
            animation = new SpriteAnimation(sprite, 4, 20);

            animation.Scale = 1.5f;
        }

        public override void Update(GameTime gameTime, Vector2 playerPosition, bool isPlayerDead)
        {
            animation.Position = new Vector2(Position.X - 16, Position.Y - 16);
            animation.Update(gameTime);

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!isPlayerDead)
            {
                if (Vector2.Distance(lastPosition, Position) < 1f)
                {
                    stuckTimer += dt;
                }
                else
                {
                    stuckTimer = 0;
                }

                lastPosition = Position;

                Vector2 moveDirection = playerPosition - Position;
                moveDirection.Normalize();

                zigzagTimer += dt;
                if (zigzagTimer >= 0.3f)
                {
                    zigzagLeft = !zigzagLeft;
                    zigzagTimer = 0;
                }

                Vector2 perpendicularDirection = zigzagLeft
                    ? new Vector2(-moveDirection.Y, moveDirection.X)
                    : new Vector2(moveDirection.Y, -moveDirection.X);

                Vector2 proposedPosition = Position + (moveDirection + perpendicularDirection * 0.5f) * Speed * dt;

                if (CanMove(proposedPosition))
                {
                    Position = proposedPosition;
                }
                else
                {
                    Vector2 straightPosition = Position + moveDirection * Speed * dt;

                    if (CanMove(straightPosition))
                    {
                        Position = straightPosition;
                    }
                    else
                    {
                        Vector2 avoidDirection = GetAvoidDirection(moveDirection);
                        Vector2 newPosition = Position + avoidDirection * Speed * dt;

                        if (CanMove(newPosition))
                        {
                            Position = newPosition;
                        }
                        else
                        {
                            TryRandomMove(dt);
                        }
                    }
                }

                if (stuckTimer > 2.0f)
                {
                    TryRandomMove(dt);
                    stuckTimer = 0;
                }
            }
        }

        private Vector2 GetAvoidDirection(Vector2 moveDirection)
        {
            Random random = new Random();
            bool avoidLeft = random.NextDouble() > 0.5;

            if (avoidLeft)
            {
                return new Vector2(-moveDirection.Y, moveDirection.X);
            }
            else
            {
                return new Vector2(moveDirection.Y, -moveDirection.X);
            }
        }

        private void TryRandomMove(float dt)
        {
            Random random = new Random();
            Vector2 randomDirection = new Vector2(
                (float)(random.NextDouble() * 2 - 1),
                (float)(random.NextDouble() * 2 - 1)
            );
            randomDirection.Normalize();

            Vector2 randomPosition = Position + randomDirection * Speed * dt;
            if (CanMove(randomPosition))
            {
                Position = randomPosition;
            }
        }

    }

}
