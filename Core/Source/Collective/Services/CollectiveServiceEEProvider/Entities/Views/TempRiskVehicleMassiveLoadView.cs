using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.CollectiveServices.EEProvider.Entities.Views
{
    [Serializable()]
    public class TempRiskVehicleMassiveLoadView : BusinessView
    {
        public BusinessCollection TempRisks
        {
            get
            {
                return this["TempRisk"];
            }
        }

        public BusinessCollection CoTempRisks
        {
            get
            {
                return this["CoTempRisk"];
            }
        }

        public BusinessCollection TempRiskVehicles
        {
            get
            {
                return this["TempRiskVehicle"];
            }
        }

        public BusinessCollection CoTempRiskVehicles
        {
            get
            {
                return this["CoTempRiskVehicle"];
            }
        }

        public BusinessCollection TempRiskBeneficiaries
        {
            get
            {
                return this["TempRiskBeneficiary"];
            }
        }

        public BusinessCollection TempRiskClauses
        {
            get
            {
                return this["TempRiskClause"];
            }
        }

        public BusinessCollection MassiveCollectiveDetails
        {
            get
            {
                return this["MassiveCollectiveDetail"];
            }
        }


        public BusinessCollection TempRiskCoverDetails
        {
            get
            {
                return this["TempRiskCoverDetail"];
            }
        }

        public BusinessCollection TempRiskDetails
        {
            get
            {
                return this["TempRiskDetail"];
            }
        }

        public BusinessCollection TempRiskDetailAccessories
        {
            get
            {
                return this["TempRiskDetailAccessory"];
            }
        }
    }
}
