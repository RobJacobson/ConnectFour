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
        public int TotalIterations { get; }

        public override int GetNextMoveDerived(Board board)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString() + $", Iterations: " + TotalIterations;
        }
    }

}
