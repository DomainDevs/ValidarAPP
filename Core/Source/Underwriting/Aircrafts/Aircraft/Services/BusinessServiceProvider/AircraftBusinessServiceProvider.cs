using System;
using System.Collections.Generic;
using Sistran.Core.Application.Aircrafts.AircraftBusinessService.Models;
using Sistran.Core.Application.Aircrafts.AircraftBusinessService.EEProvider.Assemblers;
using Sistran.Core.Application.Aircrafts.AircraftBusinessService.EEProvider.Resources;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.Managers;
using COMMEN = Sistran.Core.Application.Common.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using COMMON = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Application.Aircrafts.AircraftBusinessService.EEProvider.Views;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.Aircrafts.AircraftBusinessService.EEProvider.Business;

namespace Sistran.Core.Application.Aircrafts.AircraftBusinessService.EEProvider
{
    public class AircraftBusinessServiceProvider : IAircraftBusinessService
    {
        #region Aircraft
        #region GetAircraftTypes        
        /// <summary>
        /// Obtiener la lista de tipos de mercancias
        /// </summary>
        /// <returns>Lista de Mercancias</returns>
        public List<AircraftType> GetAircraftTypes(int prefixid)
        {
            try
            {
                return ModelAssembler.CreateAircraftTypes(DataFacadeManager.GetObjects(typeof(COMMEN.AircraftType)));
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetAircraftTypes), ex);
            }
        }
        #region GetMakes
        public List<Make> GetMakes()
        {
            try
            {
                return ModelAssembler.CreateMakes(DataFacadeManager.GetObjects(typeof(COMMON.AircraftMake)));
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
                return ModelAssembler.CreateModelByMakeIds(DataFacadeManager.GetObjects(typeof(COMMON.AircraftModel)));
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
                return ModelAssembler.CreateOperators(DataFacadeManager.GetObjects(typeof(COMMON.AircraftOperator)));
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
                return ModelAssembler.CreateRegisters(DataFacadeManager.GetObjects(typeof(COMMON.AircraftRegister)));
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRegisters), ex);
            }
        }
        #endregion
        # region GetType
        public List<AircraftType> GetType(int prefixId)
        {
            try
            {
                return ModelAssembler.CreateTypes(DataFacadeManager.GetObjects(typeof(COMMON.AircraftType)));
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetType), ex);
            }
        }
        #endregion
        #region GetTypesByPrefixId

        #endregion

        #region GetTypesByPrefixIdView
        public List<AircraftType> GetTybeByTypesByPrefixId(int prefixId)
        {
            try
            {

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(COMMEN.AircraftTypePrefix.Properties.PrefixCode, typeof(COMMEN.AircraftTypePrefix).Name);
                filter.Equal();
                filter.Constant(prefixId);

                AircraftTypePrefixView typePrefixView = new AircraftTypePrefixView();
                ViewBuilder builder = new ViewBuilder("TypePrefixView");
                builder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, typePrefixView);

                if (typePrefixView.AircraftTypePrefixs.Count > 0)
                {
                    return ModelAssembler.CreateTypesByPrefixIds(typePrefixView.AircraftTypes);
                }
                return new List<AircraftType>();
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetTypesByPrefixId), ex);
            }
        }
        #endregion
        #region GetUses
        public List<Use> GetUses(int prefixId)
        {
            try
            {
                return ModelAssembler.CreateUses(DataFacadeManager.GetObjects(typeof(COMMON.AircraftUse)));
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetUses), ex);
            }
        }

        #endregion

        #region GetTypesByPrefixIdView
        public List<Use> GetUseByusessByPrefixId(int prefixId)
        {
            try
            {

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(COMMEN.AircraftUsePrefix.Properties.PrefixCode, typeof(COMMEN.AircraftUsePrefix).Name);
                filter.Equal();
                filter.Constant(prefixId);

                AircraftsUsePrefixView usePrefixView = new AircraftsUsePrefixView();
                ViewBuilder builder = new ViewBuilder("UsePrefixView");
                builder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, usePrefixView);

                if (usePrefixView.AircraftUsePrefixs.Count > 0)
                {
                    return ModelAssembler.CreateUsesByPrefixIds(usePrefixView.AircraftUses);
                }
                return new List<Use>();
            }
            catch (Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetUsesByPrefixId), ex);
            }
        }
        #endregion
        #endregion
        #endregion

        #region Claims


        public List<Aircraft> GetRiskAircraftsByEndorsementId(int endorsemetId)
        {
            try
            {
                BusinessAircraft businessAircraft = new BusinessAircraft();
                return businessAircraft.GetRiskAircraftsByEndorsementId(endorsemetId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public Aircraft GetRiskAircraftByRiskId(int riskId)
        {
            try
            {
                BusinessAircraft businessAircraft = new BusinessAircraft();
                return businessAircraft.GetRiskAircraftByRiskId(riskId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Aircraft> GetRiskAircraftsByInsuredId(int insuredId)
        {
            try
            {
                BusinessAircraft businessAircraft = new BusinessAircraft();
                return businessAircraft.GetRiskAircraftsByInsuredId(insuredId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion
    }
}