using Sistran.Core.Application.UniquePersonService.V1.Enums;
using MOUP = Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using System.Diagnostics;
using System.Linq;
using MUp = Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.V1.Entities.views;
using Sistran.Core.Framework.DAF.Engine;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class SupplierBusiness
    {

        public List<MUp.SupplierAccountingConcept> GetSupplierAccountingConceptsBySupplierId(int SupplierId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<MUp.SupplierAccountingConcept> supplierAccountingConcept = new List<MUp.SupplierAccountingConcept>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePersonV1.Entities.SupplierAccountingConcept.Properties.SupplierCode, typeof(UniquePersonV1.Entities.SupplierAccountingConcept).Name);
            filter.Equal();
            filter.Constant(SupplierId);
            BusinessCollection businessCollection = new BusinessCollection();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(UniquePersonV1.Entities.SupplierAccountingConcept), filter.GetPredicate()));
            }

            supplierAccountingConcept = ModelAssembler.CreateSupplierAccountingConcepts(businessCollection);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetSupplierAccountingConceptBySupplierId");

            return ModelAssembler.CreateSupplierAccountingConcepts(businessCollection);
        }

        public List<MUp.SupplierGroupSupplier> GetGroupSupplierBySupplierId(int SupplierId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<MUp.SupplierGroupSupplier> supplierGroupSupplier = new List<MUp.SupplierGroupSupplier>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePersonV1.Entities.SupplierGroupSupplier.Properties.SupplierCode, typeof(UniquePersonV1.Entities.SupplierGroupSupplier).Name);
            filter.Equal();
            filter.Constant(SupplierId);
            BusinessCollection businessCollection = new BusinessCollection();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(UniquePersonV1.Entities.SupplierGroupSupplier), filter.GetPredicate()));
            }

            supplierGroupSupplier = ModelAssembler.CreateSupplierGroupSupplier(businessCollection);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetSupplierAccountingConceptBySupplierId");

            return ModelAssembler.CreateSupplierGroupSupplier(businessCollection);
        }

        /// <summary>
        /// Obtener lista de proveedores
        /// </summary>
        public List<MUp.AccountingConcept> GetAccountingConcepts()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var businessCollection = DataFacadeManager.GetObjects(typeof(AccountingConcept));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetAccountingConcepts");
            return ModelAssembler.CreateAccountingConcepts(businessCollection);
        }



        /// <summary>
        /// Obtener lista de proveedores
        /// </summary>
        public List<MUp.SupplierProfile> GetSupplierProfiles()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var businessCollection = DataFacadeManager.GetObjects(typeof(SupplierProfile));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetSupplierProfile");
            return ModelAssembler.CreateSupplierProfiles(businessCollection);
        }

        /// <summary>
        /// Obtener lista de proveedores
        /// </summary>
        public List<MUp.SupplierProfile> GetSupplierTypeProfileById(int suppilierTypeId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var filter = new ObjectCriteriaBuilder();
            filter.Property(SupplierTypeProfile.Properties.SupplierTypeCode, typeof(SupplierTypeProfile).Name);
            filter.Equal();
            filter.Constant(suppilierTypeId);
            var supplierTypeProfiles = DataFacadeManager.GetObjects(typeof(SupplierTypeProfile), filter.GetPredicate()).ToList();

            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetSupplierById");

            stopWatch.Stop();

            return ModelAssembler.CreateSupplierTypeProfiles(supplierTypeProfiles.Cast<SupplierTypeProfile>().ToList());

        }

        /// <summary>
        /// Obtener lista de proveedores
        /// </summary>
        public List<MUp.Supplier> GetSuppliers()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var businessCollection = DataFacadeManager.GetObjects(typeof(Supplier));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetSuppliers");
            return ModelAssembler.CreateSuppliers(businessCollection);
        }


        /// <summary>
        /// Obtener lista de proveedores
        /// </summary>
        public MUp.Supplier GetSupplierById(int SupplierId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var filter = new ObjectCriteriaBuilder();
            filter.Property(Supplier.Properties.SupplierCode, typeof(Supplier).Name);
            filter.Equal();
            filter.Constant(SupplierId);
            var supplierCollection = (Supplier)DataFacadeManager.GetObjects(typeof(Supplier), filter.GetPredicate()).FirstOrDefault();

            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetSupplierById");

            stopWatch.Stop();

            return ModelAssembler.CreateSupplier(supplierCollection);


        }


        /// <summary>
        /// Obtener lista de tipos de proveedores
        /// </summary>
        public List<MUp.SupplierType> GetSupplierTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var businessCollection = DataFacadeManager.GetObjects(typeof(SupplierType));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetSupplierTypes");
            return ModelAssembler.CreateSupplierTypes(businessCollection);
        }

        /// <summary>
        /// Obtener lista de tipos de proveedores declinados
        /// </summary>
        public List<MUp.SupplierDeclinedType> GetSupplierDeclinedTypes()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var businessCollection = DataFacadeManager.GetObjects(typeof(SupplierDeclinedType));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetProviderDeclinedType");
            return ModelAssembler.CreateSupplierDeclinedTypes(businessCollection);
        }

        /// <summary>
        /// Obtener grupo de proveedores 
        /// </summary>
        public List<MUp.GroupSupplier> GetGroupSupplier()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var businessCollection = DataFacadeManager.GetObjects(typeof(GroupSupplier));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetProviderDeclinedType");
            return ModelAssembler.CreateGroupsSupplier(businessCollection);
        }

        /// <summary>
        /// Crear nuevo proveedor
        /// </summary>
        /// <param name="supplier">modelo supplier</param>
        /// <returns></returns>
        public MUp.Supplier CreateSupplier(MUp.Supplier supplier)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            Models.Supplier supplierPerson = CreateSupplierInformationBasic(supplier);
            supplier.AccountingConcepts = CreateSupplierAccountingConcepts(supplier.AccountingConcepts, supplierPerson.Id);
            supplier.GroupSupplier = CreateGroupSupplier(supplier.GroupSupplier, supplierPerson.Id);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.CreateSupplier");
            return supplierPerson;
        }

        /// <summary>
        /// Crear información basica de proveedor
        /// </summary>
        public MUp.Supplier CreateSupplierInformationBasic(MUp.Supplier Supplier)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            UniquePersonV1.Entities.Supplier supplierEntity = EntityAssembler.CreateSupplier(Supplier);
            DataFacadeManager.Insert(supplierEntity);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.CreateSupplierInformationBasic");
            return ModelAssembler.CreateSupplier(supplierEntity);
        }

        /// <summary>
        /// Consultar proveedor por individualID
        /// </summary>
        public MUp.Supplier GetSupplierByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            MUp.Supplier supplier = new MUp.Supplier();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePersonV1.Entities.Supplier.Properties.IndividualId, typeof(UniquePersonV1.Entities.Supplier).Name);
            filter.Equal();
            filter.Constant(individualId);
            BusinessCollection businessCollection = new BusinessCollection();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(UniquePersonV1.Entities.Supplier), filter.GetPredicate()));
            }

            supplier = ModelAssembler.CreateSuppliers(businessCollection).FirstOrDefault();
            if (supplier != null && supplier.Id > 0)
            {
                supplier.GroupSupplier = GetSupplierGroupSupplierBySupplierId(supplier.Id);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetSupplierByIndividualId");
            return supplier;
        }

        /// <summary>
        /// Consultar grupo de proveedor por individualID
        /// </summary>
        public List<MUp.GroupSupplier> GetSupplierGroupSupplierBySupplierId(int supplierId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<MUp.SupplierGroupSupplier> supplierGroupSupplier = new List<MUp.SupplierGroupSupplier>();
            List<MUp.GroupSupplier> groupSupplier = new List<MUp.GroupSupplier>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePersonV1.Entities.SupplierGroupSupplier.Properties.SupplierCode, typeof(UniquePersonV1.Entities.SupplierGroupSupplier).Name);
            filter.Equal();
            filter.Constant(supplierId);
            BusinessCollection businessCollection = new BusinessCollection();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(UniquePersonV1.Entities.SupplierGroupSupplier), filter.GetPredicate()));
            }

            if (businessCollection.Count > 0)
            {
                supplierGroupSupplier = ModelAssembler.CreateSupplierGroupSupplier(businessCollection);
                groupSupplier = supplierGroupSupplier.Select(x => new MUp.GroupSupplier { Id = x.GroupSupplierCd }).ToList();
            }
            

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetSupplierByIndividualId");
            return groupSupplier;
        }




        /// <summary>
        /// Actualizar proveedor
        /// </summary>        
        public MUp.Supplier UpdateSupplier(MUp.Supplier supplier)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            UpdateSupplierInformationBasic(supplier);
            supplier.AccountingConcepts = CreateSupplierAccountingConcepts(supplier.AccountingConcepts, supplier.Id);
            supplier.GroupSupplier = CreateGroupSupplier(supplier.GroupSupplier, supplier.Id);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.UpdateProvider");
            return supplier;
        }

        /// <summary>
        /// Crear nuevo proveedor
        /// </summary>
        /// <param name="supplier">modelo supplier</param>
        /// <returns></returns>
        public void UpdateSupplierInformationBasic(MUp.Supplier supplier)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PrimaryKey key = UniquePersonV1.Entities.Supplier.CreatePrimaryKey(supplier.Id);
            UniquePersonV1.Entities.Supplier supplierEntity = (UniquePersonV1.Entities.Supplier)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            if (supplierEntity != null)
            {
                if (supplier.Profile != null)
                {
                    supplierEntity.SupplierProfileCode = supplier.Profile.Id;
                }
                else {
                    supplierEntity.SupplierProfileCode = null;
                }


                if (supplier.Type != null)
                {
                    supplierEntity.SupplierTypeCode = supplier.Type.Id;
                }
                else {
                    supplierEntity.SupplierTypeCode = null;
                }

                supplierEntity.DeclinedDate = supplier.DeclinedDate;
                supplierEntity.DeclinedReason = supplier.DeclinedReason;
                supplierEntity.Enabled = supplier.Enabled == null ? false : supplier.Enabled;
                supplierEntity.CheckPayableTo = supplier.CheckPayableTo;
                supplierEntity.OrderCheck = supplier.OrderCheck == null ? false : supplier.OrderCheck;
                if (supplier.PaymentAccountType != null)
                {
                    supplierEntity.PaymentAccountTypeCode = supplier.PaymentAccountType.Id;
                }
                else {
                    supplierEntity.PaymentAccountTypeCode = null;
                }
                supplierEntity.IndividualId = supplier.IndividualId;
                supplierEntity.Name = supplier.Name;
                supplierEntity.ModificationDate = supplier.ModificationDate;
                supplierEntity.Observation = supplier.Observation;
                if (supplier.DeclinedType != null)
                {
                    supplierEntity.SupplierDeclinedTypeCode = supplier.DeclinedType.Id;
                }
                else {
                    supplierEntity.SupplierDeclinedTypeCode = null;
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.UpdateSupplierInformationBasic");
            DataFacadeManager.Update(supplierEntity);
        }


        public List<MUp.AccountingConcept> CreateSupplierAccountingConcepts(List<MUp.AccountingConcept> accountingConcepts, int SupplierId)
        {
            List<MUp.AccountingConcept> accountingConceptResult = new List<MUp.AccountingConcept>();

            if (accountingConcepts != null)
            {
                List<MUp.SupplierAccountingConcept> supplierAccountingConcept = GetSupplierAccountingConceptsBySupplierId(SupplierId);

                foreach (MUp.AccountingConcept accountingConcept in accountingConcepts)
                {

                    if (!(supplierAccountingConcept.Exists(x => (x.AccountingConcept.Id == accountingConcept.Id && x.Supplier.Id == SupplierId))))
                    {
                        CreateSupplierAccountingConcept(SupplierId, accountingConcept.Id);
                    }
                }

                List<MUp.SupplierAccountingConcept> supplierAccountingConceptDeletes = (from t in supplierAccountingConcept where !accountingConcepts.Any(x => x.Id == t.AccountingConcept.Id && SupplierId == t.Supplier.Id) select t).ToList();

                foreach (MUp.SupplierAccountingConcept supplierAccountingConceptDelete in supplierAccountingConceptDeletes)
                {
                    DeleteSupplierAccountingConcept(supplierAccountingConceptDelete.Id);
                }

            }

            return accountingConceptResult;
        }

        public List<MUp.GroupSupplier> CreateGroupSupplier(List<MUp.GroupSupplier> groupSuppliers, int SupplierId)
        {
            if (groupSuppliers != null)
            {
                List<MUp.SupplierGroupSupplier> groupsSuppliers = GetGroupSupplierBySupplierId(SupplierId);

                foreach (MUp.GroupSupplier groupSupplier in groupSuppliers)
                {

                    if (!(groupsSuppliers.Exists(x => (x.GroupSupplierCd == groupSupplier.Id && x.SupplierCd == SupplierId))))
                    {
                        CreateGroupSupplier(SupplierId, groupSupplier.Id);
                    }
                }

                List<MUp.SupplierGroupSupplier> groupSupplierDeletes = (from t in groupsSuppliers where !groupSuppliers.Any(x => x.Id == t.GroupSupplierCd && SupplierId == t.SupplierCd) select t).ToList();

                foreach (MUp.SupplierGroupSupplier groupSupplierDelete in groupSupplierDeletes)
                {
                    DeleteGroupSupplier(groupSupplierDelete.SupplierGroupSupplierCd);
                }

            }

            return groupSuppliers;
        }

        public void CreateSupplierAccountingConcept(int SupplierId, int accountingConceptId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            MUp.SupplierAccountingConcept ModelsupplierAccountingConcept = new MUp.SupplierAccountingConcept { AccountingConcept = new MUp.AccountingConcept { Id = accountingConceptId }, Supplier = new MUp.Supplier { Id = SupplierId } };
            UniquePersonV1.Entities.SupplierAccountingConcept SupplierAccountingConceptEntity = EntityAssembler.CreateSupplierAccountingConcept(ModelsupplierAccountingConcept);
            DataFacadeManager.Insert(SupplierAccountingConceptEntity);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.CreateSupplierAccountingConcept");
        }

        public void CreateGroupSupplier(int SupplierId, int GroupSupplierCd)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            MUp.SupplierGroupSupplier ModelSupplierGroupSupplier = new MUp.SupplierGroupSupplier { SupplierCd = SupplierId, GroupSupplierCd = GroupSupplierCd };
            UniquePersonV1.Entities.SupplierGroupSupplier SupplierGroupSupplierEntity = EntityAssembler.CreateSupplierAccountingConcept(ModelSupplierGroupSupplier);
            DataFacadeManager.Insert(SupplierGroupSupplierEntity);
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.CreateSupplierAccountingConcept");
        }


        public void DeleteSupplierAccountingConcept(int supplierAccountingConceptId)
        {
            PrimaryKey primaryKey = UniquePersonV1.Entities.SupplierAccountingConcept.CreatePrimaryKey(supplierAccountingConceptId);
            DataFacadeManager.Delete(primaryKey);
        }

        public void DeleteGroupSupplier(int supplierGroupSupplierCd)
        {
            PrimaryKey primaryKey = UniquePersonV1.Entities.SupplierGroupSupplier.CreatePrimaryKey(supplierGroupSupplierCd);
            DataFacadeManager.Delete(primaryKey);
        }

    }
}