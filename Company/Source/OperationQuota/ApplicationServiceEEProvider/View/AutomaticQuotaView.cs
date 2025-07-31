using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.OperationQuotaServices.EEProvider.View
{
     public class AutomaticQuotaView : BusinessView
    {
        public BusinessCollection AutomaticQuota
        {
            get
            {
                return this["AutomaticQuota"];
            }
        }

        public BusinessCollection Indicator
        {
            get
            {
                return this["Indicator"];
            }
        }

        public BusinessCollection Third
        {
            get
            {
                return this["Third"];
            }
        }

        public BusinessCollection Utility
        {
            get
            {
                return this["Utility"];
            }
        }
        public BusinessCollection SummaryUtility
        {
            get
            {
                return this["SummaryUtility"];
            }
        }
        
        public BusinessCollection Prospect
        {
            get
            {
                return this["Prospect"];
            }
        }
        public BusinessCollection Person
        {
            get
            {
                return this["Person"];
            }
        }

        public BusinessCollection Company
        {
            get
            {
                return this["Company"];
            }
        }

    }
}
