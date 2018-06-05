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
        public MinimaxRandomAgent(Color player, int plyDepth, double decay) 
            : base(player, plyDepth, decay)
        {
        }

        // Returns a random integer in [-1000, 1000] as a heuristic
        public override int Heuristic(Board board, int column)
        {
            return Randomizer.Next(-1000, 1001);
        }
    }
}
