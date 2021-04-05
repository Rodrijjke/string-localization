using System.Globalization;
using System.IO;
using System.Xml.Serialization;

namespace Localizer.Sources
{
    public class LocalFileSource : ILocalizationSource
    {
        private readonly string filePath;
        
        public LocalFileSource(string filePath)
        {
            this.filePath = filePath;
        }

        public LocalizationSourceType Type => LocalizationSourceType.LocalFile;
        
        public string FindString(string textCode, CultureInfo culture)
        {
            var serializer = new XmlSerializer(typeof(StringTranslationRecord[]));
            using var fileStream = File.OpenRead(filePath);
            var records = (StringTranslationRecord[]) serializer.Deserialize(fileStream);
            if (records == null)
                return null;
            foreach (var record in records)
            {
                if (record.TextCode == textCode && record.CultureId == culture.LCID)
                    return record.Translation;
            }
            return null;
        }

        public class StringTranslationRecord
        {
            public string TextCode { get; init; }
            public int CultureId { get; init; }
            public string Translation { get; init; }
        }
    }
}