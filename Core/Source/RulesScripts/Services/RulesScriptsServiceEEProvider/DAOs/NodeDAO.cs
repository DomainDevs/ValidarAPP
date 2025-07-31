using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    internal class NodeDAO
    {
        /// <summary>
        /// crea un Node
        /// </summary>
        /// <param name="Node"></param>
        /// <returns></returns>
        public static Node CreateNode(Node Node)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(Node);
                return Node;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateNode", ex);
            }

        }

        /// <summary>
        /// edita un Node
        /// </summary>
        /// <param name="Node"></param>
        /// <returns></returns>
        public static Node UpdateNode(Node Node)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(Node);
                return Node;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener UpdateNode", ex);
            }

        }

        /// <summary>
        /// elimina un Node
        /// </summary>
        /// <param name="Node"></param>
        /// <returns></returns>
        public static void DeleteNode(Node Node)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(Node);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener DeleteNode", ex);
            }

        }

        /// <summary>
        /// obtiene un Node a partir de NodeId, ScriptId
        /// </summary>
        /// <param name="Node"></param>
        /// <returns></returns>
        public static Node FindNode(int NodeId, int ScriptId)
        {
            try
            {
                Node Node = null;
                PrimaryKey key = Entities.Node.CreatePrimaryKey(NodeId, ScriptId);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    Node = (Node)daf.GetObjectByPrimaryKey(key);
                }

                return Node;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener FindNode", ex);
            }

        }

        /// <summary>
        /// obtiene una lsita de Node a partir del filtro
        /// </summary>
        /// <param name="Node"></param>
        /// <returns></returns>
        public static BusinessCollection ListNode(Predicate filter, string[] sort)
        {
            try
            {
                BusinessCollection businessCollection = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessCollection = new BusinessCollection(daf.SelectObjects(typeof(Node), filter, sort));
                }
                return businessCollection;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListNode", ex);
            }

        }


        /// <summary>
        /// obtiene una lsita de Node a partir del filtro
        /// </summary>
        /// <param name="Node"></param>
        /// <returns></returns>
        public static BusinessCollection GetListNode(ObjectCriteriaBuilder filterNode, string[] sort)
        {
            try
            {
                BusinessCollection businessCollection = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessCollection = new BusinessCollection(daf.SelectObjects(typeof(Node), filterNode.GetPredicate(), sort));
                }
                return businessCollection;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListNode", ex);
            }

        }
    }
}
