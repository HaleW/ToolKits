using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Utils
{
    public class ConditionsResult
    {
        public Dictionary<Conditions, string[]> IncludeFile { get; set; }
        public Dictionary<Conditions, string[]> ExcludeFile { get; set; }
        public Dictionary<Conditions, string[]> IncludeFolder { get; set; }
        public Dictionary<Conditions, string[]> ExcludeFolder { get; set; }
    }
}
