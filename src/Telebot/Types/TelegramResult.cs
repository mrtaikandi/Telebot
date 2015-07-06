namespace Taikandi.Telebot.Types
{
    public class TelegramResult<TResult>
    {
        public bool Ok { get; set; }

        public string Description { get; set; }

        public TResult Result { get; set; }
    }
}