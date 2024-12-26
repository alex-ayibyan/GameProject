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
    class ChooseDifficultyState : IGameState
    {
        private GameWorld _gameWorld;
        private SpriteFont _menuFont;
        private Controller _controller;
        public int _selectedDifficultyIndex;
        private Rectangle _oneButtonRectangle;
        private Rectangle _twoButtonRectangle;
        private Rectangle _threeButtonRectangle;
        private Rectangle _fourButtonRectangle;
        private Rectangle _backButtonRectangle;
        private KeyboardState _previousKeyboardState;

        private string[] _difficultyTexts = { "1 (Easy)", "2 (Normal)", "3 (Hard)", "4 (Very Hard)", "Back" };
        private Rectangle[] _difficultyButtonRectangles;

        public ChooseDifficultyState(GameWorld gameWorld, SpriteFont menuFont, Controller controller)
        {
            _gameWorld = gameWorld;
            _menuFont = menuFont;
            _controller = controller;

            Vector2 oneTextSize = _menuFont.MeasureString("1 (Easy)");
            _oneButtonRectangle = new Rectangle(450, 200, (int)oneTextSize.X, (int)oneTextSize.Y);

            Vector2 twoTextSize = _menuFont.MeasureString("2 (Normal)");
            _twoButtonRectangle = new Rectangle(450, 250, (int)twoTextSize.X, (int)twoTextSize.Y);

            Vector2 threeTextSize = _menuFont.MeasureString("3 (Hard)");
            _threeButtonRectangle = new Rectangle(450, 300, (int)threeTextSize.X, (int)threeTextSize.Y);

            Vector2 fourTextSize = _menuFont.MeasureString("4 (Very Hard)");
            _fourButtonRectangle = new Rectangle(450, 350, (int)fourTextSize.X, (int)fourTextSize.Y);

            Vector2 backTextSize = _menuFont.MeasureString("Back");
            _backButtonRectangle = new Rectangle(450, 400, (int)backTextSize.X, (int)backTextSize.Y);

            _difficultyButtonRectangles = new Rectangle[] { _oneButtonRectangle, _twoButtonRectangle, _threeButtonRectangle, _fourButtonRectangle, _backButtonRectangle };

            _selectedDifficultyIndex = 0;
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Down) && !_previousKeyboardState.IsKeyDown(Keys.Down))
            {
                if (_selectedDifficultyIndex < _difficultyButtonRectangles.Length - 1)
                {
                    _selectedDifficultyIndex++;
                }
            }

            if (keyboardState.IsKeyDown(Keys.Up) && !_previousKeyboardState.IsKeyDown(Keys.Up))
            {
                if (_selectedDifficultyIndex > 0)
                {
                    _selectedDifficultyIndex--;
                }
            }

            if (keyboardState.IsKeyDown(Keys.Enter) && !_previousKeyboardState.IsKeyDown(Keys.Enter))
            {
                if (_selectedDifficultyIndex == 4)
                {
                    _gameWorld.ChangeState(GameStates.StartScreen);
                }
                else
                {
                    _controller.difficultyLevel = _selectedDifficultyIndex + 1;
                    Debug.WriteLine($"Difficulty selected: {_controller.difficultyLevel}");
                    _controller.SetDifficulty();
                    _gameWorld.GoBackToPreviousState();
                }
            }

            _previousKeyboardState = keyboardState;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _difficultyButtonRectangles.Length; i++)
            {
                Color buttonColor = (i == _selectedDifficultyIndex) ? Color.Yellow : Color.White;
                spriteBatch.DrawString(_menuFont, _difficultyTexts[i], new Vector2(_difficultyButtonRectangles[i].X, _difficultyButtonRectangles[i].Y), buttonColor);
            }
        }
    }
}
