using System;
using System.Collections.Generic;
using System.Globalization;

namespace Localizer.Sources
{
    public class InMemorySource : ILocalizationSource
    {
        private readonly Dictionary<string, Dictionary<int, string>> storage;

        public InMemorySource()
        {
            storage = new Dictionary<string, Dictionary<int, string>>();
        }

        public LocalizationSourceType Type => LocalizationSourceType.InMemory;

        public string FindString(string textCode, CultureInfo culture)
        {
            var stringTranslations = GetAllStringTranslations(textCode);
            var cultureId = culture.LCID;
            return !stringTranslations.ContainsKey(cultureId) 
                ? null 
                : stringTranslations[cultureId];
        }

        private Dictionary<int, string> GetAllStringTranslations(string textCode)
        {
            return !storage.ContainsKey(textCode) 
                ? new Dictionary<int, string>() 
                : storage[textCode];
        }

        public void SaveString(string textCode, CultureInfo culture, string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value), "Cannot save null string to localization source");
            if (string.IsNullOrWhiteSpace(textCode))
                throw new ArgumentException("Cannot save string translation with empty text code", nameof(textCode));
            if (culture == null)
                throw new ArgumentNullException(nameof(culture), "Cannot save translation to null culture");

            if (!storage.ContainsKey(textCode))
                storage[textCode] = new Dictionary<int, string>();
            storage[textCode][culture.LCID] = value;
        }
    }
}