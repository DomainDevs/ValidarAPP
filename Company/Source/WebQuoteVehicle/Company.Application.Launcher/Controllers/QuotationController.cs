using Newtonsoft.Json.Linq;
using Sistran.Company.Application.ModelServices.Models.Param;
using Sistran.Company.Application.PrintingServices.Models;
using Sistran.Company.Application.ProductServices.Models;
using Sistran.Company.Application.QuotationServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Company.Application.Vehicles.Models;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.RulesScriptsServices.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Enums = Sistran.Core.Application.UniquePersonService.Enums;
using VEMO = Sistran.Core.Application.Vehicles.VehicleServices.Models;
using ENUMSM = Sistran.Core.Application.ModelServices.Enums;
using System.IO;
using System.Threading.Tasks;
using Sistran.Core.Application.Utilities.Helper;
using System.Net;

namespace Sistran.Core.Framework.UIF.Web.Controllers
{
    [Authorize]
    public class QuotationController : Controller
    {

        private static int policyTypeId = Convert.ToInt32(ConfigurationManager.AppSettings["PolicyTypeId"]);
        private static int prefixId = Convert.ToInt32(ConfigurationManager.AppSettings["PrefixId"]);
        private static int branchId = Convert.ToInt32(ConfigurationManager.AppSettings["BranchId"]);
        private static int entityId = Convert.ToInt32(ConfigurationManager.AppSettings["EntityId"]);
        private static int hasRCExcessId = Convert.ToInt32(ConfigurationManager.AppSettings["HasRCExcessId"]);
        private static int rcExcessId = Convert.ToInt32(ConfigurationManager.AppSettings["RCExcessId"]);
        private static int discountId = Convert.ToInt32(ConfigurationManager.AppSettings["DiscountId"]);

        private static List<Make> makes = new List<Make>();
        private static List<VEMO.Use> uses = new List<VEMO.Use>();
        private static List<Sistran.Core.Application.Vehicles.Models.Type> types = new List<Sistran.Core.Application.Vehicles.Models.Type>();
        private static List<RatingZone> ratingZones = new List<RatingZone>();
        private static List<ListEntityValue> discounts = new List<ListEntityValue>();
        private static List<CompanyProduct> products = new List<CompanyProduct>();
        private static List<ListEntityValue> rcExcesses = new List<ListEntityValue>();
        private static List<LimitRCExecess> rcExcessesJSON = new List<LimitRCExecess>();
        private static List<LimitRc> limitRcs = new List<LimitRc>();


