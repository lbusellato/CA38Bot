﻿using System;

namespace Ca38Bot.Board
{
    public static class Masks
    {
        public static ulong EMPTY = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000;
        public static ulong FULL =  0b11111111_11111111_11111111_11111111_11111111_11111111_11111111_11111111;
        /* WHITE PIECES STARTING POSITION */
        public static ulong WHITEPAWNS =   0b00000000_00000000_00000000_00000000_00000000_00000000_11111111_00000000;
        public static ulong WHITEROOKS =   0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_10000001;
        public static ulong WHITEKNIGHTS = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_01000010;
        public static ulong WHITEBISHOPS = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00100100;
        public static ulong WHITEQUEEN =   0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00010000;
        public static ulong WHITEKING =    0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00001000;
        /* BLACK PIECES STARTING POSITION */
        public static ulong BLACKPAWNS =   0b00000000_11111111_00000000_00000000_00000000_00000000_00000000_00000000;
        public static ulong BLACKROOKS =   0b10000001_00000000_00000000_00000000_00000000_00000000_00000000_00000000;
        public static ulong BLACKKNIGHTS = 0b01000010_00000000_00000000_00000000_00000000_00000000_00000000_00000000;
        public static ulong BLACKBISHOPS = 0b00100100_00000000_00000000_00000000_00000000_00000000_00000000_00000000;
        public static ulong BLACKQUEEN =   0b00010000_00000000_00000000_00000000_00000000_00000000_00000000_00000000;
        public static ulong BLACKKING =    0b00001000_00000000_00000000_00000000_00000000_00000000_00000000_00000000;
        /* FILE AND RANK MASKS */
        public static ulong MASKFILEA = 0b10000000_10000000_10000000_10000000_10000000_10000000_10000000_10000000;
        public static ulong MASKFILEB = 0b01000000_01000000_01000000_01000000_01000000_01000000_01000000_01000000;
        public static ulong MASKFILEC = 0b00100000_00100000_00100000_00100000_00100000_00100000_00100000_00100000;
        public static ulong MASKFILED = 0b00010000_00010000_00010000_00010000_00010000_00010000_00010000_00010000;
        public static ulong MASKFILEE = 0b00001000_00001000_00001000_00001000_00001000_00001000_00001000_00001000;
        public static ulong MASKFILEF = 0b00000100_00000100_00000100_00000100_00000100_00000100_00000100_00000100;
        public static ulong MASKFILEG = 0b00000010_00000010_00000010_00000010_00000010_00000010_00000010_00000010;
        public static ulong MASKFILEH = 0b00000001_00000001_00000001_00000001_00000001_00000001_00000001_00000001;
        public static ulong MASKRANK1 = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_11111111;
        public static ulong MASKRANK2 = 0b00000000_00000000_00000000_00000000_00000000_00000000_11111111_00000000;
        public static ulong MASKRANK3 = 0b00000000_00000000_00000000_00000000_00000000_11111111_00000000_00000000;
        public static ulong MASKRANK4 = 0b00000000_00000000_00000000_00000000_11111111_00000000_00000000_00000000;
        public static ulong MASKRANK5 = 0b00000000_00000000_00000000_11111111_00000000_00000000_00000000_00000000;
        public static ulong MASKRANK6 = 0b00000000_00000000_11111111_00000000_00000000_00000000_00000000_00000000;
        public static ulong MASKRANK7 = 0b00000000_11111111_00000000_00000000_00000000_00000000_00000000_00000000;
        public static ulong MASKRANK8 = 0b11111111_00000000_00000000_00000000_00000000_00000000_00000000_00000000;
        /* FILE AND RANK CLEARS */
        public static ulong CLEARFILEA = 0b01111111_01111111_01111111_01111111_01111111_01111111_01111111_01111111;
        public static ulong CLEARFILEB = 0b10111111_10111111_10111111_10111111_10111111_10111111_10111111_10111111;
        public static ulong CLEARFILEC = 0b11011111_11011111_11011111_11011111_11011111_11011111_11011111_11011111;
        public static ulong CLEARFILED = 0b11101111_11101111_11101111_11101111_11101111_11101111_11101111_11101111;
        public static ulong CLEARFILEE = 0b11110111_11110111_11110111_11110111_11110111_11110111_11110111_11110111;
        public static ulong CLEARFILEF = 0b11111011_11111011_11111011_11111011_11111011_11111011_11111011_11111011;
        public static ulong CLEARFILEG = 0b11111101_11111101_11111101_11111101_11111101_11111101_11111101_11111101;
        public static ulong CLEARFILEH = 0b11111110_11111110_11111110_11111110_11111110_11111110_11111110_11111110;
        public static ulong CLEARRANK1 = 0b11111111_11111111_11111111_11111111_11111111_11111111_11111111_00000000;
        public static ulong CLEARRANK2 = 0b11111111_11111111_11111111_11111111_11111111_11111111_00000000_11111111;
        public static ulong CLEARRANK3 = 0b11111111_11111111_11111111_11111111_11111111_00000000_11111111_11111111;
        public static ulong CLEARRANK4 = 0b11111111_11111111_11111111_11111111_00000000_11111111_11111111_11111111;
        public static ulong CLEARRANK5 = 0b11111111_11111111_11111111_00000000_11111111_11111111_11111111_11111111;
        public static ulong CLEARRANK6 = 0b11111111_11111111_00000000_11111111_11111111_11111111_11111111_11111111;
        public static ulong CLEARRANK7 = 0b11111111_00000000_11111111_11111111_11111111_11111111_11111111_11111111;
        public static ulong CLEARRANK8 = 0b00000000_11111111_11111111_11111111_11111111_11111111_11111111_11111111;
        /* SQUARE MASK */
        public static ulong MASKA1 = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_10000000;
        public static ulong MASKA2 = 0b00000000_00000000_00000000_00000000_00000000_00000000_10000000_00000000;
        public static ulong MASKA3 = 0b00000000_00000000_00000000_00000000_00000000_10000000_00000000_00000000;
        public static ulong MASKA4 = 0b00000000_00000000_00000000_00000000_10000000_00000000_00000000_00000000;
        public static ulong MASKA5 = 0b00000000_00000000_00000000_10000000_00000000_00000000_00000000_00000000;
        public static ulong MASKA6 = 0b00000000_00000000_10000000_00000000_00000000_00000000_00000000_00000000;
        public static ulong MASKA7 = 0b00000000_10000000_00000000_00000000_00000000_00000000_00000000_00000000;
        public static ulong MASKA8 = 0b10000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000;

