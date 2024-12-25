using Comora;
using GameProject.Map;
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
    public class StartScreenState : IGameState
    {
        private GameWorld _gameWorld;
        private Camera _camera;

        private KeyboardState _previousKeyboardState;

        private SpriteFont _titleFont;
        private SpriteFont _menuFont;

        private int _selectedButtonIndex;

        private Rectangle _startButtonRectangle;
        private Rectangle _quitButtonRectangle;

        private string[] _buttonTexts = { "Start", "Quit", "Settings", "Help" };
        private Rectangle _helpButtonRectangle;
        private Rectangle[] _buttonRectangles;
        private Rectangle _settingsButtonRectangle;

        private bool _musicStarted = false;

        public StartScreenState(GameWorld gameWorld, SpriteFont titleFont, SpriteFont menuFont, Camera camera)
        {
            _gameWorld = gameWorld;
            _titleFont = titleFont;
            _menuFont = menuFont;
            _camera = camera;

            Vector2 startTextSize = _menuFont.MeasureString("Start");
            _startButtonRectangle = new Rectangle(450, 300, (int)startTextSize.X, (int)startTextSize.Y);

            Vector2 quitTextSize = _menuFont.MeasureString("Quit");
            _quitButtonRectangle = new Rectangle(450, 400, (int)quitTextSize.X, (int)quitTextSize.Y);

            Vector2 settingsTextSize = _menuFont.MeasureString("Settings");
            _settingsButtonRectangle = new Rectangle(450, 500, (int)settingsTextSize.X, (int)settingsTextSize.Y);

            Vector2 helpTextSize = _menuFont.MeasureString("Help");
            _helpButtonRectangle = new Rectangle(450, 600, (int)helpTextSize.X, (int)helpTextSize.Y);


            _buttonRectangles = new Rectangle[] { _startButtonRectangle, _quitButtonRectangle, _settingsButtonRectangle, _helpButtonRectangle };
            _selectedButtonIndex = 0;
        }

        public void Update(GameTime gameTime)
        {

            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Down) && !_previousKeyboardState.IsKeyDown(Keys.Down))
            {
                if (_selectedButtonIndex < _buttonRectangles.Length - 1)
                {
                    _selectedButtonIndex++;
                }
            }

            if (keyboardState.IsKeyDown(Keys.Up) && !_previousKeyboardState.IsKeyDown(Keys.Up))
            {
                if (_selectedButtonIndex > 0)
                {
                    _selectedButtonIndex--;
                }
            }

            if (keyboardState.IsKeyDown(Keys.Enter) || keyboardState.IsKeyDown(Keys.Space))
            {
                if (_selectedButtonIndex == 0)
                {
                    _gameWorld.ChangeState(GameStates.Playing);
                }
                else if (_selectedButtonIndex == 1)
                {
                    Environment.Exit(0);
                }
                else if (_selectedButtonIndex == 2)
                {
                    Debug.WriteLine("Settings Button Pressed");
                }
                else if (_selectedButtonIndex == 3)
                {
                    Debug.WriteLine("Help Button Pressed");
                }
            }

            _previousKeyboardState = keyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 titlePosition = new Vector2(450, 100);
            spriteBatch.DrawString(_titleFont, "Welcome To My Game", titlePosition, Color.White);

            for (int i = 0; i < _buttonRectangles.Length; i++)
            {
                Color buttonColor = (i == _selectedButtonIndex) ? Color.Yellow : Color.White;
                spriteBatch.DrawString(_menuFont, _buttonTexts[i], new Vector2(_buttonRectangles[i].X, _buttonRectangles[i].Y), buttonColor);
            }
        }
    }
}
