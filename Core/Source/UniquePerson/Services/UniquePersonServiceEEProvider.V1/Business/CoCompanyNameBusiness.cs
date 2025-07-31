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
    public class CoCompanyNameBusiness
    {


        /// <summary>
        /// Obtener lista de razones sociales
        /// </summary>
        public List<MUp.CompanyName> GetCoCompanyNames()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var businessCollection = DataFacadeManager.GetObjects(typeof(CoCompanyName));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetCoCompanyNames");
            return ModelAssembler.CreateCompaniesName(businessCollection);
        }

        /// <summary>
        /// Consultar razones sociales por individualID
        /// </summary>
        public List<MUp.CompanyName> GetCoCompanyNamesByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List <MUp.CompanyName> CoCompanyName = new List<MUp.CompanyName>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePersonV1.Entities.CoCompanyName.Properties.IndividualId, typeof(UniquePersonV1.Entities.CoCompanyName).Name);
            filter.Equal();
            filter.Constant(individualId);
            BusinessCollection businessCollection = new BusinessCollection();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(UniquePersonV1.Entities.CoCompanyName), filter.GetPredicate()));
            }

            CoCompanyName = ModelAssembler.CreateCompaniesName(businessCollection);
            
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetCoCompanyNamesByIndividualId");
            return CoCompanyName;
        }

        ///// <summary>
        ///// Crear nuevo proveedor
        ///// </summary>
        ///// <param name="supplier">modelo supplier</param>
        ///// <returns></returns>
        //public List<MUp.CompanyName> CreateCoCompanyNames(List<MUp.CompanyName> CoCompanyName)
        //{
        //    Stopwatch stopWatch = new Stopwatch();
        //    stopWatch.Start();
        //    //Models.Supplier supplierPerson = CreateSupplierInformationBasic(supplier);
        //    //supplier.AccountingConcepts = CreateSupplierAccountingConcepts(supplier.AccountingConcepts, supplierPerson.Id);
        //    stopWatch.Stop();
        //    Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.CreateSupplier");
        //    return CoCompanyName;
        //}

        public MUp.CompanyName CreateCoCompanyNames(MUp.CompanyName companyName)
        {

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            var companyNamebusinessEntity = EntityAssembler.CreateCoCompanyName(companyName);
            companyNamebusinessEntity = (UniquePersonV1.Entities.CoCompanyName)DataFacadeManager.Insert(companyNamebusinessEntity);
            MUp.CompanyName individualTaxModel = ModelAssembler.CreateCompanyName(companyNamebusinessEntity);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CompanyNameDAO");
            return individualTaxModel;

        }

        public MUp.CompanyName UpdateCoCompanyName(MUp.CompanyName companyName)
        {

            PrimaryKey primaryKey = UniquePersonV1.Entities.CoCompanyName.CreatePrimaryKey(companyName.IndividualId, companyName.NameNum);
            UniquePersonV1.Entities.CoCompanyName companyNameEntities = (UniquePersonV1.Entities.CoCompanyName)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            if (companyNameEntities != null)
            { 
                companyNameEntities.TradeName = companyName.TradeName;
                companyNameEntities.IsMain = companyName.IsMain;
                companyNameEntities.PhoneDataCode = companyName.Phone.Id;
                companyNameEntities.AddressDataCode = companyName.Address.Id;
                companyNameEntities.EmailDataCode = companyName.Email.Id;
                companyNameEntities.Enabled = companyName.Enabled;
                DataFacadeManager.Update(companyNameEntities);
            }
            return ModelAssembler.CreateCompanyName(companyNameEntities);
        }

        public int CountBusinessNameByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<MUp.CompanyName> CoCompanyName = new List<MUp.CompanyName>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePersonV1.Entities.CoCompanyName.Properties.IndividualId, typeof(UniquePersonV1.Entities.CoCompanyName).Name);
            filter.Equal();
            filter.Constant(individualId);
            BusinessCollection businessCollection = new BusinessCollection();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(UniquePersonV1.Entities.CoCompanyName), filter.GetPredicate()));
            }
            int countBusinessName = 0;
            if (businessCollection.Count > 0)
            {
                CoCompanyName = ModelAssembler.CreateCompaniesName(businessCollection);
                countBusinessName = CoCompanyName.LastOrDefault().NameNum + 1;
            }
            else
            {
                countBusinessName = 1;
            }
            
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetCoCompanyNamesByIndividualId");
            return countBusinessName;

        }

    }
}