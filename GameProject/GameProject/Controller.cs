using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameProject.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace GameProject
{
    public class Controller
    {
        public static double timer = 2D;
        public static double maxTime = 2D;
        static Random rnd = new Random();
        public static bool inGame = false;
        private MapGenerator gameMap;

        public Controller(MapGenerator map)
        {
            gameMap = map;
        }

        public void Update(GameTime gametime, Texture2D enemySprite)
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

            // If timer hits zero, spawn an enemy and reset the timer
            if (timer <= 0)
            {
                SpawnEnemy(enemySprite);
                timer = maxTime;

                // Gradually reduce spawn interval for increased difficulty
                if (maxTime > 0.5)
                {
                    maxTime -= 0.05D;
                }
            }
        }

        // Method to handle enemy spawning at random positions
        private void SpawnEnemy(Texture2D sprite)
        {
            int side = rnd.Next(5);  // Random side (0-4)

            // Define random spawn positions based on the side
            Vector2 spawnPosition = GetRandomSpawnPosition(side);

            // Create and add the enemy to the enemies list
            Enemy.enemies.Add(new Enemy(spawnPosition, sprite, gameMap));

            Console.WriteLine($"Enemy spawned. Total enemies: {Enemy.enemies.Count}");
        }

        // Get a spawn position based on the selected side
        private Vector2 GetRandomSpawnPosition(int side)
        {
            switch (side)
            {
                case 0:
                    return new Vector2(44 * 32, 6 * 32);  // Example spawn point on side 0
                case 1:
                    return new Vector2(30 * 32, 6 * 32);  // Example spawn point on side 1
                case 2:
                    return new Vector2(38 * 32, 44 * 32); // Example spawn point on side 2
                case 3:
                    return new Vector2(5 * 32, 6 * 32);   // Example spawn point on side 3
                case 4:
                    return new Vector2(11 * 32, 44 * 32);  // Example spawn point on side 4
                default:
                    return Vector2.Zero;  // Default to origin if no match
            }
        }



    }
}
