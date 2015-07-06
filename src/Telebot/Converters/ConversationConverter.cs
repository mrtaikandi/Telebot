namespace Taikandi.Telebot.Converters
{
    using System;
    
    using Newtonsoft.Json.Linq;

    using Types;

    internal class ConversationConverter : JsonCreationConverter<IConversation>
    {
        #region Methods

        protected override IConversation Create(Type objectType, JObject jsonObject)
        {
            if( FieldExists("title", jsonObject) )
                return new GroupChat();

            return FieldExists("first_name", jsonObject) ? new User() : null;
        }

        private static bool FieldExists(string fieldName, JObject jsonObject)
        {
            return jsonObject[fieldName] != null;
        }

        #endregion
    }
}