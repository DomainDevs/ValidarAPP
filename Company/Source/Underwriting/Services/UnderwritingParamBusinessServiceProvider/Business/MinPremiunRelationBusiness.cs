using Sistran.Company.Application.UnderwritingParamBusinessService.Model;
using Sistran.Company.Application.UnderwritingParamBusinessServiceProvider.DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingParamBusinessServiceProvider.Business
{
    public class MinPremiunRelationBusiness
    {
        public CompanyParamMinPremiunRelation Create(CompanyParamMinPremiunRelation param)
        {
            var dao = new MinPremiunRelationDAO();
            return dao.Create(param);
        }
        public CompanyParamMinPremiunRelation Update(CompanyParamMinPremiunRelation param)
        {
            var dao = new MinPremiunRelationDAO();
            return dao.Update(param);
        }
        public string Delete(CompanyParamMinPremiunRelation param)
        {
            var dao = new MinPremiunRelationDAO();
            return dao.Delete(param.Id);
        }
        public List<CompanyParamMinPremiunRelation> GetAll()
        {
            var dao = new MinPremiunRelationDAO();
            return dao.GetAll();
        }
        public List<CompanyParamMinPremiunRelation> GetByPrefixIdAndProductName(int PrefixId, string ProductName)
        {
            var dao = new MinPremiunRelationDAO();
            return dao.GetByPrefixIdAndProductName(PrefixId, ProductName);
        }

        public string GenerateExcel(string fileName)
        {
            var dao = new MinPremiunRelationDAO();
            var result = dao.GetListMinPremiumRelation();
            return dao.GenerateExcel(result, fileName);
        }
        public List<CompanyParamCoverage> GetCoverageByPrefixId(int PrefixId)
        {
            var business = new MinPremiunRelationDAO();
            return business.GetCoverageByPrefixId(PrefixId);
        }
        public List<CompanyParamCoverage> GetAllMinRange()
        {
            var business = new MinPremiunRelationDAO();
            return business.GetAllMinRange();
        }
    }
}
