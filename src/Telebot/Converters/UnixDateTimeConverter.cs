namespace Taikandi.Telebot.Converters
{
    using System;
    using System.Globalization;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Converters;

    internal class UnixDateTimeConverter : DateTimeConverterBase
    {
        #region Constants and Fields

        private static readonly DateTimeOffset UnixStart = new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero);

        #endregion

        #region Public Methods

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
            if( reader.Value == null )
                return null;

            var time = long.Parse(Convert.ToString(reader.Value));
            return UnixStart.AddSeconds(time);
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(Convert.ToString(((DateTimeOffset)value - UnixStart).TotalSeconds, CultureInfo.InvariantCulture));
        }

        #endregion
    }
}