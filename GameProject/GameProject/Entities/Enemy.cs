using GameProject.Graphics;
using GameProject.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameProject.Entities
{
    public class Enemy
    {
        public static readonly List<Enemy> Enemies = new();

        public Vector2 Position { get; set; }
        public int Speed { get; set; }
        public bool Dead { get; set; }
        public int Radius { get; set; }
        public double Health { get; set; }

        public SpriteAnimation animation;
        public MapGenerator _gameMap;

        public Enemy(Vector2 position, Texture2D sprite, MapGenerator gameMap)
        {
            Position = position;
            animation = new SpriteAnimation(sprite, 9, 20);
            _gameMap = gameMap;

            Dead = false;
            Speed = 100;
            Radius = 30;
            Health = 1;
        }

        public bool CanMove(Vector2 newPosition)
        {
            int tileSize = 32;
            int tileX = (int)(newPosition.X / tileSize);
            int tileY = (int)(newPosition.Y / tileSize);
            Vector2 tileKey = new Vector2(tileX, tileY);

            if (_gameMap.Collision != null)
            {
                if (_gameMap.Collision.TryGetValue(tileKey, out int tileValue))
                {
                    if (tileValue == 96)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public virtual void Update(GameTime gameTime, Vector2 playerPosition, bool isPlayerDead)
        {
            animation.Position = new Vector2(Position.X - 16, Position.Y - 16);
            animation.Update(gameTime);

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!isPlayerDead)
            {
                Vector2 moveDirection = playerPosition - Position;
                moveDirection.Normalize();

                Vector2 proposedPosition = Position + moveDirection * Speed * dt;

                if (CanMove(proposedPosition))
                {
                    Position = proposedPosition;
                }
                else
                {
                    Vector2[] potentialMoves = new Vector2[]
                    {
                        Position + new Vector2(-1, 0) * Speed * dt,
                        Position + new Vector2(1, 0) * Speed * dt,
                        Position + new Vector2(0, -1) * Speed * dt,
                        Position + new Vector2(0, 1) * Speed * dt
                    };


                    Vector2 bestMove = Vector2.Zero;
                    float bestDistance = float.MaxValue;
                    bool moveFound = false;

                    foreach (var move in potentialMoves)
                    {
                        if (CanMove(move))
                        {
                            float distanceToPlayer = Vector2.Distance(move, playerPosition);

                            if (distanceToPlayer < bestDistance)
                            {
                                bestDistance = distanceToPlayer;
                                bestMove = move;
                                moveFound = true;
                            }
                        }
                    }

                    if (moveFound)
                    {
                        Position = bestMove;
                    }
                    else
                    {
                        Vector2 oppositeDirection = -moveDirection;
                        Vector2 backMove = Position + oppositeDirection * Speed * dt;

                        if (CanMove(backMove))
                        {
                            Position = backMove;
                        }
                    }
                }
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
    }
}
