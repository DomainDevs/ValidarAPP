using Sistran.Company.Application.SarlaftBusinessServices.Models;
using Sistran.Company.Application.SarlaftBusinessServicesProvider.DAO;
using Sistran.Core.Application.AuthorizationPoliciesServices.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.SarlaftBusinessServicesProvider.Business
{
    public class CompanySarlaftOperationBusiness
    {

        public List<AuthorizationRequest> GetSarlaftAuthorizationRequestByIndividualId(int individualId)
        {
            try
            {
                
                SarlaftDAO sarlaftDao = new SarlaftDAO();
                List<CompanyTmpSarlaftOperation> companyTmpSarlaftOperations = sarlaftDao.GetSarlaftOperationTmp(individualId);
                List<AuthorizationRequest> authorizationRequests = new List<AuthorizationRequest>();

                foreach (CompanyTmpSarlaftOperation companyTmpSarlaftOperation in companyTmpSarlaftOperations)
                {
                    AuthorizationRequest request = new AuthorizationRequest
                    {
                        Status = (Core.Application.AuthorizationPoliciesServices.Enums.TypeStatus)Enum.Parse(typeof(Core.Application.AuthorizationPoliciesServices.Enums.TypeStatus), companyTmpSarlaftOperation.StatusId.ToString()),
                        AuthorizationRequestId = companyTmpSarlaftOperation.RequestId,
                        FunctionType = (Core.Application.AuthorizationPoliciesServices.Enums.TypeFunction)Enum.Parse(typeof(Core.Application.AuthorizationPoliciesServices.Enums.TypeFunction), companyTmpSarlaftOperation.FunctionId.ToString()),

                    };
                    authorizationRequests.Add(request);
                }

                return authorizationRequests;



            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}
