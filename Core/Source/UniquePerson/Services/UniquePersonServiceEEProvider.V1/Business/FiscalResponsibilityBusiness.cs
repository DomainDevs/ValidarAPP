using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class FiscalResponsibilityBusiness
    {
        public List<Models.FiscalResponsibility> GetFiscalResponsibility()
        {
            try
            {
                return ModelAssembler.CreateFiscalresponsibilities(DataFacadeManager.GetObjects(typeof(UniquePersonV1.Entities.FiscalResponsibility)));
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Models.FiscalResponsibility GetFiscaResponsibilityById(int id)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePersonV1.Entities.FiscalResponsibility.Properties.Id, typeof(UniquePersonV1.Entities.FiscalResponsibility).Name);
            filter.Equal();
            filter.Constant(id);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.FiscalResponsibility), filter.GetPredicate()));
            return ModelAssembler.CreateFiscalresponsibilities(businessCollection).FirstOrDefault();

        }
    }
}
