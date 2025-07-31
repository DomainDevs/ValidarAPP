using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Viewss
{
    [Serializable()]
    public class CoverageDeductibleView : BusinessView
    {
        public BusinessCollection CoverageDeductibles
        {
            get
            {
                return this["CoverageDeductible"];
            }
        }

        public BusinessCollection Deductibles
        {
            get
            {
                return this["Deductible"];
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
