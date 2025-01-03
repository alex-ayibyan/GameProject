﻿using GameProject.Entities;
using GameProject.GameState;
using GameProject.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace GameProject.Controllers
{
    public static class EntityController
    {

        private static readonly MapGenerator _gameWorld;

        public static void CreateBullet(Vector2 position, Direction direction, Texture2D texture, bool firedByPlayer)
        {
            var bullet = new Bullet(position, direction, texture, firedByPlayer, _gameWorld);
            Bullet.bullets.Add(bullet);
        }


        public static void CreateEnemy(Vector2 position, Texture2D texture)
        {
            var enemy = new Enemy(position, texture, _gameWorld);
            Enemy.Enemies.Add(enemy);
        }


        public static void Update(GameTime gameTime, Player player, Vector2 playerPosition, bool isPlayerDead, ScoreController score)
        {

            foreach (var bullet in Bullet.bullets)
            {
                bullet.Update(gameTime);

                if (bullet.FiredByPlayer)
                {
                    foreach (var enemy in Enemy.Enemies)
                    {


                        int sum = bullet.radius + enemy.Radius;
                        if (Vector2.Distance(bullet.Position, enemy.Position) < sum)
                        {
                            bullet.Collided = true;

                            if (enemy is TankEnemy tankEnemy)
                            {
                                tankEnemy.TakeDamage();
                                score.UpdateScore(0);
                            }
                            else
                            {
                                enemy.Dead = true;
                                score.UpdateScore(10);
                            }

                        }
                    }
                }
                if (!bullet.FiredByPlayer)
                {
                    int sum = bullet.radius + player.Radius;
                    if (Vector2.Distance(bullet.Position, player.Position) < sum)
                    {
                        bullet.Collided = true;
                        player.TakeDamage();
                    }
                }
            }

            Bullet.bullets.RemoveAll(b => b.Collided);
            Enemy.Enemies.RemoveAll(e => e.Dead);


            foreach (var enemy in Enemy.Enemies)
            {
                enemy.Update(gameTime, playerPosition, isPlayerDead);

                int sum = 32 + enemy.Radius;
                if (Vector2.Distance(player.Position, enemy.Position) < sum && !player.dead)
                {
                    player.TakeDamage();
                }

            }
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            foreach (var bullet in Bullet.bullets)
            {
                bullet.Draw(spriteBatch);
            }

            foreach (var enemy in Enemy.Enemies)
            {
                enemy.Draw(spriteBatch);
            }
        }


        public static void RemoveBullet(Bullet bullet)
        {
            Bullet.bullets.Remove(bullet);
        }
    }
}
