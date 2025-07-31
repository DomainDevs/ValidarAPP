using System;
using Sistran.Core.Application.CommonService.Models;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.MassiveRenewalServices.Enums;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using ENT = Sistran.Core.Application.Massive.Entities;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductModel = Sistran.Core.Application.ProductServices.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.MassiveServices.Enums;
using AutoMapper;
using COMMON = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.MassiveRenewalServices.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        public static MassiveRenewal CreateMassiveRenewal(ENT.MassiveLoad massiveLoadEntity, ENT.MassiveRenewal massiveRenewalEntity)
        {
            MassiveRenewal massiveRenewal = new MassiveRenewal();
            massiveRenewal.Id = massiveRenewalEntity.MassiveLoadId;
            massiveRenewal.CurrentFrom = massiveRenewalEntity.CurrentFrom;
            massiveRenewal.CurrentTo = massiveRenewalEntity.CurrentTo;
            massiveRenewal.LoadType = new LoadType
            {
                Id = massiveLoadEntity.LoadTypeId.GetValueOrDefault()
            };
            massiveRenewal.Agency = new IssuanceAgency
            {
                Id = massiveRenewalEntity.AgencyId.GetValueOrDefault(),
                Agent = new IssuanceAgent()
                {
                    IndividualId = massiveRenewalEntity.AgentId.GetValueOrDefault()
                }
            };
            massiveRenewal.Prefix = new Prefix
            {
                Id = massiveRenewalEntity.PrefixId
            };
            massiveRenewal.RequestId = massiveRenewalEntity.RequestId;
            massiveRenewal.Product = new ProductServices.Models.Product
            {
                Id = massiveRenewalEntity.ProductId.GetValueOrDefault()
            };
            if (massiveRenewalEntity.HolderId.HasValue)
            {
                massiveRenewal.Hoder = new Holder
                {
                    InsuredId = massiveRenewalEntity.HolderId.GetValueOrDefault(),
                    CustomerType = Services.UtilitiesServices.Enums.CustomerType.Individual,
                    IndividualType = Services.UtilitiesServices.Enums.IndividualType.Person
                };
            }

            massiveRenewal.Status = (MassiveLoadStatus)massiveLoadEntity.StatusId;
            massiveRenewal.CoveredRiskType = massiveRenewalEntity.CoveredRiskTypeCode == null ? (CoveredRiskType?)null : (CoveredRiskType)massiveRenewalEntity.CoveredRiskTypeCode.Value;

            return massiveRenewal;
        }

        public static List<MassiveRenewalRow> CreateMassiveRenewalRows(BusinessCollection businessCollection)
        {
            List<MassiveRenewalRow> massiveRenewallProcesses = new List<MassiveRenewalRow>();

            foreach (ENT.MassiveRenewalRow entityMassiveRenewal in businessCollection)
            {
                massiveRenewallProcesses.Add(ModelAssembler.CreateMassiveRenewalRow(entityMassiveRenewal));
            }

            return massiveRenewallProcesses;
        }

        public static MassiveRenewalRow CreateMassiveRenewalRow(ENT.MassiveRenewalRow massiveRenewalRowEntity)
        {
            var massiveRenewalRow = new MassiveRenewalRow
            {
                Id = massiveRenewalRowEntity.Id,
                HasError = massiveRenewalRowEntity.HasError,
                MassiveRenewalId = massiveRenewalRowEntity.MassiveLoadId,
                RowNumber = massiveRenewalRowEntity.RowNumber,
                SerializedRow = massiveRenewalRowEntity.SerializedRow,
                TemporalId = massiveRenewalRowEntity.TempId,
                Observations = massiveRenewalRowEntity.Observations,
                Risk = new Risk
                {
                    Policy = new Policy
                    {
                        Id = massiveRenewalRowEntity.PolicyId,
                        DocumentNumber = massiveRenewalRowEntity.DocumentNumber.GetValueOrDefault(),
                        PolicyType = new PolicyType()
                        {
                            Description = massiveRenewalRowEntity.PolicyType,
                        },
                        Summary = new Summary
                        {
                            FullPremium = massiveRenewalRowEntity.Premium.GetValueOrDefault(),
                        }
                    },
                    Description = massiveRenewalRowEntity.RiskDescription
                },
                HasEvents = massiveRenewalRowEntity.HasEvents.GetValueOrDefault(),
                Status = (MassiveLoadProcessStatus?)massiveRenewalRowEntity.StatusId,
                TotalCommission = massiveRenewalRowEntity.Commiss.GetValueOrDefault(),

            };
            if (massiveRenewalRowEntity.EndorsementId.HasValue)
            {
                massiveRenewalRow.Risk.Policy.Endorsement = new Endorsement()
                {
                    Id = massiveRenewalRowEntity.EndorsementId.Value
                };
            }
            return massiveRenewalRow;
        }

        #region automaper
        public static IMapper CreateMapMassiveRenewal()
        {
            var config = MapperCache.GetMapper<MassiveLoad, MassiveRenewal>(cfg =>
            {
                cfg.CreateMap<MassiveLoad, MassiveRenewal>();
            });
            return config;
        }

        public static IMapper CreateMapField()
        {
            var config = MapperCache.GetMapper<COMMON.Field, COMMON.Field>(cfg =>
            {
                cfg.CreateMap<COMMON.Field, COMMON.Field>();
            });
            return config;
        }
        #endregion
    }
}