using System;
using System.Configuration;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RulesFunctionCollection : ConfigurationElementCollection
    {
        public RulesFunctionCollection()
        {

        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override string ElementName
        {
            get
            {
                return "function";
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new RuleFunctionElement();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((RuleFunctionElement)element).Name;
        }

        public RuleFunctionElement this[int index]
        {
            get
            {
                return (RuleFunctionElement)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }

                BaseAdd(index, value);
            }
        }

        new public RuleFunctionElement this[string Name]
        {
            get
            {
                return (RuleFunctionElement)BaseGet(Name);
            }
        }

        public int IndexOf(RuleFunctionElement url)
        {
            return BaseIndexOf(url);
        }

        public void Add(RuleFunctionElement url)
        {
            BaseAdd(url);
        }

        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(RuleFunctionElement url)
        {
            if (BaseIndexOf(url) >= 0)
            {
                BaseRemove(url.Name);
            }
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }
    }
}