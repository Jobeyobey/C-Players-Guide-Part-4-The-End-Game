﻿using System.Numerics;
using TheFinalBattleSettings;
using static TheFinalBattleComponents.Helpers;

namespace TheFinalBattleComponents
{
    // TheFinalBattle will track the status of the whole game and run it
    public class TheFinalBattle
    {
        public Player Player1 { get; private set; }
        public Player Player2 { get; private set; }
        public bool Player1Turn { get; private set; } = true;

        public TheFinalBattle() // Constructor
        {
            int setupCode = Settings.SetupGame(); // Prompt user for setupCode

            if (setupCode == 1) // Human v Human
            {
                Player1 = new Player(true);
                Player2 = new Player(true);
            }
            else if (setupCode == 2) // Human v Computer
            {
                Player1 = new Player(true);
                Player2 = new Player(false);
            }
            else if (setupCode == 3) // Computer v Computer
            {
                Player1 = new Player(false);
                Player2 = new Player(false);
            }
        }

        public void Run() // Run the game
        {
            bool heroWin = false;
            MakeHeroParty(this);

            // Keep playing through rounds until hero wins or loses
            for (int round = 1; round <= Settings.NumRounds; round++)
            {
                MakeMonsterParty(this, round); // Create monster party for current round

                heroWin = PlayRound(); // Play round until hero wins or loses

                if (!heroWin) break; // If hero does not win, break loop and continue to EndGame

                ConsoleHelpWriteLine("The enemy party has been defeated!", ConsoleColor.Yellow); // Else if hero wins, announce it
            }

            EndGame(heroWin); // Declare winner
        }

        // A round consists of multiple turns, ending when a party has no characters left.
        public bool PlayRound()
        {
            int turnNumber = 0;

            // Core Round Loop
            while (true)
            {
                Player activePlayer;
                Character activeChar;
                int charIndex;

                // Find which character's turn it is
                if (Player1Turn)
                {
                    activePlayer = Player1;
                    charIndex = turnNumber % Player1.Party.Count;
                    activeChar = Player1.Party[charIndex];
                }
                else
                {
                    activePlayer = Player2;
                    charIndex = turnNumber % Player2.Party.Count;
                    activeChar = Player2.Party[charIndex];
                }

                DisplayGameStatus(this, activeChar);

                TakeTurn(activePlayer, activeChar);

                // Check each party for dead characters
                StatusCheck(Player1.Party);
                StatusCheck(Player2.Party);

                // Check if either party has no characters left
                if (Player1.Party.Count == 0)
                {
                    return false; // heroWin == false
                }
                else if (Player2.Party.Count == 0)
                {
                    return true; // heroWin == true
                }

                Player1Turn = !Player1Turn; // Flip who's turn it is

                // Increment turn number if this is the end of Player2's turn
                if (!Player1Turn) turnNumber++;
            }
        }

        public void StatusCheck(List<Character> party)
        {
            List<Character> toBeRemoved = new List<Character>();

            // Add list of dead characters to new list, to prevent errors removing them while iterating
            foreach (Character character in party)
            {
                if (character.CurrentHp == 0)
                {
                    toBeRemoved.Add(character);
                }
            }

            // Remove characters from party using temporary list
            foreach (Character character in toBeRemoved)
            {
                ConsoleHelpWriteLine($"{character.Name} has been defeated!", ConsoleColor.Yellow);
                party.Remove(character);
            }
        }

        // End game, declaring winner
        public void EndGame(bool heroWin)
        {
            if (heroWin)
            {
                ConsoleHelpWriteLine($"The heroes have triumphed in their battle against The Uncoded One! The people are free to code again.", ConsoleColor.Yellow);
            }
            else
            {
                ConsoleHelpWriteLine($"The heroes have fallen against The Uncoded One! The reign of terror continues.", ConsoleColor.Red);
            }
        }

        public void TakeTurn(Player activePlayer, Character activeChar)
        {
            // Reserve memory for objects
            IAction action = null;

            ConsoleHelpWriteLine($"It is {activeChar.Name}'s turn...", ConsoleColor.Yellow);

            while (action == null)
            {
                // Prompt for input
                ActionType chosenAction = PickAction(activePlayer.isHuman);

                // Resolve input
                if (chosenAction == ActionType.Nothing) action = new NothingAction(activeChar);
                else if (chosenAction == ActionType.Attack) action = PickAttack(this, activeChar, activePlayer);
                else if (chosenAction == ActionType.UseItem) action = PickItem(this, activeChar, activePlayer);
                else action = null; // Failsafe in case of error
            }

            action.Execute(this);
        }

        // List actions character can take
        public ActionType PickAction(bool isHuman)
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

        public IAction PickAttack(TheFinalBattle game, Character character, Player activePlayer)
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
            if (activePlayer == Player1)
                targetPlayer = Player2;
            else
                targetPlayer = Player1;

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

        public IAction PickItem(TheFinalBattle game, Character character, Player activePlayer)
        {
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
                targetPlayer = activePlayer == Player1 ? Player1 : Player2; // Target friendly party
            }
            else
            {
                targetPlayer = activePlayer == Player1 ? Player2 : Player1; // Target enemy party
            }

            // Pick target. '0' is to pick another action
            Character target = PickTarget(targetPlayer.Party, activePlayer.isHuman);
            if (target == null)
                return null;

            // Add all possible items here
            if (chosenItem == ItemType.HealthPotion) return new HealthPotion(activePlayer, character, target);
            else return new Punch(character, target);
        }

        public Character PickTarget(List<Character> targetParty, bool isHuman)
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
