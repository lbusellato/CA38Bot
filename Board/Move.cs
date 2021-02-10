using System;
using Ca38Bot.Board;

namespace Ca38Bot.Board
{
    public class Move
    {
        public string[] Coords = new string[64]
        {
            "h1", "g1", "f1", "e1", "d1", "c1", "b1", "a1",
            "h2", "g2", "f2", "e2", "d2", "c2", "b2", "a2",
            "h3", "g3", "f3", "e3", "d3", "c3", "b3", "a3",
            "h4", "g4", "f4", "e4", "d4", "c4", "b4", "a4",
            "h5", "g5", "f5", "e5", "d5", "c5", "b5", "a5",
            "h6", "g6", "f6", "e6", "d6", "c6", "b6", "a6",
            "h7", "g7", "f7", "e7", "d7", "c7", "b7", "a7",
            "h8", "g8", "f8", "e8", "d8", "c8", "b8", "a8",
        };

        public string[] PieceNames = new string[6]
        {
            "P", "R", "N", "B", "Q", "K"
        };
        /*
         * MOVE
         * 0b {31-18} {19} {18 17 16} {15 14} {13 12} {11 12 9 8 7 6} {5 4 3 2 1 0}
         * 
         * 19 capture:  0 false 1 true
         * 18-16 piece:    000 pawn
         *                 001 rook
         *                 010 knight
         *                 011 bishop
         *                 100 queen
         *                 101 king
         * 15-14 special:   00 nothing
         *                  01 promotion
         *                  10 en passant
         *                  11 castling
         * 13-12 promotion: 00 queen
         *                  01 rook
         *                  10 knight
         *                  11 bishop
         * 11-6 origin square 0-63
         * 5-0 destination square 0-63
         * 
         */
        uint toMask =         0b00000000000000111111;
        uint fromMask =       0b00000000111111000000;
        uint promotionMask =  0b00000011000000000000;
        uint specialMask =    0b00001100000000000000;
        uint pieceMask =      0b01110000000000000000;
        uint captureMask =    0b10000000000000000000;
        uint m = 0;

        public static ushort PieceIndex(Piece piece) => piece switch
        {
            Board.Piece.ROOK => 0b001,
            Board.Piece.KNIGHT => 0b010,
            Board.Piece.BISHOP => 0b011,
            Board.Piece.QUEEN => 0b100,
            Board.Piece.KING => 0b101,
            _ => 0b000
        };

        public string To
        {
            get { return Coords[(int)(m & toMask)]; }
            set { }
            
        }
        public string From
        {
            get { return Coords[(int)((m & fromMask) >> 6)]; }
            set { }
        }

        public string Piece
        {
            get { return PieceNames[(m & pieceMask) >> 16]; }
            set { }
        }
        public Move(ushort piece, ushort to, ushort from, ushort promotion, ushort special, ushort capture)
        {
            m |= to;
            m |= (uint)(from << 6);
            m |= (uint)(promotion << 12);
            m |= (uint)(special << 14);
            m |= (uint)(piece << 16);
            m |= (uint)(capture << 17);
        }

        int BinSearch(string[] o, int l, int r, string t)
        {
            int p = (r - l) / 2;
            if (o[p] == t) return p;
            else if (String.Compare(o[p], t) < 0) return BinSearch(o, p + 1, r, t);
            else if (String.Compare(o[p], t) > 0) return BinSearch(o, l, p - 1, t);
            else return -1;
        }

        public int GetSquareIndex(string target)
        {
            return BinSearch(Coords, 0, 63, target);
        }


    }
}
