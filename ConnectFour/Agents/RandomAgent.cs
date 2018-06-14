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
            // Guess a random column until we get a non-full column
            int move;
            do
            {
                move = Randomizer.Next(0, board.Width);
            } while (board.ColHeight[move] == board.Height);

            // Return this choice as our move
            return new Move(this.Token, move, 0, null);
        }
    }
}
