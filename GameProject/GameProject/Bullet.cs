using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject
{
    public class Bullet
    {
        public static List<Bullet> bullets = new List<Bullet>();

        private Vector2 position;
        private int speed = 1000;
        public int radius = 18;
        private Direction direction;
        private bool collided = false;

        private Texture2D texture;
        private MapGenerator _gameMap;


        public Bullet(Vector2 newPosition, Direction newDirection, Texture2D bulletTexture, MapGenerator gameMap)
        {
            position = newPosition;
            direction = newDirection;
            texture = bulletTexture;
            _gameMap = gameMap;
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

        private bool CanMove(Vector2 newPosition)
        {
            int tileSize = 32; // Ensure this matches your actual tile size
            int tileX = (int)(newPosition.X / tileSize);
            int tileY = (int)(newPosition.Y / tileSize);
            Vector2 tileKey = new Vector2(tileX, tileY);

            if (_gameMap.Collision != null)
            {
                if (_gameMap.Collision.TryGetValue(tileKey, out int tileValue))
                {
                    // If the tile is solid (blocked), prevent movement
                    if (tileValue == 96) // Solid tile, collision detected (assuming 96 is the solid tile)
                    {
                        return false;
                    }
                }
            }

            return true; // No collision, movement is allowed
        }

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 proposedPosition = position; // Start with the current position

            // Calculate the proposed new position based on direction
            switch (direction)
            {
                case Direction.Right:
                    proposedPosition.X += speed * dt;
                    break;

                case Direction.Left:
                    proposedPosition.X -= speed * dt;
                    break;

                case Direction.Up:
                    proposedPosition.Y -= speed * dt;
                    break;

                case Direction.Down:
                    proposedPosition.Y += speed * dt;
                    break;
            }

            // Check for collision before updating position
            if (CanMove(proposedPosition))
            {
                position = proposedPosition; // Update position if no collision
            }
            else
            {
                collided = true; // Mark bullet as collided if blocked
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(position.X - 160, position.Y - 90), Color.White);
        }
    }
}
