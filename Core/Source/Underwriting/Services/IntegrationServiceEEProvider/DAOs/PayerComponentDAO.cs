using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Integration.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using System.Collections.Generic;
using System.Linq;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
namespace Sistran.Core.Integration.UnderwritingServices.EEProvider.DAOs
{
    public class PayerComponentDAO
    {
        public static List<ComponentTypeDTO> GetComponentTypes()
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(QUOEN.Component.Properties.ComponentTypeCode, typeof(QUOEN.Component).Name);
            filter.In();
            filter.ListValue();
            filter.Constant(ComponentType.Premium);
            filter.Constant(ComponentType.Expenses);
            filter.Constant(ComponentType.Taxes);
            filter.Constant(ComponentType.Surcharges);
            filter.Constant(ComponentType.Discounts);
            filter.EndList();
            List<QUOEN.Component> components = DataFacadeManager.Instance.GetDataFacade().List<QUOEN.Component>(filter.GetPredicate()).Cast<QUOEN.Component>().ToList();
            return DTOAssembler.CreateComponents(components);
        }
    }
}
