using System;

namespace Ca38Bot.Board
{
    public class Chessboard
    {
        /* PIECES */
        public enum Piece{
            WHITEPAWN,
            BLACKPAWN,
            ROOK,
            BISHOP,
            KNIGHT,
            KING,
            QUEEN
        }

        /* WHITE PIECES */
        public ulong WhitePawns;
        public ulong WhiteRooks;
        public ulong WhiteBishops;
        public ulong WhiteKnights;
        public ulong WhiteKing;
        public ulong WhiteQueen;

        /* BLACK PIECES */
        public ulong BlackPawns;
        public ulong BlackRooks;
        public ulong BlackBishops;
        public ulong BlackKnights;
        public ulong BlackKing;
        public ulong BlackQueen;

        /* DERIVED */
        public ulong WhitePieces;
        public ulong BlackPieces;
        public ulong AllPieces;

        public Chessboard() 
        {
            WhitePawns = Masks.WHITEPAWNS;
            WhiteRooks = Masks.WHITEROOKS;
            WhiteBishops = Masks.WHITEBISHOPS;
            WhiteKnights = Masks.WHITEKNIGHTS;
            WhiteKing = Masks.WHITEKING;
            WhiteQueen = Masks.WHITEQUEEN;

            BlackPawns = Masks.BLACKPAWNS;
            BlackRooks = Masks.BLACKROOKS;
            BlackBishops = Masks.BLACKBISHOPS;
            BlackKnights = Masks.BLACKKNIGHTS;
            BlackKing = Masks.BLACKKING;
            BlackQueen = Masks.BLACKQUEEN;

            WhitePieces =  WhitePawns | WhiteRooks | WhiteBishops | WhiteKnights | WhiteKing | WhiteQueen;
            BlackPieces = BlackPawns | BlackRooks | BlackBishops | BlackKnights | BlackKing | BlackQueen;
            AllPieces = WhitePieces | BlackPieces;
        }

        public void LoadFEN(string fen)
        {
            string[] subs = fen.Split("/");
            string board = String.Join("", subs);
            string bb = "";
            for (int j = 0; j < 12; ++j)
            {
                bb = "";
                char ctl = j switch
                {
                    0 => 'P',
                    1 => 'R',
                    2 => 'B',
                    3 => 'N',
                    4 => 'K',
                    5 => 'Q',
                    6 => 'p',
                    7 => 'r',
                    8 => 'b',
                    9 => 'n',
                    10 => 'k',
                    11 => 'q',
                    _ => ' '
                };
                for (int i = 0; i < board.Length; ++i)
                {
                    bb += board[i] switch
                    {
                        '1' => "0",
                        '2' => "00",
                        '3' => "000",
                        '4' => "0000",
                        '5' => "00000",
                        '6' => "000000",
                        '7' => "0000000",
                        '8' => "00000000",
                        _ => (ctl == board[i]) ? "1" : "0"
                    };
                }
                ulong _bb = Convert(bb);
                switch (j)
                {
                    case 0:
                        WhitePawns = _bb;
                        break;
                    case 1:
                        WhiteRooks = _bb;
                        break;
                    case 2:
                        WhiteBishops = _bb;
                        break;
                    case 3:
                        WhiteKnights = _bb;
                        break;
                    case 4:
                        WhiteKing = _bb;
                        break;
                    case 5:
                        WhiteQueen = _bb;
                        break;
                    case 6:
                        BlackPawns = _bb;
                        break;
                    case 7:
                        BlackRooks = _bb;
                        break;
                    case 8:
                        BlackBishops = _bb;
                        break;
                    case 9:
                        BlackKnights = _bb;
                        break;
                    case 10:
                        BlackKing = _bb;
                        break;
                    case 11:
                        BlackQueen = _bb;
                        break;
                    default:
                        break;
                }
            }

            WhitePieces = WhitePawns | WhiteRooks | WhiteBishops | WhiteKnights | WhiteKing | WhiteQueen;
            BlackPieces = BlackPawns | BlackRooks | BlackBishops | BlackKnights | BlackKing | BlackQueen;
            AllPieces = WhitePieces | BlackPieces;
        }

