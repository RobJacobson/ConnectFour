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
        const string SHOW = "Display each move? (y or n)";
        const string END  = "New game? (r to replay, n for new game, q to quit)";

        static void Main(string[] args)
        {
            // Extract commane line parameters
            string p1Type = args[0];
            string p2Type = args[1];

            // Determine whether to use verbose mode (printing each move)
            char verbose = PromptChar(SHOW, new char[] { 'y', 'n' });

            // Start main game-play loop
            Play(p1Type, p2Type, (verbose == 'y'));
        }

        private static void Play(string p1Type, string p2Type, bool verbose)
        {
            // Get the next random seed (lets us replay game with same seed)
            Random randomSeed = new Random();
            int seed = randomSeed.Next();

            // Loop for each new or replayed game
            while (true)
            {
                // Define the two agent types and seed their shared RNG
                // (This allows us to 
                Agent player1 = AgentFactory(p1Type, Color.Red);
                Agent player2 = AgentFactory(p2Type, Color.Yel);
                Agent.Reseed(seed);

                // Create new game on standard 7 x 6 board
                GameEngine game = new GameEngine(player1, player2, 7, 6);

                // Play until we get a winner
                Move winner = game.Start(verbose);
                ShowResult(winner);

                // Prompt for next game options (new game, replay, or quit)
                char keypress = PromptChar(END, new char[] { 'r', 'n', 'q' });
                Console.WriteLine();

                // Process 
                switch (keypress)
                {
                    case 'n': seed = randomSeed.Next(); break;
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
            } while (Int32.TryParse(response, out result));
            return result;
        }


        // Factory method to construct agent from command-line parameter
        private static Agent AgentFactory(string type, Color token)
        {
            switch (type)
            {
                case "Human":
                    return new HumanAgent(token);

                case "Random":
                    return new RandomAgent(token);

                case "Minimax":
                    int plies = PromptInt("   Ply depth for MiniMax:");
                    return new MinimaxAgent(token, plies);
            }
            throw new ArgumentException($"Invalid agent name: { type }");
        }

    }
}
