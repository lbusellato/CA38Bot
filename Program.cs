using Ca38Bot.Board;
using Ca38Bot.DAL;
using Ca38Bot.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ca38Bot
{
    /* TO DO LIST 
     * 
     * TODO: Implement resign
     * TODO: Implement threefold repetition
     * TODO: Implement 50 move draw rule
     * 
     */
    public static class Program
    {
        private static readonly Chessboard board = new Chessboard();
        private static TelegramBotClient Bot;
        private static readonly Random rng = new Random();

        public static async Task Main()
        {
            StreamReader file = new StreamReader("token.txt");
            string token = file.ReadLine();
            file.Close();
            Bot = new TelegramBotClient(token);

            var me = await Bot.GetMeAsync();
            Console.Title = me.Username;

            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;

            Bot.StartReceiving(Array.Empty<UpdateType>());
            Console.WriteLine($"Start listening for @{me.Username}");

            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            using GamesDbContext db = new GamesDbContext();
            if (db.Games.SingleOrDefault(g => g.ChatID == messageEventArgs.Message.Chat.Id) == null)
            {
                db.Games.Add(new Models.Games { ChatID = messageEventArgs.Message.Chat.Id });
                db.SaveChanges();
            }

            var message = messageEventArgs.Message;
            if (message == null || message.Type != MessageType.Text)
                return;

            switch (message.Text.Split(' ').First())
            {
                // Send inline keyboard
                case "/gioca":
                    await SendInlineKeyboard(message, null, null);
                    break;
                case "/fen":
                    string fen = "";
                    if (db.Games.SingleOrDefault(g => g.ChatID == messageEventArgs.Message.Chat.Id) != null)
                    {
                        if (db.Games.SingleOrDefault(g => g.ChatID == messageEventArgs.Message.Chat.Id).BotGame != null)
                        {
                            fen = db.Games.SingleOrDefault(g => g.ChatID == messageEventArgs.Message.Chat.Id).BotGame;
                        }
                        else
                        {
                            fen = "Non abbiamo una partita in corso";
                        }
                    }
                    else
                    {
                        fen = "Non abbiamo una partita in corso";
                    }
                    await Bot.SendTextMessageAsync(
                        chatId: message.Chat.Id,
                        text: fen,
                        parseMode: ParseMode.Html
                    );
                    break;
                case "/reset":
                    Games g = db.Games.SingleOrDefault(g => g.ChatID == messageEventArgs.Message.Chat.Id);
                    if (g != null)
                    {
                        db.Games.Remove(g);
                        db.SaveChanges();
                    }
                    break;
                default:
                    break;
            }
        }

        // Send inline keyboard
        // You can process responses in BotOnCallbackQueryReceived handler
        static async Task SendInlineKeyboard(Message message, List<Move> keys, string botMove)
        {
            using GamesDbContext db = new GamesDbContext();
            await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            string res = "";
            InlineKeyboardMarkup inlineKeyboard = new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Something went wrong", "-"));
            if (db.Games.SingleOrDefault(g => g.ChatID == message.Chat.Id) != null && 
                db.Games.SingleOrDefault(g => g.ChatID == message.Chat.Id).BotGame == null)
            {
                res = "Con che colore vuoi giocare?";
                inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                        InlineKeyboardButton.WithCallbackData("Bianco", "w"),
                        InlineKeyboardButton.WithCallbackData("Casuale", "r"),
                        InlineKeyboardButton.WithCallbackData("Nero", "b"),
                });
            }
            else
            {
                int prevBoard = db.Games.SingleOrDefault(g => g.ChatID == message.Chat.Id).PrevBoardID;
                if (prevBoard != 0)
                {
                    await Bot.DeleteMessageAsync(
                        chatId: message.Chat.Id,
                        messageId: prevBoard
                    );
                }
                await Bot.DeleteMessageAsync(
                    chatId: message.Chat.Id,
                    messageId: message.MessageId
                ); 
                var file = new InputOnlineFile(board.FENToPng());
                Message msg = await Bot.SendPhotoAsync(
                    chatId: message.Chat.Id,
                    photo: file
                    );
                db.Games.SingleOrDefault(g => g.ChatID == message.Chat.Id).PrevBoardID = msg.MessageId;
                db.SaveChanges();
                res = "Tocca a te" + (botMove == null ? "" : ", ho mosso " + botMove);
                List<InlineKeyboardButton> keyList = new List<InlineKeyboardButton>();
                List<List<InlineKeyboardButton>> keyListList = new List<List<InlineKeyboardButton>>();
                int count = 0;
                foreach (Move m in keys)
                {
                    string text = (m.From == m.To) ? m.Piece : (m.Piece != "P" ? m.Piece : (m.Captures == "x" && m.Duplicate == "0" ? m.From[0].ToString() : "")) + (m.Duplicate == "0" ? "" : m.Duplicate) + m.Captures + m.To;
                    string callbackData = ((m.From == m.To) ? m.Piece : m.From + m.To) + "/" + text;
                    keyList.Add(InlineKeyboardButton.WithCallbackData(text, callbackData));
                    count++;
                    if(count > 5)
                    {
                        keyListList.Add(keyList);
                        keyList = new List<InlineKeyboardButton>();
                        count = 0;
                    }
                }
                keyListList.Add(keyList);
                if (keyListList.Count() == 0)
                {
                    inlineKeyboard = new InlineKeyboardMarkup(InlineKeyboardButton.WithCallbackData("Something went wrong", "-"));
                }
                else
                {
                    if (keys.ElementAt(0).From != keys.ElementAt(0).To)
                    {
                        keyListList.Add(new List<InlineKeyboardButton>() { InlineKeyboardButton.WithCallbackData("Indietro", "Back")});
                    }
                    inlineKeyboard = new InlineKeyboardMarkup(keyListList);
                }
            }
            await Bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: res,
                replyMarkup: inlineKeyboard
            );
        }

        // Process Inline Keyboard callback data
        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            using GamesDbContext db = new GamesDbContext();
            string botMove = null;
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;
            string data = callbackQuery.Data.Split("/")[0];
            string playerMove = callbackQuery.Data.Split("/").Length > 1 ? callbackQuery.Data.Split("/")[1] : null;
            List<Move> keys = new List<Move>();

            if (db.Games.SingleOrDefault(g => g.ChatID == callbackQuery.Message.Chat.Id) != null)
            {
                if (db.Games.SingleOrDefault(g => g.ChatID == callbackQuery.Message.Chat.Id).BotGame == null)
                {
                    string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
                    string first = data switch
                    {
                        "w" => "/p",
                        "r" => (rng.Next(0, 2) == 0) ? "/p" : "/b",
                        "b" => "/b",
                        _ => "/p"
                    };
                    fen += first;
                    db.Games.SingleOrDefault(g => g.ChatID == callbackQuery.Message.Chat.Id).BotGame = fen;
                    db.SaveChanges();
                    board.LoadFEN(fen);
                    keys = board.GetMovablePieces(Side.WHITE);

                    board.player = true;
                    if (first == "/b")
                    {
                        board.player = false;
                        List<Move> botMovablePieces = new List<Move>();
                        botMovablePieces = board.GetMovablePieces(Side.WHITE);
                        string p = botMovablePieces.ElementAt(rng.Next(0, botMovablePieces.Count())).Piece;
                        List<Move> botMoves = p switch
                        {
                            "P" => board.GetValidMoves(Side.WHITE, Piece.WHITEPAWN),
                            "R" => board.GetValidMoves(Side.WHITE, Piece.ROOK),
                            "N" => board.GetValidMoves(Side.WHITE, Piece.KNIGHT),
                            "B" => board.GetValidMoves(Side.WHITE, Piece.BISHOP),
                            "Q" => board.GetValidMoves(Side.WHITE, Piece.QUEEN),
                            "K" => board.GetValidMoves(Side.WHITE, Piece.KING),
                            _ => null
                        };
                        int rnd = rng.Next(0, botMoves.Count());
                        Move move = botMoves.ElementAt(rnd);
                        ushort from = (ushort)Move.GetSquareIndex(move.From);
                        ushort to = (ushort)Move.GetSquareIndex(move.To);
                        board.Make(new Move(0, to, from, 0, 0, 0), board.player ? Side.BLACK : Side.WHITE);
                        db.Games.SingleOrDefault(g => g.ChatID == callbackQuery.Message.Chat.Id).BotGame = fen;
                        db.SaveChanges();
                        botMove = (move.From == move.To) ? move.Piece : (move.Piece != "P" ? move.Piece : (move.Captures == "x" && move.Duplicate == "0" ? move.From[0].ToString() : "")) + (move.Duplicate == "0" ? "" : move.Duplicate) + move.Captures + move.To;
                        string history = db.Games.SingleOrDefault(g => g.ChatID == callbackQuery.Message.Chat.Id).GameHistory;
                        history += botMove + ".";
                        db.Games.SingleOrDefault(g => g.ChatID == callbackQuery.Message.Chat.Id).GameHistory = history;
                        db.SaveChanges();

                        keys = board.GetMovablePieces(Side.BLACK);
                    }
                }
                else if ("rbw".IndexOf(callbackQuery.Data) == -1)
                {
                    board.LoadFEN(db.Games.SingleOrDefault(g => g.ChatID == callbackQuery.Message.Chat.Id).BotGame);
                    keys = data switch
                    {
                        "Back" => board.GetMovablePieces(board.player ? Side.WHITE : Side.BLACK),
                        "P" => board.GetValidMoves(board.player ? Side.WHITE : Side.BLACK, board.player ? Piece.WHITEPAWN : Piece.BLACKPAWN),
                        "R" => board.GetValidMoves(board.player ? Side.WHITE : Side.BLACK, Piece.ROOK),
                        "N" => board.GetValidMoves(board.player ? Side.WHITE : Side.BLACK, Piece.KNIGHT),
                        "B" => board.GetValidMoves(board.player ? Side.WHITE : Side.BLACK, Piece.BISHOP),
                        "Q" => board.GetValidMoves(board.player ? Side.WHITE : Side.BLACK, Piece.QUEEN),
                        "K" => board.GetValidMoves(board.player ? Side.WHITE : Side.BLACK, Piece.KING),
                        _ => null
                    };
                    if(keys == null)
                    {
                        string history;
                        ushort from = (ushort)Move.GetSquareIndex(data.Substring(0, 2));
                        ushort to = (ushort)Move.GetSquareIndex(data.Substring(2, 2));
                        Move pMove = new Move(0, to, from, 0, 0, 0);
                        board.Make(pMove, board.player ? Side.WHITE : Side.BLACK);
                        if (playerMove != null)
                        {
                            history = db.Games.SingleOrDefault(g => g.ChatID == callbackQuery.Message.Chat.Id).GameHistory;
                            history += playerMove + ".";
                            db.Games.SingleOrDefault(g => g.ChatID == callbackQuery.Message.Chat.Id).GameHistory = history;
                            db.SaveChanges();
                        }
                        List<Move> botMovablePieces = new List<Move>();
                        botMovablePieces = board.GetMovablePieces(board.player ? Side.BLACK : Side.WHITE);
                        string p = botMovablePieces.ElementAt(rng.Next(0, botMovablePieces.Count())).Piece;
                        List<Move> botMoves = p switch
                        {
                            "P" => board.GetValidMoves(board.player ? Side.BLACK : Side.WHITE, board.player ? Piece.BLACKPAWN : Piece.WHITEPAWN),
                            "R" => board.GetValidMoves(board.player ? Side.BLACK : Side.WHITE, Piece.ROOK),
                            "N" => board.GetValidMoves(board.player ? Side.BLACK : Side.WHITE, Piece.KNIGHT),
                            "B" => board.GetValidMoves(board.player ? Side.BLACK : Side.WHITE, Piece.BISHOP),
                            "Q" => board.GetValidMoves(board.player ? Side.BLACK : Side.WHITE, Piece.QUEEN),
                            "K" => board.GetValidMoves(board.player ? Side.BLACK : Side.WHITE, Piece.KING),
                            _ => null
                        };
                        int rnd = rng.Next(0, botMoves.Count());
                        Move move = botMoves.ElementAt(rnd);
                        from = (ushort)Move.GetSquareIndex(move.From);
                        to = (ushort)Move.GetSquareIndex(move.To);
                        board.Make(new Move(0, to, from, 0, 0, 0), board.player ? Side.BLACK : Side.WHITE);
                        botMove = (move.From == move.To) ? move.Piece : (move.Piece != "P" ? move.Piece : (move.Captures == "x" && move.Duplicate == "0" ? move.From[0].ToString() : "")) + (move.Duplicate == "0" ? "" : move.Duplicate) + move.Captures + move.To;

                        history = db.Games.SingleOrDefault(g => g.ChatID == callbackQuery.Message.Chat.Id).GameHistory;
                        history += botMove + ".";
                        db.Games.SingleOrDefault(g => g.ChatID == callbackQuery.Message.Chat.Id).GameHistory = history;
                        db.SaveChanges();

                        keys = board.GetMovablePieces(board.player ? Side.WHITE : Side.BLACK);
                    }
                }
            }
            if(keys != null && keys.Count() > 0)
            {
                db.Games.SingleOrDefault(g => g.ChatID == callbackQuery.Message.Chat.Id).BotGame = board.fen;
                db.SaveChanges();
                await SendInlineKeyboard(callbackQuery.Message, keys, botMove);
            }
        }
    }
}