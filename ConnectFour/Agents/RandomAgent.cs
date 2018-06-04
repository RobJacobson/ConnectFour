using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Gameplay;

namespace ConnectFour.Agents
{

    // An agent without any intelligence who just guesses random moves
    class RandomAgent : Agent
    {
        // Call base constructor
        public RandomAgent(Color player) : base(player) { }


        // Selects a random column to play based on the board's state
        public override int GetNextMoveDerived(Board board)
        {
            // Guess a random number in range [0, board.Cols - 1]
            int col = Agent.Randomizer.Next(0, board.Width);

            // Continue guessing if initial guess was invalid (column full)
            while (board.ColHeight[col] >= board.Height)
            {
                col = Agent.Randomizer.Next(0, board.Width);
            }

            return col;
        }
    }
}
