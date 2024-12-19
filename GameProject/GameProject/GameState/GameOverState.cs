using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
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
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                _world.Reset();
                _world.ChangeState(GameStates.Playing);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_world.MenuFont, "Game Over! Press Space to Restart", new Vector2(200, 100), Color.White);
        }
    }
}
