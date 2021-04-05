using System;
using System.Globalization;
using Localizer.Sources;

namespace Localizer
{
    public interface ILocalizationFactory
    {
        string GetString(LocalizationCode localizationCode, CultureInfo culture = null);
        void RegisterSource(ILocalizationSource source, int priority);
    }

    public class LocalizationFactory : ILocalizationFactory
    {
        private readonly ILocalizationSourcesList sources;

        public LocalizationFactory(ILocalizationSourcesList sources)
        {
            this.sources = sources;
        }

        public string GetString(LocalizationCode code, CultureInfo culture = null)
        {
            var definedCulture = GetDefaultCultureIfNotDefined(culture);
            return TranslateToCulture(code, definedCulture);
        }
        
        private static CultureInfo GetDefaultCultureIfNotDefined(CultureInfo culture)
        {
            var currThreadCulture = CultureInfo.CurrentCulture;
            return culture ?? currThreadCulture;
        }

        private string TranslateToCulture(LocalizationCode code, CultureInfo culture)
        {
            var specifiedSourceType = code.SourceType;
            if (specifiedSourceType == LocalizationSourceType.Any)
                return TranslateUsingHighestPrioritySource(code, culture);

            var source = sources.GetByType(specifiedSourceType);
            return TranslateWithSpecialSource(source, code.TextCode, culture);
        }

        private string TranslateUsingHighestPrioritySource(LocalizationCode code, CultureInfo culture)
        {
            string translation = null;
            foreach (var source in sources.EnumerateByPriority())
            {
                translation = source.FindString(code.TextCode, culture);
                if (translation != null)
                    break;
            }
            if (translation == null)
                throw new TranslationNotFoundException(code.TextCode, culture);
            return translation;
        }

        private static string TranslateWithSpecialSource(ILocalizationSource source, string textCode, CultureInfo culture)
        {
            var translation = source.FindString(textCode, culture);
            if (translation == null)
                throw new TranslationNotFoundException(textCode, culture);
            return translation;
        }

        public void RegisterSource(ILocalizationSource source, int priority)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source),
                    "Cannot register localization source because it is null");
            sources.Insert(source, priority);
        }
    }
}