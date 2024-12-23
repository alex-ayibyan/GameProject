using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        public static void Update(GameTime gametime, Texture2D sprite)
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
                int side = rnd.Next(5);

                switch (side)
                {
                    case 0:
                        Vector2 spawnPosition1 = new Vector2(44 * 32, 6 * 32);
                        Enemy.enemies.Add(new Enemy(spawnPosition1, sprite));
                        break;
                    case 1:
                        Vector2 spawnPosition2 = new Vector2(30 * 32, 6 * 32);
                        Enemy.enemies.Add(new Enemy(spawnPosition2, sprite));
                        break;
                    case 2:
                        Vector2 spawnPosition3 = new Vector2(38 * 32, 44 * 32);
                        Enemy.enemies.Add(new Enemy(spawnPosition3, sprite));
                        break;
                    case 3:
                        Vector2 spawnPosition4 = new Vector2(5 * 32, 6 * 32);
                        Enemy.enemies.Add(new Enemy(spawnPosition4, sprite));
                        break;
                    case 4:
                        Vector2 spawnPosition5 = new Vector2(11 * 32, 44 * 32);
                        Enemy.enemies.Add(new Enemy(spawnPosition5, sprite));
                        break;
                }
                Console.WriteLine($"Enemy spawned. Total enemies: {Enemy.enemies.Count}");
                timer = maxTime;

                if (maxTime > 0.5)
                {
                    maxTime -= 0.05D;
                }
            }
        }

        
    }
}
