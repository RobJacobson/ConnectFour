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

        const string MODE = "Select game mode (b for batch mode, s for single game)";
        const string NUMBER = "Select number of iterations:";
        const string VERBOSE = "Display each move? (y or n)";
        const string END_GAME = "New game? (r to replay, n for new game, m for main menu, q to quit)";
        const string END_TEST = "Run test again? (r to run test, m for main menu, q to quit)";

        const double MINIMAX_DECAY = 1;

        static string[] SELECT_AGENT_MESSAGE =
        {
            "    Select player type:",
            "      1. Random-move agent",
            "      2. Minimax agent, no heuristic",
            "      3. Minimax agent, random-number heuristic",
            "      4. Minimax agent, nearest-neighbor heuristic",
            "      4. Puny human",
            ""
        };


        static void Main(string[] args)
        {
            MainMenu();

            // Pause before exiting
            Console.Write("Press enter to continue");
            Console.ReadLine();
        }

        private static void MainMenu()
        {
            // Prompt for game mode (batch or single-game)
            char key = PromptChar(MODE, new char[] { 'b', 's' });
            Console.WriteLine();

            // Prompt for type of each agent and create agents
            Agent agent1 = SelectAgent("Red", Color.Red);
            Agent agent2 = SelectAgent("Yellow", Color.Yel);

            // Start the appropriate game mode
            if (key == 'b')
            {
                PlayBatch(agent1, agent2);
            }
            else
            {
                PlaySingle(agent1, agent2);
            }
        }

        private static void PlayBatch(Agent agent1, Agent agent2)
        {
            // Prompt for the number of batch repetitions
            int reps = PromptInt(NUMBER);

            // Track the number of wins per player
            int redWins = 0;
            int yelWins = 0;
            int draw = 0;

            // Play 'count' number of games without user input
            for (int rep = 0; rep < reps; rep++)
            {
                // Create and start new game on standard 7 x 6 board
                var game = new GameEngine(agent1, agent2, 7, 6, false);
                Console.Write("Game {rep}: ");

                // Start one game and play until it ends
                Move winner = game.Start();
                ShowResult(winner);

                // Count the number of victories per side
                if (winner == null)
                {
                    draw++;
                }
                else if (winner.Token == Color.Red)
                {
                    redWins++;
                }
                else
                {
                    yelWins++;
                }

            }

            // Print the summary results
            Console.WriteLine($"Red wins: {redWins}, Yellow wins: {yelWins}, draws: {draw}");
            Console.WriteLine();
        }

        private static void PlaySingle(Agent agent1, Agent agent2)
        {
            // Get next random seed (we can replay game by reusing this seed)
            var randomSeedGenerator = new Random();
            int seed = randomSeedGenerator.Next();

            // Loop until user selects "quit"
            while (true)
            {
                // Seed the agent's RNG with the current seed
                Agent.Reseed(seed);

                // Create and start new game on standard 7 x 6 board
                GameEngine game = new GameEngine(agent1, agent2, 7, 6, true);

                // Start one game and play until it ends
                Move winner = game.Start();
                ShowResult(winner);

                // Play another game?
                char key = PromptChar(END_GAME, new char[] { 'r', 'n', 'm', 'q' });
                switch (key)
                {
                    case 'r':
                        break;

                    case 'n':
                        seed = randomSeedGenerator.Next();
                        break;

                    case 'm':
                        MainMenu();
                        break;

                    case 'q':
                        return;
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
            Console.Write(prompt + "  > ");
            do
            {
                response = Char.ToLower(Console.ReadKey().KeyChar);
            } while (!validChars.Contains(response));
            Console.WriteLine();
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
            Console.WriteLine();
            return result;
        }


        // Prompts user to select agent, then creates and returns new agent
        private static Agent SelectAgent(string player, Color token)
        {
            // Display "select agent" message and wait for input
            Console.WriteLine($"Enter Player {player} paramaters:");
            foreach (string line in SELECT_AGENT_MESSAGE)
            {
                Console.WriteLine(line);
            }
            char input = PromptChar("    ", new char[] { '1', '2', '3', '4', '5' });

            // Create and return a new agent using the AgentFactory method
            Agent agent = AgentFactory(input, token);
            return agent;
        }


        // Factory method to return agent from command-line parameter
        private static Agent AgentFactory(char number, Color token)
        {
            int plies = 0;
            switch (number)
            {
                case '1':
                    return new RandomAgent(token);

                case '2':
                    plies = PromptInt("     Ply depth for MiniMax:");
                    return new MinimaxAgent(token, plies);

                case '3':
                    plies = PromptInt("     Ply depth for MiniMax:");
                    return new MinimaxRandomAgent(token, plies);

                case '4':
                    plies = PromptInt("     Ply depth for MiniMax:");
                    return new MinimaxKnnAgent(token, plies);

                case '5':
                    return new HumanAgent(token);
            }
            throw new ArgumentException($"Invalid agent number");
        }

    }
}
