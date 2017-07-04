using System.Collections.Generic;

namespace ICS.Models.Merge
{
    public class HomeMerge
    {
        public IEnumerable<Steps_Translate> step { get; set; }
        public IEnumerable<Slide_Translate> slide { get; set; }
        public int slideActive { get; set; }
    }
}