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
    public class Enemy
    {
        public static List<Enemy> enemies = new List<Enemy>();

        private Vector2 position = new Vector2(0, 0);
        private int speed = 10;
        public SpriteAnimation animation;
        public int radius = 30;
        private bool dead = false;

        public Enemy(Vector2 newPosition, Texture2D sprite)
        {
            position = newPosition;
            animation = new SpriteAnimation(sprite, 9, 20);
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

        public void Update(GameTime gameTime, Vector2 playerPosition, bool isPlayerDead)
        {
            animation.Position = new Vector2(position.X - 16, position.Y - 16);
            animation.Update(gameTime);

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!isPlayerDead)
            {
                Vector2 moveDirection = playerPosition - position;
                moveDirection.Normalize();
                position += moveDirection * speed * dt;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
    }
}
