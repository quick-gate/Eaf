namespace QGate.Eaf.Domain.Metadatas.Models
{
    public class MetadataTranslation
    {
        public MetadataTranslation(string name, string languageCode)
        {
            Name = name;
            LanguageCode = languageCode;
        }
        public string LanguageCode { get; set; }
        public string Name { get; set; }
    }
}
