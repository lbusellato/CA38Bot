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
            await Bot.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: res,
                replyMarkup: inlineKeyboard
            );
        }

        private static Piece p;
        // Process Inline Keyboard callback data
        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            using GamesDbContext db = new GamesDbContext();
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;
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