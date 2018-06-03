using ConnectFour.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour.Agents
{
    // Represents an agent with a preprogrammed list of moves (for testing)
    class ProgrammedAgent : Agent
    {
        Queue<int> moves;

        public ProgrammedAgent(Color player, params int[] moves) : base(player)
        {
            this.moves = new Queue<int>(moves);
        }

        public override int GetNextMoveDerived(Board board)
        {
            return moves.Dequeue();
        }

    }
}
