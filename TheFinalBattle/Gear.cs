﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFinalBattleSettings;
using static TheFinalBattleComponents.ConsoleHelpers;

namespace TheFinalBattleComponents
{
    // Base gear class
    public abstract class Gear
    {
        public abstract string Name { get; init; }
        public abstract string AttackName { get; init; }
        public abstract int Damage { get; set; }
        public abstract int Accuracy { get; init; }
        public abstract DamageType Type { get; init; }
        public abstract void Special();
    }

    public class Sword : Gear
    {
        override public string Name { get; init; } = "Sword";
        override public string AttackName { get; init; } = "Slash";
        override public int Damage { get; set; } = Settings.SwordDamage;
        override public int Accuracy { get; init; } = Settings.SwordAccuracy;
        override public DamageType Type { get; init; } = DamageType.Normal;
        override public void Special() { }
        public Sword() { }
    }

    public class Dagger : Gear
    {
        override public string Name { get; init; } = "Dagger";
        override public string AttackName { get; init; } = "Stab";
        override public int Damage { get; set; } = Settings.DaggerDamage;
        override public int Accuracy { get; init; } = Settings.DaggerAccuracy;
        override public DamageType Type { get; init; } = DamageType.Normal;
        override public void Special() { }
        public Dagger() { }
    }

    public class Bow : Gear
    {
        override public string Name { get; init; } = "Bow";
        override public string AttackName { get; init; } = "Quick Shot";
        override public int Damage { get; set; } = Settings.BowDamage;
        override public int Accuracy { get; init; } = Settings.BowAccuracy;
        override public DamageType Type { get; init; } = DamageType.Normal;
        override public void Special() { }
        public Bow() { }
    }

    public class CannonOfConsolas : Gear
    {
        override public string Name { get; init; } = "Cannon of Consolas";
        override public string AttackName { get; init; } = "Cannon Blast";
        override public int Damage { get; set; } = Settings.CannonOfConsolasDamage;
        override public int Accuracy { get; init; } = Settings.CannonOfConsolasAccuracy;
        public int CannonCharge { get; set; } = 0;
        override public DamageType Type { get; init; } = DamageType.Normal;
        override public void Special()
        {
            // The Cannon of Consolas does extra damage on every 3rd and 5th shot. When both of those requirements are satisfied, it does an extra powerful shot!
            CannonCharge++;
            if (CannonCharge % 5 == 0 && CannonCharge % 3 == 0)
            {
                Damage = Settings.CannonOfConsolasDamage * 10;
            }
            else if (CannonCharge % 3 == 0)
            {
                Damage = Settings.CannonOfConsolasDamage * 3;
            }
            else if (CannonCharge % 5 == 0)
            {
                Damage = Settings.CannonOfConsolasDamage * 5;
            }
            else
            {
                Damage = Settings.CannonOfConsolasDamage;
            }
        }
        public CannonOfConsolas() { }
    }
}
