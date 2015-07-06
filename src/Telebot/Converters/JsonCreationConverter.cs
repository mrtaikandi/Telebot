namespace Taikandi.Telebot.Converters
{
    using System;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    internal abstract class JsonCreationConverter<T> : JsonConverter
    {
        #region Public Methods

        public override bool CanConvert(Type objectType)
        {
            switch( typeof(T).Name )
            {
                case "IConversation":
                    return true;
                default:
                    return false;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            var jsonObject = JObject.Load(reader);

            // Create target object based on JObject
            var target = this.Create(objectType, jsonObject);

            // Populate the object properties
            serializer.Populate(jsonObject.CreateReader(), target);

            return target;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object
        /// </summary>
        /// <param name="objectType">
        /// type of object expected
        /// </param>
        /// <param name="jsonObject">
        /// contents of JSON object that will be deserialized
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        protected abstract T Create(Type objectType, JObject jsonObject);

        #endregion
    }
}