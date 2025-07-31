using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class SuretyContractCategoriesDAO
    {
        public List<SuretyContractCategories> GetSuretyContractCategories()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.SuretyContractCategories)));
            return ModelAssembler.CreateSuretyContractCategories(businessCollection);
        }
    }
}