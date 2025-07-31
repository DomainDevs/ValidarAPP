using Sistran.Core.Application.CommonServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using COMMML = Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    public class ParameterDAO
    {
        /// <summary>
        /// Obtener lista de parametros por id's
        /// </summary>
        /// <param name="parameters">Lista de id's</param>
        /// <returns>Lista de parametros</returns>
        public List<COMMML.Parameter> GetParametersByParameterIds(List<COMMML.Parameter> parameters)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.Parameter.Properties.ParameterId, typeof(COMMEN.Parameter).Name);
            filter.In().ListValue();
            foreach (COMMML.Parameter item in parameters)
            {
                filter.Constant(item.Id);
            }

            filter.EndList();
            BusinessCollection businessCollection = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(COMMEN.Parameter), filter.GetPredicate()));
            }

            return ModelAssembler.CreateParameters(businessCollection);
        }

        /// <summary>
        /// Obtener lista de parametros por id's
        /// </summary>
        /// <param name="parameters">Lista de id's</param>
        /// <returns>Lista de parametros</returns>
        public List<COMMML.Parameter> GetParametersByIds(List<int> ids)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.Parameter.Properties.ParameterId, typeof(COMMEN.Parameter).Name);
            filter.In().ListValue();
            foreach (int id in ids)
            {
                filter.Constant(id);
            }

            filter.EndList();
            BusinessCollection businessCollection = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(COMMEN.Parameter), filter.GetPredicate()));
            }

            return ModelAssembler.CreateParameters(businessCollection);
        }

        /// <summary>
        /// Obtener lista de parametros por id's
        /// </summary>
        /// <param name="parameters">Lista de id's</param>
        /// <returns>Lista de parametros</returns>
        public List<COMMML.Parameter> GetParametersByDescriptions(List<string> parameters)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.Parameter.Properties.Description, typeof(COMMEN.Parameter).Name);
            filter.In().ListValue();
            foreach (string item in parameters)
            {
                filter.Constant(item);
            }

            filter.EndList();
            BusinessCollection businessCollection = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(COMMEN.Parameter), filter.GetPredicate()));
            }


            return ModelAssembler.CreateParameters(businessCollection);
        }

        public COMMML.Parameter GetParameterByParameterId(int parameterId)
        {
            PrimaryKey primaryKey = COMMEN.Parameter.CreatePrimaryKey(parameterId);
            COMMEN.Parameter entityParameter = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                entityParameter = (COMMEN.Parameter)daf.GetObjectByPrimaryKey(primaryKey);
            }

            if (entityParameter != null)
            {
                return ModelAssembler.CreateParameter(entityParameter);
            }
            else
            {
                return null;
            }
        }

        public COMMML.Parameter GetParameterByDescription(string description)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(COMMEN.Parameter.Properties.Description, description);
            BusinessCollection parameters = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                parameters = new BusinessCollection(daf.SelectObjects(typeof(COMMEN.Parameter), filter.GetPredicate(), null));
            }


            return ModelAssembler.CreateParameter(parameters.Cast<COMMEN.Parameter>().First());
        }

        /// <summary>
        /// ActualizarLista de parametros
        /// </summary>
        /// <param name="parameterId">parametro</param>
        /// <returns></returns>
        public COMMML.Parameter UpdateParameters(COMMML.Parameter parameter)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = COMMEN.Parameter.CreatePrimaryKey(parameter.Id);
            COMMEN.Parameter parameterEntity = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                parameterEntity = (COMMEN.Parameter)daf.GetObjectByPrimaryKey(key);

            }

            if (parameterEntity != null)
            {
                parameterEntity.NumberParameter = parameter.NumberParameter;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(parameterEntity);
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.UpdateParameters");
                return parameter;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.UpdateParameters");
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="package"></param>
        /// <returns></returns>
        public static COMMEN.Parameter UpdateParameter(COMMEN.Parameter parameter)
        {
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(parameter);
            return parameter;
        }

        /// <summary>
        /// Buscar un Parametro especifico tabla CO_PARAMETER
        /// </summary>
        /// <param name="parameterId">Id paremetro</param>
        /// <returns>Parameter</returns>
        public static COMMML.Parameter GetExtendedParameterByParameterId(int parameterId)
        {
            COMMEN.CoParameter coParameter = null;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = COMMEN.CoParameter.CreatePrimaryKey(parameterId);
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                coParameter = (COMMEN.CoParameter)daf.GetObjectByPrimaryKey(key);
            }

            if (coParameter != null)
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetExtendedParameterByParameterId");
                return ModelAssembler.CreateCoParameter(coParameter);
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetExtendedParameterByParameterId");
                return null;
            }
        }

        /// <summary>
        /// Actualizar parametro CO
        /// </summary>
        /// <param name="parameter">Parametro</param>
        /// <returns>Parametro</returns>
        public static COMMML.Parameter UpdateExtendedParameter(COMMML.Parameter parameter)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = COMMEN.CoParameter.CreatePrimaryKey(parameter.Id);
            COMMEN.CoParameter coParameter = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                coParameter = (COMMEN.CoParameter)daf.GetObjectByPrimaryKey(key);
            }

            if (coParameter != null)
            {
                coParameter.BoolParameter = parameter.BoolParameter ?? false;
                coParameter.DateParameter = parameter.DateParameter;
                coParameter.NumberParameter = parameter.NumberParameter;
                coParameter.TextParameter = parameter.TextParameter;
                coParameter.PercentageParameter = parameter.PercentageParameter;
                coParameter.AmountParameter = parameter.AmountParameter;

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(coParameter);
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.UpdateExtendedParameter");
                return parameter;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.UpdateExtendedParameter");
                return null;
            }
        }

        /// <summary>
        /// Obtener lista de parametros por id's
        /// </summary>
        /// <param name="parameters">Lista de id's</param>
        /// <returns>Lista de parametros</returns>
        public List<COMMML.Parameter> GetExtendedParameters(List<COMMML.Parameter> parameters)
        {
            BusinessCollection businessCollection = null;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.CoParameter.Properties.ParameterId, typeof(COMMEN.CoParameter).Name);
            filter.In().ListValue();
            foreach (COMMML.Parameter item in parameters)
            {
                filter.Constant(item.Id);
            }
            filter.EndList();

            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(COMMEN.CoParameter), filter.GetPredicate()));
            }

            return ModelAssembler.CreateCoParameters(businessCollection);
        }
    }
}
