namespace Taikandi.Telebot.Types
{
    using Newtonsoft.Json;

    internal class Error
    {
        [JsonProperty("ok")]
        public bool Ok { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("error_code")]
        public string ErrorCode { get; set; }
    }
}