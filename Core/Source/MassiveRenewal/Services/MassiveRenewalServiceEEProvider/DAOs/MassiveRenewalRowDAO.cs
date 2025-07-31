using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.MassiveRenewalServices.EEProvider.Assemblers;
using Sistran.Core.Application.MassiveRenewalServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using ENT = Sistran.Core.Application.Massive.Entities;
using Sistran.Core.Application.MassiveUnderwritingServices.Enums;
using Sistran.Core.Framework;
using Sistran.Core.Application.MassiveRenewalServices.EEProvider.Resources;

namespace Sistran.Core.Application.MassiveRenewalServices.EEProvider.DAOs
{
    public class MassiveRenewalRowDAO
    {
        public MassiveRenewalRow CreateMassiveRenewalRow(MassiveRenewalRow massiveRenewalRow)
        {
            ENT.MassiveRenewalRow massiveRenewalRowEntity = EntityAssembler.CreateMassiveRenewalRow(massiveRenewalRow);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(massiveRenewalRowEntity);
            if (massiveRenewalRowEntity == null)
            {
                return null;
            }
            massiveRenewalRow.Id = massiveRenewalRowEntity.Id;
            return massiveRenewalRow;
        }

        public MassiveRenewalRow UpdateMassiveRenewalRow(MassiveRenewalRow massiveRenewalRowModel)
        {
            PrimaryKey primaryKey = ENT.MassiveRenewalRow.CreatePrimaryKey(massiveRenewalRowModel.Id);
            ENT.MassiveRenewalRow massiveRenewalRow = (ENT.MassiveRenewalRow)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            if (massiveRenewalRow == null)
            {
                return null;
            }
            massiveRenewalRow.HasError = massiveRenewalRowModel.HasError;
            massiveRenewalRow.MassiveLoadId = massiveRenewalRowModel.MassiveRenewalId;
            massiveRenewalRow.Observations = massiveRenewalRowModel.Observations;
            if (massiveRenewalRowModel.Risk != null && massiveRenewalRowModel.Risk.Policy != null)
            {
                massiveRenewalRow.PolicyId = massiveRenewalRowModel.Risk.Policy.Id;
                massiveRenewalRow.DocumentNumber = massiveRenewalRowModel.Risk.Policy.DocumentNumber;
                massiveRenewalRow.RiskDescription = massiveRenewalRowModel.Risk.Description;

                if (massiveRenewalRowModel.Risk.Policy.Summary != null)
                {
                    massiveRenewalRow.Premium = massiveRenewalRowModel.Risk.Policy.Summary.FullPremium;
                }

                if (massiveRenewalRowModel.Risk.Policy.PolicyType != null)
                {
                    massiveRenewalRow.PolicyType = massiveRenewalRowModel.Risk.Policy.PolicyType.Description;
                }
                if (massiveRenewalRowModel.Risk.Policy.Endorsement != null)
                {
                    massiveRenewalRow.EndorsementId = massiveRenewalRowModel.Risk.Policy.Endorsement.Id;
                }
            }
            massiveRenewalRow.Commiss = massiveRenewalRowModel.TotalCommission;
            massiveRenewalRow.HasEvents = massiveRenewalRowModel.HasEvents;

            massiveRenewalRow.RowNumber = massiveRenewalRowModel.RowNumber;
            massiveRenewalRow.SerializedRow = massiveRenewalRowModel.SerializedRow;
            massiveRenewalRow.StatusId = (int?)massiveRenewalRowModel.Status;
            massiveRenewalRow.TempId = massiveRenewalRowModel.TemporalId;
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(massiveRenewalRow);
            return massiveRenewalRowModel;
        }

