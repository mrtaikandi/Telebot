namespace Taikandi.Telebot
{
    using System;

    /// <summary>
    /// Represents errors that occur when Telegram API servers are not available.
    /// </summary>
#if !NETSTANDARD
    [Serializable]
#endif
    public class ServiceUnavailableException : Exception
    {
        public ServiceUnavailableException() : this("Telegram API servers are not available. (502 - Bad Gateway)") { }

        public ServiceUnavailableException(string message)
            : base(message) { }

        public ServiceUnavailableException(string message, Exception inner)
            : base(message, inner) { }

#if !NETSTANDARD
        protected ServiceUnavailableException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
#endif
    }
}