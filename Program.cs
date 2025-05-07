using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Threading.Tasks;
using System.Diagnostics.SymbolStore;
using System.Diagnostics;
using System.ComponentModel.Design;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;


namespace Dungeon_game
{
    internal class Program
    {
        public static int PlayerPosX;
        public static int PlayerPosY;
        public static bool alive = true;
        static void Build(int height, int width, ref Cave cave)
        {

            Random random = new Random();
            for (int y = 0; y < cave.height; y++)
            {
                for (int x = 0; x < cave.width; x++)
                {
                    if (random.Next(0, 100) <= 70)
                    {
                        cave.tiles[y, x] = new Tile(true, false, false, false, false); // wals
                    }
                    if (random.Next(0, 100) >= 65)
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
            for (int i = 0; i < 50; i++)
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
                System.Threading.Thread.Sleep(16);
                Console.SetCursorPosition(0, 0);
            }

            for (int i = 0; i < 60; i++)
            {
                SpawnMonster(ref cave);
            }
            FindSpawn(ref cave);


            cave.PrintCave(ref cave);
        }
        static void aliveCheck(ref Cave cave)
        {
            for (int y = 0; y < cave.height; y++)
            {
                for (int x = 0; x < cave.width; x++)
                {
                    if (cave.tiles[y, x].GetSymbol() == 'M' && PlayerPosX == x && PlayerPosY == y)
                    {
                        alive = false;
                    }
                }
            }
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


            if (newX >= 0 && newX < cave.width &&
                newY >= 0 && newY < cave.height &&
                (cave.tiles[newY, newX].GetSymbol() == ' '
                || cave.tiles[newY, newX].GetSymbol() == 'M'
                || cave.tiles[newY, newX].GetSymbol() == '*'))
            {
                cave.tiles[PlayerPosY, PlayerPosX].MakeFloor();
                cave.Update(PlayerPosX, PlayerPosY, cave.tiles[PlayerPosY, PlayerPosX]);
                PlayerPosX = newX;
                PlayerPosY = newY;
                cave.tiles[PlayerPosY, PlayerPosX].MakePlayer();
                cave.Update(PlayerPosX, PlayerPosY, cave.tiles[PlayerPosY, PlayerPosX]);
            }

            cave.PrintCave(ref cave);


        }

        static void MonsterMultiplication(ref Cave cave)
        {
            Random random = new Random();

            for (int y = 0; y < cave.height; y++)
            {
                for (int x = 0; x < cave.width; x++)
                {

                    if (cave.tiles[y, x].GetSymbol() == 'M')
                    {
                        int chance = random.Next(0, 100);


                        if (chance <= 5 && y + 1 < cave.height && cave.tiles[y + 1, x].GetSymbol() != 'M')
                        {
                            if (cave.tiles[y + 1, x].GetSymbol() == '@') alive = false;
                            if (cave.tiles[y + 1, x].GetSymbol() != '#') cave.tiles[y + 1, x].MakeMonster();
                        }

                        else if (chance > 5 && chance <= 10 && y - 1 >= 0 && cave.tiles[y - 1, x].GetSymbol() != 'M')
                        {
                            if (cave.tiles[y - 1, x].GetSymbol() == '@') alive = false;
                            if (cave.tiles[y - 1, x].GetSymbol() != '#') cave.tiles[y - 1, x].MakeMonster();
                        }

                        else if (chance > 10 && chance <= 15 && x + 1 < cave.width && cave.tiles[y, x + 1].GetSymbol() != 'M')
                        {
                            if (cave.tiles[y, x + 1].GetSymbol() == '@') alive = false;
                            if (cave.tiles[y, x + 1].GetSymbol() != '#') cave.tiles[y, x + 1].MakeMonster();
                        }

                        else if (chance > 15 && chance <= 20 && x - 1 >= 0 && cave.tiles[y, x - 1].GetSymbol() != 'M')
                        {
                            if (cave.tiles[y, x - 1].GetSymbol() == '@') alive = false;
                            if (cave.tiles[y, x - 1].GetSymbol() != '#') cave.tiles[y, x - 1].MakeMonster();
                        }
                    }
                }
            }
        }


        static void Menu()
        {
            string fileName = "HighScores.txt";
            string contents = File.ReadAllText(fileName);
            Console.WriteLine("Welcome to the Dungeon Game!");
            Console.WriteLine("How Long can you survive?");
            Console.WriteLine("You can kill the monsters but if They gang up on you they can kill you...");
            Console.WriteLine("You have the strength to push through them but they do too. Avoid than at all costs");
            
            System.Threading.Thread.Sleep(6000);


        }


        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            int width = 120;
            int height = 67;
            Menu();

            string input = "";

            try
            {
                Console.WriteLine("Select Your difficulty  \n1. Hard\n2. Medium\n3.  Easy");
                Console.Write("Enter 1, 2, or 3: ");
                input = Console.ReadLine();

                if (input != "1" && input != "2" && input != "3")
                    throw new ArgumentOutOfRangeException();

                
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.WriteLine("Invalid input. Please enter 1, 2, or 3 only >:(");
            }


            switch (input)
            {
                case "1":
                    width = 80;
                    height = 67;
                    break;
                case "2":
                    width = 120;
                    height = 67;
                    break;
                case "3":
                    width = 240;
                    height = 67;
                    break;

            }

            Cave cave = new Cave(height, width);
            Console.ReadKey();
            IntroScene(ref cave, height, width);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (alive)
            {
                Controls(ref cave);
                MonsterMultiplication(ref cave);
                aliveCheck(ref cave);
            }
            stopwatch.Stop();
            Console.SetCursorPosition(0, 0);
            Console.Clear();
            Console.WriteLine($"You lasted{stopwatch.Elapsed.TotalSeconds} seconds!");
            Console.WriteLine("You are Dead");

            System.Threading.Thread.Sleep(10000);

            Console.ReadKey();
        }
    }
}