        public static ulong MASKB1 = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_01000000;
        public static ulong MASKB2 = 0b00000000_00000000_00000000_00000000_00000000_00000000_01000000_00000000;
        public static ulong MASKB3 = 0b00000000_00000000_00000000_00000000_00000000_01000000_00000000_00000000;
        public static ulong MASKB4 = 0b00000000_00000000_00000000_00000000_01000000_00000000_00000000_00000000;
        public static ulong MASKB5 = 0b00000000_00000000_00000000_01000000_00000000_00000000_00000000_00000000;
        public static ulong MASKB6 = 0b00000000_00000000_01000000_00000000_00000000_00000000_00000000_00000000;
        public static ulong MASKB7 = 0b00000000_01000000_00000000_00000000_00000000_00000000_00000000_00000000;
        public static ulong MASKB8 = 0b01000000_00000000_00000000_00000000_00000000_00000000_00000000_00000000;

        public static ulong MASKC1 = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00100000;
        public static ulong MASKC2 = 0b00000000_00000000_00000000_00000000_00000000_00000000_00100000_00000000;
        public static ulong MASKC3 = 0b00000000_00000000_00000000_00000000_00000000_00100000_00000000_00000000;
        public static ulong MASKC4 = 0b00000000_00000000_00000000_00000000_00100000_00000000_00000000_00000000;
        public static ulong MASKC5 = 0b00000000_00000000_00000000_00100000_00000000_00000000_00000000_00000000;
        public static ulong MASKC6 = 0b00000000_00000000_00100000_00000000_00000000_00000000_00000000_00000000;
        public static ulong MASKC7 = 0b00000000_00100000_00000000_00000000_00000000_00000000_00000000_00000000;
        public static ulong MASKC8 = 0b00100000_00000000_00000000_00000000_00000000_00000000_00000000_00000000;

