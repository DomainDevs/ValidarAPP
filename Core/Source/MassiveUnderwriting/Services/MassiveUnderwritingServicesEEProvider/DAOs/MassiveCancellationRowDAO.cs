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
using Sistran.Core.Application.CommonService.Enums;

namespace Sistran.Core.Application.MassiveUnderwritingServices.EEProvider.DAOs
{
    public class MassiveCancellationRowDAO
    {
        /// <summary>
        /// Crear Proceso De Cargue Masivo
        /// </summary>
        /// <param name="massiveLoadProcess">Proceso De Cargue Masivo</param>
        /// <returns>Proceso De Cargue Masivo</returns>
        public MassiveCancellationRow CreateMassiveCancellationRow(MassiveCancellationRow massiveLoadProcess)
        {
            MSVEN.MassiveCancellationRow entityMassiveLoadProcess = EntityAssembler.CreateMassiveCancellationRow(massiveLoadProcess);

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
        public MassiveCancellationRow UpdateMassiveCancellationRows(MassiveCancellationRow massiveLoadProcess)
        {
            PrimaryKey primaryKey = MSVEN.MassiveCancellationRow.CreatePrimaryKey(massiveLoadProcess.Id);
            MSVEN.MassiveCancellationRow entityMassiveCancellationRow = (MSVEN.MassiveCancellationRow)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

            if (entityMassiveCancellationRow != null)
            {
                entityMassiveCancellationRow.StatusId = (int)massiveLoadProcess.Status;
                entityMassiveCancellationRow.HasError = massiveLoadProcess.HasError;
                entityMassiveCancellationRow.Observations = massiveLoadProcess.Observations;
                entityMassiveCancellationRow.SerializedRow = massiveLoadProcess.SerializedRow;
                entityMassiveCancellationRow.Commiss = massiveLoadProcess.TotalCommission;
                entityMassiveCancellationRow.HasEvents = massiveLoadProcess.HasEvents;
                entityMassiveCancellationRow.SubCoveredRiskTypeCode = (int)massiveLoadProcess.SubcoveredRiskType;

                if (massiveLoadProcess.Risk != null && massiveLoadProcess.Risk.Policy != null)
                {
                    entityMassiveCancellationRow.TempId = massiveLoadProcess.Risk.Policy.Id;
                    entityMassiveCancellationRow.DocumentNumber = massiveLoadProcess.Risk.Policy.DocumentNumber;
                    entityMassiveCancellationRow.RiskDescription = massiveLoadProcess.Risk.Description;

                    if (massiveLoadProcess.Risk.Policy.Summary != null)
                    {
                        entityMassiveCancellationRow.Premium = massiveLoadProcess.Risk.Policy.Summary.FullPremium;
                    }

                    if (massiveLoadProcess.Risk.Policy.PolicyType != null)
                    {
                        entityMassiveCancellationRow.PolicyType = massiveLoadProcess.Risk.Policy.PolicyType.Description;
                    }
                    if (massiveLoadProcess.Risk.Policy.Endorsement != null)
                    {
                        entityMassiveCancellationRow.EndorsementId = massiveLoadProcess.Risk.Policy.Endorsement.Id;
                    }

                }

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityMassiveCancellationRow);

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
        public List<MassiveCancellationRow> GetMassiveCancellationRowsByMassiveLoadIdSubCoveredRiskType(int massiveLoadProcessId, SubCoveredRiskType? subCoveredRiskType, MassiveLoadProcessStatus? massiveLoadProcessStatus, bool? withError, bool? withEvent)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(MSVEN.MassiveCancellationRow.Properties.MassiveLoadId, typeof(MSVEN.MassiveCancellationRow).Name);
            filter.Equal();
            filter.Constant(massiveLoadProcessId);
            if (subCoveredRiskType.HasValue)
            {
                filter.And();
                filter.Property(MSVEN.MassiveCancellationRow.Properties.SubCoveredRiskTypeCode, typeof(MSVEN.MassiveCancellationRow).Name);
                filter.Equal();
                filter.Constant(subCoveredRiskType);
            }
            if (massiveLoadProcessStatus.HasValue)
            {
                filter.And();
                filter.Property(MSVEN.MassiveCancellationRow.Properties.StatusId, typeof(MSVEN.MassiveCancellationRow).Name);
                filter.Equal();
                filter.Constant(massiveLoadProcessStatus.Value);
            }

            if (withError.HasValue && withError.Value && withEvent.HasValue && withEvent.Value)
            {
                filter.And();
                filter.OpenParenthesis();
                filter.Property(MSVEN.MassiveCancellationRow.Properties.HasError, typeof(MSVEN.MassiveCancellationRow).Name);
                filter.Equal();
                filter.Constant(withError.Value);
                filter.Or();
                filter.Property(MSVEN.MassiveCancellationRow.Properties.HasEvents, typeof(MSVEN.MassiveCancellationRow).Name);
                filter.Equal();
                filter.Constant(withEvent.Value);
                filter.CloseParenthesis();
            }
            else
            {

                if (withError.HasValue)
                {
                    filter.And();
                    filter.Property(MSVEN.MassiveCancellationRow.Properties.HasError, typeof(MSVEN.MassiveCancellationRow).Name);
                    filter.Equal();
                    filter.Constant(withError.Value);
                }

                if (withEvent.HasValue)
                {
                    filter.And();
                    filter.Property(MSVEN.MassiveCancellationRow.Properties.HasEvents, typeof(MSVEN.MassiveCancellationRow).Name);
                    filter.Equal();
                    filter.Constant(withEvent.Value);
                }
            }


            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(MSVEN.MassiveCancellationRow), filter.GetPredicate()));
            List<MassiveCancellationRow> massiveCancellationRows = ModelAssembler.CreateMassiveCancellationRows(businessCollection);

