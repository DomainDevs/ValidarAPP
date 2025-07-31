using System.Collections;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DTOs
{
    /// <summary>
    /// Summary description for RuleDefCollection.
    /// </summary>
    public class RuleDefCollection : CollectionBase
    {

        public RuleDef this[int index]
        {
            get
            {
                return (RuleDef)InnerList[index];
            }
            set
            {
                InnerList[index] = value;
            }
        }

        public void Add(RuleDef rule)
        {
            InnerList.Add(rule);
        }

        public void Remove(RuleDef rule)
        {
            InnerList.Remove(rule);
        }
    }
}