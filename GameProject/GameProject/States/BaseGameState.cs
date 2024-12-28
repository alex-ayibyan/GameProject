using Comora;
using GameProject.Controllers;
using GameProject.Entities;
using GameProject.GameState;
using GameProject.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.States
{
    public abstract class BaseGameState : IGameState
    {
        protected GameWorld _world;
        protected Player _player;
        protected ScoreController _score;
        protected Camera _camera;
        protected MapGenerator _mapGenerator;

        protected BaseGameState(GameWorld world, Player player, ScoreController score, Camera camera)
        {
            _world = world;
            _player = player;
            _score = score;
            _camera = camera;
            _mapGenerator = _world.GameMap;
        }

        public virtual void UpdateCommon(GameTime gameTime)
        {
            _player.Update(gameTime);
            _camera.Position = new Vector2(_player.Position.X, _player.Position.Y);
            _camera.Update(gameTime);
            EntityController.Update(gameTime, _player, _player.Position, _player.dead, _score);
        }

        public virtual void DrawCommon(SpriteBatch spriteBatch)
        {
            _mapGenerator.Draw(spriteBatch);
            _player.animation.Draw(spriteBatch, _player.isInvincible
                ? ((int)(_player.invincibilityTimer * 5) % 2 == 0 ? Color.White * 0.5f : Color.White)
                : Color.White);

            spriteBatch.DrawString(_world.GeneralFont, $"Score: {_score.Score}", new Vector2(2300, 1000), Color.White);
            spriteBatch.DrawString(_world.GeneralFont, $"Difficulty: {_world._controller.difficultyLevel}", new Vector2(2300, 1100), Color.White);

            for (int i = 0; i < _player.lives; i++)
            {
                spriteBatch.Draw(_world.lifeTexture, new Vector2(2200 + i * 120, 1200), _player.isInvincible
                    ? ((int)(_player.invincibilityTimer * 4) % 2 == 0 ? Color.White * 0.5f : Color.White)
                    : Color.White);
            }
            EntityController.Draw(spriteBatch);
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract Song GetBackgroundMusic();
    }
}

