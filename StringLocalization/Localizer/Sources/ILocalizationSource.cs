using System.Globalization;

namespace Localizer.Sources
{
    public interface ILocalizationSource
    {
        LocalizationSourceType Type { get; }
        string FindString(string textCode, CultureInfo culture);
    }
}