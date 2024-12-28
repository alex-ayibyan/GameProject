using Comora;
using GameProject.Controllers;
using GameProject.States;
using GameProject.States.BaseStates;
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
    public class ChooseDifficultyState : BaseMenuState
    {
        private Rectangle _backButtonRectangle;

        public ChooseDifficultyState(GameWorld gameWorld, SpriteFont titleFont, SpriteFont menuFont, Controller controller, Camera camera)
            : base(gameWorld, titleFont, menuFont, controller, camera, new string[] { "1 (Easy)", "2 (Normal)", "3 (Hard)", "4 (Very Hard)", "Back" },
                new Rectangle[] {
                new Rectangle(600, 300, (int)menuFont.MeasureString("1 (Easy)").X, (int)menuFont.MeasureString("1 (Easy)").Y),
                new Rectangle(600, 350, (int)menuFont.MeasureString("2 (Normal)").X, (int)menuFont.MeasureString("2 (Normal)").Y),
                new Rectangle(600, 400, (int)menuFont.MeasureString("3 (Hard)").X, (int)menuFont.MeasureString("3 (Hard)").Y),
                new Rectangle(600, 450, (int)menuFont.MeasureString("4 (Very Hard)").X, (int)menuFont.MeasureString("4 (Very Hard)").Y),
                new Rectangle(600, 500, (int)menuFont.MeasureString("Back").X, (int)menuFont.MeasureString("Back").Y)
                })
        {
            _backButtonRectangle = _buttonRectangles[4];
        }

        protected override void HandleButtonSelection()
        {
            if (_selectedButtonIndex == 4)
            {
                _gameWorld.ChangeState(GameStates.StartScreen);
            }
            else
            {
                _controller.DifficultyLevel = _selectedButtonIndex + 1;
                Debug.WriteLine($"Difficulty selected: {_controller.DifficultyLevel}");
                _controller.SetDifficulty();
                _gameWorld.GoBackToPreviousState();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Vector2 titlePosition = new Vector2(600, 100);
            spriteBatch.DrawString(_titleFont, "Choose Difficulty", titlePosition, Color.White);
        }

        public override Song GetBackgroundMusic()
        {
            return _gameWorld.StartMusic;
        }

        
    }
}
