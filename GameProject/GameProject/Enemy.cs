using GameProject.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace GameProject
{
    public class Enemy
    {
        public static List<Enemy> enemies = new List<Enemy>();

        private Vector2 position = new Vector2(0, 0);
        private int speed = 100;
        public SpriteAnimation animation;
        public int radius = 30;
        private bool dead = false;
        public MapGenerator _gameMap;

        public Enemy(Vector2 newPosition, Texture2D sprite, MapGenerator gameMap)
        {
            position = newPosition;
            animation = new SpriteAnimation(sprite, 9, 20);
            _gameMap = gameMap;
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public bool Dead
        {
            get 
            { 
                return dead; 
            }
            set
            {
                dead = value;
            }
        }

        private bool CanMove(Vector2 newPosition)
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

        public void Update(GameTime gameTime, Vector2 playerPosition, bool isPlayerDead)
        {
            animation.Position = new Vector2(position.X - 16, position.Y - 16);
            animation.Update(gameTime);

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!isPlayerDead)
            { 
                Vector2 moveDirection = playerPosition - position;
                moveDirection.Normalize();

                Vector2 proposedPosition = position + moveDirection * speed * dt;

                if (CanMove(proposedPosition))
                {
                    position = proposedPosition;
                }
                else
                {
                    Vector2[] potentialMoves = new Vector2[]
                    {
                        position + new Vector2(-1, 0) * speed * dt, 
                        position + new Vector2(1, 0) * speed * dt, 
                        position + new Vector2(0, -1) * speed * dt, 
                        position + new Vector2(0, 1) * speed * dt 
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
                        position = bestMove;
                    }
                    else
                    {
                        Vector2 oppositeDirection = -moveDirection;
                        Vector2 backMove = position + oppositeDirection * speed * dt;

                        if (CanMove(backMove))
                        {
                            position = backMove;
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
