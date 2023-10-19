using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheFinalBattleSettings;
using static TheFinalBattleComponents.ConsoleHelpers;

namespace TheFinalBattleComponents
{
    public class Helpers
    {
        // Pick an integer between 0 and 'max'.
        // Used for having human/computer pick an action from action/attack/item menus
        public static int PickFromMenu(int max, bool isHuman)
        {
            int choice;

            if (isHuman) // If player is human, prompt them to pick an option
            {
                // Ensure player choice is valid
                while (true)
                {
                    if (int.TryParse(ConsoleHelpReadLine(ConsoleColor.Cyan), out choice))
                    {
                        if (choice >= 0 && choice <= max)
                        {
                            break;
                        }
                    }

                    ConsoleHelpWriteLine("Please pick a valid number from the menu.", ConsoleColor.Red);
                }
            }
            else // If player is not human, pick a random option
            {
                Thread.Sleep(Settings.Delay);

                Random random = new Random();
                choice = random.Next(max) + 1;
            }

            return choice;
        }

        // List actions character can take
        public static ActionType PickAction(bool isHuman)
        {
            // Print list of actions with index numbers for player to pick from
            int index = 0;
            foreach (ActionType action in Enum.GetValues(typeof(ActionType)))
            {
                index++;
                ConsoleHelpWriteLine($"{index} - {action}", ConsoleColor.White);
            }

            int actionIndex = PickFromMenu(index, isHuman);

            ActionType chosenAction = (ActionType)actionIndex - 1; // '-1' because array is zero-indexed

            return chosenAction;
        }

        public static ActionType ComputerAction(TheFinalBattle game, Player activePlayer)
        {
            // Print menu to console despite player being computer (Just nice to see what options are available when watching)
            int index = 0;
            foreach (ActionType action in Enum.GetValues(typeof(ActionType)))
            {
                index++;
                ConsoleHelpWriteLine($"{index} - {action}", ConsoleColor.White);
            }

            Thread.Sleep(Settings.Delay);

            // Check if party has items to use
            if (activePlayer.Items.Count == 0)
            {
                return ActionType.Attack;
            }
            else
            {
                // Check whether items in inventory are offensive or defensive
                bool hasDefensiveItem = false;
                bool hasOffensiveItem = false;

                foreach (ItemType item in activePlayer.Items)
                {
                    if (item == ItemType.HealthPotion)
                        hasDefensiveItem = true;
                    // ADD ELSE IF FOR OFFENSIVE ITEMS WHEN ADDED
                }

                // Check if available items are useful
                bool healUseful = false;

                if (hasDefensiveItem)
                {
                    foreach (Character character in activePlayer.Party)
                    {
                        if (character.CurrentHp <= character.MaxHp / 2)
                        {
                            healUseful = true;
                        }
                    }
                }
                // Calculate what action to take
                Random random = new Random();
                int randomInt = random.Next(100);
                int attackChance;
                int itemChance;

                if (healUseful)
                {
                    attackChance = 75;
                    itemChance = 25;

                    if (randomInt < attackChance)
                        return ActionType.Attack;
                    else if (randomInt < attackChance + itemChance)
                        return ActionType.UseItem;
                    else
                        return ActionType.Nothing;
                }
                else
                {
                    return ActionType.Attack;
                }
            }
        }

        public static IAction PickAttack(TheFinalBattle game, Character character, Player activePlayer)
        {
            ConsoleHelpWriteLine("Pick an attack.", ConsoleColor.Yellow);

            // Print list of attacks with index numbers for player to pick from
            ConsoleHelpWriteLine($"0 - Pick another action", ConsoleColor.White);
            int index = 0;
            foreach (AttackType attack in character.attackList)
            {
                index++;
                ConsoleHelpWriteLine($"{index} - {attack}", ConsoleColor.White);
            }

            int chosenIndex = PickFromMenu(index, activePlayer.isHuman);

            // Calculate target player
            Player targetPlayer;
            if (activePlayer == game.Player1)
                targetPlayer = game.Player2;
            else
                targetPlayer = game.Player1;

            // Pick target. Returns null if user wants to pick another action
            Character target = PickTarget(targetPlayer.Party, activePlayer.isHuman);
            if (target == null)
                return null;

            AttackType chosenAttackName = character.attackList[chosenIndex - 1]; // '-1' because array is zero-indexed

            // Add all possible attacks here
            if (chosenAttackName == AttackType.BoneCrunch) return new BoneCrunch(character, target);
            if (chosenAttackName == AttackType.Unraveling) return new Unraveling(character, target);
            else return new Punch(character, target);
        }