        private ulong Convert(string input)
        {
            ulong res = 0;
            for (int i = 0; i < input.Length; ++i)
            {
                Int32.TryParse(input[i].ToString(), out int d);
                if(d == 0 && i == input.Length - 1)
                {
                    break; //Special case to weed out issues with calculating zero to the power of zero (which should be zero goddammit)
                }
                res += (ulong)Math.Pow(2 * d, 63 - i);
            }
            return res;
        }

        public ulong GetMovementMask(Piece p, ulong OwnPieces, ulong pos)
        {
            ulong res = 0;
            ulong s1;
            ulong s2;
            ulong s3;
            ulong s4;
            ulong s5;
            ulong s6;
            ulong s7;
            ulong s8;
            ulong p1;
            ulong p2;
            ulong a1;
            ulong a2;
            ulong a;
            switch (p)
            {
                case Piece.KING:
                    // These are used to solve under/over flows in the position (eg if the king is on the a or h file)
                    // by removing the king from them (obtaining an empty bitboard with the shifts, and thus an invalid movement)
                    ulong posAmask = pos & Masks.CLEARFILEA;
                    ulong posHmask = pos & Masks.CLEARFILEH;
                    s1 = pos << 8;
                    s2 = posHmask << 7;
                    s3 = posHmask >> 1;
                    s4 = posHmask >> 9;
                    s5 = pos >> 8;
                    s6 = posAmask >> 7;
                    s7 = posAmask << 1;
                    s8 = posAmask << 9;
                    res = (s1 | s2 | s3 | s4 | s5 | s6 | s7 | s8) & ~OwnPieces;
                    break;
                case Piece.KNIGHT:
                    ulong c1 = Masks.CLEARFILEA & Masks.CLEARFILEB; 
                    ulong c2 = Masks.CLEARFILEA;
                    ulong c3 = Masks.CLEARFILEH;
                    ulong c4 = Masks.CLEARFILEH & Masks.CLEARFILEG;
                    s1 = (pos & c1) << 6;
                    s2 = (pos & c2) << 15;
                    s3 = (pos & c3) << 17;
                    s4 = (pos & c4) << 10;
                    s5 = (pos & c4) >> 6;
                    s6 = (pos & c3) >> 15;
                    s7 = (pos & c2) >> 17;
                    s8 = (pos & c1) >> 10;
                    res = (s1 | s2 | s3 | s4 | s5 | s6 |s7 | s8) & ~OwnPieces;
                    break;
                case Piece.WHITEPAWN:
                    //Check if the pawn can move up one step
                    p1 = (pos << 8) & ~AllPieces;
                    //Check if the pawn can double move
                    p2 = ((p1 & Masks.MASKRANK3) << 8) & ~AllPieces;
                    //Check both diagonals in case the pawn can attack there, minding the overflow on the a and h files
                    a1 = (pos & Masks.CLEARFILEA) << 7;
                    a2 = (pos & Masks.CLEARFILEH) << 9;
                    //Join both attack routes and check if there's a piece to attack on them
                    a = (a1 | a2) & BlackPieces;
                    res = p1 | p2 | a;
                    break;
                case Piece.BLACKPAWN:
                    //Check if the pawn can move down one step
                    p1 = (pos >> 8) & ~AllPieces;
                    //Check if the pawn can double move
                    p2 = ((p1 & Masks.MASKRANK6) >> 8) & ~AllPieces;
                    //Check both diagonals in case the pawn can attack there, minding the overflow on the a and h files
                    a1 = (pos & Masks.CLEARFILEA) >> 7;
                    a2 = (pos & Masks.CLEARFILEH) >> 9;
                    //Join both attack routes and check if there's a piece to attack on them
                    a = (a1 | a2) & WhitePieces;
                    res = p1 | p2 | a;
                    break;
                default:
                    break;
            }
            return res;
        }

    }
}
