using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Gameplay;

namespace ConnectFour.Agents
{
    abstract class MinimaxAgent : Agent
    {
        private int minmaxIterations = 0;

        // Override base constructor
        public MinimaxAgent(Color player) : base(player) { }

        public override int GetNextMoveDerived(Board board)
        {
            minmaxIterations++;
            throw new NotImplementedException();
        }


        // Unique implementation of heuristic for each derived MinMax agent
        public abstract int Heuristic(Board board);


        // Show the total number of MinMax iterations for each derived agent
        public override string ToString()
        {
            return base.ToString() + $", Iterations: " + minmaxIterations;
        }
    }

}
