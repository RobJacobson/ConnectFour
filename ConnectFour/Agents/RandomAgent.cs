using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Gameplay;

namespace ConnectFour.Agents
{

    // An agent without any intelligence who just guesses random moves
    class RandomAgent : AbstractAgent
    {
        // Call base constructor
        public RandomAgent(Token player) : base(player) { }


        // Selects a random column from the columns with available moves
        public override Move GetNextMoveDerived(Board board)
        {
            // Get list of all available columns to play
            List<int> moves = board.GetAvailableMoves();

            // Return a random member of the list
            int r = Randomizer.Next(0, moves.Count);
            return new Move(Token, r, 0);            
        }
    }
}
