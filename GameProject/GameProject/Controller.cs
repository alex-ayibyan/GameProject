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
                int side = rnd.Next(4);

                switch (side)
                {
                    case 0:
                        Enemy.enemies.Add(new Enemy(new Vector2(0, rnd.Next(0, 1000)), sprite));
                        break;
                    case 1:
                        Enemy.enemies.Add(new Enemy(new Vector2(0, rnd.Next(0, 1000)), sprite));
                        break;
                    case 2:
                        Enemy.enemies.Add(new Enemy(new Vector2(rnd.Next(0, 1000), 0), sprite));
                        break;
                    case 3:
                        Enemy.enemies.Add(new Enemy(new Vector2(rnd.Next(0, 1000), 1000), sprite));
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
