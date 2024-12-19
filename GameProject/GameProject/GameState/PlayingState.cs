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
        private Texture2D _enemyTexture;
        private MapGenerator _mapGenerator;
        private List<Tile> tiles;
        


        public PlayingState(GameWorld world, Player player, ScoreController score, Camera camera, Texture2D enemyTexture)
        {
            _world = world;
            _player = player;
            _score = score;
            _camera = camera;
            _enemyTexture = enemyTexture;
            _bg = bg;
        }

        public void Update(GameTime gameTime)
        {
            _player.Update(gameTime);

            _camera.Position = new Vector2(_player.Position.X, _player.Position.Y);
            _camera.Update(gameTime);

            Controller.Update(gameTime, _enemyTexture);
            EntityController.Update(gameTime, _player, _player.Position, _player.dead, _score);

            if (_player.dead)
            {
                _world.ChangeState(GameStates.GameOver);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _player.animation.Draw(spriteBatch);

            EntityController.Draw(spriteBatch);

            spriteBatch.DrawString(_score.Font, $"Score: {_score.Score}", new Vector2(1150, 100), Color.White);
        }
    }
}
