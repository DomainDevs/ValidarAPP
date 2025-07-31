using Sistran.Core.Application.EventsServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Linq;
using Model = Sistran.Core.Application.EventsServices.Models;
using EVENTEN = Sistran.Core.Application.Events.Entities;

namespace Sistran.Core.Application.EventsServices.EEProvider.DAOs
{
    public class EventQueryTypeDAO
    {
        /// <summary>
        /// consulta los tipos de consultas
        /// </summary>
        /// <returns></returns>
        public List<Model.EventQueryType> GetQueryTypesCode()
        {
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EVENTEN.CoEventQueryType)));
                return ModelAssembler.CreateEventQueryType(businessCollection).OrderBy(x => x.QueryTypeCode).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetQueryTypesCode", ex);
            }
        }
    }
}
