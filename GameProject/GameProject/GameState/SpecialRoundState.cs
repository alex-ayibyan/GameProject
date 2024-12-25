using GameProject.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.GameState
{
    public class SpecialRoundState : IGameState
    {
        private double displayTime = 1;
        private bool transitionBackToPlaying = false;
        private GameWorld gameWorld;
        private MapGenerator gameMap;
        private ScoreController scoreController;
        private Vector2[] tankEnemySpawnPositions = new Vector2[]
        {
        new Vector2(64 * 32, 40 * 32), // Spawn Position 1 (Top/Right)
        new Vector2(64 * 32, 58 * 32), // Spawn Position 2 (Bottom/Right)
        new Vector2(33 * 32, 58 * 32), // Spawn Position 3 (Bottom/Left)
        new Vector2(33 * 32, 41 * 32)  // Spawn Position 4 (Top/Left)
        };

        public SpriteFont GeneralFont { get; private set; }

        public SpecialRoundState(GameWorld gameWorld, MapGenerator gameMap, SpriteFont generalFont)
        {
            this.gameWorld = gameWorld;
            this.gameMap = gameMap;
            GeneralFont = generalFont;
        }

        public void Update(GameTime gameTime)
        {
            displayTime -= gameTime.ElapsedGameTime.TotalSeconds;

            if (displayTime <= 0 && !transitionBackToPlaying)
            {
                RemoveTankEnemies();

                gameWorld.ChangeState(GameStates.Playing);
                transitionBackToPlaying = true;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {

        }

        public void StartSpecialRound()
        {
            Enemy.enemies.Clear();
            
            foreach (var spawnPosition in tankEnemySpawnPositions)
            {
                var specialTank = new TankEnemy(spawnPosition, gameWorld.TankEnemy, gameMap, gameWorld)
                {
                    IsStationary = true,
                    CanShootBackAtPlayer = true
                };
                Enemy.enemies.Add(specialTank);
            }
        }

        public void RemoveTankEnemies()
        {
            Enemy.enemies.RemoveAll(e => e is TankEnemy && e.Dead);
        }


    }
}

