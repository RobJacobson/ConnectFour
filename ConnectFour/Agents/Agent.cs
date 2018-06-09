using System;
using System.Diagnostics;
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
        // The token color for this agent
        public Token Token { get; }

        // A shared random-number generator for all agents
        protected static Random Randomizer { get; private set; } = new Random();

        // Stopwatch for timing each agent's moves
        public Stopwatch Clock { get; }
      
        // Base constructor to assign the player's color
        public Agent(Token token)
        {
            Token = token;
            Clock = new Stopwatch();
        }


        // Requests and returns next column number to play from derived agent
        public Move GetNextMove(Board board)
        {
            Clock.Start();
            Move move = GetNextMoveDerived(board);
            Clock.Stop();
            return move;
        }


        // Abstract method for derived agent to return column of next move
        public abstract Move GetNextMoveDerived(Board board);


        // Return string for pretty-print output using derived class's name
        public override string ToString()
        {
            string type = this.GetType().Name;
            return $"Player {Token} ({type})";
        }


        // Counts the given player's tokens in the given array slice
        public int Count(Token player, IEnumerable<Token> slice)
        {
            int count = 0;
            foreach (Token token in slice)
            {
                if (token == player)
                {
                    count++;
                }
            }
            return count;
        }


        // Constructs a new RNG using the specified seed
        public static void Reseed(int seed)
        {
            Agent.Randomizer = new Random(seed);
        }
        

    }
}