        public static ulong MASKD1 = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00010000;
        public static ulong MASKD2 = 0b00000000_00000000_00000000_00000000_00000000_00000000_00010000_00000000;
        public static ulong MASKD3 = 0b00000000_00000000_00000000_00000000_00000000_00010000_00000000_00000000;
        public static ulong MASKD4 = 0b00000000_00000000_00000000_00000000_00010000_00000000_00000000_00000000;
        public static ulong MASKD5 = 0b00000000_00000000_00000000_00010000_00000000_00000000_00000000_00000000;
        public static ulong MASKD6 = 0b00000000_00000000_00010000_00000000_00000000_00000000_00000000_00000000;
        public static ulong MASKD7 = 0b00000000_00010000_00000000_00000000_00000000_00000000_00000000_00000000;
        public static ulong MASKD8 = 0b00010000_00000000_00000000_00000000_00000000_00000000_00000000_00000000;

        public static ulong MASKE1 = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00001000;
        public static ulong MASKE2 = 0b00000000_00000000_00000000_00000000_00000000_00000000_00001000_00000000;
        public static ulong MASKE3 = 0b00000000_00000000_00000000_00000000_00000000_00001000_00000000_00000000;
        public static ulong MASKE4 = 0b00000000_00000000_00000000_00000000_00001000_00000000_00000000_00000000;
        public static ulong MASKE5 = 0b00000000_00000000_00000000_00001000_00000000_00000000_00000000_00000000;
        public static ulong MASKE6 = 0b00000000_00000000_00001000_00000000_00000000_00000000_00000000_00000000;
        public static ulong MASKE7 = 0b00000000_00001000_00000000_00000000_00000000_00000000_00000000_00000000;
        public static ulong MASKE8 = 0b00001000_00000000_00000000_00000000_00000000_00000000_00000000_00000000;

        public static ulong MASKF1 = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000100;
        public static ulong MASKF2 = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000100_00000000;
        public static ulong MASKF3 = 0b00000000_00000000_00000000_00000000_00000000_00000100_00000000_00000000;
        public static ulong MASKF4 = 0b00000000_00000000_00000000_00000000_00000100_00000000_00000000_00000000;
        public static ulong MASKF5 = 0b00000000_00000000_00000000_00000100_00000000_00000000_00000000_00000000;
        public static ulong MASKF6 = 0b00000000_00000000_00000100_00000000_00000000_00000000_00000000_00000000;
        public static ulong MASKF7 = 0b00000000_00000100_00000000_00000000_00000000_00000000_00000000_00000000;
        public static ulong MASKF8 = 0b00000100_00000000_00000000_00000000_00000000_00000000_00000000_00000000;

        public static ulong MASKG1 = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000010;
        public static ulong MASKG2 = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000010_00000000;
        public static ulong MASKG3 = 0b00000000_00000000_00000000_00000000_00000000_00000010_00000000_00000000;
        public static ulong MASKG4 = 0b00000000_00000000_00000000_00000000_00000010_00000000_00000000_00000000;
        public static ulong MASKG5 = 0b00000000_00000000_00000000_00000010_00000000_00000000_00000000_00000000;
        public static ulong MASKG6 = 0b00000000_00000000_00000010_00000000_00000000_00000000_00000000_00000000;
        public static ulong MASKG7 = 0b00000000_00000010_00000000_00000000_00000000_00000000_00000000_00000000;
        public static ulong MASKG8 = 0b00000010_00000000_00000000_00000000_00000000_00000000_00000000_00000000;

        public static ulong MASKH1 = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000000_00000001;
        public static ulong MASKH2 = 0b00000000_00000000_00000000_00000000_00000000_00000000_00000001_00000000;
        public static ulong MASKH3 = 0b00000000_00000000_00000000_00000000_00000000_00000001_00000000_00000000;
        public static ulong MASKH4 = 0b00000000_00000000_00000000_00000000_00000001_00000000_00000000_00000000;
        public static ulong MASKH5 = 0b00000000_00000000_00000000_00000001_00000000_00000000_00000000_00000000;
        public static ulong MASKH6 = 0b00000000_00000000_00000001_00000000_00000000_00000000_00000000_00000000;
        public static ulong MASKH7 = 0b00000000_00000001_00000000_00000000_00000000_00000000_00000000_00000000;
        public static ulong MASKH8 = 0b00000001_00000000_00000000_00000000_00000000_00000000_00000000_00000000;

