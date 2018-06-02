using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour.Gameplay
{
    // Represents one square on game board with an empty or color token
    public class Square
    {
        // Column coordinate
        public int Col { get; }

        // Row coordinate
        public int Row { get; }

        // Index of upper-right diagonal
        public int UpperRightDiag { get; }

        // Index of upper-left 
        public int UpperLeftDiag { get; }

        // Token color
        public Color Token { get; set; }

        // Constructor for new square; called in Board con
        public Square(int col, int row, Color token)
        {
            Col = col;
            Row = row;
            Token = token;
        }
    }
}
