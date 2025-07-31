
using AutoMapper;
using Sistran.Core.Application.BaseEndorsementService.DTOs;
using Sistran.Core.Application.EndorsementBaseService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using TMPEN = Sistran.Core.Application.Temporary.Entities;
namespace Sistran.Core.Application.BaseEndorsementService.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region EndorsementReason

        public static EndorsementReason CreateEndorsementReason(ISSEN.EndorsementReason endorsementReason)
        {
            return new EndorsementReason()
            {
                Id = endorsementReason.EndoReasonCode,
                Description = endorsementReason.Description
            };
        }

        public static List<EndorsementReason> CreateEndorsementReasons(BusinessCollection businessCollection)
        {
            List<EndorsementReason> endorsementReasons = new List<EndorsementReason>();

            foreach (ISSEN.EndorsementReason field in businessCollection)
            {
                endorsementReasons.Add(ModelAssembler.CreateEndorsementReason(field));
            }

            return endorsementReasons;
        }

        #endregion

        #region Endorsement

        public static Endorsement CreateEndorsementByTempSubscription(TMPEN.TempSubscription tempSubscription)
        {
            Endorsement endorsement = new Endorsement
            {
                Id = tempSubscription.EndorsementId.Value,
                PolicyId = tempSubscription.PolicyId.Value,
                EndorsementType = (EndorsementType)tempSubscription.EndorsementTypeCode.Value
            };

            if (tempSubscription.OperationId.HasValue)
            {
                endorsement.TemporalId = tempSubscription.OperationId.Value;
            }
            else
            {
                endorsement.TemporalId = tempSubscription.TempId;
            }

            return endorsement;
        }

        #endregion

        #region EndorsementModificactiontype

        /// <summary>
        /// Creates the type of the endorsement modification.
        /// </summary>
        /// <param name="endorsementModificationType">Type of the endorsement modification.</param>
        /// <returns></returns>
        public static EndorsementTypeDTO CreateEndorsementModificationType(PARAMEN.EndorsementModificationType endorsementModificationType)
        {
            IMapper mapper = AutoMapperAssembler.CreateMapModificationType();
            return mapper.Map<PARAMEN.EndorsementModificationType, EndorsementTypeDTO>(endorsementModificationType);
        }

        /// <summary>
        /// Creates the type of the endorsement modification.
        /// </summary>
        /// <param name="endorsementModificationTypes">The endorsement modification types.</param>
        /// <returns></returns>
        public static List<EndorsementTypeDTO> CreateEndorsementModificationTypes(List<PARAMEN.EndorsementModificationType> endorsementModificationTypes)
        {
            IMapper mapper = AutoMapperAssembler.CreateMapModificationType();
            return mapper.Map<List<PARAMEN.EndorsementModificationType>, List<EndorsementTypeDTO>>(endorsementModificationTypes);
        }

        #endregion

    }
}
