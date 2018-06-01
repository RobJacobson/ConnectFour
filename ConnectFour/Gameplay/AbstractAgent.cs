using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Gameplay;

namespace ConnectFour
{
    // Abstract base class for agent
    public abstract class AbstractAgent
    {
        // The color of this agent
        public Color Player { get; }

        // Base constructor to assign the player's color
        public AbstractAgent(Color player)
        {
            Player = player;
        }

        // Abstract method for custom logic to determine this agent's next move
        public abstract Move GetNextMove(Board board);

        // Return string for pretty-print output using derived class's name
        public override string ToString()
        {
            return $"Player { Player } ({ this.GetType().Name })";
        }

    }
}
