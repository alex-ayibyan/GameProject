using GameProject.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject
{
    public class TankEnemy : Enemy
    {
        private int tankSpeed = 50;
        private int health = 2;

        public TankEnemy(Vector2 newPosition, Texture2D sprite, MapGenerator gameMap) : base(newPosition, sprite, gameMap)
        {

        }

        public override void Update(GameTime gameTime, Vector2 playerPosition, bool isPlayerDead)
        {
            animation.Position = new Vector2(Position.X - 16, Position.Y - 16);
            animation.Update(gameTime);

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!isPlayerDead)
            {
                Vector2 moveDirection = playerPosition - Position;
                moveDirection.Normalize();

                Vector2 proposedPosition = Position + moveDirection * tankSpeed * dt;

                if (CanMove(proposedPosition))
                {
                    position = proposedPosition;
                }
            }
        }

        public void TakeDamage()
        {
            health--;
            if (health <= 0)
            {
                Dead = true;
            }
        }
    }
}
