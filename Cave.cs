using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_game
{
    public class Cave
    {
        public int width;
        public int height;
        public Tile[,] tiles;
        public Cave(int height, int width)
        {
            this.width = width;
            this.height = height;
            tiles = new Tile[height, width];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    tiles[y, x] = new Tile(false, false, false, false, false); // default
                }
            }
        }
        

    }
}
