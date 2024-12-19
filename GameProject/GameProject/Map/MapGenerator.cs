using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace GameProject.Map
{
    public class MapGenerator
    {
        private Texture2D _tilesetTexture;
        private Dictionary<Vector2, int> ground;
        private Dictionary<Vector2, int> objects;
        private Dictionary<Vector2, int> collision;

        public MapGenerator(ContentManager content)
        {
            _tilesetTexture = content.Load<Texture2D>("Textures/CosmicLilac_Tiles32x32");
        }

        public void LoadMap(string groundLayerPath, string objectsLayerPath, string collisionLayerPath)
        {
            ground = LoadLayer(groundLayerPath);
            objects = LoadLayer(objectsLayerPath);
            collision = LoadLayer(collisionLayerPath);
        }

        private Dictionary<Vector2, int> LoadLayer(string filepath)
        {
            var layer = new Dictionary<Vector2, int>();

            using (StreamReader reader = new StreamReader(filepath))
            {
                int y = 0;
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] items = line.Split(',');
                    for (int x = 0; x < items.Length; x++)
                    {
                        if (int.TryParse(items[x], out int value) && value >= 0)
                        {
                            layer[new Vector2(x, y)] = value;
                        }
                    }
                    y++;
                }
            }

            return layer;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawLayer(spriteBatch, ground, _tilesetTexture, 0);
            
            DrawLayer(spriteBatch, objects, _tilesetTexture, 1);

            DrawLayer(spriteBatch, collision, _tilesetTexture, 2);
        }

        private void DrawLayer(SpriteBatch spriteBatch, Dictionary<Vector2, int> layer, Texture2D texture, int tileSize)
        {
            foreach (var item in layer)
            {
                Rectangle destRect = new Rectangle((int)item.Key.X * 32, (int)item.Key.Y * 32, 32, 32);
                int tileIndex = item.Value;
                int tilesPerRow = texture.Width / 32;

                Rectangle srcRect = new Rectangle(
                    (tileIndex % tilesPerRow) * 32,
                    (tileIndex / tilesPerRow) * 32,
                    32,
                    32
                );

                spriteBatch.Draw(texture, destRect, srcRect, Color.White);
            }
        }



    }
}


