using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.AuthorizationPoliciesServices.EEProvider.DAOs;
using Sistran.Company.Application.AuthorizationPoliciesServices.Models;

namespace Sistran.Company.Application.AuthorizationPoliciesServices.EEProvider.Business
{
    public class ReportsPoliciesBusiness
    {
        public string GenerateFileToReportAuthorizationPolicies(string fileName, CompanyPolicyValid cpv)
        {
            ReportsPoliciesDAO policyDao = new ReportsPoliciesDAO();
            return policyDao.GenerateExcelReportAuthorizationPolicies(fileName, cpv);
        }
    }
}
