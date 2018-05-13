using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Gameplay;
using ConnectFour.Agents;

namespace ConnectFour
{
    class Program
    {
        static void Main(string[] args)
        {
            // Define the two players competing against each other
            //            Agent player1 = new HumanAgent(Token.Red);
            Agent player1 = new RandomAgent(Token.Red);
            Agent player2 = new RandomAgent(Token.Yellow);

            // Identify players
            Console.WriteLine($"New game: { player1 } vs { player2 }");
            Console.WriteLine();

            // Create new game on standard 7 x 6 board
            Game game = new Game(player1, player2, 7, 6);

            // Play until we get a winner
            Token winner = game.Start();

            // Wait before exiting
            Console.WriteLine();
            Console.WriteLine($"Player { winner } wins!!!");
            Console.ReadLine();

        }
    }
}
