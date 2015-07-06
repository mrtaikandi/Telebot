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

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if( reader.Value == null )
                return null;

            var time = long.Parse(Convert.ToString(reader.Value));
            return UnixStart.AddSeconds(time);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(Convert.ToString(((DateTime)value - UnixStart).TotalSeconds, CultureInfo.InvariantCulture));
        }

        #endregion
    }
}