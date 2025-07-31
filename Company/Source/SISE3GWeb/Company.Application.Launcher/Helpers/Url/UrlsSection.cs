using System.Configuration;

namespace Sistran.Core.Framework.UIF.Web.Helpers
{
    public class UrlsSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true, IsRequired = true)]
        public UrlCollection UrlAddresses
        {
            get
            {
                return (UrlCollection)this[""];
            }
            set
            {
                this[""] = value;
            }
        }
    }
    public class UrlCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new UrlElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((UrlElement)element).Name;
        }
        public UrlElement this[string idx]
        {
            get { return (UrlElement)BaseGet(idx); }
        }
    }

    public class UrlElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }

        [ConfigurationProperty("url", IsRequired = true)]
        public string Url
        {
            get
            {
                return (string)this["url"];
            }
            set
            {
                this["url"] = value;
            }
        }
    }

}