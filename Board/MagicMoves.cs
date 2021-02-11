using System.Collections.Generic;

namespace Ca38Bot.Board
{
    /* SQUARE INDEXES
     * 
     * 8 - 63 62 61 60 59 58 57 56
     * 7 - 55 54 53 52 51 50 49 48
     * 6 - 47 46 45 44 43 42 41 40
     * 5 - 39 38 37 36 35 34 33 32 
     * 4 - 31 30 29 28 27 26 25 24
     * 3 - 23 22 21 20 19 18 17 16
     * 2 - 15 14 13 12 11 10  9  8
     * 1 -  7  6  5  4  3  2  1  0
     *      a  b  c  d  e  f  g  h
     *      
    */
    public class MagicMoves
    {
        /* BIT MANIPULATION */
        /// <summary>
        /// Sets the i-th bit in the number u to 1
        /// </summary>
        /// <param name="u">A 64-bit number</param>
        /// <param name="i">Bit index</param>
        public void SetBit(ref ulong u, int i)
        {
            u |= (1UL << i);
        }

        /// <summary>
        /// Get the state of the i-th bit in the number u
        /// </summary>
        /// <param name="u">A 64-bit number</param>
        /// <param name="i">Bit index</param>
        /// <returns>0 if the bit is not set, 1 if it is</returns>
        public int GetBit(ulong u, int i)
        {
            return ((u & (1UL << i)) > 0 ? 1 : 0);
        }

        /// <summary>
        /// Set the i-th bit in the provided number u to 0
        /// </summary>
        /// <param name="u"></param>
        /// <param name="i"></param>
        public void PopBit(ref ulong u, int i)
        {
            u ^= (1UL << i);
        }

        /// <summary>
        /// Iteratively count set bits in the provided 64-bit number
        /// TODO: consider using a faster approach
        /// </summary>
        /// <param name="u">A 64-bit number</param>
        /// <returns>The number of set bits in u</returns>
        public int CountBits(ulong u)
        {
            int count = 0;
            while (u > 0)
            {
                if (GetBit(u, 0) == 1)
                {
                    count++;
                }
                u >>= 1;
            }
            return count;
        }

        /// <summary>
        /// Finds the index of the 1st least significant set bit in v
        /// TODO: consider using a faster approach
        /// </summary>
        /// <param name="u">A 64-bit number</param>
        /// <returns>The index of the first least significant set bit in u</returns>
        public int GetLS1BIndex(ulong u)
        {
            int index = 0;
            while (GetBit(u, 0) == 0)
            {
                index++;
                u >>= 1;
            }
            return index;
        }

