using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using MOUP = Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENUP = Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonV1.Entities;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class AddressBusiness
    {
        public List<MOUP.Address> CreateAddresses(int individualId, List<MOUP.Address> addresses)
        {
            foreach (MOUP.Address address in addresses)
            {
                address.Id = CreateAddress(individualId, address).Id;
            }

            return addresses;
        }

        public MOUP.Address CreateAddress(int individualId, MOUP.Address address)
        {
            ENUP.Address entityAddress = EntityAssembler.CreateAddress(address);
            entityAddress.IndividualId = individualId;
            entityAddress = DataFacadeManager.Insert(entityAddress) as ENUP.Address;
            return ModelAssembler.CreateAddress(entityAddress);
        }

        public List<MOUP.Address> UpdateAddresses(int individualId, List<MOUP.Address> addresses)
        {
            foreach (MOUP.Address address in addresses)
            {
                UpdateAddress(individualId, address);
            }

            return addresses;
        }

        public MOUP.Address UpdateAddress(int individualId, MOUP.Address address)
        {
            PrimaryKey primaryKey = UniquePersonV1.Entities.Address.CreatePrimaryKey(individualId,address.Id);
            ENUP.Address entityAddress = (ENUP.Address)DataFacadeManager.GetObject(primaryKey);

            entityAddress.AddressTypeCode = address.AddressType.Id;
            entityAddress.IsMailingAddress = address.IsPrincipal;
            entityAddress.StreetTypeCode = 1;
            entityAddress.Street = address.Description;
            entityAddress.CityCode = address.City.Id;
            entityAddress.StateCode = address.City.State.Id;
            entityAddress.CountryCode = address.City.State.Country.Id;
            DataFacadeManager.Update(entityAddress);
            return ModelAssembler.CreateAddress(entityAddress);
        }

        public List<Models.Address> GetAddresses(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Address.Properties.IndividualId, typeof(Address).Name);
            filter.Equal();
            filter.Constant(individualId);
            
            var businessCollection = DataFacadeManager.GetObjects(typeof(Address), filter.GetPredicate());

            List< MOUP.Address> addresses = ModelAssembler.CreateAddresses(businessCollection);
            
            businessCollection = DataFacadeManager.GetObjects(typeof(AddressType));

            List<MOUP.AddressType> addressTypes = ModelAssembler.CreateAddressTypes(businessCollection);

            foreach (MOUP.Address item in addresses)
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
            return addresses;
        }

        
    }
}