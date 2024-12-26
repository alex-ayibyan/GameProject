using Comora;
using GameProject.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.GameState
{
    public class PlayingState : IGameState
    {
        private GameWorld _world;
        private Player _player;
        private ScoreController _score;
        private Camera _camera;

        private Texture2D _regularEnemyTexture;
        private Texture2D _fastEnemyTexture;
        private Texture2D _tankEnemyTexture;

        private MapGenerator _mapGenerator;
        private Controller _controller;


        private bool debugMode = false;
        private float _debugTimer = 0f; 
        private float _debugInterval = 4f;

        public PlayingState(GameWorld world, Player player, ScoreController score, Camera camera, Texture2D regularEnemyTexture, Texture2D fastEnemyTexture, Texture2D tankEnemyTexture, Controller controller)
        {
            _world = world;
            _player = player;
            _score = score;
            _camera = camera;
            _regularEnemyTexture = regularEnemyTexture;
            _fastEnemyTexture = fastEnemyTexture;
            _tankEnemyTexture = tankEnemyTexture;
            _mapGenerator = _world.GameMap;
            _controller = controller;


        }

        public void Update(GameTime gameTime)
        {
            _player.Update(gameTime);
            _controller.SetDifficulty();

            _debugTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_debugTimer >= _debugInterval)
            {
                Debug.WriteLine($"Timer: {gameTime.ElapsedGameTime.TotalSeconds}, Trigger: {_controller.specialTankRoundTriggered}, GameState: {_world._currentState}");
                _debugTimer = 0f;
            }

            _camera.Position = new Vector2(_player.Position.X, _player.Position.Y);
            //_camera.Position = new Vector2(1500, 1500);
            _camera.Update(gameTime);

            _controller.Update(gameTime, _regularEnemyTexture, _fastEnemyTexture, _tankEnemyTexture);
            EntityController.Update(gameTime, _player, _player.Position, _player.dead, _score);

            if (_player.dead)
            {
                if (_controller.specialTankRoundTriggered)
                {
                    CancelSpecialRound();
                }
                _world.ChangeState(GameStates.GameOver);
            }
        }

        private void CancelSpecialRound()
        {
            Enemy.enemies.RemoveAll(e => e is TankEnemy);
            _controller.specialTankRoundTriggered = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _mapGenerator.Draw(spriteBatch);
            _player.animation.Draw(spriteBatch);
           

            EntityController.Draw(spriteBatch);

            spriteBatch.DrawString(_world.GeneralFont, $"Score: {_score.Score}", new Vector2(2300, 1000), Color.White);

            spriteBatch.DrawString(_world.GeneralFont, $"Difficulty: {_controller.difficultyLevel}", new Vector2(2300, 1100), Color.White);

            if (debugMode)
            {
                DrawDebugRectangles(spriteBatch);
            }
        }

        private void DrawDebugRectangles(SpriteBatch spriteBatch)
        {
            // Draw a rectangle around the player
            Rectangle playerRectangle = new Rectangle((int)_player.Position.X, (int)_player.Position.Y, 32, 32);
            spriteBatch.Draw(_mapGenerator.GetRectangleTexture(), playerRectangle, Color.Red);  // Red for player

            foreach (var enemy in Enemy.enemies)
            {
                Rectangle enemyRectangle = new Rectangle((int)enemy.Position.X, (int)enemy.Position.Y, 32, 32);
                spriteBatch.Draw(_mapGenerator.GetRectangleTexture(), enemyRectangle, Color.Green);  // Green for enemies
            }

            foreach (var bullet in Bullet.bullets)
            {
                Rectangle bulletRectangle = new Rectangle((int)bullet.Position.X, (int)bullet.Position.Y, 32, 32);
                spriteBatch.Draw(_mapGenerator.GetRectangleTexture(), bulletRectangle, Color.Orange);  // Orange for bullets
            }
        }
    }
}
