using System.Configuration;

namespace ApplicationServer.ManagerSection
{
    public class ConnectionManagerEndpointsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ConnectionManagerEndpointElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ConnectionManagerEndpointElement)element).Name;
        }
    }
}
