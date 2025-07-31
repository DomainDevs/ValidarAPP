using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    internal class EdgeAnswerDAO
    {
        /// <summary>
        /// crea un EdgeAnswer
        /// </summary>
        /// <param name="EdgeAnswer"></param>
        /// <returns></returns>
        public static EdgeAnswer CreateEdgeAnswer(EdgeAnswer EdgeAnswer)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(EdgeAnswer);
                return EdgeAnswer;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateEdgeAnswer", ex);
            }

        }

        /// <summary>
        /// edita un EdgeAnswer
        /// </summary>
        /// <param name="EdgeAnswer"></param>
        /// <returns></returns>
        public static EdgeAnswer UpdateEdgeAnswer(EdgeAnswer EdgeAnswer)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(EdgeAnswer);
                return EdgeAnswer;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener UpdateEdgeAnswer", ex);
            }

        }

        /// <summary>
        /// elimina un EdgeAnswer
        /// </summary>
        /// <param name="EdgeAnswer"></param>
        /// <returns></returns>
        public static void DeleteEdgeAnswer(EdgeAnswer EdgeAnswer)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(EdgeAnswer);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener DeleteEdgeAnswer", ex);
            }

        }

        /// <summary>
        /// obtiene un EdgeAnswer a partir de EdgeAnswerId, ConceptId, EntitiyId
        /// </summary>
        /// <param name="EdgeAnswer"></param>
        /// <returns></returns>
        public static EdgeAnswer FindEdgeAnswer(int EdgeAnswerId, int ConceptId, int EntitiyId)
        {
            try
            {
                EdgeAnswer EdgeAnswer = null;
                PrimaryKey key = Entities.EdgeAnswer.CreatePrimaryKey(EdgeAnswerId, ConceptId, EntitiyId);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    EdgeAnswer = (EdgeAnswer)daf.GetObjectByPrimaryKey(key);
                }
                return EdgeAnswer;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener FindEdgeAnswer", ex);
            }

        }

        /// <summary>
        /// obtiene una lista de EdgeAnswer a partir del filtro
        /// </summary>
        /// <param name="EdgeAnswer"></param>
        /// <returns></returns>
        public static BusinessCollection ListEdgeAnswer(Predicate filter, string[] sort)
        {
            try
            {
                BusinessCollection businessCollection = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessCollection = new BusinessCollection(daf.SelectObjects(typeof(EdgeAnswer), filter, sort));
                }
                return businessCollection;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListEdgeAnswer", ex);
            }

        }

    }
}
