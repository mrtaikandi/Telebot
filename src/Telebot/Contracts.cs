namespace Taikandi.Telebot
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;

    using JetBrains.Annotations;

    internal static class Contracts
    {
        #region Methods

        internal static void EnsureByteCount(string argumentValue, [NotNull] string paramName, int maxCount = 64)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(paramName), "!string.IsNullOrWhiteSpace(paramName)");

            if( !string.IsNullOrWhiteSpace(argumentValue) && Encoding.UTF8.GetByteCount(argumentValue) > maxCount )
                throw new ArgumentException($"Argument must be less than {maxCount} bytes.", paramName);
        }

        [ContractAnnotation("value:null=>halt")]
        internal static void EnsureNotNull<T>([NoEnumeration]T value, [NotNull] string paramName)
            where T : class
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(paramName), "!string.IsNullOrWhiteSpace(paramName)");

            if( value == null )
                throw new ArgumentNullException(paramName);
        }

        [ContractAnnotation("value:null=>halt")]
        internal static void EnsureNotNull(string value, [NotNull] string paramName)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(paramName), "!string.IsNullOrWhiteSpace(paramName)");

            if( string.IsNullOrWhiteSpace(value) )
                throw new ArgumentException($"{paramName} cannot be null or empty.", paramName);
        }

        internal static void EnsurePositiveNumber(long value, [NotNull] string paramName)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(paramName), "!string.IsNullOrWhiteSpace(paramName)");

            if( value <= 0 )
                throw new ArgumentException($"{paramName} must be a greater than zero.", paramName);
        }

        internal static void EnsureNotNegativeNumber(long value, [NotNull] string paramName)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(paramName), "!string.IsNullOrWhiteSpace(paramName)");

            if( value < 0 )
                throw new ArgumentException($"{paramName} must be a equals or greater than zero.", paramName);
        }

        internal static void EnsureFileExists([NotNull] string filePath)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(filePath), "!string.IsNullOrWhiteSpace(filePath)");

            if( !File.Exists(filePath) )
                throw new FileNotFoundException($"Unable to find the requested file at '{filePath}'.", filePath);
        }

        internal static void EnsureNotZero(long value, [NotNull] string paramName)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(paramName), "!string.IsNullOrWhiteSpace(paramName)");

            if (value == 0)
                throw new ArgumentException($"{paramName} cannot be 0.", paramName);
        }

        #endregion
    }
}