//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace ConnectFour.Gameplay
//{
//    // Struct to represent a (column, row) pair
//    public struct Point
//    {
//        public byte Col { get; }
//        public byte Row { get; }

//        public Point(int col, int row)
//        {
//            Col = (byte)col;
//            Row = (byte)row;
//        }

//        // Returns a new (column, row) pair 
//        public Point Move(Point delta)
//        {
//            return new Point(Col + delta.Col, Row + delta.Row);
//        }
//    }
//}
