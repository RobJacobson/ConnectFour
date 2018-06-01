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
        public Color Color { get; }

        // Base constructor to assign the player's color
        public AbstractAgent(Color player)
        {
            Color = player;
        }

        // Abstract method for derived agent to return column of next move
        public abstract int GetNextMove(Board board);

        // Return string for pretty-print output using derived class's name
        public override string ToString()
        {
            return $"Player { Color } ({ this.GetType().Name })";
        }

    }
}
