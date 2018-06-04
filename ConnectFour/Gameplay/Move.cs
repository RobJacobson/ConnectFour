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
        public Color Token { get; }
        public int   Col   { get; }
        public int   Row   { get; }
        public int   Turn  { get; }

        // Constructs a new Move object
        public Move(Color token, int col, int row, int turn)
        {
            Token = token;
            Col   = col;
            Row   = row;
            Turn  = turn;
        }

        // Returns pretty-print text to represent move
        public override string ToString()
        {
            return $"{ Turn }: { Token } => ({ Col }, { Row })";
        }

        // Returns the console color that matches this token
        public ConsoleColor DisplayColor()
        {
            return (Token == Color.Red) ? ConsoleColor.Red : ConsoleColor.Yellow;
        }
    }
}
