using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
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
        public abstract bool HasSpecial { get; init; }
        public abstract void Special(Attack attack);
    }

    public class Sword : Gear
    {
        override public string Name { get; init; } = "Sword";
        override public string AttackName { get; init; } = "Slash";
        override public int Damage { get; set; } = Settings.SwordDamage;
        override public int Accuracy { get; init; } = Settings.SwordAccuracy;
        override public DamageType Type { get; init; } = DamageType.Normal;
        override public bool HasSpecial { get; init; } = false;
        override public void Special(Attack attack) { }
        public Sword() { }
    }

    public class Dagger : Gear
    {
        override public string Name { get; init; } = "Dagger";
        override public string AttackName { get; init; } = "Stab";
        override public int Damage { get; set; } = Settings.DaggerDamage;
        override public int Accuracy { get; init; } = Settings.DaggerAccuracy;
        override public DamageType Type { get; init; } = DamageType.Normal;
        override public bool HasSpecial { get; init; } = false;
        override public void Special(Attack attack) { }
        public Dagger() { }
    }

    public class Bow : Gear
    {
        override public string Name { get; init; } = "Bow";
        override public string AttackName { get; init; } = "Quick Shot";
        override public int Damage { get; set; } = Settings.BowDamage;
        override public int Accuracy { get; init; } = Settings.BowAccuracy;
        override public DamageType Type { get; init; } = DamageType.Normal;
        override public bool HasSpecial { get; init; } = false;
        override public void Special(Attack attack) { }
        public Bow() { }
    }

    public class CannonOfConsolas : Gear
    {
        override public string Name { get; init; } = "Cannon of Consolas";
        override public string AttackName { get; init; } = "Cannon Blast";
        override public int Damage { get; set; } = Settings.CannonOfConsolasDamage;
        override public int Accuracy { get; init; } = Settings.CannonOfConsolasAccuracy;
        public int CannonCharge { get; set; } = 1;
        override public DamageType Type { get; init; } = DamageType.Energy;
        override public bool HasSpecial { get; init; } = true;
        override public void Special(Attack attack)
        {
            ActionHelper.DoAttack(attack);

            // The Cannon of Consolas does extra damage on every 3rd and 5th shot. When both of those requirements are satisfied, it does an extra powerful shot!
            CannonCharge++;
            if (CannonCharge % 5 == 0 && CannonCharge % 3 == 0)
            {
                Damage = Settings.CannonOfConsolasDamage * 8;
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

            ConsoleHelpWriteLine($"Cannon charge: {CannonCharge}. Next shot damage: {Damage}", ConsoleColor.Yellow);
        }
        public CannonOfConsolas() { }
    }

    public class Tome : Gear
    {
        override public string Name { get; init; } = "Tome";
        override public string AttackName { get; init; } = "Tome";
        override public int Damage { get; set; } = Settings.TomeDamage;
        override public int Accuracy { get; init; } = Settings.TomeAccuracy;
        override public DamageType Type { get; init; } = DamageType.Normal;
        override public bool HasSpecial { get; init; } = true;
        override public void Special(Attack attack)
        {
            ConsoleHelpWriteLine($"{attack.ActiveChar.Name} attempts to curse {attack.TargetChar.Name}", ConsoleColor.Yellow);

            Random random = new Random();
            int randomInt = random.Next(100);

            if(randomInt < Accuracy)
            {
                bool isCursed = false;

                foreach (NegativeStatus status in attack.TargetChar.negativeStatuses)
                {
                    if (status == NegativeStatus.Cursed)
                        isCursed = true;
                }

                if (!isCursed)
                {
                    attack.TargetChar.negativeStatuses.Add(NegativeStatus.Cursed);
                    ConsoleHelpWriteLine($"{attack.TargetChar.Name} has been cursed!", ConsoleColor.Yellow);
                }
                else
                {
                    ConsoleHelpWriteLine($"{attack.TargetChar.Name} is already cursed!", ConsoleColor.Yellow);
                }
            }
            else
            {
                ConsoleHelpWriteLine($"{attack.TargetChar.Name} resisted the curse!", ConsoleColor.Yellow);
            }
        }

        public Tome() { }
    }
}
