using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Utils
{
    public class ConditionUitls
    {
        public static ConditionsResult CondotionsIsNull(ConditionsResult conditions)
        {
            ConditionsResult result = new ConditionsResult
            {
                IncludeFile = DicIsNull(conditions.IncludeFile),
                IncludeFolder = DicIsNull(conditions.IncludeFolder),
                ExcludeFile = DicIsNull(conditions.ExcludeFile),
                ExcludeFolder = DicIsNull(conditions.ExcludeFolder)
            };

            return result;
        }

        private static Dictionary<Conditions, string[]> DicIsNull(Dictionary<Conditions, string[]> dic)
        {
            Dictionary<Conditions, string[]> result = new Dictionary<Conditions, string[]>();

            foreach (KeyValuePair<Conditions, string[]> condition in dic)
            {
                if (condition.Value.Length == 1 && string.IsNullOrEmpty(condition.Value[0])) continue;

                result.Add(condition.Key, condition.Value);

            }

            return result;
        }
    }
}