        /* SQUARE CLEAR */
        public static ulong CLEARA1 = 0b11111111_11111111_11111111_11111111_11111111_11111111_11111111_01111111;
        public static ulong CLEARA2 = 0b11111111_11111111_11111111_11111111_11111111_11111111_01111111_11111111;
        public static ulong CLEARA3 = 0b11111111_11111111_11111111_11111111_11111111_01111111_11111111_11111111;
        public static ulong CLEARA4 = 0b11111111_11111111_11111111_11111111_01111111_11111111_11111111_11111111;
        public static ulong CLEARA5 = 0b11111111_11111111_11111111_01111111_11111111_11111111_11111111_11111111;
        public static ulong CLEARA6 = 0b11111111_11111111_01111111_11111111_11111111_11111111_11111111_11111111;
        public static ulong CLEARA7 = 0b11111111_01111111_11111111_11111111_11111111_11111111_11111111_11111111;
        public static ulong CLEARA8 = 0b01111111_11111111_11111111_11111111_11111111_11111111_11111111_11111111;

        public static ulong CLEARB1 = 0b11111111_11111111_11111111_11111111_11111111_11111111_11111111_10111111;
        public static ulong CLEARB2 = 0b11111111_11111111_11111111_11111111_11111111_11111111_10111111_11111111;
        public static ulong CLEARB3 = 0b11111111_11111111_11111111_11111111_11111111_10111111_11111111_11111111;
        public static ulong CLEARB4 = 0b11111111_11111111_11111111_11111111_10111111_11111111_11111111_11111111;
        public static ulong CLEARB5 = 0b11111111_11111111_11111111_10111111_11111111_11111111_11111111_11111111;
        public static ulong CLEARB6 = 0b11111111_11111111_10111111_11111111_11111111_11111111_11111111_11111111;
        public static ulong CLEARB7 = 0b11111111_10111111_11111111_11111111_11111111_11111111_11111111_11111111;
        public static ulong CLEARB8 = 0b10111111_11111111_11111111_11111111_11111111_11111111_11111111_11111111;

        public static ulong CLEARC1 = 0b11111111_11111111_11111111_11111111_11111111_11111111_11111111_11011111;
        public static ulong CLEARC2 = 0b11111111_11111111_11111111_11111111_11111111_11111111_11011111_11111111;
        public static ulong CLEARC3 = 0b11111111_11111111_11111111_11111111_11111111_11011111_11111111_11111111;
        public static ulong CLEARC4 = 0b11111111_11111111_11111111_11111111_11011111_11111111_11111111_11111111;
        public static ulong CLEARC5 = 0b11111111_11111111_11111111_11011111_11111111_11111111_11111111_11111111;
        public static ulong CLEARC6 = 0b11111111_11111111_11011111_11111111_11111111_11111111_11111111_11111111;
        public static ulong CLEARC7 = 0b11111111_11011111_11111111_11111111_11111111_11111111_11111111_11111111;
        public static ulong CLEARC8 = 0b11011111_11111111_11111111_11111111_11111111_11111111_11111111_11111111;

        public static ulong CLEARD1 = 0b11111111_11111111_11111111_11111111_11111111_11111111_11111111_11101111;
        public static ulong CLEARD2 = 0b11111111_11111111_11111111_11111111_11111111_11111111_11101111_11111111;
        public static ulong CLEARD3 = 0b11111111_11111111_11111111_11111111_11111111_11101111_11111111_11111111;
        public static ulong CLEARD4 = 0b11111111_11111111_11111111_11111111_11101111_11111111_11111111_11111111;
        public static ulong CLEARD5 = 0b11111111_11111111_11111111_11101111_11111111_11111111_11111111_11111111;
        public static ulong CLEARD6 = 0b11111111_11111111_11101111_11111111_11111111_11111111_11111111_11111111;
        public static ulong CLEARD7 = 0b11111111_11101111_11111111_11111111_11111111_11111111_11111111_11111111;
        public static ulong CLEARD8 = 0b11101111_11111111_11111111_11111111_11111111_11111111_11111111_11111111;

