namespace Taikandi.Telebot.Extensions
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    /// <summary>
    /// Extension methods to allow strongly typed objects to be read from <see cref="HttpContent" />
    /// instances.
    /// </summary>
    internal static class HttpContentExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns a <see cref="Task" /> that will yield an object of the specified type
        /// <typeparamref name="T" /> from the <paramref name="content" /> instance.
        /// </summary>
        /// <typeparam name="T">The type of the object to read.</typeparam>
        /// <param name="content">
        /// The <see cref="HttpContent" /> instance from which to read.
        /// </param>
        /// <returns>An object instance of the specified type.</returns>
        /// <remarks>
        /// This override use the built-in collection of formatters.
        /// </remarks>
        public static async Task<T> ReadAsAsync<T>(this HttpContent content)
        {
            var value = await content.ReadAsStringAsync().ConfigureAwait(false);
            return value == null ? default(T) : JsonConvert.DeserializeObject<T>(value);
        }

        #endregion
    }
}