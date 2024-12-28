using Comora;
using GameProject.Controllers;
using GameProject.Map;
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
    public class StartScreenState : BaseMenuState
    {
        public StartScreenState(GameWorld gameWorld, SpriteFont titleFont, SpriteFont menuFont, Controller controller, Camera camera)
        : base(gameWorld, titleFont, menuFont, controller, camera, new string[] { "Start", "Choose Difficulty", "Controls", "Quit" },
            new Rectangle[] {
                new Rectangle(600, 300, (int)menuFont.MeasureString("Start").X, (int)menuFont.MeasureString("Start").Y),
                new Rectangle(600, 400, (int)menuFont.MeasureString("Choose Difficulty").X, (int)menuFont.MeasureString("Choose Difficulty").Y),
                new Rectangle(600, 500, (int)menuFont.MeasureString("Controls").X, (int)menuFont.MeasureString("Controls").Y),
                new Rectangle(600, 600, (int)menuFont.MeasureString("Quit").X, (int)menuFont.MeasureString("Quit").Y)
            })
        { }

        protected override void HandleButtonSelection()
        {
            switch (_selectedButtonIndex)
            {
                case 0:
                    _controller.SetDifficulty();
                    _gameWorld.ChangeState(GameStates.Playing);
                    break;
                case 1:
                    _gameWorld.ChangeState(GameStates.ChooseDifficulty);
                    break;
                case 2:
                    _gameWorld.ChangeState(GameStates.ControlsState);
                    break;
                case 3:
                    Environment.Exit(0);
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            Vector2 titlePosition = new Vector2(600, 100);
            spriteBatch.DrawString(_titleFont, "Welcome To My Game", titlePosition, Color.White);
        }

        public override Song GetBackgroundMusic()
        {
            return _gameWorld.StartMusic;
        }
    }
}
