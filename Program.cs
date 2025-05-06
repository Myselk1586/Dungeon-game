using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_game
{
    internal class Program
    {
        public static int PlayerPosX;
        public static int PlayerPosY;
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
                if (cave.tiles[y, x].GetSymbol() != ' ')
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
            Random random = new Random();
            do
            {
                x = random.Next(3, cave.width); 
                y = random.Next(3, cave.height);
            } while (cave.tiles[y, x].GetSymbol() == '#');
            cave.tiles[y, x].MakePlayer();
            PlayerPosX = x;
            PlayerPosY = y;

        }
        static void IntroScene(ref Cave cave, int height, int width)
        {
            Build(height, width, ref cave);
            for (int i = 0; i < 8; i++)
            {
                Console.SetCursorPosition(0, 0);
                SmoothCave(ref cave);
                cave.PrintCave(ref cave);
                System.Threading.Thread.Sleep(1000);
                Console.SetCursorPosition(0, 0);
            }

            for (int i = 0; i < 60; i++) // Adjust the number of monsters as needed
            {
                SpawnMonster(ref cave);
            }
            FindSpawn(ref cave);

            // Print the dungeon map
            cave.PrintCave(ref cave);
        }
        static void Controls(ref Cave cave)
        { 
            ConsoleKeyInfo key = Console.ReadKey(true);
            int newX = PlayerPosX;
            int newY = PlayerPosY;

            switch (key.Key)
            {
                case ConsoleKey.W:
                    newY--; 
                    break;
                case ConsoleKey.S:
                    newY++; 
                    break;
                case ConsoleKey.A:
                    newX--; 
                    break;
                case ConsoleKey.D:
                    newX++; 
                    break;
                case ConsoleKey.Escape:
                    return;
                default:
                    Console.WriteLine("That was NOT a valid key.");
                    break;
            }

            // Check if new position is within bounds and walkable
            if (newX >= 0 && newX < cave.width &&
                newY >= 0 && newY < cave.height &&
                (cave.tiles[newY, newX].GetSymbol() == ' ' 
                || cave.tiles[newY,newX].GetSymbol() == 'M'
                || cave.tiles[newY, newX].GetSymbol() == '*') )
            {
                cave.tiles[PlayerPosY, PlayerPosX].MakeFloor(); 
                PlayerPosX = newX;
                PlayerPosY = newY;
                cave.tiles[PlayerPosY, PlayerPosX].MakePlayer(); 
            }
            
            cave.PrintCave(ref cave);


        }
        static void Main(string[] args)
        {
            int width = 80;
            int height = 67;
            Cave cave = new Cave(height, width);
            Console.ReadKey();
            IntroScene(ref cave, height, width);
            while (true)
            {
                Controls(ref cave);
            }
            

            // Build dungeon


            Console.ReadKey();
        }
    }
}
