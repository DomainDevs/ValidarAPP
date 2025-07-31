using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using System;
using System.Collections.Generic;

namespace Sistran.Core.Framework.UIF.Web.Areas.Massive.Models
{
    public class ModelAssembler
    {
        private static TMassive CreateMassiveLoad<TMassive>(MassiveViewModel massiveViewModel) where TMassive : MassiveLoad
        {
            TMassive massiveLoad = Activator.CreateInstance<TMassive>();
            massiveLoad.LoadType = new LoadType()
            {
                Id = massiveViewModel.LoadTypeId,
                ProcessType = (MassiveProcessType)massiveViewModel.ProcessTypeId
            };
            massiveLoad.Description = massiveViewModel.LoadName;
            massiveLoad.File = new Core.Services.UtilitiesServices.Models.File()
            {
                Name = massiveViewModel.FileName
            };
            massiveLoad.User = new Application.UniqueUserServices.Models.User()
            {
                UserId = SessionHelper.GetUserId(),
                AccountName = SessionHelper.GetUserName(),
                AuthenticationType = Application.UniqueUserServices.Enums.UniqueUserTypes.AuthenticationType.Standard
            };
            return massiveLoad;
        }

        public static MassiveRenewal CreateMassiveRenewal(MassiveViewModel massiveViewModel)
        {
            MassiveRenewal massiveRenewal = CreateMassiveLoad<MassiveRenewal>(massiveViewModel);
            massiveRenewal.Product = new Application.ProductServices.Models.Product()
            {
                Id = massiveViewModel.ProductId
            };
            massiveRenewal.Prefix = new Prefix()
            {
                Id = massiveViewModel.PrefixId
            };
            massiveRenewal.Branch = new Branch()
            {
                Id = massiveViewModel.BranchId.GetValueOrDefault()
            };
            massiveRenewal.CoveredRiskType = (CoveredRiskType)massiveViewModel.MassiveFileType;
            return massiveRenewal;
        }


        public static MassiveEmission CreateMassiveEmission(MassiveViewModel massiveViewModel)
        {
            MassiveEmission massiveEmission = CreateMassiveLoad<MassiveEmission>(massiveViewModel);

            massiveEmission.Agency = new IssuanceAgency()
            {
                Id = massiveViewModel.AgencyId,
                Agent = new IssuanceAgent()
                {
                    IndividualId = massiveViewModel.AgentId
                }
            };
            massiveEmission.Prefix = new Prefix()
            {
                Id = massiveViewModel.PrefixId
            };
            massiveEmission.Branch = new Branch()
            {
                Id = massiveViewModel.BranchId.GetValueOrDefault()
            };
            massiveEmission.CoveredRiskType = (CoveredRiskType)massiveViewModel.MassiveFileType;

            //massiveEmission.Product = new Application.UnderwritingServices.Models.Product()
            //{
            //    Id = massiveViewModel.ProductId
            //};

            //if (massiveViewModel.BillingGroupId.GetValueOrDefault() > 0)
            //{
            //    massiveEmission.BillingGroupId = massiveViewModel.BillingGroupId.Value;
            //}

            //if (massiveViewModel.RequestGroupId.GetValueOrDefault() > 0)
            //{
            //    massiveEmission.RequestId = massiveViewModel.RequestGroupId.Value;
            //    massiveEmission.RequestNumber = massiveViewModel.RequestGroupNumber.Value;
            //}

            if (massiveViewModel.SalesPointId.GetValueOrDefault() > 0)
            {
                massiveEmission.Branch.SalePoints = new List<SalePoint>();
                massiveEmission.Branch.SalePoints.Add(new SalePoint
                {
                    Id = massiveViewModel.SalesPointId.Value
                });
            }

            if (massiveViewModel.BusinessTypeId.GetValueOrDefault() > 0)
            {
                massiveEmission.BusinessType = (BusinessType)massiveViewModel.BusinessTypeId.Value;
            }

            return massiveEmission;
        }
        public static CollectiveEmission CreateCollectiveEmission(MassiveViewModel massiveViewModel)
        {
            CollectiveEmission collectiveEmission = new CollectiveEmission
            {
                LoadType = new LoadType
                {
                    Id = massiveViewModel.LoadTypeId,
                    ProcessType = (MassiveProcessType)massiveViewModel.ProcessTypeId
                },
                Agency = new IssuanceAgency
                {
                    Id = massiveViewModel.AgencyId,
                    Agent = new IssuanceAgent
                    {
                        IndividualId = massiveViewModel.AgentId
                    }
                },
                Prefix = new Prefix
                {
                    Id = massiveViewModel.PrefixId
                },
                Branch = new Branch
                {
                    Id = massiveViewModel.BranchId.GetValueOrDefault()
                },
                Product = new Application.ProductServices.Models.Product
                {
                    Id = massiveViewModel.ProductId
                },
                Description = massiveViewModel.LoadName,
                File = new Core.Services.UtilitiesServices.Models.File
                {
                    Name = massiveViewModel.FileName
                },
                CoveredRiskType = (CoveredRiskType)massiveViewModel.MassiveFileType
        };

            return collectiveEmission;
        }
        public static Policy CreatePolicyByRenewalViewModel(RenewalViewModel renewalViewModel)
        {

            List<IssuanceAgency> agencies = new List<IssuanceAgency>();
            agencies.Add(new IssuanceAgency
            {
                Id = renewalViewModel.AgencyId.GetValueOrDefault(),
                Agent = new IssuanceAgent
                {
                    IndividualId = renewalViewModel.AgentId.GetValueOrDefault()
                }
            });

            return new Policy
            {
                Prefix = new Prefix
                {
                    Id = renewalViewModel.PrefixId
                },
                CurrentFrom = Convert.ToDateTime(renewalViewModel.DueDateFrom),
                CurrentTo = Convert.ToDateTime(renewalViewModel.DueDateTo),
                Agencies = agencies,
                Branch = new Branch
                {
                    Id = renewalViewModel.BranchId.GetValueOrDefault()
                },

                Holder = new Holder
                {
                    IndividualId = renewalViewModel.HolderId.GetValueOrDefault()
                },
                Request = new Request
                {
                    Id = renewalViewModel.RequestGroupId.GetValueOrDefault()
                }
            };
        }
        public static MassiveLoad CreateCancellationMassiveLoad(CancellationMassiveViewModel cancellationMassiveViewModel)
        {
            MassiveLoad massiveLoad = new MassiveLoad
            {
                Description = cancellationMassiveViewModel.LoadName,
                File = new Core.Services.UtilitiesServices.Models.File { Name = cancellationMassiveViewModel.FileName, Description = cancellationMassiveViewModel.FileName },
                LoadType = new LoadType
                {
                    Id = (int)SubMassiveProcessType.CancellationMassive,
                    ProcessType = MassiveProcessType.Cancellation
                }
            };
            return massiveLoad;
        }
        public static MassiveLoad CreateCancellationMassiveLoad(MassiveViewModel cancellationMassiveViewModel)
        {
            MassiveLoad massiveLoad = new MassiveLoad
            {
                Description = cancellationMassiveViewModel.LoadName,
                File = new Core.Services.UtilitiesServices.Models.File { Name = cancellationMassiveViewModel.FileName, Description = cancellationMassiveViewModel.FileName },
                LoadType = new LoadType
                {
                    Id = (int)SubMassiveProcessType.CancellationMassive,
                    ProcessType = MassiveProcessType.Cancellation
                }
            };
            return massiveLoad;
        }

    }
}