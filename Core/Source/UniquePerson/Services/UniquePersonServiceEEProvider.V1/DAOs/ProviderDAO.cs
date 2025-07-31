using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Entities.views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MUp = Sistran.Core.Application.UniquePersonService.V1.Models;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    public class SupplierDAO
    {
        ///// <summary>
        ///// Obtener lista de tipos de proveedores
        ///// </summary>
        //public List<MUp.ProviderType> GetProviderTypes()
        //{
        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();
        //    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.ProviderType)));

        //    stopWatch.Stop();
        //    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetProviderTypes");
        //    return ModelAssembler.CreateProviderTypes(businessCollection);
        //}

        ///// <summary>
        ///// Obtener lista de tipos de proveedores declinados
        ///// </summary>
        //public List<MUp.ProviderDeclinedType> GetProviderDeclinedType()
        //{
        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();
        //    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.ProviderDeclinedType)));

        //    stopWatch.Stop();
        //    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetProviderDeclinedType");
        //    return ModelAssembler.CreateProviderDeclinedTypes(businessCollection);
        //}

        ///// <summary>
        ///// Crear información basica de proveedor
        ///// </summary>
        //public MUp.Supplier CreateSupplierInformationBasic(MUp.Supplier Supplier)
        //{
        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();
        //    UniquePersonV1.Entities.Provider providerEntity = EntityAssembler.CreateSupplier(Supplier);
        //    DataFacadeManager.Instance.GetDataFacade().InsertObject(providerEntity);

        //    stopWatch.Stop();
        //    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreateProviderInformationBasic");
        //    return ModelAssembler.CreateSupplier(providerEntity);
        //}

        ///// <summary>
        ///// Crear información basica de proveedor
        ///// </summary>
        //public List<MUp.ProviderSpeciality> CreateSupplierSpeciality(MUp.Supplier provider, int providerId)
        //{
        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();
        //    List<MUp.ProviderSpeciality> providerSpeciality = new List<MUp.ProviderSpeciality>();
        //    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //    filter.PropertyEquals(UniquePersonV1.Entities.ProviderSpeciality.Properties.ProviderCode, providerId);
        //    DataFacadeManager.Instance.GetDataFacade().Delete<UniquePersonV1.Entities.ProviderSpeciality>(filter.GetPredicate());
        //    if (provider.SupplierProfiles != null)
        //    {
        //        foreach (var item in provider.SupplierProfiles)
        //        {
        //            item.Id = providerId;
        //            //UniquePersonV1.Entities.ProviderSpeciality providerSpecialityEnt = EntityAssembler.CreateProviderSpeciality(item);
        //            //DataFacadeManager.Instance.GetDataFacade().InsertObject(providerSpecialityEnt);
        //            //providerSpeciality.Add(ModelAssembler.CreateProviderSpeciality(providerSpecialityEnt));
        //        }
        //    }

        //    stopWatch.Stop();
        //    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreateProviderSpeciality");
        //    return providerSpeciality;
        //}

        ///// <summary>
        ///// Crear concepto de pago de proveedor
        ///// </summary>
        //public List<MUp.ProviderPaymentConcept> CreateSupplierPaymentConcept(MUp.Supplier Supplier, int providerId)
        //{
        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();
        //    List<MUp.ProviderPaymentConcept> providerPaymentConcept = new List<MUp.ProviderPaymentConcept>();
        //    List<PaymentConcept> PaymentConcepts = DelegateService.commonServiceCore.GetPaymentConcept();
        //    for (int i = 0; i < Supplier.ProviderPaymentConcept.Count; i++)
        //    {
        //        if (Supplier.ProviderPaymentConcept[i].Id == 0)
        //        {
        //            Supplier.ProviderPaymentConcept[i].ProviderId = providerId;
        //            UniquePersonV1.Entities.ProviderPaymentConcept providerPaymentConceptEnt = EntityAssembler.CreateProviderPaymentConcept(Supplier.ProviderPaymentConcept[i]);
        //            DataFacadeManager.Instance.GetDataFacade().InsertObject(providerPaymentConceptEnt);
        //            string descriptionConceptPayment = PaymentConcepts.Where(b => b.Id == Supplier.ProviderPaymentConcept[i].PaymentConcept.Id).FirstOrDefault().Description;
        //            providerPaymentConcept.Add(ModelAssembler.CreateProviderPaymentConceptByDescription(providerPaymentConceptEnt, descriptionConceptPayment));
        //        }
        //        else if (Supplier.ProviderPaymentConcept[i].PaymentConcept.Id < 0)
        //        {
        //            PrimaryKey primaryKey = UniquePersonV1.Entities.ProviderPaymentConcept.CreatePrimaryKey(Supplier.ProviderPaymentConcept[i].Id);
        //            UniquePersonV1.Entities.ProviderPaymentConcept providerPaymentConceptEnt = (UniquePersonV1.Entities.ProviderPaymentConcept)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
        //            if (providerPaymentConcept != null)
        //            {
        //                DataFacadeManager.Instance.GetDataFacade().DeleteObject(providerPaymentConceptEnt);
        //            }
        //        }
        //        else
        //        {
        //            providerPaymentConcept.Add(Supplier.ProviderPaymentConcept[i]);
        //        }
        //    }

        //    stopWatch.Stop();
        //    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreateProviderPaymentConcept");
        //    return providerPaymentConcept;
        //}

        ///// <summary>
        ///// Crear nuevo proveedor
        ///// </summary>
        ///// <param name="provider">modelo provider</param>
        ///// <returns></returns>
        //public void UpdateSupplierInformationBasic(MUp.Supplier provider)
        //{
        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();
        //    PrimaryKey key = UniquePersonV1.Entities.Provider.CreatePrimaryKey(provider.Id);
        //    UniquePersonV1.Entities.Provider providerEntity = (UniquePersonV1.Entities.Provider)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        //    if (providerEntity != null)
        //    {
        //        providerEntity.ProviderTypeCode = provider.Type.Id;
        //        providerEntity.OriginTypeCode = provider.OriginType.Id;
        //        providerEntity.ProviderDeclinedTypeCode = provider.ProviderDeclinedTypeId;
        //        providerEntity.Observation = provider.Observation;
        //        providerEntity.ModificationDate = provider.ModificationDate;
        //        providerEntity.DeclinationDate = provider.DeclinationDate;
        //        providerEntity.SpecialityDefault = provider.SupplierProfileDefault.Id;
        //    }

        //    stopWatch.Stop();
        //    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.UpdateProviderInformationBasic");
        //    DataFacadeManager.Instance.GetDataFacade().UpdateObject(providerEntity);
        //}

        ///// <summary>
        ///// Consultar proveedor por individualID
        ///// </summary>
        //public MUp.Supplier GetSupplierByIndividualId(int individualId)
        //{
        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();
        //    MUp.Supplier provider = new MUp.Supplier();
        //    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
        //    filter.Property(UniquePersonV1.Entities.Provider.Properties.IndividualId, typeof(UniquePersonV1.Entities.Provider).Name);
        //    filter.Equal();
        //    filter.Constant(individualId);
        //    BusinessCollection businessCollection = new BusinessCollection();
        //    using (var daf = DataFacadeManager.Instance.GetDataFacade())
        //    {
        //        businessCollection = new BusinessCollection(daf.SelectObjects(typeof(UniquePersonV1.Entities.Provider), filter.GetPredicate()));
        //    }

        //    provider = ModelAssembler.CreateSuppliers(businessCollection).FirstOrDefault();

        //    if (provider != null)
        //    {
        //        //provider.ProviderSpeciality = new List<MUp.ProviderSpeciality>();
        //        filter = new ObjectCriteriaBuilder();
        //        filter.Property(UniquePersonV1.Entities.ProviderSpeciality.Properties.ProviderCode, typeof(UniquePersonV1.Entities.ProviderSpeciality).Name);
        //        filter.Equal();
        //        filter.Constant(provider.Id);
        //        using (var daf = DataFacadeManager.Instance.GetDataFacade())
        //        {
        //            businessCollection = new BusinessCollection(daf.SelectObjects(typeof(UniquePersonV1.Entities.ProviderSpeciality), filter.GetPredicate()));
        //        }
        //        //provider.ProviderSpeciality = ModelAssembler.CreateProviderSpecialitys(businessCollection);

        //        provider.ProviderPaymentConcept = new List<MUp.ProviderPaymentConcept>();
        //        ProviderPaymentConceptView view = new ProviderPaymentConceptView();
        //        ViewBuilder builder = new ViewBuilder("ProviderPaymentConceptView");
        //        filter = new ObjectCriteriaBuilder();
        //        filter.Property(UniquePersonV1.Entities.ProviderPaymentConcept.Properties.ProviderCode, typeof(UniquePersonV1.Entities.ProviderPaymentConcept).Name);
        //        filter.Equal();
        //        filter.Constant(provider.Id);
        //        builder.Filter = filter.GetPredicate();
        //        using (var daf = DataFacadeManager.Instance.GetDataFacade())
        //        {
        //           daf.FillView(builder, view);
        //        }
        //        provider.ProviderPaymentConcept = ModelAssembler.CreateProviderPaymentConcepts(view);
        //    }

        //    stopWatch.Stop();
        //    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetProviderByIndividualId");
        //    return provider;
        //}

        ///// <summary>
        ///// Crear nuevo proveedor
        ///// </summary>
        ///// <param name="supplier">modelo provider</param>
        ///// <returns></returns>
        //public MUp.Supplier CreateSupplier(MUp.Supplier supplier)
        //{
        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();
        //    Models.Supplier providerPerson = CreateSupplierInformationBasic(supplier);
        //    //providerPerson.ProviderSpeciality = CreateSupplierSpeciality(supplier, providerPerson.Id);
        //    providerPerson.ProviderPaymentConcept = CreateSupplierPaymentConcept(supplier, providerPerson.Id);

        //    stopWatch.Stop();
        //    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreateProvider");
        //    return providerPerson;
        //}

        ///// <summary>
        ///// Actualizar proveedor
        ///// </summary>        
        //public MUp.Supplier UpdateSupplier(MUp.Supplier supplier)
        //{
        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();
        //    UpdateSupplierInformationBasic(supplier);
        //   // supplier.ProviderSpeciality = CreateSupplierSpeciality(supplier, supplier.Id);
        //    supplier.ProviderPaymentConcept = CreateSupplierPaymentConcept(supplier, supplier.Id);

        //    stopWatch.Stop();
        //    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.UpdateProvider");
        //    return supplier;
        //}

    }
}
