namespace Taikandi.Telebot
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Reflection;

    using JetBrains.Annotations;

    using Newtonsoft.Json;

    internal static class MultipartFormDataContentExtensions
    {
        #region Methods

        /// <summary>
        /// Add HTTP content to a collection of <see cref="HttpContent" /> objects that get serialized to
        /// multipart/form-data MIME type.
        /// </summary>
        /// <typeparam name="T">Type of the content to add.</typeparam>
        /// <param name="content">The <see cref="MultipartFormDataContent" /> object to extend.</param>
        /// <param name="name">The name for the HTTP content to add.</param>
        /// <param name="value">The content to add to the collection.</param>
        /// <param name="fileName">The name of the HTTP content to add to the collection.</param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="content" /> is null -or- The <paramref name="name" /> is <c>null</c> or
        /// contains only white space characters.</exception>
        internal static void Add<T>([NotNull] this MultipartFormDataContent content, [NotNull] string name, T value, string fileName = null)
        {
            if( content == null )
                throw new ArgumentNullException(nameof(content));

            if( string.IsNullOrWhiteSpace(name) )
                throw new ArgumentNullException(nameof(name));

            var httpContent = ConvertToContent(value);

            if( string.IsNullOrWhiteSpace(fileName) )
                content.Add(httpContent, name);
            else
                content.Add(httpContent, name, fileName);
        }

        /// <summary>
        /// Add HTTP content to a collection of <see cref="HttpContent" /> objects that get serialized to
        /// multipart/form-data MIME type.
        /// </summary>
        /// <typeparam name="T">Type of the content to add.</typeparam>
        /// <param name="content">The <see cref="MultipartFormDataContent" /> object to extend.</param>
        /// <param name="condition">the condition expression to evaluate. If the condition is <c>true</c> provided
        /// <paramref name="name" /> and <paramref name="value" /> gets added to the
        /// <paramref name="content" />.</param>
        /// <param name="name">The name for the HTTP content to add.</param>
        /// <param name="value">The content to add to the collection.</param>
        /// <param name="fileName">The name of the HTTP content to add to the collection.</param>
        /// <exception cref="System.ArgumentNullException">The <paramref name="content" /> is null -or- The <paramref name="name" /> is <c>null</c> or
        /// contains only white space characters.</exception>
        internal static void AddIf<T>([NotNull] this MultipartFormDataContent content, bool condition, [NotNull] string name, T value, string fileName = null)
        {
            if( condition )
                Add(content, name, value, fileName);
        }

        private static HttpContent ConvertToContent<T>(T value)
        {
            if( value == null )
                return new StringContent(string.Empty);

            var valueType = value.GetType();
            if( valueType.GetTypeInfo().IsValueType )
                return new StringContent(value.ToString().ToLowerInvariant());

            if( valueType == typeof(string) )
                return new StringContent(value as string);

            if( valueType == typeof(Stream) )
                return new StreamContent(value as Stream);

            if( valueType == typeof(FileStream) )
                return new StreamContent(value as FileStream);

            return new StringContent(JsonConvert.SerializeObject(value, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
        }

        #endregion
    }
}