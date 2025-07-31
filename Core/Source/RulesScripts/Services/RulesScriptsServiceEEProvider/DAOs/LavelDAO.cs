using Sistran.Core.Application.RulesScriptsServices.EEProvider.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;

namespace Sistran.Core.Application.RulesScriptsServices.EEProvider.DAOs
{
    internal class LevelDAO
    {
        /// <summary>
        /// crea un Level
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static Level CreateLevel(Level level)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().InsertObject(level);
                return level;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener CreateLevel", ex);
            }
        }

        /// <summary>
        /// actualiza un Level
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static Level UpdateLevel(Level level)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(level);
                return level;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener UpdateLevel", ex);
            }

        }

        /// <summary>
        /// elimina un Level
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static void DeleteLevel(Level level)
        {
            try
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(level);
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener DeleteLevel", ex);
            }

        }

        /// <summary>
        /// obtiene una lista de level a partir del filtro
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static BusinessCollection ListLevel(Predicate filter, string[] sort)
        {
            try
            {
                return new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Level), filter, sort));
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener ListLevel", ex);
            }

        }

        /// <summary>
        /// obtiene lso id de facade segun el nivel
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public static int[] GetLevelsEntity(Enums.Level levels)
        {
            try
            {
                int[] numbers = null;

                switch (levels)
                {
                    case Enums.Level.General:
                        numbers = new int[] { 83 };
                        break;
                    case Enums.Level.Coverage:
                        numbers = new int[] { 83, 84, 85 };
                        break;
                    case Enums.Level.Component:
                        numbers = new int[] { 83, 86 };
                        break;
                    case Enums.Level.Commission:
                        numbers = new int[] { 83, 87 };
                        break;
                    case Enums.Level.Risk:
                        numbers = new int[] { 83, 85 };
                        break;
                    default:
                        break;
                }

                return numbers;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error Obtener GetQuestionsByLevel", ex);
            }

        }
    }
}
