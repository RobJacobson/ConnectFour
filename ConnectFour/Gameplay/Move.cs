using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour.Gameplay
{
    // Struct to represent one token on board at given (col, row) coordinates
    public struct Move
    {
        public Color Token  { get; }
        public byte  Col    { get; }
        public byte  Row    { get; }

        // Constructs a new Move struct
        public Move(Color token, int col, int row)
        {
            Token = token;
            Col = (byte)col;
            Row = (byte)row;
        }

        public override string ToString()
        {
            return $"{ Token } --> ({ Col }, { Row })";
        }

        // Returns the corresponding console color this player
        public ConsoleColor OutputColor()
        {
            return ((Token == Color.Red) ? ConsoleColor.Red : ConsoleColor.Yellow);
        }

    }
}
