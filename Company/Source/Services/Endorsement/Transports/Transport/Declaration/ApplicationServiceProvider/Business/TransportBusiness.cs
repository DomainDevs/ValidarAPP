using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.Transports.Endorsement.Declaration.ApplicationServices.DTOs;
using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Company.Application.Transports.Endorsement.Declaration.ApplicationServices.EEProvider.Enums;
using Sistran.Company.Application.Transports.Endorsement.Declaration.ApplicationServices.EEProvider.Assemblers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.Transports.Endorsement.Declaration.ApplicationServices.EEProvider.Business
{
    public class TransportBusiness
    {
        public DeclarationDTO CreateDeclaration(DeclarationDTO declarationDTO)
        {
            try
            {
                return CreateTemporal(declarationDTO);
            }
            catch (Exception ex)
            {

                throw new BusinessException(ex.Message);
            }
        }

        public DeclarationDTO QuotateDeclaration(DeclarationDTO declarationDTO)
        {
            throw new NotImplementedException();
        }

        public DeclarationDTO GetPolicyByPrefixIdBranchIdPolicyNumber(int prefixId, int branchId, decimal policyNumber)
        {
            CompanyPolicy companyPolicy = DelegateService.underwritingService.GetCurrentCompanyPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, policyNumber);
            return DTOAssembler.CreatePolicy(companyPolicy);
        }

        public DeclarationDTO GetTransportsByPolicyId(int PolicyId, string currentFrom)
        {
            List<EndorsementDTO> lastEndorsements = new List<EndorsementDTO>();
            List<TransportDTO> transportDTOs = DelegateService.transportApplicationService.GetTransportsByPolicyId(PolicyId);
            List<EndorsementDTO> endorsements = DelegateService.transportApplicationService.GetEndorsementByEndorsementTypeIdPolicyId((int)EndorsementType.DeclarationEndorsement, PolicyId);
            if (endorsements.Count > 0)
            {
                endorsements = endorsements.OrderByDescending(f => f.IdEndorsement).ToList();
                lastEndorsements.Add(endorsements[0]);
            }
            else
            {
                endorsements = DelegateService.transportApplicationService.GetEndorsementByEndorsementTypeIdPolicyId((int)EndorsementType.DeclarationEndorsement, PolicyId);
                if (endorsements.Count > 0)
                {
                    endorsements = endorsements.OrderByDescending(f => f.IdEndorsement).ToList();
                    lastEndorsements.Add(endorsements[0]);
                }
            }
            int DeclararatioPeriod = (int)transportDTOs[0].DeclarationPeriodId;
            int BillingPeriodId = (int)transportDTOs[0].BillingPeriodId;
            DeclarationDTO declaration = DTOAssembler.CreateDeclarationDTO(transportDTOs);
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

            TransportBusiness transportBusiness = new TransportBusiness();
            declaration.Days = transportBusiness.CalculateDays(declaration.Endorsment);
            return declaration;
        }


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
            };
        }
        public DeclarationDTO GetCompanyEndorsementByEndorsementTypeIdPolicyId(int endorsementTypeId, int PolicyId)
        {
            List<EndorsementDTO> endorsements = DelegateService.transportApplicationService.GetEndorsementByEndorsementTypeIdPolicyId(endorsementTypeId, PolicyId);
            return DTOAssembler.CreateEndorsment(endorsements);
        }

        public DeclarationDTO GetTransportByRiskId(TransportDTO Risk)
        {
            List<TransportDTO> transportDTOs = DelegateService.transportApplicationService.GetTransportsByPolicyId(Risk.PolicyId);
            return DTOAssembler.CreateDeclarationDTOByRiskId(transportDTOs, Risk);
        }

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

        public DeclarationDTO CreateTemporal(DeclarationDTO declarationDTO)
        {
            DeclarationDTO policy = new DeclarationDTO();

            return policy = DTOAssembler.DeclarationDTO(
            DelegateService.DeclarationService.CreateEndorsementDeclaration(ModelAssembler.CreateCompanyPolicy(declarationDTO), ModelAssembler.CreateFormValues(declarationDTO)));

        }

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
            DeclarationDTO declaration = DTOAssembler.CreateDeclarationDTO(DelegateService.transportApplicationService.GetNextDeclarationEndorsementByPolicyId(policyId));
            declaration.Days = CalculateDays(declaration.CurrentFrom, declaration.CurrentTo);
            return declaration;
        }

        /// <summary>
        /// Crea el endoso de declaración
        /// </summary>
        /// <param name="companyPolicy">Póliza</param>
        /// <returns>Endoso de declaración</returns>
        public CompanyPolicy CreateEndorsementDeclaration(CompanyPolicy companyPolicy)
        {
            companyPolicy = DelegateService.DeclarationService.CreateEndorsementDeclaration(companyPolicy, new Dictionary<string, object>());
            return companyPolicy;
        }

    }
}
