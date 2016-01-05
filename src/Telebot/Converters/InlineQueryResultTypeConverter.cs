namespace Taikandi.Telebot.Converters
{
    using System;

    using Newtonsoft.Json;

    using Taikandi.Telebot.Types;

    internal class InlineQueryResultTypeConverter : JsonConverter
    {
        #region Public Methods and Operators

        /// <summary>
        /// Determines whether this instance can convert the specified object type.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <returns>
        /// <c>true</c> if this instance can convert the specified object type; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(InlineQueryResultType);
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>
        /// The object value.
        /// </returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = InlineQueryResultType.None;
            if( reader.Value == null )
                return result;
            
            var value = reader.Value.ToString().Replace("_", string.Empty);
            Enum.TryParse(value, true, out result);

            return result;
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var resultType = (InlineQueryResultType)value;
            switch( resultType )
            {
                case InlineQueryResultType.Article:
                    writer.WriteValue("article");
                    break;
                case InlineQueryResultType.Photo:
                    writer.WriteValue("photo");
                    break;
                case InlineQueryResultType.Gif:
                    writer.WriteValue("gif");
                    break;
                case InlineQueryResultType.Mpeg4Gif:
                    writer.WriteValue("mpeg4_gif");
                    break;
                case InlineQueryResultType.Video:
                    writer.WriteValue("video");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }            
        }

        #endregion
    }
}