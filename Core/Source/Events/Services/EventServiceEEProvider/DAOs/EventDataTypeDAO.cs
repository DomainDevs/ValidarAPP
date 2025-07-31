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
    public class EventDataTypeDAO
    {
        /// <summary>
        /// obtiene los tipos de datos
        /// </summary>
        /// <returns></returns>
        public List<Models.EventDataType> GetDataTypes()
        {
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(EVENTEN.CoEventDataType)));
                return ModelAssembler.CreateListDataType(businessCollection).OrderBy(x => x.Description).ToList();
            }
            catch (Exception ex)
            {
                throw new BusinessException("Excepcion en: GetDataTypes", ex);
            }
        }
    }
}
