using System.Configuration;

namespace ApplicationServer.ManagerSection
{
    public class ConnectionManagerEndpointElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("address", IsRequired = true)]
        public string Address
        {
            get { return (string)this["address"]; }
            set { this["address"] = value; }
        }

        [ConfigurationProperty("api", IsRequired = true)]
        public string Api
        {
            get { return (string)this["api"]; }
            set { this["api"] = value; }
        }
    }
}
