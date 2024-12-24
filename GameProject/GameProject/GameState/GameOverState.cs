using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.GameState
{
    public class GameOverState : IGameState
    {
        private GameWorld _world;

        public GameOverState(GameWorld world)
        {
            _world = world;
        }

        public void Update(GameTime gameTime)
        {

            _world.Reset();
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                _world.ChangeState(GameStates.Playing);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 textSize = _world.GeneralFont.MeasureString("Game Over! Press Space to Restart");

            Vector2 textPosition = new Vector2(1600 - textSize.X / 2, 1500 - textSize.Y / 2);

            spriteBatch.DrawString(_world.GeneralFont, "Game Over! Press Space to Restart", textPosition, Color.White);
        }
    }
}
