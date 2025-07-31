using System;
using Sistran.Core.Framework.Views;
using Sistran.Core.Framework.DAF;

namespace Sistran.Company.Application.MassiveServices.EEProvider.Entities.View
{
    [Serializable()]
    public class CoProductAgentView : BusinessView
    {
        public BusinessCollection Products
        {
            get
            {
                return this["Product"];
            }
        }

        public BusinessCollection ProductAgents
        {
            get
            {
                return this["ProductAgent"];
            }
        }

        public BusinessCollection CoProducts
        {
            get
            {
                return this["CoProduct"];
            }
        }
    }
}
