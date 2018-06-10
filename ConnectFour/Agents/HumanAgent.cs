using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Gameplay;

namespace ConnectFour.Agents
{
    // Determines next move by promting human player to select a column
    public class HumanAgent : AbstractAgent
    {
        // Call base constructor
        public HumanAgent(Token player) : base(player) { }

        // Override GetNextMove with custom logic
        public override Move GetNextMoveDerived(Board board)
        {
            // Prompt the user to enter column number
            int col = 0;
            do
            {
                Console.Write($"{base.Token,-8}> ");
            } while (!int.TryParse(Console.ReadLine(), out col));

            // Return that move
            return new Move(Token, col, 0);
        }

    }
}
