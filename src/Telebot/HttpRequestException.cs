namespace Taikandi.Telebot
{
    using System.Net;
    using System.Net.Http;

    public class HttpRequestException : System.Net.Http.HttpRequestException
    {
        #region Constructors and Destructors

        public HttpRequestException(HttpResponseMessage responseMessage)
            : this(responseMessage.StatusCode, responseMessage.ReasonPhrase) { }

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