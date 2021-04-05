using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using Localizer.Sources;
using NUnit.Framework;

namespace Localizer.Test
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class LocalizationFactory_Should
    {
        private CultureInfo enCulture;
        private CultureInfo ruCulture;
        private string localizationsFilePath;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            enCulture = new CultureInfo("en-US");
            ruCulture = new CultureInfo("ru-RU");
            localizationsFilePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "TestLocalizations.xml");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            if (File.Exists(localizationsFilePath))
                File.Delete(localizationsFilePath);
        }
        
        [Test]
        public void ProvideTranslationFromSpecificSource()
        {
            var inMemorySource = new InMemorySource();
            inMemorySource.SaveString("greeting", enCulture, "Hello world!");
            var sourcesList = new LocalizationSourcesList();
            var localizationFactory = new LocalizationFactory(sourcesList);
            localizationFactory.RegisterSource(inMemorySource, 0);

            var code = new LocalizationCode("greeting", LocalizationSourceType.InMemory);
            var translatedString = localizationFactory.GetString(code, enCulture);
            
            Assert.AreEqual("Hello world!", translatedString);
        }

        [Test]
        public void ProvideCorrectTranslation_WhenSeveralTranslationsExist()
        {
            var inMemorySource = new InMemorySource();
            inMemorySource.SaveString("greeting", enCulture, "Hello world!");
            inMemorySource.SaveString("greeting", ruCulture, "Привет мир!");
            var sourcesList = new LocalizationSourcesList();
            var localizationFactory = new LocalizationFactory(sourcesList);
            localizationFactory.RegisterSource(inMemorySource, 0);

            var code = new LocalizationCode("greeting", LocalizationSourceType.InMemory);
            var translatedString = localizationFactory.GetString(code, ruCulture);
            
            Assert.AreEqual("Привет мир!", translatedString); 
        }
        
        [Test]
        public void ProvideTranslationFromHighlyPrioritizedSource()
        {
            var inMemorySource = new InMemorySource();
            inMemorySource.SaveString("greeting", enCulture, "Hello from memory!");
            CreateFileWithTranslations(localizationsFilePath, new []
            {
                new LocalFileSource.StringTranslationRecord
                {
                    TextCode = "greeting",
                    CultureId = enCulture.LCID,
                    Translation = "Hello from file!"
                }
            });
            var localFileSource = new LocalFileSource(localizationsFilePath);
            var sourcesList = new LocalizationSourcesList();
            var localizationFactory = new LocalizationFactory(sourcesList);
            localizationFactory.RegisterSource(inMemorySource, 0);
            localizationFactory.RegisterSource(localFileSource, 1);

            var code = new LocalizationCode("greeting");
            var translatedString = localizationFactory.GetString(code);
            
            Assert.AreEqual("Hello from file!", translatedString);
        }

        private void CreateFileWithTranslations(string filePath, LocalFileSource.StringTranslationRecord[] records)
        {
            var serializer = new XmlSerializer(typeof(LocalFileSource.StringTranslationRecord[]));
            using var fileStream = File.OpenWrite(filePath);
            serializer.Serialize(fileStream, records);
        }
    }
}