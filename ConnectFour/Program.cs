using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Gameplay;
using ConnectFour.Agents;
using System.Collections;

namespace ConnectFour
{
    class Program
    {


        const string SHOW_MOVE_MESSAGE = "Display each move? (y or n)";
        const string END_GAME_MESSAGE = "New game? (r to replay, n for new game, a to repick agent, q to quit)";

        static string[] SELECT_AGENT_MESSAGE =
        {
            "",
            "  Select player agent:",
            "    1. Random-move agent",
            "    2. Minimax agent, no heuristic",
            "    3. Minimax agent, random heuristic",
            "    4. Puny human",

        };


        static void Main(string[] args)
        {

            // Determine whether to use verbose mode (printing each move)
            char verbose = PromptChar(SHOW_MOVE_MESSAGE, new char[] { 'y', 'n' });
            Console.WriteLine();

            // Prompt for type of each agent and create agents
            Agent agent1 = SelectAgent("Red", Color.Red);
            Agent agent2 = SelectAgent("Yellow", Color.Yel);
            
            // Start main game-play loop
            Play(agent1, agent2, (verbose == 'y'));
        }


        private static void Play(Agent agent1, Agent agent2, bool verbose)
        {
            // Get next random seed (lets us replay game by reusing same seed)
            Random randomSeed = new Random();
            int seed = randomSeed.Next();

            // Loop for each new or replayed game
            while (true)
            {
                // Update the agents' random number generator with current seed
                Agent.Reseed(seed);

                // Create and start new game on standard 7 x 6 board
                GameEngine game = new GameEngine(agent1, agent2, 7, 6, verbose);

                // Play until we get a winner
                Move winner = game.Start();
                ShowResult(winner);

                // Prompt for next game options (relay, new game, or quit)
                char keypress = PromptChar(END_GAME_MESSAGE, new char[] { 'r', 'n', 'a', 'q' });
                Console.WriteLine();

                // Process user input at the end of each game
                switch (keypress)
                {
                    case 'r': break;
                    case 'n': seed = randomSeed.Next(); break;
                    case 'a':
                        agent1 = SelectAgent("Red", Color.Red);
                        agent2 = SelectAgent("Yellow", Color.Yel);
                        seed = randomSeed.Next();
                        break;
                    case 'q': return;
                }

            }
        }


        // Display winner and prompt for next game options
        private static void ShowResult(Move winner)
        {
            // Display winner
            if (winner == null)
            {
                Console.WriteLine("Tie game");
            }
            else
            {
                Console.WriteLine($"Player { winner.Token } wins!");
            }
            Console.WriteLine();
        }


        // Shows prompt, waits for valid keypress, and returns keypress
        private static char PromptChar(string prompt, char[] validChars)
        {
            char response;
            do
            {
                Console.Write(prompt + "  > ");
                response = Char.ToLower(Console.ReadKey().KeyChar);
                Console.WriteLine();
            } while (!validChars.Any(c => c == response));
            return response;
        }


        // Shows prompt and returns integer from keyboard
        private static int PromptInt(string prompt)
        {
            string response;
            int result;
            do
            {
                Console.Write(prompt + "  > ");
                response = Console.ReadLine();
            } while (!Int32.TryParse(response, out result));
            return result;
        }


        // Prompts user to select agent, then creates and returns new agent
        private static Agent SelectAgent(string player, Color token)
        {
            // Display initial message to select agent for given player
            Console.WriteLine($"Enter Player {player} paramaters:");
            foreach (string line in SELECT_AGENT_MESSAGE)
            {
                Console.WriteLine(line);
            }
            Console.WriteLine();

            // Display a cursor and wait for user input within range [1-4]
            int input;
            do
            {
                input = PromptInt("    ");
            } while (input < 1 || input > 4);
            Console.WriteLine();

            // Create and return a new agent using the AgentFactory method
            Agent agent = AgentFactory(input, token);
            return agent;
        }


        // Factory method to return 5agent from command-line parameter
        private static Agent AgentFactory(int number, Color token)
        {
            int plies = 0;
            switch (number)
            {
                case 1:
                    return new RandomAgent(token);

                case 2:
                    plies = PromptInt("   Ply depth for MiniMax:");
                    return new MinimaxAgent(token, plies, 0.99);

                case 3:
                    plies = PromptInt("   Ply depth for MiniMax:");
                    return new MinimaxRandomAgent(token, plies, 0.99);

                case 4:
                    return new HumanAgent(token);
            }
            throw new ArgumentException($"Invalid agent number");
        }

    }
}
