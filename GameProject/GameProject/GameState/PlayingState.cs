using Comora;
using GameProject.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
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



        public PlayingState(GameWorld world, Player player, ScoreController score, Camera camera, Texture2D regularEnemyTexture, Texture2D fastEnemyTexture, Texture2D tankEnemyTexture)
        {
            _world = world;
            _player = player;
            _score = score;
            _camera = camera;
            _regularEnemyTexture = regularEnemyTexture;
            _fastEnemyTexture = fastEnemyTexture;
            _tankEnemyTexture = tankEnemyTexture;
            _mapGenerator = _world.GameMap;

            _controller = new Controller(_world ,_mapGenerator, score);
        }

        public void Update(GameTime gameTime)
        {
            _player.Update(gameTime);

            _camera.Position = new Vector2(_player.Position.X, _player.Position.Y);
            //_camera.Position = new Vector2(1500, 1500);
            _camera.Update(gameTime);

            _controller.Update(gameTime, _regularEnemyTexture, _fastEnemyTexture, _tankEnemyTexture);
            EntityController.Update(gameTime, _player, _player.Position, _player.dead, _score);

            if (_player.dead)
            {
                _world.ChangeState(GameStates.GameOver);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        { 
            _mapGenerator.Draw(spriteBatch);
            _player.animation.Draw(spriteBatch);
           

            EntityController.Draw(spriteBatch);

            spriteBatch.DrawString(_score.Font, $"Score: {_score.Score}", new Vector2(1150, 100), Color.White);

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

            // Assuming enemies are part of EntityController or a similar collection
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
