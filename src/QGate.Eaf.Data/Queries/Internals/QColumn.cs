using System.Collections.Generic;

namespace QGate.Eaf.Data.Queries.Internals
{
    public class QColumn
    {
        public IList<string> Properties { get; set; }
        public string Path { get; set; }
        public string Alias { get; set; }
        public bool IsKey { get; set; }
    }
}
