using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class CompanyDeductibleView: BusinessView
    {
        public BusinessCollection Deductibles
        {
            get
            {
                return this["Deductible"];
            }
        }

        public BusinessCollection LineBusinesses
        {
            get
            {
                return this["LineBusiness"];
            }
        }

        public BusinessCollection DeductibleUnits
        {
            get
            {
                return this["DeductibleUnit"];
            }
        }

        public BusinessCollection MinimumDeductibleUnits
        {
            get
            {
                return this["MinimumDeductibleUnit"];
            }
        }

        public BusinessCollection DeductibleSubjects
        {
            get
            {
                return this["DeductibleSubject"];
            }
        }

        public BusinessCollection MinimumDeductibleSubjects
        {
            get
            {
                return this["MinimumDeductibleSubject"];
            }
        }

        public BusinessCollection Currencies
        {
            get
            {
                return this["Currency"];
            }
        }
    }
}
