using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_game
{
    internal class Program
    {
        static void BuildDungeon(int height, int width, ref Cave cave)
        {
            
            Random random = new Random();
            for(int y = 0; y < cave.height; y++)
            {
                for(int x = 0; x < cave.width; x++)
                {
                    if (random.Next(0, 100) <= 65)
                    {
                        cave.tiles[y,x] = new Tile(true, false, false, false, false); // wals
                    }
                    if (random.Next(0, 100) <= 30)
                    {
                        cave.tiles[y, x] = new Tile(false, false, false, false, false); // floor
                    }
                    if (random.Next(0, 100) <= 1)
                    {
                        cave.tiles[y, x] = new Tile(false, false, true, false, false);
                    }


                }
            }
        }
        static void Smoothing(ref Cave cave, int x, int y)
        {
            // Early exit for out of bounds
            if (y < 3 || y >= cave.height - 3 || x < 3 || x >= cave.width - 3)
            {
                cave.tiles[y, x].MakeWall();
                return;
            }

            // Loop over the entire cave grid
            for (int yCoord = 0; yCoord < cave.height; yCoord++)
            {
                for (int xCoord = 0; xCoord < cave.width; xCoord++)
                {
                    // Pass the correct coordinates to WallCount
                    int wallCount = WallCount(ref cave, xCoord, yCoord);

                    // Apply wall/floor based on the count
                    if (wallCount >= 5)
                    {
                        cave.tiles[yCoord, xCoord].MakeWall();
                    }
                    else
                    {
                        cave.tiles[yCoord, xCoord].MakeFloor();
                    }
                }
            }
        }

        static int WallCount(ref Cave cave, int x, int y)
        {
            int count = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int surrX = x + j;
                    int surrY = y + i;

                    
                    if (surrX < 0 || surrY < 0 || surrX >= cave.width || surrY >= cave.height)
                    {
                        count++; 
                    }
                    else if (cave.tiles[surrY, surrX].GetSymbol() == '#')
                    {
                        count++; 
                    }
                }
            }

            return count;
        }
        
        static void Main(string[] args)
        {
            int width = 80;
            int height = 60;

            
            Cave cave = new Cave(height, width);

            // Build dungeon
            BuildDungeon(height, width, ref cave);
            for (int i = 0; i < 6; i++)
            {
                for (int y = 3; y < height; y++)
                {
                    for (int x = 3; x < width; x++)
                    {

                        Smoothing(ref cave, x, y);

                    }
                }
            }
            
            // Print the dungeon map
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write(cave.tiles[y, x].GetSymbol());
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
}
