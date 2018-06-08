using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour.Gameplay
{
    public struct Point
    {
        public int Col { get; }
        public int Row { get; }

        public Point(int col, int row)
        {
            Col = col;
            Row = row;
        }

        public Point Move(int colDelta, int rowDelta)
        {
            return new Point(Col + colDelta, Row + rowDelta);
        }

        public override string ToString()
        {
            return $"({Col}, {Row})";
        }
    }
}
