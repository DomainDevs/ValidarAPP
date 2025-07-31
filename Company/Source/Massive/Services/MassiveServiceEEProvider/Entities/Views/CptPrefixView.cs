using System;
using Sistran.Core.Framework.Views;
using Sistran.Core.Framework.DAF;

namespace Sistran.Company.Application.MassiveServices.EEProvider.Entities.View
{
    [Serializable()]
    public class CptPrefixView : BusinessView
    {
        //public BusinessCollection AgentPrefixes
        //{
        //    get
        //    {
        //        return this["AgentPrefix"];
        //    }
        //}

        public BusinessCollection Prefixes
        {
            get
            {
                return this["Prefix"];
            }
        }

        public BusinessCollection CptPrefix
        {
            get
            {
                return this["CptPrefix"];
            }
        }
    }
}
