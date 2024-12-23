using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
                    Vector2 leftMove = position + new Vector2(-1, 0) * speed * dt;
                    Vector2 rightMove = position + new Vector2(1, 0) * speed * dt;
                    Vector2 upMove = position + new Vector2(0, -1) * speed * dt;
                    Vector2 downMove = position + new Vector2(0, 1) * speed * dt;

                    // Check all possible directions and move to the first valid one, prioritizing horizontal moves
                    if (CanMove(leftMove) && moveDirection.X > 0) // Move left only if the player is to the right
                    {
                        position = leftMove; // Move left if possible
                    }
                    else if (CanMove(rightMove) && moveDirection.X < 0) // Move right only if the player is to the left
                    {
                        position = rightMove; // Move right if possible
                    }
                    else if (CanMove(upMove)) // If blocked vertically, try moving up
                    {
                        position = upMove;
                    }
                    else if (CanMove(downMove)) // If still blocked, try moving down
                    {
                        position = downMove;
                    }
                    else
                    {
                        // If all directions are blocked, back off slightly and reattempt
                        Vector2 backOffMove = position - moveDirection * speed * dt * 0.5f; // Move back a little
                        if (CanMove(backOffMove))
                        {
                            position = backOffMove; // Move back if possible
                        }
                        else
                        {
                            // If still blocked, attempt random directions (side-step or wiggle)
                            RandomSideStep();
                        }
                    }
                }
            }
        }

        // Method to handle side-stepping if stuck (wiggling)
        private void RandomSideStep()
        {
            // Try random side-stepping movements (left or right)
            Random random = new Random();
            int sideDirection = random.Next(2); // 0 for left, 1 for right

            Vector2 sideMove;
            if (sideDirection == 0)
            {
                sideMove = position + new Vector2(-10, 0) * speed * 0.5f; // Move left
            }
            else
            {
                sideMove = position + new Vector2(10, 0) * speed * 0.5f; // Move right
            }

            // Try side movement if possible
            if (CanMove(sideMove))
            {
                position = sideMove;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
    }
}
