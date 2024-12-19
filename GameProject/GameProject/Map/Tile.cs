using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProject.Map
{
    public class Tile
    {
       
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public bool IsCollidable { get; set; }

        public Tile(Texture2D texture, Vector2 position, bool isCollidable)
        {
            Texture = texture;
            Position = position;
            IsCollidable = isCollidable;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }

}
