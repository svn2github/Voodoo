using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Voodoo.Net
{
    /// <summary>
    /// 采集规则操作类
    /// </summary>
    public class CollectionRulesView
    {
        /// <summary>
        /// 根据域名获得规则
        /// </summary>
        /// <param name="Domain"></param>
        /// <param name="Rules"></param>
        /// <returns></returns>
        public static CollectionRules GetRuleByDmain(string Domain,List<CollectionRules> Rules)
        {
            try
            {
                return Rules.Where(p => p.Domain == Domain).ToList()[0];
            }
            catch
            {
                return null;
            }
        }
    }
}
