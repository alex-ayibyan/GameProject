using Comora;
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
    public class StartScreenState : IGameState
    {
        private GameWorld _gameWorld;
        private SpriteFont _menuFont;
        private Camera _camera;

        private Rectangle _startButtonRectangle;
        private Texture2D _buttonTexture;

        private MouseState _previousMouseState;
        private Rectangle _clickableArea = new Rectangle(700, 540, 290, 140);

        public StartScreenState(GameWorld gameWorld, SpriteFont menuFont, Camera camera, Texture2D buttonTexture)
        {
            _gameWorld = gameWorld;
            _menuFont = menuFont;
            _camera = camera;

            _buttonTexture = buttonTexture;

            _startButtonRectangle = new Rectangle(300, 300, 293, 143);

        }

        public void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            // Debug.WriteLine($"Mouse Position: {mouseState.Position}, Left Button: {mouseState.LeftButton}");

            if (_clickableArea.Contains(mouseState.Position) && mouseState.LeftButton == ButtonState.Pressed)
            {
                Console.WriteLine("Button Clicked!");
                _gameWorld.ChangeState(GameStates.Playing);
            }

            _previousMouseState = mouseState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_menuFont, "Welcome to the Game!", new Vector2(300, 200), Color.White);

            spriteBatch.Draw(_buttonTexture, _startButtonRectangle, Color.White);
            Texture2D debugTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            debugTexture.SetData(new[] { Color.Red });

            spriteBatch.Draw(debugTexture, _startButtonRectangle, Color.Red * 0.5f);
        }

        
    }
}
