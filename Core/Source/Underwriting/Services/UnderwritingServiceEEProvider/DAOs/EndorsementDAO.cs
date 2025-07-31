using Sistran.Co.Application.Data;
using Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class EndorsementDAO
    {
        /// <summary>
        /// Obtener Endosos Validos de Una Póliza
        /// </summary>
        /// <param name="policyId">Id Póliza</param>
        /// <returns>Endosos</returns>
        public List<Models.Endorsement> GetEffectiveEndorsementsByPolicyId(int policyId)
        {
            NameValue[] parameters = new NameValue[1];

            parameters[0] = new NameValue("POLICY_ID", policyId);

            DataTable result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("TMP.GET_EFFECTIVE_ENDORSEMENTS_POLICY", parameters);
            }

            if (result != null && result.Rows.Count > 0)
            {
                List<Models.Endorsement> endorsements = new List<Models.Endorsement>();

                foreach (DataRow arrayItem in result.Rows)
                {
                    endorsements.Add(new Models.Endorsement
                    {
                        Id = Convert.ToInt32(arrayItem[0]),
                        EndorsementType = (Enums.EndorsementType)Convert.ToInt32(arrayItem[1]),
                        CurrentFrom = Convert.ToDateTime(arrayItem[2]),
                        CurrentTo = Convert.ToDateTime(arrayItem[3]),
                        IsCurrent = Convert.ToBoolean(arrayItem[4])
                    });
                }

                return endorsements;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener Endosos Validos de Una Póliza
        /// </summary>
        /// <param name="policyId">Id Póliza</param>
        /// <returns>Endosos</returns>
        public List<Models.Endorsement> GetEndorsementsByPolicyId(int policyId)
        {
            NameValue[] parameters = new NameValue[1];

            parameters[0] = new NameValue("POLICY_ID", policyId);

            DataTable result;

            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("TMP.GET_ENDORSEMENTS_POLICY", parameters);
            }

            if (result != null && result.Rows.Count > 0)
            {
                List<Models.Endorsement> endorsements = new List<Models.Endorsement>();

                foreach (DataRow arrayItem in result.Rows)
                {
                    endorsements.Add(new Models.Endorsement
                    {
                        Id = Convert.ToInt32(arrayItem[0]),
                        EndorsementType = (Enums.EndorsementType)Convert.ToInt32(arrayItem[1]),
                        CurrentFrom = Convert.ToDateTime(arrayItem[2]),
                        CurrentTo = Convert.ToDateTime(arrayItem[3]),
                        IsCurrent = Convert.ToBoolean(arrayItem[4])
                    });
                }

                return endorsements;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener Endoso actual por placa y póliza
        /// </summary>
        /// <param name="policyId">policyId</param>
        /// <param name="licensePlate">licensePlate</param>
        /// <returns>Endoso</returns>
        public Models.Endorsement GetCurrentEndorsementByPolicyIdLicensePlateId(int policyId, string licensePlate)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Policy.Properties.PolicyId, typeof(Policy).Name);
            filter.Equal();
            filter.Constant(policyId);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);
            filter.And();
            filter.Property(ISSEN.RiskVehicle.Properties.LicensePlate, typeof(ISSEN.RiskVehicle).Name);
            filter.Equal();
            filter.Constant(licensePlate);

            Models.Endorsement endorsement = new Models.Endorsement();

            EndorsementView view = new EndorsementView();
            ViewBuilder builder = new ViewBuilder("ExistsVehicleView");
            builder.Filter = filter.GetPredicate();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }

            if (view.EndorsementRisks.Count > 0)
            {
                foreach (ISSEN.EndorsementRisk item in view.EndorsementRisks)
                {
                    endorsement.Id = item.EndorsementId;
                }
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs.GetCurrentEndorsementByPolicyIdLicensePlateId");

            return endorsement;
        }
    }
}
