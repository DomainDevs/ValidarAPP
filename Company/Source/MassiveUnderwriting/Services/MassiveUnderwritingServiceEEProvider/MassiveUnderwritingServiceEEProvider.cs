using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.MassiveUnderwritingServices.EEProvider.DAOs;
using Sistran.Company.Application.MassiveUnderwritingServices.EEProvider.Resources;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.EEProvider;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.Utilities.Configuration;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Company.Application.MassiveServices.Models;

namespace Sistran.Company.Application.MassiveUnderwritingServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class MassiveUnderwritingServiceEEProvider : MassiveUnderwritingServiceEEProviderCore, IMassiveUnderwritingService
    {
        public MassiveUnderwritingServiceEEProvider()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        public GroupCoverage CreateGroupCoverage(Row row, int productId)
        {
            try
            {
                MassiveLoadRiskDAO dao = new MassiveLoadRiskDAO();
                return dao.CreateGroupCoverage(row, productId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateGroupCoverage), ex);
            }
        }
               
        public RatingZone CreateRatingZone(Row row, int prefixId)
        {
            try
            {
                MassiveLoadRiskDAO dao = new MassiveLoadRiskDAO();
                return dao.CreateRatingZone(row, prefixId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateRatingZone), ex);
            }
        }

        public MassiveLoad IssuanceMassiveEmission(int massiveLoadId)
        {
            try
            {
                MassiveLoadPolicyDAO massiveLoadPolicyDAO = new MassiveLoadPolicyDAO();
                return massiveLoadPolicyDAO.IssuanceMassiveEmission(massiveLoadId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateIssuePolicy), ex);
            }
        }

        public CompanyPolicy CreateCompanyPolicy(MassiveEmission massiveLoad, MassiveEmissionRow massiveEmissionRow, File file, string templatePropertyName, List<FilterIndividual> filtersIndividuals)
        {
            try
            {
                MassiveLoadPolicyDAO massiveLoadPolicyDAO = new MassiveLoadPolicyDAO();
                return massiveLoadPolicyDAO.CreateCompanyPolicy(massiveLoad, massiveEmissionRow, file, templatePropertyName, filtersIndividuals);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateCompanyPolicy), ex);
            }
        }

        public CompanyIssuanceInsured CreateInsured(Row row, Holder holder)
        {
            try
            {
                MassiveLoadRiskDAO massiveLoadRiskDAO = new MassiveLoadRiskDAO();
                return massiveLoadRiskDAO.CreateInsured(row, holder);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateInsured), ex);
            }
        }

        public List<Beneficiary> CreateAdditionalBeneficiaries(Template beneficiariesTemplate)
        {
            try
            {
                MassiveLoadRiskDAO massiveLoadRiskDAO = new MassiveLoadRiskDAO();
                return massiveLoadRiskDAO.CreateAdditionalBeneficiaries(beneficiariesTemplate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateAdditionalBeneficiaries), ex);
            }
        }

        public Beneficiary CreateBeneficiary(Row row, CompanyIssuanceInsured insured)
        {
            try
            {
                MassiveLoadRiskDAO massiveLoadRiskDAO = new MassiveLoadRiskDAO();
                return null;//massiveLoadRiskDAO.CreateBeneficiary(row, insured);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateBeneficiary), ex);
            }
        }

        public LimitRc CreateLimitRc(Row row, int prefixId, int productId, int policyTypeId)
        {
            try
            {
                MassiveLoadRiskDAO massiveLoadRiskDAO = new MassiveLoadRiskDAO();
                return massiveLoadRiskDAO.CreateLimitRc(row, prefixId, productId, policyTypeId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateLimitRc), ex);
            }
        }

        public string PrintMassiveLoad(int massiveLoadId, int rangeFrom, int rangeTo, User user, bool checkIssuedDetail)
        {
            try
            {
                MassivePrintDAO massivePrintDAO = new MassivePrintDAO();
                return massivePrintDAO.PrintMassiveLoad(massiveLoadId, rangeFrom, rangeTo, user, checkIssuedDetail);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorPrintMassiveLoad), ex);
            }
        }
        
        /// <summary>
        /// Obtiene las validaciones de las personas en la plantilla de emision
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="prefixId"></param>
        /// <param name="template"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public List<ValidationIdentificator> GetPersonValidationsByEmissionTemplate(int fileId, int prefixId, Template template, Row row)
        {
            try
            {
                MassiveValidationDAO massiveValidationDAO = new MassiveValidationDAO();
                return massiveValidationDAO.GetPersonValidationsByEmissionTemplate(fileId, prefixId, template, row);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExecuteValidations), ex);
            }
        }

        /// <summary>
        /// Gets the general validations for massive load (Emmission Template).
        /// </summary>
        /// <param name="file">The massive file emission.</param>
        /// <param name="template">File template.</param>
        /// <param name="endorsementType">Specific Row.</param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public List<ValidationIdentificator> GetGeneralValidationsByEmissionTemplate(int fileId, int prefixId, Template template, Row row, int agentId, int agentTypeId, int productId, int request, int billingGroup, int userId)
        {
            try
            {
                MassiveValidationDAO massiveValidationDAO = new MassiveValidationDAO();
                return massiveValidationDAO.GetGeneralValidationsByEmissionTemplate(fileId, prefixId, template, row, agentId, agentTypeId, productId, request, billingGroup, userId);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExecuteValidations), ex);
            }
        }

        public List<ValidationPhoneType> GetPhoneTypesValidationsByEmissionTemplate(int fileId, int prefixId, Template template, Row row)
        {
            try
            {
                MassiveValidationDAO massiveValidationDAO = new MassiveValidationDAO();
                return massiveValidationDAO.GetPhoneTypesValidationsByEmissionTemplate(fileId, prefixId, template, row);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExecuteValidations), ex);
            }
        }

        public List<ValidationRegularExpression> GetRegularExpressionValidationsByEmisionTemplate(int fileId, Row row)
        {
            try
            {
                MassiveValidationDAO massiveValidationDAO = new MassiveValidationDAO();
                return massiveValidationDAO.GetRegularExpressionValidationsByEmisionTemplate(fileId, row);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExecuteValidations), ex);
            }
        }

        public List<ValidationIdentificator> GetGeneralValidationsByEmissionTempleteHolder(int fileId, int prefixId, Template template, Row row, List<ValidationIdentificator> validations)
        {
            try
            {
                MassiveValidationDAO massiveValidationDAO = new MassiveValidationDAO();
                return massiveValidationDAO.GetGeneralValidationsByEmissionTempleteHolder(fileId, prefixId, template, row, validations);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExecuteValidations), ex);
            }
        }

        public List<ValidationIdentificator> GetGeneralValidationsByEmissionTempleteInsured(int fileId, int prefixId, Template template, Row row, List<ValidationIdentificator> validations)
        {
            try
            {
                MassiveValidationDAO massiveValidationDAO = new MassiveValidationDAO();
                return massiveValidationDAO.GetGeneralValidationsByEmissionTempleteInsured(fileId, prefixId, template, row, validations);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExecuteValidations), ex);
            }
        }

        public List<ValidationIdentificator> GetGeneralValidationsByEmissionTempleteBeneficiary(int fileId, int prefixId, Template template, Row row, List<ValidationIdentificator> validations)
        {
            try
            {
                MassiveValidationDAO massiveValidationDAO = new MassiveValidationDAO();
                return massiveValidationDAO.GetGeneralValidationsByEmissionTempleteBeneficiary(fileId, prefixId, template, row, validations);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExecuteValidations), ex);
            }
        }

        public List<ValidationIdentificator> GetGeneralValidationsByEmissionTempleteCurrency(int fileId, int prefixId, Template template, Row row, List<ValidationIdentificator> validations)
        {
            try
            {
                MassiveValidationDAO massiveValidationsDAO = new MassiveValidationDAO();
                return massiveValidationsDAO.GetGeneralValidationsByEmissionTempleteCurrency(fileId, prefixId, template, row, validations);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExecuteValidations), ex);
            }
        }

        public List<ValidationIdentificator> GetValidationsByAdditionalBeneficiaries(int fileId, Template template)
        {
            try
            {
                MassiveValidationDAO massiveValidationsDAO = new MassiveValidationDAO();
                return massiveValidationsDAO.GetValidationsByAdditionalBeneficiaries(fileId, template);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExecuteValidations), ex);
            }
        }

        public List<ValidationPhoneType> GetPhoneTypeValidationsByAdditionalBeneficiaries(int fileId, Template template)
        {
            try
            {
                MassiveValidationDAO massiveValidationsDAO = new MassiveValidationDAO();
                return massiveValidationsDAO.GetPhoneTypeValidationsByAdditionalBeneficiaries(fileId, template);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExecuteValidations), ex);
            }
        }

        public List<ValidationRegularExpression> GetRegularExpressionValidationsByAdditionalBeneficiaries(int fileId, Template template)
        {
            try
            {
                MassiveValidationDAO massiveValidationsDAO = new MassiveValidationDAO();
                return massiveValidationsDAO.GetRegularExpressionValidationsByAdditionalBeneficiaries(fileId, template);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExecuteValidations), ex);
            }
        }

        public CompanyPolicy GetPolicyByOperationId(int policyOperationId)
        {
            try
            {
                return new MassiveLoadPolicyDAO().GetPolicyByOperationId(policyOperationId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyPolicy), ex);
            }
        }

        public List<PendingOperation> GetRiskPendingOperationsByPolicyOperationId(int policyOperationId)
        {
            try
            {
                return new MassiveLoadRiskDAO().GetRiskPendingOperationsByPolicyOperationId(policyOperationId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyRisks), ex);
            }
        }

         public List<ValidationIdentificator> GetValidationsClauseLevel(int fileId, int prefixId, Template template, int coveredRiskType)
        {
            try
            {
                MassiveValidationDAO massiveValidationsDAO = new MassiveValidationDAO();
                return massiveValidationsDAO.GetValidationsClauseLevel(fileId, template, prefixId, coveredRiskType);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExecuteValidations), ex);
            }
        }

        public List<CompanyPolicy> GetCompanyPoliciesToIssueByOperationIds(List<int> policiesOperationIds)
        {
            try
            {
                MassiveLoadPolicyDAO massiveLoadPolicyDAO = new MassiveLoadPolicyDAO();
                return massiveLoadPolicyDAO.GetCompanyPoliciesToIssueByOperationIds(policiesOperationIds);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyPolicies), ex);
            }
        }

        public CompanyPolicy GetCompanyPolicyToIssueByOperationId(int policyOperationId)
        {
            try
            {
                MassiveLoadPolicyDAO massiveLoadPolicyDAO = new MassiveLoadPolicyDAO();
                return massiveLoadPolicyDAO.GetCompanyPolicyToIssueByOperationId(policyOperationId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCompanyPolicy), ex);
            }
        }

        // <summary>
        // Genera el archivo de error del proceso de emisión masiva
        // </summary>
        // <param name = "massiveLoadId" ></ param >
        // < returns ></ returns >
        public string GenerateFileErrorsMassiveEmission(int massiveLoadId)
        {
            try
            {
                MassiveLoadRiskDAO massiveLoadRiskDAO = new MassiveLoadRiskDAO();
                return massiveLoadRiskDAO.GenerateFileErrorsMassiveEmission(massiveLoadId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGenerateFile), ex);
            }
        }

        // <summary>
        // Genera el archivo de error del proceso de cancelacion masiva
        // </summary>
        // <param name = "massiveLoadId" ></ param >
        // < returns ></ returns >
        public string GenerateFileErrorsMassiveCancellation(int massiveLoadId)
        {
            try
            {
                MassiveLoadRiskDAO massiveLoadRiskDAO = new MassiveLoadRiskDAO();
                return massiveLoadRiskDAO.GenerateFileErrorsMassiveCancellation(massiveLoadId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGenerateFile), ex);
            }
        }
    }
}