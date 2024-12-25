using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.GameState;
using GameProject.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Formats.Asn1.AsnWriter;


namespace GameProject
{
    public enum EnemyType
    {
        Regular,
        Fast,
        Tank
    }
    public class Controller
    {
        public static double timer = 2D;
        public static double maxTime = 2D;
        static Random rnd = new Random();
        public static bool inGame = false;
        private MapGenerator gameMap;
        private ScoreController scoreController;
        private GameWorld gameWorld;

        public bool specialTankRoundTriggered = false;

        public Controller(GameWorld gameWorld, MapGenerator map, ScoreController score)
        {
            this.gameWorld = gameWorld;
            gameMap = map;
            scoreController = score;
        }

        public void Update(GameTime gametime, Texture2D regularEnemyTexture, Texture2D fastEnemyTexture, Texture2D tankEnemyTexture)
        {
            if (inGame)
            {
                timer -= gametime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                KeyboardState kState = Keyboard.GetState();
                if (kState.GetPressedKeyCount() > 0)
                {
                    inGame = true;
                }
            }

            if (scoreController.Score >= 30 && !specialTankRoundTriggered)
            {
                TriggerSpecialRound(tankEnemyTexture);
            }

            if (timer <= 0 && !specialTankRoundTriggered)
            {
                SpawnEnemy(regularEnemyTexture, fastEnemyTexture);
                timer = maxTime;

                if (maxTime > 0.5)
                {
                    maxTime -= 0.05D;
                }
            }
        }

        private void TriggerSpecialRound(Texture2D tankEnemyTexture)
        {
            // Trigger special round in the game world
            specialTankRoundTriggered = true;

            // Switch to SpecialRoundState
            gameWorld.ChangeState(GameStates.SpecialRound);

            // Start the special round logic (spawn tank enemies)
            var specialRoundState = (SpecialRoundState)gameWorld._currentState;
            specialRoundState.StartSpecialRound();
        }

        private void SpawnEnemy(Texture2D regularEnemyTexture, Texture2D fastEnemyTexture)
        {
            int side = rnd.Next(5);

            EnemyType selectedType = scoreController.Score >= 20 ? EnemyType.Fast : EnemyType.Regular;

            Vector2 spawnPosition = GetRandomSpawnPosition(side);
            Enemy newEnemy = selectedType switch
            {
                EnemyType.Fast => new FastEnemy(spawnPosition, fastEnemyTexture, gameMap),
                _ => new Enemy(spawnPosition, regularEnemyTexture, gameMap)
            };

            Enemy.enemies.Add(newEnemy);
        }

        private Vector2 GetRandomSpawnPosition(int side)
        {
            switch (side)
            {
                case 0:
                    return new Vector2(54 * 32, 30 * 32);
                case 1:
                    return new Vector2(30 * 32, 30 * 32);
                case 2:
                    return new Vector2(69 * 32, 30 * 32);
                case 3:
                    return new Vector2(36 * 32, 69 * 32);
                case 4:
                    return new Vector2(68 * 32, 69 * 32);
                default:
                    return Vector2.Zero;
            }
        }
    }
}
