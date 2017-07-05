using System.Collections.Generic;

namespace ICS.Models.Merge
{
    public class ServicesMerge
    {
        public string menu { get; set; }
        public string image { get; set; }
        public IEnumerable<Services_Translate> services { get; set; }
    }
}