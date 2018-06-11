using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour.Gameplay
{
    // Class to represent each move in a game
    public class Move
    {
        // Move's token color, column number, row number and turn number
        public Token Token { get; }
        public int   Col   { get; }
        public int   Score { get; }
        public Move  Next  { get; }

        // Constructs a new Move object
        public Move(Token token, int col, int score, Move next)
        {
            Token = token;
            Col   = col;
            Score = score;
            Next  = next;
        }

        // Returns textual representation of move
        public override string ToString()
        {
            string result = $"{ Token } => Col {Col} ({ Score:n0})";
            if (Next != null)
            {
                result += Environment.NewLine + Next.ToString();
            }
            return result;
        }

        // Returns the console color that matches this token
        public ConsoleColor DisplayColor()
        {
            return (Token == Token.Red) ? ConsoleColor.Red : ConsoleColor.Yellow;
        }

    }
}
