using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectFour.Gameplay
{
    // Represents an action of inserting player's token into the given column
    public class Move
    {
        public Token Tok { get; }
        public int   Col { get; }
        public int   Row { get; set; }

        public Move(Token player, int column)
        {
            Tok = player;
            Col = (byte)column;
            Row = -1;
        }

        public override string ToString()
        {
            return $"Player { Tok } into ({ Col }, { Row })";
        }

    }
}
