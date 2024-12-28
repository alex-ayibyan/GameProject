using Comora;
using GameProject.Controllers;
using GameProject.Entities;
using GameProject.Map;
using GameProject.States.BaseStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace GameProject.States
{
    public class PlayingState : BaseGameState
    {
        private readonly Texture2D _regularEnemyTexture;
        private readonly Texture2D _fastEnemyTexture;
        private readonly Texture2D _tankEnemyTexture;
        private readonly Controller _controller;

        public PlayingState(GameWorld world, Player player, ScoreController score, Camera camera, Texture2D regularEnemyTexture, Texture2D fastEnemyTexture, Texture2D tankEnemyTexture, Controller controller)
            : base(world, player, score, camera)
        {
            _regularEnemyTexture = regularEnemyTexture;
            _fastEnemyTexture = fastEnemyTexture;
            _tankEnemyTexture = tankEnemyTexture;
            _controller = controller;
        }

        public override void Update(GameTime gameTime)
        {
            UpdateCommon(gameTime);
            _controller.SetDifficulty();
            _controller.Update(gameTime, _regularEnemyTexture, _fastEnemyTexture, _tankEnemyTexture);

            if (_player.dead)
            {
                if (_controller.SpecialTankRoundTriggered)
                {
                    CancelSpecialRound();
                }
                _world.ChangeState(GameStates.GameOver);
            }
        }

        private void CancelSpecialRound()
        {
            Enemy.Enemies.RemoveAll(e => e is TankEnemy);
            _controller.SpecialTankRoundTriggered = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawCommon(spriteBatch);
        }

        public override Song GetBackgroundMusic()
        {
            return _world.PlayMusic;
        }
    }
}
