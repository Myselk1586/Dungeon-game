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
        
        public void PrintCave(ref Cave cave)
        {
            Console.SetCursorPosition(0, 0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (cave.tiles[y, x].GetSymbol() == '#')
                    {
                        Console.BackgroundColor = ConsoleColor.DarkGray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else if (cave.tiles[y, x].GetSymbol() == 'M')
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                    }
                    else if (cave.tiles[y, x].GetSymbol() == '@' )
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                    }
                    else if (cave.tiles[y, x].GetSymbol() == ' ')
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }

                    Console.Write(cave.tiles[y, x].GetSymbol());
                }
                Console.WriteLine();
            }
        }
        public void Update(int x, int y, Tile tile)
        {
            Console.SetCursorPosition(x, y);

            switch (tile.GetSymbol())
            {
                case '#':
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case 'M':
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case '@':
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                default:
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
            }

            Console.Write(tile.GetSymbol());
            Console.ResetColor();
        }

    }
}
