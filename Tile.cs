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
        private bool isFloor;

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
            hasPlayer = false;
            hasMonster = false;
            hasTreasure = false;
            isFloor = false;

        }
        public void MakeFloor()
        {
            isFloor = true;
            isWall = false;
            hasPlayer = false;
            hasMonster = false;
            hasTreasure = false;

        }
        public void MakePlayer()
        {
            hasPlayer = true;
            isWall = false;
            isFloor = false;
            hasMonster = false;
            hasTreasure = false;

        }
        public  void MakeMonster()
        {
            hasMonster = true;
            isWall = false;
            isFloor = false;
            hasPlayer = false;
            hasTreasure = false;

        }
        public void MakeTreasure()
        {
            hasTreasure = true;
            isWall = false;
            isFloor = false;
            hasPlayer = false;
            hasMonster = false;
        }

        public char GetSymbol()
        {
            if (hasPlayer)
            {
                return '@';
            }
            else if (hasMonster)
            {
                return 'M';
            }
            else if (hasTreasure)
            {
                return '*';
            }
            else if (isWall)
            {
                return '#';
            }
            else if (isFloor)
            {
                return ' ';
            }
            else 
            {
                return 'X';
            }
            
        }
    }
}
