using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectFour.Gameplay;

namespace ConnectFour.Agents
{
    // Abstract base class for agent
    public abstract class Agent
    {
        // The color of this agent
        public Color Color { get; }

        // A list of every move (by column number)
        public List<int> Moves { get; }


        // Base constructor to assign the player's color
        public Agent(Color player)
        {
            Color = player;
            Moves = new List<int>();
        }


        // Base method for initiating next move and storing result
        public int GetNextMove(Board board)
        {
            int column = GetNextMoveDerived(board);
            Moves.Add(column);
            return column;
        }


        // Abstract method for derived agent to return column of next move
        public abstract int GetNextMoveDerived(Board board);


        // Return string for pretty-print output using derived class's name
        public override string ToString()
        {
            return $"Player { Color } ({ this.GetType().Name })";
        }


        // Counts the given player's tokens in the given array slice
        public int Count(Color player, IEnumerable<Color> slice)
        {
            int count = 0;
            foreach (Color token in slice)
            {
                if (token == player)
                {
                    count++;
                }
            }
            return count;
        }

    }
}
