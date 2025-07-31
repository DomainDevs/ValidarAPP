using System.Xml;

namespace Sistran.Core.Application.AuditServices.EEProvider.Helpers
{
    /// <summary>
    /// Helper Convertir String en XmlDocument
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// Ases the XML.
        /// </summary>
        /// <param name="xmlDoc">The XML document.</param>
        /// <returns></returns>
        public static XmlDocument AsXml(this string xmlDoc)
        {
            if (!string.IsNullOrEmpty(xmlDoc))
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlDoc);
                return doc;
            }
            else
            {
                return null;
            }
        }

    }

}
