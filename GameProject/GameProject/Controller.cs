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
                     
            if (timer <= 0)
            {
                SpawnEnemy(enemySprite);
                timer = maxTime;

                if (maxTime > 0.5)
                {
                    maxTime -= 0.05D;
                }
            }
        }

        private void SpawnEnemy(Texture2D sprite)
        {
            int side = rnd.Next(5);

            Vector2 spawnPosition = GetRandomSpawnPosition(side);

            Enemy.enemies.Add(new Enemy(spawnPosition, sprite, gameMap));

            Console.WriteLine($"Enemy spawned. Total enemies: {Enemy.enemies.Count}");
        }

        private Vector2 GetRandomSpawnPosition(int side)
        {
            switch (side)
            {
                case 0:
                    return new Vector2(54 * 32, 30 * 32); //Spawn Top/Mid
                case 1:
                    return new Vector2(30 * 32, 30 * 32); // Spawn Top/Left
                case 2:
                    return new Vector2(69 * 32, 30 * 32); // Spawn Top/Right
                case 3:
                    return new Vector2(36 * 32, 69 * 32);   // Spawn Bottom/Left
                case 4:
                    return new Vector2(68 * 32, 69 * 32);  // Spawn Bottom/Right
                default:
                    return Vector2.Zero;
            }
        }



    }
}
