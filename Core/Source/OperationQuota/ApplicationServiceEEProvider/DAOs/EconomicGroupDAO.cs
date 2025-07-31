using Sistran.Core.Application.OperationQuotaServices.EEProvider.Assemblers;
using Sistran.Core.Application.OperationQuotaServices.EEProvider.Models.EconomicGroup;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Framework.Queries;
using System.Threading.Tasks;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;
namespace Sistran.Core.Application.OperationQuotaServices.EEProvider.DAOs
{
    public class EconomicGroupDAO
    {
        /// <summary>
        /// Retorna el Id del Ultimo Registro de la tabla
        /// </summary>
        /// <returns></returns>
        public int GetEconomicGroupEventId()
        {
            string[] sortColumn = new string[1];
            sortColumn[0] = "-" + UPEN.EconomicGroupEvent.Properties.EconomicGroupEventCode;
            UPEN.EconomicGroupEvent economicGroupEvent = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPEN.EconomicGroupEvent), sortColumn)).Cast<UPEN.EconomicGroupEvent>().FirstOrDefault();
            return Convert.ToInt32(economicGroupEvent?.EconomicGroupEventCode);
        }
        /// <summary>
        /// Crea el grupo economico
        /// </summary>
        /// <param name="economicGroupEvent"></param>
        /// <returns></returns>
        public EconomicGroupEvent CreateEconomicGroupEvent(EconomicGroupEvent economicGroupEvent)
        {
            economicGroupEvent.payload = JsonHelper.SerializeObjectToJson(economicGroupEvent.EconomicGroupOperatingQuota);
            UPEN.EconomicGroupEvent entityEconomicGroupEvent = EntityAssembler.CreateEconomicGroupEvent(economicGroupEvent);
            DataFacadeManager.Insert(entityEconomicGroupEvent);
            economicGroupEvent = ModelAssembler.CreateEconomicGroupEvent(entityEconomicGroupEvent);
            economicGroupEvent.EconomicGroupOperatingQuota = JsonHelper.DeserializeJson<Economicgroupoperatingquota>(economicGroupEvent.payload);
            return economicGroupEvent;
        }

        /// <summary>
        /// Asigna el individual al grupo economico
        /// </summary>
        /// <param name="consortiumEvent"></param>
        /// <returns></returns>
        public EconomicGroupEvent AssigendIndividualToEconomicGroupEvent(EconomicGroupEvent economicGroupEvent)
        {
            economicGroupEvent.payload = JsonHelper.SerializeObjectToJson(economicGroupEvent.EconomicGroupPartners);
            UPEN.EconomicGroupEvent entityeconomicGroupEvent = EntityAssembler.CreateEconomicGroupEvent(economicGroupEvent);
            DataFacadeManager.Insert(entityeconomicGroupEvent);
            return ModelAssembler.CreateEconomicGroupEvent(entityeconomicGroupEvent);
        }

        /// <summary>
        /// Recupera los participantes de un consorcio
        /// </summary>
        /// <param name="IndividualId"></param>
        /// <returns></returns>
        public List<EconomicGroupEvent> GetExistingGroupEconomicEventByeconomicGroupId(int economicGroupId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(UPEN.EconomicGroupEvent.Properties.EconomicGroupId, typeof(UPEN.EconomicGroupEvent).Name, economicGroupId);
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(UPEN.EconomicGroupEvent), filter.GetPredicate());
            List<EconomicGroupEvent> economicGroupEvents = ModelAssembler.CreateEconomicGroupEvents(businessObjects);

            return economicGroupEvents.GroupBy(x => x.IndividualId).Select(x => x.OrderByDescending(o => o.IssueDate).First()).ToList();
        }

        /// <summary>
        /// Valida si se encuentra el participante asignado a un grupo económico
        /// </summary>
        /// <param name="IndividualId"></param>
        /// <returns></returns>
        public EconomicGroupEvent GetExistingParticipantGroupEconomicEventByIndividualId(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(UPEN.EconomicGroupEvent.Properties.IndividualId, typeof(UPEN.EconomicGroupEvent).Name, individualId);
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(UPEN.EconomicGroupEvent), filter.GetPredicate());
            List<EconomicGroupEvent> economicGroupEvents = ModelAssembler.CreateEconomicGroupEvents(businessObjects);

            if (economicGroupEvents.Count > 0)
            {
                return economicGroupEvents.Last();
            }
            else {
                return new EconomicGroupEvent();
            }

            
        }

        /// <summary>
        /// Recupera los participantes de un consorcio
        /// </summary>
        /// <param name="IndividualId"></param>
        /// <returns></returns>
        public List<EconomicGroupEvent> GetEconomicGroups()
        {
            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(UPEN.EconomicGroupEvent));
            List<EconomicGroupEvent> economicGroupEvents = ModelAssembler.CreateEconomicGroupEvents(businessObjects);
            return economicGroupEvents;
        }

    }
}
