﻿using System;
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
        public Gear Equipped { get; set; }
        public List<AttackType> attackList = new List<AttackType>(); // List for each character to add their available actions to upon construction

        // Basic constructor
        public Character(Gear gear)
        {
            Equipped = gear;
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

    // Main player character. The 'Coding Hero'
    public class MainCharacter : Character
    {
        public MainCharacter(string name, Gear gear) : base (gear)
        {
            Name = name;
            MaxHp = 25;
            CurrentHp = MaxHp;
            Equipped = gear;

            // Add actions character can do here
            attackList.Add(AttackType.Punch);
        }
    }

    // Basic Skeleton
    public class Skeleton : Character
    {
        public Skeleton(string name, Gear gear) : base (gear)
        {
            Name = name;
            MaxHp = 5;
            CurrentHp = MaxHp;
            Equipped = gear;

            // Add actions character can do here
            attackList.Add(AttackType.BoneCrunch);
        }
    }

    // Final boss
    public class TheUncodedOne : Character
    {
        public TheUncodedOne(string name, Gear gear) : base(gear)
        {
            Name = name;
            MaxHp = 15;
            CurrentHp = MaxHp;
            Equipped = gear;

            // Add actions character can do here
            attackList.Add(AttackType.Unraveling);
        }
    }
}
