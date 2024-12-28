using Comora;
using GameProject.Controllers;
using GameProject.Entities;
using GameProject.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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

            _camera.Position = new Vector2(_player.Position.X, _player.Position.Y);
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

        public Song GetBackgroundMusic()
        {
            return _world.playMusic;
        }
    }
}
