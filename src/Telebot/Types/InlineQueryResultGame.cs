namespace Taikandi.Telebot.Types
{
    using Newtonsoft.Json;

    /// <summary>
    /// Represents a <see cref="Game"/>.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class InlineQueryResultGame : InlineQueryResult
    {
        #region Public Properties

        /// <summary>
        /// Gets or sets a short name of the game
        /// </summary>
        [JsonProperty("game_short_name", Required = Required.Always)]
        public string GameShortName { get; set; }

        /// <summary>Gets or sets the type of the result.</summary>
        public override InlineQueryResultType Type => InlineQueryResultType.Game;

        #endregion
    }
}