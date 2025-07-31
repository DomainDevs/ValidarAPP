using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Company.Application.UniquePersonListRiskBusinessServiceProvider.Entities.Views
{
    [Serializable()]
    public class MassiveListRiskView : BusinessView
    {
        public BusinessCollection CiaAsynchronousProcessListRiskMassiveLoad
        {
            get
            {
                return this["CiaAsynchronousProcessListRiskMassiveLoad"];
            }
        }
        public BusinessCollection CiaAsynchronousProcessListriskRow
        {
            get
            {
                return this["CiaAsynchronousProcessListriskRow"];
            }
        }
    }
}
