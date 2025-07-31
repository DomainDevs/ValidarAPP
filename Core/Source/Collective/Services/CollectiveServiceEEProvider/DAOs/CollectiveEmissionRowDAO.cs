using Sistran.Core.Application.CollectiveServices.Models;
using MSVEN = Sistran.Core.Application.Massive.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.CollectiveServices.EEProvider.Assemblers;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.CollectiveServices.Enums;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using System.Linq;
using Newtonsoft.Json;
using AutoMapper;
using Sistran.Core.Services.UtilitiesServices.Models;

namespace Sistran.Core.Application.CollectiveServices.EEProvider.DAOs
{
    public class CollectiveEmissionRowDAO
    {

        /// <summary>
        /// Crear Proceso De Cargue de colectivas
        /// </summary>
        /// <param name="collectiveLoadProcess">Proceso De Cargue Masivo</param>
        /// <returns>Proceso De Cargue Masivo</returns>
        public CollectiveEmissionRow CreateCollectiveEmissionRow(CollectiveEmissionRow collectiveEmissionRow)
        {
            MSVEN.CollectiveEmissionRow entityCollectiveLoadProcess = EntityAssembler.CreateCollectiveEmissionRow(collectiveEmissionRow);

            DataFacadeManager.Instance.GetDataFacade().InsertObject(entityCollectiveLoadProcess);

            if (entityCollectiveLoadProcess != null)
            {
                collectiveEmissionRow.Id = entityCollectiveLoadProcess.Id;
                return collectiveEmissionRow;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Actualizar Proceso De Cargue colectivo
        /// </summary>
        /// <param name="collectiveLoadProcess">Proceso De Cargue Masivo</param>
        /// <returns>Proceso De Cargue Masivo</returns>
        public CollectiveEmissionRow UpdateCollectiveEmissionRow(CollectiveEmissionRow collectiveEmissionRow)
        {
            PrimaryKey primaryKey = MSVEN.CollectiveEmissionRow.CreatePrimaryKey(collectiveEmissionRow.Id);
            MSVEN.CollectiveEmissionRow entityColletiveEmissionRow = (MSVEN.CollectiveEmissionRow)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

            if (entityColletiveEmissionRow != null)
            {
                entityColletiveEmissionRow.StatusId = (int)collectiveEmissionRow.Status;
                entityColletiveEmissionRow.RiskId = collectiveEmissionRow.Risk == null ? 0 : collectiveEmissionRow.Risk.RiskId;
                entityColletiveEmissionRow.HasError = collectiveEmissionRow.HasError;
                entityColletiveEmissionRow.Observations = collectiveEmissionRow.Observations;
                entityColletiveEmissionRow.SerializedRow = collectiveEmissionRow.SerializedRow;
                entityColletiveEmissionRow.MassiveLoadId = collectiveEmissionRow.MassiveLoadId;
                entityColletiveEmissionRow.RiskDescription = collectiveEmissionRow.Risk == null ? "" : collectiveEmissionRow.Risk.Description;
                entityColletiveEmissionRow.Premium = collectiveEmissionRow.Premium;
                entityColletiveEmissionRow.RowNumber = collectiveEmissionRow.RowNumber;
                entityColletiveEmissionRow.HasEvents = collectiveEmissionRow.HasEvents;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityColletiveEmissionRow);

                return collectiveEmissionRow;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Actualizar Proceso De Cargue coletivo
        /// </summary>
        /// <param name="collectiveLoadProcess">Proceso De Cargue Masivo</param>
        /// <returns>Proceso De Cargue Masivo</returns>
        public List<CollectiveEmissionRow> GetCollectiveEmissionRowByMassiveLoadId(int collectiveLoadId, CollectiveLoadProcessStatus collectiveLoadProcessStatus)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(MSVEN.CollectiveEmissionRow.Properties.MassiveLoadId, typeof(MSVEN.CollectiveEmissionRow).Name);
            filter.Equal();
            filter.Constant(collectiveLoadId);
            filter.And();
            filter.Property(MSVEN.CollectiveEmissionRow.Properties.StatusId, typeof(MSVEN.CollectiveEmissionRow).Name);
            filter.Equal();
            filter.Constant(collectiveLoadProcessStatus);
            filter.And();
            filter.Property(MSVEN.CollectiveEmissionRow.Properties.HasError, typeof(MSVEN.CollectiveEmissionRow).Name);
            filter.Equal();
            filter.Constant(false);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(MSVEN.CollectiveEmissionRow), filter.GetPredicate()));
            List<CollectiveEmissionRow> collectiveEmissionRows = ModelAssembler.CreateCollectiveEmissionRow(businessCollection);

            return collectiveEmissionRows;
        }
        
        /// <summary>
        /// Actualizar Proceso De Cargue colectivo
        /// </summary>
        /// <param name="collectiveLoadProcess">Proceso De Cargue Masivo</param>
        /// <returns>Proceso De Cargue Masivo</returns>
        public List<CollectiveEmissionRow> GetCollectiveEmissionRowByMassiveLoadIdCollectiveLoadProcessStatus(int collectiveLoadId, CollectiveLoadProcessStatus? collectiveLoadProcessStatus, bool? withError, bool? withEvent)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(MSVEN.CollectiveEmissionRow.Properties.MassiveLoadId, typeof(MSVEN.CollectiveEmissionRow).Name);
            filter.Equal();
            filter.Constant(collectiveLoadId);

            if (collectiveLoadProcessStatus.HasValue)
            {
                filter.And();
                filter.Property(MSVEN.CollectiveEmissionRow.Properties.StatusId, typeof(MSVEN.CollectiveEmissionRow).Name);
                filter.Equal();
                filter.Constant(collectiveLoadProcessStatus.Value);
            }

            if (withError.HasValue)
            {
                filter.And();
                filter.Property(MSVEN.CollectiveEmissionRow.Properties.HasError, typeof(MSVEN.CollectiveEmissionRow).Name);
                filter.Equal();
                filter.Constant(withError.Value);
            }

            if (withEvent.HasValue)
            {
                filter.And();
                filter.Property(MSVEN.CollectiveEmissionRow.Properties.HasEvents, typeof(MSVEN.CollectiveEmissionRow).Name);
                filter.Equal();
                filter.Constant(withEvent.Value);
            }


            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(MSVEN.CollectiveEmissionRow), filter.GetPredicate()));
            List<CollectiveEmissionRow> collectiveEmissionRows = ModelAssembler.CreateCollectiveEmissionRows(businessCollection);

            return collectiveEmissionRows;
        }


        /// <summary>
        /// Consulta un EmissionCollectiveRow
        /// </summary>
        /// <param name="id"></param>
        /// <returns>CollectiveEmissionRow</returns>
        public CollectiveEmissionRow GetCollectiveEmissionRowById(int id)
        {
            PrimaryKey primaryKey = MSVEN.CollectiveEmissionRow.CreatePrimaryKey(id);
            MSVEN.CollectiveEmissionRow entityCollectiveEmissionRow = (MSVEN.CollectiveEmissionRow)DataFacadeManager.GetObject(primaryKey);
            return ModelAssembler.CreateCollectiveEmissionRow(entityCollectiveEmissionRow);
        }

    }
}

