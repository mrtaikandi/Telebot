// ReSharper disable ConsiderUsingConfigureAwait
namespace TelebotConsole
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;

    using Newtonsoft.Json;

    using Taikandi.Telebot;
    using Taikandi.Telebot.Types;

    internal class TelebotConsole
    {
        #region Fields

        private readonly string _telegramApiKey;

        private long _offset;

        private Telebot _telebot;

        #endregion

        #region Constructors and Destructors

        public TelebotConsole()
        {
            var builder = new ConfigurationBuilder();
            builder.AddUserSecrets();
            var configuration = builder.Build();

            // Get Telegram API key from user secrets.
            // https://docs.asp.net/en/latest/security/app-secrets.html
            this._telegramApiKey = "100639095:AAFetqG3-uHEFaBIr_tOc0Tlvgc-RTlciec"; // configuration["TelegramApiKey"];

            // Identifier of the first update to be returned. Must be greater by one than the highest among
            // the identifiers of previously received updates. By default, updates starting with the earliest
            // unconfirmed update are returned. An update is considered confirmed as soon as getUpdates is
            // called with an offset higher than its update_id.
            this._offset = 684029274;
        }

        #endregion

        #region Public Methods and Operators

        public async Task Run()
        {
            Console.WriteLine("Running Telebot with key: {0}", this._telegramApiKey);
            this._telebot = new Telebot(this._telegramApiKey);

            while( true )
            {
                try
                {
                    var updates = (await this._telebot.GetUpdatesAsync(this._offset)).ToList();
                    if( updates.Any() )
                    {
                        this._offset = updates.Max(u => u.Id) + 1;

                        foreach( var update in updates )
                        {
                            switch( update.Type )
                            {
                                case UpdateType.Message:
                                    await this.CheckMessages(update);
                                    break;
                                case UpdateType.InlineQuery:
                                    await this.CheckInlineQuery(update);
                                    break;
                                case UpdateType.ChosenInlineResult:
                                    this.CheckChosenInlineResult(update);
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                    }

                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
                catch( Exception ex )
                {
                    LogException(ex);

                    Console.WriteLine("Press 'Enter' to continue...");
                    Console.ReadLine();

                    WriteLine(ConsoleColor.DarkGray, "----------------------------------------------");
                }
            }            
        }

        private void CheckChosenInlineResult(Update update)
        {
            WriteLine(ConsoleColor.Blue, "Received ChosenInlineResult:");
            Dump(update, ConsoleColor.Blue);
        }

        private async Task CheckInlineQuery(Update update)
        {
            WriteLine(ConsoleColor.Green, "Received InlineQuery:");
            Dump(update, ConsoleColor.Green);

            var results = new InlineQueryResult[]
                                         {
                                             new InlineQueryResultArticle(
                                                     Guid.NewGuid().ToString("N"),
                                                     "This is a title",
                                                     "This is a message."
                                                     ) {ParseMode = ParseMode.Markdown},
                                             new InlineQueryResultPhoto(
                                                     Guid.NewGuid().ToString("N"),
                                                     "https://telegram.org/file/811140636/1/hzUbyxse42w/4cd52d0464b44e1e5b",
                                                     "https://telegram.org/file/811140636/1/hzUbyxse42w/4cd52d0464b44e1e5b"
                                                     ),
                                             new InlineQueryResultGif(
                                                     Guid.NewGuid().ToString("N"),
                                                     "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2c/Rotating_earth_%28large%29.gif/200px-Rotating_earth_%28large%29.gif",
                                                     "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2c/Rotating_earth_%28large%29.gif/200px-Rotating_earth_%28large%29.gif"
                                                     )
                                         };

            var answerId = update.InlineQuery.Id;
            WriteLine(ConsoleColor.DarkGreen, "Sending: ");
            WriteLine(ConsoleColor.DarkGreen, $"Answer Id: {answerId}");
            Dump(results, ConsoleColor.DarkGreen);

            await this._telebot.AnswerInlineQueryAsync(answerId, results);
        }

        #endregion

        #region Methods

        private static void Dump<TResult>(TResult response, ConsoleColor color = ConsoleColor.White)
        {
            var serializedResult = JsonConvert.SerializeObject(response, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            WriteLine(color, serializedResult);
        }

        private static void WriteLine(ConsoleColor color, string message)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);

            Console.ResetColor();
        }

        private static void LogException(Exception ex)
        {
            WriteLine(ConsoleColor.Red, ex.ToString());
        }

        private Task CheckMessages(Update update)
        {
            Dump(update);

            if( string.IsNullOrWhiteSpace(update.Message?.Text) )
                return Task.FromResult(0);

            var message = update.Message.Text.ToLowerInvariant();
            switch( message )
            {
                case "/sendphoto":
                    return this.SendPhoto(update);
                case "/chataction":
                    return this.SendChatAction(update);
                default:
                    return this.EchoAsync(update);
            }
        }

        private async Task EchoAllAsync(IEnumerable<Update> updates)
        {
            foreach( var update in updates )
            {
                await this.EchoAsync(update);
            }
        }

        private async Task EchoAsync(Update update)
        {
            if( update.Message == null )
                return;

            var message = update.Message;
            await this._telebot.SendMessageAsync(message, message.Text);
        }

        private Task SendChatAction(Update update)
        {
            return this._telebot.SendChatAction(update.Message.Chat.Id.ToString(), ChatAction.Typing);
        }

        private Task SendPhoto(Update update)
        {
            var actionTask = this._telebot.SendChatAction(update.Message.Chat.Id.ToString(), ChatAction.UploadPhoto);
            var sendTask = this._telebot.SendPhotoFromFileAsync(update.Message.Chat.Id.ToString(), @"D:\Temp\brekeke-frog-symbol.jpg", "The Frog!");

            return Task.WhenAll(actionTask, sendTask);
        }

        #endregion
    }
}