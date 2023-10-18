using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFinalBattleComponents
{
    public class Player // Player object tracking whether player is human and status of party
    {
        public bool isHuman { get; }
        public List<Character> Party = new List<Character>();
        public List<ItemType> Items = new List<ItemType>();

        public Player(bool isHuman)
        {
            this.isHuman = isHuman;
        }
    }
}
