using GameProject.Controllers;
using GameProject.GameState;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comora;

namespace GameProject.States.BaseStates
{
    public abstract class BaseMenuState : IGameState
    {
        protected GameWorld _gameWorld;
        protected SpriteFont _titleFont;
        protected SpriteFont _menuFont;
        protected Controller _controller;
        protected int _selectedButtonIndex;
        protected Rectangle[] _buttonRectangles;
        protected string[] _buttonTexts;
        protected Camera _camera;

        private KeyboardState _previousKeyboardState;

        public BaseMenuState(GameWorld gameWorld, SpriteFont titleFont, SpriteFont menuFont, Controller controller, Camera camera, string[] buttonTexts, Rectangle[] buttonRectangles)
        {
            _gameWorld = gameWorld;
            _titleFont = titleFont;
            _menuFont = menuFont;
            _controller = controller;
            _buttonTexts = buttonTexts;
            _buttonRectangles = buttonRectangles;
            _selectedButtonIndex = 0;
            _camera = camera;
        }

        public virtual void Update(GameTime gameTime)
        {
            
            if (_gameWorld.ElapsedTimeSinceStateChange < _gameWorld.StateChangeDelay)
            {
                return;
            }

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

            if (keyboardState.IsKeyDown(Keys.Space) && !_previousKeyboardState.IsKeyDown(Keys.Space))
            {
                HandleButtonSelection();
            }

            _previousKeyboardState = keyboardState;
        }

        protected abstract void HandleButtonSelection();

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < _buttonRectangles.Length; i++)
            {
                Color buttonColor = (i == _selectedButtonIndex) ? Color.Yellow : Color.White;
                spriteBatch.DrawString(_menuFont, _buttonTexts[i], new Vector2(_buttonRectangles[i].X, _buttonRectangles[i].Y), buttonColor);
            }
        }

        public abstract Song GetBackgroundMusic();
    }
}
