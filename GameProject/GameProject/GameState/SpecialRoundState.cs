using Comora;
using GameProject.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

                Debug.WriteLine($"AllTankEnemiesDead: {allTankEnemiesDead}, NoEnemiesLeft: {noEnemiesLeft}, PlayerDead: {_world.Player.dead}");

                if (allTankEnemiesDead || noEnemiesLeft || _world.Player.dead)
                {
                    EndSpecialRound();
                }
            }
            Debug.WriteLine($"TankEnemies remaining: {Enemy.enemies.Count(e => e is TankEnemy && !e.Dead)}");
            Debug.WriteLine($"Timer: {gameTime.ElapsedGameTime.TotalSeconds}, Trigger: {_controller.specialTankRoundTriggered}, GameState: {_world._currentState})");
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _mapGenerator.Draw(spriteBatch);
            _player.animation.Draw(spriteBatch);
            EntityController.Draw(spriteBatch);

            spriteBatch.DrawString(_world.GeneralFont, $"Score: {_score.Score}", new Vector2(2300, 1000), Color.White);
            // Draw the enemies (TankEnemies in this case)
            foreach (var enemy in Enemy.enemies)
            {
                enemy.Draw(spriteBatch);
            }

            // Optionally display a message indicating the special round
            spriteBatch.DrawString(_world.GeneralFont, "Special Round!", new Vector2(2300, 1400), Color.Yellow);
        }

        public void StartSpecialRound()
        {
            Enemy.enemies.Clear();
            
            foreach (var spawnPosition in tankEnemySpawnPositions)
            {
                var specialTank = new TankEnemy(spawnPosition, _world.TankEnemy, _mapGenerator, _world)
                {
                    IsStationary = true,
                    CanShootBackAtPlayer = true
                };
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
    }
}

