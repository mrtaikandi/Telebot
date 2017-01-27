namespace Taikandi.Telebot.Types
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The member's status in the chat.
    /// </summary>
    public enum ChatMemberStatus
    {
        /// <summary>
        /// Indicates that the user status is creator.
        /// </summary>
        [EnumMember(Value = "creator")]
        Creator,

        /// <summary>
        /// Indicates that the user status is administrator.
        /// </summary>
        [EnumMember(Value = "administrator")]
        Administrator,

        /// <summary>
        /// Indicates that the user status is member.
        /// </summary>
        [EnumMember(Value = "member")]
        Member,

        /// <summary>
        /// Indicates that the user status is left.
        /// </summary>
        [EnumMember(Value = "left")]
        Left,

        /// <summary>
        /// Indicates that the user status is kicked.
        /// </summary>
        [EnumMember(Value = "kicked")]
        Kicked
    }
}