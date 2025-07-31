using System.Collections.Generic;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.DAF;
using MSVEN = Sistran.Core.Application.Massive.Entities;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.ProductServices.Models;

namespace Sistran.Core.Application.MassiveUnderwritingServices.EEProvider.Assemblers
{
    public class ModelAssembler
    {

        public static MassiveEmission CreateMassiveEmission(MSVEN.MassiveLoad entityMassiveLoad, MSVEN.MassiveEmission entityMassiveEmission)
        {
            MassiveEmission massiveEmission = new MassiveEmission()
            {
                LoadType = new LoadType
                {
                    Id = entityMassiveLoad.LoadTypeId.GetValueOrDefault()
                },
                Agency = new IssuanceAgency()
                {
                    Id = entityMassiveEmission.AgencyId,
                    Agent = new IssuanceAgent()
                    {
                        IndividualId = entityMassiveEmission.AgentId
                    }
                },
                BillingGroupId = entityMassiveEmission.BillingGroupId,
                Branch = new Branch() { Id = entityMassiveEmission.BranchId },
                BusinessType = (BusinessType?)entityMassiveEmission.BusinessTypeId,

                Id = entityMassiveEmission.MassiveLoadId,
                Prefix = new Prefix() { Id = entityMassiveEmission.PrefixId },
                Product = new Product() { Id = entityMassiveEmission.ProductId },
                RequestId = entityMassiveEmission.RequestId,
                RequestNumber = entityMassiveEmission.RequestNum,
                HasError = entityMassiveLoad.HasError,
                Status = (MassiveLoadStatus)entityMassiveLoad.StatusId,
                CoveredRiskType = entityMassiveEmission.CoveredRiskTypeCode == null ? (CoveredRiskType?)null : (CoveredRiskType)entityMassiveEmission.CoveredRiskTypeCode.Value
            };

            return massiveEmission;
        }

        public static List<MassiveEmissionRow> CreateMassiveEmissionRows(BusinessCollection businessCollection)
        {
            List<MassiveEmissionRow> massiveEmissionRows = new List<MassiveEmissionRow>();

            foreach (MSVEN.MassiveEmissionRow massiveEmissionRow in businessCollection)
            {
                massiveEmissionRows.Add(ModelAssembler.CreateMassiveEmissionRow(massiveEmissionRow));
            }

            return massiveEmissionRows;
        }

        public static MassiveEmissionRow CreateMassiveEmissionRow(MSVEN.MassiveEmissionRow entityMassiveLoadProcess)
        {
            MassiveEmissionRow massiveEmissionRow = new MassiveEmissionRow
            {
                Id = entityMassiveLoadProcess.Id,
                Risk = new Risk
                {
                    Policy = new Policy
                    {
                        Id = entityMassiveLoadProcess.TempId.GetValueOrDefault(),
                        DocumentNumber = entityMassiveLoadProcess.DocumentNumber.GetValueOrDefault(),
                        Endorsement = new Endorsement()
                        {
                            Id = entityMassiveLoadProcess.EndorsementId.GetValueOrDefault()
                        },
                        PolicyType = new PolicyType
                        {
                            Description = entityMassiveLoadProcess.PolicyType,
                        },
                        Summary = new Summary
                        {
                            FullPremium = entityMassiveLoadProcess.Premium.GetValueOrDefault(),
                        }
                    },
                    Description = entityMassiveLoadProcess.RiskDescription
                },
                Status = (MassiveLoadProcessStatus)entityMassiveLoadProcess.StatusId,
                HasError = entityMassiveLoadProcess.HasError.GetValueOrDefault(),
                SerializedRow = entityMassiveLoadProcess.SerializedRow,
                MassiveLoadId = entityMassiveLoadProcess.MassiveLoadId,
                Observations = entityMassiveLoadProcess.Observations,
                RowNumber = entityMassiveLoadProcess.RowNumber,
                TotalCommission = entityMassiveLoadProcess.Commiss.GetValueOrDefault(),
                HasEvents = entityMassiveLoadProcess.HasEvents.GetValueOrDefault(),
                TempId=entityMassiveLoadProcess.TempId

            };
            return massiveEmissionRow;
        }


        public static List<MassiveCancellationRow> CreateMassiveCancellationRows(BusinessCollection businessCollection)
        {
            List<MassiveCancellationRow> massiveCancellationRows = new List<MassiveCancellationRow>();

            foreach (MSVEN.MassiveCancellationRow massiveCancellationRow in businessCollection)
            {
                massiveCancellationRows.Add(ModelAssembler.CreateMassiveCancellationRow(massiveCancellationRow));
            }

            return massiveCancellationRows;
        }

        public static MassiveCancellationRow CreateMassiveCancellationRow(MSVEN.MassiveCancellationRow entityMassiveLoadProcess)
        {
            MassiveCancellationRow massiveCancellationRow = new MassiveCancellationRow
            {
                Id = entityMassiveLoadProcess.Id,
                Risk = new Risk
                {
                    Policy = new Policy
                    {
                        Id = entityMassiveLoadProcess.TempId.GetValueOrDefault(),
                        DocumentNumber = entityMassiveLoadProcess.DocumentNumber.GetValueOrDefault(),
                        Endorsement = new Endorsement()
                        {
                            Id = entityMassiveLoadProcess.EndorsementId.GetValueOrDefault()
                        },
                        PolicyType = new PolicyType
                        {
                            Description = entityMassiveLoadProcess.PolicyType,
                        },
                        Summary = new Summary
                        {
                            FullPremium = entityMassiveLoadProcess.Premium.GetValueOrDefault(),
                        }
                    },
                    Description = entityMassiveLoadProcess.RiskDescription
                },
                Status = (MassiveLoadProcessStatus)entityMassiveLoadProcess.StatusId,
                HasError = entityMassiveLoadProcess.HasError.GetValueOrDefault(),
                SerializedRow = entityMassiveLoadProcess.SerializedRow,
                MassiveLoadId = entityMassiveLoadProcess.MassiveLoadId,
                Observations = entityMassiveLoadProcess.Observations,
                RowNumber = entityMassiveLoadProcess.RowNumber,
                TotalCommission = entityMassiveLoadProcess.Commiss.GetValueOrDefault(),
                HasEvents = entityMassiveLoadProcess.HasEvents.GetValueOrDefault(),
                SubcoveredRiskType = (SubCoveredRiskType) entityMassiveLoadProcess.SubCoveredRiskTypeCode,
                tempId = entityMassiveLoadProcess.TempId


            };
            return massiveCancellationRow;
        }
    }
}