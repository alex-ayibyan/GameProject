﻿using Comora;
using GameProject.Controllers;
using GameProject.Entities;
using GameProject.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.GameState
{
    public class SpecialRoundState : IGameState
    {
        private double displayTime = 1;
        private bool transitionBackToPlaying = false;

        private GameWorld _world;
        private Player _player;
        private ScoreController _score;
        private Camera _camera;

        private Texture2D _regularEnemyTexture;
        private Texture2D _fastEnemyTexture;
        private Texture2D _tankEnemyTexture;

        private MapGenerator _mapGenerator;
        private Controller _controller;

        private float _debugTimer = 0f;
        private float _debugInterval = 4f;

        public double roundCounter = 0;

        private Vector2[] tankEnemySpawnPositions = new Vector2[]
        {
            new Vector2(64 * 32, 40 * 32), // Spawn Position 1 (Top/Right)
            new Vector2(64 * 32, 58 * 32), // Spawn Position 2 (Bottom/Right)
            new Vector2(33 * 32, 58 * 32), // Spawn Position 3 (Bottom/Left)
            new Vector2(33 * 32, 41 * 32)  // Spawn Position 4 (Top/Left)
        };

        public SpecialRoundState(GameWorld world, Player player, ScoreController score, Camera camera, Texture2D tankEnemyTexture)
        {
            _world = world;
            _player = player;
            _score = score;
            _camera = camera;
            _tankEnemyTexture = tankEnemyTexture;
            _mapGenerator = _world.GameMap;

            _controller = new Controller(_world, _mapGenerator, score);
        }

        public void Update(GameTime gameTime)
        {
            
            displayTime -= gameTime.ElapsedGameTime.TotalSeconds;


            _player.Update(gameTime);

            _camera.Position = new Vector2(_player.Position.X, _player.Position.Y);
            //_camera.Position = new Vector2(1500, 1500);
            _camera.Update(gameTime);

            _controller.Update(gameTime, _regularEnemyTexture, _fastEnemyTexture, _tankEnemyTexture);
            EntityController.Update(gameTime, _player, _player.Position, _player.dead, _score);

            if(!transitionBackToPlaying)
    {
                bool allTankEnemiesDead = Enemy.enemies.All(e => !(e is TankEnemy) || e.Dead);
                bool noEnemiesLeft = !Enemy.enemies.Any();

                

                if (_debugTimer >= _debugInterval)
                {
                    Debug.WriteLine($"AllTankEnemiesDead: {allTankEnemiesDead}, NoEnemiesLeft: {noEnemiesLeft}, PlayerDead: {_world.Player.dead}");
                    _debugTimer = 0f;
                }
                if (allTankEnemiesDead || noEnemiesLeft || _world.Player.dead)
                {
                    EndSpecialRound();
                }
            }
            if (_debugTimer >= _debugInterval)
            {
                Debug.WriteLine($"Timer: {gameTime.ElapsedGameTime.TotalSeconds}, Trigger: {_controller.specialTankRoundTriggered}, GameState: {_world._currentState}");

                // Reset the timer
                _debugTimer = 0f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _mapGenerator.Draw(spriteBatch);
            if (_player.isInvincible)
            {
                if ((int)(_player.invincibilityTimer * 5) % 2 == 0)
                {
                    _player.animation.Draw(spriteBatch, Color.White * 0.5f);
                }
                else
                {
                    _player.animation.Draw(spriteBatch, Color.White);
                }
            }
            else
            {
                _player.animation.Draw(spriteBatch, Color.White);
            }

            EntityController.Draw(spriteBatch);

            spriteBatch.DrawString(_world.GeneralFont, $"Score: {_score.Score}", new Vector2(2300, 1000), Color.White);

            foreach (var enemy in Enemy.enemies)
            {
                enemy.Draw(spriteBatch);
            }

            spriteBatch.DrawString(_world.GeneralFont, "Special Round!", new Vector2(2300, 1400), Color.Yellow);

            spriteBatch.DrawString(_world.GeneralFont, $"Enemy Health: {roundCounter+1}", new Vector2(2300, 1500), Color.White);

            spriteBatch.DrawString(_world.GeneralFont, $"Difficulty: {_controller.difficultyLevel}", new Vector2(2300, 1100), Color.White);

            for (int i = 0; i < _player.lives; i++)
            {
                if (_player.isInvincible)
                {
                    if ((int)(_player.invincibilityTimer * 4) % 2 == 0)
                    {
                        spriteBatch.Draw(_world.lifeTexture, new Vector2(2200 + i * 120, 1200), Color.White * 0.5f);
                    }
                    else
                    {
                        spriteBatch.Draw(_world.lifeTexture, new Vector2(2200 + i * 120, 1200), Color.White);
                    }
                }
                else
                {
                    spriteBatch.Draw(_world.lifeTexture, new Vector2(2200 + i * 120, 1200), Color.White);
                }
            }
        }

        public void StartSpecialRound()
        {
            roundCounter += 0.5;

            foreach (var spawnPosition in tankEnemySpawnPositions)
            {
                var specialTank = new TankEnemy(spawnPosition, _tankEnemyTexture, _mapGenerator, _world, _controller)
                {
                    IsStationary = true,
                    CanShootBackAtPlayer = true,
                };
                specialTank.IncreaseHealth(roundCounter);
                Enemy.enemies.Add(specialTank);
            }
            transitionBackToPlaying = false;

        }

        private void EndSpecialRound()
        {
            Debug.WriteLine($"SpecialRoundTriggered flag before reset: {_controller.specialTankRoundTriggered}");

            RemoveTankEnemies();
            _controller.specialTankRoundTriggered = false;
            Debug.WriteLine("Special Round Ended and Flag Reset.");
            _world.ChangeState(GameStates.Playing);
            transitionBackToPlaying = true;

        }

        public void RemoveTankEnemies()
        {
            Enemy.enemies.RemoveAll(e => e is TankEnemy && e.Dead);
        }

        public void ResetRoundCounter()
        {
            roundCounter = 0;
        }

        public Song GetBackgroundMusic()
        {
            return _world.specialRoundMusic;
        }
    }
}
