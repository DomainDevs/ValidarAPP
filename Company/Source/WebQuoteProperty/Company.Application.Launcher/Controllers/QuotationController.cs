using System;
using System.Linq;
using System.Web.Mvc;
using System.Configuration;
using System.Collections.Generic;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePersonService.Models;
using UPEN = Sistran.Core.Application.UniquePersonService.Enums;
using Sistran.Company.Application.UnderwritingServices.Models;
using System.Text;
using Sistran.Company.Application.Location.PropertyServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Company.Application.UniquePersonServices.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Company.Application.PrintingServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Framework.UIF.Web.App_GlobalResources;
using Sistran.Core.Application.UniquePersonService.Enums;

namespace Sistran.Core.Framework.UIF.Web.Controllers
{
    [Authorize]
    public class QuotationController : Controller
    {
        private static int prefixId = Convert.ToInt32(ConfigurationManager.AppSettings["PrefixId"]);
        private static List<Country> countries = new List<Country>();
        private static List<DocumentType> documentTypes = new List<DocumentType>();

        public ActionResult Quotation()
        {
            return View();
        }

        public ActionResult GetCurrentDate()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.commonService.GetDate());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        public ActionResult GetCountries()
        {
            try
            {
                if (countries.Count == 0)
                {
                    countries = DelegateService.commonService.GetCountries();
                }

                return new UifJsonResult(true, countries.OrderBy(x => x.Description));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        public ActionResult GetStatesByCountryId(int countryId)
        {
            try
            {
                List<State> states = countries.First(x => x.Id == countryId).States;
                return new UifJsonResult(true, states.OrderBy(x => x.Description));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        public ActionResult GetCitiesByCountryIdStateId(int countryId, int stateId)
        {
            try
            {
                List<City> cities = countries.First(x => x.Id == countryId).States.First(y => y.Id == stateId).Cities;
                return new UifJsonResult(true, cities.OrderBy(x => x.Description));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        public ActionResult GetDocumentTypesByIndividualType(int individualType)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.uniquePersonService.GetDocumentTypes(individualType).OrderBy(x => x.Description));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        public ActionResult GetProducts()
        {
            try
            {
                return new UifJsonResult(true, DelegateService.underwritingService.GetProductsByPrefixIdIsGreen(prefixId, true).OrderBy(x => x.Description));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        public ActionResult GetCoverageGroupsByProductId(int productId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.underwritingService.GetGroupCoverages(productId).OrderBy(x => x.Description));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        public ActionResult GetInsuredObjectsByProductIdCoverageGroupId(int productId, int coverageGroupId)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.underwritingService.GetInsuredObjectsByProductIdGroupCoverageId(productId, coverageGroupId, prefixId).OrderBy(x => x.Description));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        public ActionResult GetNormalizedAddress(string address)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.uniquePersonService.NormalizeAddress(address));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        public ActionResult GetProspectByIndividualTypeDocumentTypeIdDocumentNumber(UPEN.IndividualType individualType, int documentTypeId, string documentNumber)
        {
            try
            {
                Prospect prospect = DelegateService.uniquePersonService.GetProspectByIndividualTypeDocumentTypeIdDocument(individualType, documentTypeId, documentNumber);

                if (prospect != null)
                {
                    return new UifJsonResult(true, prospect);
                }
                else
                {
                    return new UifJsonResult(false, Language.MessageCustomerNotFound);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        public ActionResult GetProspectByDescription(string description)
        {
            try
            {
                int personId;
                Prospect prospect = null;
                bool isNumeric = int.TryParse(description, out personId);

                if (isNumeric)
                {
                    prospect = DelegateService.uniquePersonService.GetProspectByDescription(description, InsuredSearchType.DocumentNumber).FirstOrDefault();
                }

                if (prospect != null)
                {
                    return new UifJsonResult(true, prospect);
                }
                else
                {
                    return new UifJsonResult(false, Language.MessageCustomerNotFound);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }
        public ActionResult SetInsuredValue(int productId, int coverageGroupId, int insuredObjectId, decimal insuredValue, List<CompanyInsuredObject> insuredObjects)
        {
            try
            {
                CompanyInsuredObject insuredObject = new CompanyInsuredObject();

                if (insuredObjects == null)
                {
                    insuredObjects = new List<CompanyInsuredObject>();
                }

                if (insuredObjects.Exists(x => x.Id == insuredObjectId))
                {
                    insuredObject = insuredObjects.First(x => x.Id == insuredObjectId);
                    insuredObjects.Remove(insuredObject);
                }
                else
                {
                    insuredObject = DelegateService.underwritingService.GetCompanyInsuredObjectByProductIdGroupCoverageIdInsuredObjectId(productId, coverageGroupId, insuredObjectId);
                }

                if (insuredValue > 0)
                {
                    insuredObject.Amount = insuredValue;
                    insuredObjects.Add(insuredObject);
                }

                return new UifJsonResult(true, insuredObjects);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        public ActionResult Quotate(QuotationViewModel quotationViewModel, List<CompanyInsuredObject> companyInsuredObjects)
        {
            try
            {
                if (companyInsuredObjects != null && companyInsuredObjects.Count > 0)
                {
                    ValidateCustomer(quotationViewModel);
                    CreateCustomer(quotationViewModel);

                    CompanyPropertyRisk property = CreateCompanyPropertyRisk(quotationViewModel, companyInsuredObjects);
                    property = DelegateService.quotationService.QuoteProperty(property);
                    SetPremiumInsuredObjects(property);

                    return new UifJsonResult(true, property);
                }
                else
                {
                    return new UifJsonResult(false, Language.ValidationMinimumInsuredObjects);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        public ActionResult GetQuotationByQuotationId(int quotationId)
        {
            try
            {
                List<CompanyPolicy> policies = DelegateService.quotationService.GetCompanyPoliciesByQuotationIdVersionPrefixId(quotationId, 0, 0, 0);
                policies = policies.Where(x => x.Prefix.Id == prefixId).ToList();

                if (policies.Count > 0)
                {
                    return new UifJsonResult(true, policies);
                }
                else
                {
                    return new UifJsonResult(false, Language.MessageQuotationNotExist);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        public ActionResult GetQuotationByTemporalId(int temporalId)
        {
            try
            {
                CompanyPropertyRisk property = DelegateService.propertyService.GetCompanyPropertiesByTemporalId(temporalId).First();
                property.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);
                SetPremiumInsuredObjects(property);

                return new UifJsonResult(true, property);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        public ActionResult GenerateReport(int temporalId)
        {
            try
            {
                CompanyFilterReport companyFilterReport = new CompanyFilterReport
                {
                    TemporalId = temporalId,
                    User = new User
                    {
                        AccountName = SessionHelper.GetUserName()
                    }
                };

                return new UifJsonResult(true, DelegateService.printingService.GenerateReport(companyFilterReport));
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        public ActionResult SendToEmail(int temporalId)
        {
            try
            {
                CompanyFilterReport companyFilterReport = new CompanyFilterReport
                {
                    TemporalId = temporalId,
                    User = new User
                    {
                        AccountName = SessionHelper.GetUserName()
                    }
                };

                string urlFile = DelegateService.printingService.GenerateReport(companyFilterReport).Substring(5);

                CompanyPropertyRisk property = DelegateService.propertyService.GetCompanyPropertiesByTemporalId(temporalId).First();
                property.Policy = DelegateService.underwritingService.GetCompanyPolicyByTemporalId(temporalId, false);

                EmailCriteria emailCriteria = new EmailCriteria();
                emailCriteria.Addressed = new List<string>();
                emailCriteria.Addressed.Add(property.MainInsured.CompanyName.Email.Description);
                emailCriteria.Subject = Language.LabelQuotation;
                emailCriteria.Files = new List<string>();
                emailCriteria.Files.Add(urlFile);

                StringBuilder builder = new StringBuilder(Language.ContentEmailQuotation);
                builder.Replace("{{logo}}", ConfigurationManager.AppSettings["LogoCompany"]);
                builder.Replace("{{quotation}}", property.Policy.Endorsement.QuotationId.ToString());
                builder.Replace("{{date}}", property.Policy.BeginDate.ToShortDateString());
                builder.Replace("{{personName}}", property.Policy.Holder.Name);
                builder.Replace("{{fullAddress}}", property.Description);
                builder.Replace("{{value}}", property.Policy.Summary.AmountInsured.ToString("C2"));
                builder.Replace("{{premium}}", property.Policy.Summary.Premium.ToString("C2"));
                builder.Replace("{{taxes}}", property.Policy.Summary.Taxes.ToString("C2"));
                builder.Replace("{{expenses}}", property.Policy.Summary.Expenses.ToString("C2"));
                builder.Replace("{{total}}", property.Policy.Summary.FullPremium.ToString("C2"));

                StringBuilder builderDetail = new StringBuilder();

                List<int> insuredObjectIds = property.Coverages.Select(x => x.InsuredObject.Id).Distinct().ToList();

                foreach (int insuredObjectId in insuredObjectIds)
                {
                    StringBuilder builderItem = new StringBuilder(Language.ContentInsuredObject);
                    builderItem.Replace("{{itemTitle}}", property.Coverages.First(x => x.InsuredObject.Id == insuredObjectId).Description);
                    builderItem.Replace("{{itemValue}}", property.Coverages.First(x => x.InsuredObject.Id == insuredObjectId).PremiumAmount.ToString("C2"));
                    builderDetail.Append(builderItem.ToString());

                    foreach (CompanyCoverage coverage in property.Coverages.Where(x => x.InsuredObject.Id == insuredObjectId))
                    {
                        builderItem = new StringBuilder(Language.ContentCoverage);

                        builderItem.Replace("{{itemTitle}}", coverage.Description);

                        if (coverage.Deductible != null)
                        {
                            builderItem.Replace("{{itemDeductible}}", coverage.Deductible.Description);
                        }
                        else
                        {
                            builderItem.Replace("{{itemDeductible}}", "");
                        }

                        builderDetail.Append(builderItem.ToString());
                    }
                }

                builder.Replace("{{itemDetails}}", builderDetail.ToString());

                emailCriteria.Message = builder.ToString();

                bool sendEmail = DelegateService.commonService.SendEmailAsync(emailCriteria).Result;

                if (sendEmail)
                {
                    return new UifJsonResult(true, Language.MessageEmailSend);
                }
                else
                {
                    return new UifJsonResult(false, Language.ErrorSendEmail);
                }
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        private void ValidateCustomer(QuotationViewModel quotationViewModel)
        {
            StringBuilder stringBuilder = new StringBuilder();

            if (quotationViewModel.CustomerId == 0)
            {
                if (quotationViewModel.IndividualTypeId == (int)UPEN.IndividualType.Person)
                {
                    if (string.IsNullOrEmpty(quotationViewModel.Names))
                    {
                        stringBuilder.Append(Language.ValidationRequiredNames + " <br/> ");
                    }

                    if (string.IsNullOrEmpty(quotationViewModel.Surname))
                    {
                        stringBuilder.Append(Language.ValidationRequiredSurname + " <br/> ");
                    }

                    if (string.IsNullOrEmpty(quotationViewModel.PersonAddress))
                    {
                        stringBuilder.Append(Language.ValidationRequiredCorrespondenceAddress + " <br/> ");
                    }

                    if (string.IsNullOrEmpty(quotationViewModel.PersonEmail))
                    {
                        stringBuilder.Append(Language.ValidationRequiredEmail);
                    }
                }
                else if (quotationViewModel.IndividualTypeId == (int)UPEN.IndividualType.LegalPerson)
                {
                    if (string.IsNullOrEmpty(quotationViewModel.TradeName))
                    {
                        stringBuilder.Append(Language.ValidationRequiredTradeName + " <br/> ");
                    }

                    if (string.IsNullOrEmpty(quotationViewModel.CompanyAddress))
                    {
                        stringBuilder.Append(Language.ValidationRequiredCorrespondenceAddress + " <br/> ");
                    }

                    if (string.IsNullOrEmpty(quotationViewModel.CompanyEmail))
                    {
                        stringBuilder.Append(Language.ValidationRequiredEmail);
                    }
                }

                if (stringBuilder.Length > 0)
                {
                    throw new Exception(stringBuilder.ToString());
                }
            }
        }

        private void CreateCustomer(QuotationViewModel quotationViewModel)
        {
            if (quotationViewModel.CustomerTypeId != (int)UPEN.CustomerType.Individual)
            {
                Prospect prospect = CreateProspect(quotationViewModel);

                if (prospect.Id == 0)
                {
                    prospect = DelegateService.uniquePersonService.CreateProspect(prospect);

                    quotationViewModel.CustomerId = prospect.Id;
                    quotationViewModel.CustomerTypeId = (int)UPEN.CustomerType.Prospect;
                }
                else
                {
                    prospect = DelegateService.uniquePersonService.UpdateProspect(prospect);
                }
            }
        }

        private Prospect CreateProspect(QuotationViewModel quotationViewModel)
        {
            Prospect prospect = new Prospect
            {
                Id = quotationViewModel.CustomerId,
                IndividualType = (UPEN.IndividualType)quotationViewModel.IndividualTypeId,
                IdentificationDocument = new IdentificationDocument
                {
                    Number = quotationViewModel.DocumentNumber,
                    DocumentType = new DocumentType
                    {
                        Id = quotationViewModel.DocumentTypeId
                    }
                }
            };

            if (prospect.IndividualType == UPEN.IndividualType.Person)
            {
                prospect.Name = quotationViewModel.Names;
                prospect.Surname = quotationViewModel.Surname;
                prospect.SecondSurname = quotationViewModel.SecondSurname;
                prospect.BirthDate = Convert.ToDateTime(quotationViewModel.BirthDate);
                prospect.Gender = quotationViewModel.Gender;
                prospect.CompanyName = new CompanyName
                {
                    Address = new Address
                    {
                        Description = quotationViewModel.PersonAddress
                    },
                    Phone = new Phone
                    {
                        Description = quotationViewModel.PersonPhone
                    },
                    Email = new Email
                    {
                        Description = quotationViewModel.PersonEmail
                    }
                };
            }
            else if (prospect.IndividualType == UPEN.IndividualType.LegalPerson)
            {
                prospect.TradeName = quotationViewModel.TradeName;
                prospect.CompanyName = new CompanyName
                {
                    Address = new Address
                    {
                        Description = quotationViewModel.CompanyAddress
                    },
                    Phone = new Phone
                    {
                        Description = quotationViewModel.CompanyPhone
                    },
                    Email = new Email
                    {
                        Description = quotationViewModel.CompanyEmail
                    }
                };
            }

            return prospect;
        }

        private CompanyPropertyRisk CreateCompanyPropertyRisk(QuotationViewModel quotationViewModel, List<CompanyInsuredObject> companyInsuredObjects)
        {
            CompanyPropertyRisk property = new CompanyPropertyRisk
            {
                Description = quotationViewModel.FullAddress,
                FullAddress = quotationViewModel.FullAddress,
                ConstructionYear = quotationViewModel.ConstructionYear,
                City = new City
                {
                    Id = quotationViewModel.CityId,
                    State = new State
                    {
                        Id = quotationViewModel.StateId,
                        Country = new Country
                        {
                            Id = quotationViewModel.CountryId
                        }
                    }
                },
                GroupCoverage = new GroupCoverage
                {
                    Id = quotationViewModel.CoverageGroupId
                },
                MainInsured = new CompanyInsured
                {
                    IndividualId = quotationViewModel.CustomerId,
                    CustomerType = (UPEN.CustomerType)quotationViewModel.CustomerTypeId
                },
                Policy = new CompanyPolicy
                {
                    Id = quotationViewModel.TemporalId,
                    Endorsement = new Endorsement
                    {
                        QuotationId = quotationViewModel.QuotationId,
                        QuotationVersion = quotationViewModel.QuotationVersion
                    },
                    Holder = new Holder
                    {
                        IndividualId = quotationViewModel.CustomerId,
                        CustomerType = (UPEN.CustomerType)quotationViewModel.CustomerTypeId
                    },
                    Prefix = new Prefix
                    {
                        Id = prefixId
                    },
                    Product = new CompanyProduct
                    {
                        Id = quotationViewModel.ProductId
                    },
                    UserId = SessionHelper.GetUserId()
                },
                Coverages = new List<CompanyCoverage>()
            };

            if (property.Policy.Endorsement.QuotationVersion == 0)
            {
                property.Policy.Endorsement.QuotationVersion = 1;
            }

            foreach (CompanyInsuredObject companyInsuredObject in companyInsuredObjects)
            {
                property.Coverages.Add(new CompanyCoverage
                {
                    InsuredObject = companyInsuredObject
                });
            }

            return property;
        }

        private void SetPremiumInsuredObjects(CompanyPropertyRisk property)
        {
            foreach (CompanyCoverage coverage in property.Coverages)
            {
                coverage.InsuredObject.Premium = property.Coverages.Where(x => x.InsuredObject.Id == coverage.InsuredObject.Id).ToList().Sum(y => y.PremiumAmount);
            }
        }

        //public ActionResult ValidateIntermediates()
        //{
        //    try
        //    {
        //        if (DelegateService.uniqueUserService.GetCountAgenciesByUserId(SessionHelper.GetUser()) == 0)
        //        {
        //            return new UifJsonResult(false, App_GlobalResources.Language.ErrorUserWithOutIntermediates);
        //        }
        //        else
        //        {
        //            return new UifJsonResult(true, null);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchAgency);
        //    }
        //}

        #region Advanced search quoation
        public PartialViewResult QuotationAdvancedSearch()
        {
            return PartialView();
        }

        #endregion

        public ActionResult GetUsersByQuery(string query)
        {
            try
            {
                int personId;
                bool isNumeric = int.TryParse(query, out personId);
                if (isNumeric)
                {
                    query = "";
                }

                List<User> users = DelegateService.uniqueUserService.GetUserByTextPersonId(query, personId);
                return Json(users, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetUser, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult GetPersonsByQuery(string query)
        {
            try
            {
                List<Prospect> prospects = DelegateService.uniquePersonService.GetAdvancedProspectByDescription(query);
                foreach (Prospect item in prospects)
                {
                    if (item.TradeName == null || item.TradeName == "")
                    {
                        item.TradeName = item.Name + " " + item.Surname;
                    }
                }
                return Json(prospects, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorGetUser, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetPoliciesByPolicy(CompanyPropertyRisk policy)
        {
            try
            {                
                List<CompanyPropertyRisk> policies = DelegateService.quotationService.GetCompanyPropertyByCompanyPolicy(policy);
                if (policies.Count > 0)
                {
                    return new UifJsonResult(true, policies.OrderBy(b => b.Policy.Endorsement.QuotationId).ToList());
                }
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorTempNoExist);

            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetQuotationAdv);
            }
        }
    }
}