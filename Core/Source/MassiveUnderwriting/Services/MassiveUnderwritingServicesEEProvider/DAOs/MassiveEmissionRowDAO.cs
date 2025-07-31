using System.Collections.Generic;
using Sistran.Core.Application.MassiveUnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using MSVEN = Sistran.Core.Application.Massive.Entities;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Framework;
using Sistran.Core.Application.MassiveUnderwritingServices.EEProvider.Resources;
using System.Linq;

namespace Sistran.Core.Application.MassiveUnderwritingServices.EEProvider.DAOs
{
    public class MassiveEmissionRowDAO
    {
        /// <summary>
        /// Crear Proceso De Cargue Masivo
        /// </summary>
        /// <param name="massiveLoadProcess">Proceso De Cargue Masivo</param>
        /// <returns>Proceso De Cargue Masivo</returns>
        public MassiveEmissionRow CreateMassiveEmissionRow(MassiveEmissionRow massiveLoadProcess)
        {
            MSVEN.MassiveEmissionRow entityMassiveLoadProcess = EntityAssembler.CreateMassiveEmissionRow(massiveLoadProcess);

            DataFacadeManager.Instance.GetDataFacade().InsertObject(entityMassiveLoadProcess);

            if (entityMassiveLoadProcess != null)
            {
                massiveLoadProcess.Id = entityMassiveLoadProcess.Id;
                return massiveLoadProcess;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Actualizar Proceso De Cargue Masivo
        /// </summary>
        /// <param name="massiveLoadProcess">Proceso De Cargue Masivo</param>
        /// <returns>Proceso De Cargue Masivo</returns>
        public MassiveEmissionRow UpdateMassiveEmissionRows(MassiveEmissionRow massiveLoadProcess)
        {
            PrimaryKey primaryKey = MSVEN.MassiveEmissionRow.CreatePrimaryKey(massiveLoadProcess.Id);
            MSVEN.MassiveEmissionRow entityMassiveEmissionRow = (MSVEN.MassiveEmissionRow)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

            if (entityMassiveEmissionRow != null)
            {
                entityMassiveEmissionRow.StatusId = (int)massiveLoadProcess.Status;
                entityMassiveEmissionRow.HasError = massiveLoadProcess.HasError;
                entityMassiveEmissionRow.Observations = massiveLoadProcess.Observations;
                entityMassiveEmissionRow.SerializedRow = massiveLoadProcess.SerializedRow;
                entityMassiveEmissionRow.Commiss = massiveLoadProcess.TotalCommission;
                entityMassiveEmissionRow.HasEvents = massiveLoadProcess.HasEvents;

                if (massiveLoadProcess.Risk != null && massiveLoadProcess.Risk.Policy != null)
                {
                    entityMassiveEmissionRow.TempId = massiveLoadProcess.Risk.Policy.Id;
                    entityMassiveEmissionRow.DocumentNumber = massiveLoadProcess.Risk.Policy.DocumentNumber;
                    entityMassiveEmissionRow.RiskDescription = massiveLoadProcess.Risk.Description;

                    if (massiveLoadProcess.Risk.Policy.Summary != null)
                    {
                        entityMassiveEmissionRow.Premium = massiveLoadProcess.Risk.Policy.Summary.FullPremium;
                    }

                    if (massiveLoadProcess.Risk.Policy.PolicyType != null)
                    {
                        entityMassiveEmissionRow.PolicyType = massiveLoadProcess.Risk.Policy.PolicyType.Description;
                    }
                    if (massiveLoadProcess.Risk.Policy.Endorsement != null)
                    {
                        entityMassiveEmissionRow.EndorsementId = massiveLoadProcess.Risk.Policy.Endorsement.Id;
                    }

                }

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityMassiveEmissionRow);

                return massiveLoadProcess;
            }
            else
            {
                return null;
            }
        }

        ///// <summary>
        ///// Actualizar Proceso De Cargue Masivo
        ///// </summary>
        ///// <param name="massiveLoadProcess">Proceso De Cargue Masivo</param>
        ///// <returns>Proceso De Cargue Masivo</returns>
        public List<MassiveEmissionRow> GetMassiveEmissionRowsByMassiveLoadIdMassiveLoadProcessStatus(int massiveLoadProcessId, MassiveLoadProcessStatus? massiveLoadProcessStatus, bool? withError, bool? withEvent)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(MSVEN.MassiveEmissionRow.Properties.MassiveLoadId, typeof(MSVEN.MassiveEmissionRow).Name);
            filter.Equal();
            filter.Constant(massiveLoadProcessId);

            if (massiveLoadProcessStatus.HasValue)
            {
                filter.And();
                filter.Property(MSVEN.MassiveEmissionRow.Properties.StatusId, typeof(MSVEN.MassiveEmissionRow).Name);
                filter.Equal();
                filter.Constant(massiveLoadProcessStatus.Value);
            }

            if (withError.HasValue && withError.Value && withEvent.HasValue && withEvent.Value)
            {
                filter.And();
                filter.OpenParenthesis();
                filter.Property(MSVEN.MassiveEmissionRow.Properties.HasError, typeof(MSVEN.MassiveEmissionRow).Name);
                filter.Equal();
                filter.Constant(withError.Value);
                filter.Or();
                filter.Property(MSVEN.MassiveEmissionRow.Properties.HasEvents, typeof(MSVEN.MassiveEmissionRow).Name);
                filter.Equal();
                filter.Constant(withEvent.Value);
                filter.CloseParenthesis();
            }
            else
            {

                if (withError.HasValue)
                {
                    filter.And();
                    filter.Property(MSVEN.MassiveEmissionRow.Properties.HasError, typeof(MSVEN.MassiveEmissionRow).Name);
                    filter.Equal();
                    filter.Constant(withError.Value);
                }

                if (withEvent.HasValue)
                {
                    filter.And();
                    filter.Property(MSVEN.MassiveEmissionRow.Properties.HasEvents, typeof(MSVEN.MassiveEmissionRow).Name);
                    filter.Equal();
                    filter.Constant(withEvent.Value);
                }
            }


            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(MSVEN.MassiveEmissionRow), filter.GetPredicate()));
            List<MassiveEmissionRow> massiveEmissionRows = ModelAssembler.CreateMassiveEmissionRows(businessCollection);

