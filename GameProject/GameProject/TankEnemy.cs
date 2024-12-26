using GameProject.GameState;
using GameProject.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject
{
    public class TankEnemy : Enemy
    {
        public bool IsStationary { get; set; }
        public bool CanShootBackAtPlayer { get; set; }
        private double shootTimer = 2D;

        public GameWorld _world;
        public Player _player;
        private Controller controller;

        public TankEnemy(Vector2 newPosition, Texture2D sprite, MapGenerator gameMap, GameWorld world, int difficultyLevel) : base(newPosition, sprite, gameMap)
        {
            Speed = 50;
            Radius = 40;
            Health = -1 + difficultyLevel;
            animation = new SpriteAnimation(sprite, 8, 10);

            animation.Scale = 2.5f;

            IsStationary = true;
            CanShootBackAtPlayer = true;

            _world = world;
        }

        public override void Update(GameTime gameTime, Vector2 playerPosition, bool isPlayerDead)
        {
            base.Update(gameTime, playerPosition, isPlayerDead);

            if (IsStationary)
            {
                Speed = 0;
            }

            if (CanShootBackAtPlayer)
            {
                shootTimer -= gameTime.ElapsedGameTime.TotalSeconds;

                if (shootTimer <= 0)
                {
                    ShootAtPlayer();
                    shootTimer = 2D;
                }
            }
        }

        private void ShootAtPlayer()
        {
            Random random = new Random();

            Direction bulletDirection = (Direction)random.Next(0, 4);


            Bullet newBullet = new Bullet(this.Position, bulletDirection, _world.Player.bulletTexture, false, _world.GameMap);
            Bullet.bullets.Add(newBullet);
        }

        public void TakeDamage()
        {
            Health --;
            Debug.WriteLine($"Enemy Health: {Health}");
            if (Health <= 0)
            {
                Dead = true;
            }
        }
    }
}
