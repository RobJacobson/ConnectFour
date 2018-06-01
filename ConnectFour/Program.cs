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
        static void Main(string[] args)
        {
            while (true)
            {
                // Define the two players competing against each other
                // Agent player1 = new HumanAgent(Token.Red);

                // Construct preprogrammed agents (always play same move, for debugging)
                // Agent player1 = new ProgrammedAgent(Token.Red, new int[] { 1, 6, 2, 6, 2, 5, 3, 5, 3, 6, 5, 4, 0, 1, 4, 3, 5, 1, 5, 0, 4 });
                // Agent player2 = new ProgrammedAgent(Token.Yellow, new int[] { 1, 0, 6, 2, 0, 1, 6, 2, 4, 1, 3, 0, 4, 5, 2, 6, 0, 3, 2, 3, 4 });

                // Construct random agents (no intelligence)
                AbstractAgent player1 = new RandomAgent(Color.Red);
                AbstractAgent player2 = new RandomAgent(Color.Yel);

                // Identify players
                Console.WriteLine($"New game: { player1 } vs { player2 }");
                Console.WriteLine();

                // Create new game on standard 7 x 6 board
                Game game = new Game(player1, player2, 7, 6);

                // Play until we get a winner
                (Color winner, Board board) = game.Start();

                // Display winner
                Console.WriteLine();
                if (winner == Color.None)
                {
                    Console.WriteLine("Tie game");
                }
                else
                {
                    Console.WriteLine($"Player { winner } wins!!!");
                }

                // Output each player's moves by column number
                //string p1 = String.Join(", ", board.MovesByPlayer(player1));
                //string p2 = String.Join(", ", board.MovesByPlayer(player2));
                //Console.WriteLine($"{player1} moves: {p1}");
                //Console.WriteLine($"{player2} moves: {p2}");

                // Pause before continuing
                Console.ReadLine();
            }

        }

    }
}
