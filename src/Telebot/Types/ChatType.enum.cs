namespace Taikandi.Telebot.Types
{
    using System.Runtime.Serialization;

    /// <summary>Provides list of supported chat types.</summary>
    public enum ChatType
    {
        /// <summary>
        /// Indicates that the chat is a channel.
        /// </summary>
        [EnumMember(Value = "channel")]
        Channel, 

        /// <summary>
        /// Indicates that the chat is a group chat.
        /// </summary>
        [EnumMember(Value = "group")]
        Group, 

        /// <summary>
        /// Indicates that the chat is a private chat.
        /// </summary>
        [EnumMember(Value = "private")]
        Private, 

        /// <summary>
        /// Indicates that the chat is a supergroup chat.
        /// </summary>
        [EnumMember(Value = "supergroup")]
        SuperGroup
    }
}