        public static ulong CLEARE1 = 0b11111111_11111111_11111111_11111111_11111111_11111111_11111111_11110111;
        public static ulong CLEARE2 = 0b11111111_11111111_11111111_11111111_11111111_11111111_11110111_11111111;
        public static ulong CLEARE3 = 0b11111111_11111111_11111111_11111111_11111111_11110111_11111111_11111111;
        public static ulong CLEARE4 = 0b11111111_11111111_11111111_11111111_11110111_11111111_11111111_11111111;
        public static ulong CLEARE5 = 0b11111111_11111111_11111111_11110111_11111111_11111111_11111111_11111111;
        public static ulong CLEARE6 = 0b11111111_11111111_11110111_11111111_11111111_11111111_11111111_11111111;
        public static ulong CLEARE7 = 0b11111111_11110111_11111111_11111111_11111111_11111111_11111111_11111111;
        public static ulong CLEARE8 = 0b11110111_11111111_11111111_11111111_11111111_11111111_11111111_11111111;

        public static ulong CLEARF1 = 0b11111111_11111111_11111111_11111111_11111111_11111111_11111111_11111011;
        public static ulong CLEARF2 = 0b11111111_11111111_11111111_11111111_11111111_11111111_11111011_11111111;
        public static ulong CLEARF3 = 0b11111111_11111111_11111111_11111111_11111111_11111011_11111111_11111111;
        public static ulong CLEARF4 = 0b11111111_11111111_11111111_11111111_11111011_11111111_11111111_11111111;
        public static ulong CLEARF5 = 0b11111111_11111111_11111111_11111011_11111111_11111111_11111111_11111111;
        public static ulong CLEARF6 = 0b11111111_11111111_11111011_11111111_11111111_11111111_11111111_11111111;
        public static ulong CLEARF7 = 0b11111111_11111011_11111111_11111111_11111111_11111111_11111111_11111111;
        public static ulong CLEARF8 = 0b11111011_11111111_11111111_11111111_11111111_11111111_11111111_11111111;

        public static ulong CLEARG1 = 0b11111111_11111111_11111111_11111111_11111111_11111111_11111111_11111101;
        public static ulong CLEARG2 = 0b11111111_11111111_11111111_11111111_11111111_11111111_11111101_11111111;
        public static ulong CLEARG3 = 0b11111111_11111111_11111111_11111111_11111111_11111101_11111111_11111111;
        public static ulong CLEARG4 = 0b11111111_11111111_11111111_11111111_11111101_11111111_11111111_11111111;
        public static ulong CLEARG5 = 0b11111111_11111111_11111111_11111101_11111111_11111111_11111111_11111111;
        public static ulong CLEARG6 = 0b11111111_11111111_11111101_11111111_11111111_11111111_11111111_11111111;
        public static ulong CLEARG7 = 0b11111111_11111101_11111111_11111111_11111111_11111111_11111111_11111111;
        public static ulong CLEARG8 = 0b11111101_11111111_11111111_11111111_11111111_11111111_11111111_11111111;

        public static ulong CLEARH1 = 0b11111111_11111111_11111111_11111111_11111111_11111111_11111111_11111110;
        public static ulong CLEARH2 = 0b11111111_11111111_11111111_11111111_11111111_11111111_11111110_11111111;
        public static ulong CLEARH3 = 0b11111111_11111111_11111111_11111111_11111111_11111110_11111111_11111111;
        public static ulong CLEARH4 = 0b11111111_11111111_11111111_11111111_11111110_11111111_11111111_11111111;
        public static ulong CLEARH5 = 0b11111111_11111111_11111111_11111110_11111111_11111111_11111111_11111111;
        public static ulong CLEARH6 = 0b11111111_11111111_11111110_11111111_11111111_11111111_11111111_11111111;
        public static ulong CLEARH7 = 0b11111111_11111110_11111111_11111111_11111111_11111111_11111111_11111111;
        public static ulong CLEARH8 = 0b11111110_11111111_11111111_11111111_11111111_11111111_11111111_11111111;
    }
}
