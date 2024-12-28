using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using GameProject.GameState;
using GameProject.Graphics;
using GameProject.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace GameProject.Entities
{
    public class Player
    {
        static public Vector2 defaultPosition = new Vector2(1600, 1500);

        public Vector2 position = defaultPosition;
        private int speed = 300;
        public Direction direction = Direction.Down;
        private bool isMoving = false;
        private KeyboardState kStateOld = Keyboard.GetState();
        public bool dead = false;
        public float scale = 1.0f;
        public int Radius = 20;

        public SpriteAnimation animation;

        public SpriteAnimation[] animations = new SpriteAnimation[4];
        public Texture2D bulletTexture;

        public SoundEffect shootSound;

        public MapGenerator GameMap;

        public int lives = 3;
        public bool isInvincible = false;
        public double invincibilityTimer = 0.0;
        private const double invincibilityDuration = 2.0;

        private double shootCooldown = 0.5;
        private double timeSinceLastShot = 0.0;

        public float Scale
        {
            get { return scale; }
        }

        public Vector2 Position
        {
            get { return position; }
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

            if (isInvincible)
            {
                invincibilityTimer -= dt;
                if (invincibilityTimer <= 0)
                {
                    isInvincible = false;
                }
            }

            timeSinceLastShot += dt;

            isMoving = false;
            Vector2 proposedPosition = position;

            if (kState.GetPressedKeyCount() > 0)
            {
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
            }

            if (kState.IsKeyDown(Keys.LeftShift))
            {
                speed = 600;
            }
            else
            {
                speed = 300;
            }

            if (dead)
            {
                isMoving = false;
            }

            if (isMoving)
            {
                proposedPosition = position;

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
            }
            animation = animations[(int)direction];
            animation.Position = new Vector2(position.X - 32, position.Y - 32);


            if (kState.IsKeyDown(Keys.Space) && timeSinceLastShot >= shootCooldown)
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

            if (kState.IsKeyDown(Keys.Space) && kStateOld.IsKeyUp(Keys.Space) && timeSinceLastShot >= shootCooldown)
            {
                Debug.WriteLine($"Firing bullet at position: {position}, Direction: {direction}");
                shootSound.Play();
                Bullet.bullets.Add(new Bullet(position, direction, bulletTexture, true, GameMap));
                timeSinceLastShot = 0.0;

            }

            kStateOld = kState;
        }

        private bool CanMove(Vector2 newPosition)
        {
            int tileSize = 32;
            int tileX = (int)(newPosition.X / tileSize);
            int tileY = (int)(newPosition.Y / tileSize);
            Vector2 tileKey = new Vector2(tileX, tileY);

            if (GameMap != null && GameMap.Collision != null)
            {
                if (GameMap.Collision.TryGetValue(tileKey, out int tileValue))
                {
                    //Debug.WriteLine($"Checking collision at Tile ({tileX}, {tileY}) - Value: {tileValue}");

                    if (tileValue == 96) // collisiontile
                    {
                        //Debug.WriteLine($"Collision detected at Tile ({tileX}, {tileY}) - Value: {tileValue}");
                        return false;
                    }
                }
                else
                {

                }
            }
            else
            {
                Debug.WriteLine("GameMap or Collision layer is null.");
            }

            return true;
        }

        public void TakeDamage()
        {
            if (isInvincible) return;

            lives--;

            if (lives <= 0)
            {
                dead = true;
            }
            else
            {
                isInvincible = true;
                invincibilityTimer = invincibilityDuration;
            }
        }

        public void Reset()
        {
            lives = 3;
            position = defaultPosition;
            dead = false;
            isMoving = false;
        }
    }
}
