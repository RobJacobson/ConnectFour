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
        // The color of this agent (red or black)
        public Token Color { get; }


        // Constructor to assign the player's color
        public Agent(Token color)
        {
            Color = color;
        }


        // Given current state, calculates and returns agent's next move
        public abstract Move GetNextMove(Board board);


        // Return string for pretty-print output using derived class name
        public override string ToString()
        {
            return $"Player { Color } ({ this.GetType().Name })";
        }

    }
}
