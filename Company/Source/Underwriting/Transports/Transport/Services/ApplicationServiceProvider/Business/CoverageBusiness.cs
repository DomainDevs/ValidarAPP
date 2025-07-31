using Sistran.Company.Application.Transports.TransportApplicationService.DTOs;
using Sistran.Company.Application.Transports.TransportApplicationService.EEProvider.Assemblers;
using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.Collections.Generic;

namespace Sistran.Company.Application.Transports.TransportApplicationService.EEProvider.Business
{
    public class CoverageBusiness
    {
        public CoverageBusiness()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coverageDTO"></param>
        /// <returns></returns>
        public CoverageDTO QuotateCoverage(CoverageDTO coverageDto, TransportDTO transportDTO, bool runRulesPre, bool runRulesPost)
        {
            CompanyCoverage companyCoverage = ModelAssembler.CreateCompanyCoverage(coverageDto);
            CompanyTransport companyTransport = ModelAssembler.CreateTransport(transportDTO, new CompanyTransport());
            companyCoverage = DelegateService.transportBusinessService.QuotateCompanyCoverage(companyTransport, companyCoverage, runRulesPre, runRulesPost);
            coverageDto = DTOAssembler.CreateCoverageDTO(companyCoverage);
            return coverageDto;
        }
        
        /// <summary>
        /// Retorna un listado de coberturas
        /// </summary>
        /// <param name="insuredObjectId">Identificador del objeto del seguro</param>
        /// <param name="groupCoverageId">Identificador del grupo de cobertura</param>
        /// <param name="productId">Identificador del producto</param>
        /// <returns>Listado de coberturas</returns>
        public List<CoverageDTO> GetCoveragesByProductIdGroupCoverageIdInsuredObjectId(int productId, int groupCoverageId, int insuredObjectId)
        {
            return DTOAssembler.CreateCoveragesDtos(DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(insuredObjectId, groupCoverageId, productId));
        }
        
        /// <summary>
        /// Calcula riesgo del transporte
        /// </summary>
        /// <param name="TransportDTO">Transport</param>
        /// <param name="runRulesPre">reglas pre</param>
        /// <param name="runRulesPost">reglas pos</param>
        /// <returns>transportDTO</returns>
        public TransportDTO QuotateTransport(TransportDTO transportDTO, bool runRulesPre, bool runRulesPost)
        {
            return DTOAssembler.CreateTransport(DelegateService.transportBusinessService.QuotateCompanyTransport(ModelAssembler.CreateTransport(transportDTO, new CompanyTransport()), runRulesPre, runRulesPost));
        }

        public CoverageDTO ExcludeCoverage(int temporalId, int riskId, int coverageId)
        {
            return DTOAssembler.CreateCoverageDTO(DelegateService.transportBusinessService.ExcludeCompanyCoverage(temporalId, riskId, coverageId));
        }

        /// <summary>
        /// Retorna el listado de coverturas por Objeto del seguro .
        /// </summary>
        /// <param name="insuredObjectId">Identificador del Objeto del seguro</param>
        /// <returns>Lista de Cobreturas</returns>
        public List<CoverageDTO> GetCoveragesByInsuredObjectId(int insuredObjectId)
        {
            List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectId(insuredObjectId);
            return DTOAssembler.CreateCoveragesDtos(coverages);
        }

        /// <summary>
        /// Return un listado de objetos de seguro filtrados por identificador de
        ///     producto y grupo de cobertura
        /// </summary>
        /// <param name="productId">Identificador del producto</param>
        /// <param name="groupCoverageId">Identificador del grupo de cobertura</param>
        /// <param name="prefixId">Identificador del ramo</param>
        /// <returns>Listado de objetos de seguro</returns>
        public List<InsuredObjectDTO> GetInsuredObjectsByProductIdGroupCoverageId(int productId, int groupCoverageId, int prefixId)
        {
            return DTOAssembler.CreateInsuredObjects(DelegateService.underwritingService.GetInsuredObjectsByProductIdGroupCoverageId(productId, groupCoverageId, prefixId));
        }

        /// <summary>
        /// Retorna el listado de tipos de tasa
        /// </summary>
        /// <returns>Listado de tipos de tasa</returns>
        public List<SelectObjectDTO> GetRateTypes()
        {
            return DTOAssembler.CreateRateTypes();
        }

        /// <summary>
        /// Retorna el listado de tipos de cálculo
        /// </summary>
        /// <returns>Listado de identificador descripción</returns>
        public List<SelectObjectDTO> GetCalculationTypes()
        {
            return DTOAssembler.CreateCalculationTypes();
        }

        /// <summary>
        /// Obtiene le listado de deducibles por identificador de coberturas
        /// </summary>
        /// <param name="coverageId">Identificador de cobertura</param>
        /// <returns>Listado de deducibles</returns>
        public List<DeductibleDTO> GetDeductiblesByCoverageId(int coverageId)
        {
            return DTOAssembler.CreateDeductibles(DelegateService.underwritingService.GetDeductiblesByCoverageId(coverageId));
        }
    }
}