        public List<MassiveRenewalRow> GetMassiveLoadProcessByMassiveRenewalProcessId(int massiveRenewalId, MassiveLoadProcessStatus massiveLoadProcessStatus)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ENT.MassiveRenewalRow.Properties.MassiveLoadId, typeof(ENT.MassiveRenewalRow).Name);
            filter.Equal();
            filter.Constant(massiveRenewalId);
            filter.And();
            filter.Property(ENT.MassiveRenewalRow.Properties.StatusId, typeof(ENT.MassiveRenewalRow).Name);
            filter.Equal();
            filter.Constant(massiveLoadProcessStatus);
            filter.And();
            filter.Property(ENT.MassiveRenewalRow.Properties.HasError, typeof(ENT.MassiveRenewalRow).Name);
            filter.Equal();
            filter.Constant(false);
            filter.And();
            filter.Property(ENT.MassiveRenewalRow.Properties.HasEvents, typeof(ENT.MassiveRenewalRow).Name);
            filter.Equal();
            filter.Constant(false);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ENT.MassiveRenewalRow), filter.GetPredicate()));
            List<MassiveRenewalRow> massiveLoadProcesses = businessCollection.Select(x => ModelAssembler.CreateMassiveRenewalRow((ENT.MassiveRenewalRow)x)).ToList();
            return massiveLoadProcesses;
        }

        /// <summary>
        /// Excluye y elimina los temporales del cargue
        /// </summary>
        /// <param name="massiveRenewalId">Id del Cargue</param>
        /// <param name="temps">Lista de temporales</param>
        /// <param name="userName">Nombre del Usuario</param>
        public bool ExcludeMassiveRenewalRowsTemporals(int massiveRenewalId, List<int> temps, string userName)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ENT.MassiveRenewalRow.Properties.MassiveLoadId, typeof(ENT.MassiveRenewalRow).Name);
            filter.Equal();
            filter.Constant(massiveRenewalId);
            filter.And();
            filter.Property(ENT.MassiveRenewalRow.Properties.HasError, typeof(ENT.MassiveRenewalRow).Name);
            filter.Equal();
            filter.Constant(false);
            filter.And();
            filter.Property(ENT.MassiveRenewalRow.Properties.RowNumber, typeof(ENT.MassiveRenewalRow).Name);
            filter.In().ListValue();
            foreach (int temporal in temps)
            {
                filter.Constant(temporal);
            }
            filter.EndList();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ENT.MassiveRenewalRow), filter.GetPredicate()));
            if (businessCollection.Count > 0)
            {
                List<MassiveRenewalRow> massiveRenewalRows = ModelAssembler.CreateMassiveRenewalRows(businessCollection);
                massiveRenewalRows.RemoveAll(u => u.HasEvents);
                if (massiveRenewalRows.Count > 0)
                {
                    foreach (MassiveRenewalRow massiveRenewalRow in massiveRenewalRows)
                    {
                        massiveRenewalRow.HasError = true;
                        massiveRenewalRow.Observations = Resources.Errors.ExcludedTemporalMessage + " " + userName;
                        UpdateMassiveRenewalRow(massiveRenewalRow);
                        DelegateService.underwritingServiceCore.DeleteTemporalsByOperationId(massiveRenewalRow.TemporalId.GetValueOrDefault());
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public List<MassiveRenewalRow> GetMassiveRenewalRowsByMassiveLoadIdMassiveLoadProcessStatus(int massiveLoadProcessId, MassiveLoadProcessStatus? massiveLoadProcessStatus, bool? withError, bool? withEvent)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ENT.MassiveRenewalRow.Properties.MassiveLoadId, typeof(ENT.MassiveRenewalRow).Name);
            filter.Equal();
            filter.Constant(massiveLoadProcessId);

            if (massiveLoadProcessStatus.HasValue)
            {
                filter.And();
                filter.Property(ENT.MassiveRenewalRow.Properties.StatusId, typeof(ENT.MassiveRenewalRow).Name);
                filter.Equal();
                filter.Constant(massiveLoadProcessStatus.Value);
            }

            if (withError.HasValue && withError.Value && withEvent.HasValue && withEvent.Value)
            {
                filter.And();
                filter.OpenParenthesis();
                filter.Property(ENT.MassiveRenewalRow.Properties.HasError, typeof(ENT.MassiveRenewalRow).Name);
                filter.Equal();
                filter.Constant(withError.Value);
                filter.Or();
                filter.Property(ENT.MassiveRenewalRow.Properties.HasEvents, typeof(ENT.MassiveRenewalRow).Name);
                filter.Equal();
                filter.Constant(withEvent.Value);
                filter.CloseParenthesis();
            }
            else
            {

                if (withError.HasValue)
                {
                    filter.And();
                    filter.Property(ENT.MassiveRenewalRow.Properties.HasError, typeof(ENT.MassiveRenewalRow).Name);
                    filter.Equal();
                    filter.Constant(withError.Value);
                }

                if (withEvent.HasValue)
                {
                    filter.And();
                    filter.Property(ENT.MassiveRenewalRow.Properties.HasEvents, typeof(ENT.MassiveRenewalRow).Name);
                    filter.Equal();
                    filter.Constant(withEvent.Value);
                }
            }


            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ENT.MassiveRenewalRow), filter.GetPredicate()));
            List<MassiveRenewalRow> massiveRenewalRows = businessCollection.Select(x => ModelAssembler.CreateMassiveRenewalRow((ENT.MassiveRenewalRow)x)).ToList();

            return massiveRenewalRows;
        }


    }
}
