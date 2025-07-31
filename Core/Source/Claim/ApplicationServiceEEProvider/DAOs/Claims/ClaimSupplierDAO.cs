using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.ClaimServices.EEProvider.Views.Claims;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class ClaimSupplierDAO
    {
        public List<ClaimSupplier> GetSuppliersBySupplierProfile(SupplierProfile supplierProfile)
        {
            List<ClaimSupplier> suppliers = new List<ClaimSupplier>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(UPEN.Supplier.Properties.SupplierProfileCode, typeof(UPEN.Supplier).Name, supplierProfile);
            filter.And();
            filter.OpenParenthesis();
            filter.PropertyEquals(UPEN.Supplier.Properties.Enabled, typeof(UPEN.Supplier).Name, true);
            filter.Or();
            filter.Property(UPEN.Supplier.Properties.Enabled, typeof(UPEN.Supplier).Name);
            filter.IsNull();
            filter.Or();
            filter.Property(UPEN.Supplier.Properties.DeclinedDate, typeof(UPEN.Supplier).Name);
            filter.IsNull();
            filter.CloseParenthesis();

            SupplierView supplierView = new SupplierView();
            ViewBuilder viewBuilder = new ViewBuilder("SupplierView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, supplierView);

            if (supplierView.Suppliers.Count > 0)
            {
                foreach (UPEN.Supplier entitySupplier in supplierView.Suppliers)
                {
                    UPEN.Individual entityIndividual = supplierView.Individuals.Cast<UPEN.Individual>().FirstOrDefault(x => x.IndividualId == entitySupplier.IndividualId);

                    if (entityIndividual != null && entityIndividual.IndividualTypeCode == (int)IndividualType.Person)
                    {
                        UPEN.Person entityPerson = supplierView.Persons.Cast<UPEN.Person>().FirstOrDefault(x => x.IndividualId == entitySupplier.IndividualId);

                        if (entityPerson != null)
                            suppliers.Add(ModelAssembler.CreateSupplier(entityPerson, entitySupplier));
                    }
                    else
                    {
                        UPEN.Company entityCompany = supplierView.Companys.Cast<UPEN.Company>().FirstOrDefault(x => x.IndividualId == entitySupplier.IndividualId);

                        if (entityCompany != null)
                            suppliers.Add(ModelAssembler.CreateSupplier(entityCompany, entitySupplier));
                    }
                }
            }

            return suppliers;
        }

        public List<ClaimSupplier> GetSuppliersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            List<ClaimSupplier> suppliers = new List<ClaimSupplier>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (insuredSearchType == InsuredSearchType.IndividualId)
            {
                var individualId = Convert.ToInt32(description);
                filter.Property(UPEN.Supplier.Properties.IndividualId, typeof(UPEN.Supplier).Name);
                filter.Equal();
                filter.Constant(individualId);
            }
            else if (insuredSearchType == InsuredSearchType.DocumentNumber)
            {
                filter.Property(UPEN.Person.Properties.IdCardNo, typeof(UPEN.Person).Name);
                filter.Like();
                filter.Constant(description + '%');
                filter.Or();
                filter.Property(UPEN.Person.Properties.Name, typeof(UPEN.Person).Name);
                filter.Like();
                filter.Constant('%' + description + '%');
                filter.Or();
                filter.Property(UPEN.Person.Properties.Surname, typeof(UPEN.Person).Name);
                filter.Like();
                filter.Constant('%' + description + '%');
                filter.Or();
                filter.Property(UPEN.Company.Properties.TributaryIdNo, typeof(UPEN.Company).Name);
                filter.Like();
                filter.Constant(description + '%');
                filter.Or();
                filter.Property(UPEN.Company.Properties.TradeName, typeof(UPEN.Company).Name);
                filter.Like();
                filter.Constant('%' + description + '%');
            }

            SupplierView supplierView = new SupplierView();
            ViewBuilder viewBuilder = new ViewBuilder("SupplierView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, supplierView);

            if (supplierView.Suppliers.Count > 0)
            {
                foreach (UPEN.Supplier entitySupplier in supplierView.Suppliers)
                {
                    UPEN.Individual entityIndividual = supplierView.Individuals.Cast<UPEN.Individual>().First(x => x.IndividualId == entitySupplier.IndividualId);

                    if (entityIndividual.IndividualTypeCode == (int)IndividualType.Person)
                    {
                        suppliers.Add(ModelAssembler.CreateSupplier(supplierView.Persons.Cast<UPEN.Person>().First(x => x.IndividualId == entitySupplier.IndividualId), entitySupplier));
                    }
                    else
                    {
                        suppliers.Add(ModelAssembler.CreateSupplier(supplierView.Companys.Cast<UPEN.Company>().First(x => x.IndividualId == entitySupplier.IndividualId), entitySupplier));
                    }
                }
            }
            return suppliers;
        }
    }
}
