using Sistran.Company.Application.UnderwritingServices.EEProvider.Helpers;
using Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.Models;
using UPEN = Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Company.Application.UnderwritingServices.EEProvider.Assemblers;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAOs
{
    class HolderCompDAO
    {
        public List<Holder> GetHoldersByDocument(string document, CustomerType? customerType)
        {
            List<IssuanceInsured> insureds = new InsuredDAO().GetInsuredsByDescriptionInsuredSearchTypeCustomerType(document, InsuredSearchType.DocumentNumber, customerType);
            List<Holder> holders = new List<Holder>();
            if (insureds != null && insureds.Count > 0)
            {
                if (insureds.Count == 1)
                    holders = insureds.ToHolderFullModelList();
                    
                else
                    holders = insureds.ToHolderModelList();
            }
            return holders;
        }
        public List<Holder> GetPersonOrCompanyByDescription(string description, CustomerType? customerType)
        {
            List<IssuanceInsured> insureds = new InsuredDAO().GetPersonOrCompanyByByDescriptionInsuredSearchTypeCustomerType(description, InsuredSearchType.DocumentNumber, customerType);
            List<Holder> holders = new List<Holder>();
            if (insureds != null && insureds.Count > 0)
            {
                if (insureds.Count == 1)
                    holders = insureds.ToHolderFullModelList();

                else
                    holders = insureds.ToHolderModelList();
            }
            return holders;
        }

        public Tuple<Holder,List<IssuanceCompanyName>> GetHolderByIndividualId(string individualId, CustomerType? customerType)
        {
            List<IssuanceInsured> insureds = new InsuredDAO().GetInsuredsByDescriptionInsuredSearchTypeCustomerType(individualId, InsuredSearchType.IndividualId, customerType);

            if (insureds != null && insureds.Count > 0)
            {
                var insured = insureds[0].ToHolderFullModel();
                //var details = GetHolderDetails(insured.IndividualId, customerType);
                List<IssuanceCompanyName> details = new List<IssuanceCompanyName>();
                details.Add(insureds[0].CompanyName);
                return new Tuple<Holder,List<IssuanceCompanyName>> (insured, details);
            }
            return null;

        }

        public List<IssuanceCompanyName> GetHolderDetails(int individualId, CustomerType? customerType)
        {
            BusinessCollection businessCollection = new BusinessCollection();
            List<IssuanceCompanyName> companyNames = new List<IssuanceCompanyName>();

            if (customerType == CustomerType.Individual)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(UPEN.Address.Properties.IndividualId, typeof(UPEN.Address).Name);
                filter.Equal();
                filter.Constant(individualId);
                businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPEN.Address), filter.GetPredicate()));

                List<UPEN.Address> entityAddresses = businessCollection.Cast<UPEN.Address>().OrderByDescending(x => x.IsMailingAddress).ToList();

                filter = new ObjectCriteriaBuilder();
                filter.Property(UPEN.Phone.Properties.IndividualId, typeof(UPEN.Phone).Name);
                filter.Equal();
                filter.Constant(individualId);
                businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPEN.Phone), filter.GetPredicate()));

                List<UPEN.Phone> entityPhones = businessCollection.Cast<UPEN.Phone>().OrderByDescending(x => x.IsMain).ToList();

                filter = new ObjectCriteriaBuilder();
                filter.Property(UPEN.Email.Properties.IndividualId, typeof(UPEN.Email).Name);
                filter.Equal();
                filter.Constant(individualId);
                businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPEN.Email), filter.GetPredicate()));


                List<UPEN.Email> entityEmails = businessCollection.Cast<UPEN.Email>().OrderByDescending(x => x.IsMailingAddress).ToList();

                if (entityAddresses.Count > 0)
                {
                    foreach (UPEN.Address entityAddress in entityAddresses)
                    {
                        IssuanceCompanyName issuanceCompanyName = new IssuanceCompanyName
                        {
                            Address = ModelAssembler.CreateAddress(entityAddress)
                        };

                        if (entityPhones.Count > 0)
                        {
                            issuanceCompanyName.Phone = ModelAssembler.CreatePhone(entityPhones.First());
                            entityPhones.RemoveAt(0);
                        }

                        if (entityEmails.Count > 0)
                        {
                            issuanceCompanyName.Email = ModelAssembler.CreateEmail(entityEmails.First());
                            entityEmails.RemoveAt(0);
                        }

                        companyNames.Add(issuanceCompanyName);
                    }
                }
            }
            else
            {
                PrimaryKey primaryKey = UPEN.Prospect.CreatePrimaryKey(individualId);
                UPEN.Prospect entityProspect = (UPEN.Prospect)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

                companyNames.Add(new IssuanceCompanyName
                {
                    Address = new IssuanceAddress
                    {
                        Description = entityProspect.Street
                    },
                    Phone = new IssuancePhone
                    {
                        Description = entityProspect.PhoneNumber.HasValue ? entityProspect.PhoneNumber.ToString() : ""
                    },
                    Email = new IssuanceEmail
                    {
                        Description = entityProspect.EmailAddress
                    }
                });
            }

            return companyNames;
        }

    }
}
