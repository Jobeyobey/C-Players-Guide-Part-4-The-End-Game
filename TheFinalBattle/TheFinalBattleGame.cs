using System.Numerics;
using TheFinalBattleSettings;

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
            MakeHeroParty();

            // Keep playing through rounds until hero wins or loses
            for (int round = 1; round <= Settings.NumRounds; round++)
            {
                MakeMonsterParty(round); // Create monster party for current round

                heroWin = PlayRound(); // Play round until hero wins or loses

                if (!heroWin) break; // If hero does not win, break loop and continue to EndGame

                Console.WriteLine("The enemy party has been defeated!"); // Else if hero wins, announce it
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
                Thread.Sleep(Settings.Delay);

                TakeTurn(Player1Turn, turnNumber);

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

                Console.WriteLine(); // Create space in console to differentiate turns
            }
        }
        public void MakeHeroParty()
        {
            // Get player to input name for MainCharacter
            Console.WriteLine("What will the True Programmer's name be? ");
            string? name = Console.ReadLine();

            while (true) // Check name is valid
            {
                if (name == null || name == "")
                {
                    Console.WriteLine("Please input a valid name. ");
                    name = Console.ReadLine();
                }
                else
                {
                    break;
                }
            }

            Player1.Party.Add(new MainCharacter(name));
            // Add extra hero characters here
        }

        // Depending on current round, make relevant monster party.
        // Add rounds/monsters here as required
        public void MakeMonsterParty(int round)
        {
            Player2.Party.Clear(); // Make sure Monster party list is clear

            if (round == 1)
            {
                Player2.Party.Add(new Skeleton("SKELETON ONE"));
            }
            else if (round == 2)
            {
                Player2.Party.Add(new Skeleton("SKELETON ONE"));
                Player2.Party.Add(new Skeleton("SKELETON TWO"));
            }
            else if (round == 3)
            {
                Player2.Party.Add(new Skeleton("SKELETON ONE"));
                Player2.Party.Add(new TheUncodedOne("THE UNCODED ONE"));
                Player2.Party.Add(new Skeleton("SKELETON TWO"));
            }
            // New rounds go here by adding extra "else if's"
            // Number of rounds must be updated in Settings.cs
        }

        public void TakeTurn(bool player1Turn, int turnNumber)
        {
            // Reserve memory for objects
            IAction action;
            Player activePlayer;
            Character activeChar;
            int charIndex;

            // Find which character's turn it is
            if (player1Turn)
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

            // Announce beginning of turn and prompt for input
            Console.WriteLine($"It is {activeChar.Name}'s turn...");
            ActionType chosenAction = PickAction(activePlayer.isHuman);

            // Resolve input
            if (chosenAction == ActionType.Nothing) action = new NothingAction(activeChar);
            if (chosenAction == ActionType.Attack)  action = PickAttack(this, activeChar, activePlayer.isHuman);
            else                                    action = new NothingAction(activeChar); // Failsafe in case of error

            action.Execute(this);
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
                Console.WriteLine($"{character.Name} has been defeated!");
                party.Remove(character);
            }
        }

        // End game, declaring winner
        public void EndGame(bool heroWin)
        {
            if (heroWin)
            {
                Console.WriteLine($"The heroes have triumphed in their battle against The Uncoded One! The people are free to code again.");
            }
            else
            {
                Console.WriteLine($"The heroes have fallen against The Uncoded One! The reign of terror continues.");
            }
        }

        // List actions character can take
        public ActionType PickAction(bool isHuman)
        {
            // Print list of actions with index numbers for player to pick from
            int index = 0;
            foreach (ActionType action in Enum.GetValues(typeof(ActionType)))
            {
                index++;
                Console.WriteLine($"{index} - {action}");
            }

            int actionIndex = PickFromMenu(index, isHuman);

            ActionType chosenAction = (ActionType)actionIndex;

            return chosenAction;
        }

        public IAction PickAttack(TheFinalBattle game, Character character, bool isHuman)
        {
            Console.WriteLine("Pick an attack.");

            // Print list of attacks with index numbers for player to pick from
            int index = 0;
            foreach (AttackType attack in character.attackList)
            {
                index++;
                Console.WriteLine($"{index} - {attack}");
            }

            int chosenIndex = PickFromMenu(index, isHuman);

            // Pick target of attack
            Character target = PickTarget(isHuman);

            AttackType chosenAttackName = character.attackList[chosenIndex];

            // Add all possible attacks here
            if (chosenAttackName == AttackType.BoneCrunch) return new BoneCrunch(character, target);
            if (chosenAttackName == AttackType.Unraveling) return new Unraveling(character, target);
            else return new Punch(character, target);
        }

        public Character PickTarget(bool isHuman)
        {
            Console.WriteLine("Pick a target.");

            List<Character> targetParty = new List<Character>();
            if (Player1Turn)
            {
                targetParty = Player2.Party;
            }
            else
            {
                targetParty = Player1.Party;
            }

            // List available target
            int index = 0;
            foreach (Character target in targetParty)
            {
                index++;
                Console.WriteLine($"{index} - {target.Name}");
            }

            int chosenTarget = PickFromMenu(index, isHuman);

            return targetParty[chosenTarget];
        }

        public static int PickFromMenu(int max, bool isHuman)
        {
            int choice;

            if (isHuman)
            {
                choice = Convert.ToInt32(Console.ReadLine());

                while (true)
                {
                    if (choice > 0 && choice <= max)
                    {
                        break;
                    }
                    Console.WriteLine("Please a number from the available choices.");
                    choice = Convert.ToInt32(Console.ReadLine());
                }
            }
            else
            {
                Random random = new Random();
                choice = random.Next(max);
            }

            return choice - 1;
        }
    }
}
