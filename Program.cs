using Ca38Bot.Board;
using Ca38Bot.DAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace Ca38Bot
{
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
            Bot.OnInlineQuery += BotOnInlineQueryReceived;
            Bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            Bot.OnReceiveError += BotOnReceiveError;

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
                    await SendInlineKeyboard(message, null);
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
                default:
                    break;
            }
        }

        // Send inline keyboard
        // You can process responses in BotOnCallbackQueryReceived handler
        static async Task SendInlineKeyboard(Message message, string[] keys)
        {
            using GamesDbContext db = new GamesDbContext();
            await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);
            string game;
            string res1 = "";
            string res2 = "";
            // Simulate longer running task
            await Task.Delay(500);
            InlineKeyboardMarkup inlineKeyboard;
            if (db.Games.SingleOrDefault(g => g.ChatID == message.Chat.Id).BotGame == null)
            {
                res2 = "Con che colore vuoi giocare?";
                inlineKeyboard = new InlineKeyboardMarkup(new[]
                {
                        InlineKeyboardButton.WithCallbackData("Bianco", "w"),
                        InlineKeyboardButton.WithCallbackData("Casuale", "r"),
                        InlineKeyboardButton.WithCallbackData("Nero", "b"),
                });
            }
            else
            {
                game = db.Games.SingleOrDefault(g => g.ChatID == message.Chat.Id).BotGame;
                res1 += Render(game);
                List<List<InlineKeyboardButton>> keylistlist = new List<List<InlineKeyboardButton>>
                {
                    new List<InlineKeyboardButton>()
                };
                if (keys != null)
                {
                    res2 = "Tocca a te";
                    int cnt = 0;
                    int i = 0;
                    foreach (string s in keys)
                    {
                        if (s == null || s == "")
                        {
                            continue;
                        }
                        if(s.Length == 4)
                        {
                            keylistlist[i].Add(InlineKeyboardButton.WithCallbackData(s.Substring(2, 2), s));
                        }
                        else
                        {
                            keylistlist[i].Add(InlineKeyboardButton.WithCallbackData(s, s));
                        }
                        cnt++;
                        if (cnt > 6)
                        {
                            keylistlist.Add(new List<InlineKeyboardButton>());
                            cnt = 0;
                            i++;
                        }
                    }
                    string pieces = "PRNBQK";
                    if(pieces.IndexOf(keylistlist[0].ElementAt(0).Text) == -1)
                    {
                        keylistlist[i].Add(InlineKeyboardButton.WithCallbackData("Back", "Back"));
                    }
                }
                inlineKeyboard = new InlineKeyboardMarkup(keylistlist);
            }
            if (res1 != "")
            {
                await Bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: res1,
                    parseMode: ParseMode.Html
                );
            }
            if (res2 != "")
            {
                await Bot.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: res2,
                    replyMarkup: inlineKeyboard
                );
            }
        }

        private static string Render(string fen)
        {
            string res = "<pre>";
            string[] subs = fen.Split("/");
            for (int i = 0; i < subs.Length - 1; ++i)
            {
                res += (8 - i) + " ";
                for(int j = 0; j < subs[i].Length; ++j)
                {
                    if(Char.IsDigit(subs[i][j]))
                    {
                        Int32.TryParse(subs[i][j].ToString(), out int spaces);
                        for(int k = 0; k < spaces; ++k)
                        {
                            res += "- ";
                        }
                    }
                    else
                    {
                        res += subs[i][j].ToString() + " ";
                    }
                }
                res += "\n";
            }
            res += "  ";
            for(int i = 97; i < 105; ++i)
            {
                res += (char)i + " ";
            }
            res += "</pre>";
            return res;
        }

        private static string[] availableMoves = null;
        private static Chessboard.Piece p;
        // Process Inline Keyboard callback data
        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            using GamesDbContext db = new GamesDbContext();
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;
            string fen;
            //If we don't have a game in progress start a new one
            string[] movablePieces = null;
            if (db.Games.SingleOrDefault(g => g.ChatID == callbackQuery.Message.Chat.Id).BotGame == null)
            {
                fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
                string first = callbackQuery.Data switch
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
                movablePieces = board.GetMovablePieces(Chessboard.Side.WHITE);
            }
            else
            {
                fen = db.Games.SingleOrDefault(g => g.ChatID == callbackQuery.Message.Chat.Id).BotGame;
                board.LoadFEN(fen);
                string pieces = "PRNBQK";
                if(pieces.IndexOf(callbackQuery.Data) != -1)
                {
                    p = callbackQuery.Data switch
                    {
                        "P" => Chessboard.Piece.WHITEPAWN,
                        "R" => Chessboard.Piece.ROOK,
                        "N" => Chessboard.Piece.KNIGHT,
                        "B" => Chessboard.Piece.BISHOP,
                        "Q" => Chessboard.Piece.QUEEN,
                        "K" => Chessboard.Piece.KING,
                        _ => 0
                    };
                    availableMoves = board.GetValidMoves(Chessboard.Side.WHITE, p);
                }
                else
                {
                    if (callbackQuery.Data != "Back")
                    {
                        string from, to;
                        from = callbackQuery.Data.Substring(0, 2);
                        to = callbackQuery.Data.Substring(2, 2);
                        board.Move(from, to);
                        board.LoadFEN(board.fen);
                        db.Games.SingleOrDefault(g => g.ChatID == callbackQuery.Message.Chat.Id).BotGame = board.fen;
                        db.SaveChanges();

                        string[] botMovablePieces = board.GetMovablePieces(Chessboard.Side.BLACK);
                        int m = rng.Next(0, botMovablePieces.Length);
                        p = botMovablePieces.ElementAt(m) switch
                        {
                            "P" => Chessboard.Piece.BLACKPAWN,
                            "R" => Chessboard.Piece.ROOK,
                            "N" => Chessboard.Piece.KNIGHT,
                            "B" => Chessboard.Piece.BISHOP,
                            "Q" => Chessboard.Piece.QUEEN,
                            "K" => Chessboard.Piece.KING,
                            _ => 0
                        };
                        string o = (botMovablePieces.ElementAt(m) != "P") ? botMovablePieces.ElementAt(m) : "";
                        string[] botAvailableMoves = board.GetValidMoves(Chessboard.Side.BLACK, p);
                        m = rng.Next(0, botAvailableMoves.Length);
                        string move = botAvailableMoves.ElementAt(m);
                        from = move.Substring(0, 2);
                        to = move.Substring(2, 2);
                        o += to;
                        board.Move(from, to);
                        board.LoadFEN(board.fen);
                        db.Games.SingleOrDefault(g => g.ChatID == callbackQuery.Message.Chat.Id).BotGame = board.fen;
                        db.SaveChanges();

                        await Bot.SendTextMessageAsync(
                            chatId: callbackQuery.Message.Chat.Id,
                            text: "Ho mosso " + o,
                            parseMode: ParseMode.Html
                        );
                    }
                    movablePieces = board.GetMovablePieces(Chessboard.Side.WHITE);
                }
            }
            if(movablePieces == null)
            {
                await SendInlineKeyboard(callbackQuery.Message, availableMoves);
            }
            else
            {
                await SendInlineKeyboard(callbackQuery.Message, movablePieces);
            }
        }

        #region Inline Mode

        private static async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        {
            Console.WriteLine($"Received inline query from: {inlineQueryEventArgs.InlineQuery.From.Id}");

            InlineQueryResultBase[] results = {
            // displayed result
            new InlineQueryResultArticle(
                id: "3",
                title: "TgBots",
                inputMessageContent: new InputTextMessageContent(
                    "hello"
                )
            )
        };
            await Bot.AnswerInlineQueryAsync(
                inlineQueryId: inlineQueryEventArgs.InlineQuery.Id,
                results: results,
                isPersonal: true,
                cacheTime: 0
            );
        }

        private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        #endregion

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("Received error: {0} — {1}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message
            );
        }
    }
}