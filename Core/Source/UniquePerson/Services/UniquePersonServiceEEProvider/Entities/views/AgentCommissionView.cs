using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.Entities.views
{
    [Serializable()]
    public class AgentCommissionView : BusinessView
    {
        public BusinessCollection AgentCommissions
        {
            get
            {
                return this["AgencyCommissRate"];
            }
        }

        public BusinessCollection AgentAgencys
        {
            get
            {
                return this["AgentAgency"];
            }
        }
        public BusinessCollection Prefixes
        {
            get
            {
                return this["Prefix"];
            }
        }

        public BusinessCollection LineBusinesss
        {
            get
            {
                return this["LineBusiness"];
            }
        }
        public BusinessCollection SubLineBusinesss
        {
            get
            {
                return this["SubLineBusiness"];
            }
        }
    }
}
