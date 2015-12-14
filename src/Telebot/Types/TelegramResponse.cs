namespace Taikandi.Telebot.Types
{
    public class TelegramResponse<TResult>
    {
        #region Public Properties

        public string Description { get; set; }

        public bool Ok { get; set; }

        public TResult Result { get; set; }

        #endregion
    }
}