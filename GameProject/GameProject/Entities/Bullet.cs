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

namespace GameProject.Entities
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

        public bool FiredByPlayer { get; set; }


        public Bullet(Vector2 newPosition, Direction newDirection, Texture2D bulletTexture, bool firedByPLayer, MapGenerator gameMap)
        {
            position = newPosition;
            direction = newDirection;
            texture = bulletTexture;
            FiredByPlayer = firedByPLayer;
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

        public void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 proposedPosition = position;


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


            if (CanMove(proposedPosition))
            {
                position = proposedPosition;
            }
            else
            {
                collided = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, new Vector2(position.X - 20, position.Y), Color.White);
        }
    }
}
