using System.Configuration;

namespace Sistran.Core.Application.Utilities.RulesEngine
{
    public class RuleFunctionElement : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get
            {
                return (string)base["name"];
            }
            set
            {
                base["name"] = value;
            }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string AssemblyName
        {
            get
            {
                return (string)base["type"];
            }
            set
            {
                base["type"] = value;
            }
        }

        [ConfigurationProperty("methodname", IsRequired = true)]
        public string MethodName
        {
            get
            {
                return (string)base["methodname"];
            }
            set
            {
                base["methodname"] = value;
            }
        }

        public RuleFunctionElement()
        {

        }

        public RuleFunctionElement(string functionName, string assemblyName, string methodName)
        {
            this.Name = functionName;
            this.AssemblyName = assemblyName;
            this.MethodName = methodName;
        }
    }
}