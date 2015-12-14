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
            this._telegramApiKey = configuration["TelegramApiKey"];

            // Identifier of the first update to be returned. Must be greater by one than the highest among
            // the identifiers of previously received updates. By default, updates starting with the earliest
            // unconfirmed update are returned. An update is considered confirmed as soon as getUpdates is
            // called with an offset higher than its update_id.
            this._offset = 684029000;
        }

        #endregion

        #region Public Methods and Operators

        public async Task Run()
        {
            Console.WriteLine("Running Telebot with key: {0}", this._telegramApiKey);
            this._telebot = new Telebot(this._telegramApiKey);

            try
            {
                while( true )
                {
                    var update = (await this._telebot.GetUpdatesAsync(this._offset)).ToList();                    
                    if( update.Any() )
                    {
                        Dump(update);
                        this._offset = update.Max(u => u.Id) + 1;

                        foreach( var result in update )
                        {
                            await this.CheckMessages(result);
                        }
                    }

                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
            catch( Exception ex )
            {
                LogException(ex);
            }

            Console.WriteLine("Press any key to exit.");
            Console.Read();
        }

        #endregion

        #region Methods

        private static void Dump<TResult>(TResult response)
        {
            var serializedResult = JsonConvert.SerializeObject(response, Formatting.Indented);
            Console.Write(serializedResult);
        }

        private static void LogException(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex);

            Console.ResetColor();
        }

        private Task CheckMessages(Update update)
        {
            if( string.IsNullOrWhiteSpace(update.Message.Text) )
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