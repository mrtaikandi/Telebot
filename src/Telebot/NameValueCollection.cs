namespace Taikandi.Telebot
{
    using System.Collections.Generic;

    internal class NameValueCollection : List<KeyValuePair<string, string>>
    {
        public void Add(string key, string value)
        {
            this.Add(new KeyValuePair<string, string>(key, value));
        }
    }
}