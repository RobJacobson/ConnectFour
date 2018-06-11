using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Gameplay;

namespace ConnectFour.Agents
{
    // Class to model a Minimax heuristic  that just picks a random value
    class MinimaxRandomAgent : MinimaxAgent
    {
        public MinimaxRandomAgent(Token player, int plyDepth) 
            : base(player, plyDepth)
        {
        }

        // Returns a random integer in [-1000000, 1000000] as a heuristic
        public override int Heuristic(Board board, int column, Token player)
        {
            return Randomizer.Next(-1000000, 1000001);
        }
    }
}
