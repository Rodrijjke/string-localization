using System.Globalization;

namespace Localizer
{
    public class LocalizationString
    {
        private readonly LocalizationCode code;
        private readonly ILocalizationFactory localizationFactory;

        public LocalizationString(ILocalizationFactory localizationFactory, LocalizationCode code)
        {
            this.localizationFactory = localizationFactory;
            this.code = code;
        }

        public string Translate(CultureInfo culture = null)
        {
            return localizationFactory.GetString(code, culture);
        }
    }
}