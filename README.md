What is Telebot
--------------------------------
Telebot is a well [documented](https://msdn.microsoft.com/en-us/library/b2s063f7%28vs.71%29.aspx?f=255&MSPPError=-2147217396) .NET client library for the [Telegram Bot API](https://core.telegram.org/bots/api) client written completely in C#.

Where can I get it?
--------------------------------
First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [Telebot](https://www.nuget.org/packages/Telebot/) from the package manager console:

    PM> Install-Package Telebot

How can I use it?
--------------------------------
> For Persian how-to guide, please [visit my blog](https://taikandi.com/blog/creating-telegram-bot-using-telebot/).

Simply instanciate `Telebot` class and use available methods.
`Telebot` internally uses [HttpClient](https://msdn.microsoft.com/en-us/library/system.net.http.httpclient%28v=vs.118%29.aspx) so, it is recommended not to dispose it after every request. 
I suggest to read the `Lifecycle` section of the [Designing Evolvable Web APIs with ASP.NET](http://chimera.labs.oreilly.com/books/1234000001708/ch14.html#_httpclient_class).

```csharp
// Provides a base class for communicating with Telegram Bot API servers.
private readonly Telebot _telebot = new Telebot("123456:ABC-DEF1234ghIkl-zyx57W2v1u123ew11");

public async Task RunAsync()
{
    // Used for getting only the unconfirmed updates.
    // More information at https://core.telegram.org/bots/api#getting-updates
    var offset = 0L;

    while( true )
    {
        // Use this method to receive incoming updates using long polling.
        // Or use Telebot.SetWebhook() method to specifiy a url to receive incoming updates.
        List<Update> updates = (await this._telebot.GetUpdatesAsync(offset).ConfigureAwait(false)).ToList();
        if( updates.Any() )
        {
            offset = updates.Max(u => u.Id) + 1;

            foreach( Update update in updates )
            {
                switch( update.Type )
                {
                    case UpdateType.Message:
                        await this.CheckMessagesAsync(update.Message).ConfigureAwait(false);
                        break;
                    case UpdateType.InlineQuery:
                        await this.CheckInlineQueryAsync(update).ConfigureAwait(false);
                        break;
                    case UpdateType.ChosenInlineResult:
                        this.CheckChosenInlineResult(update);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        await Task.Delay(TimeSpan.FromSeconds(1)).ConfigureAwait(false);
    }
}

private async Task CheckMessagesAsync(Message message)
{
    // Assume we are doing more than echoing stuff.
    if( message == null )
        return;

    // This method will tell the user that something is happening on the bot's side.
    // It is recommended to use this method when a response from the bot 
    // will take a noticeable amount of time to arrive.
    await this._telebot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing).ConfigureAwait(false);
    await Task.Delay(TimeSpan.FromSeconds(3)).ConfigureAwait(false);

    // This method will sends a text message (obviously).
    await this._telebot.SendMessageAsync(message.Chat.Id, message.Text ?? "Hmmmm").ConfigureAwait(false);
}

private async Task CheckInlineQueryAsync(Update update)
{
    // To see available inline query results:
    // https://core.telegram.org/bots/api#answerinlinequery
    var results = new InlineQueryResult[]
                      {
                          new InlineQueryResultArticle(
                              Guid.NewGuid().ToString("N"),
                              "This is a title",
                              "This is a message.")
                              {
                                  ParseMode = ParseMode.Markdown
                              },
                          new InlineQueryResultPhoto(
                              Guid.NewGuid().ToString("N"),
                              "https://telegram.org/file/811140636/1/hzUbyxse42w/4cd52d0464b44e1e5b",
                              "https://telegram.org/file/811140636/1/hzUbyxse42w/4cd52d0464b44e1e5b"),
                          new InlineQueryResultGif(
                              Guid.NewGuid().ToString("N"),
                              "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2c/Rotating_earth_%28large%29.gif/200px-Rotating_earth_%28large%29.gif",
                              "https://upload.wikimedia.org/wikipedia/commons/thumb/2/2c/Rotating_earth_%28large%29.gif/200px-Rotating_earth_%28large%29.gif")
                      };

    await this._telebot.AnswerInlineQueryAsync(update.InlineQuery.Id, results).ConfigureAwait(false);
}

private void CheckChosenInlineResult(Update update)
{
    Console.WriteLine("Received ChosenInlineResult.");
}
```
