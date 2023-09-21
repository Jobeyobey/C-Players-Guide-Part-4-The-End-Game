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

        // Basic constructor
        public Character(int maxHp, int currentHP)
        {
            MaxHp = maxHp;
            CurrentHP = currentHP;
        }
        public void SkipTurn()
        {
            Console.WriteLine($"{Name} did NOTHING");
        }
        public abstract void SpecialAttack();
    }

    public class Skeleton : Character
    {
        public Skeleton(int maxHp, int currentHP) : base(maxHp, currentHP)
        {
            Name = "SKELETON";
        }
        public override void SpecialAttack()
        {
            //TODO
        }
    }
}
