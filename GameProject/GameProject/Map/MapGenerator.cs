using Microsoft.Xna.Framework;
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
        private SpriteBatch spriteBatch;
        TmxMap map;
        Texture2D tileset;
        int tilesetTilesWide;
        int tileWidth;
        int tileHeight;

        public MapGenerator(SpriteBatch _spriteBatch, TmxMap _map, Texture2D _tileset, int _tilesetTilesWide, int _tileWidth, int _tileHeight)
        {
            spriteBatch = _spriteBatch;
            map = _map;
            tileset = _tileset;
            tilesetTilesWide = _tilesetTilesWide;
            tileWidth = _tileWidth;
            tileHeight = _tileHeight;
        }

        public void Draw()
        {
            spriteBatch.Begin();//Strating the drawing to the screen
            for (var i = 0; i < map.Layers.Count; i++)//This loops through all the tile map layers present on our tile map
            {
                for (var j = 0; j < map.Layers[i].Tiles.Count; j++)//this loops through the tiles in each tile layer
                {
                    int gid = map.Layers[i].Tiles[j].Gid;//Getting the GID
                    if (gid == 0)
                    {
                        //If empty then do nothing
                    }
                    else//If not empty
                    {//Some complex math to check for the tile position :(
                        int tileFrame = gid - 1;
                        int column = tileFrame % tilesetTilesWide;
                        int row = (int)Math.Floor((double)tileFrame / (double)tilesetTilesWide);
                        float x = (j % map.Width) * map.TileWidth;
                        float y = (float)Math.Floor(j / (double)map.Width) * map.TileHeight;
                        Rectangle tilesetRec = new Rectangle((tileWidth) * column, (tileHeight) * row, tileWidth, tileHeight);//The origin rectangle
                        spriteBatch.Draw(tileset, new Rectangle((int)x, (int)y, tileWidth, tileHeight), tilesetRec, Color.White);//Drawing the tile
                    }
                }
            }
            spriteBatch.End();
        }
    }
}


