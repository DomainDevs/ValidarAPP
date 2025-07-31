using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.CommonServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    public class ClaimDAO
    {
        #region ClaimNoticeType
        public List<ClaimNoticeType> GetClaimNoticeTypes()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PARAMEN.ClaimNoticeType)));
            return ModelAssembler.CreateClaimNoticeTypes(businessCollection);
        }
        #endregion
    }
}
