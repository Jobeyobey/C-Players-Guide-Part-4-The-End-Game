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
        public int MaxHp { get; init; }
        public int CurrentHp { get; set; }
        public List<AttackType> attackList = new List<AttackType>(); // List for each character to add their available actions to upon construction

        // Basic constructor
        public Character()
        {
        }

        // Use this to damage or heal a character. Positive integers heal, negative integers damage.
        public void AlterHp(int amount)
        {
            CurrentHp += amount;
            if (CurrentHp > MaxHp)
            {
                CurrentHp = MaxHp;
            }
            else if (CurrentHp <= 0)
            {
                CurrentHp = 0;
                // TODO Trigger death
            }
        }
    }

    public class MainCharacter : Character
    {
        public MainCharacter(string name)
        {
            Name = name;
            MaxHp = 25;
            CurrentHp = MaxHp;

            // Add actions character can do here
            attackList.Add(AttackType.Punch);
        }
    }

    public class Skeleton : Character
    {
        public Skeleton(string name)
        {
            Name = name;
            MaxHp = 5;
            CurrentHp = MaxHp;

            // Add actions character can do here
            attackList.Add(AttackType.BoneCrunch);
        }
    }
}
