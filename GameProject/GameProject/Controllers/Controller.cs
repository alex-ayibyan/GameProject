using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GameProject.Entities;
using GameProject.GameState;
using GameProject.Map;
using GameProject.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Formats.Asn1.AsnWriter;


namespace GameProject.Controllers
{
    
    public class Controller
    {
        public static double Timer = 2D;
        private double maxTime = 2D;
        public double ShootingSpeed;
        static readonly Random rnd = new();
        private static bool inGame = false;
        private readonly MapGenerator gameMap;
        private readonly ScoreController scoreController;
        private readonly GameWorld gameWorld;

        public bool SpecialTankRoundTriggered = false;

        private double specialRoundCooldownTimer = 0;
        private const double cooldownTime = 5;
        private bool specialRoundOnCooldown = false;

        private int lastSpecialRoundScore = 0;

        public int DifficultyLevel = 1;

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
                Timer -= gametime.ElapsedGameTime.TotalSeconds;
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

            if (gameWorld.CurrentState is PlayingState && SpecialTankRoundTriggered)
            {
                SpecialTankRoundTriggered = false;
                specialRoundOnCooldown = true;
                specialRoundCooldownTimer = 0;
                Debug.WriteLine("Resetting specialTankRoundTriggered flag in PlayingState.");
            }

            if (specialRoundOnCooldown)
            {
                specialRoundCooldownTimer += gametime.ElapsedGameTime.TotalSeconds;

                if (specialRoundCooldownTimer >= cooldownTime)
                {
                    specialRoundOnCooldown = false;
                    specialRoundCooldownTimer = 0;
                    Debug.WriteLine("Cooldown complete, ready for next special round.");
                }
            }

            if (scoreController.Score >= lastSpecialRoundScore + 30 && !SpecialTankRoundTriggered && !specialRoundOnCooldown)
            {
                lastSpecialRoundScore = scoreController.Score;
                TriggerSpecialRound(tankEnemyTexture);
            }
            else if (Timer <= 0 && !SpecialTankRoundTriggered)
            {
                SpawnEnemy(regularEnemyTexture, fastEnemyTexture);
                Timer = maxTime;

                if (maxTime > 0.5)
                {
                    maxTime -= 0.05D;
                }
            }

        }

        public void SetDifficulty()
        {
            switch (DifficultyLevel)
            {
                case 1:
                    maxTime = 2D;
                    ShootingSpeed = 1.6D;
                    fastSpeed = 150;
                    break;

                case 2:
                    maxTime = 1.5D;
                    ShootingSpeed = 1.3D;
                    fastSpeed = 170;
                    break;

                case 3:
                    maxTime = 1.0D;
                    ShootingSpeed = 1D;
                    fastSpeed = 190;
                    break;

                case 4:
                    maxTime = 0.5D;
                    ShootingSpeed = 0.8D;
                    fastSpeed = 250;
                    break;
            }
            Debug.WriteLine($"Difficulty Level: {DifficultyLevel}, maxTime: {maxTime}, fastSpeed: {fastSpeed}, shootingSpeed: {ShootingSpeed}");
        }

        private void TriggerSpecialRound(Texture2D tankEnemyTexture)
        {
            if (!SpecialTankRoundTriggered)
            {
                Enemy.Enemies.Clear();
                SpecialTankRoundTriggered = true;

                gameWorld.ChangeState(GameStates.SpecialRound);

                var specialRoundState = (SpecialRoundState)gameWorld.CurrentState;
                specialRoundState.StartSpecialRound();

                Debug.WriteLine("Special Round triggered.");
            }
        }

        private void SpawnEnemy(Texture2D regularEnemyTexture, Texture2D fastEnemyTexture)
        {
            int side = rnd.Next(5);

            EnemyType selectedType;
            if (scoreController.Score >= 30)
            {
                selectedType = rnd.Next(2) == 0 ? EnemyType.Fast : EnemyType.Regular;
            }
            else
            {
                selectedType = EnemyType.Regular;
            }

            Vector2 spawnPosition = GetRandomSpawnPosition(side);
            Enemy newEnemy = selectedType switch
            {
                EnemyType.Fast => new FastEnemy(spawnPosition, fastEnemyTexture, gameMap, fastSpeed),
                _ => new Enemy(spawnPosition, regularEnemyTexture, gameMap)
            };

            Enemy.Enemies.Add(newEnemy);
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
            Timer = 2D;
            maxTime = 2D;
            lastSpecialRoundScore = 0;
        }
    }
}
