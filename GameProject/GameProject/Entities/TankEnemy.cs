using GameProject.Controllers;
using GameProject.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Graphics;

namespace GameProject.Entities
{
    public class TankEnemy : Enemy
    {

        public bool IsStationary { get; set; }
        public bool CanShootBackAtPlayer { get; set; }

        private double shootTimer;
        private double baseShootingSpeed;

        public GameWorld _world;
        public Player _player;
        private readonly Controller _controller;

        public TankEnemy(Vector2 newPosition, Texture2D sprite, MapGenerator gameMap, GameWorld world, Controller controller) : base(newPosition, sprite, gameMap)
        {
            Speed = 50;
            Radius = 40;
            Health = 1;
            animation = new SpriteAnimation(sprite, 5, 10);

            animation.Scale = 2.5f;
            IsStationary = true;
            CanShootBackAtPlayer = true;

            _world = world;
            _controller = controller;

            baseShootingSpeed = Math.Max(_controller.ShootingSpeed, 0.8);
            shootTimer = baseShootingSpeed;
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
                    baseShootingSpeed = Math.Max(_controller.ShootingSpeed, 0.8);
                    shootTimer = baseShootingSpeed;
                }
            }
        }

        private void ShootAtPlayer()
        {
            Random random = new Random();

            Direction bulletDirection = (Direction)random.Next(0, 4);

            Bullet newBullet = new Bullet(Position, bulletDirection, _world.FireBullet, false, _world.GameMap);
            Bullet.bullets.Add(newBullet);
        }

        public void TakeDamage()
        {
            Health--;
            Debug.WriteLine($"Enemy Health: {Health}");
            if (Health <= 0)
            {
                Dead = true;
            }
        }
        public void IncreaseHealth(double amount)
        {
            Health += amount;
            Debug.WriteLine($"TankEnemy Health Increased: {Health}");
        }
    }
}