        public ActionResult Vehicle()
        {
            try
            {
                QuotationModelsView quotationModel = new QuotationModelsView();
                quotationModel.Title = App_GlobalResources.Language.TitleQuotation;

                return View(quotationModel);

            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult QuotationVehicleSearch()
        {
            try
            {
                QuotationModelsView quotationModel = new QuotationModelsView();
                quotationModel.Title = App_GlobalResources.Language.TitleQuotation;

                return View(quotationModel);

            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult ValidateIntermediates()
        {
            try
            {
                if (DelegateService.uniqueUserService.GetCountAgenciesByUserId(SessionHelper.GetUserId()) == 0)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorUserWithOutIntermediates);
                }
                else
                {
                    return new UifJsonResult(true, null);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchAgency);
            }
        }

        public ActionResult GetVehicleByPlateFasecoldaCode(string plate, string fasecoldaCode)
        {
            try
            {
                CompanyVehicle vehicle = null;

                if (!string.IsNullOrEmpty(plate))
                {
                    vehicle = DelegateService.vehicleService.GetVehicleByLicensePlate(plate);
                }

                if (vehicle != null)
                {
                    return new UifJsonResult(true, vehicle);
                }
                else if (!string.IsNullOrEmpty(fasecoldaCode))
                {
                    vehicle = DelegateService.vehicleService.GetVehicleByFasecoldaCode(fasecoldaCode, DateTime.Now.Year);

                    if (vehicle != null)
                    {
                        return new UifJsonResult(true, vehicle);
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorVehicleNoFound);
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorVehicleNoFound);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchVehicle);
            }
        }

        public ActionResult GetMakes()
        {
            try
            {
                if (makes.Count == 0)
                {
                    makes = DelegateService.vehicleService.GetMakes();
                }
                return new UifSelectResult(makes.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifSelectResult(App_GlobalResources.Language.ErrorSearchMakes);
            }
        }

        public ActionResult GetModelsByMakeId(int makeId)
        {
            try
            {
                List<Model> models = DelegateService.vehicleService.GetModelsByMakeId(makeId);
                return new UifSelectResult(models.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifSelectResult(App_GlobalResources.Language.ErrorSearchModels);
            }
        }

        public ActionResult GetVersionsByMakeIdModelId(int makeId, int modelId)
        {
            try
            {
                List<Sistran.Core.Application.Vehicles.Models.Version> versions = DelegateService.vehicleService.GetVersionsByMakeIdModelId(makeId, modelId);
                return new UifSelectResult(versions.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifSelectResult(App_GlobalResources.Language.ErrorSearchVersions);
            }
        }

        public ActionResult GetUses()
        {
            try
            {
                if (uses.Count == 0)
                {
                    uses = DelegateService.vehicleService.GetUses();
                }
                return new UifSelectResult(uses.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifSelectResult(App_GlobalResources.Language.ErrorSearchUses);
            }
        }

        public ActionResult GetTypesByTypeId(int typeId)
        {
            try
            {
                if (types.Count == 0)
                {
                    types = DelegateService.vehicleService.GetTypes();
                }
                return new UifSelectResult(types.Where(x => x.Id == typeId).OrderBy(y => y.Description));
            }
            catch (Exception)
            {
                return new UifSelectResult(false, App_GlobalResources.Language.ErrorSearchTypes);
            }
        }

        public ActionResult GetYearsByMakeIdModelIdVersionId(int makeId, int modelId, int versionId)
        {
            try
            {
                List<Year> years = DelegateService.vehicleService.GetYearsByMakeIdModelIdVersionId(makeId, modelId, versionId);
                return new UifSelectResult(years.Where(x => x.Price != 0).OrderByDescending(x => x.Description));
            }
            catch (Exception)
            {
                return new UifSelectResult(false, App_GlobalResources.Language.ErrorSearchYears);
            }
        }

        public ActionResult GetVehicleByMakeIdModelIdVersionId(int makeId, int modelId, int versionId)
        {
            try
            {
                CompanyVehicle vehicle = DelegateService.vehicleService.GetVehicleByMakeIdModelIdVersionId(makeId, modelId, versionId);

                if (vehicle != null)
                {
                    return new UifJsonResult(true, vehicle);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorVehicleNoFound);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchVehicle);
            }
        }

        public ActionResult GetPriceByMakeIdModelIdVersionId(int makeId, int modelId, int versionId, int year)
        {
            try
            {
                return new UifJsonResult(true, DelegateService.vehicleService.GetPriceByMakeIdModelIdVersionId(makeId, modelId, versionId, year));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchPriceVehicle);
            }
        }

        public ActionResult GetRatingZones()
        {
            try
            {
                if (ratingZones != null)
                {
                    ratingZones = DelegateService.underwritingService.GetRatingZonesByPrefixId(prefixId);
                }
                return new UifSelectResult(ratingZones.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifSelectResult(App_GlobalResources.Language.ErrorSearchRatingZones);
            }
        }

        public ActionResult GetDocumentTypesByIndividualType(int individualType)
        {
            try
            {
                List<DocumentType> documentTypes = DelegateService.uniquePersonService.GetDocumentTypes(individualType);
                documentTypes = documentTypes.OrderBy(x => x.Description).ToList();
                return new UifSelectResult(documentTypes, documentTypes.First().Id);
            }
            catch (Exception)
            {
                return new UifSelectResult(false, App_GlobalResources.Language.ErrorSearchDocumentType);
            }
        }

        public ActionResult GetDiscounts()
        {
            try
            {
                if (discounts != null)
                {
                    discounts = DelegateService.rulesService.GetListEntityValuesByConceptIdEntityId(discountId, entityId);
                }
                return new UifSelectResult(discounts ?? new List<ListEntityValue>());
            }
            catch (Exception)
            {
                return new UifSelectResult(App_GlobalResources.Language.ErrorSearchDiscounts);
            }
        }

        public ActionResult GetProducts()
        {
            try
            {
                if (products != null)
                {
                    var agencyId = 0;
                    if (AccountController.UsersSession.Count > 0)
                    {
                        agencyId = AccountController.UsersSession[0].AgencyId;
                    }
                    products = DelegateService.underwritingService.GetCompanyProductsByAgentIdPrefixIdIsGreen(agencyId, prefixId, true);
                }
                return new UifSelectResult(products);
            }
            catch (Exception ex)
            {
                return new UifSelectResult(App_GlobalResources.Language.ErrorSearchProducts);
            }
        }

        public ActionResult GetGroupCoveragesByProductId(int productId)
        {
            try
            {
                List<GroupCoverage> groupCoverages = DelegateService.underwritingService.GetGroupCoverages(productId);
                return new UifSelectResult(groupCoverages.OrderBy(x => x.Description));
            }
            catch (Exception)
            {
                return new UifSelectResult(App_GlobalResources.Language.ErrorSearchGroupCoverages);
            }
        }

        public ActionResult GetCoveragesByProductIdGroupCoverageId(int productId, int groupCoverageId)
        {
            try
            {
                List<Coverage> coverages = DelegateService.underwritingService.GetCoveragesByProductIdGroupCoverageIdPrefixId(productId, groupCoverageId, prefixId);
                return Json(coverages.Where(x => x.IsSelected == true).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(App_GlobalResources.Language.ErrorSearchCoverages);
            }
        }

        public ActionResult GetRCExcesses()
        {
            try
            {
                if (rcExcesses != null)
                {
                    rcExcesses = DelegateService.rulesService.GetListEntityValuesByConceptIdEntityId(rcExcessId, entityId);
                }
                return new UifSelectResult(rcExcesses);
            }
            catch (Exception)
            {
                return new UifSelectResult(App_GlobalResources.Language.ErrorSearchRCExcesses);
            }
        }

        public ActionResult GetLimitRcByFilter(int productId)
        {
            try
            {
                if (limitRcs != null)
                {
                    limitRcs = DelegateService.underwritingService.GetLimitsRcByPrefixIdProductIdPolicyTypeId(prefixId, productId, policyTypeId);
                    return new UifSelectResult(limitRcs ?? new List<LimitRc>());
                }
                return new UifSelectResult(limitRcs);
            }
            catch (Exception ex)
            {
                return new UifSelectResult(App_GlobalResources.Language.ErrorSearchRCExcesses);
            }
        }

        public ActionResult GetProspectByIndividualTypeDocumentTypeIdDocument(IndividualType individualType, int documentTypeId, string document)
        {
            try
            {
                Application.UniquePersonService.Models.Prospect prospect = DelegateService.uniquePersonService.GetProspectByIndividualTypeDocumentTypeIdDocument((Enums.IndividualType)individualType, documentTypeId, document);

                if (prospect != null)
                {
                    return new UifJsonResult(true, prospect);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchDocument);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchCustomer);
            }
        }

        public ActionResult Quote(QuotationModelsView quotationModel)
        {
            try
            {
                string messageValidateModel = ValidateModel(quotationModel);

                if (messageValidateModel.Length == 0)
                {
                    if (quotationModel.CustomerType != (int)CustomerType.Individual)
                    {
                        Application.UniquePersonService.Models.Prospect prospect = GetProspect(quotationModel);

                        if (prospect.Id == 0)
                        {
                            // Se consulta el prospecto por número de documento
                            Application.UniquePersonService.Models.Prospect tmpProspect =
                                DelegateService.uniquePersonService.GetProspectByIndividualTypeDocumentTypeIdDocument(
                                    (Core.Application.UniquePersonService.Enums.IndividualType)quotationModel.IndividualType,
                                    prospect.IdentificationDocument.DocumentType.Id,
                                    prospect.IdentificationDocument.Number);

                            if (tmpProspect != null && tmpProspect.Id > 0)
                            {
                                prospect.Id = tmpProspect.Id;
                                quotationModel.CustomerId = prospect.Id;
                                quotationModel.CustomerType = (int)prospect.CustomerType;
                            }
                        }

                        if (prospect.Id == 0)
                        {
                            prospect = DelegateService.uniquePersonService.CreateProspect(prospect);

                            quotationModel.CustomerId = prospect.Id;
                            quotationModel.CustomerType = (int)CustomerType.Prospect;
                        }
                        else
                        {
                            prospect = DelegateService.uniquePersonService.UpdateProspect(prospect);
                        }
                    }

                    CompanyVehicle vehiclePolicy = GetVehicle(quotationModel);
                    vehiclePolicy = DelegateService.quotationService.QuoteVehicle(vehiclePolicy);

                    if (vehiclePolicy != null)
                    {
                        return new UifJsonResult(true, vehiclePolicy);
                    }
                    else
                    {
                        return new UifJsonResult(false, App_GlobalResources.Language.ErrorQuote);
                    }
                }
                else
                {
                    return new UifJsonResult(false, messageValidateModel);
                }


            }
            catch (Exception ex)
            {
                string[] messages = ex.Message.Split('|');

                if (messages[0] == "Sistran.Core.Framework.BAF.BusinessException")
                {
                    string message = (string)HttpContext.GetGlobalResourceObject("Language", messages[1]);

                    if (string.IsNullOrEmpty(message))
                    {
                        return new UifJsonResult(false, messages[1]);
                    }
                    else
                    {
                        return new UifJsonResult(false, message);
                    }
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorQuote);
                }
            }
        }

        private string ValidateModel(QuotationModelsView quotationModel)
        {
            if (string.IsNullOrEmpty(quotationModel.FasecoldaCode))
            {
                return App_GlobalResources.Language.ErrorRequiredFasecoldaCore;
            }

            if (quotationModel.MakeId == 0)
            {
                return App_GlobalResources.Language.ErrorRequiredMake;
            }

            if (quotationModel.ModelId == 0)
            {
                return App_GlobalResources.Language.ErrorRequiredModel;
            }

            if (quotationModel.VersionId == 0)
            {
                return App_GlobalResources.Language.ErrorRequiredVersion;
            }

            if (quotationModel.Use == 0)
            {
                return App_GlobalResources.Language.ErrorRequiredUse;
            }

            if (quotationModel.Type == 0)
            {
                return App_GlobalResources.Language.ErrorRequiredType;
            }

            if (quotationModel.Year == 0)
            {
                return App_GlobalResources.Language.ErrorRequiredYear;
            }

            if (quotationModel.InsuredAmount <= 0)
            {
                return App_GlobalResources.Language.ErrorRequiredInsuredAmount;
            }

            if (quotationModel.RatingZone == 0)
            {
                return App_GlobalResources.Language.ErrorRequiredRatingZone;
            }

            if (quotationModel.Product == 0)
            {
                return App_GlobalResources.Language.ErrorRequiredProduct;
            }

            if (quotationModel.GroupCoverage == 0)
            {
                return App_GlobalResources.Language.ErrorRequiredGroupCoverage;
            }

            if (string.IsNullOrEmpty(quotationModel.Document))
            {
                return App_GlobalResources.Language.ErrorRequiredDocumentNumber;
            }

            if (quotationModel.IndividualType == (int)IndividualType.Person)
            {
                if (string.IsNullOrEmpty(quotationModel.Names))
                {
                    return App_GlobalResources.Language.ErrorRequiredName;
                }

                if (string.IsNullOrEmpty(quotationModel.Surname))
                {
                    return App_GlobalResources.Language.ErrorRequiredSurname;
                }

                if (string.IsNullOrEmpty(quotationModel.BirthDate))
                {
                    return App_GlobalResources.Language.ErrorRequiredBirthDate;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(quotationModel.TradeName))
                {
                    return App_GlobalResources.Language.ErrorRequiredTradeName;
                }
            }

            return "";
        }

        public ActionResult GetQuotationByQuotationId(int quotationId)
        {
            try
            {
                CompanyVehicle vehiclePolicy = DelegateService.quotationService.GetCompanyVehicleByQuotationId(quotationId);

                if (vehiclePolicy != null)
                {
                    List<DynamicConcept> dynamicConcepts = new List<DynamicConcept>();

                    if (vehiclePolicy.Risk.DynamicProperties.Exists(x => x.Id == discountId))
                    {
                        dynamicConcepts.Add(vehiclePolicy.Risk.DynamicProperties.First(x => x.Id == discountId));
                    }
                    if (vehiclePolicy.Risk.DynamicProperties.Exists(x => x.Id == rcExcessId))
                    {
                        dynamicConcepts.Add(vehiclePolicy.Risk.DynamicProperties.First(x => x.Id == rcExcessId));
                    }

                    vehiclePolicy.Risk.DynamicProperties = dynamicConcepts;

                    return new UifJsonResult(true, vehiclePolicy);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorQuotationNoExist);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchQuotation);
            }
        }

        private Application.UniquePersonService.Models.Prospect GetProspect(QuotationModelsView quotationModel)
        {
            Application.UniquePersonService.Models.Prospect prospect = new Application.UniquePersonService.Models.Prospect
            {
                Id = quotationModel.CustomerId,
                IndividualType = (Enums.IndividualType)quotationModel.IndividualType,
                IdentificationDocument = new IdentificationDocument
                {
                    Number = quotationModel.Document,
                    DocumentType = new DocumentType
                    {
                        Id = quotationModel.DocumentType
                    }
                },
                CompanyName = new CompanyName
                {
                    Address = new Address
                    {
                        Description = quotationModel.Address
                    },
                    Phone = new Phone
                    {
                        Description = quotationModel.Phone
                    },
                    Email = new Email
                    {
                        Description = quotationModel.Email
                    }
                }
            };

            if (prospect.IndividualType == Enums.IndividualType.Person)
            {
                prospect.Name = quotationModel.Names;
                prospect.Surname = quotationModel.Surname;
                prospect.SecondSurname = quotationModel.SecondSurname;
                prospect.BirthDate = Convert.ToDateTime(quotationModel.BirthDate);
                prospect.Gender = quotationModel.Gender;
            }
            else if (prospect.IndividualType == Enums.IndividualType.LegalPerson)
            {
                prospect.TradeName = quotationModel.TradeName;
            }

            return prospect;
        }

        private CompanyPolicy GetPolicy(QuotationModelsView quotationModel)
        {
            CompanyPolicy policy = new CompanyPolicy
            {
                Id = quotationModel.Id,
                Endorsement = new Company.Application.UnderwritingServices.CompanyEndorsement
                {
                    TemporalId = quotationModel.TemporalId.GetValueOrDefault(),
                    QuotationId = quotationModel.QuotationId.GetValueOrDefault(),
                    QuotationVersion = quotationModel.QuotationVersion.GetValueOrDefault(1)
                },
                Holder = new Holder
                {
                    IndividualId = quotationModel.CustomerId,
                    CustomerType = (Core.Services.UtilitiesServices.Enums.CustomerType)quotationModel.CustomerType,
                    IndividualType = Core.Services.UtilitiesServices.Enums.IndividualType.Company
                },
                Prefix = new Company.Application.CommonServices.Models.CompanyPrefix
                {
                    Id = prefixId
                },
                Product = new CompanyProduct
                {
                    Id = quotationModel.Product
                },
                UserId = SessionHelper.GetUserId()
            };

            return policy;
        }

        private CompanyVehicle GetVehicle(QuotationModelsView quotationModel)
        {
            CompanyVehicle vehicle = new CompanyVehicle
            {
                //ReplacementVehicle = quotationModel.ReplacementVehicle,
                Risk = new CompanyRisk()
                {
                    Policy = GetPolicy(quotationModel),
                    Description = string.IsNullOrEmpty(quotationModel.Plate) ? "" : quotationModel.Plate,
                    RatingZone = new CompanyRatingZone()
                    {
                        Id = quotationModel.RatingZone
                    },
                    MainInsured = new CompanyIssuanceInsured()
                    {
                        IndividualId = quotationModel.CustomerId,
                        CustomerType = (Core.Services.UtilitiesServices.Enums.CustomerType)quotationModel.CustomerType,
                        IndividualType = (Core.Services.UtilitiesServices.Enums.IndividualType)quotationModel.IndividualType

                    },
                    GroupCoverage = new GroupCoverage
                    {
                        Id = quotationModel.GroupCoverage
                    },

                    LimitRc = new CompanyLimitRc
                    {
                        Id = quotationModel.LimitRC
                    },
                    Number = quotationModel.RiskNum
                },
                Make = new CompanyMake()
                {
                    Id = quotationModel.MakeId,
                    Description = quotationModel.MakeDescription
                },
                Model = new CompanyModel()
                {
                    Id = quotationModel.ModelId,
                    Description = quotationModel.ModelDescription
                },
                Version = new CompanyVersion()
                {
                    Id = quotationModel.VersionId,
                    Description = quotationModel.VersionDescription,
                    Type = new Company.Application.Vehicles.Models.CompanyType()
                    {
                        Id = quotationModel.Type
                    },
                    Fuel = new CompanyFuel()
                    {
                        Id = 1
                    },
                    Body = new CompanyBody()
                    {
                        Id = 1
                    }
                },
                LicensePlate = string.IsNullOrEmpty(quotationModel.Plate) ? "" : quotationModel.Plate,
                Fasecolda = new Fasecolda
                {
                    Description = quotationModel.FasecoldaCode
                },
                Use = new CompanyUse()
                {
                    Id = quotationModel.Use
                },
                Year = quotationModel.Year,
                Price = quotationModel.InsuredAmount,
                PriceAccesories = quotationModel.AmountAccesories,
                ServiceType = new CompanyServiceType { Id = 1 }
            };
            vehicle.Accesories= new List<CompanyAccessory>();
            vehicle.Risk.DynamicProperties = new List<DynamicConcept>();
            vehicle.Risk.DynamicProperties.Add(new DynamicConcept
            {
                Id = discountId,
                EntityId = entityId,
                Value = quotationModel.Discount

            });
            vehicle.Risk.DynamicProperties.Add(new DynamicConcept
            {
                Id = 6018,
                EntityId = entityId,
                Value = quotationModel.ReplacementVehicle
            });
           
            return vehicle;
        }

        public ActionResult DownloadPDF(int quotationId)
        {
            try
            {
                if (quotationId > 0)
                {
                    CompanyVehicle vehiclePolicy = DelegateService.quotationService.GetCompanyVehicleByQuotationId(quotationId);

                    CompanyFilterReport companyFilterReport = new CompanyFilterReport
                    {
                        TemporalId = vehiclePolicy.Risk.Policy.Id,
                        User = new User
                        {
                            AccountName = SessionHelper.GetUserName()
                        }
                    };

                    return new UifJsonResult(true, DelegateService.printingService.GenerateReport(companyFilterReport));
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorQuotationNumber);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGenerateFile);
            }
        }

        public ActionResult GetCompanyQuotationVehicleSearch(int tempId)
        {
            try
            {
                List<CompanyQuotationVehicleSearch> quotationVehicleSearch = DelegateService.quotationService.CompanyQuotationVehicleSearches(tempId);
                return new UifSelectResult(quotationVehicleSearch);
            }
            catch (Exception)
            {
                return new UifSelectResult(App_GlobalResources.Language.ErrorSearchGroupCoverages);
            }
        }

        public ActionResult GenerateReportQuotation(int temporaryId, int prefixId, int quotationId, int versionId)
        {
            try
            {
                int iIntestosImpresion = int.Parse(ConfigurationManager.AppSettings["IntentosImpresion"].ToString());
                bool bError = false;
                string strPath = DelegateService.printingService.GenerateReportJetForm(
                    new QuotationInfo
                    {
                        TempId = temporaryId,
                        QuotationId = quotationId,
                        VersionId = versionId,
                        CommonProperties =
                    new CommonProperties { PrefixId = prefixId, UserName = SessionHelper.GetUserName(), IsMassive = false }
                    });
                int IsExternal = int.Parse(ConfigurationManager.AppSettings["IsExternal"].ToString());
                var filenamefromPath = strPath.Split(new char[] { '\\' }).Last();

                if (IsExternal == 1)
                {
                    string user = DelegateService.commonService.GetKeyApplication("UserDomain");
                    string password = DelegateService.commonService.GetKeyApplication("DomainPassword");
                    string path = DelegateService.commonService.GetKeyApplication("UserRemotePath");

                    using (NetworkConnection networkCon = new NetworkConnection(path, new NetworkCredential(user, password)))
                    {
                        if (networkCon._resultConnection == 0)
                        {
                            for (int i = 0; i < iIntestosImpresion; i++)
                            {
                                if (!System.IO.File.Exists(strPath))
                                {
                                    System.Threading.Thread.Sleep(1000);
                                    bError = true;
                                }
                                else
                                {
                                    bError = false;
                                    break;
                                }
                            }

                            if (!bError)
                                return new UifJsonResult(true, new { Url = this.Url.Action("ShowPdfFile", "Quotation") + "?url=" + strPath, Filename = filenamefromPath, FilePathResult = strPath });
                            else
                            {
                                CompanyPrinting companyPrinting = DelegateService.printingService.SavePrintingProcess(strPath, 3, SessionHelper.GetUserId(), 1, 0);
                                return new UifJsonResult(true, companyPrinting.Id);
                            }
                        }
                        else
                            return new UifJsonResult(false, App_GlobalResources.Language.ErrorGenerateFile);
                    }
                }
                else
                {
                    for (int i = 0; i < iIntestosImpresion; i++)
                    {
                        if (!System.IO.File.Exists(strPath))
                        {
                            System.Threading.Thread.Sleep(1000);
                            bError = true;
                        }
                        else
                        {
                            bError = false;
                            break;
                        }
                    }
                    if (!bError)
                        return new UifJsonResult(true, new { Url = this.Url.Action("ShowPdfFile", "Quotation") + "?url=" + strPath, Filename = filenamefromPath, FilePathResult = strPath });
                    else
                    {
                        CompanyPrinting companyPrinting = DelegateService.printingService.SavePrintingProcess(strPath, 3, SessionHelper.GetUserId(), 1, 0);
                        return new UifJsonResult(true, companyPrinting.Id);
                    }
                }

            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(ex.Message));
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public FileResult ShowPdfFile(string url)
        {
            var pathToTheFile = url;
            var fileStream = new FileStream(pathToTheFile, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fileStream, "application/pdf");
        }

        public async Task<ActionResult> SendToEmail(string email, string filePath)
        {
            try
            {
                //Primero prueba
                EmailCriteria emailCriteria = new EmailCriteria();
                emailCriteria.Addressed = new List<string>();
                emailCriteria.Files = new List<string>();
                emailCriteria.Addressed.Add(email);
                emailCriteria.Message = "Envio Documento Previsora";
                emailCriteria.Subject = "Previsora";

                if (System.IO.File.Exists(filePath))
                    emailCriteria.Files.Add(filePath);

                var resp = await DelegateService.utilitiesService.SendEmailAsync(emailCriteria);
                string respond = (resp) ? App_GlobalResources.Language.SentEmail : App_GlobalResources.Language.ErrorSentEmail;

                return new UifJsonResult(true, resp);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSentEmail);
            }
        }

        public UifJsonResult UpdateCompanyPrintedDateByTempId(int tempId) {
            try
            {
                DelegateService.quotationService.UpdateCompanyPrintedDateByTempId(tempId);
                return new UifJsonResult(true, "");
            }
            catch (Exception)
            {
                return new UifJsonResult(false, ""/*App_GlobalResources.Language.ErrorUpdatePrintedDate*/);
            }
        }
    }
}