        public static IAction PickItem(TheFinalBattle game, Character character, Player activePlayer)
        {
            if (!activePlayer.isHuman)
                return ComputerItem(game, character, activePlayer);

            // Check player has items to use
            if (activePlayer.Items.Count == 0)
            {
                ConsoleHelpWriteLine("You have no items in your inventory.", ConsoleColor.Red);
                return null;
            }

            ConsoleHelpWriteLine("Pick an item to use.", ConsoleColor.Yellow);

            // Print list of items with index numbers for player to pick from
            ConsoleHelpWriteLine($"0 - Pick another action", ConsoleColor.White);
            int index = 0;
            foreach (ItemType item in activePlayer.Items)
            {
                index++;
                ConsoleHelpWriteLine($"{index} - {item}", ConsoleColor.White);
            }

            // Pick item to use. '0' is to pick another action
            int chosenIndex = PickFromMenu(index, activePlayer.isHuman);
            if (chosenIndex == 0)
                return null;

            ItemType chosenItem = activePlayer.Items[chosenIndex - 1]; // '-1' because array is zero-indexed

            // Choose target party depending on whether it's an offensive/defensive item
            Player targetPlayer;
            if (chosenItem == ItemType.HealthPotion)
            {
                targetPlayer = activePlayer == game.Player1 ? game.Player1 : game.Player2; // Target friendly party
            }
            else
            {
                targetPlayer = activePlayer == game.Player1 ? game.Player2 : game.Player1; // Target enemy party
            }

            // Pick target. '0' is to pick another action
            Character target = PickTarget(targetPlayer.Party, activePlayer.isHuman);
            if (target == null)
                return null;

            // Add all possible items here
            if (chosenItem == ItemType.HealthPotion) return new HealthPotion(activePlayer, character, target);
            else return null;
        }

        public static IAction ComputerItem(TheFinalBattle game, Character activeChar, Player activePlayer)
        {
            ItemType chosenItem;
            ConsoleHelpWriteLine("Pick an item to use.", ConsoleColor.Yellow);

            // Print list of items with index numbers for player to pick from
            int index = 0;
            foreach (ItemType item in activePlayer.Items)
            {
                index++;
                ConsoleHelpWriteLine($"{index} - {item}", ConsoleColor.White);
            }

            // Check what items are available
            bool hasHealthPotion = false;

            foreach (ItemType item in activePlayer.Items)
            {
                if (item == ItemType.HealthPotion)
                    hasHealthPotion = true;
            }

            // Check if health potion can/should be used
            if (hasHealthPotion)
            {
                bool needsHealing = false;
                Character damagedChar = null;

                // Check if any characters need healing and track character most in need
                foreach (Character partyMember in activePlayer.Party)
                {
                    if (partyMember.CurrentHp <= partyMember.MaxHp / 2)
                    {
                        needsHealing = true;

                        if (damagedChar == null)
                            damagedChar = partyMember;
                        else if (partyMember.CurrentHp / partyMember.MaxHp < damagedChar.CurrentHp / damagedChar.MaxHp)
                            damagedChar = partyMember;
                    }
                }

                // If a character needs healing, heal the one most in need
                if (needsHealing)
                {
                    return new HealthPotion(activePlayer, activeChar, damagedChar); 
                }
            }

            return null;
        }

        public static Character PickTarget(List<Character> targetParty, bool isHuman)
        {
            ConsoleHelpWriteLine("Pick a target.", ConsoleColor.Yellow);

            // List available target
            ConsoleHelpWriteLine($"0 - Pick another action", ConsoleColor.White);
            int index = 0;
            foreach (Character target in targetParty)
            {
                index++;
                ConsoleHelpWriteLine($"{index} - {target.Name}", ConsoleColor.White);
            }

            // Prompt user to pick target. If user returns '0', they want to go back and pick another action.
            int chosenTarget = PickFromMenu(index, isHuman);
            if (chosenTarget == 0)
                return null;

            return targetParty[chosenTarget - 1]; // '-1' because array is zero-indexed
        }
    }
}
