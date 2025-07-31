using Sistran.Core.Application.EventsServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Linq;
using EVENTEN = Sistran.Core.Application.Events.Entities;

namespace Sistran.Core.Application.EventsServices.EEProvider.DAOs
{
    public class EventLevelsDAO
    {
        /// <summary>
        /// obtiene la lista de niveles
        /// </summary>
        /// <returns></returns>
        public List<Models.EventLevels> GetLevels()
        {
            try
            {
                //BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Entity.CoEventLevels)));
                BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(EVENTEN.CoEventLevels));

                return ModelAssembler.CreateListEventLevels(businessCollection).OrderBy(x => x.LevelId).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetLevels", ex);
            }
        }
    }
}
