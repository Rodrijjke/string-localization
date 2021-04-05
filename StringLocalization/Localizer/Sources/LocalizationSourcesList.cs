using System.Collections.Generic;
using System.Linq;

namespace Localizer.Sources
{
    public interface ILocalizationSourcesList
    {
        ILocalizationSource GetByType(LocalizationSourceType type);
        IEnumerable<ILocalizationSource> EnumerateByPriority();
        void Insert(ILocalizationSource source, int priority);
    }
    
    public class LocalizationSourcesList : ILocalizationSourcesList
    {
        private readonly Dictionary<LocalizationSourceType, (int Priority, ILocalizationSource Source)> prioritizedSources;

        public LocalizationSourcesList()
        {
            prioritizedSources = new Dictionary<LocalizationSourceType, (int Priority, ILocalizationSource Source)>();
        }

        public ILocalizationSource GetByType(LocalizationSourceType type)
        {
            return prioritizedSources[type].Source;
        }

        public IEnumerable<ILocalizationSource> EnumerateByPriority()
        {
            return prioritizedSources.Values
                .OrderByDescending(prioritizedSource => prioritizedSource.Priority)
                .Select(prioritizedSource => prioritizedSource.Source);
        }

        public void Insert(ILocalizationSource source, int priority)
        {
            prioritizedSources[source.Type] = (priority, source);
        }
    }
}