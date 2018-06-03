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
        public Color Token { get; }
        public int   Col   { get; }
        public int   Row   { get; }
        public int   Turn  { get; }

        // Constructs a new Move struct
        public Move(Color token, int col, int row, int turn)
        {
            Token = token;
            Col = col;
            Row = row;
            Turn = turn;
        }

        public override string ToString()
        {
            return $"{ Turn }: { Token } => ({ Col }, { Row })";
        }

    }
}
