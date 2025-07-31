using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using models = Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    public class PhoneDAO
    {
        /// <summary>
        /// Obtener lista de tipos de teléfono
        /// </summary>
        /// <returns></returns>
        public List<models.PhoneType> GetPhoneTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PhoneType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetPhoneTypes");
            return ModelAssembler.CreatePhoneTypes(businessCollection);
        }

        /// <summary>
        /// Obtener lista de teléfonos asociados a un individuo
        /// </summary>
        /// <param name="individualId">Id del individuo</param>
        /// <returns></returns>
        public List<models.Phone> GetPhonesByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Phone.Properties.IndividualId, typeof(Phone).Name);
            filter.Equal();
            filter.Constant(individualId);
            BusinessCollection businessCollection = new BusinessCollection();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(Phone), filter.GetPredicate()));
            }

            List<models.Phone> phones = ModelAssembler.CreatePhones(businessCollection);
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(PhoneType)));
            }
            List<models.PhoneType> phoneTypes = ModelAssembler.CreatePhoneTypes(businessCollection);
            foreach (models.Phone item in phones)
            {
                if (item.PhoneType.Id != 0)
                {
                    item.PhoneType = phoneTypes.First(x => x.Id == item.PhoneType.Id);
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetPhonesByIndividualId");
            return phones;
        }

        /// <summary>
        /// Obtener lista de teléfonos por un filtro especifico
        /// </summary>
        /// <param name="filter">Filtro</param>
        /// <returns></returns>
        public List<models.Phone> GetPhonesByFilter(ObjectCriteriaBuilder filter)
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Phone), filter.GetPredicate()));

            List<models.Phone> phones = ModelAssembler.CreatePhones(businessCollection);
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(PhoneType)));
            }
            List<models.PhoneType> phoneTypes = ModelAssembler.CreatePhoneTypes(businessCollection);

            foreach (models.Phone item in phones)
            {
                if (item.PhoneType.Id != 0)
                {
                    item.PhoneType = phoneTypes.First(x => x.Id == item.PhoneType.Id);
                }
            }
            return phones;
        }

        /// <summary>
        /// Obtener lista de tipos de email
        /// </summary>
        /// <returns></returns>
        public models.Phone CreatePhone(models.Phone phone, int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Phone.Properties.IndividualId, typeof(Phone).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(Phone.Properties.PhoneNumber, typeof(Phone).Name);
            filter.Equal();
            filter.Constant(phone.Description);

            List<models.Phone> phones = GetPhonesByFilter(filter);
            if (phones.Count == 0)
            {
                Phone phoneEntity = EntityAssembler.CreatePhone(phone, individualId);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.InsertObject(phoneEntity);
                }
                return ModelAssembler.CreatePhone(phoneEntity);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Actualizar teléfono
        /// </summary>
        /// <param name="phone">Modelo teléfono</param>
        /// <returns></returns>
        public models.Phone UpdatePhone(models.Phone phone, int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Phone.Properties.IndividualId, typeof(Phone).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(Phone.Properties.PhoneNumber, typeof(Phone).Name);
            filter.Equal();
            filter.Constant(phone.Description);
            filter.And();
            filter.Not();
            filter.Property(Phone.Properties.DataId, typeof(Phone).Name);
            filter.Equal();
            filter.Constant(phone.Id);

            List<models.Phone> phones = GetPhonesByFilter(filter);
            if (phones.Count == 0)
            {
                PrimaryKey key = Phone.CreatePrimaryKey(individualId, phone.Id);
                Phone phoneEntity = new Phone();
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    phoneEntity = (Phone)daf.GetObjectByPrimaryKey(key);
                }
                phoneEntity.PhoneTypeCode = phone.PhoneType.Id;
                phoneEntity.PhoneNumber = Convert.ToInt64(phone.Description);
                phoneEntity.IsMain = phone.IsMain;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.UpdateObject(phoneEntity);
                }
                return ModelAssembler.CreatePhone(phoneEntity);
            }
            else
            {
                return null;
            }
        }
    }
}
