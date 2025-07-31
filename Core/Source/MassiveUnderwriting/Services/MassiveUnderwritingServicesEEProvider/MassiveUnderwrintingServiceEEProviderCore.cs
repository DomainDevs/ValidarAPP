using System;
using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.MassiveUnderwritingServices.EEProvider.DAOs;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Application.MassiveServices.Models;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.MassiveUnderwritingServices.EEProvider.Resources;
using Sistran.Core.Application.Utilities.Configuration;

namespace Sistran.Core.Application.MassiveUnderwritingServices.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class MassiveUnderwritingServiceEEProviderCore : IMassiveUnderwritingServiceCore
    {
        public MassiveUnderwritingServiceEEProviderCore()
        {
            Errors.Culture = KeySettings.ServiceResourceCulture;
        }

        /// <summary>
        /// Crear Cargue Emisión
        /// </summary>
        /// <param name="massiveEmission">Cargue</param>
        /// <returns>Cargue</returns>
        public MassiveEmission CreateMassiveEmission(MassiveEmission massiveEmission)
        {
            try
            {
                MassiveEmissionDAO massiveEmissionDAO = new MassiveEmissionDAO();
                return massiveEmissionDAO.CreateMassiveEmission(massiveEmission);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateMassiveLoad), ex);
            }
        }

        /// <summary>
        /// Actualizar cargue
        /// </summary>
        /// <param name="massiveEmission">Cargue</param>
        /// <returns>Cargue</returns>
        public MassiveEmission UpdateMassiveEmission(MassiveEmission massiveEmission)
        {
            try
            {
                MassiveEmissionDAO massiveEmissionDAO = new MassiveEmissionDAO();
                return massiveEmissionDAO.UpdateMassiveEmission(massiveEmission);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorUpdateMassiveLoad), ex);
            }
        }

        /// <summary>
        /// Crear Fila
        /// </summary>
        /// <param name="massiveEmissionRow">Fila</param>
        /// <returns>Fila</returns>
        public MassiveEmissionRow CreateMassiveEmissionRow(MassiveEmissionRow massiveEmissionRow)
        {
            try
            {
                MassiveEmissionRowDAO massiveEmissionRowDAO = new MassiveEmissionRowDAO();
                return massiveEmissionRowDAO.CreateMassiveEmissionRow(massiveEmissionRow);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateRow), ex);
            }
        }

        /// <summary>
        /// Actualizar fila
        /// </summary>
        /// <param name="massiveEmissionRow">Fila</param>
        /// <returns>Fila</returns>
        public MassiveEmissionRow UpdateMassiveEmissionRows(MassiveEmissionRow massiveEmissionRow)
        {
            try
            {
                MassiveEmissionRowDAO massiveEmissionRowDAO = new MassiveEmissionRowDAO();
                return massiveEmissionRowDAO.UpdateMassiveEmissionRows(massiveEmissionRow);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorUpdateRow), ex);
            }
        }

        public List<MassiveEmissionRow> GetMassiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(int massiveLoadId, MassiveLoadProcessStatus? massiveLoadProcessStatus, bool? withError, bool? withEvent)
        {
            try
            {
                MassiveEmissionRowDAO dao = new MassiveEmissionRowDAO();
                return dao.GetMassiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(massiveLoadId, massiveLoadProcessStatus, withError, withEvent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRows), ex);
            }
        }

        public List<MassiveEmissionRow> GetFinalizedMassiveEmissionRowsByMassiveLoadId(int massiveLoadId)
        {
            try
            {
                MassiveEmissionRowDAO dao = new MassiveEmissionRowDAO();
                return dao.GetFinalizedMassiveEmissionRowsByMassiveLoadId(massiveLoadId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRows), ex);
            }            
        }

        /// <summary>
        /// Actualiza los temporales a excluir
        /// </summary>
        /// <param name="massiveLoadId">Id del cargue</param>
        /// <param name="temps">Lista de temporales</param>
        /// <param name="userName">Usuario actual</param>
        /// <param name="deleteTemporal">si debe borrar el temporal excluido</param>
        public bool ExcludeMassiveEmissionRowsTemporals(int massiveLoadId, List<int> temps, string userName, bool deleteTemporal)
        {
            try
            {
                MassiveEmissionRowDAO massiveEmissionRowDAO = new MassiveEmissionRowDAO();
                return massiveEmissionRowDAO.ExcludeMassiveEmissionRowsByTemporals(massiveLoadId, temps, userName, deleteTemporal);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExcludeRows), ex);
            }
        }
        
        public List<MassiveCancellationRow> GetMassiveCancellationRowsWithErrorsWithEventsByMassiveLoadId(int massiveLoadId, bool? withErrors, bool? withEvents)
        {
            try
            {
                MassiveCancellationRowDAO massiveCancellationRowDAO = new MassiveCancellationRowDAO();
                return massiveCancellationRowDAO.GetMassiveCancellationRowsByMassiveLoadId(massiveLoadId, withErrors, withEvents);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRows), ex);
            }
        }

        public MassiveEmission GetMassiveEmissionByMassiveLoadId(int massiveLoadId)
        {
            try
            {
                MassiveEmissionDAO dao = new MassiveEmissionDAO();
                return dao.GetMassiveEmissionByMassiveLoadId(massiveLoadId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetMassiveLoad), ex);
            }
        }

        public List<MassiveCancellationRow> GetMassiveCancellationRowsByMassiveLoadIdSubCoveredRiskType(int massiveLoadId, SubCoveredRiskType? subCoveredRiskType, MassiveLoadProcessStatus? massiveLoadProcessStatus, bool? withError, bool? withEvent)
        {
            try
            {
                MassiveCancellationRowDAO dao = new MassiveCancellationRowDAO();
                return dao.GetMassiveCancellationRowsByMassiveLoadIdSubCoveredRiskType(massiveLoadId, subCoveredRiskType, massiveLoadProcessStatus, withError, withEvent);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRows), ex);
            }
        }

        /// <summary>
        /// Crear Fila
        /// </summary>
        /// <param name="massiveEmissionRow">Fila</param>
        /// <returns>Fila</returns>
        public MassiveCancellationRow CreateMassiveCancellationRow(MassiveCancellationRow massiveCancellationRow)
        {
            try
            {
                MassiveCancellationRowDAO massiveCancellationRowDAO = new MassiveCancellationRowDAO();
                return massiveCancellationRowDAO.CreateMassiveCancellationRow(massiveCancellationRow);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateRow), ex);
            }
        }

        /// <summary>
        /// Actualizar fila
        /// </summary>
        /// <param name="massiveEmissionRow">Fila</param>
        /// <returns>Fila</returns>
        public MassiveCancellationRow UpdateMassiveCancellationRows(MassiveCancellationRow massiveCancellationRow)
        {
            try
            {
                MassiveCancellationRowDAO massiveCancellationRowDAO = new MassiveCancellationRowDAO();
                return massiveCancellationRowDAO.UpdateMassiveCancellationRows(massiveCancellationRow);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorUpdateRow), ex);
            }
        }

        public MassiveEmissionRow GetMassiveEmissionRowByMassiveLoadIdRowNumber(int massiveLoadProcessId, int rowNumber)
        {
            try
            {
                MassiveEmissionRowDAO emissionRowDao = new MassiveEmissionRowDAO();
                return emissionRowDao.GetMassiveEmissionRowByMassiveLoadIdRowNumber(massiveLoadProcessId, rowNumber);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetRow), ex);
            }
        }

        public bool ExcludeMassiveCancellationRowsByTemporals(int massiveLoadId, List<int> temps, string userName, bool deleteTemporal)
        {
            try
            {
                MassiveCancellationRowDAO massiveEmissionRowDAO = new MassiveCancellationRowDAO();
                return massiveEmissionRowDAO.ExcludeMassiveCancellationRowsByTemporals(massiveLoadId, temps, userName, deleteTemporal);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorExcludeRows), ex);
            }
        }

       
    }
}