using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Gameplay;

namespace ConnectFour.Agents
{
    // Determines next move by promting human player to select a column
    public class HumanAgent : Agent
    {
        // Call base constructor
        public HumanAgent(Color player) : base(player) { }

        // Override GetNextMove with custom logic
        public override int GetNextMoveDerived(Board board)
        {
            // Prompt for input
            Console.Write($"{base.Token,-8}> ");

            // Read input and repeat if input was invalid (not a number)
            int col = 0;
            while (!int.TryParse(Console.ReadLine(), out col))
            {
                Console.Write($"{base.Token,-8}> ");
            }

            return col;
        }

    }
}
