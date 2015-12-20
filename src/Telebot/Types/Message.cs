namespace Taikandi.Telebot.Types
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    using Newtonsoft.Json;

    using Taikandi.Telebot.Converters;

    /// <summary>This object represents a message.</summary>
    public class Message
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets the information about the file if the message is an audio file (Optional).
        /// </summary>
        [JsonProperty("audio")]
        public Audio Audio { get; set; }

        /// <summary>Gets or sets the caption of the photo or video.</summary>
        [JsonProperty("caption")]
        public string Caption { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the channel has been created. This is an optional service
        /// message.
        /// </summary>
        [JsonProperty("channel_chat_created")]
        public bool ChannelChatCreated { get; set; }

        /// <summary>
        /// Gets or sets the conversation the message belongs to.
        /// </summary>
        [JsonProperty("chat", Required = Required.Always)]
        public Chat Chat { get; set; }

        /// <summary>
        /// Gets or sets the information about the contact if the message is a shared contact (Optional).
        /// </summary>
        [JsonProperty("contact")]
        public Contact Contact { get; set; }

        /// <summary>Gets or sets the date the message was sent.</summary>
        [JsonProperty("date", Required = Required.Always)]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset Date { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the group photo is deleted.
        /// </summary>
        [JsonProperty("delete_chat_photo")]
        public bool DeleteChatPhoto { get; set; }

        /// <summary>
        /// Gets or sets the information about the file if the message is a general file (Optional).
        /// </summary>
        [JsonProperty("document")]
        public Document Document { get; set; }

        /// <summary>
        /// Gets or sets the date the original message was sent for forwarded messages (Optional).
        /// </summary>
        [JsonProperty("forward_date")]
        [JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTimeOffset ForwardDate { get; set; }

        /// <summary>
        /// Gets or sets the sender of the original message for forwarded messages (Optional).
        /// </summary>
        [JsonProperty("forward_from")]
        public User ForwardFrom { get; set; }

        /// <summary>Gets or sets the sender.</summary>
        [JsonProperty("from", Required = Required.Always)]
        public User From { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the group is created.
        /// </summary>
        [JsonProperty("group_chat_created")]
        public bool GroupChatCreated { get; set; }

        /// <summary>Gets or sets the unique message identifier</summary>
        [JsonProperty("message_id", Required = Required.Always)]
        [Range(Common.IdentifierMinValue, long.MaxValue)]
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the information about the member (which might be a bo itself) removed from the group
        /// (Optional).
        /// </summary>
        [JsonProperty("left_chat_participant")]
        public User LeftChatParticipant { get; set; }

        /// <summary>
        /// Gets or sets the information about the location if the message is a shared location (Optional).
        /// </summary>
        [JsonProperty("location")]
        public Location Location { get; set; }

        /// <summary>
        /// Gets or sets the supergroup identifier that the group has been migrated to (Optional).
        /// </summary>
        [JsonProperty("migrate_from_chat_id")]
        public int MigrateFromChatId { get; set; }

        /// <summary>
        /// Gets or sets the supergroup identifier to migrate the group to (Optional).
        /// </summary>
        [JsonProperty("migrate_to_chat_id")]
        public int MigrateToChatId { get; set; }

        /// <summary>
        /// Gets or sets the information about the new member (which might be a bo itself) added to the group
        /// (Optional).
        /// </summary>
        [JsonProperty("new_chat_participant")]
        public User NewChatParticipant { get; set; }

        /// <summary>Gets or sets the group new photo (Optional).</summary>
        [JsonProperty("new_chat_photo")]
        public IList<PhotoSize> NewChatPhoto { get; set; }

        /// <summary>
        /// Gets or sets the value of the group new title (Optional).
        /// </summary>
        [JsonProperty("new_chat_title")]
        public string NewChatTitle { get; set; }

        /// <summary>
        /// Gets or sets the available sizes of the photo if the message is a photo (Optional).
        /// </summary>
        [JsonProperty("photo")]
        public IList<PhotoSize> Photos { get; set; }

        /// <summary>
        /// Gets or sets the original message for replies.
        /// <para>
        /// Note that the Message object in this field will not contain further reply_to_message fields even if
        /// it itself is a reply.
        /// </para>
        /// </summary>
        [JsonProperty("reply_to_message")]
        public Message ReplyToMessage { get; set; }

        /// <summary>
        /// Gets or sets the information about the sticker if the message is a sticker (Optional).
        /// </summary>
        [JsonProperty("sticker")]
        public Sticker Sticker { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the supergroup has been created. This is an optional
        /// service message.
        /// </summary>
        [JsonProperty("supergroup_chat_created")]
        public bool SupergroupChatCreated { get; set; }

        /// <summary>
        /// Gets or sets the actual UTF-8 text of the message for the text messages (Optional).
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets the type of message.
        /// </summary>
        public MessageType Type
        {
            get
            {
                if( !string.IsNullOrWhiteSpace(this.Text) )
                    return MessageType.Text;

                if( this.Sticker != null )
                    return MessageType.Sticker;

                if( this.Photos != null && this.Photos.Any() )
                    return MessageType.Photo;

                if( this.Audio != null )
                    return MessageType.Audio;

                if( this.Voice != null )
                    return MessageType.Voice;

                if( this.Video != null )
                    return MessageType.Video;

                if( this.Document != null )
                    return MessageType.Document;

                if( this.Location != null )
                    return MessageType.Location;

                if( this.Contact != null )
                    return MessageType.Contact;

                return MessageType.Unknown;
            }
        }

        /// <summary>
        /// Gets or sets the information about the video if message is a video (Optional).
        /// </summary>
        [JsonProperty("video")]
        public Video Video { get; set; }

        /// <summary>
        /// Gets or sets the information about the file if the message is a voice message (Optional).
        /// </summary>
        [JsonProperty("voice")]
        public Voice Voice { get; set; }

        #endregion
    }
}