            return massiveEmissionRows;
        }

        /// <summary>
        /// Actualiza los temporales a excluir
        /// </summary>
        /// <param name="massiveLoadId">Id del cargue</param>
        /// <param name="temps">Lista de temporales</param>
        /// <param name="userName">Usuario actual</param>
        /// <param name="deleteTemporal">si debe borrar el temporal excluido</param>
        public bool ExcludeMassiveEmissionRowsByTemporals(int massiveLoadId, List<int> temps, string userName, bool deleteTemporal)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(MSVEN.MassiveEmissionRow.Properties.MassiveLoadId, typeof(MSVEN.MassiveEmissionRow).Name);
            filter.Equal();
            filter.Constant(massiveLoadId);
            filter.And();
            filter.Property(MSVEN.MassiveEmissionRow.Properties.HasError, typeof(MSVEN.MassiveEmissionRow).Name);
            filter.Equal();
            filter.Constant(false);
            filter.And();
            filter.Property(MSVEN.MassiveEmissionRow.Properties.HasEvents, typeof(MSVEN.MassiveEmissionRow).Name);
            filter.Equal();
            filter.Constant(false);
            filter.And();
            filter.Property(MSVEN.MassiveEmissionRow.Properties.RowNumber, typeof(MSVEN.MassiveEmissionRow).Name);
            filter.In().ListValue();
            foreach (int temporal in temps)
            {
                filter.Constant(temporal);
            }
            filter.EndList();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(MSVEN.MassiveEmissionRow), filter.GetPredicate()));

            if (temps.Count == businessCollection.Count)
            {
                List<MassiveEmissionRow> massiveEmissionRows = ModelAssembler.CreateMassiveEmissionRows(businessCollection);

                foreach (MassiveEmissionRow massiveEmissionRow in massiveEmissionRows)
                {
                    massiveEmissionRow.HasError = true;
                    massiveEmissionRow.Observations = Resources.Errors.MessageExcludedTemporalMessage + " " + userName;
                    UpdateMassiveEmissionRows(massiveEmissionRow);
                    if (deleteTemporal)
                    {
                        DelegateService.underwritingServiceCore.DeleteTemporalsByOperationId(massiveEmissionRow.Risk.Policy.Id);
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }

        ///// <summary>
        ///// Retorna los procesos finalizados
        ///// </summary>
        ///// <param name="massiveLoadId">Proceso De Cargue Masivo</param>
        ///// <returns>Proceso De Cargue Masivo</returns>
        public List<MassiveEmissionRow> GetFinalizedMassiveEmissionRowsByMassiveLoadId(int massiveLoadId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(MSVEN.MassiveEmissionRow.Properties.MassiveLoadId, typeof(MSVEN.MassiveEmissionRow).Name);
            filter.Equal();
            filter.Constant(massiveLoadId);
            filter.And();
            filter.Property(MSVEN.MassiveEmissionRow.Properties.StatusId, typeof(MSVEN.MassiveEmissionRow).Name);
            filter.In();
            filter.ListValue();
            filter.Constant(MassiveLoadProcessStatus.Issuance);
            filter.Constant(MassiveLoadProcessStatus.Finalized);
            filter.EndList();


            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(MSVEN.MassiveEmissionRow), filter.GetPredicate()));
            List<MassiveEmissionRow> massiveEmissionRows = ModelAssembler.CreateMassiveEmissionRows(businessCollection);

            return massiveEmissionRows;
        }

        public MassiveEmissionRow GetMassiveEmissionRowByMassiveLoadIdRowNumber(int massiveLoadId, int rowNumber)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(MSVEN.MassiveEmissionRow.Properties.MassiveLoadId, typeof(MSVEN.MassiveEmissionRow).Name);
            filter.Equal();
            filter.Constant(massiveLoadId);
            filter.And();
            filter.Property(MSVEN.MassiveEmissionRow.Properties.RowNumber, typeof(MSVEN.MassiveEmissionRow).Name);
            filter.Equal();
            filter.Constant(rowNumber);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(MSVEN.MassiveEmissionRow), filter.GetPredicate()));
            MassiveEmissionRow massiveEmissionRow = ModelAssembler.CreateMassiveEmissionRow(businessCollection.Cast<MSVEN.MassiveEmissionRow>().First());

            return massiveEmissionRow;
        }

       
    }
}