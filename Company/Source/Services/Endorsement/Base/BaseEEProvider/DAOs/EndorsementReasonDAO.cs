using Sistran.Company.Application.BaseEndorsementService.EEProvider.Assemblers;
using Sistran.Company.Application.EndorsementBaseService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Company.Application.BaseEndorsementService.EEProvider.DAOs
{
    public class CiaEndorsementReasonDAO
    {
        /// <summary>
        /// Obtener lista de motivos del endoso
        /// </summary>
        /// <param name="EndorsementType">Tipo de endoso</param>
        /// <returns>Lista de motivos del endoso</returns>
        public List<CiaEndorsementReason> GetEndorsementReasonsByEndorsementType(EndorsementType endorsementType)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.EndorsementReason.Properties.EndoTypeCode, typeof(ISSEN.EndorsementReason).Name);
            filter.Equal();
            filter.Constant(endorsementType);

            BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(ISSEN.EndorsementReason), filter.GetPredicate());

            if (businessCollection != null)
            {
                return ModelAssembler.CreateCiaEndorsementReasons(businessCollection);
            }
            else
            {
                return null;
            }
        }
    }
}
