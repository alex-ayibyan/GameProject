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
    public class GameOverState : BaseMenuState
    {
        private bool _hasReset = false;

        public GameOverState(GameWorld gameWorld, SpriteFont titleFont, SpriteFont menuFont, Controller controller, Camera camera)
            : base(gameWorld, titleFont, menuFont, controller, camera, new string[] { "Restart", "Back to main menu", "Quit" },
                new Rectangle[] {
                new Rectangle(600, 300, (int)menuFont.MeasureString("Restart").X, (int)menuFont.MeasureString("Restart").Y),
                new Rectangle(600, 400, (int)menuFont.MeasureString("Back to main menu").X, (int)menuFont.MeasureString("Back to main menu").Y),
                new Rectangle(600, 500, (int)menuFont.MeasureString("Quit").X, (int)menuFont.MeasureString("Quit").Y)
                })
        { }

        public override void Update(GameTime gameTime)
        {
            _camera.Position = new Vector2(1000, 400);

            if (!_hasReset)
            {
                _gameWorld.Reset();
                _hasReset = true;
            }

            base.Update(gameTime);
        }

        protected override void HandleButtonSelection()
        {
            switch (_selectedButtonIndex)
            {
                case 0:
                    _controller.SetDifficulty();
                    _gameWorld.ChangeState(GameStates.Playing);
                    break;
                case 1:
                    _gameWorld.ChangeState(GameStates.StartScreen);
                    break;
                case 2:
                    Environment.Exit(0);
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Vector2 titlePosition = new Vector2(600, 100);
            spriteBatch.DrawString(_titleFont, "Game Over", titlePosition, Color.White);
        }

        public override Song GetBackgroundMusic()
        {
            return _gameWorld.GameOverMusic;
        }
    }
}
