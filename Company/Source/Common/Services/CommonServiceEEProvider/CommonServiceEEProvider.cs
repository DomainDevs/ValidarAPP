using Sistran.Company.Application.CommonServices.EEProvider.DAOs;
using Sistran.Company.Application.CommonServices.EEProvider.Resources;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace Sistran.Company.Application.CommonServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CommonServiceEEProvider : Core.Application.CommonServices.EEProvider.CommonServiceEEProvider, ICommonService
    {

        public List<SubLineBusiness> GetSubLineBusiness()
        {
            try
            {
                var subLineBusinessDAO = new SubLineBusinessDAO(DataFacadeManager.Instance.GetDataFacade());
                return subLineBusinessDAO.GetSubLineBusiness();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<CompositionType> GetCompositionTypes()
        {

            try
            {
                var compositionTypeDAO = new CompositionTypeDAO(DataFacadeManager.Instance.GetDataFacade());
                return compositionTypeDAO.GetCompositionTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Obtiene todos los tipos de detalle
        /// </summary>
        /// <returns></returns>
        public List<DetailType> GetDetailTypes()
        {
            try
            {
                var detailTypeDAO = new DetailTypeDAO(DataFacadeManager.Instance.GetDataFacade());
                return detailTypeDAO.GetDetailTypes();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }  
        }

        public List<Nomenclature> GetNomenclatures()
        {
            try
            {
                NomenclatureDAO nomenclatureDAO = new NomenclatureDAO();
                return nomenclatureDAO.GetNomenclatures();
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorGetScoreCreditDefaultById, ex);
            }
        }

        public CompanyParameter FindCoParameter(int parameterId)
        {
            try
            {
                ParameterDAO parameterDAO = new ParameterDAO();
                return parameterDAO.FindCoParameter(parameterId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(Errors.ErrorFindCoParameter, ex);
            }
        }

        public List<Nomenclature> GetNomenclaturesTask(int id , string Nomenclature, string Abreviature, bool getAllData)
        {
            try
            {
                NomenclatureDAO nomenclatureDAO = new NomenclatureDAO();
                var result = nomenclatureDAO.GetNomenclaturesTask(id, Nomenclature, Abreviature, false);
                DataFacadeManager.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public List<Nomenclature> GetTransformAddress(string Nomenclature)
        {
            try
            {
                NomenclatureDAO nomenclatureDAO = new NomenclatureDAO();
                var result = nomenclatureDAO.GetNomenclatures(Nomenclature);
                DataFacadeManager.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        
    }
}
