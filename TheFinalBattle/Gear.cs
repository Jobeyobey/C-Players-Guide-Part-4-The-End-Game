using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFinalBattleSettings;

namespace TheFinalBattleComponents
{
    // Base gear class
    public abstract class Gear
    {
        public abstract string Name { get; init; }
        public abstract string AttackName { get; init; }
        public abstract int Damage { get; init; }
        public abstract int Accuracy { get; init; }
        public abstract DamageType Type { get; init; }
    }

    public class Sword : Gear
    {
        override public string Name { get; init; } = "Sword";
        override public string AttackName { get; init; } = "Slash";
        override public int Damage { get; init; } = Settings.SwordDamage;
        override public int Accuracy { get; init; } = Settings.SwordAccuracy;
        override public DamageType Type { get; init; } = DamageType.Normal;
        public Sword() { }
    }

    public class Dagger : Gear
    {
        override public string Name { get; init; } = "Dagger";
        override public string AttackName { get; init; } = "Stab";
        override public int Damage { get; init; } = Settings.DaggerDamage;
        override public int Accuracy { get; init; } = Settings.DaggerAccuracy;
        override public DamageType Type { get; init; } = DamageType.Normal;
        public Dagger() { }
    }

    public class Bow : Gear
    {
        override public string Name { get; init; } = "Bow";
        override public string AttackName { get; init; } = "Quick Shot";
        override public int Damage { get; init; } = Settings.BowDamage;
        override public int Accuracy { get; init; } = Settings.BowAccuracy;
        override public DamageType Type { get; init; } = DamageType.Normal;
        public Bow() { }
    }
}
