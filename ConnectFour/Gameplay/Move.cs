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

        // Constructs a new Move object
        public Move(Token token, int col, int score)
        {
            Token = token;
            Col   = col;
            Score = score;
        }

        // Returns pretty-print text to represent move
        public override string ToString()
        {
            return $"{ Token } => Col {Col} ({Score})";
        }

        // Returns the console color that matches this token
        public ConsoleColor DisplayColor()
        {
            return (Token == Token.Red) ? ConsoleColor.Red : ConsoleColor.Yellow;
        }

    }
}
