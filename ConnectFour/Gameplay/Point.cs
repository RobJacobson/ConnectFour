using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour.Gameplay
{
    struct Point
    {
        public sbyte Col { get; }

        public sbyte Row { get; }

        public Point(int col, int row)
        {
            Col = (sbyte)col;
            Row = (sbyte)row;
        }

        public Point Move(Point delta)
        {
            return new Point(Col + delta.Col, Row + delta.Row);
        }
    }
}
