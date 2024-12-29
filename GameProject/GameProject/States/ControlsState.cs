using GameProject.Controllers;
using GameProject.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.GameState
{
    class ControlsState : IGameState
    {
        private readonly GameWorld _gameWorld;
        private readonly SpriteFont _menuFont;
        private readonly SpriteFont _titleFont;

        public ControlsState(GameWorld gameWorld, SpriteFont menuFont, SpriteFont titleFont)
        {
            _gameWorld = gameWorld;
            _menuFont = menuFont;
            _titleFont = titleFont;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Space))
            {
                _gameWorld.ChangeState(GameStates.StartScreen);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        { 
            spriteBatch.DrawString(_titleFont, "Game Controls:", new Vector2(400, 100), Color.White);
            spriteBatch.DrawString(_menuFont, "Move Up: Arrow Up", new Vector2(400, 250), Color.White);
            spriteBatch.DrawString(_menuFont, "Move Down: Arrow Down", new Vector2(400, 320), Color.White);
            spriteBatch.DrawString(_menuFont, "Move Left: Arrow Left", new Vector2(400, 390), Color.White);
            spriteBatch.DrawString(_menuFont, "Move Right: Arrow Right", new Vector2(400, 460), Color.White);
            spriteBatch.DrawString(_menuFont, "Shoot: Spacebar", new Vector2(400, 530), Color.White);
            spriteBatch.DrawString(_menuFont, "Sprint: LeftShift", new Vector2(400, 600), Color.White);
            spriteBatch.DrawString(_menuFont, "Quit: Escape", new Vector2(400, 670), Color.White);
            spriteBatch.DrawString(_menuFont, "Press space to go back", new Vector2(400, 800), Color.White);
        }

        public Song GetBackgroundMusic()
        {
            MediaPlayer.IsRepeating = true;
            return _gameWorld.StartMusic;
        }
    }
}
