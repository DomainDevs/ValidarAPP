using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.UniquePersonService.Entities.views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MUp = Sistran.Core.Application.UniquePersonService.Models;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    public class ProviderDAO
    {
        /// <summary>
        /// Obtener lista de tipos de proveedores
        /// </summary>
        public List<MUp.ProviderType> GetProviderTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePerson.Entities.ProviderType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetProviderTypes");
            return ModelAssembler.CreateProviderTypes(businessCollection);
        }

        /// <summary>
        /// Obtener lista de tipos de proveedores declinados
        /// </summary>
        public List<MUp.ProviderDeclinedType> GetProviderDeclinedType()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePerson.Entities.ProviderDeclinedType)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetProviderDeclinedType");
            return ModelAssembler.CreateProviderDeclinedTypes(businessCollection);
        }

        /// <summary>
        /// Crear información basica de proveedor
        /// </summary>
        public MUp.Provider CreateProviderInformationBasic(MUp.Provider provider)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            UniquePerson.Entities.Provider providerEntity = EntityAssembler.CreateProvider(provider);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(providerEntity);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.CreateProviderInformationBasic");
            return ModelAssembler.CreateProvider(providerEntity);
        }

        /// <summary>
        /// Crear información basica de proveedor
        /// </summary>
        public List<MUp.ProviderSpeciality> CreateProviderSpeciality(MUp.Provider provider, int providerId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<MUp.ProviderSpeciality> providerSpeciality = new List<MUp.ProviderSpeciality>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(UniquePerson.Entities.ProviderSpeciality.Properties.ProviderCode, providerId);
            DataFacadeManager.Instance.GetDataFacade().Delete<UniquePerson.Entities.ProviderSpeciality>(filter.GetPredicate());
            if (provider.ProviderSpeciality != null)
            {
                foreach (var item in provider.ProviderSpeciality)
                {
                    item.ProviderId = providerId;
                    UniquePerson.Entities.ProviderSpeciality providerSpecialityEnt = EntityAssembler.CreateProviderSpeciality(item);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(providerSpecialityEnt);
                    providerSpeciality.Add(ModelAssembler.CreateProviderSpeciality(providerSpecialityEnt));
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.CreateProviderSpeciality");
            return providerSpeciality;
        }

        /// <summary>
        /// Crear concepto de pago de proveedor
        /// </summary>
        public List<MUp.ProviderPaymentConcept> CreateProviderPaymentConcept(MUp.Provider provider, int providerId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<MUp.ProviderPaymentConcept> providerPaymentConcept = new List<MUp.ProviderPaymentConcept>();
            List<PaymentConcept> PaymentConcepts = DelegateService.commonServiceCore.GetPaymentConcept();
            for (int i = 0; i < provider.ProviderPaymentConcept.Count; i++)
            {
                if (provider.ProviderPaymentConcept[i].Id == 0)
                {
                    provider.ProviderPaymentConcept[i].ProviderId = providerId;
                    UniquePerson.Entities.ProviderPaymentConcept providerPaymentConceptEnt = EntityAssembler.CreateProviderPaymentConcept(provider.ProviderPaymentConcept[i]);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(providerPaymentConceptEnt);
                    string descriptionConceptPayment = PaymentConcepts.Where(b => b.Id == provider.ProviderPaymentConcept[i].PaymentConcept.Id).FirstOrDefault().Description;
                    providerPaymentConcept.Add(ModelAssembler.CreateProviderPaymentConceptByDescription(providerPaymentConceptEnt, descriptionConceptPayment));
                }
                else if (provider.ProviderPaymentConcept[i].PaymentConcept.Id < 0)
                {
                    PrimaryKey primaryKey = UniquePerson.Entities.ProviderPaymentConcept.CreatePrimaryKey(provider.ProviderPaymentConcept[i].Id);
                    UniquePerson.Entities.ProviderPaymentConcept providerPaymentConceptEnt = (UniquePerson.Entities.ProviderPaymentConcept)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
                    if (providerPaymentConcept != null)
                    {
                        DataFacadeManager.Instance.GetDataFacade().DeleteObject(providerPaymentConceptEnt);
                    }
                }
                else
                {
                    providerPaymentConcept.Add(provider.ProviderPaymentConcept[i]);
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.CreateProviderPaymentConcept");
            return providerPaymentConcept;
        }

        /// <summary>
        /// Crear nuevo proveedor
        /// </summary>
        /// <param name="provider">modelo provider</param>
        /// <returns></returns>
        public void UpdateProviderInformationBasic(MUp.Provider provider)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = UniquePerson.Entities.Provider.CreatePrimaryKey(provider.Id);
            UniquePerson.Entities.Provider providerEntity = (UniquePerson.Entities.Provider)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            if (providerEntity != null)
            {
                providerEntity.ProviderTypeCode = provider.ProviderTypeId;
                providerEntity.OriginTypeCode = provider.OriginTypeId;
                providerEntity.ProviderDeclinedTypeCode = provider.ProviderDeclinedTypeId;
                providerEntity.Observation = provider.Observation;
                providerEntity.ModificationDate = provider.ModificationDate;
                providerEntity.DeclinationDate = provider.DeclinationDate;
                providerEntity.SpecialityDefault = provider.SpecialityDefault;
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.UpdateProviderInformationBasic");
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(providerEntity);
        }

        /// <summary>
        /// Consultar proveedor por individualID
        /// </summary>
        public MUp.Provider GetProviderByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            MUp.Provider provider = new MUp.Provider();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePerson.Entities.Provider.Properties.IndividualId, typeof(UniquePerson.Entities.Provider).Name);
            filter.Equal();
            filter.Constant(individualId);
            BusinessCollection businessCollection = new BusinessCollection();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(UniquePerson.Entities.Provider), filter.GetPredicate()));
            }

            provider = ModelAssembler.CreateProviders(businessCollection).FirstOrDefault();

            if (provider != null)
            {
                provider.ProviderSpeciality = new List<MUp.ProviderSpeciality>();
                filter = new ObjectCriteriaBuilder();
                filter.Property(UniquePerson.Entities.ProviderSpeciality.Properties.ProviderCode, typeof(UniquePerson.Entities.ProviderSpeciality).Name);
                filter.Equal();
                filter.Constant(provider.Id);
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessCollection = new BusinessCollection(daf.SelectObjects(typeof(UniquePerson.Entities.ProviderSpeciality), filter.GetPredicate()));
                }
                provider.ProviderSpeciality = ModelAssembler.CreateProviderSpecialitys(businessCollection);

                provider.ProviderPaymentConcept = new List<MUp.ProviderPaymentConcept>();
                ProviderPaymentConceptView view = new ProviderPaymentConceptView();
                ViewBuilder builder = new ViewBuilder("ProviderPaymentConceptView");
                filter = new ObjectCriteriaBuilder();
                filter.Property(UniquePerson.Entities.ProviderPaymentConcept.Properties.ProviderCode, typeof(UniquePerson.Entities.ProviderPaymentConcept).Name);
                filter.Equal();
                filter.Constant(provider.Id);
                builder.Filter = filter.GetPredicate();
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                   daf.FillView(builder, view);
                }
                provider.ProviderPaymentConcept = ModelAssembler.CreateProviderPaymentConcepts(view);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetProviderByIndividualId");
            return provider;
        }

        /// <summary>
        /// Crear nuevo proveedor
        /// </summary>
        /// <param name="provider">modelo provider</param>
        /// <returns></returns>
        public MUp.Provider CreateProvider(MUp.Provider provider)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Models.Provider providerPerson = CreateProviderInformationBasic(provider);
            providerPerson.ProviderSpeciality = CreateProviderSpeciality(provider, providerPerson.Id);
            providerPerson.ProviderPaymentConcept = CreateProviderPaymentConcept(provider, providerPerson.Id);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.CreateProvider");
            return providerPerson;
        }

        /// <summary>
        /// Actualizar proveedor
        /// </summary>        
        public MUp.Provider UpdateProvider(MUp.Provider provider)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            UpdateProviderInformationBasic(provider);
            provider.ProviderSpeciality = CreateProviderSpeciality(provider, provider.Id);
            provider.ProviderPaymentConcept = CreateProviderPaymentConcept(provider, provider.Id);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.UpdateProvider");
            return provider;
        }

    }
}
