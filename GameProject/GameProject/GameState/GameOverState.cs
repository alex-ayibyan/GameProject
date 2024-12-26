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
    public class GameOverState : IGameState
    {
        private bool _hasReset = false;

        private GameWorld _gameWorld;
        private Controller _controller;

        private KeyboardState _previousKeyboardState;

        private SpriteFont _titleFont;
        private SpriteFont _menuFont;

        private int _selectedButtonIndex;

        private Rectangle _restartButtonRectangle;
        private Rectangle _newDifficultyButtonRectangle;
        private Rectangle _mainMenuButtonRectangle;
        private Rectangle _quitButtonRectangle;

        private string[] _buttonTexts = { "Restart", "Choose New Difficulty", "Back to main menu", "Quit" };
        private Rectangle[] _buttonRectangles;


        public GameOverState(GameWorld gameWorld, SpriteFont titleFont, SpriteFont menuFont, Controller controller, Camera camera)
        {
            _gameWorld = gameWorld;
            _controller = controller;
            _titleFont = titleFont;
            _menuFont = menuFont;

            Vector2 restartTextSize = _menuFont.MeasureString("Restart");
            _restartButtonRectangle = new Rectangle(600, 300, (int)restartTextSize.X, (int)restartTextSize.Y);

            Vector2 newDifficultyTextSize = _menuFont.MeasureString("Choose New Difficulty");
            _newDifficultyButtonRectangle = new Rectangle(600, 400, (int)newDifficultyTextSize.X, (int)newDifficultyTextSize.Y);

            Vector2 mainMenuTextSize = _menuFont.MeasureString("Back to main menu");
            _mainMenuButtonRectangle = new Rectangle(600, 500, (int)mainMenuTextSize.X, (int)mainMenuTextSize.Y);

            Vector2 quitTextSize = _menuFont.MeasureString("Quit");
            _quitButtonRectangle = new Rectangle(600, 600, (int)quitTextSize.X, (int)quitTextSize.Y);


            _buttonRectangles = new Rectangle[] { _restartButtonRectangle, _newDifficultyButtonRectangle, _mainMenuButtonRectangle, _quitButtonRectangle};
            _selectedButtonIndex = 0;
        }

        public void Update(GameTime gameTime)
        {

            _gameWorld.Reset();


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

            if (keyboardState.IsKeyDown(Keys.Enter) && !_previousKeyboardState.IsKeyDown(Keys.Enter))
            {
                if (_selectedButtonIndex == 0)
                {
                    _controller.SetDifficulty();
                    _gameWorld.ChangeState(GameStates.Playing);
                }
                else if (_selectedButtonIndex == 1)
                {
                    _gameWorld.ChangeState(GameStates.ChooseDifficulty);
                }
                else if (_selectedButtonIndex == 2)
                {
                    _gameWorld.ChangeState(GameStates.StartScreen);
                }
                else if (_selectedButtonIndex == 3)
                {
                    Environment.Exit(0);
                }
            }

            _previousKeyboardState = keyboardState;
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
    }
}
