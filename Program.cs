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
     * TODO: Re-implement playing mechanics
     * TODO: Implement en passant
     * TODO: Implement castling
     * TODO: Implement check
     * TODO: Implement checkmate
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
        static Message prevBoard = null;
        static async Task SendInlineKeyboard(Message message, List<Move> keys)
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
                if(prevBoard != null)
                {
                    await Bot.DeleteMessageAsync(
                        chatId: prevBoard.Chat.Id,
                        messageId: prevBoard.MessageId
                    );
                }
                await Bot.DeleteMessageAsync(
                    chatId: message.Chat.Id,
                    messageId: message.MessageId
                ); 
                var file = new InputOnlineFile(board.FENToPng());
                prevBoard = await Bot.SendPhotoAsync(
                    chatId: message.Chat.Id,
                    photo: file
                    );
                res = "Tocca a te";
                List<InlineKeyboardButton> keyList = new List<InlineKeyboardButton>();
                List<List<InlineKeyboardButton>> keyListList = new List<List<InlineKeyboardButton>>();
                int count = 0;
                int j = 0;
                string duplicate;
                foreach (Move m in keys)
                {
                    duplicate = "";
                    if (m.From != m.To)
                    {
                        for (int i = 0; i < keys.Count(); ++i)
                        {
                            if (i == j) continue;
                            if (m.To == keys.ElementAt(i).To)
                            {
                                if (m.From[0] == keys.ElementAt(i).From[0])
                                {
                                    duplicate = m.From[1].ToString();
                                }
                                else if (m.From[1] == keys.ElementAt(i).From[1])
                                {
                                    duplicate = m.From[0].ToString();
                                }
                                else 
                                {
                                    duplicate = m.From[0].ToString();
                                }
                            }
                        }
                    }
                    j++;
                    string text = (m.From == m.To) ? m.Piece : (m.Piece != "P" ? m.Piece : "") + duplicate + m.Captures + m.To;
                    string callbackData = (m.From == m.To) ? m.Piece : m.From + m.To;
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

            var callbackQuery = callbackQueryEventArgs.CallbackQuery;

            List<Move> keys = new List<Move>();

            if (db.Games.SingleOrDefault(g => g.ChatID == callbackQuery.Message.Chat.Id) != null)
            {
                if (db.Games.SingleOrDefault(g => g.ChatID == callbackQuery.Message.Chat.Id).BotGame == null)
                {
                    string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR";
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

                    keys = board.GetMovablePieces(Side.WHITE);
                }
                else
                {
                    keys = callbackQuery.Data switch
                    {
                        "Back" => board.GetMovablePieces(Side.WHITE),
                        "P" => board.GetValidMoves(Side.WHITE, Piece.WHITEPAWN),
                        "R" => board.GetValidMoves(Side.WHITE, Piece.ROOK),
                        "N" => board.GetValidMoves(Side.WHITE, Piece.KNIGHT),
                        "B" => board.GetValidMoves(Side.WHITE, Piece.BISHOP),
                        "Q" => board.GetValidMoves(Side.WHITE, Piece.QUEEN),
                        "K" => board.GetValidMoves(Side.WHITE, Piece.KING),
                        _ => null
                    };
                    if(keys == null)
                    {
                        ushort from = (ushort)Move.GetSquareIndex(callbackQuery.Data.Substring(0, 2));
                        ushort to = (ushort)Move.GetSquareIndex(callbackQuery.Data.Substring(2, 2));
                        board.Move(new Move(0, to, from, 0, 0, 0));
                        keys = board.GetMovablePieces(Side.WHITE);
                    }
                }
            }
            if(keys != null)
            {
                await SendInlineKeyboard(callbackQuery.Message, keys);
            }
        }
    }
}