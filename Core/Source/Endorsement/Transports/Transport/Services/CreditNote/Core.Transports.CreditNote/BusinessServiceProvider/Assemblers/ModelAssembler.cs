using Sistran.Core.Application.Transports.Endorsement.CreditNote.BusinessServices.Models;
using PARAM = Sistran.Core.Application.Parameters.Entities;
using System.Collections.Generic;
using Sistran.Core.Framework.DAF;


namespace Sistran.Core.Application.Transports.CreditNote.BusinessService.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        /// <summary>
        /// Lista Tipo de Endoso
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns></returns>
        public static List<EndorsementType> GetEndorsmenteTypesHasQuotation(BusinessCollection businessCollection)
        {
            List<EndorsementType> endorsementByTypeHasQuotations = new List<EndorsementType>();
            foreach (PARAM.EndorsementType field in businessCollection)
            {
                endorsementByTypeHasQuotations.Add(CreateEndorsementByTypeHasQuotation(field));
            } 
            return endorsementByTypeHasQuotations;
        }
        /// <summary>
        /// Un Tipo de Endoso
        /// </summary>
        /// <returns></returns>
        internal static EndorsementType CreateEndorsementByTypeHasQuotation(PARAM.EndorsementType endorsementType)
        {
            if (endorsementType == null)
                return null;

            return new EndorsementType
            {
                Id = endorsementType.EndoTypeCode,
                Description = endorsementType.Description
            };
        }
    }
}
