using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using MOUP = Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENUP = Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.UniquePersonV1.Entities;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class PhoneBusiness
    {
        public List<MOUP.Phone> CreatePhones(int individualId, List<MOUP.Phone> phones)
        {
            foreach (MOUP.Phone phone in phones)
            {
                CreatePhone(individualId, phone);
            }

            return phones;
        }

        public MOUP.Phone CreatePhone(int individualId, MOUP.Phone phone)
        {
            ENUP.Phone entityPhone = EntityAssembler.CreatePhone(phone, individualId);
            entityPhone = DataFacadeManager.Insert(entityPhone) as ENUP.Phone;
            phone.Id = entityPhone.DataId;
            return ModelAssembler.CreatePhone(entityPhone);
        }

        public List<MOUP.Phone> UpdatePhones(int individualId, List<MOUP.Phone> phones)
        {
            foreach (MOUP.Phone phone in phones)
            {
                if(phone.Id==0)
                    CreatePhone(individualId, phone);
                else
                    UpdatePhone(individualId, phone);
            }

            return GetPhones(individualId);
        }

        public MOUP.Phone UpdatePhone(int individualId, MOUP.Phone phone)
        {
            var primaryKey = UniquePersonV1.Entities.Phone.CreatePrimaryKey(individualId, phone.Id);
            ENUP.Phone entityPhone = (ENUP.Phone)DataFacadeManager.GetObject(primaryKey);
            
            entityPhone.PhoneTypeCode = phone.PhoneType.Id;
            entityPhone.PhoneNumber = Convert.ToInt64(phone.Description);
            entityPhone.IsMain = phone.IsMain;
            entityPhone.Extension = phone.Extension;
            entityPhone.CountryCode = phone.CountryCode;
            entityPhone.CityCode = phone.CityCode;
            entityPhone.ScheduleAvailability = phone.ScheduleAvailability;

            DataFacadeManager.Update(entityPhone);
            return ModelAssembler.CreatePhone(entityPhone);
        }

        public List<Models.Phone> GetPhones(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Phone.Properties.IndividualId, typeof(Phone).Name);
            filter.Equal();
            filter.Constant(individualId);

            var businessCollection = DataFacadeManager.GetObjects(typeof(Phone), filter.GetPredicate());

            List<MOUP.Phone> phones = ModelAssembler.CreatePhones(businessCollection);

            businessCollection = DataFacadeManager.GetObjects(typeof(PhoneType));

            List<MOUP.PhoneType> phoneTypes = ModelAssembler.CreatePhoneTypes(businessCollection);

            foreach (MOUP.Phone item in phones)
            {
                if (item.PhoneType.Id != 0)
                {
                    item.PhoneType = phoneTypes.First(x => x.Id == item.PhoneType.Id);
                }
            }
            return phones;
        }

        public List<Models.PhoneType> GetPhoneTypes()
        {
            return ModelAssembler.CreatePhoneTypes(DataFacadeManager.GetObjects(typeof(UniquePersonV1.Entities.PhoneType)));
        }
    }
}