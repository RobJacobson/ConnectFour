using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Gameplay;

namespace ConnectFour
{
    // Abstract base class for agent
    public abstract class Agent
    {
        // The color of this agent
        public Color Tok { get; }

        // Base constructor to assign the player's color
        public Agent(Color color)
        {
            Tok = color;
        }

        // Abstract method for custom logic to determine this agent's next move
        public abstract Move GetNextMove(Board board);

        // Return string for pretty-print output using derived class's name
        public override string ToString()
        {
            return $"Player { Tok } ({ this.GetType().Name })";
        }

    }
}
