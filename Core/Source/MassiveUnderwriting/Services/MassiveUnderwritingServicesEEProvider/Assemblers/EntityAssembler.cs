using System;
using System.Linq;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using MSVEN = Sistran.Core.Application.Massive.Entities;
using Sistran.Core.Application.MassiveServices.Models;

namespace Sistran.Core.Application.MassiveUnderwritingServices.EEProvider.Assemblers
{
    public class EntityAssembler
    {
        internal static MSVEN.MassiveEmissionRow CreateMassiveEmissionRow(MassiveEmissionRow massiveEmision)
        {
            int? tempId = default(int?);
            int? endorsementId = default(int?);
            if (massiveEmision.Risk != null && massiveEmision.Risk.Policy != null)
            {
                tempId = massiveEmision.Risk.Policy.Id;
                if (massiveEmision.Risk.Policy.Endorsement != null)
                {
                    endorsementId = massiveEmision.Risk.Policy.Endorsement.Id;
                }
            }

            MSVEN.MassiveEmissionRow entityMassiveEmision = new MSVEN.MassiveEmissionRow()
            {
                HasError = massiveEmision.HasError,
                MassiveLoadId = massiveEmision.MassiveLoadId,
                Observations = massiveEmision.Observations,
                RowNumber = massiveEmision.RowNumber,
                SerializedRow = massiveEmision.SerializedRow,
                StatusId = (int)massiveEmision.Status,
                HasEvents = massiveEmision.HasEvents,
                TempId = tempId,
                EndorsementId = endorsementId

            };
            return entityMassiveEmision;
        }

        internal static MSVEN.MassiveCancellationRow CreateMassiveCancellationRow(MassiveCancellationRow massiveCancellationRow)
        {
            int? tempId = default(int?);
            int? endorsementId = default(int?);
            if (massiveCancellationRow.Risk != null && massiveCancellationRow.Risk.Policy != null)
            {
                tempId = massiveCancellationRow.Risk.Policy.Id;
                if (massiveCancellationRow.Risk.Policy.Endorsement != null)
                {
                    endorsementId = massiveCancellationRow.Risk.Policy.Endorsement.Id;
                }
            }

            MSVEN.MassiveCancellationRow entityMassiveEmision = new MSVEN.MassiveCancellationRow()
            {
                HasError = massiveCancellationRow.HasError,
                MassiveLoadId = massiveCancellationRow.MassiveLoadId,
                Observations = massiveCancellationRow.Observations,
                RowNumber = massiveCancellationRow.RowNumber,
                SerializedRow = massiveCancellationRow.SerializedRow,
                StatusId = (int)massiveCancellationRow.Status,
                HasEvents = massiveCancellationRow.HasEvents,
                TempId = tempId,
                EndorsementId = endorsementId,
                SubCoveredRiskTypeCode = (int)massiveCancellationRow.SubcoveredRiskType
                

            };
            return entityMassiveEmision;
        }

        internal static MSVEN.MassiveEmission CreateMassiveEmission(MassiveEmission massiveEmission)
        {
            MSVEN.MassiveEmission entityMassiveEmission = new MSVEN.MassiveEmission(massiveEmission.Id)
            {
                PrefixId = massiveEmission.Prefix.Id,
                BranchId = massiveEmission.Branch.Id
            };

            if (massiveEmission.Agency != null)
            {
                entityMassiveEmission.AgencyId = massiveEmission.Agency.Id;
                if (massiveEmission.Agency.Agent != null)
                {
                    entityMassiveEmission.AgentId = massiveEmission.Agency.Agent.IndividualId;
                }
            }

            if (massiveEmission.Product != null)
            {
                entityMassiveEmission.ProductId = massiveEmission.Product.Id;
            }

            if (massiveEmission.BillingGroupId > 0)
            {
                entityMassiveEmission.BillingGroupId = massiveEmission.BillingGroupId;
            }

            if (massiveEmission.RequestId > 0)
            {
                entityMassiveEmission.RequestId = massiveEmission.RequestId;
                entityMassiveEmission.RequestNum = massiveEmission.RequestNumber;
            }

            if (massiveEmission.Branch.SalePoints != null && massiveEmission.Branch.SalePoints.Count > 0)
            {
                entityMassiveEmission.SalePointId = massiveEmission.Branch.SalePoints.First().Id;
            }

            if (massiveEmission.BusinessType.HasValue)
            {
                entityMassiveEmission.BusinessTypeId = (int)massiveEmission.BusinessType.Value;
            }

            if (massiveEmission.CoveredRiskType.HasValue)
            {
                entityMassiveEmission.CoveredRiskTypeCode = (int)massiveEmission.CoveredRiskType.Value;
            }

            return entityMassiveEmission;
        }
    }
}