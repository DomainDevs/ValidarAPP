using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs
{
    public class BeneficiaryTypeDAO
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<CompanyBeneficiaryType> GetCompanyBeneficiaryTypes()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.BeneficiaryType)));
            return ModelAssembler.CreateCompanyBeneficiaryTypes(businessCollection);
        }
    }
}
