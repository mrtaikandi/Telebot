namespace Taikandi.Telebot
{
    using System.Collections.Generic;

    internal class NameValueCollection : List<KeyValuePair<string, string>>
    {
        #region Public Methods and Operators

        /// <summary>
        /// Adds a key value pair to the underlying collection.
        /// </summary>
        /// <param name="key">The object defined in each key/value pair.</param>
        /// <param name="value">The definition associated with key.</param>
        public void Add(string key, string value)
        {
            this.Add(new KeyValuePair<string, string>(key, value));
        }

        /// <summary>
        /// Adds a key value pair to the underlying collection.
        /// </summary>
        /// <param name="key">The object defined in each key/value pair.</param>
        /// <param name="value">The definition associated with key.</param>
        public void Add(string key, bool value)
        {
            this.Add(key, value.ToString());
        }

        /// <summary>
        /// Adds a key value pair to the underlying collection.
        /// </summary>
        /// <param name="key">The object defined in each key/value pair.</param>
        /// <param name="value">The definition associated with key.</param>
        public void Add(string key, int value)
        {
            this.Add(key, value.ToString());
        }

        /// <summary>
        /// Adds a key value pair to the underlying collection.
        /// </summary>
        /// <param name="key">The object defined in each key/value pair.</param>
        /// <param name="value">The definition associated with key.</param>
        public void Add(string key, long value)
        {
            this.Add(key, value.ToString());
        }

        /// <summary>
        /// Adds a key value pair to the underlying collection.
        /// </summary>
        /// <param name="condition">
        /// the condition expression to evaluate. If the condition is <c>true</c> provided
        /// <paramref name="key" /> and <paramref name="value" /> gets added to the collection.
        /// </param>
        /// <param name="key">The object defined in each key/value pair.</param>
        /// <param name="value">The definition associated with key.</param>
        public void AddIf(bool condition, string key, string value)
        {
            this.Add(new KeyValuePair<string, string>(key, value));
        }

        #endregion
    }
}