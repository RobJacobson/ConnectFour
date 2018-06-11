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

        const string MODE      = "Select game mode (S for single game, T for tournament)";
        const string NUMBER    = "    Select number of iterations:";
        const string VERBOSE   = "    Display each move? (Y or N)";
        const string END_GAME  = "    New game? (R to replay, N for new game, M for main menu, Q to quit)";
        const string END_TEST  = "    New tournament? (N for new tournament, M for main menu, Q to quit)";
        const string PLY_DEPTH = "    Ply depth for MiniMax:";
        const string CARET     = "    > ";

        const double MINIMAX_DECAY = 1;

        const string SELECT_AGENT_MESSAGE =
@"
Select agent type for {0}:
    1. Random-move agent
    2. Minimax agent, no heuristic
    3. Minimax agent, random-number heuristic
    4. Minimax agent, 1-nn heuristic
    5. Minimax agent, 2-nn heuristic
    6. Minimax agent, 3-nn heuristic
    7. Puny human";


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
            char key = PromptChar(MODE, new char[] { 's', 't' });
            Console.WriteLine();

            // Prompt for type of each agent and create agents
            AbstractAgent agent1 = SelectAgent("Red", Token.Red);
            AbstractAgent agent2 = SelectAgent("Yellow", Token.Yel);

            // Start the appropriate game mode
            if (key == 't')
            {
                PlayBatch(agent1, agent2);
            }
            else
            {
                PlaySingle(agent1, agent2);
            }
        }

        private static void PlayBatch(AbstractAgent agent1, AbstractAgent agent2)
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
                ShowResult(game.Board, game.Moves, winner);

                // Count the number of victories per side
                if (winner == null)
                {
                    draw++;
                }
                else if (winner.Token == Token.Red)
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

        private static void PlaySingle(AbstractAgent agent1, AbstractAgent agent2)
        {
            // Get next random seed (we can replay game by reusing this seed)
            var randomSeedGenerator = new Random();
            int seed = randomSeedGenerator.Next();

            // Loop until user selects "quit"
            while (true)
            {
                // Seed the agent's RNG with the current seed
                AbstractAgent.Reseed(seed);

                // Create and start new game on standard 7 x 6 board
                GameEngine game = new GameEngine(agent1, agent2, 7, 6, true);

                // Start one game and play until it ends
                Move winner = game.Start();
                ShowResult(game.Board, game.Moves, winner);

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
        private static void ShowResult(Board board, List<Move> moves, Move winner)
        {
            // Show board
            Output.ShowMove(board, moves.Last());

            // Print final turn number
            Console.Write(board.NumTokens + ": ");

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
            Console.WriteLine(prompt);
            do
            {
                Console.Write(CARET);
                response = Char.ToLower(Console.ReadKey().KeyChar);
            } while (!validChars.Contains(response));
            Console.WriteLine();
            Console.WriteLine();
            return response;
        }


        // Shows prompt and returns integer from keyboard
        private static int PromptInt(string prompt)
        {
            string response;
            int result;
            Console.WriteLine(prompt);
            do
            {
                Console.Write(CARET);
                response = Console.ReadLine();
            } while (!Int32.TryParse(response, out result));
            Console.WriteLine();
            return result;
        }


        // Prompts user to select agent, then creates and returns new agent
        private static AbstractAgent SelectAgent(string player, Token token)
        {
            // Display "select agent" message and wait for input
            string prompt = String.Format(SELECT_AGENT_MESSAGE, player);

            // Prompt and get value from user
            int agentNo = PromptInt(prompt);

            // Create and return a new agent using the AgentFactory method
            AbstractAgent agent = AgentFactory(agentNo, token);
            return agent;
        }


        // Factory method to return agent from command-line parameter
        private static AbstractAgent AgentFactory(int agentNo, Token token)
        {
            int plies = 0;
            switch (agentNo)
            {
                case 1:
                    return new RandomAgent(token);

                case 2:
                    plies = PromptInt(PLY_DEPTH);
                    return new MinimaxAgent(token, plies);

                case 3:
                    plies = PromptInt(PLY_DEPTH);
                    return new MinimaxRandomAgent(token, plies);

                case 4:
                    plies = PromptInt(PLY_DEPTH);
                    return new MinimaxKnnAgent(token, plies, 1);

                case 5:
                    plies = PromptInt(PLY_DEPTH);
                    return new MinimaxKnnAgent(token, plies, 2);

                case 6:
                    plies = PromptInt(PLY_DEPTH);
                    return new MinimaxKnnAgent(token, plies, 3);

                case 7:
                    return new HumanAgent(token);
            }
            throw new ArgumentException($"Invalid agent number");
        }

    }
}
