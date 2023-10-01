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
            Player1 = new Player(false);
            Player2 = new Player(false);
        }

        public void Run() // Run the game
        {
            bool heroWin = false;
            MakeHeroParty();

            // Keep playing through rounds until hero wins or loses
            for (int round = 1; round <= Settings.NumRounds; round++)
            {
                MakeMonsterParty(round);
                heroWin = PlayRound();
                if (!heroWin)
                    break;

                Console.WriteLine("The enemy party has been defeated!");
            }
            EndGame(heroWin); // Declare winner
        }

        public bool PlayRound() // A round consists of multiple turns, ending when a party has no characters left.
        {
            int turnNumber = 0;

            while (true) // Round Loop
            {
                Thread.Sleep(1000);

                TakeTurn(Player1Turn, turnNumber);
                StatusCheck(Player1.Party);
                StatusCheck(Player2.Party);

                // Check if the hero has won the round
                if (Player1.Party.Count == 0)
                {
                    return false;
                }
                else if (Player2.Party.Count == 0)
                {
                    return true;
                }

                Player1Turn = !Player1Turn; // Flip who's turn it is

                // Increment turn number if this is the end of Player2's turn
                if (!Player1Turn)
                    turnNumber++;

                Console.WriteLine(); // Create space to differentiate turns
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

            // Create and add TOG to party
            Player1.Party.Add(new MainCharacter(name));
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
        }

        public void TakeTurn(bool player1Turn, int turnNumber)
        {
            IAction action; // Prepare memory for action

            // Get current player to pick action for active character
            if (player1Turn)
            {
                int characterIndex = turnNumber % Player1.Party.Count;
                action = GetAction(this, Player1.Party[characterIndex], Player1.isHuman);
            }
            else
            {
                int characterIndex = turnNumber % Player2.Party.Count;
                action = GetAction(this, Player2.Party[characterIndex], Player2.isHuman);
            }

            action.Execute(this); // Execute chosen action
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

        public IAction GetAction(TheFinalBattle game, Character character, bool isHuman)
        {
            Console.WriteLine($"It is {character.Name}'s turn...");
            ActionType chosenAction = PickAction(isHuman);

            // Resolve chosen action
            if (chosenAction == ActionType.Nothing) return new NothingAction(character);
            if (chosenAction == ActionType.Attack)  return PickAttack(game, character, isHuman);

            // Code should not be able to get to here, but if it somehow does, do 'nothing' action.
            return new NothingAction(character);
        }

        // List actions character can take
        public ActionType PickAction(bool isHuman)
        {
            // Print list of available actions. Also add actions to easily accessible list.
            List<string> actionList = new List<string>();
            foreach (ActionType action in Enum.GetValues(typeof(ActionType)))
            {
                Console.WriteLine($"- {action}");

                string tempAction = action.ToString().ToLower(); // Ensure actions are lower case to easier match user inputs later on
                actionList.Add(tempAction);
            }

            string? chosenAction; // Prepare chosenAction string for player/computer to fill in below
            if (isHuman)
            {
                chosenAction = HumanPickFromList(actionList);
            }
            else // If computer player, pick random action
            {
                chosenAction = ComputerPickFromList(actionList);
            }

            // Player or computer should have by now input a valid action. Resolve action.
            if (chosenAction == "attack") return ActionType.Attack;
            else                          return ActionType.Nothing;
        }

        public IAction PickAttack(TheFinalBattle game, Character character, bool isHuman)
        {
            Console.WriteLine("Pick an attack.");

            // Print list of available attacks. Also add attacks to easily accessible list.
            List<string> attackList = new List<string>();
            foreach (AttackType attack in character.attackList)
            {
                Console.WriteLine($"- {attack}");

                string tempAttack = attack.ToString().ToLower(); // Ensure actions are lower case to easier match user inputs later on
                attackList.Add(tempAttack);
            }

            string? chosenAttack; // Prepare chosenAction string for player/computer to fill in below
            if (isHuman)
            {
                chosenAttack = HumanPickFromList(attackList);
            }
            else // If computer player, pick random action
            {
                chosenAttack = ComputerPickFromList(attackList);
            }

            // Pick target of attack
            Character target = PickTarget(isHuman);

            if (chosenAttack == "bonecrunch") return new BoneCrunch(character, target);
            else                              return new Punch(character, target);
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
            List<string> targetList = new List<string>();
            foreach (Character target in targetParty)
            {
                Console.WriteLine($"- {target.Name}");

                string tempTarget = target.ToString().ToLower(); // Ensure actions are lower case to easier match user inputs later on
                targetList.Add(tempTarget);
            }

            // Let player pick their target
            string chosenTarget;

            if (isHuman)
            {
                chosenTarget = HumanPickFromList(targetList);
            }
            else
            {
                chosenTarget = ComputerPickFromList(targetList);
            }

            int targetIndex = targetList.IndexOf(chosenTarget);

            return targetParty[targetIndex];
        }

        // Input a list (actions, attacks etc.). Return a string indicating human's choice.
        public static string HumanPickFromList(List<string> list)
        {
            string? chosenAction = Console.ReadLine();

            // Prevent chosenAction being null
            while (chosenAction == null)
            {
                Console.WriteLine("Please input a valid action.");
                chosenAction = Console.ReadLine();
            }

            // Ensure chosenAction is valid
            while (true)
            {
                chosenAction = chosenAction.ToLower();
                if (list.Contains(chosenAction)) break;

                // If code reached here, input was invalid
                Console.WriteLine("That is not a valid choice. Pick a choice from the list.");
                chosenAction = Console.ReadLine();
            }

            return chosenAction;
        }

        // Input a list (actions, attacks etc.). Return a string indicating computer's choice.
        public static string ComputerPickFromList(List<string> list)
        {
            Thread.Sleep(1200); // Delay computer player choice for easier reading

            // Pick a random action for character from available list
            Random random = new Random();
            int randomIndex = random.Next(list.Count);
            string chosenAction = list[randomIndex];

            return chosenAction;
        }
    }
}