        public ulong[] GetUniqueBitboards(ulong u)
        {
            ulong[] res;
            ulong cpy = u;
            if (cpy == 0) return null;
            res = new ulong[CountBits(cpy)];
            for (int i = 0; i < CountBits(u); ++i)
            {
                int bitIndex = GetLS1BIndex(cpy);
                res[i] = (1UL << bitIndex);
                PopBit(ref cpy, bitIndex);
            }
            return res;
        }
        public int[] GetUniqueSquares(ulong u)
        {
            List<int> res = new List<int>();
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
            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    if ((u & fileMasks[i] & rankMasks[j]) > 0)
                    {
                        res.Add(63 - (i + 8 * j));
                    }
                }
            }
            return res.ToArray();
        }

        /* PRE CALCULATED MOVEMENT MASKS */
        public ulong[] KingMasks = new ulong[64]{
            0x302,
            0x705,
            0xe0a,
            0x1c14,
            0x3828,
            0x7050,
            0xe0a0,
            0xc040,
            0x30203,
            0x70507,
            0xe0a0e,
            0x1c141c,
            0x382838,
            0x705070,
            0xe0a0e0,
            0xc040c0,
            0x3020300,
            0x7050700,
            0xe0a0e00,
            0x1c141c00,
            0x38283800,
            0x70507000,
            0xe0a0e000,
            0xc040c000,
            0x302030000,
            0x705070000,
            0xe0a0e0000,
            0x1c141c0000,
            0x3828380000,
            0x7050700000,
            0xe0a0e00000,
            0xc040c00000,
            0x30203000000,
            0x70507000000,
            0xe0a0e000000,
            0x1c141c000000,
            0x382838000000,
            0x705070000000,
            0xe0a0e0000000,
            0xc040c0000000,
            0x3020300000000,
            0x7050700000000,
            0xe0a0e00000000,
            0x1c141c00000000,
            0x38283800000000,
            0x70507000000000,
            0xe0a0e000000000,
            0xc040c000000000,
            0x302030000000000,
            0x705070000000000,
            0xe0a0e0000000000,
            0x1c141c0000000000,
            0x3828380000000000,
            0x7050700000000000,
            0xe0a0e00000000000,
            0xc040c00000000000,
            0x203000000000000,
            0x507000000000000,
            0xa0e000000000000,
            0x141c000000000000,
            0x2838000000000000,
            0x5070000000000000,
            0xa0e0000000000000,
            0x40c0000000000000,
        };
        public ulong[] KnightMasks = new ulong[64]{
            0x20400,
            0x50800,
            0xa1100,
            0x142200,
            0x284400,
            0x508800,
            0xa01000,
            0x402000,
            0x2040004,
            0x5080008,
            0xa110011,
            0x14220022,
            0x28440044,
            0x50880088,
            0xa0100010,
            0x40200020,
            0x204000402,
            0x508000805,
            0xa1100110a,
            0x1422002214,
            0x2844004428,
            0x5088008850,
            0xa0100010a0,
            0x4020002040,
            0x20400040200,
            0x50800080500,
            0xa1100110a00,
            0x142200221400,
            0x284400442800,
            0x508800885000,
            0xa0100010a000,
            0x402000204000,
            0x2040004020000,
            0x5080008050000,
            0xa1100110a0000,
            0x14220022140000,
            0x28440044280000,
            0x50880088500000,
            0xa0100010a00000,
            0x40200020400000,
            0x204000402000000,
            0x508000805000000,
            0xa1100110a000000,
            0x1422002214000000,
            0x2844004428000000,
            0x5088008850000000,
            0xa0100010a0000000,
            0x4020002040000000,
            0x400040200000000,
            0x800080500000000,
            0x1100110a00000000,
            0x2200221400000000,
            0x4400442800000000,
            0x8800885000000000,
            0x100010a000000000,
            0x2000204000000000,
            0x4020000000000,
            0x8050000000000,
            0x110a0000000000,
            0x22140000000000,
            0x44280000000000,
            0x88500000000000,
            0x10a00000000000,
            0x20400000000000,
        };
        public ulong[] WhitePawnMasks = new ulong[64]{
            0x100,
            0x200,
            0x400,
            0x800,
            0x1000,
            0x2000,
            0x4000,
            0x8000,
            0x1010000,
            0x2020000,
            0x4040000,
            0x8080000,
            0x10100000,
            0x20200000,
            0x40400000,
            0x80800000,
            0x1000000,
            0x2000000,
            0x4000000,
            0x8000000,
            0x10000000,
            0x20000000,
            0x40000000,
            0x80000000,
            0x100000000,
            0x200000000,
            0x400000000,
            0x800000000,
            0x1000000000,
            0x2000000000,
            0x4000000000,
            0x8000000000,
            0x10000000000,
            0x20000000000,
            0x40000000000,
            0x80000000000,
            0x100000000000,
            0x200000000000,
            0x400000000000,
            0x800000000000,
            0x1000000000000,
            0x2000000000000,
            0x4000000000000,
            0x8000000000000,
            0x10000000000000,
            0x20000000000000,
            0x40000000000000,
            0x80000000000000,
            0x100000000000000,
            0x200000000000000,
            0x400000000000000,
            0x800000000000000,
            0x1000000000000000,
            0x2000000000000000,
            0x4000000000000000,
            0x8000000000000000,
            0x0,
            0x0,
            0x0,
            0x0,
            0x0,
            0x0,
            0x0,
            0x0,
        };
        public ulong[] BlackPawnMasks = new ulong[64]{
            0x0,
            0x0,
            0x0,
            0x0,
            0x0,
            0x0,
            0x0,
            0x0,
            0x1,
            0x2,
            0x4,
            0x8,
            0x10,
            0x20,
            0x40,
            0x80,
            0x100,
            0x200,
            0x400,
            0x800,
            0x1000,
            0x2000,
            0x4000,
            0x8000,
            0x10000,
            0x20000,
            0x40000,
            0x80000,
            0x100000,
            0x200000,
            0x400000,
            0x800000,
            0x1000000,
            0x2000000,
            0x4000000,
            0x8000000,
            0x10000000,
            0x20000000,
            0x40000000,
            0x80000000,
            0x100000000,
            0x200000000,
            0x400000000,
            0x800000000,
            0x1000000000,
            0x2000000000,
            0x4000000000,
            0x8000000000,
            0x10100000000,
            0x20200000000,
            0x40400000000,
            0x80800000000,
            0x101000000000,
            0x202000000000,
            0x404000000000,
            0x808000000000,
            0x1000000000000,
            0x2000000000000,
            0x4000000000000,
            0x8000000000000,
            0x10000000000000,
            0x20000000000000,
            0x40000000000000,
            0x80000000000000,
        };
        public ulong[] BishopMasks = new ulong[64]{
            0x40201008040200,
            0x402010080400,
            0x4020100a00,
            0x40221400,
            0x2442800,
            0x204085000,
            0x20408102000,
            0x2040810204000,
            0x20100804020000,
            0x40201008040000,
            0x4020100a0000,
            0x4022140000,
            0x244280000,
            0x20408500000,
            0x2040810200000,
            0x4081020400000,
            0x10080402000200,
            0x20100804000400,
            0x4020100a000a00,
            0x402214001400,
            0x24428002800,
            0x2040850005000,
            0x4081020002000,
            0x8102040004000,
            0x8040200020400,
            0x10080400040800,
            0x20100a000a1000,
            0x40221400142200,
            0x2442800284400,
            0x4085000500800,
            0x8102000201000,
            0x10204000402000,
            0x4020002040800,
            0x8040004081000,
            0x100a000a102000,
            0x22140014224000,
            0x44280028440200,
            0x8500050080400,
            0x10200020100800,
            0x20400040201000,
            0x2000204081000,
            0x4000408102000,
            0xa000a10204000,
            0x14001422400000,
            0x28002844020000,
            0x50005008040200,
            0x20002010080400,
            0x40004020100800,
            0x20408102000,
            0x40810204000,
            0xa1020400000,
            0x142240000000,
            0x284402000000,
            0x500804020000,
            0x201008040200,
            0x402010080400,
            0x2040810204000,
            0x4081020400000,
            0xa102040000000,
            0x14224000000000,
            0x28440200000000,
            0x50080402000000,
            0x20100804020000,
            0x40201008040200,
        };
        public ulong[] RookMasks = new ulong[64]{
            0x101010101017e,
            0x202020202027c,
            0x404040404047a,
            0x8080808080876,
            0x1010101010106e,
            0x2020202020205e,
            0x4040404040403e,
            0x8080808080807e,
            0x1010101017e00,
            0x2020202027c00,
            0x4040404047a00,
            0x8080808087600,
            0x10101010106e00,
            0x20202020205e00,
            0x40404040403e00,
            0x80808080807e00,
            0x10101017e0100,
            0x20202027c0200,
            0x40404047a0400,
            0x8080808760800,
            0x101010106e1000,
            0x202020205e2000,
            0x404040403e4000,
            0x808080807e8000,
            0x101017e010100,
            0x202027c020200,
            0x404047a040400,
            0x8080876080800,
            0x1010106e101000,
            0x2020205e202000,
            0x4040403e404000,
            0x8080807e808000,
            0x1017e01010100,
            0x2027c02020200,
            0x4047a04040400,
            0x8087608080800,
            0x10106e10101000,
            0x20205e20202000,
            0x40403e40404000,
            0x80807e80808000,
            0x17e0101010100,
            0x27c0202020200,
            0x47a0404040400,
            0x8760808080800,
            0x106e1010101000,
            0x205e2020202000,
            0x403e4040404000,
            0x807e8080808000,
            0x7e010101010100,
            0x7c020202020200,
            0x7a040404040400,
            0x76080808080800,
            0x6e101010101000,
            0x5e202020202000,
            0x3e404040404000,
            0x7e808080808000,
            0x7e01010101010100,
            0x7c02020202020200,
            0x7a04040404040400,
            0x7608080808080800,
            0x6e10101010101000,
            0x5e20202020202000,
            0x3e40404040404000,
            0x7e80808080808000,
        };
        public ulong[] QueenMasks = new ulong[64]{
            0x4121110905037e,
            0x24222120a067c,
            0x4044424140e7a,
            0x80808482a1c76,
            0x1010101254386e,
            0x2020222428705e,
            0x4042444850603e,
            0x82848890a0c07e,
            0x21110905037e00,
            0x4222120a067c00,
            0x44424140e7a00,
            0x808482a1c7600,
            0x10101254386e00,
            0x20222428705e00,
            0x42444850603e00,
            0x848890a0c07e00,
            0x110905037e0300,
            0x22120a067c0600,
            0x4424140e7a0e00,
            0x8482a1c761c00,
            0x101254386e3800,
            0x222428705e7000,
            0x444850603e6000,
            0x8890a0c07ec000,
            0x905037e030500,
            0x120a067c060a00,
            0x24140e7a0e1400,
            0x482a1c761c2a00,
            0x1254386e385400,
            0x2428705e702800,
            0x4850603e605000,
            0x90a0c07ec0a000,
            0x5037e03050900,
            0xa067c060a1200,
            0x140e7a0e142400,
            0x2a1c761c2a4800,
            0x54386e38541200,
            0x28705e70282400,
            0x50603e60504800,
            0xa0c07ec0a09000,
            0x37e0305091100,
            0x67c060a122200,
            0xe7a0e14244400,
            0x1c761c2a480800,
            0x386e3854121000,
            0x705e7028242200,
            0x603e6050484400,
            0xc07ec0a0908800,
            0x7e030509112100,
            0x7c060a12224200,
            0x7a0e1424440400,
            0x761c2a48080800,
            0x6e385412101000,
            0x5e702824222000,
            0x3e605048444200,
            0x7ec0a090888400,
            0x7e03050911214100,
            0x7c060a1222420200,
            0x7a0e142444040400,
            0x761c2a4808080800,
            0x6e38541210101000,
            0x5e70282422202000,
            0x3e60504844424000,
            0x7ec0a09088848200,
        };

        /* ATTACK TABLES */
        public ulong[,] BishopAttacks = new ulong[64, 512];
        public ulong[,] RookAttacks = new ulong[64, 4096];
        /* MAGIC NUMBERS */
        readonly int[] BishopShift = new int[64] {
            58, 59, 59, 59, 59, 59, 59, 58,
            59, 59, 59, 59, 59, 59, 59, 59,
            59, 59, 57, 57, 57, 57, 57, 59,
            59, 59, 57, 55, 55, 57, 57, 59,
            59, 59, 57, 55, 55, 57, 57, 59,
            59, 59, 57, 57, 57, 57, 57, 59,
            59, 59, 59, 59, 59, 59, 59, 59,
            58, 59, 59, 59, 59, 59, 59, 58
        };
        readonly int[] RookShift = new int[64] {
            52, 53, 53, 53, 53, 53, 53, 52,
            53, 54, 54, 54, 54, 54, 54, 53,
            53, 54, 54, 54, 54, 54, 54, 53,
            53, 54, 54, 54, 54, 54, 54, 53,
            53, 54, 54, 54, 54, 54, 54, 53,
            53, 54, 54, 54, 54, 54, 54, 53,
            53, 54, 54, 54, 54, 54, 54, 53,
            52, 53, 53, 53, 53, 53, 53, 52
        };
        readonly ulong[] RookMagics = new ulong[64] {
            0x8a80104000800020,
            0x140002000100040,
            0x2801880a0017001,
            0x100081001000420,
            0x200020010080420,
            0x3001c0002010008,
            0x8480008002000100,
            0x2080088004402900,
            0x800098204000,
            0x2024401000200040,
            0x100802000801000,
            0x120800800801000,
            0x208808088000400,
            0x2802200800400,
            0x2200800100020080,
            0x801000060821100,
            0x80044006422000,
            0x100808020004000,
            0x12108a0010204200,
            0x140848010000802,
            0x481828014002800,
            0x8094004002004100,
            0x4010040010010802,
            0x20008806104,
            0x100400080208000,
            0x2040002120081000,
            0x21200680100081,
            0x20100080080080,
            0x2000a00200410,
            0x20080800400,
            0x80088400100102,
            0x80004600042881,
            0x4040008040800020,
            0x440003000200801,
            0x4200011004500,
            0x188020010100100,
            0x14800401802800,
            0x2080040080800200,
            0x124080204001001,
            0x200046502000484,
            0x480400080088020,
            0x1000422010034000,
            0x30200100110040,
            0x100021010009,
            0x2002080100110004,
            0x202008004008002,
            0x20020004010100,
            0x2048440040820001,
            0x101002200408200,
            0x40802000401080,
            0x4008142004410100,
            0x2060820c0120200,
            0x1001004080100,
            0x20c020080040080,
            0x2935610830022400,
            0x44440041009200,
            0x280001040802101,
            0x2100190040002085,
            0x80c0084100102001,
            0x4024081001000421,
            0x20030a0244872,
            0x12001008414402,
            0x2006104900a0804,
            0x1004081002402
        };
        readonly ulong[] BishopMagics = new ulong[64] {
            0x40040844404084,
            0x2004208a004208,
            0x10190041080202,
            0x108060845042010,
            0x581104180800210,
            0x2112080446200010,
            0x1080820820060210,
            0x3c0808410220200,
            0x4050404440404,
            0x21001420088,
            0x24d0080801082102,
            0x1020a0a020400,
            0x40308200402,
            0x4011002100800,
            0x401484104104005,
            0x801010402020200,
            0x400210c3880100,
            0x404022024108200,
            0x810018200204102,
            0x4002801a02003,
            0x85040820080400,
            0x810102c808880400,
            0xe900410884800,
            0x8002020480840102,
            0x220200865090201,
            0x2010100a02021202,
            0x152048408022401,
            0x20080002081110,
            0x4001001021004000,
            0x800040400a011002,
            0xe4004081011002,
            0x1c004001012080,
            0x8004200962a00220,
            0x8422100208500202,
            0x2000402200300c08,
            0x8646020080080080,
            0x80020a0200100808,
            0x2010004880111000,
            0x623000a080011400,
            0x42008c0340209202,
            0x209188240001000,
            0x400408a884001800,
            0x110400a6080400,
            0x1840060a44020800,
            0x90080104000041,
            0x201011000808101,
            0x1a2208080504f080,
            0x8012020600211212,
            0x500861011240000,
            0x180806108200800,
            0x4000020e01040044,
            0x300000261044000a,
            0x802241102020002,
            0x20906061210001,
            0x5a84841004010310,
            0x4010801011c04,
            0xa010109502200,
            0x4a02012000,
            0x500201010098b028,
            0x8040002811040900,
            0x28000010020204,
            0x6000020202d0240,
            0x8918844842082200,
            0x4010011029020020
        };

        ulong GenAttackMask(Chessboard board, Side side)
        {
            ulong attackMask = 0UL;
            ulong[] bb = GetUniqueBitboards(side == Side.WHITE ? board.WhiteBishops : board.BlackBishops);
            int[] squares;
            for (int i = 0; i < bb.Length; ++i)
            {
                squares = GetUniqueSquares(bb[i]);
                for (int j = 0; j < squares.Length; ++j)
                {
                    attackMask |= GetMoves(board, side, Piece.BISHOP, squares[j]);
                }
            }
            bb = GetUniqueBitboards(side == Side.WHITE ? board.WhiteKnights : board.BlackKnights);
            for (int i = 0; i < bb.Length; ++i)
            {
                squares = GetUniqueSquares(bb[i]);
                for (int j = 0; j < squares.Length; ++j)
                {
                    attackMask |= GetMoves(board, side, Piece.KNIGHT, squares[j]);
                }
            }
            bb = GetUniqueBitboards(side == Side.WHITE ? board.WhiteRooks : board.BlackRooks);
            for (int i = 0; i < bb.Length; ++i)
            {
                squares = GetUniqueSquares(bb[i]);
                for (int j = 0; j < squares.Length; ++j)
                {
                    attackMask |= GetMoves(board, side, Piece.ROOK, squares[j]);
                }
            }
            bb = GetUniqueBitboards(side == Side.WHITE ? board.WhiteQueen : board.BlackQueen);
            for (int i = 0; i < bb.Length; ++i)
            {
                squares = GetUniqueSquares(bb[i]);
                for (int j = 0; j < squares.Length; ++j)
                {
                    attackMask |= GetMoves(board, side, Piece.QUEEN, squares[j]);
                }
            }
            bb = GetUniqueBitboards(side == Side.WHITE ? board.WhitePawns : board.BlackPawns);
            for (int i = 0; i < bb.Length; ++i)
            {
                squares = GetUniqueSquares(bb[i]);
                for (int j = 0; j < squares.Length; ++j)
                {
                    attackMask |= GetMoves(board, side, side == Side.WHITE ? Piece.WHITEPAWN : Piece.BLACKPAWN, squares[j]);
                }
            }
            return attackMask;
        }

        /// <summary>
        /// Generates the attack bitboard for a bishop on the provided square with the provided board occupancy
        /// </summary>
        /// <param name="square">The index of the square the bishop is on</param>
        /// <param name="occupancy">A bitboard representing all pieces on the board</param>
        /// <returns>A bitboard representing reachable targets for the bishop</returns>
        ulong GenBishopAttacks(int square, ulong occupancy)
        {
            ulong attacks = 0UL;
            int r, f;
            int tr = square / 8;
            int tf = square % 8;
            for (r = tr + 1, f = tf + 1; r <= 7 && f <= 7; r++, f++)
            {
                attacks |= (1UL << (r * 8 + f));
                if (((1UL << (r * 8 + f)) & occupancy) > 0) break;
            }
            for (r = tr - 1, f = tf + 1; r >= 0 && f <= 7; r--, f++)
            {
                attacks |= (1UL << (r * 8 + f));
                if (((1UL << (r * 8 + f)) & occupancy) > 0) break;
            }
            for (r = tr + 1, f = tf - 1; r <= 7 && f >= 0; r++, f--)
            {
                attacks |= (1UL << (r * 8 + f));
                if (((1UL << (r * 8 + f)) & occupancy) > 0) break;
            }
            for (r = tr - 1, f = tf - 1; r >= 0 && f >= 0; r--, f--)
            {
                attacks |= (1UL << (r * 8 + f));
                if (((1UL << (r * 8 + f)) & occupancy) > 0) break;
            }
            return attacks;
        }

        /// <summary>
        /// Generates the attack bitboard for a rook on the provided square with the provided board occupancy
        /// </summary>
        /// <param name="square">The index of the square the rook is on</param>
        /// <param name="occupancy">A bitboard representing all pieces on the board</param>
        /// <returns>A bitboard representing reachable targets for the rook</returns>
        ulong GenRookAttacks(int square, ulong occupancy)
        {
            ulong attacks = 0UL;
            int r, f;
            int tr = square / 8;
            int tf = square % 8;
            for (r = tr + 1; r <= 7; r++)
            {
                attacks |= (1UL << (r * 8 + tf));
                if (((1UL << (r * 8 + tf)) & occupancy) > 0) break;
            }
            for (r = tr - 1; r >= 0; r--)
            {
                attacks |= (1UL << (r * 8 + tf));
                if (((1UL << (r * 8 + tf)) & occupancy) > 0) break;
            }

            for (f = tf + 1; f <= 7; f++)
            {
                attacks |= (1UL << (tr * 8 + f));
                if (((1UL << (tr * 8 + f)) & occupancy) > 0) break;
            }

            for (f = tf - 1; f >= 0; f--)
            {
                attacks |= (1UL << (tr * 8 + f));
                if (((1UL << (tr * 8 + f)) & occupancy) > 0) break;
            }
            return attacks;
        }

        /// <summary>
        /// Generates the movement bitboard for a pawn of the provided color on the provided square
        /// </summary>
        /// <param name="board">The board the pawn is on</param>
        /// <param name="side">The color of the pawn</param>
        /// <param name="square">The index of the square the pawn is on</param>
        /// <returns>A bitboard representing reachable targets for the pawn, including en-passant</returns>
        ulong GenPawnMoves(Chessboard board, Side side, int square)
        {
            ulong pos = (1UL << square);
            ulong p1, p2, a1, a2, a;
            if (side == Side.WHITE)
            {
                p1 = (pos << 8) & ~board.AllPieces;
                p2 = ((p1 & Masks.MASKRANK3) << 8) & ~board.AllPieces;
                a1 = (pos & Masks.CLEARFILEH) << 7;
                a2 = (pos & Masks.CLEARFILEA) << 9;
                a = (a1 | a2) & board.BlackPieces | (a1 | a2) & board.EnPassant;
            }
            else
            {
                p1 = (pos >> 8) & ~board.AllPieces;
                p2 = ((p1 & Masks.MASKRANK6) >> 8) & ~board.AllPieces;
                a1 = (pos & Masks.CLEARFILEA) >> 7;
                a2 = (pos & Masks.CLEARFILEH) >> 9;
                a = (a1 | a2) & board.WhitePieces | (a1 | a2) & board.EnPassant;
            }
            return (a | p1 | p2);
        }

        ulong GenKingMoves(Chessboard board, Side side, int square)
        {
            return (KingMasks[square] & ((side == Side.WHITE) ? ~board.WhitePieces : ~board.BlackPieces)) &
                ~GenAttackMask(board, (side == Side.WHITE) ? Side.BLACK : Side.WHITE);
        }

        /// <summary>
        /// Generates an occupancy bitboard from the provided square and the provided attack mask
        /// </summary>
        /// <param name="index">The index of the bitboard</param>
        /// <param name="bitCount">The number of set bits in the attack mask</param>
        /// <param name="mask">The attack mask</param>
        /// <returns>The generated occupancy bitboard</returns>
        ulong SetOccupancy(ulong index, int bitCount, ulong mask)
        {
            ulong occupancy = 0UL;
            for (int i = 0; i < bitCount; ++i)
            {
                int square = GetLS1BIndex(mask);
                PopBit(ref mask, square);
                if ((index & (1UL << i)) > 0)
                {
                    occupancy |= (1UL << square);
                }
            }
            return occupancy;
        }

        /// <summary>
        /// Returns a bitboard containing all legal moves for the piece
        /// TODO: implement castling, checkmate, pins
        /// </summary>
        /// <param name="board">The board the piece is on</param>
        /// <param name="side">The color of the piece</param>
        /// <param name="piece">The piece</param>
        /// <param name="square">The square the piece is on</param>
        /// <returns></returns>
        public ulong GetMoves(Chessboard board, Side side, Piece piece, int square)
        {
            ulong res = 0L;
            ulong occupancy = board.AllPieces;
            ulong occupancyCPY = board.AllPieces;
            switch (piece)
            {
                case Piece.QUEEN:
                    occupancy &= BishopMasks[square];
                    occupancy *= BishopMagics[square];
                    occupancy >>= BishopShift[square];
                    occupancyCPY &= RookMasks[square];
                    occupancyCPY *= RookMagics[square];
                    occupancyCPY >>= RookShift[square];
                    res = (BishopAttacks[square, occupancy] | RookAttacks[square, occupancyCPY]);
                    break;
                case Piece.BISHOP:
                    occupancy &= BishopMasks[square];
                    occupancy *= BishopMagics[square];
                    occupancy >>= BishopShift[square];
                    res = BishopAttacks[square, occupancy];
                    break;
                case Piece.ROOK:
                    occupancy &= RookMasks[square];
                    occupancy *= RookMagics[square];
                    occupancy >>= RookShift[square];
                    res = RookAttacks[square, occupancy];
                    break;
                case Piece.WHITEPAWN:
                    res = GenPawnMoves(board, side, square);
                    break;
                case Piece.BLACKPAWN:
                    res = GenPawnMoves(board, side, square);
                    break;
                case Piece.KING:
                    res = GenKingMoves(board, side, square);
                    break;
                case Piece.KNIGHT:
                    res = KnightMasks[square];
                    break;
            }
            return res & ((side == Side.WHITE) ? ~board.WhitePieces : ~board.BlackPieces);
        }

        /* INIT */
        /// <summary>
        /// Initializes the arrays of precalculated attacking bitboards for the selected piece
        /// </summary>
        /// <param name="bishop">Boolean that switches the generation from bishop (true) to rook (false)</param>
        public void InitSliders(bool bishop)
        {
            for (int square = 0; square < 64; ++square)
            {
                ulong attackMask = (bishop) ? BishopMasks[square] : RookMasks[square];
                int bitCount = CountBits(attackMask);
                ulong occupancyIndexes = (1UL << bitCount);
                for (ulong index = 0; index < occupancyIndexes; ++index)
                {
                    if (bishop)
                    {
                        ulong occupancy = SetOccupancy(index, bitCount, attackMask);
                        uint magicIndex = (uint)((occupancy * BishopMagics[square]) >> BishopShift[square]);
                        BishopAttacks[square, magicIndex] = GenBishopAttacks(square, occupancy);
                    }
                    else
                    {
                        ulong occupancy = SetOccupancy(index, bitCount, attackMask);
                        uint magicIndex = (uint)((occupancy * RookMagics[square]) >> RookShift[square]);
                        RookAttacks[square, magicIndex] = GenRookAttacks(square, occupancy);
                    }
                }
            }
        }

        /// <summary>
        /// Initialize both the rook and the bishops' pre-calculated array of attacking bitboards
        /// </summary>
        public void Init()
        {
            InitSliders(true);
            InitSliders(false);
        }
    }
}