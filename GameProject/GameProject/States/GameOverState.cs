using Comora;
using GameProject.Controllers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        private bool _hasReset = false;

        private GameWorld _gameWorld;
        private Controller _controller;
        private Camera _camera;

        private KeyboardState _previousKeyboardState;
        private KeyboardState _currentKeyboardState;

        private SpriteFont _titleFont;
        private SpriteFont _menuFont;

        private int _selectedButtonIndex;

        private Rectangle _restartButtonRectangle;
        private Rectangle _mainMenuButtonRectangle;
        private Rectangle _quitButtonRectangle;

        private string[] _buttonTexts = { "Restart", "Back to main menu", "Quit" };
        private Rectangle[] _buttonRectangles;


        public GameOverState(GameWorld gameWorld, SpriteFont titleFont, SpriteFont menuFont, Controller controller, Camera camera)
        {
            _gameWorld = gameWorld;
            _controller = controller;
            _titleFont = titleFont;
            _menuFont = menuFont;
            _camera = camera;

            Vector2 restartTextSize = _menuFont.MeasureString("Restart");
            _restartButtonRectangle = new Rectangle(600, 300, (int)restartTextSize.X, (int)restartTextSize.Y);

            Vector2 mainMenuTextSize = _menuFont.MeasureString("Back to main menu");
            _mainMenuButtonRectangle = new Rectangle(600, 400, (int)mainMenuTextSize.X, (int)mainMenuTextSize.Y);

            Vector2 quitTextSize = _menuFont.MeasureString("Quit");
            _quitButtonRectangle = new Rectangle(600, 500, (int)quitTextSize.X, (int)quitTextSize.Y);


            _buttonRectangles = new Rectangle[] { _restartButtonRectangle, _mainMenuButtonRectangle, _quitButtonRectangle};
            _selectedButtonIndex = 0;
        }

        public void Update(GameTime gameTime)
        {
            _camera.Position = new Vector2(1000, 400);
            
            if (!_hasReset)
            {
                _gameWorld.Reset();
                _hasReset = true;
            }


            _currentKeyboardState = Keyboard.GetState();

            if (_currentKeyboardState.IsKeyDown(Keys.Down) && !_previousKeyboardState.IsKeyDown(Keys.Down))
            {
                if (_selectedButtonIndex < _buttonRectangles.Length - 1)
                {
                    _selectedButtonIndex++;
                }
            }

            if (_currentKeyboardState.IsKeyDown(Keys.Up) && !_previousKeyboardState.IsKeyDown(Keys.Up))
            {
                if (_selectedButtonIndex > 0)
                {
                    _selectedButtonIndex--;
                }
            }

            if (_currentKeyboardState.IsKeyDown(Keys.Space) && !_previousKeyboardState.IsKeyDown(Keys.Space))
            {
                if (_selectedButtonIndex == 0)
                {
                    _controller.SetDifficulty();
                    _gameWorld.ChangeState(GameStates.Playing);
                }
                else if (_selectedButtonIndex == 1)
                {
                    _gameWorld.ChangeState(GameStates.StartScreen);
                }
                else if (_selectedButtonIndex == 2)
                {
                    Environment.Exit(0);
                }
            }

            _previousKeyboardState = _currentKeyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 titlePosition = new Vector2(600, 100);
            spriteBatch.DrawString(_titleFont, "Game Over", titlePosition, Color.White);

            for (int i = 0; i < _buttonRectangles.Length; i++)
            {
                Color buttonColor = (i == _selectedButtonIndex) ? Color.Yellow : Color.White;
                spriteBatch.DrawString(_menuFont, _buttonTexts[i], new Vector2(_buttonRectangles[i].X, _buttonRectangles[i].Y), buttonColor);
            }
        }

        public Song GetBackgroundMusic()
        {
            return _gameWorld.gameOverMusic;
        }
    }
}
