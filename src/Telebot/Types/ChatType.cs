namespace Taikandi.Telebot.Types
{
    using System;

    /// <summary>Provides list of supported chat types.</summary>
    public sealed class ChatType
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatType" /> class.
        /// </summary>
        /// <param name="value">The value of the property.</param>
        private ChatType(string value)
        {
            this.Value = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value indicating that the chat is a channel.
        /// </summary>
        public static ChatType Channel => new ChatType("channel");

        /// <summary>
        /// Gets a value indicating that the chat is a group chat.
        /// </summary>
        public static ChatType Group => new ChatType("group");

        /// <summary>
        /// Gets a value indicating that the chat is a private chat.
        /// </summary>
        public static ChatType Private => new ChatType("private");

        /// <summary>
        /// Gets a value indicating that the chat is a supergroup chat.
        /// </summary>
        public static ChatType Supergroup => new ChatType("supergroup");

        public string Value { get; }

        #endregion

        #region Public Methods and Operators

        public static bool operator ==(ChatType a, ChatType b)
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
        /// Performs an implicit conversion from <see cref="ChatType" /> to <see cref="System.String" />.
        /// </summary>
        /// <param name="a">The <see cref="ChatType" /> to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator string (ChatType a)
        {
            return a?.Value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="ChatType" /> to <see cref="System.String" />.
        /// </summary>
        /// <param name="a">The <see cref="ChatType" /> to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator ChatType(string a)
        {
            if( string.IsNullOrWhiteSpace(a) )
                throw new NotSupportedException();

            if( a == Channel.Value )
                return Channel;
            if( a == Group.Value )
                return Group;
            if( a == Private.Value )
                return Private;
            if( a == Supergroup.Value )
                return Supergroup;

            throw new NotSupportedException();
        }



        public static bool operator !=(ChatType a, ChatType b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object" /> is equal to the current
        /// <see cref="T:System.Object" />.
        /// </summary>
        /// <returns>
        /// true if the specified object  is equal to the current object; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current object.</param>
        public override bool Equals(object obj)
        {
            var me = obj as ChatType;
            return this.Equals(me);
        }

        /// <summary>
        /// Determines whether the specified <see cref="ChatType" /> is equal to the current
        /// <see cref="ChatType" />.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the specified <see cref="ChatType" /> is equal to the current instance; otherwise,
        /// <c>false</c>.
        /// </returns>
        /// <param name="other">
        /// The <see cref="ChatType" /> to compare with the current instance.
        /// </param>
        public bool Equals(ChatType other)
        {
            return other != null && string.Equals(this.Value, other.Value);
        }

        /// <summary>Serves as a hash function for a particular type.</summary>
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
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return this.Value;
        }

        #endregion
    }
}