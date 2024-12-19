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
        private TmxMap _map;
        private Texture2D _tilesetTexture;
        private Texture2D _tileset;
        private int _tileHeight;
        private int _tileWidth;

        public MapGenerator(TmxMap map, ContentManager content)
        {
            _map = map;
            _tileset = content.Load<Texture2D>("Textures/" + _map.Tilesets[0].Name);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (var i = 0; i < _map.Layers[0].Tiles.Count; i++)
            {
                int gid = _map.Layers[0].Tiles[i].Gid;

                // Empty tile, do nothing
                if (gid == 0)
                {

                }
                else
                {
                    int tileFrame = gid - 1;
                    int row = tileFrame / (_tileset.Height / _tileHeight);

                    float x = (i % _map.Width) * _map.TileWidth;
                    float y = (float)Math.Floor(i / (double)_map.Width) * _map.TileHeight;

                    Rectangle tilesetRec = new Rectangle(_tileWidth * tileFrame, _tileHeight * row, 32, 32);

                    spriteBatch.Draw(_tileset, new Rectangle((int)x, (int)y, 32, 32), tilesetRec, Color.White);
                }
            }
        }
    }
}


