
using Sistran.Company.Application.UniquePerson.Entities;
using Sistran.Company.Application.UniquePersonParamService.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonParamService.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Application.Utilities.Error;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sistran.Company.Application.UniquePersonParamService.EEProvider.DAOs
{
    public class CompanyAddressDAO
    {
        public Result<List<CompanyAddress>, ErrorModel> GetAddress(Boolean?  isMail)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (isMail.Equals(true))
            {
                filter.Property(CptAddressType.Properties.IsElectronicMail, typeof(CptAddressType).Name);
                filter.Equal();
                filter.Constant(isMail);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CptAddressType), filter.GetPredicate()));
                List<CompanyAddress> lstAddress = ModelAssembler.GetAddress(businessCollection);
                return new ResultValue<List<CompanyAddress>, ErrorModel>(lstAddress);
            }
            else
            {
                filter.Property(CptAddressType.Properties.IsElectronicMail, typeof(CptAddressType).Name);
                filter.Equal();
                filter.Constant(isMail);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CptAddressType), filter.GetPredicate()));
                List<CompanyAddress> lstAddress = ModelAssembler.GetAddress(businessCollection);
                return new ResultValue<List<CompanyAddress>, ErrorModel>(lstAddress);
            }
        }
    }
}
