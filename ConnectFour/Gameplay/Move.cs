using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour.Gameplay
{
    // Struct to represent action of inserting token into given col and row
    public struct Move
    {
        public Color Player { get; }
        public byte  Col    { get; }
        public byte  Row    { get; }

        // Constructs a new Move struct
        public Move(Color player, int col, int row)
        {
            Player = player;
            Col = (byte)col;
            Row = (byte)row;
        }

        public override string ToString()
        {
            return $"{ Player } --> ({ Col }, { Row })";
        }

        // Returns the corresponding console color this player
        public ConsoleColor OutputColor()
        {
            return ((Player == Color.Red) ? ConsoleColor.Red : ConsoleColor.Yellow);
        }

    }
}
