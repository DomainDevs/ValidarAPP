using Sistran.Core.Application.BaseEndorsementService.EEProvider.Assemblers;
using Sistran.Core.Application.EndorsementBaseService.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Core.Application.BaseEndorsementService.EEProvider.DAOs
{
    public class EndorsementReasonDAO
    {
        /// <summary>
        /// Obtener lista de motivos del endoso
        /// </summary>
        /// <param name="EndorsementType">Tipo de endoso</param>
        /// <returns>Lista de motivos del endoso</returns>
        public List<EndorsementReason> GetEndorsementReasonsByEndorsementType(EndorsementType endorsementType)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.EndorsementReason.Properties.EndoTypeCode, typeof(ISSEN.EndorsementReason).Name);
            filter.Equal();
            filter.Constant(endorsementType);
            if (endorsementType == EndorsementType.Cancellation)
            {
                filter.And();
                filter.Property(ISSEN.EndorsementReason.Properties.IsEnabled, typeof(ISSEN.EndorsementReason).Name);
                filter.Equal();
                filter.Constant(true);
            }

            if (endorsementType == EndorsementType.Modification)
            {
                filter.And();
                filter.Property(ISSEN.EndorsementReason.Properties.IsEnabled, typeof(ISSEN.EndorsementReason).Name);
                filter.Equal();
                filter.Constant(true);
            }
            BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(ISSEN.EndorsementReason), filter.GetPredicate());

            if (businessCollection != null)
            {
                return ModelAssembler.CreateEndorsementReasons(businessCollection);
            }
            else
            {
                return null;
            }
        }
    }
}
