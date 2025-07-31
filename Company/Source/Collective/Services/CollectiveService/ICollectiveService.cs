using System.ServiceModel;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Collections.Generic;
using Sistran.Core.Application.CollectiveServices;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.CollectiveServices.Models;
using Sistran.Core.Application.CollectiveServices.Models;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Company.Application.MassiveServices.Models;

namespace Sistran.Company.Application.CollectiveServices
{
    [ServiceContract]
    public interface ICollectiveService : ICollectiveServiceCore
    {
        [OperationContract]
        CompanyPolicy CreateCompanyPolicy(CollectiveEmission collectiveLoad, File file, string templatePropertyName, List<FilterIndividual> filtersIndividuals, int riskCount);

        [OperationContract]
        GroupCoverage CreateGroupCoverage(Row row, int productId);

        [OperationContract]
        RatingZone CreateRatingZone(Row row, int prefixId);

        [OperationContract]
        List<CompanyCoverage> CreateCoverages(int productId, int groupCoverageId, int prefixId);

        [OperationContract]
        LimitRc CreateLimitRc(Row row, int prefixId, int productId, int policyTypeId);

        [OperationContract]
        IssuedCollectiveLoad CreateIssuedPolicy(List<int> collectiveLoadIds, int tempId);

        [OperationContract]
        MassiveLoad IssuanceCollectiveEmission(int massiveLoadId);

        [OperationContract]
        MassiveLoad IssuanceCollectiveEndorsement(int massiveLoadId);

        [OperationContract]
        string PrintCollectiveLoad(int massiveLoadId, int rangeFrom, int rangeTo, User user, bool checkIssuedDetail);

        [OperationContract]
        void ExcludeCompanyCollectiveEmissionRowsTemporals(int massiveLoadId, List<int> temps, string userName, bool deleteTemporal);

        [OperationContract]
        List<ValidationIdentificator> GetGeneralValidationsByEmissionTemplate(File file, Row row, int userId);

        [OperationContract]
        List<ValidationPhoneType> GetPhoneTypesValidationsByEmissionTemplate(File file, Row row);

        [OperationContract]
        List<ValidationIdentificator> GetGeneralValidationsByRiskTemplate(int fileId, Row row);

        [OperationContract]
        List<ValidationPhoneType> GetPhoneTypesValidationsByRiskDetailTemplate(int fileId, Template template);

        [OperationContract]
        List<ValidationIdentificator> GetValidationsByAdditionalBeneficiariesTemplate(File file, Template template);

        [OperationContract]
        List<ValidationPhoneType> GetPhoneTypeValidationsByAdditionalBeneficiaries(int fileId, Template template);

        [OperationContract]
        List<ValidationRegularExpression> GetRegularExpressionValidationsByEmisionTemplate(int fileId, Row row);

        [OperationContract]
        List<ValidationRegularExpression> GetRegularExpressionValidationsByRiskTemplate(int fileId, Row row);

        [OperationContract]
        List<ValidationRegularExpression> GetRegularExpressionValidationsByAdditionalBeneficiaries(int fileId, Template template);
    }
}