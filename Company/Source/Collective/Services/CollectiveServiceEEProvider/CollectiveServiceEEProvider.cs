using Sistran.Company.Application.CollectiveServices.EEProvider.DAOs;
using Sistran.Company.Application.CollectiveServices.EEProvider.Resources;
using Sistran.Company.Application.CollectiveServices.Models;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CollectiveServices.EEProvider;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.CollectiveServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CollectiveServiceEEProvider : CollectiveServiceEEProviderCore, ICollectiveService
    {
        public CollectiveServiceEEProvider()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        public CompanyPolicy CreateCompanyPolicy(CollectiveEmission collectiveLoad, File file, string templatePropertyName, List<FilterIndividual> filtersIndividuals, int riskCount)
        {
            try
            {
                CollectiveLoadPolicyDAO collectiveLoadPolicyDAO = new CollectiveLoadPolicyDAO();
                return collectiveLoadPolicyDAO.CreateCompanyPolicy(collectiveLoad, file, templatePropertyName, filtersIndividuals, riskCount);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSetPolicyData), ex);
            }
        }

        public GroupCoverage CreateGroupCoverage(Row row, int productId)
        {
            try
            {
                CollectiveLoadRiskDAO dao = new CollectiveLoadRiskDAO();
                return dao.CreateGroupCoverage(row, productId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSetGroupCoverageData), ex);
            }
        }

        public RatingZone CreateRatingZone(Row row, int prefixId)
        {
            try
            {
                CollectiveLoadRiskDAO dao = new CollectiveLoadRiskDAO();
                return dao.CreateRatingZone(row, prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSetRatingZoneData), ex);
            }
        }

        public List<CompanyCoverage> CreateCoverages(int productId, int groupCoverageId, int prefixId)
        {
            try
            {
                CollectiveLoadRiskDAO dao = new CollectiveLoadRiskDAO();
                return dao.CreateCoverages(productId, groupCoverageId, prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCoverages), ex);
            }
        }

        public LimitRc CreateLimitRc(Row row, int prefixId, int productId, int policyTypeId)
        {
            try
            {
                CollectiveLoadRiskDAO collectiveLoadRiskDAO = new CollectiveLoadRiskDAO();
                return collectiveLoadRiskDAO.CreateLimitRc(row, prefixId, productId, policyTypeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSetLimitRcData), ex);
            }
        }

        public void ExcludeCompanyCollectiveEmissionRowsTemporals(int massiveLoadId, List<int> temps, string userName, bool deleteTemporal)
        {
            try
            {
                CollectiveLoadPolicyDAO collectiveLoadPolicyDAO = new CollectiveLoadPolicyDAO();
                collectiveLoadPolicyDAO.ExcludeCompanyCollectiveEmissionRowsTemporals(massiveLoadId, temps, userName, deleteTemporal);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExcludeRows), ex);
            }
        }

        public IssuedCollectiveLoad CreateIssuedPolicy(List<int> collectiveLoadIds, int tempId)
        {
            try
            {
                CollectiveLoadPolicyDAO dao = new CollectiveLoadPolicyDAO();
                return dao.CreateIssuedPolicy(collectiveLoadIds, tempId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorIssuePolicy), ex);
            }
        }

        public MassiveLoad IssuanceCollectiveEmission(int massiveLoadId)
        {
            try
            {
                CollectiveLoadPolicyDAO dao = new CollectiveLoadPolicyDAO();
                return dao.IssuanceCollectiveEmission(massiveLoadId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorIssuePolicy), ex);
            }
        }

        public MassiveLoad IssuanceCollectiveEndorsement(int massiveLoadId)
        {
            try
            {
                CollectiveLoadPolicyDAO dao = new CollectiveLoadPolicyDAO();
                return dao.IssuanceCollectiveEndorsement(massiveLoadId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorIssueEndorsement), ex);
            }
        }

        public string PrintCollectiveLoad(int massiveLoadId, int rangeFrom, int rangeTo, User user, bool checkIssuedDetail)
        {
            try
            {
                CollectivePrintDAO dao = new CollectivePrintDAO();
                return dao.PrintCollectiveLoad(massiveLoadId, rangeFrom, rangeTo, user, checkIssuedDetail);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorPrintMassiveLoad), ex);
            }
        }

        public List<ValidationIdentificator> GetGeneralValidationsByEmissionTemplate(File file, Row row, int userId)
        {
            try
            {
                CollectiveValidationDAO dao = new CollectiveValidationDAO();
                return dao.GetGeneralValidationsByEmissionTemplate(file, row, userId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorAddValidations), ex);
            }
        }

        public List<ValidationIdentificator> GetGeneralValidationsByRiskTemplate(int fileId, Row row)
        {
            try
            {
                CollectiveValidationDAO dao = new CollectiveValidationDAO();
                return dao.GetGeneralValidationsByRiskTemplate(fileId, row);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorAddValidations), ex);
            }
        }

        public List<ValidationPhoneType> GetPhoneTypesValidationsByRiskDetailTemplate(int fileId, Template template)
        {
            try
            {
                CollectiveValidationDAO dao = new CollectiveValidationDAO();
                return dao.GetPhoneTypesValidationsByRiskDetailTemplate(fileId, template);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorAddValidations), ex);
            }
        }

        public List<ValidationIdentificator> GetValidationsByAdditionalBeneficiariesTemplate(File file, Template template)
        {
            try
            {
                CollectiveValidationDAO dao = new CollectiveValidationDAO();
                return dao.GetValidationsByAdditionalBeneficiariesTemplate(file, template);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorAddValidations), ex);
            }
        }

        public List<ValidationPhoneType> GetPhoneTypesValidationsByEmissionTemplate(File file, Row row)
        {
            try
            {
                CollectiveValidationDAO dao = new CollectiveValidationDAO();
                return dao.GetPhoneTypesValidationsByEmissionTemplate(file, row);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorAddValidations), ex);
            }
        }

        public List<ValidationPhoneType> GetPhoneTypeValidationsByAdditionalBeneficiaries(int fileId, Template template)
        {
            try
            {
                CollectiveValidationDAO dao = new CollectiveValidationDAO();
                return dao.GetPhoneTypeValidationsByAdditionalBeneficiaries(fileId, template);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorAddValidations), ex);
            }
        }

        public List<ValidationRegularExpression> GetRegularExpressionValidationsByEmisionTemplate(int fileId, Row row)
        {
            try
            {
                CollectiveValidationDAO dao = new CollectiveValidationDAO();
                return dao.GetRegularExpressionValidationsByEmisionTemplate(fileId, row);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorAddValidations), ex);
            }
        }

        public List<ValidationRegularExpression> GetRegularExpressionValidationsByRiskTemplate(int fileId, Row row)
        {
            try
            {
                CollectiveValidationDAO dao = new CollectiveValidationDAO();
                return dao.GetRegularExpressionValidationsByRiskTemplate(fileId, row);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorAddValidations), ex);
            }
        }

        public List<ValidationRegularExpression> GetRegularExpressionValidationsByAdditionalBeneficiaries(int fileId, Template template)
        {
            try
            {
                CollectiveValidationDAO dao = new CollectiveValidationDAO();
                return dao.GetRegularExpressionValidationsByAdditionalBeneficiaries(fileId, template);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorAddValidations), ex);
            }
        }

       
    }
}