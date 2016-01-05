namespace Taikandi.Telebot.Types
{
    /// <summary>Provides available message types.</summary>
    public enum MessageType
    {
        /// <summary>
        /// Indicates the message type is unknown.
        /// </summary>
        Unknown, 

        /// <summary>Indicates that the message is a text message.</summary>
        Text,

        /// <summary>Indicates that the message is a sticker.</summary>
        Sticker,

        /// <summary>Indicates that the message is a photo message.</summary>
        Photo,

        /// <summary>Indicates that the message is an audio message.</summary>
        Audio,

        /// <summary>Indicates that the message is a voice message.</summary>
        Voice,        

        /// <summary>Indicates that the message is a video message.</summary>
        Video,  

        /// <summary>Indicates that the message contains a document.</summary>
        Document,         

        /// <summary>Indicates that the message is a location.</summary>
        Location, 

        /// <summary>
        /// Indicates that the message is a contact information.
        /// </summary>
        Contact
    }
}