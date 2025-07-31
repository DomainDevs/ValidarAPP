using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    internal class EdgeDAO
    {
        /// <summary>
        /// crea un Edge
        /// </summary>
        /// <param name="Edge"></param>
        /// <returns></returns>
        public static Edge CreateEdge(Edge Edge)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(Edge);
                return Edge;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetConceptControl", ex);
            }

        }

        /// <summary>
        /// edita un Edge
        /// </summary>
        /// <param name="Edge"></param>
        /// <returns></returns>
        public static Edge UpdateEdge(Edge Edge)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(Edge);
                return Edge;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener UpdateEdge", ex);
            }

        }

        /// <summary>
        /// elimina un Edge
        /// </summary>
        /// <param name="Edge"></param>
        /// <returns></returns>
        public static void DeleteEdge(Edge Edge)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(Edge);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener DeleteEdge", ex);
            }

        }

        /// <summary>
        /// obtiene un Edge  a partir del EdgeId
        /// </summary>
        /// <param name="Edge"></param>
        /// <returns></returns>
        public static Edge FindEdge(int EdgeId)
        {
            try
            {
                Edge Edge = null;
                PrimaryKey key = Entities.Edge.CreatePrimaryKey(EdgeId);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    Edge = (Edge)daf.GetObjectByPrimaryKey(key);
                }

                return Edge;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener FindEdge", ex);
            }

        }

        /// <summary>
        /// obtiene una lista de Edge a partir del filtro
        /// </summary>
        /// <param name="Edge"></param>
        /// <returns></returns>
        public static BusinessCollection ListEdge(Predicate filter, string[] sort)
        {
            try
            {
                BusinessCollection businessCollection = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessCollection = new BusinessCollection(daf.SelectObjects(typeof(Edge), filter, sort));
                }

                return businessCollection;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListEdge", ex);
            }

        }
    }
}
