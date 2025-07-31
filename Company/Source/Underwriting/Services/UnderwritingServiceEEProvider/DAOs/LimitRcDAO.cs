using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs
{
    public class LimitRcDAO
    {
        public CompanyLimitRc GetCompanyLimitRcById(int id)
        {
            CoLimitsRcDAO coLimitsRcDAO = new CoLimitsRcDAO();
            LimitRc limitRc = coLimitsRcDAO.GetLimitRcById(id);
            return ModelAssembler.CreateCompanyLimitRc(limitRc);
        }
    }
}
