using Sistran.Core.Application.Marines.MarineBusinessService.EEProvider.Assemblers;
using Sistran.Core.Application.Marines.MarineBusinessService.EEProvider.Resources;
using Sistran.Core.Application.Marines.MarineBusinessService.EEProvider.Views;
using Sistran.Core.Application.Marines.MarineBusinessService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Core.Application.Marines.MarineBusinessService.EEProvider
{
    public class MarineBusinessServiceProvider : IMarineBusinessService
    {
        #region Marine
        #region GetMarineTypes        
        /// <summary>
        /// Obtiener la lista de tipos de mercancias
        /// </summary>
        /// <returns>Lista de Mercancias</returns>
        public List<MarineType> GetMarineTypes(int prefixid)
        {
            try
            {
                return ModelAssembler.CreateMarineTypes(DataFacadeManager.GetObjects(typeof(COMMEN.AircraftType)));
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetMarineTypes), ex);
            }
        }



        #endregion
        #region GetMakes
        public List<Make> GetMakes()
        {
            try
            {
                return ModelAssembler.CreateMakes(DataFacadeManager.GetObjects(typeof(COMMEN.AircraftMake)));
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetMake), ex);
            }
        }
        #endregion
        #region GetModelByMakeId
        public List<Model> GetModelByMakeId(int makeId)
        {
            try
            {
                return ModelAssembler.CreateModelByMakeIds(DataFacadeManager.GetObjects(typeof(COMMEN.AircraftModel)));
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetModelByMakeId), ex);
            }
        }
        #endregion
        #region GetOperators
        public List<Operator> GetOperators()
        {
            try
            {
                return ModelAssembler.CreateOperators(DataFacadeManager.GetObjects(typeof(COMMEN.AircraftOperator)));
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetOperators), ex);
            }
        }
        #endregion
        #region GetRegisters
        public List<Register> GetRegisters()
        {
            try
            {
                return ModelAssembler.CreateRegisters(DataFacadeManager.GetObjects(typeof(COMMEN.AircraftRegister)));
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRegisters), ex);
            }
        }
        #endregion
        # region GetType
        public List<MarineType> GetType(int prefixId)
        {
            try
            {
                return ModelAssembler.CreateTypes(DataFacadeManager.GetObjects(typeof(COMMEN.AircraftType)));
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetType), ex);
            }
        }
        #endregion
        # region GetTypesByPrefixId
        public List<MarineTypePrefix> GetTypesByPrefixId()
        {
            try
            {
                return ModelAssembler.CreateTypesByPrefixIds(DataFacadeManager.GetObjects(typeof(COMMEN.AircraftTypePrefix)));
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetTypesByPrefixId), ex);
            }
        }
        public List<Use> GetUseByusessByPrefixId(int prefixId)
        {
            try
            {

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(COMMEN.AircraftUsePrefix.Properties.PrefixCode, typeof(COMMEN.AircraftUsePrefix).Name);
                filter.Equal();
                filter.Constant(prefixId);

                MarinesUsePrefixView usePrefixView = new MarinesUsePrefixView();
                ViewBuilder builder = new ViewBuilder("UsePrefixView");
                builder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, usePrefixView);

                if (usePrefixView.AircraftUsePrefixs.Count > 0)
                {
                    return ModelAssembler.CreateUses(usePrefixView.AircraftUses);
                }
                return new List<Use>();
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetUsesByPrefixId), ex);
            }
        }
        #endregion
        #region GetUses
        public List<Use> GetUses(int prefixId)
        {
            try
            {
                return ModelAssembler.CreateUses(DataFacadeManager.GetObjects(typeof(COMMEN.AircraftUse)));
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetUses), ex);
            }
        }
        public List<UsePrefix> GetUsesByPrefixId(int prefixId)
        {
            try
            {
                return ModelAssembler.CreateUsesByPrefixIds(DataFacadeManager.GetObjects(typeof(COMMEN.AircraftUsePrefix)));
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetUsesByPrefixId), ex);
            }
        }
        #endregion

        #endregion

        public List<Marine> GetMarinesByEndorsementIdModuleType(int endorsementId)
        {
            List<Marine> marines = new List<Marine>();
            marines = GetClaimCompanyMarinesByEndorsementId(endorsementId);
            return marines;
        }    
              

        public List<Marine> GetClaimCompanyMarinesByEndorsementId(int endorsementId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(endorsementId);

            List<Marine> companyMarines = new List<Marine>();
            ClaimRiskMarineView claimRiskMarineView = new ClaimRiskMarineView();
            ViewBuilder viewBuilder = new ViewBuilder("ClaimRiskMarineView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimRiskMarineView);

            if (claimRiskMarineView.RiskAircrafts.Count > 0)
            {
                foreach (ISSEN.RiskAircraft entityRiskAircraft in claimRiskMarineView.RiskAircrafts)
                {
                    ISSEN.Risk entityRisk = claimRiskMarineView.Risks.Cast<ISSEN.Risk>().First(x => x.RiskId == entityRiskAircraft.RiskId);
                    ISSEN.EndorsementRisk entityEndorsementRisk = claimRiskMarineView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().First(x => x.RiskId == entityRiskAircraft.RiskId);
                    ISSEN.Policy entityPolicy = claimRiskMarineView.Policies.Cast<ISSEN.Policy>().First(x => x.PolicyId == entityEndorsementRisk.PolicyId);
                    COMMEN.AircraftUse entityAircraftUse = claimRiskMarineView.RiskAircraftUses.Cast<COMMEN.AircraftUse>().FirstOrDefault(x => x.AircraftUseCode == entityRiskAircraft.AircraftUseCode);

                    Marine companyMarine = ModelAssembler.CreateMarine(entityRisk, entityRiskAircraft, entityEndorsementRisk);

                    companyMarine.UseId = Convert.ToInt32(entityRiskAircraft.AircraftUseCode);
                    companyMarine.UseDescription = entityAircraftUse?.Description;
                    companyMarine.NumberRegister = entityRiskAircraft.RegisterNo;
                    companyMarine.OperatorId = Convert.ToInt32(entityRiskAircraft.AircraftOperatorCode);
                    companyMarine.Risk.Description = entityRiskAircraft.AircraftDescription + " - " + entityRiskAircraft.AircraftYear;

                    companyMarine.Risk.Policy = new Policy
                    {
                        Id = entityPolicy.PolicyId,
                        DocumentNumber = entityPolicy.DocumentNumber,
                        Endorsement = new Endorsement
                        {
                            Id = entityEndorsementRisk.EndorsementId
                        },
                        Prefix = new CommonService.Models.Prefix
                        {
                            Id = entityPolicy.PrefixCode
                        }
                    };

                    ObjectCriteriaBuilder filterSumAssured = new ObjectCriteriaBuilder();
                    filterSumAssured.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                    filterSumAssured.Equal();
                    filterSumAssured.Constant(entityEndorsementRisk.EndorsementId);

                    SumAssuredMarineView assuredView = new SumAssuredMarineView();
                    ViewBuilder builderAssured = new ViewBuilder("SumAssuredMarineView");
                    builderAssured.Filter = filterSumAssured.GetPredicate();
                    DataFacadeManager.Instance.GetDataFacade().FillView(builderAssured, assuredView);

                    decimal insuredAmount = 0;

                    foreach (var item in assuredView.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList())
                    {
                        insuredAmount += item.LimitAmount;
                    }

                    companyMarine.Risk.AmountInsured = insuredAmount;

                    companyMarines.Add(companyMarine);
                }

                return companyMarines;
            }

            return companyMarines;
        }

    }
}