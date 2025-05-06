using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dungeon_game
{
    public class Tile
    {
        private bool isWall;
        private bool hasPlayer;
        private bool hasMonster;
        private bool hasTreasure;
        private bool isVisible;

        public Tile(bool isWall, bool hasPlayer, bool hasMonster, bool hasTreasure, bool isVisible)
        {
            this.isWall = isWall;
            this.hasPlayer = hasPlayer;
            this.hasMonster = hasMonster;
            this.hasTreasure = hasTreasure;
            this.isVisible = isVisible;
        }
        public void MakeWall()
        {
            isWall = true;
        }
        public void MakeFloor()
        {
            isWall = false;
        }
        public void MakePlayer()
        {
            hasPlayer = true;
        }
        public  void MakeMonster()
        {
            hasMonster = true;
        }
        
        public char GetSymbol()
        {
            if (isWall)
            {
                return '#';
            }
            else if (hasPlayer)
            {
                return 'P';
            }
            else if (hasMonster)
            {
                return '0';
            }
            else if (hasTreasure)
            {
                return 'T';
            }
            else
            {
                return ' ';
            }
        }
    }
}
