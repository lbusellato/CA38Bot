using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ca38Bot.Board
{
    public class Chessboard
    {
        /* SIDES */
        public enum Side
        {
            WHITE,
            BLACK,
        }
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

        public string fen;

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

            fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
        }

        public void LoadFEN(string fen)
        {
            string[] subs = fen[0..^2].Split("/");
            string board = String.Join("", subs);
            string bb;
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

            this.fen = fen;
        }

        public void FENToPng()
        {

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

        public string[] GetValidMoves(Side side, Piece piece)
        {
            string move;
            List<string> res = new List<string>();
            ulong[] fileMasks = new ulong[]
            {
                Masks.MASKFILEA,
                Masks.MASKFILEB,
                Masks.MASKFILEC,
                Masks.MASKFILED,
                Masks.MASKFILEE,
                Masks.MASKFILEF,
                Masks.MASKFILEG,
                Masks.MASKFILEH,
            };
            ulong[] rankMasks = new ulong[]
            {
                Masks.MASKRANK8,
                Masks.MASKRANK7,
                Masks.MASKRANK6,
                Masks.MASKRANK5,
                Masks.MASKRANK4,
                Masks.MASKRANK3,
                Masks.MASKRANK2,
                Masks.MASKRANK1,
            };
            ulong piecePos = piece switch
            {
                Piece.WHITEPAWN => WhitePawns,
                Piece.BLACKPAWN => BlackPawns,
                Piece.ROOK => (side == Side.WHITE) ? WhiteRooks : BlackRooks,
                Piece.KNIGHT => (side == Side.WHITE) ? WhiteKnights : BlackKnights,
                Piece.BISHOP => (side == Side.WHITE) ? WhiteBishops : BlackBishops,
                Piece.QUEEN => (side == Side.WHITE) ? WhiteQueen : BlackQueen,
                Piece.KING => (side == Side.WHITE) ? WhiteKing : BlackKing,
                _ => 0,
            };

            List<ulong> startingMask = new List<ulong>();
            List<string> startingPos = new List<string>();
            int x, y;
            for (int j = 0; j < 64; ++j)
            {
                move = "";
                x = j / 8;
                y = j % 8;
                if ((piecePos & fileMasks[x] & rankMasks[y]) != 0)
                {
                    move += x switch
                    {
                        0 => "a",
                        1 => "b",
                        2 => "c",
                        3 => "d",
                        4 => "e",
                        5 => "f",
                        6 => "g",
                        7 => "h",
                        _ => "",
                    };
                    move += (8 - y).ToString();
                    startingPos.Add(move);
                    startingMask.Add(piecePos & fileMasks[x] & rankMasks[y]);
                }
            }
            ulong mask;
            for (int i = 0; i < startingMask.Count(); ++i)
            {
                mask = GetMovementMask(piece, (side == Side.WHITE) ? WhitePieces : BlackPieces, startingMask.ElementAt(i));
                for (int j = 0; j < 64; ++j)
                {
                    move = "";
                    x = j / 8;
                    y = j % 8;
                    if ((mask & fileMasks[x] & rankMasks[y]) != 0)
                    {
                        move += x switch
                        {
                            0 => "a",
                            1 => "b",
                            2 => "c",
                            3 => "d",
                            4 => "e",
                            5 => "f",
                            6 => "g",
                            7 => "h",
                            _ => "",
                        };
                        move += (8 - y).ToString();
                        res.Add(startingPos.ElementAt(i) + move);
                    }
                }
            }
            return res.ToArray();
        }

        public string[] GetMovablePieces(Side side)
        {
            List<string> res = new List<string>();
            ulong[] fileMasks = new ulong[]
            {
                Masks.MASKFILEA,
                Masks.MASKFILEB,
                Masks.MASKFILEC,
                Masks.MASKFILED,
                Masks.MASKFILEE,
                Masks.MASKFILEF,
                Masks.MASKFILEG,
                Masks.MASKFILEH,
            };
            ulong[] rankMasks = new ulong[]
            {
                Masks.MASKRANK8,
                Masks.MASKRANK7,
                Masks.MASKRANK6,
                Masks.MASKRANK5,
                Masks.MASKRANK4,
                Masks.MASKRANK3,
                Masks.MASKRANK2,
                Masks.MASKRANK1,
            };
            for (int i = 0; i < 6; ++i)
            {
                Piece piece = i switch
                {
                    0 => (side == Side.WHITE) ? Piece.WHITEPAWN : Piece.BLACKPAWN,
                    1 => Piece.ROOK,
                    2 => Piece.KNIGHT,
                    3 => Piece.BISHOP,
                    4 => Piece.QUEEN,
                    5 => Piece.KING,
                    _ => 0,
                };
                ulong piecePos = i switch
                {
                    0 => (side == Side.WHITE) ? WhitePawns : BlackPawns,
                    1 => (side == Side.WHITE) ? WhiteRooks : BlackRooks,
                    2 => (side == Side.WHITE) ? WhiteKnights : BlackKnights,
                    3 => (side == Side.WHITE) ? WhiteBishops : BlackBishops,
                    4 => (side == Side.WHITE) ? WhiteQueen : BlackQueen,
                    5 => (side == Side.WHITE) ? WhiteKing : BlackKing,
                    _ => 0,
                };
                ulong mask = GetMovementMask(piece, (side == Side.WHITE) ? WhitePieces : BlackPieces, piecePos);
                int x, y;
                for(int j = 0; j < 64; ++j)
                {
                    x = j / 8;
                    y = j % 8;
                    if ((mask & fileMasks[x] & rankMasks[y]) != 0)
                    {
                        res.Add(piece switch{
                            Piece.BLACKPAWN => "P",
                            Piece.WHITEPAWN => "P",
                            Piece.ROOK => "R",
                            Piece.BISHOP => "B",
                            Piece.KNIGHT => "N",
                            Piece.QUEEN => "Q",
                            Piece.KING => "K",
                            _ => "-"
                        });
                        break;
                    }
                }
            }
            return res.ToArray();
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
                    s1 = (pos & c1) << 10;
                    s2 = (pos & c2) << 17;
                    s3 = (pos & c3) << 15;
                    s4 = (pos & c4) << 6;
                    s5 = (pos & c4) >> 10;
                    s6 = (pos & c3) >> 17;
                    s7 = (pos & c2) >> 15;
                    s8 = (pos & c1) >> 6;
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

        public void Move(string s1, string s2)
        {
            List<string> _fen = new List<string>();
            string y;
            char[] subs;
            StringBuilder builder;
            foreach (string s in fen.Split("/"))
            {
                builder = new StringBuilder(s);
                builder.Replace("1", "s");
                builder.Replace("2", "ss");
                builder.Replace("3", "sss");
                builder.Replace("4", "ssss");
                builder.Replace("5", "sssss");
                builder.Replace("6", "ssssss");
                builder.Replace("7", "sssssss");
                builder.Replace("8", "ssssssss");

                y = builder.ToString();
                _fen.Add(y);
            }
            int x1 = s1[0] switch
            {
                'a' => 0,
                'b' => 1,
                'c' => 2,
                'd' => 3,
                'e' => 4,
                'f' => 5,
                'g' => 6,
                'h' => 7,
                _ => 0,
            }; ;
            Int32.TryParse(s1[1].ToString(), out int y1);
            y1 = 8 - y1;
            int x2 = s2[0] switch
            {
                'a' => 0,
                'b' => 1,
                'c' => 2,
                'd' => 3,
                'e' => 4,
                'f' => 5,
                'g' => 6,
                'h' => 7,
                _ => 0,
            }; ;
            Int32.TryParse(s2[1].ToString(), out int y2);
            y2 = 8 - y2;
            char oldPiece = _fen[y1].ElementAt(x1);
            subs = _fen[y1].ToCharArray();
            subs[x1] = 's';
            _fen[y1] = String.Join("", subs);
            subs = _fen[y2].ToCharArray();
            subs[x2] = oldPiece;
            _fen[y2] = String.Join("", subs);
            string tmp = "";
            foreach (string s in _fen)
            {
                builder = new StringBuilder(s);
                builder.Replace("ssssssss", "8");
                builder.Replace("sssssss", "7");
                builder.Replace("ssssss", "6");
                builder.Replace("sssss", "5");
                builder.Replace("ssss", "4");
                builder.Replace("sss", "3");
                builder.Replace("ss", "2");
                builder.Replace("s", "1");
                string t = builder.ToString();
                tmp += t;
                tmp += "/";
            }
            string res = tmp.Remove(tmp.Length - 1, 1);
            this.fen = res;
        }
    }
}
