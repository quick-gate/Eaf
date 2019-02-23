using System;
using System.Collections.Generic;

namespace QGate.Eaf.Domain.Metadatas.Models
{
    public class MetadataBase
    {
        private Type _type;
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrWhiteSpace(StorageName))
                {
                    StorageName = value;
                }

                _name = value;
            }
        }
        public Type Type
        {
            get { return _type; }
            set
            {
                if(string.IsNullOrWhiteSpace(Name))
                {
                    Name = value.AssemblyQualifiedName;

                    if (this is EntityMetadata)
                    {
                        var nameSegments = Name.Split(',');
                        Name = string.Concat(nameSegments[0], ",", nameSegments[1]);
                    }
                }

                if (string.IsNullOrWhiteSpace(StorageName))
                {
                    Name = value.Name;
                }

                _type = value;
            }
        }

        public string StorageName { get; set; }

        public IList<MetadataTranslation> Translations { get; set; }
    }
}