            return massiveCancellationRows;
        }

        public List<MassiveCancellationRow> GetMassiveCancellationRowsByMassiveLoadId(int massiveLoadId, bool? withErrors, bool? withEvents)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(MSVEN.MassiveCancellationRow.Properties.MassiveLoadId, typeof(MSVEN.MassiveCancellationRow).Name);
            filter.Equal();
            filter.Constant(massiveLoadId);

            if (withErrors.HasValue)
            {
                filter.And();
                filter.Property(MSVEN.MassiveCancellationRow.Properties.HasError, typeof(MSVEN.MassiveCancellationRow).Name);
                filter.Equal();
                filter.Constant(withErrors.Value);
            }

            if (withEvents.HasValue)
            {
                filter.And();
                filter.Property(MSVEN.MassiveCancellationRow.Properties.HasEvents, typeof(MSVEN.MassiveCancellationRow).Name);
                filter.Equal();
                filter.Constant(withEvents.Value);
            }

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(MSVEN.MassiveCancellationRow), filter.GetPredicate()));
            List<MassiveCancellationRow> massiveCancellationRows = new List<MassiveCancellationRow>();
            if (businessCollection != null && businessCollection.Count > 0)
            {
                massiveCancellationRows = ModelAssembler.CreateMassiveCancellationRows(businessCollection);
            }
            return massiveCancellationRows;
        }


        /// <summary>
        /// Excluye del temporario de cancelacion
        /// </summary>
        /// <param name="massiveLoadId"></param>
        /// <param name="temps"></param>
        /// <param name="userName"></param>
        /// <param name="deleteTemporal"></param>
        /// <returns></returns>
        public bool ExcludeMassiveCancellationRowsByTemporals(int massiveLoadId, List<int> temps, string userName, bool deleteTemporal)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(MSVEN.MassiveCancellationRow.Properties.MassiveLoadId, typeof(MSVEN.MassiveCancellationRow).Name);
            filter.Equal();
            filter.Constant(massiveLoadId);
            filter.And();
            filter.Property(MSVEN.MassiveCancellationRow.Properties.HasError, typeof(MSVEN.MassiveCancellationRow).Name);
            filter.Equal();
            filter.Constant(false);
            filter.And();
            filter.Property(MSVEN.MassiveCancellationRow.Properties.HasEvents, typeof(MSVEN.MassiveCancellationRow).Name);
            filter.Equal();
            filter.Constant(false);
            filter.And();
            filter.Property(MSVEN.MassiveCancellationRow.Properties.RowNumber, typeof(MSVEN.MassiveCancellationRow).Name);
            filter.In().ListValue();
            foreach (int temporal in temps)
            {
                int rowNumber = temporal;
                //if (rowNumber > 0)
                //    rowNumber = rowNumber - 1;
                //if (rowNumber < 0)
                //    rowNumber = 0;
                filter.Constant(rowNumber);
            }
            filter.EndList();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(MSVEN.MassiveCancellationRow), filter.GetPredicate()));

            if (temps.Count == businessCollection.Count)
            {
                List<MassiveCancellationRow> massiveCancellationRows = ModelAssembler.CreateMassiveCancellationRows(businessCollection);

                foreach (MassiveCancellationRow massiveCancellationRow in massiveCancellationRows)
                {
                    massiveCancellationRow.HasError = true;
                    massiveCancellationRow.Observations = Resources.Errors.MessageExcludedTemporalMessage + " " + userName;
                    massiveCancellationRow.Status = MassiveLoadProcessStatus.Tariff;
                    UpdateMassiveCancellationRows(massiveCancellationRow);
                    if (deleteTemporal)
                    {
                        DelegateService.underwritingServiceCore.DeleteTemporalsByOperationId(massiveCancellationRow.Risk.Policy.Id);
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }

    }
}