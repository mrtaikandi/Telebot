namespace Taikandi.Telebot.Types
{
    public class ChatAction
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatAction" /> class.
        /// </summary>
        /// <param name="value">The chat action value.</param>
        private ChatAction(string value)
        {
            this.Value = value;
        }

        #endregion

        #region Public Properties

        public static ChatAction FindLocation => new ChatAction("find_location");

        public static ChatAction RecordAudio => new ChatAction("record_audio");

        public static ChatAction RecordVideo => new ChatAction("record_video");

        public static ChatAction Typing => new ChatAction("typing");

        public static ChatAction UploadAudio => new ChatAction("upload_audio");

        public static ChatAction UploadDocument => new ChatAction("upload_document");

        public static ChatAction UploadPhoto => new ChatAction("upload_photo");

        public static ChatAction UploadVideo => new ChatAction("upload_video");

        public string Value { get; }

        #endregion

        #region Public Methods and Operators

        public static bool operator ==(ChatAction a, ChatAction b)
        {
            // If both are null, or both are same instance, return true.
            if( ReferenceEquals(a, b) )
                return true;

            // If one is null, but not both, return false.            
            if( (object)a == null || (object)b == null )
                return false;

            // Return true if the fields match:
            return a.Equals(b);
        }

        public static bool operator !=(ChatAction a, ChatAction b)
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
            var chatAction = obj as ChatAction;
            return this.Equals(chatAction);
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

        #region Methods

        /// <summary>
        /// Determines whether the specified <see cref="ChatAction" /> is equal to the current
        /// <see cref="ChatAction" />.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the specified <see cref="ChatAction" /> is equal to the current instance; otherwise,
        /// <c>false</c>.
        /// </returns>
        /// <param name="other">
        /// The <see cref="ChatAction" /> to compare with the current instance.
        /// </param>
        protected bool Equals(ChatAction other)
        {
            return other != null && string.Equals(this.Value, other.Value);
        }

        #endregion
    }
}