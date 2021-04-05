using System;
using System.Globalization;

namespace Localizer
{
    public class TranslationNotFoundException : Exception
    {
        public TranslationNotFoundException(string textCode, CultureInfo culture)
            : base($"There is no translation to culture '{culture}' for string with code {textCode}")
        {
            TextCode = textCode;
            Culture = culture;
        }

        public string TextCode { get; }
        public CultureInfo Culture { get; }
    }
}