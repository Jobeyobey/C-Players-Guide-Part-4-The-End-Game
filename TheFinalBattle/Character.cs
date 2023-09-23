using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFinalBattleComponents
{
    // A character is an in-game character with a name, hp and items
    public abstract class Character
    {
        public string Name { get; init; } = "Unnamed";
        public int MaxHp { get; }
        public int CurrentHP { get; private set; }
        public List<Action> actionList = new List<Action>(); // List for each character to add their available actions to upon construction

        // Basic constructor
        public Character(int maxHp)
        {
            MaxHp = maxHp;
            CurrentHP = maxHp;
        }
    }

    public class MainCharacter : Character
    {
        public MainCharacter(int maxHp, string name) : base(maxHp)
        {
            Name = name;

            // Add actions character can do here
            actionList.Add(Action.Nothing);
        }
    }

    public class Skeleton : Character
    {
        public Skeleton(int maxHp) : base(maxHp)
        {
            Name = "SKELETON";

            // Add actions character can do here
            actionList.Add(Action.Nothing);
        }
    }
}
