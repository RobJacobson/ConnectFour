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
        public int IterationsTurn { get; private set; }
        public int IterationsTotal { get; private set; }
        public int PlyDepth { get; }

        // Override base constructor
        public MinimaxAgent(Color player, int plyDepth) : base(player)
        {
            PlyDepth = plyDepth;
        }


        // Returns next move using MinMax search with varying heuristics
        public override int GetNextMoveDerived(Board board)
        {
            IterationsTurn = 0;
            throw new NotImplementedException();
        }


        public int Min(Board board, int depth)
        {
            throw new NotImplementedException();
        }


        // Unique heuristic to be implemented by each derived MinMaxAgent
        public abstract int Heuristic(Board board);


        // Show the number of MinMax iterations for each derived agent
        public override string ToString()
        {
            string iterations = $", { IterationsTurn }, { IterationsTotal }";
            return base.ToString() + iterations;
        }
    }

}
