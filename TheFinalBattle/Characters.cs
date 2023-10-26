using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFinalBattleComponents
{
    // A character is an in-game party member with a name, hp, items and more
    public abstract class Character
    {
        public string Name { get; init; } = "Unnamed";
        public int MaxHp { get; init; }
        public int CurrentHp { get; set; }
        public DefenseType Defense { get; set; } = DefenseType.None;
        public Gear Equipped { get; set; } = null;
        public List<AttackType> attackList = new List<AttackType>(); // List for each character to add their available actions to upon construction

        // Basic constructor
        public Character(Gear gear)
        {
            if (gear != null)
            {
                Equipped = gear;
                attackList.Add(AttackType.Weapon);
            }
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
            }
        }
    }

    // HEROES
    public class MainCharacter : Character
    {
        public MainCharacter(string name, Gear gear) : base (gear)
        {
            Name = name;
            MaxHp = 25;
            CurrentHp = MaxHp;
            Defense = DefenseType.ObjectSight;
            Equipped = gear;

            // Add actions character can do here. 'Insert' because I want the character-specific attacks to be listed first.
            attackList.Insert(0, AttackType.Punch);
        }
    }

    public class VinFletcher : Character
    {
        public VinFletcher(Gear gear) : base (gear)
        {
            Name = "Vin Fletcher";
            MaxHp = 15;
            CurrentHp = MaxHp;
            Equipped = gear;

            // Add actions character can do here
            attackList.Insert(0, AttackType.PileOn);
        }
    }

    public class MylaraAndSkorin : Character
    {
        public MylaraAndSkorin(Gear gear) : base(gear)
        {
            Name = "Mylara and Skorin";
            MaxHp = 10;
            CurrentHp = MaxHp;
            Equipped = gear;

            // Add actions character can do here
            attackList.Insert(0, AttackType.PileOn);
        }
    }

    // MONSTERS
    public class Skeleton : Character
    {
        public Skeleton(string name, Gear gear) : base (gear)
        {
            Name = name;
            MaxHp = 5;
            CurrentHp = MaxHp;
            Equipped = gear;

            // Add actions character can do here
            attackList.Insert(0, AttackType.BoneCrunch);
        }
    }

    public class StoneAmarok : Character
    {
        public StoneAmarok(string name, Gear gear) : base(gear)
        {
            Name = name;
            MaxHp = 4;
            CurrentHp = MaxHp;
            Equipped = gear;
            Defense = DefenseType.StoneArmour;

            // Add actions character can do here
            attackList.Insert(0, AttackType.Bite);
        }
    }

    public class TheUncodedOne : Character
    {
        public TheUncodedOne(string name, Gear gear) : base(gear)
        {
            Name = name;
            MaxHp = 15;
            CurrentHp = MaxHp;
            Equipped = gear;

            // Add actions character can do here
            attackList.Insert(0, AttackType.Unraveling);
        }
    }
}
