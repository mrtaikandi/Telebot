What is Telebot
--------------------------------
Telebot is a well [documented](https://msdn.microsoft.com/en-us/library/b2s063f7%28vs.71%29.aspx?f=255&MSPPError=-2147217396) .NET client library for the [Telegram Bot API](https://core.telegram.org/bots/api) client written completely in C#.

Where can I get it?
--------------------------------
First, [install NuGet](http://docs.nuget.org/docs/start-here/installing-nuget). Then, install [Telebot](https://www.nuget.org/packages/Telebot/) from the package manager console:

    PM> Install-Package Telebot

How can I use it?
--------------------------------

Simply instanciate `Telebot` class and use available methods.

```csharp
public async Task Main(string[] args)
{
    // Provides a base class for communicating with Telegram Bot API servers.
    var telebot = new Telebot("123456:ABC-DEF1234ghIkl-zyx57W2v1u123ew11");

    // Used for getting only the unconfirmed updates.
    // More information at https://core.telegram.org/bots/api#getting-updates
    var offset = 0; 

    while( true )
    {
        // Use this method to receive incoming updates using long polling.
        // Or use telebot.SetWebhook() method to specifiy a url to receive incoming updates.
        TelegramResult<IList<Update>> response = await telebot.GetUpdatesAsync(offset);
        if( response.Ok && response.Result.Any() )
        {
            offset = response.Result.Max(u => u.Id) + 1;        

            foreach( Update update in response.Result )
            {
                await this.CheckMessagesAsync(telebot, update.Message);
            }
        }

        await Task.Delay(TimeSpan.FromSeconds(1));
    }
}

private async Task CheckMessages(Telebot telebot, Message message)
{
    // Assume we are doing more than echoing stuff.
    if( message == null )
        return Task.FromResult(0);

    // This method will tell the user that something is happening on the bot's side.
    // It is recommended to use this method when a response from the bot 
    // will take a noticeable amount of time to arrive.
    await telebot.SendChatAction(message.Chat.Id, ChatAction.Typing);
    await Task.Delay(TimeSpan.FromSeconds(3));
    
    // This method will sends a text message (obviously).
    return await telebot.SendMessageAsync(message.Chat.Id, message.Text ?? "Hmmmm");        
}
```