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
        private double displayTime = 2; // Display for 2 seconds
        private bool transitionBackToPlaying = false; // Flag to indicate if we should transition back to playing
        private GameWorld gameWorld; // Reference to the GameWorld to change states
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
                transitionBackToPlaying = true; // Ensure we only do this once

            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (displayTime > 0)
            {
                // Display the "Special Round" message
                spriteBatch.DrawString(GeneralFont, "Special Round", new Vector2(1000, 1000), Color.Red);
            }
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
            // Remove tank enemies once all are dead
            Enemy.enemies.RemoveAll(e => e is TankEnemy && e.Dead);
        }
    }
}

