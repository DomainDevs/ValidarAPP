using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    internal class QuestionDAO
    {
        /// <summary>
        /// crea un Question
        /// </summary>
        /// <param name="Question"></param>
        /// <returns></returns>
        public static Question CreateQuestion(Question Question)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(Question);
                return Question;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateQuestion", ex);
            }

        }

        /// <summary>
        /// actualiza un Question
        /// </summary>
        /// <param name="Question"></param>
        /// <returns></returns>
        public static Question UpdateQuestion(Question Question)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(Question);
                return Question;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener UpdateQuestion", ex);
            }

        }

        /// <summary>
        /// elimina un Question
        /// </summary>
        /// <param name="Question"></param>
        /// <returns></returns>
        public static void DeleteQuestion(Question Question)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(Question);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener DeleteQuestion", ex);
            }

        }

        /// <summary>
        /// obtiene un Question a partir de QuestionId
        /// </summary>
        /// <param name="Question"></param>
        /// <returns></returns>
        public static Question FindQuestion(int QuestionId)
        {
            try
            {
                Question Question = null;
                PrimaryKey key = Entities.Question.CreatePrimaryKey(QuestionId);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    Question = (Question)daf.GetObjectByPrimaryKey(key);
                }

                return Question;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener FindQuestion", ex);
            }

        }

        /// <summary>
        /// obtiene  una lista de Question a partir del filtro
        /// </summary>
        /// <param name="Question"></param>
        /// <returns></returns>
        public static BusinessCollection ListQuestions(Predicate filter, string[] sort)
        {
            try
            {
                BusinessCollection businessCollection = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessCollection = new BusinessCollection(daf.SelectObjects(typeof(Question), filter, sort));
                }
                return businessCollection;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListQuestions", ex);
            }

        }

    }
}
