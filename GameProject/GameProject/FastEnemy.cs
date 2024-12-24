using GameProject.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject
{
    public class FastEnemy : Enemy
    {
        
        public FastEnemy(Vector2 newPosition, Texture2D sprite, MapGenerator gameMap) : base(newPosition, sprite, gameMap)
        {
            Speed = 200;
            Radius = 20;
            animation = new SpriteAnimation(sprite, 4, 20);

            animation.Scale = 1.5f;
        }

        
    }
}
