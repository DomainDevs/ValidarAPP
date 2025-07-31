using Sistran.Core.Application.Locations.EEProvider.Enums;
using System.Collections.Generic;
using Sistran.Core.Application.Locations.EEProvider.Assemblers;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Core.Application.Locations.EEProvider.DAOs
{
    public class NomenclatureAddressDAO
    {
        /// <summary>
        /// Obtener lista de subfijos
        /// </summary>
        /// <returns></returns>
        public List<Models.Suffix> GetSuffixes()
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(StreetType.Properties.ViaTypeCode, typeof(StreetType).Name);
            filter.Equal();
            filter.Constant(ViaTypes.Suffix);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(StreetType), filter.GetPredicate()));
            return ModelAssembler.CreateSuffixes(businessCollection);
        }

        /// <summary>
        /// Obtener lista de apartamentos o oficina
        /// </summary>
        /// <returns></returns>
        public List<Models.ApartmentOrOffice> GetApartmentsOrOffices()
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(StreetType.Properties.ViaTypeCode, typeof(StreetType).Name);
            filter.Equal();
            filter.Constant(ViaTypes.ApartmentOffice);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(StreetType), filter.GetPredicate()));
            return ModelAssembler.CreateApartmentsOrOffices(businessCollection);
        }

        /// <summary>
        /// Obtener lista de tipos de calle
        /// </summary>
        /// <returns></returns>
        public List<Models.RouteType> GetRouteTypes()
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(StreetType.Properties.ViaTypeCode, typeof(StreetType).Name);
            filter.Equal();
            filter.Constant(ViaTypes.RouteType);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(StreetType), filter.GetPredicate()));
            return ModelAssembler.CreateRouteTypes(businessCollection);
        }
    }
}
