using Sistran.Company.Application.Transports.Endorsement.Adjustment.ApplicationServices.DTOs;
using Sistran.Company.Application.Transports.Endorsement.Adjustment.ApplicationServices.EEProvider.Assemblers;
using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.Framework.BAF;
using Sistran.Company.Application.UnderwritingServices;
using AutoMapper;
using Sistran.Core.Application.UnderwritingServices.Models;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Transports.Endorsement.Adjustment.ApplicationServices.EEProvider.Business
{
    public class AdjustmentTransportBusiness
    {
        public List<CompanyRisk> CreateAdjustment(List<TransportDTO> companyTransports)
        {

            List<CompanyRisk> Risks = new List<CompanyRisk>();



            return Risks;
        }

        public AdjustmentDTO CreateTemporal(AdjustmentDTO adjustmentDTO)
        {
            AdjustmentDTO policy = new AdjustmentDTO();
            //si el valor de las declaraciones es mayor a la Prima en Deposito se cobrara con la adicional; 
            //pero si la sumatoria de las declaraciones es menor a la Prima en Deposito, el valor del ajuste será la Prima en Deposito
            //int IdendorsementAdjusment=0;

            //List<EndorsementDTO> EndorsementsAdjuement = DelegateService.transportsService.GetEndorsementByEndorsementTypeIdPolicyId((int)EndorsementType.AdjustmentEndorsement, adjustmentDTO.PolicyId);
            //if (EndorsementsAdjuement.Count > 0)
            //{
            //    EndorsementsAdjuement = EndorsementsAdjuement.OrderByDescending(f => f.IdEndorsement).ToList();
            //    IdendorsementAdjusment = EndorsementsAdjuement[0].IdEndorsement;
            //}
            //List<EndorsementDTO> EndorsementsDeclaration = DelegateService.transportsService.GetEndorsementByEndorsementTypeIdPolicyId((int)EndorsementType.DeclarationEndorsement, adjustmentDTO.PolicyId);
            //if (EndorsementsDeclaration.Count > 0)
            //{
            //    decimal FullPremiumAmountDeclaration=0;
            //    EndorsementsDeclaration = EndorsementsDeclaration.Where(x => x.IdEndorsement > IdendorsementAdjusment).ToList();
            //    foreach (var endorsementdeclaration in EndorsementsDeclaration)
            //    {
            //        List<CompanyCoverage> companyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(adjustmentDTO.PolicyId, endorsementdeclaration.IdEndorsement, adjustmentDTO.RiskId);
            //        FullPremiumAmountDeclaration = FullPremiumAmountDeclaration + companyCoverages[0].PremiumAmount;
            //    }
            //    if (FullPremiumAmountDeclaration > 0)
            //    { }
            //}

            return policy = DTOAssembler.CreateAjusmentsumary(
            DelegateService.adjustmentService.CreateEndorsementAdjustment(
            ModelAssembler.CreateCompanyPolicy(adjustmentDTO), ModelAssembler.CreateFormValues(adjustmentDTO)));


        }

        public AdjustmentDTO QuotateAdjustment(AdjustmentDTO adjustmentDTO)
        {
            throw new NotImplementedException();
        }

        public AdjustmentDTO GetTransportsByPolicyId(int policyId, string currentFrom)
        {
            AdjustmentDTO AdjustmentEndorsement = new AdjustmentDTO();
            // list riesgos 
            List<TransportDTO> transportDTO = DelegateService.transportsService.GetTransportsByPolicyId(policyId);
            int declarationPeriodId = (int)transportDTO[0].DeclarationPeriodId;
            int billingPeriodId = (int)transportDTO[0].BillingPeriodId;
            string declarationDescription = transportDTO[0].DeclarationDescription;
            string billingDescription = transportDTO[0].BillingDescription;
            List<EndorsementDTO> endorsements = new List<EndorsementDTO>();
            List<EndorsementDTO> endorsementsDaclaration = new List<EndorsementDTO>();
            DateTime currentfrom = DateTime.Parse(currentFrom);
            List<EndorsementDTO> Endorsements = DelegateService.transportsService.GetEndorsementByEndorsementTypeIdPolicyId((int)EndorsementType.AdjustmentEndorsement, policyId);
            if (Endorsements.Count > 0)
            {
                Endorsements = Endorsements.OrderByDescending(f => f.IdEndorsement).ToList();
                endorsements.Add(Endorsements[0]);
            }
            endorsementsDaclaration = DelegateService.transportsService.GetEndorsementByEndorsementTypeIdPolicyId((int)EndorsementType.DeclarationEndorsement, policyId);
            if (endorsementsDaclaration.Count > 0)
            {
                int quatityDeclaration = endorsementsDaclaration.Count;
                int Moths = currentfrom.Month - DateTime.Today.Month;
                if (quatityDeclaration > 0)
                {
                    if (endorsements.Count > 0)
                    {
                        AdjustmentEndorsement = CalculateDays(endorsements[0].CurrentFrom.ToString(), billingPeriodId);
                    }
                    else
                    {
                        AdjustmentEndorsement = CalculateDays(currentFrom, billingPeriodId);
                    }
                }
                AdjustmentEndorsement.Transports = DTOAssembler.CreateAjusmentTransports(transportDTO);
            }

            return AdjustmentEndorsement;

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

        public AdjustmentDTO CalculateDays(string CurrentFrom, int BillingPeriodId)
        {
            AdjustmentDTO adjusment = new AdjustmentDTO();
            DateTime currentFrom = Convert.ToDateTime(CurrentFrom);
            DateTime currentTo = new DateTime();

            if (BillingPeriodId == 6)
            {
                currentTo = currentFrom.AddMonths(12);
            }
            if (BillingPeriodId == 5)
            {
                currentTo = currentFrom.AddMonths(6);
            }
            if (BillingPeriodId == 4)
            {
                currentTo = currentFrom.AddMonths(4);
            }
            if (BillingPeriodId == 3)
            {
                currentTo = currentFrom.AddMonths(3);
            }
            if (BillingPeriodId == 2)
            {
                currentTo = currentFrom.AddMonths(2);
            }
            if (BillingPeriodId == 1)
            {
                currentTo = currentFrom.AddMonths(1);
            }
            TimeSpan timeSpan = currentTo - currentFrom;
            adjusment.Days = timeSpan.Days;
            adjusment.CurrentTo = Convert.ToDateTime(currentTo);
            adjusment.CurrentFrom = Convert.ToDateTime(currentFrom);
            return adjusment;
        }
        public AdjustmentDetailsDTO GetEndorsementByEndorsementTypeDeclarationPolicyId(int policyId, int riskId)
        {
            AdjustmentDetailsDTO details = new AdjustmentDetailsDTO();
            List<DeclarationDTO> detailsDeclaration = new List<DeclarationDTO>();
            List<EndorsementDTO> EndorsementsDeclaration = DelegateService.transportsService.GetEndorsementByEndorsementTypeIdPolicyId((int)EndorsementType.DeclarationEndorsement, policyId);
            foreach (var Endorsement in EndorsementsDeclaration)
            {
                List<CompanyCoverage> companyCoverages = DelegateService.underwritingService.GetCompanyCoveragesByPolicyIdEndorsementIdRiskId(policyId, Endorsement.IdEndorsement, (int)Endorsement.RiskId);
                details.PremiumDepositAmount = companyCoverages[0].PremiumAmount;
                details.HasPremiumDeposit = companyCoverages[0].IsMinPremiumDeposit;
                DeclarationDTO Declaration = new DeclarationDTO();
                Declaration.DeclaredAmount = companyCoverages[0].DeclaredAmount;
                Declaration.Description = companyCoverages[0].EndorsementType.ToString();
                detailsDeclaration.Add(Declaration);
            }
            details.DeclarationEndorsements = detailsDeclaration;
            return details;
        }

        public EndorsementDTO GetTemporalEndorsementByPolicyId(int policyId)
        {
            EndorsementDTO endorsement = DelegateService.transportsService.GetTemporalEndorsementByPolicyId(policyId);
            return endorsement;
        }

        public AdjustmentDTO GetTransportsByTemporalId(int temporalId, bool isMasive)
        {
            return DTOAssembler.CreateAjusmentsumary(DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, isMasive));
        }

        public AdjustmentDTO GetAdjustmentEndorsementByPolicyId(int policyId)
        {
            AdjustmentDTO adjustment = DTOAssembler.CreateAdjustmentDTO(DelegateService.transportsService.GetNextAdjustmentEndorsementByPolicyId(policyId));
            adjustment.Days = CalculateDays(adjustment.CurrentFrom, adjustment.CurrentTo);
            return adjustment;
        }

        public int CalculateDays(DateTime from, DateTime to)
        {
            TimeSpan timeSpan = to - from;
            int days = timeSpan.Days;
            return days;

        }
    }
}
