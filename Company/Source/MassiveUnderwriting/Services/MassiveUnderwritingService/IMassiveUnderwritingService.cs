using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.MassiveServices.Enums;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Company.Application.MassiveServices.Models;

namespace Sistran.Company.Application.MassiveUnderwritingServices
{
    [ServiceContract]
    public interface IMassiveUnderwritingService : IMassiveUnderwritingServiceCore
    {
        [OperationContract]
        GroupCoverage CreateGroupCoverage(Row row, int productId);

        [OperationContract]
        RatingZone CreateRatingZone(Row row, int prefixId);

        [OperationContract]
        CompanyPolicy CreateCompanyPolicy(MassiveEmission massiveLoad, MassiveEmissionRow massiveEmissionRow, File file, string templatePropertyName, List<FilterIndividual> filtersIndividuals);


        [OperationContract]
        MassiveLoad IssuanceMassiveEmission(int massiveLoadId);

        [OperationContract]
        LimitRc CreateLimitRc(Row row, int prefixId, int productId, int policyTypeId);

        /// <summary>
        /// Generar Reporte masivo
        /// </summary>
        /// <param name="massiveLoadId"></param>
        /// <param name="rangeFrom"></param>
        /// <param name="rangeTo"></param>
        /// <returns></returns>
        [OperationContract]
        string PrintMassiveLoad(int massiveLoadId, int rangeFrom, int rangeTo, User user, bool checkIssuedDetail);
        
        [OperationContract]
        List<ValidationIdentificator> GetPersonValidationsByEmissionTemplate(int fileId, int prefixId, Template template, Row row);

        [OperationContract]
        List<ValidationIdentificator> GetGeneralValidationsByEmissionTemplate(int fileId, int prefixId, Template template, Row row, int agentId, int agentTypeId, int productId, int request, int billingGroup, int userId);

        [OperationContract]
        List<ValidationPhoneType> GetPhoneTypesValidationsByEmissionTemplate(int fileId, int prefixId, Template template, Row row);

        [OperationContract]
        List<ValidationRegularExpression> GetRegularExpressionValidationsByEmisionTemplate(int fileId, Row row);

        [OperationContract]
        List<ValidationIdentificator> GetGeneralValidationsByEmissionTempleteHolder(int fileId, int prefixId, Template template, Row row, List<ValidationIdentificator> validations);

        [OperationContract]
        List<ValidationIdentificator> GetGeneralValidationsByEmissionTempleteInsured(int fileId, int prefixId, Template template, Row row, List<ValidationIdentificator> validations);

        [OperationContract]
        List<ValidationIdentificator> GetGeneralValidationsByEmissionTempleteBeneficiary(int fileId, int prefixId, Template template, Row row, List<ValidationIdentificator> validations);

        [OperationContract]
        List<ValidationIdentificator> GetGeneralValidationsByEmissionTempleteCurrency(int fileId, int prefixId, Template template, Row row, List<ValidationIdentificator> validations);

        [OperationContract]
        List<ValidationIdentificator> GetValidationsByAdditionalBeneficiaries(int fileId, Template template);

        [OperationContract]
        List<ValidationPhoneType> GetPhoneTypeValidationsByAdditionalBeneficiaries(int fileId, Template template);

        [OperationContract]
        List<ValidationRegularExpression> GetRegularExpressionValidationsByAdditionalBeneficiaries(int fileId, Template template);

        [OperationContract]
        CompanyPolicy GetPolicyByOperationId(int policyOperationId);

        [OperationContract]
        List<ValidationIdentificator> GetValidationsClauseLevel(int fileId, int prefixId, Template template, int coveredRiskType);

        [OperationContract]
        List<CompanyPolicy> GetCompanyPoliciesToIssueByOperationIds(List<int> policiesOperationIds);

        [OperationContract]
        CompanyPolicy GetCompanyPolicyToIssueByOperationId(int policyOperationId);

        /// <summary>
        /// Genera el archivo de error del proceso de emisión masiva
        /// </summary>
        /// <param name="massiveLoadId"></param>
        /// <returns></returns>
        [OperationContract]
        string GenerateFileErrorsMassiveEmission(int massiveLoadId);

        /// <summary>
        /// Genera el archivo de error del proceso de cancelacion masiva
        /// </summary>
        /// <param name="massiveLoadId"></param>
        /// <returns></returns>
        [OperationContract]
        string GenerateFileErrorsMassiveCancellation(int massiveLoadId);
    }

}