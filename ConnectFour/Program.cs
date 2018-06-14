using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Gameplay;
using ConnectFour.Agents;
using System.Collections;
using System.Diagnostics;

namespace ConnectFour
{
    class Program
    {
        static int gameCount = 0;

        const string MODE      = "Select game mode (S for single game, T for tournament)";
        const string NUMBER    = "    Select number of iterations:";
        const string VERBOSE   = "    Display each move? (Y or N)";
        const string END_GAME  = "    New game? (R to replay, N for new game, M for main menu, Q to quit)";
        const string END_TEST  = "    New tournament? (N for new tournament, M for main menu, Q to quit)";
        const string PLY_DEPTH = "    Ply depth for MiniMax:";
        const string CARET     = "    > ";
        const string SELECT_AGENT_MESSAGE =
@"Select agent type for {0}:
    1. Random-move agent
    2. Minimax agent, no heuristic
    3. Minimax agent, random-number heuristic
    4. Minimax agent, 1-nn heuristic
    5. Minimax agent, 2-nn heuristic
    6. Minimax agent, 3-nn heuristic
    7. Minimax agent, goal-directed heuristic
    8. Puny human";


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
            string response = PromptString(MODE, new string[] { "s", "t" });

            // Prompt for type of each agent and create agents
            AbstractAgent agent1 = SelectAgent("Red", Token.Red);
            AbstractAgent agent2 = SelectAgent("Yellow", Token.Yel);

            // Start the appropriate game mode
            if (response == "t")
            {
                PlayBatch(agent1, agent2);
            }
            else
            {
                Output.ScrollUp(25);
                PlaySingle(agent1, agent2);
            }
        }

        private static void PlayBatch(AbstractAgent agent1, AbstractAgent agent2)
        {
            // Prompt for the number of batch repetitions
            int rounds = PromptInt(NUMBER);

            // Track the number of wins per player
            int redWins = 0;
            int yelWins = 0;
            int draws = 0;
            int moves = 0;

            // Create and start timer
            var sw = Stopwatch.StartNew();

            // Play 'count' number of games without user input
            for (int round = 0; round < rounds; round++)
            {
                // Start new game and alternate Red and Yellow as player 1
                GameEngine game;
                if (round % 2 == 0)
                {
                    game = PlayGame(agent1, agent2, false);
                }
                else
                {
                    game = PlayGame(agent2, agent1, false);
                }

                // Count the number of victories per side
                if (game.Winner == null)
                {
                    draws++;
                }
                else if (game.Winner.Token == Token.Red)
                {
                    redWins++;
                }
                else
                {
                    yelWins++;
                }
                moves += game.Moves.Count;

            }

            // Report overall iterations and time
            sw.Stop();
            Console.WriteLine();
            Console.WriteLine($"Iterations:    {rounds}");
            Console.WriteLine($"Running time:  {sw.ElapsedMilliseconds} ms");
            Console.WriteLine($"Average time:  {((double)sw.ElapsedMilliseconds) / rounds} ms");
            Console.WriteLine($"Average turns: {((double)moves) / rounds}");
            Console.WriteLine();

            // Print the summary results
            Console.WriteLine($"\t\tRed\t\t\tYellow\t\t\tNone");

            // Output number of games won
            Console.Write($"Games won:\t");
            Console.Write($"{redWins}\t({((double)redWins) / rounds:p1})\t\t");
            Console.Write($"{yelWins}\t({((double)yelWins) / rounds:p1})\t\t");
            Console.Write($"{draws  }\t({((double)draws)   / rounds:p1})\t\t");


            int mmCount1 = 0;
            if (agent1 is MinimaxAgent)
            {
                mmCount1 = (agent1 as MinimaxAgent).MinimaxCount;
            }

            int mmCount2 = 0;
            if (agent2 is MinimaxAgent)
            {
                mmCount2 = (agent2 as MinimaxAgent).MinimaxCount;
            }

            // Output number of average iterations
            Console.Write($"Avg. Minimax Steps\t");
            Console.Write($"{mmCount1 / (double)rounds}\t\t");
            Console.Write($"{mmCount2 / (double)rounds}");
            Console.WriteLine();

            // Output average duration of each game
            Console.Write($"Avg. Duration\t");
            Console.Write($"{(agent1.Clock.ElapsedMilliseconds * 1000.0 / rounds)} us\t");
            Console.Write($"{(agent2.Clock.ElapsedMilliseconds * 1000.0 / rounds)} us");
            Console.WriteLine();

            MainMenu();

        }

        private static void PlaySingle(AbstractAgent agent1, AbstractAgent agent2)
        {
            // Get next random seed (we can replay game by reusing this seed)
            var randomSeedGenerator = new Random();
            int seed = randomSeedGenerator.Next();

            // Loop until user selects "quit" or "main menu"
            while (true)
            {
                // Create a new RNG for agents using the selected seed
                AbstractAgent.Reseed(seed);
                PlayGame(agent1, agent2, true);

                // Play another game?
                string response = PromptString(END_GAME, new string[] { "r", "n", "m", "q" });
                switch (response)
                {
                    case "r":
                        break;

                    case "n":
                        seed = randomSeedGenerator.Next();
                        break;

                    case "m":
                        MainMenu();
                        break;

                    case "q":
                        return;
                }

            }
        }

        private static GameEngine PlayGame(AbstractAgent agent1, AbstractAgent agent2, bool verbose)
        {
            // Create and start new game on standard 7 x 6 board
            GameEngine game = new GameEngine(agent1, agent2, 7, 6, verbose);

            // Start one game and play until it ends
            Move winner = game.StartNewGame();

            // Show winning board if it wasn't shown in GameEngine
            if (!verbose)
            {
                //                Output.ShowMove(game.Board, game.Moves.Last());

            }

            // Display message
            gameCount++;
            if (gameCount % 1000 == 0)
            {
                if (winner == null)
                {
                    Console.WriteLine($"Game {gameCount}: Tie game");
                }
                else
                {
                    Console.WriteLine($"Game {gameCount}: Player { winner.Token } wins in move {game.Moves.Count}!");
                }

            }
            return game;
        }


        // Shows prompt, waits for valid keypress, and returns keypress
        private static string PromptString(string prompt, string[] expected)
        {
            string response;
            Console.WriteLine(prompt);
            do
            {
                Console.Write(CARET);
                response = Console.ReadLine().ToLower();
            } while (!expected.Contains(response));
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
                    plies = PromptInt(PLY_DEPTH);
                    return new MinimaxGoalAgent(token, plies);

                case 8:
                    return new HumanAgent(token);
            }
            throw new ArgumentException($"Invalid agent number");
        }

    }
}
