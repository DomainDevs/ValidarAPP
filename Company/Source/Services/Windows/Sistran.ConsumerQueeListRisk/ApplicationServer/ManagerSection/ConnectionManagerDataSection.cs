using System.Configuration;

namespace ApplicationServer.ManagerSection
{
    public class ConnectionManagerDataSection : ConfigurationSection
    {
        public const string SectionName = "ApiConfig";
        private const string EndpointCollectionName = "ApiEndpoints";

        [ConfigurationProperty(EndpointCollectionName)]
        [ConfigurationCollection(typeof(ConnectionManagerEndpointsCollection), AddItemName = "add")]
        public ConnectionManagerEndpointsCollection ConnectionManagerEndpoints
        {
            get
            {
                return (ConnectionManagerEndpointsCollection)base[EndpointCollectionName];
            }
        }

        private static ConnectionManagerDataSection instance = null;

        public static ConnectionManagerDataSection GetInstance()
        {
            if (instance == null)
                instance = ConfigurationManager.GetSection(SectionName) as ConnectionManagerDataSection;

            return instance;
        }
    }
}
