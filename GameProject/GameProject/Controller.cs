using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
        public double maxTime = 2D;
        static Random rnd = new Random();
        public static bool inGame = false;
        private MapGenerator gameMap;
        private ScoreController scoreController;
        private GameWorld gameWorld;

        public bool specialTankRoundTriggered = false;

        private double specialRoundCooldownTimer = 0;
        private const double CooldownTime = 5;
        private bool specialRoundOnCooldown = false;

        private int lastSpecialRoundScore = 0;

        public int difficultyLevel = 1;

        private int fastSpeed;

        public Controller(GameWorld gameWorld, MapGenerator map, ScoreController score)
        {
            this.gameWorld = gameWorld;
            gameMap = map;
            scoreController = score;
        }

        public void Update(GameTime gametime, Texture2D regularEnemyTexture, Texture2D fastEnemyTexture, Texture2D tankEnemyTexture)
        {
            if (gameWorld.Player.dead)
            {
                ResetGame();
                return;
            }

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
                    SetDifficulty();
                }
            }

            if (gameWorld._currentState is PlayingState && specialTankRoundTriggered)
            {
                specialTankRoundTriggered = false;
                specialRoundOnCooldown = true;
                specialRoundCooldownTimer = 0;
                Debug.WriteLine("Resetting specialTankRoundTriggered flag in PlayingState.");
            }

            if (specialRoundOnCooldown)
            {
                specialRoundCooldownTimer += gametime.ElapsedGameTime.TotalSeconds;

                if (specialRoundCooldownTimer >= CooldownTime)
                {
                    specialRoundOnCooldown = false;
                    specialRoundCooldownTimer = 0;
                    Debug.WriteLine("Cooldown complete, ready for next special round.");
                }
            }

        

            

            if (scoreController.Score >= (lastSpecialRoundScore + 30) && !specialTankRoundTriggered && !specialRoundOnCooldown)
            {
                lastSpecialRoundScore = scoreController.Score;
                TriggerSpecialRound(tankEnemyTexture);
            }
            else if (timer <= 0 && !specialTankRoundTriggered)
            {
                SpawnEnemy(regularEnemyTexture, fastEnemyTexture);
                timer = maxTime;

                if (maxTime > 0.5)
                {
                    maxTime -= 0.05D;
                }
            }

        }

        public void SetDifficulty()
        {
                switch (difficultyLevel)
                {
                    case 1:
                        maxTime = 2D;
                        fastSpeed = 150;
                        break;

                    case 2:
                        maxTime = 1.5D;
                        fastSpeed = 170;
                        break;

                    case 3:
                        maxTime = 1.0D;
                        fastSpeed = 190;
                        break;

                    case 4:
                        maxTime = 0.5D;
                        fastSpeed = 250;
                        break;
                }
            Debug.WriteLine($"Difficulty Level: {difficultyLevel}, maxTime: {maxTime}, fastSpeed: {fastSpeed}");
        }

        private void TriggerSpecialRound(Texture2D tankEnemyTexture)
        {
            if (!specialTankRoundTriggered)
            {
                Enemy.enemies.Clear();
                specialTankRoundTriggered = true;

                gameWorld.ChangeState(GameStates.SpecialRound);

                var specialRoundState = (SpecialRoundState)gameWorld._currentState;
                specialRoundState.StartSpecialRound();

                Debug.WriteLine("Special Round triggered.");
            }
        }

        private void SpawnEnemy(Texture2D regularEnemyTexture, Texture2D fastEnemyTexture)
        {
            int side = rnd.Next(5);

            EnemyType selectedType = scoreController.Score >= 60 ? EnemyType.Fast : EnemyType.Regular;

            Vector2 spawnPosition = GetRandomSpawnPosition(side);
            Enemy newEnemy = selectedType switch
            {
                EnemyType.Fast => new FastEnemy(spawnPosition, fastEnemyTexture, gameMap, fastSpeed),
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
        private void ResetGame()
        {
            timer = 2D;        
            maxTime = 2D;     
            lastSpecialRoundScore = 0;
        }
    }
}
