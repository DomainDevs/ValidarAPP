using Sistran.Core.Application.Massive.Entities;
using MODEL = Sistran.Core.Application.MassiveRenewalServices.Models;

namespace Sistran.Core.Application.MassiveRenewalServices.EEProvider.Assemblers
{
    public class EntityAssembler
    {
        public static MassiveRenewal CreateMassiveRenewal(MODEL.MassiveRenewal massiveRenewalModel)
        {
            MassiveRenewal massiveRenewal = new MassiveRenewal(massiveRenewalModel.Id);
            massiveRenewal.CurrentFrom = massiveRenewalModel.CurrentFrom;
            massiveRenewal.CurrentTo = massiveRenewalModel.CurrentTo;
            if (massiveRenewalModel.Agency != null)
            {
                massiveRenewal.AgencyId = massiveRenewalModel.Agency.Id;
                massiveRenewal.AgentId = massiveRenewalModel.Agency.Agent.IndividualId;
            }
            if (massiveRenewalModel.Prefix != null)
            {
                massiveRenewal.PrefixId = massiveRenewalModel.Prefix.Id;
            }
            massiveRenewal.RequestId = massiveRenewalModel.RequestId;
            if (massiveRenewalModel.Product != null)
            {
                massiveRenewal.ProductId = massiveRenewalModel.Product.Id;
            }
            if (massiveRenewalModel.Hoder != null)
            {
                massiveRenewal.HolderId = massiveRenewalModel.Hoder.InsuredId;
            }
            if (massiveRenewalModel.CoveredRiskType.HasValue)
            {
                massiveRenewal.CoveredRiskTypeCode = (int)massiveRenewalModel.CoveredRiskType.Value;
            }

            //Falta el agentId
            return massiveRenewal;
        }

        public static MassiveRenewalRow CreateMassiveRenewalRow(MODEL.MassiveRenewalRow massiveRenewalRowModel)
        {
            MassiveRenewalRow massiveRenewalRow = new MassiveRenewalRow();
            massiveRenewalRow.HasError = massiveRenewalRowModel.HasError;
            massiveRenewalRow.MassiveLoadId = massiveRenewalRowModel.MassiveRenewalId;
            massiveRenewalRow.Observations = massiveRenewalRowModel.Observations;
            if (massiveRenewalRowModel.Risk != null && massiveRenewalRowModel.Risk.Policy != null)
            {
                massiveRenewalRow.PolicyId = massiveRenewalRowModel.Risk.Policy.Id;
                if(massiveRenewalRowModel.Risk.Policy.Endorsement != null)
                {
                    massiveRenewalRow.EndorsementId = massiveRenewalRowModel.Risk.Policy.Endorsement.Id;
                }
            }
            massiveRenewalRow.RowNumber = massiveRenewalRowModel.RowNumber;
            massiveRenewalRow.SerializedRow = massiveRenewalRowModel.SerializedRow;
            massiveRenewalRow.StatusId = (int?)massiveRenewalRowModel.Status;
            massiveRenewalRow.TempId = massiveRenewalRowModel.TemporalId;
            massiveRenewalRow.HasEvents = massiveRenewalRowModel.HasEvents;

            return massiveRenewalRow;
        }
    }
}