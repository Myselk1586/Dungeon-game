using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_game
{
    internal class Program
    {
        static int PlayerPosX;
        static int PlayerPosY;
        static void Build(int height, int width, ref Cave cave)
        {

            Random random = new Random();
            for (int y = 0; y < cave.height; y++)
            {
                for (int x = 0; x < cave.width; x++)
                {
                    if (random.Next(0, 100) <= 65)
                    {
                        cave.tiles[y, x] = new Tile(true, false, false, false, false); // wals
                    }
                    if (random.Next(0, 100) <= 30)
                    {
                        cave.tiles[y, x] = new Tile(false, false, false, false, false); // floor
                    }

                }
            }
            
            
        }
        static void SpawnMonster(ref Cave cave)
        {
            Random random = new Random();
            int x = random.Next(0, cave.width);
            int y = random.Next(0, cave.height);
            for(int i = 0; i < 40; i++)
            {
                if (cave.tiles[y, x].GetSymbol() == ' ')
                {
                    x = random.Next(0, cave.width);
                    y = random.Next(0, cave.height);
                }
            }
            
            cave.tiles[y, x].MakeMonster();
        }
        static void SmoothCave(ref Cave cave)
        {
            
            Tile[,] newTiles = new Tile[cave.height, cave.width];

            for (int y = 0; y < cave.height; y++)
            {
                for (int x = 0; x < cave.width; x++)
                {
                    
                    Tile tile = new Tile(false, false, false, false, false);

                    int wallCount = WallCount(ref cave, x, y);

                    if (wallCount >= 5)
                        tile.MakeWall();
                    else
                        tile.MakeFloor();

                    newTiles[y, x] = tile;
                }
            }

            
            for (int y = 0; y < cave.height; y++)
            {
                for (int x = 0; x < cave.width; x++)
                {
                    if (x < 3 || x >= cave.width - 3 || y < 3 || y >= cave.height - 3)
                    {
                        
                        newTiles[y, x].MakeWall();
                    }
                }
            }

            
            cave.tiles = newTiles;
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
        static void FindSpawn(ref Cave cave)
        {
            int x = 0;
            int y = 0;
            for (int i=0; i < 20; i++)
            {
                Random random = new Random();
                
                x = random.Next(0, cave.width);
                y = random.Next(0, cave.height);
                if (cave.tiles[y, x].GetSymbol() != '#')
                {
                    PlayerPosX = x;
                    PlayerPosY = y;
                }
            }
            cave.tiles[y, x].MakePlayer();
            
        }

        static void Main(string[] args)
        {
            int width = 80;
            int height = 60;


            Cave cave = new Cave(height, width);

            // Build dungeon
            Build(height, width, ref cave);
            for (int i = 0; i < 8; i++)
            {
                SmoothCave(ref cave);
            }
            FindSpawn(ref cave);
            for(int i = 0; i < 5; i++)
            {
                SpawnMonster(ref cave);
            }
            

            // Print the dungeon map
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
                    else 
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }

                        Console.Write(cave.tiles[y, x].GetSymbol());
                }
                Console.WriteLine();
            }
            Console.ReadKey();
        }
    }
}
