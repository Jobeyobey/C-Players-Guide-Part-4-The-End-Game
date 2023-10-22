﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheFinalBattleComponents
{
    // Base gear class
    public abstract class Gear
    {
        public abstract string Name { get; init; }
        public abstract int Damage { get; init; }
        public abstract int HitChance { get; init; }
        public abstract DamageType Type { get; init; }
    }

    public class Sword : Gear
    {
        override public string Name { get; init; } = "Sword";
        override public int Damage { get; init; } = 2;
        override public int HitChance { get; init; } = 90;
        override public DamageType Type { get; init; } = DamageType.Normal;
        public Sword() {}
    }

    public class Dagger : Gear
    {
        override public string Name { get; init; } = "Dagger";
        override public int Damage { get; init; } = 1;
        override public int HitChance { get; init; } = 90;
        override public DamageType Type { get; init; } = DamageType.Normal;
        public Dagger() {}
    }

    public enum DamageType { Normal };
}
