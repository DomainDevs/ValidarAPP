using Sistran.Company.Application.EndorsementBaseService.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.DTOs.Filter;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Company.Application.BaseEndorsementService.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region EndorsementReason

        public static CiaEndorsementReason CreateCiaEndorsementReason(ISSEN.EndorsementReason endorsementReason)
        {
            return new CiaEndorsementReason
            {
                Id = endorsementReason.EndoReasonCode,
                Description = endorsementReason.Description
            };
        }

        public static List<CiaEndorsementReason> CreateCiaEndorsementReasons(BusinessCollection businessCollection)
        {
            List<CiaEndorsementReason> endorsementReasons = new List<CiaEndorsementReason>();

            foreach (ISSEN.EndorsementReason field in businessCollection)
            {
                endorsementReasons.Add(ModelAssembler.CreateCiaEndorsementReason(field));
            }

            return endorsementReasons;
        }

        #endregion EndorsementReason

        #region Endorsement



        #endregion Endorsement
        internal static ComponentValueDTO CreateCompanyComponentValueDTO(CompanySummary companySummary)
        {
            var imaper = AutoMapperAssembler.CreateMapCompanyComponentValueDTO();
            return imaper.Map<CompanySummary, ComponentValueDTO>(companySummary);
        }
    }
}
