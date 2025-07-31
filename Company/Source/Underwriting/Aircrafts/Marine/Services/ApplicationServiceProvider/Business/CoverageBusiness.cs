using Sistran.Company.Application.Marines.MarineApplicationService.DTOs;
using Sistran.Company.Application.Marines.MarineApplicationService.EEProvider.Assemblers;
using Sistran.Company.Application.Marines.MarineBusinessService.Models.Base;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.Collections.Generic;

namespace Sistran.Company.Application.Marines.MarineApplicationService.EEProvider.Business
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
        public CoverageDTO QuotateCoverage(CoverageDTO coverageDto, MarineDTO marineDTO, bool runRulesPre, bool runRulesPost)
        {
            CompanyCoverage companyCoverage = ModelAssembler.CreateCoverage(coverageDto);
            return DTOAssembler.CreateCoverage(DelegateService.marineBusinessService.QuotateCompanyCoverage(ModelAssembler.CreateMarine(marineDTO),companyCoverage, runRulesPre, runRulesPost));
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
            return DTOAssembler.CreateCoverages(DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectIdGroupCoverageIdProductId(insuredObjectId, groupCoverageId, productId));
        }
        
        /// <summary>
        /// Calcula riesgo del Marinee
        /// </summary>
        /// <param name="marineDTO">Marine</param>
        /// <param name="runRulesPre">reglas pre</param>
        /// <param name="runRulesPost">reglas pos</param>
        /// <returns>MarineDTO</returns>
        public MarineDTO QuotateMarine(MarineDTO marineDTO, bool runRulesPre, bool runRulesPost)
        {
            return DTOAssembler.CreateMarine(DelegateService.marineBusinessService.QuotateCompanyMarine(ModelAssembler.CreateMarine(marineDTO), runRulesPre, runRulesPost));
        }

        public CoverageDTO ExcludeCoverage(int temporalId, int riskId, int coverageId)
        {
            return DTOAssembler.CreateCoverage(DelegateService.marineBusinessService.ExcludeCompanyCoverage(temporalId, riskId, coverageId));
        }

        /// <summary>
        /// Retorna el listado de coverturas por Objeto del seguro .
        /// </summary>
        /// <param name="insuredObjectId">Identificador del Objeto del seguro</param>
        /// <returns>Lista de Cobreturas</returns>
        public List<CoverageDTO> GetCoveragesByInsuredObjectId(int insuredObjectId)
        {
            List<CompanyCoverage> coverages = DelegateService.underwritingService.GetCompanyCoveragesByInsuredObjectId(insuredObjectId);
            return DTOAssembler.CreateCoverages(coverages);
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