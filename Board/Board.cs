using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ca38Bot.Board
{
    public class Chessboard
    {
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

        public bool player;

        public string fen;

        public string[] coords = new string[64]
        {
            "h1", "g1", "f1", "e1", "d1", "c1", "b1", "a1",
            "h2", "g2", "f2", "e2", "d2", "c2", "b2", "a2",
            "h3", "g3", "f3", "e3", "d3", "c3", "b3", "a3",
            "h4", "g4", "f4", "e4", "d4", "c4", "b4", "a4",
            "h5", "g5", "f5", "e5", "d5", "c5", "b5", "a5",
            "h6", "g6", "f6", "e6", "d6", "c6", "b6", "a6",
            "h7", "g7", "f7", "e7", "d7", "c7", "b7", "a7",
            "h8", "g8", "f8", "e8", "d8", "c8", "b8", "a8"
        };
        public char[] pieceNames = new char[6]
        {
            'P', 'R', 'B', 'N', 'Q', 'K'
        };

        readonly private MagicMoves mm = new MagicMoves();

        public Chessboard() 
        {
            player = true;

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

            mm.Init();
        }

        public void LoadFEN(string fen)
        {
            player = (fen[^1] == 'p');
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

        public MemoryStream FENToPng()
        {
            string _fencpy = "";
            string _fen = "";
            string tmp;
            StringBuilder builder;
            foreach (string s in fen[0..^2].Split("/"))
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

                tmp = builder.ToString();
                _fencpy += tmp;
            }
            if(!player)
            {
                for(int i = _fencpy.Length - 1; i >= 0; --i)
                {
                    _fen += _fencpy[i].ToString();
                }
            }
            else
            {
                for (int i = 0; i < _fencpy.Length; ++i)
                {
                    _fen += _fencpy[i].ToString();
                }
            }
            var stream = new MemoryStream();
            using var board = (player) ? new Bitmap("board.png") : new Bitmap("boardrev.png");
            using (var pieces = new Bitmap("pieces.png"))
            using (var gr = Graphics.FromImage(board))
            {
                for (int j = 0; j < _fen.Length; ++j)
                {
                    if (_fen[j] != 's' && !Char.IsDigit(_fen[j]))
                    {
                        using var p = _fen[j] switch
                        {
                            'k' => pieces.Clone(new Rectangle(0, 0, 100, 100), System.Drawing.Imaging.PixelFormat.Format32bppPArgb),
                            'K' => pieces.Clone(new Rectangle(0, 100, 100, 100), System.Drawing.Imaging.PixelFormat.Format32bppPArgb),
                            'q' => pieces.Clone(new Rectangle(100, 0, 100, 100), System.Drawing.Imaging.PixelFormat.Format32bppPArgb),
                            'Q' => pieces.Clone(new Rectangle(100, 100, 100, 100), System.Drawing.Imaging.PixelFormat.Format32bppPArgb),
                            'r' => pieces.Clone(new Rectangle(200, 0, 100, 100), System.Drawing.Imaging.PixelFormat.Format32bppPArgb),
                            'R' => pieces.Clone(new Rectangle(200, 100, 100, 100), System.Drawing.Imaging.PixelFormat.Format32bppPArgb),
                            'b' => pieces.Clone(new Rectangle(300, 0, 100, 100), System.Drawing.Imaging.PixelFormat.Format32bppPArgb),
                            'B' => pieces.Clone(new Rectangle(300, 100, 100, 100), System.Drawing.Imaging.PixelFormat.Format32bppPArgb),
                            'n' => pieces.Clone(new Rectangle(400, 0, 100, 100), System.Drawing.Imaging.PixelFormat.Format32bppPArgb),
                            'N' => pieces.Clone(new Rectangle(400, 100, 100, 100), System.Drawing.Imaging.PixelFormat.Format32bppPArgb),
                            'p' => pieces.Clone(new Rectangle(500, 0, 100, 100), System.Drawing.Imaging.PixelFormat.Format32bppPArgb),
                            'P' => pieces.Clone(new Rectangle(500, 100, 100, 100), System.Drawing.Imaging.PixelFormat.Format32bppPArgb),
                            _ => pieces.Clone(new Rectangle(0, 0, 100, 100), System.Drawing.Imaging.PixelFormat.Format32bppPArgb),
                        };
                        int x = j % 8;
                        int y = j / 8;
                        gr.DrawImage(p, 50 + x * 100, 50 + y * 100);
                    }
                }
            }
            board.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            stream.Position = 0;
            return stream;
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

        public List<Move> GetValidMoves(Side side, Piece piece)
        {
            List<Move> moves = new List<Move>();
            ulong bb = piece switch
            {
                Piece.WHITEPAWN => WhitePawns,
                Piece.BLACKPAWN => BlackPawns,
                Piece.ROOK => (side == Side.WHITE) ? WhiteRooks : BlackRooks,
                Piece.BISHOP => (side == Side.WHITE) ? WhiteBishops : BlackBishops,
                Piece.KNIGHT => (side == Side.WHITE) ? WhiteKnights : BlackKnights,
                Piece.QUEEN => (side == Side.WHITE) ? WhiteQueen : BlackQueen,
                Piece.KING => (side == Side.WHITE) ? WhiteKing : BlackKing,
                _ => 0
            };
            int[] squares = mm.GetUniqueSquares(bb);
            for (int k = 0; k < squares.Length; ++k)
            {
                ulong movebb = mm.GetMoves(this, side, piece, squares[k]);
                if (movebb != 0)
                {
                    int[] destSquares = mm.GetUniqueSquares(movebb);
                    for(int j = 0; j < destSquares.Length; ++j)
                    {
                        moves.Add(new Move(
                            Board.Move.PieceIndex(piece),
                            (ushort)destSquares[j],
                            (ushort)squares[k],
                            0,
                            0,
                            0)
                        );
                    }
                }
            }
            return moves;
        }

        public bool IsOccupied(string square)
        {
            int index;
            for(index = 0; index < coords.Length; ++index)
            {
                if (square == coords[index]) break;
            }
            return (AllPieces & (1UL << index)) > 0;
        }

        public List<Move> GetMovablePieces(Side side)
        {
            List<Move> res = new List<Move>();
            for(int j = 0; j < 6; ++j)
            {
                Piece p = j switch
                {
                    0 => (side == Side.WHITE) ? Piece.WHITEPAWN : Piece.BLACKPAWN,
                    1 => Piece.ROOK,
                    2 => Piece.BISHOP,
                    3 => Piece.KNIGHT,
                    4 => Piece.QUEEN,
                    5 => Piece.KING,
                    _ => 0
                };
                ulong bb = p switch
                {
                    Piece.WHITEPAWN => WhitePawns,
                    Piece.BLACKPAWN => BlackPawns,
                    Piece.ROOK => (side == Side.WHITE) ? WhiteRooks : BlackRooks,
                    Piece.BISHOP => (side == Side.WHITE) ? WhiteBishops : BlackBishops,
                    Piece.KNIGHT => (side == Side.WHITE) ? WhiteKnights : BlackKnights,
                    Piece.QUEEN => (side == Side.WHITE) ? WhiteQueen : BlackQueen,
                    Piece.KING => (side == Side.WHITE) ? WhiteKing : BlackKing,
                    _ => 0
                };
                int[] squares = mm.GetUniqueSquares(bb);
                for (int k = 0; k < squares.Length; ++k) 
                {
                    if(mm.GetMoves(this, side, p, squares[k]) != 0)
                    {
                        res.Add(new Move(Board.Move.PieceIndex(p), 0, 0, 0, 0, 0));
                        break;
                    }
                }
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
