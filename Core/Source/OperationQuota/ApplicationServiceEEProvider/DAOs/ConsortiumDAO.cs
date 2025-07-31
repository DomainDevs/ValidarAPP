using Sistran.Core.Application.OperationQuotaServices.EEProvider.Assemblers;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Enums;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.Consortium;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using Sistran.Core.Integration.OperationQuotaServices.DTOs.Consortium;

namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.DAOs
{
    public class ConsortiumDAO
    {
        /// <summary>
        /// Retorna el Id del Ultimo Registro de la tabla
        /// </summary>
        /// <returns></returns>
        public int GetConsortiumEventId()
        {
            string[] sortColumn = new string[1];
            sortColumn[0] = "-" + UPEN.ConsortiumEvent.Properties.ConsortiumEventCode;
            UPEN.ConsortiumEvent consortiumEvent = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPEN.ConsortiumEvent), sortColumn)).Cast<UPEN.ConsortiumEvent>().FirstOrDefault();
            return Convert.ToInt32(consortiumEvent?.ConsortiumEventCode);
        }
        /// <summary>
        /// Crea el consorcio
        /// </summary>
        /// <param name="consortiumEvent"></param>
        /// <returns></returns>
        public ConsortiumEvent CreateConsortiumEvent(ConsortiumEvent consortiumEvent)
        {
            consortiumEvent.payload = JsonHelper.SerializeObjectToJson(consortiumEvent.consortium);
            UPEN.ConsortiumEvent entityConsortiumEvent = EntityAssembler.CreateConsortiumEvent(consortiumEvent);
            DataFacadeManager.Insert(entityConsortiumEvent);
            consortiumEvent = ModelAssembler.CreateConsortiumEvent(entityConsortiumEvent);
            consortiumEvent.consortium = JsonHelper.DeserializeJson<Consortium>(consortiumEvent.payload);
            return consortiumEvent;
        }

        /// <summary>
        /// Asigna el individual al consorciado
        /// </summary>
        /// <param name="consortiumEvent"></param>
        /// <returns></returns>
        public ConsortiumEvent AssigendIndividualToConsotium(ConsortiumEvent consortiumEvent)
        {
            consortiumEvent.payload = JsonHelper.SerializeObjectToJson(consortiumEvent.Consortiumpartners);
            UPEN.ConsortiumEvent entityConsortiumEvent = EntityAssembler.CreateConsortiumEvent(consortiumEvent);
            DataFacadeManager.Insert(entityConsortiumEvent);
            return ModelAssembler.CreateConsortiumEvent(entityConsortiumEvent);
        }

        /// <summary>
        /// Recupera los Consorciados de un consorcio
        /// </summary>
        /// <param name="IndividualId"></param>
        /// <returns></returns>
        public List<ConsortiumEvent> GetParticipantConsortiumEventByConsortiumId(int consortiumId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(UPEN.ConsortiumEvent.Properties.ConsortiumId, typeof(UPEN.ConsortiumEvent).Name, consortiumId);
            string[] sortColumn = new string[1];
            sortColumn[0] = "-" + UPEN.ConsortiumEvent.Properties.IssueDate;
            BusinessCollection businessObjects = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPEN.ConsortiumEvent), filter.GetPredicate(), sortColumn)); //DataFacadeManager.GetObjects(typeof(UPEN.ConsortiumEvent), filter.GetPredicate());
            List<ConsortiumEvent> consortiumEvents = ModelAssembler.CreateConsortiumEvents(businessObjects);
            return consortiumEvents;
        }

        public List<ConsortiumEvent> GetParticipantConsortiumEventByIndividualId(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(UPEN.ConsortiumEvent.Properties.IndividualId, typeof(UPEN.ConsortiumEvent).Name, individualId);
            string[] sortColumn = new string[1];
            sortColumn[0] = "-" + UPEN.ConsortiumEvent.Properties.IssueDate;
            BusinessCollection businessObjects = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPEN.ConsortiumEvent), filter.GetPredicate(), sortColumn)); //DataFacadeManager.GetObjects(typeof(UPEN.ConsortiumEvent), filter.GetPredicate());
            List<ConsortiumEvent> consortiumEvents = ModelAssembler.CreateConsortiumEvents(businessObjects);
            return consortiumEvents;
        }


        /// <summary>
        /// Recupera la existencia de un individual como un consorcio
        /// </summary>
        /// <param name="IndividualId"></param>
        /// <returns></returns>
        public List<ConsortiumEvent> GetConsortiumExistsByConsortiumId(int consortiumId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(UPEN.ConsortiumEvent.Properties.ConsortiumId, typeof(UPEN.ConsortiumEvent).Name, consortiumId);
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(UPEN.ConsortiumEvent), filter.GetPredicate());
            List<ConsortiumEvent> consortiumEvents = ModelAssembler.CreateConsortiumEvents(businessObjects);
            consortiumEvents.RemoveAll(x => x.ConsortiumEventEventType == (int)EnumEventConsortium.DISABLED_INDIVIDUAL_TO_CONSORTIUM);
            return consortiumEvents;
        }


        public List<ConsortiumEvent> GetConsortiumExistsByConsortiumIdMember(int consortiumId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(UPEN.ConsortiumEvent.Properties.ConsortiumId, typeof(UPEN.ConsortiumEvent).Name, consortiumId);
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(UPEN.ConsortiumEvent), filter.GetPredicate());
            List<ConsortiumEvent> consortiumEvents = ModelAssembler.CreateConsortiumEvents(businessObjects);
            return consortiumEvents;
        }


        public List<ConsortiumEvent> GetParticipantConsortiumEventByIndividualIdByConsortiumId(int individualId, int consortiumId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(UPEN.ConsortiumEvent.Properties.IndividualId, typeof(UPEN.ConsortiumEvent).Name, individualId);
            filter.And();
            filter.PropertyEquals(UPEN.ConsortiumEvent.Properties.ConsortiumId, typeof(UPEN.ConsortiumEvent).Name, consortiumId);
            string[] sortColumn = new string[1];
            sortColumn[0] = "-" + UPEN.ConsortiumEvent.Properties.IssueDate;
            BusinessCollection businessObjects = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPEN.ConsortiumEvent), filter.GetPredicate(), sortColumn, 1)); //DataFacadeManager.GetObjects(typeof(UPEN.ConsortiumEvent), filter.GetPredicate());
            List<ConsortiumEvent> consortiumEvents = ModelAssembler.CreateConsortiumEvents(businessObjects);
            return consortiumEvents;
        }

        public List<ConsortiumEvent> GetConsortiums()
        {
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(UPEN.ConsortiumEvent));
            List<ConsortiumEvent> consortiumEvents = ModelAssembler.CreateConsortiumEvents(businessObjects);
            return consortiumEvents;
        }

        public List<RiskConsortium> GetRiskConsortium()
        {
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(ISSEN.RiskConsortium));
            List<RiskConsortium> riskConsortiums = businessObjects.CreateRiskConsortiums();
            return riskConsortiums;
        }
        /// <summary>
        /// Consulta Consorcios Poliza
        /// </summary>
        /// <param name="endorsementid"></param>
        /// <returns></returns>
        public List<RiskConsortium> GetRiskConsortiumbyPolicy(int endorsementid)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(ISSEN.RiskConsortium.Properties.EndorsementId, typeof(ISSEN.RiskConsortium).Name, endorsementid);
            BusinessCollection businessObjects = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(ISSEN.RiskConsortium), filter.GetPredicate()));
            List<RiskConsortium> riskConsortiums = businessObjects.CreateRiskConsortiums();
            return riskConsortiums;
        }
        /// <summary>
        /// Registro del consorciado 
        /// </summary>
        /// <param name="consortiumPolicy"></param>
        /// <returns></returns>
        public RiskConsortiumDTO InsertConsotiumPolicy(RiskConsortiumDTO consortiumPolicy)
        {
            ISSEN.RiskConsortium entityConsortiumPolicy = EntityAssembler.CreateConsortiumPolicyEvent(consortiumPolicy);
            DataFacadeManager.Insert(entityConsortiumPolicy);
            return consortiumPolicy;
        }

    }
}
