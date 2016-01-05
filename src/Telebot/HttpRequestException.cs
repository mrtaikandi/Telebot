namespace Taikandi.Telebot
{
    using System.Net;
    using System.Net.Http;

    public class HttpRequestException : System.Net.Http.HttpRequestException
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestException"/> class.
        /// </summary>
        /// <param name="message">A message that describes the current exception.</param>
        public HttpRequestException(string message)
            : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestException"/> class.
        /// </summary>
        /// <param name="responseMessage">The response message.</param>
        public HttpRequestException(HttpResponseMessage responseMessage)
            : this(responseMessage.StatusCode, responseMessage.ReasonPhrase) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequestException"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="reasonPhrase">The reason phrase.</param>
        public HttpRequestException(HttpStatusCode statusCode, string reasonPhrase)
            : base($"Response status code does not indicate success: {statusCode} ({reasonPhrase})")
        {
            this.StatusCode = statusCode;
            this.ReasonPhrase = reasonPhrase;
        }

        #endregion

        #region Public Properties

        public string ReasonPhrase { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        #endregion
    }
}