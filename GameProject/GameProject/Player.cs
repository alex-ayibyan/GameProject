using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using GameProject.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameProject
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

        public MapGenerator GameMap;

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

            isMoving = false;
            Vector2 proposedPosition = position;

            // Movement input
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
                speed = 600; // Sprint speed
            }
            else
            {
                speed = 300; // Normal speed
            }

            // Prevent movement if dead
            if (dead)
            {
                isMoving = false;
            }

            // Movement logic
            if (isMoving)
            {
                // Save the proposed new position
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

                // Check if the proposed new position is valid (no collision)
                if (CanMove(proposedPosition))
                {
                    position = proposedPosition;  // Apply the movement if no collision
                }
            }

            // Update the animation
            animation = animations[(int)direction];
            animation.Position = new Vector2(position.X - 32, position.Y - 32);

            // Handle animation frame for movement or idle state
            if (kState.IsKeyDown(Keys.Space))
            {
                animation.setFrame(1); // Attack frame when space is pressed
            }
            else if (isMoving)
            {
                animation.Update(gameTime); // Update animation while moving
            }
            else
            {
                animation.setFrame(0); // Idle frame
            }

            // Bullet firing logic
            if (kState.IsKeyDown(Keys.Space) && kStateOld.IsKeyUp(Keys.Space))
            {
                Debug.WriteLine($"Firing bullet at position: {position}, Direction: {direction}");
                Bullet.bullets.Add(new Bullet(position, direction, bulletTexture, true, GameMap));
            }

            // Store the current keyboard state for next frame
            kStateOld = kState;
        }

        // Check if the player can move to the new position (collision detection)
        private bool CanMove(Vector2 newPosition)
        {
            int tileSize = 32; // Ensure this matches your actual tile size
            int tileX = (int)(newPosition.X / tileSize);
            int tileY = (int)(newPosition.Y / tileSize);
            Vector2 tileKey = new Vector2(tileX, tileY);

            if (GameMap != null && GameMap.Collision != null)
            {
                if (GameMap.Collision.TryGetValue(tileKey, out int tileValue))
                {
                    //Debug.WriteLine($"Checking collision at Tile ({tileX}, {tileY}) - Value: {tileValue}");

                    if (tileValue == 96) // Solid tile
                    {
                        //Debug.WriteLine($"Collision detected at Tile ({tileX}, {tileY}) - Value: {tileValue}");
                        return false; // Block movement
                    }
                }
                else
                {
                    //Debug.WriteLine($"Tile Key {tileKey} not found. Assuming no collision.");
                }
            }
            else
            {
                Debug.WriteLine("GameMap or Collision layer is null.");
            }

            return true; // Allow movement if no collision is detected
        }

        public void Reset()
        {
            position = defaultPosition;
            dead = false;
            isMoving = false;
        }
    }
}
