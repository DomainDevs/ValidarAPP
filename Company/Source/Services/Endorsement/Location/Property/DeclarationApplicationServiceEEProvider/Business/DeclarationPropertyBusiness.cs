using Sistran.Company.Application.DeclarationApplicationService.DTO;
using Sistran.Company.Application.DeclarationApplicationServiceEEProvider.Assembler;
using Sistran.Company.Application.DeclarationApplicationServiceEEProvider.Enum;
using Sistran.Company.Application.Location.PropertyServices.DTO;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.DeclarationApplicationServiceEEProvider.Business
{
    public class DeclarationPropertyBusiness
    {
        /// <summary>
        /// 
        /// </summary>
        public DeclarationDTO QuotateDeclaration(DeclarationDTO declarationDTO)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        public DeclarationDTO GetPolicyByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber)
        {
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCurrentCompanyPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber);
            return DTOAssembler.CreatePolicy(companyPolicy);
        }

        /// <summary>
        /// 
        /// </summary>
        public DeclarationDTO GetRisksByPolicyIdCurrentFrom(int PolicyId, string currentFrom)
        {
            List<EndorsementDTO> lastEndorsements = new List<EndorsementDTO>();
            List<CompanyPropertyRisk> risks = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(PolicyId);
            List<EndorsementDTO> endorsements = DelegateService.propertyService.GetEndorsementByEndorsementTypeIdPolicyId((int)EndorsementType.DeclarationEndorsement, PolicyId);
            if (endorsements.Count > 0)
            {
                endorsements = endorsements.OrderByDescending(f => f.IdEndorsement).ToList();
                lastEndorsements.Add(endorsements[0]);
            }
            else
            {
                endorsements = DelegateService.propertyService.GetEndorsementByEndorsementTypeIdPolicyId((int)EndorsementType.DeclarationEndorsement, PolicyId);
                if (endorsements.Count > 0)
                {
                    endorsements = endorsements.OrderByDescending(f => f.IdEndorsement).ToList();
                    lastEndorsements.Add(endorsements[0]);
                }
            }
            int DeclararatioPeriod = (int)risks[0].DeclarationPeriod.Id;
            int BillingPeriodId = (int)risks[0].BillingPeriodDepositPremium;
            DeclarationDTO declaration = DTOAssembler.CreateDeclarationDTO(risks);
            if (lastEndorsements.Count > 0)
            {
                declaration.Endorsment = CalculateSegmentByDeclarationPeriodId(DeclararatioPeriod, lastEndorsements[0]);
            }
            else
            {
                EndorsementDTO endorsement = new EndorsementDTO();
                endorsement.CurrentFrom = Convert.ToDateTime(currentFrom);
                declaration.Endorsment = CalculateSegmentByDeclarationPeriodId(DeclararatioPeriod, endorsement);
            }

            DeclarationPropertyBusiness declararionPropertyBusiness = new DeclarationPropertyBusiness();
            declaration.Days = declararionPropertyBusiness.CalculateDays(declaration.Endorsment);
            return declaration;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<InsuredObjectDTO> GetInsuredObjectsByRiskId(int riskId)
        {
            return CreateInsuredObjects(DelegateService.underwritingService.GetInsuredObjectsByRiskId(riskId));
        }

        /// <summary>
        /// Crea una lista de InsuredObjectDTO a partir de un modelo InsuredObject
        /// </summary>
        /// <param name="insuredObject">Modelo insuredObject</param>
        /// <returns>listado Objeto InsuredObjectDTO List<InsuredObjectDTO></returns>
        public static List<InsuredObjectDTO> CreateInsuredObjects(List<InsuredObject> InsuredObjects)
        {
            List<InsuredObjectDTO> insuredObjectDTO = new List<InsuredObjectDTO>();
            if (InsuredObjects != null)
            {
                foreach (var insuredObject in InsuredObjects)
                {
                    insuredObjectDTO.Add(CreateInsuredObject(insuredObject));
                }
            }
            return insuredObjectDTO;
        }

        /// <summary>
        /// Crea un objeto InsuredObjectDTO a partir de un modelo InsuredObject
        /// </summary>
        /// <param name="insuredObject">Modelo insuredObject</param>
        /// <returns>Objeto InsuredObjectDTO</returns>
        public static InsuredObjectDTO CreateInsuredObject(InsuredObject InsuredObject)
        {
            if (InsuredObject == null)
            {
                return null;
            }
            return new InsuredObjectDTO
            {
                Id = InsuredObject.Id,
                Description = InsuredObject.Description,
                InsuredLimitAmount = InsuredObject.Amount,
                PremiumAmount = InsuredObject.Premium,
                IsSelected = InsuredObject.IsSelected,
                IsMandatory = InsuredObject.IsMandatory,
                IsDeclarative = InsuredObject.IsDeclarative
            };
        }

        public bool ValidateDeclarativeInsuredObjects(decimal policyId)
        {
            return DelegateService.declarationBusinessService.ValidateDeclarativeInsuredObjects(policyId);
        }

        /// <summary>
        /// 
        /// </summary>
        public DeclarationDTO GetCompanyEndorsementByEndorsementTypeIdPolicyId(int endorsementTypeId, int PolicyId)
        {
            List<EndorsementDTO> endorsements = DelegateService.propertyService.GetEndorsementByEndorsementTypeIdPolicyId(endorsementTypeId, PolicyId);
            return DTOAssembler.CreateEndorsment(endorsements);
        }

        /// <summary>
        /// 
        /// </summary>
        public DeclarationDTO GetRiskByRiskId(CompanyPropertyRisk Risk)
        {
            List<CompanyPropertyRisk> risks = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(Risk.Risk.Policy.Id);
            return DTOAssembler.CreateDeclarationDTOByRiskId(risks, Risk);
        }

        /// <summary>
        /// 
        /// </summary>
        public int CalculateDays(EndorsementDTO endorsment)
        {
            TimeSpan timeSpan = endorsment.CurrentTo - endorsment.CurrentFrom;
            int days = timeSpan.Days;
            return days;
        }

        /// <summary>
        /// Calcula la cantidad de días entre dos fechas
        /// </summary>
        /// <param name="from">Fecha de inicio</param>
        /// <param name="to">Fecha fin</param>
        /// <returns>Cantidad de días entre dos fechas</returns>
        public int CalculateDays(DateTime from, DateTime to)
        {
            TimeSpan timeSpan = to - from;
            int days = timeSpan.Days;
            return days;
        }

        /// <summary>
        /// 
        /// </summary>
        public DeclarationDTO CreateTemporal(DeclarationDTO declarationDTO)
        {
            try
            {
                DeclarationDTO policy = new DeclarationDTO();
                CompanyPolicy companyPolicy = ModelAssembler.CreateCompanyPolicy(declarationDTO);
                Dictionary<string, object> formValue = ModelAssembler.CreateFormValues(declarationDTO);
                companyPolicy = DelegateService.declarationBusinessService.CreateEndorsementDeclaration(companyPolicy, formValue);
                return policy = DTOAssembler.DeclarationDTO(companyPolicy);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public EndorsementDTO CalculateSegmentByDeclarationPeriodId(int declaratioPeriod, EndorsementDTO endorsmentDTO)
        {
            if (declaratioPeriod == (int)DeclarationPeriod.Monthly)
            {
                endorsmentDTO.CurrentTo = endorsmentDTO.CurrentFrom.AddMonths(1);
            }
            if (declaratioPeriod == (int)DeclarationPeriod.Bimonthly)
            {
                endorsmentDTO.CurrentTo = endorsmentDTO.CurrentFrom.AddMonths(2);
            }
            if (declaratioPeriod == (int)DeclarationPeriod.Quaterly)
            {
                endorsmentDTO.CurrentTo = endorsmentDTO.CurrentFrom.AddMonths(3);
            }
            if (declaratioPeriod == (int)DeclarationPeriod.FourtQuater)
            {
                endorsmentDTO.CurrentTo = endorsmentDTO.CurrentFrom.AddMonths(4);
            }
            if (declaratioPeriod == (int)DeclarationPeriod.Biannual)
            {
                endorsmentDTO.CurrentTo = endorsmentDTO.CurrentFrom.AddMonths(6);
            }
            if (declaratioPeriod == (int)DeclarationPeriod.Annual)
            {
                endorsmentDTO.CurrentTo = endorsmentDTO.CurrentFrom.AddMonths(12);
            }
            return endorsmentDTO;
        }

        /// <summary>
        /// 
        /// </summary>
        public DeclarationDTO GetTemporalById(int temporalId, bool isMasive)
        {
            return DTOAssembler.DeclarationDTO(DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, isMasive));
        }

        /// <summary>
        /// Genera el próximo endoso de declaración para una póliza
        /// </summary>
        /// <param name="policyId">Identificador de la póliza</param>
        /// <returns>Endoso de declaración</returns>
        public DeclarationDTO GetDeclarationEndorsementByPolicyId(int policyId)
        {
            DeclarationDTO declaration = DTOAssembler.CreateDeclarationDTO(DelegateService.propertyService.GetNextDeclarationEndorsementByPolicyId(policyId));
            declaration.Days = CalculateDays(declaration.CurrentFrom, declaration.CurrentTo);
            return declaration;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<CompanyPropertyRisk> GetRisksByPolicyId(int policyId)
        {
            List<CompanyPropertyRisk> companyPropertyRisks = DelegateService.propertyService.GetCompanyPropertiesByPolicyId(policyId);
            return companyPropertyRisks;
        }
    }
}
