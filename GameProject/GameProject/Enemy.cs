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
    class Enemy
    {
        public static List<Enemy> enemies = new List<Enemy>();

        private Vector2 position = new Vector2(0, 0);
        private int speed = 150;
        public SpriteAnimation animation;

        public Enemy(Vector2 newPosition, Texture2D sprite)
        {
            position = newPosition;
            animation = new SpriteAnimation(sprite, 9, 15);
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public void Update(GameTime gameTime)
        {
            animation.Position = new Vector2(position.X - 16, position.Y - 16);
            animation.Update(gameTime);
        }
    }
}
