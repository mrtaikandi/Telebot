namespace TelebotConsole
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Framework.Configuration;
    using Microsoft.Framework.Runtime;

    using Newtonsoft.Json;

    using Taikandi.Telebot;
    using Taikandi.Telebot.Types;

    public class Program
    {
        #region Constants and Fields

        private readonly string _telegramApiKey;

        private int _offset;

        private Telebot _telebot;

        #endregion

        #region Constructors and Destructors

        public Program(IApplicationEnvironment appEnv)
        {
            this.Configuration = new ConfigurationBuilder(appEnv.ApplicationBasePath).AddUserSecrets().Build();
            this._telegramApiKey = this.Configuration.Get("TelegramApiKey");

            var offset = this.Configuration.Get("UpdateOffset");
            if( !string.IsNullOrWhiteSpace(offset) )
                this._offset = int.Parse(offset);
        }

        #endregion

        #region Properties

        private IConfiguration Configuration { get; }

        #endregion

        #region Public Methods

        public async Task Main(string[] args)
        {
            Console.WriteLine("Running Telebot with key: {0}", this._telegramApiKey);
            this._telebot = new Telebot(this._telegramApiKey);

            try
            {
                while( true )
                {
                    var update = await this._telebot.GetUpdatesAsync(this._offset);
                    if( update.Ok && update.Result.Any() )
                    {
                        Dump(update);

                        this._offset = update.Result.Max(u => u.Id) + 1;
                        this.Configuration.Set("UpdateOffset", this._offset.ToString());

                        await this.EchoAllAsync(update.Result);
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

        private static void LogException(Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(ex);

            Console.ResetColor();
        }

        #endregion

        #region Methods

        private static void Dump<TResult>(TelegramResult<TResult> result)
        {
            var serializedResult = JsonConvert.SerializeObject(result, Formatting.Indented);
            Console.Write(serializedResult);
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
            await this._telebot.SendMessageAsync(message.Chat.Id, message.Text);
        }

        #endregion
    }
}