using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    internal class NodeQuestionDAO
    {
        /// <summary>
        /// crea un NodeQuestion
        /// </summary>
        /// <param name="NodeQuestion"></param>
        /// <returns></returns>
        public static NodeQuestion CreateNodeQuestion(NodeQuestion NodeQuestion)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(NodeQuestion);
                return NodeQuestion;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateNodeQuestion", ex);
            }
            
        }

        /// <summary>
        /// actualiza un NodeQuestion
        /// </summary>
        /// <param name="NodeQuestion"></param>
        /// <returns></returns>
        public static NodeQuestion UpdateNodeQuestion(NodeQuestion NodeQuestion)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(NodeQuestion);
                return NodeQuestion;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener UpdateNodeQuestion", ex);
            }
            
        }

        /// <summary>
        /// elimina  un NodeQuestion
        /// </summary>
        /// <param name="NodeQuestion"></param>
        /// <returns></returns>
        public static void DeleteNodeQuestion(NodeQuestion NodeQuestion)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(NodeQuestion);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener DeleteNodeQuestion", ex);
            }
            
        }

        /// <summary>
        /// obtiene un NodeQuestion a partir de NodeId, QuestionId, ScriptId
        /// </summary>
        /// <param name="NodeQuestion"></param>
        /// <returns></returns>
        public static NodeQuestion FindNodeQuestion(int NodeId, int QuestionId, int ScriptId)
        {
            try
            {
                PrimaryKey key = Entities.NodeQuestion.CreatePrimaryKey(NodeId, QuestionId, ScriptId);
                NodeQuestion NodeQuestion = (NodeQuestion)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
                return NodeQuestion;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener FindNodeQuestion", ex);
            }
            
        }

        /// <summary>
        /// obtiene una lista de NodeQuestion a partir del filtro
        /// </summary>
        /// <param name="NodeQuestion"></param>
        /// <returns></returns>
        public static BusinessCollection ListNodeQuestion(Predicate filter, string[] sort)
        {
            try
            {
                return new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(NodeQuestion), filter, sort));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListNodeQuestion", ex);
            }
            
        }
    }
}
