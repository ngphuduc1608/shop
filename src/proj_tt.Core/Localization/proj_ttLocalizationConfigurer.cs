using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace proj_tt.Localization
{
    public static class proj_ttLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(proj_ttConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(proj_ttLocalizationConfigurer).GetAssembly(),
                        "proj_tt.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
