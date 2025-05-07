using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_game
{
    public class Monsters : Tile
    {
        public int Health;
        public int Damage;
        public int Speed;
        public string Name;
        public Monsters(int health, int damage, int speed, int level, string name) : base(false, false, true, false, false)
        {
            Health = health;
            Damage = damage;
            Speed = speed;
            Name = name;
        }
        public void Move(ref Cave cave, int x, int y)
        {
            Random random = new Random();
            int newX = x + random.Next(0, 2);
            int newY = y + random.Next(0, 2);
            if (newX < 0 || newX >= cave.width || newY < 0 || newY >= cave.height || cave.tiles[newY, newX].GetSymbol() == '#')
            {
                return; 
            }
            if (cave.tiles[newY, newX].GetSymbol() == ' ')
            {
                cave.tiles[y, x].MakeFloor();
                cave.Update(x, y, cave.tiles[y, x]);
                x = newX;
                y = newY;
                cave.tiles[y, x].MakeMonster();
                cave.Update(x, y, cave.tiles[y, x]);
            }
            else if (cave.tiles[newY, newX].GetSymbol() == '@')
            {
                
            }
        }
    }
    
    
}
