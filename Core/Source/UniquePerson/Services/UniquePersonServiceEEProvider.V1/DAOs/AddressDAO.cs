using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using models = Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    /// <summary>
    /// Direcciones
    /// </summary>
    public class AddressDAO
    {
        /// <summary>
        /// Obtener Tipos de Direccion
        /// </summary>
        /// <returns></returns>
        public List<Models.AddressType> GetAddressTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(AddressType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetAddressTypes");
            return ModelAssembler.CreateAddressTypes(businessCollection);
        }

        /// <summary>
        /// Crear Direccion
        /// </summary>
        /// <param name="address">Modelo Direccion</param>
        /// <param name="individualId">Individuo</param>
        /// <returns></returns>
        public Models.Address CreateAddress(Models.Address address, int individualId)
        {
            if (address.IsPrincipal)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(Address.Properties.IndividualId, typeof(Address).Name);
                filter.Equal();
                filter.Constant(individualId);
                filter.And();
                filter.Property(Address.Properties.IsMailingAddress, typeof(Address).Name);
                filter.Equal();
                filter.Constant(true);
                Address addressEntityUpdate = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    addressEntityUpdate = (Address)daf.List(typeof(Address), filter.GetPredicate()).FirstOrDefault();
                }
                if (addressEntityUpdate != null)
                {
                    addressEntityUpdate.IsMailingAddress = false;
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        daf.UpdateObject(addressEntityUpdate);
                    }
                }

            }

            Address addressEntity = EntityAssembler.CreateAddress(address);
            addressEntity.IndividualId = individualId;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.InsertObject(addressEntity);
            }
            return ModelAssembler.CreateAddress(addressEntity);
        }

        /// <summary>
        /// Obtener Direcciones
        /// </summary>
        /// <param name="individualId">Individuo</param>
        /// <returns></returns>
        public List<Models.Address> GetAddresses(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Address.Properties.IndividualId, typeof(Address).Name);
            filter.Equal();
            filter.Constant(individualId);
            BusinessCollection businessCollection = new BusinessCollection();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(Address), filter.GetPredicate()));
            }

            List<models.Address> addresses = ModelAssembler.CreateAddresses(businessCollection);
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(AddressType)));
            }
            List<models.AddressType> addressTypes = ModelAssembler.CreateAddressTypes(businessCollection);

            foreach (models.Address item in addresses)
            {
                if (item.City != null && item.City.Id != 0 && item.City.State.Id != 0 && item.City.State.Country.Id != 0)
                {
                    item.City = DelegateService.commonServiceCore.GetCityByCity(item.City);
                }

                if (item.AddressType.Id != 0)
                {
                    item.AddressType = addressTypes.First(x => x.Id == item.AddressType.Id);
                }

            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetAddresses");
            return addresses;
        }
        /// <summary>
        /// Obtener Direcciones
        /// </summary>
        /// <param name="filter">Filtro</param>
        /// <returns></returns>
        public List<Models.Address> GetAddressesByfilter(ObjectCriteriaBuilder filter)
        {


            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Address), filter.GetPredicate()));
            List<models.Address> addresses = ModelAssembler.CreateAddresses(businessCollection);

            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(AddressType)));
            List<models.AddressType> addressTypes = ModelAssembler.CreateAddressTypes(businessCollection);

            foreach (models.Address item in addresses)
            {
                if (item.City != null && item.City.Id != 0)
                {
                    item.City = DelegateService.commonServiceCore.GetCityByCity(item.City);
                }

                if (item.AddressType.Id != 0)
                {
                    item.AddressType = addressTypes.First(x => x.Id == item.AddressType.Id);
                }

            }

            return addresses;
        }

        /// <summary>
        /// Actualizar Direcciones
        /// </summary>
        /// <param name="address">Direccion</param>
        /// <param name="individualId">Individuo</param>
        /// <returns></returns>
        public Models.Address UpdateAddress(Models.Address address, int individualId)
        {

            if (address.IsPrincipal)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(Address.Properties.IndividualId, typeof(Address).Name);
                filter.Equal();
                filter.Constant(individualId);
                filter.And();
                filter.Property(Address.Properties.IsMailingAddress, typeof(Address).Name);
                filter.Equal();
                filter.Constant(true);
                Address addressEntityUpdate = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    addressEntityUpdate = (Address)daf.List(typeof(Address), filter.GetPredicate()).FirstOrDefault();
                }
                if (addressEntityUpdate != null)
                {
                    addressEntityUpdate.IsMailingAddress = false;
                    using (var daf = DataFacadeManager.Instance.GetDataFacade())
                    {
                        daf.UpdateObject(addressEntityUpdate);
                    }
                }
            }

            PrimaryKey key = Address.CreatePrimaryKey(individualId, address.Id);
            Address addressEntity = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                addressEntity = (Address)daf.GetObjectByPrimaryKey(key);
            }

            if (addressEntity != null)
            {
                addressEntity.AddressTypeCode = address.AddressType.Id;
                addressEntity.IsMailingAddress = address.IsPrincipal;
                addressEntity.Street = address.Description;
                addressEntity.CountryCode = address.City.State.Country.Id;
                

                if (address.City != null && address.City.State != null && address.City.State.Id != 0)
                {
                    addressEntity.StateCode = address.City.State.Id;
                    addressEntity.CityCode = address.City.Id;
                }
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.UpdateObject(addressEntity);
                }
                return ModelAssembler.CreateAddress(addressEntity);
            }
            else
            {
                return CreateAddress(address, individualId);
            }

        }
    }
}
