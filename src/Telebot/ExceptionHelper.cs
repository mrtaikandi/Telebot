namespace Taikandi.Telebot
{
    using System;
    using System.Text;

    internal static class ExceptionHelper
    {
        #region Methods

        internal static void ValidateArgumentByteCount(string argumentName, string argumentValue, int maxCount = 64)
        {
            if( !string.IsNullOrWhiteSpace(argumentValue) && Encoding.UTF8.GetByteCount(argumentValue) > maxCount )
                throw new ArgumentException($"Argument must be less than {maxCount} bytes.", argumentName);
        }

        #endregion
    }
}