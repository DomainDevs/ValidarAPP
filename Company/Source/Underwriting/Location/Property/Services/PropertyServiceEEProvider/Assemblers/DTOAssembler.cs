using Sistran.Company.Application.Location.PropertyServices.DTO;
using Sistran.Company.Application.UnderwritingServices;
using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Location.PropertyServices.EEProvider.Assemblers
{
    public class DTOAssembler
    {
        public static List<EndorsementDTO> CreateEndorsements(List<CompanyEndorsement> companyEndorsements)
        {
            List<EndorsementDTO> EndorsementDTOs = new List<EndorsementDTO>();

            foreach (var companyEndorsement in companyEndorsements)
            {
                EndorsementDTOs.Add(CreateEndorsementDTO(companyEndorsement));
            }

            return EndorsementDTOs;
        }
        public static EndorsementDTO CreateEndorsementDTO(CompanyEndorsement companyEndorsement)
        {
            if (companyEndorsement == null)
            {
                return null;
            }

            return new EndorsementDTO
            {
                IdEndorsement = companyEndorsement.Id,
                EndorsementType = companyEndorsement.EndorsementType,
                CurrentFrom = companyEndorsement.CurrentFrom,
                CurrentTo = companyEndorsement.CurrentTo,
                IsCurrent = companyEndorsement.IsCurrent,

            };
        }
        public static EndorsementDTO CreateEndorsementDTO(Endorsement endorsement)
        {
            if (endorsement == null)
            {
                return null;
            }

            return new EndorsementDTO
            {
                TemporalId = endorsement.TemporalId,
                EndorsementType = endorsement.EndorsementType,
                IdEndorsement = endorsement.Id,
                CurrentFrom = endorsement.CurrentFrom,
                CurrentTo = endorsement.CurrentTo
            };
        }

        public static List<InsuredObjectDTO> CreateInsuredObjects(List<InsuredObject> InsuredObjects)
        {
            List<InsuredObjectDTO> insuredObjectDTO = new List<InsuredObjectDTO>();
            if (InsuredObjects != null)
            {
                foreach (var insuredObject in InsuredObjects)
                {
                    insuredObjectDTO.Add(CreateInsuredObject(insuredObject));
                }
            }
            return insuredObjectDTO;
        }

        public static InsuredObjectDTO CreateInsuredObject(InsuredObject InsuredObject)
        {
            if (InsuredObject == null)
            {
                return null;
            }
            return new InsuredObjectDTO
            {
                Id = InsuredObject.Id,
                Description = InsuredObject.Description,
                InsuredLimitAmount = InsuredObject.Amount,
                PremiumAmount = InsuredObject.Premium,
                IsSelected = InsuredObject.IsSelected,
                IsMandatory = InsuredObject.IsMandatory,
                IsDeclarative = InsuredObject.IsDeclarative
            };
        }
    }
}
