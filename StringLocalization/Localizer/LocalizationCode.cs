using System;
using Localizer.Sources;

namespace Localizer
{
    public class LocalizationCode
    {
        public LocalizationCode(string textCode, LocalizationSourceType sourceType = LocalizationSourceType.Any)
        {
            SourceType = sourceType;
            if (string.IsNullOrWhiteSpace(textCode))
                throw new ArgumentException("Localization code should not be empty", nameof(textCode));
            TextCode = textCode;
        }
        
        public LocalizationSourceType SourceType { get; }
        public string TextCode { get; }

        public override string ToString()
        {
            return $"[TextCode: {TextCode}, Source: {SourceType}]";
        }
    }
}