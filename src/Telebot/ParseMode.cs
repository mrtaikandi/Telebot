namespace Taikandi.Telebot
{
    public sealed class ParseMode
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ParseMode" /> class.
        /// </summary>
        /// <param name="value">
        /// The value of the property.
        /// </param>
        private ParseMode(string value)
        {
            this.Value = value;
        }

        #endregion

        #region Public Properties

        public static ParseMode Markdown => new ParseMode("Markdown");

        public static ParseMode Normal => new ParseMode(string.Empty);

        public string Value { get; }

        #endregion

        #region Public Methods and Operators

        public static bool operator ==(ParseMode a, ParseMode b)
        {
            // If both are null, or both are same instance, return true.
            if( ReferenceEquals(a, b) )
                return true;

            // If one is null, but not both, return false.                       
            // ReSharper disable RedundantCast.0
            if( (object)a == null || (object)b == null )
                return false;

            // ReSharper restore RedundantCast.0
            // Return true if the fields match:
            return a.Equals(b);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ParseMode" /> to <see cref="System.String" />.
        /// </summary>
        /// <param name="a">The <see cref="ParseMode" /> to convert.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        public static implicit operator string(ParseMode a)
        {
            return a?.Value;
        }

        public static bool operator !=(ParseMode a, ParseMode b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">
        /// The object to compare with the current object.
        /// </param>
        public override bool Equals(object obj)
        {
            var me = obj as ParseMode;
            return this.Equals(me);
        }

        /// <summary>
        /// Determines whether the specified <see cref="ParseMode" /> is equal to the current <see cref="ParseMode" />.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the specified <see cref="ParseMode" /> is equal to the current instance; otherwise, <c>false</c>.
        /// </returns>
        /// <param name="other">
        /// The <see cref="ParseMode" /> to compare with the current instance.
        /// </param>
        public bool Equals(ParseMode other)
        {
            return other != null && string.Equals(this.Value, other.Value);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object" />.
        /// </returns>
        public override int GetHashCode()
        {
            return this.Value?.GetHashCode() ?? 0;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return this.Value;
        }

        #endregion
    }
}