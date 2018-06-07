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


        // Selects a random column from the columns with available moves
        public override int GetNextMoveDerived(Board board)
        {
            // Get list of available columns to play
            List<int> moves = board.GetAvailableMoves();

            // Return a random member of the list
            int move = Randomizer.Next(0, moves.Count);
            return moves[move];            
        }
    }
}
