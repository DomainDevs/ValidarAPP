using Excel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Framework.UIF2.Services;
using Sistran.Core.Framework.UIF.Web.Areas.Accounting.Models.PolicyCancellation;
using Sistran.Core.Framework.UIF.Web.Resources;
using Sistran.Core.Framework.Exceptions;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sistran.Company.Application.UniquePersonServices.V1.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Accounting.Controllers.PolicyAutomaticCancellation
{
    #region Class

    /// <summary>
    /// WorkerFactory
    /// Ejecuta proceso de manera asíncrona
    /// </summary>
    public sealed class WorkerFactory
    {
        private static volatile WorkerFactory _instance;
        private static object syncRoot = new Object();

        public static WorkerFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (_instance == null)
                            _instance = new WorkerFactory();
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// CreateWorker
        /// Inicia el proceso asíncrono
        /// </summary>
        /// <param name="cancellationProcess"></param>
        public void CreateWorker(/*CancellationProcess cancellationProcess*/)
        {
            try
            {
                /*
                ICancellationPolicyService cancellationPolicyService = ServiceManager.Instance.GetService<ICancellationPolicyService>();

                Thread thread = new Thread(() => cancellationPolicyService.SaveCancellationProcess(cancellationProcess));
                thread.Start();
                */
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }

    /// <summary>
    /// DeleteFileAttribute
    /// </summary>
    public class DeleteFileAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            filterContext.HttpContext.Response.Flush();

            // Convert the current filter context to file and get the file path
            string filePath = (filterContext.Result as FilePathResult).FileName;

            // Delete the file after download
            File.Delete(filePath);
        }
    }

    #endregion

    public class PolicyCancellationController : Controller
    {
        #region Constants

        public const string PathTemp = "~/Temp";

        #endregion

        #region Instance Variables
        readonly CommonController _commonController = new CommonController();

        #endregion

        #region Public Methods

        /// <summary>
        /// PolicyAutomaticCancellation
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult PolicyAutomaticCancellation()
        {
            try
            {

                //Setear valor por defaul de la sucursal de acuerdo al usuario que se conecta 
                ViewBag.BranchDefault = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 0);
                ViewBag.BranchDisable = _commonController.GetBranchDefaultByUserId(_commonController.GetUserIdByName(User.Identity.Name), 1);

                DateTime issueDate = DelegateService.commonService.GetModuleDateIssue(Convert.ToInt32(ConfigurationManager.AppSettings["ModuleDateUnderwriting"]), DateTime.Now);
                ViewBag.ActiveIssueDate = _commonController.DateFormat(issueDate, 1);
                ViewBag.IssueDate = _commonController.DateFormat(issueDate.Date, 2);

                return View();

            }
            catch (UnhandledException)
            {
                return View("~/Views/Error/NotFound.cshtml");
            }

        }

        /// <summary>
        /// GetBusiness
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetBusiness()
        {
            return new UifSelectResult(_commonController.GetBusinessTypes());
        }

        /// <summary>
        /// GetCancellationTypes
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetCancellationTypes()
        {
            List<object> cancellationTypes = new List<object>();

            cancellationTypes.Add(new { Id = 1, Description = "Cancelación a prorrata" });
            cancellationTypes.Add(new { Id = 2, Description = "Cancelación al 100%" });
            cancellationTypes.Add(new { Id = 3, Description = "Cancelación por saldo de cartera" });

            return new UifSelectResult(cancellationTypes);
        }

        /// <summary>
        /// GetGroupers
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetGroupers()
        {
            List<object> groupers = new List<object>();

            groupers.Add(new { Id = 1, Description = "Grupo AB" });
            groupers.Add(new { Id = 2, Description = "Grupo AC" });

            return new UifSelectResult(groupers);
        }

        /// <summary>
        /// GetInsuredByNumberOrName
        /// Obtiene los asegurados en base al número documento o nombre
        /// </summary>
        /// <param name="numberName"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetInsuredByNumberOrName(string numberName)
        {
            List<object> insurers = new List<object>();

            try
            {
                var persons = DelegateService.tempCommonService.GetInsuredByDocumentNumber(numberName);

                if (persons.Count == 0)
                {
                    persons = DelegateService.tempCommonService.GetInsuredByName(numberName);
                }
                
                foreach (var person in persons)
                {
                    insurers.Add(new
                    {
                        DocumentTypeId = person.DocumentTypeId,
                        DocumentNumber = person.DocumentNumber,
                        Id = person.IndividualId,
                        IndividualId = person.IndividualId,
                        InsuredName = person.Name.Trim(),
                    });
                }

                return new UifTableResult(insurers);
            }
            catch (Exception)
            {
                return new UifTableResult(null);
            }
        }

        /// <summary>
        /// GetIntermediaryByNumberOrName
        /// Obtiene los intermediarios en base al número documento o nombre
        /// </summary>
        /// <param name="numberName"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetIntermediaryByNumberOrName(string numberName)
        {
            List<object> jsonData = new List<object>();

            try
            {
                //TODO esto se debe ajustar ya que trae todos los agentes sin filtrar ademas contiene logica de negocio
                var agentQuerys = DelegateService.uniquePersonServiceV1.GetAgents().Where(ag => ag.EmployeePerson.IdCardNo.Contains(numberName)).ToList();


                if (agentQuerys.Count == 0)
                {
                    agentQuerys = DelegateService.uniquePersonServiceV1.GetAgentByName(numberName);
                }

                foreach (var agent in agentQuerys)
                {
                    var agency = DelegateService.uniquePersonServiceV1.GetCompanyAgencyByInvidualId(agent.IndividualId);
                    jsonData.Add(new
                    {
                        AgentAgencyId = agency[0].Id,
                        AgentType = agent.AgentType.Description,
                        AgentTypeId = agent.AgentType.Id,
                        BranchId = agency[0].Branch.Id,
                        DocumentNumber = agent.EmployeePerson.IdCardNo,
                        DocumentTypeId = 1, 
                        DocumentNumberName = agent.EmployeePerson.IdCardNo + " : " + agent.FullName,
                        Id = agent.IndividualId,
                        IndividualId = agent.IndividualId,
                        IntermediaryName = agent.FullName
                    });
                }

                return new UifTableResult(jsonData);
            }
            catch (Exception)
            {
                return new UifTableResult(null);
            }
        }

        /// <summary>
        /// GetGrouperByNumberOrName
        /// Obtiene los agrupadores en base al código o nombre
        /// </summary>
        /// <param name="numberName"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetGrouperByNumberOrName(string numberName)
        {
            List<object> groupers = new List<object>();

            groupers.Add(new { Id = 1, Description = "Grupo AB" });
            groupers.Add(new { Id = 2, Description = "Grupo AC" });

            return new UifTableResult(groupers);
            /*
            List<object> jsonData = new List<object>();

            try
            {
                var query = _parameterService.GetCancellationPolicyTypes();
                var groupers = query.Where(d => d.Description.Contains(numberName.ToUpper()));
                if (groupers.Count() == 0)
                {
                    groupers = query.Where(d => d.Id == Convert.ToInt32(numberName));
                }

                foreach (CancellationPolicyType grouper in groupers)
                {
                    jsonData.Add(new
                    {
                        Description = grouper.Description,
                        Id = grouper.Id
                    });
                }

                return new UifTableResult(jsonData);
            }
            catch (Exception)
            {
                return new UifTableResult(null);
            }
            */
        }

        /// <summary>
        /// GetPolicyByBranchPrefixPolicyNumber
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="prefixId"></param>
        /// <param name="policyNumber"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetPolicyByBranchPrefixPolicyNumber(int branchId, int prefixId, string policyNumber)
        {
            int policyId = DelegateService.underwritingService.GetCurrentPolicyByPrefixIdBranchIdPolicyNumber(prefixId, branchId, Convert.ToDecimal(policyNumber)).Endorsement.PolicyId;

            return Json(policyId, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// GetCancellationHeaderByProcessNumber
        /// Obtiene la cabecera del proceso cancelación automática de pólizas
        /// </summary>
        /// <param name="processNumber"></param>
        /// <param name="tab"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetCancellationHeaderByProcessNumber(int processNumber, string tab)
        {
            List<object> jsonData = new List<object>();

            jsonData.Add(new
            {
                ProcessDate = "16/07/2018 15:27:37",
                UserName = "ADMIN",
                TotalRecords = 5,
                TotalPremium = 1000
            });


            /*
            CancellationProcess cancellationProcess = new CancellationProcess();

            cancellationProcess.Id = processNumber;
            cancellationProcess.User = new individuals.User()
            {
                Id = SessionHelper.GetUserId(),
                Nick = SessionHelper.GetUserName(),
                Name = tab
            };
            cancellationProcess.NumberPolicies = -1;

            var newCancellationProcess = cancellationPolicyService.GetCancellationProcess(cancellationProcess);

            if (newCancellationProcess.Id > 0)
            {
                jsonData.Add(new
                {
                    ProcessDate = newCancellationProcess.Date.ToString("dd/MM/yyyy H:mm:ss"),
                    UserName = newCancellationProcess.User.Nick,
                    TotalRecords = newCancellationProcess.NumberPolicies,
                    TotalPremium = 0
                });
            }
            else
            {
                jsonData.Add(new
                {
                    UserName = "-1",
                });
            }
            */

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetCancellationDetailByProcessNumber
        /// Obtiene el detalle del proceso cancelación automática de pólizas
        /// </summary>
        /// <param name="processNumber"></param>
        /// <param name="tab"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GetCancellationDetailByProcessNumber(int processNumber, string tab)
        {
            List<object> jsonData = new List<object>();

            if (tab.Equals("G"))
            {
                jsonData.Add(new
                {
                    Id = 4318,
                    ProcessNumber = 1,
                    Annul = "",
                    CancellationMark = 0,
                    ExclusionReason = "",
                    ExclusionReasonCode = 0,
                    QuoteDueDate = "30/06/2018",
                    BranchId = 1,
                    Branch = "ARMENIA",
                    SalePointId = 1,
                    SalePoint = "PUNTO DE VENTA",
                    PrefixId = 7,
                    Prefix = "AUTOMOVILES",
                    PolicyId = 70116387,
                    Policy = "3000989",
                    InsuredId = 860006820,
                    Insured = "ASEGURADORA GRANCOLOMBIANA S.A.",
                    IntermediaryTypeId = 1,
                    IntermediaryId = 165,
                    Intermediary = "ASEGURADORA GRANCOLOMBIANA S.A.",
                    IssueDate = "31/05/2018",
                    IssueDateFrom = "27/06/2018",
                    IssueDateTo = "27/06/2019",
                    CurrencyId = 0,
                    Currency = "PESOS",
                    Amount = 500,
                    ExchangeRate = 1,
                    LocalAmount = 500,
                    ProcessingMark = 0,
                    Processed = "No",
                    Observations = "",
                    EntryNumber = "",
                    ExclusionAllowsToCancel = 1,
                    MotherPolicyId = 70116387
                });

                jsonData.Add(new
                {
                    Id = 4319,
                    ProcessNumber = 1,
                    Annul = "",
                    CancellationMark = 0,
                    ExclusionReason = "",
                    ExclusionReasonCode = 0,
                    QuoteDueDate = "29/06/2018",
                    BranchId = 1,
                    Branch = "ARMENIA",
                    SalePointId = 2,
                    SalePoint = "PUNTO DE VENTA",
                    PrefixId = 7,
                    Prefix = "AUTOMOVILES",
                    PolicyId = 58711,
                    Policy = "3010219",
                    InsuredId = 12333,
                    Insured = "CHEVYPLAN S.A.",
                    IntermediaryTypeId = 3,
                    IntermediaryId = 3790393,
                    Intermediary = "ASG SANAS AGENCIA DE SEGUROS LTDA",
                    IssueDate = "18/11/2017",
                    IssueDateFrom = "20/11/2017",
                    IssueDateTo = "20/11/2018",
                    CurrencyId = 1,
                    Currency = "PESOS",
                    Amount = 400,
                    ExchangeRate = 1,
                    LocalAmount = 400,
                    ProcessingMark = 0,
                    Processed = "No",
                    Observations = "",
                    EntryNumber = "",
                    ExclusionAllowsToCancel = 1,
                    MotherPolicyId = 4319
                });

                jsonData.Add(new
                {
                    Id = 4320,
                    ProcessNumber = 1,
                    Annul = "",
                    CancellationMark = 0,
                    ExclusionReason = "",
                    ExclusionReasonCode = 0,
                    QuoteDueDate = "dd/MM/yyyy",
                    BranchId = 1,
                    Branch = "ARMENIA",
                    SalePointId = 3,
                    SalePoint = "PUNTO DE VENTA",
                    PrefixId = 7,
                    Prefix = "AUTOMOVILES",
                    PolicyId = 61971,
                    Policy = "3006647",
                    InsuredId = 12333,
                    Insured = "CHEVYPLAN S.A.",
                    IntermediaryTypeId = 1,
                    IntermediaryId = 7270,
                    Intermediary = "PUENTES VILLAMIL JORGE HUMBERTO",
                    IssueDate = "29/10/2017",
                    IssueDateFrom = "03/11/2017",
                    IssueDateTo = "03/11/2018",
                    CurrencyId = 1,
                    Currency = "PESOS",
                    Amount = 100,
                    ExchangeRate = 1,
                    LocalAmount = 100,
                    ProcessingMark = 0,
                    Processed = "No",
                    Observations = "",
                    EntryNumber = "",
                    ExclusionAllowsToCancel = 1,
                    MotherPolicyId = 4320
                });
            }
            else if (tab.Equals("P"))
            {
                jsonData.Add(new
                {
                    Id = 4318,
                    ProcessNumber = 1,
                    AppliesCollection = "Si",
                    AppliesCollectionMark = 1,
                    QuoteDueDate = "30/06/2018",
                    BranchId = 1,
                    Branch = "ARMENIA",
                    SalePointId = 1,
                    SalePoint = "PUNTO DE VENTA",
                    PrefixId = 1,
                    Prefix = "AUTOMOVILES",
                    PolicyId = 70116387,
                    Policy = "3000989",
                    InsuredId = 860006820,
                    Insured = "ASEGURADORA GRANCOLOMBIANA S.A.",
                    IntermediaryTypeId = 1,
                    IntermediaryId = 165,
                    Intermediary = "ASEGURADORA GRANCOLOMBIANA S.A.",
                    IssueDate = "31/05/2018",
                    IssueDateFrom = "27/06/2018",
                    IssueDateTo = "27/06/2019",
                    CurrencyId = 0,
                    Currency = "PESOS",
                    Amount = 500,
                    ExchangeRate = 1,
                    LocalAmount = 500,
                    ApplicationTemporaryId = "81011",
                    MotherPolicyId = 70116387
                });
            }
            else if (tab.Equals("R"))
            {
                jsonData.Add(new
                {
                    Id = 4319,
                    ProcessNumber = 1,
                    Reprocess = "Si",
                    ReprocessMark = 1,
                    QuoteDueDate = "29/06/2018",
                    BranchId = 1,
                    Branch = "ARMENIA",
                    SalePointId = 2,
                    SalePoint = "PUNTO DE VENTA",
                    PrefixId = 7,
                    Prefix = "AUTOMOVILES",
                    PolicyId = 58711,
                    Policy = "3010219",
                    InsuredId = 12333,
                    Insured = "CHEVYPLAN S.A.",
                    IntermediaryTypeId = 3,
                    IntermediaryId = 3790393,
                    Intermediary = "ASG SANAS AGENCIA DE SEGUROS LTDA",
                    IssueDate = "18/11/2017",
                    IssueDateFrom = "20/11/2017",
                    IssueDateTo = "20/11/2018",
                    CurrencyId = 1,
                    Currency = "PESOS",
                    Amount = 400,
                    ExchangeRate = 1,
                    LocalAmount = 400,
                    Description = "ERROR ERROR",
                    MotherPolicyId = 58711
                });
            }
            else if (tab.Equals("A"))
            {
                jsonData.Add(new
                {
                    Id = 4320,
                    ProcessNumber = 1,
                    BranchId = 1,
                    Branch = "ARMENIA",
                    SalePointId = 3,
                    SalePoint = "PUNTO DE VENTA",
                    PrefixId = 7,
                    Prefix = "AUTOMOVILES",
                    PolicyId = 61971,
                    Policy = "3006647",
                    InsuredId = 12333,
                    Insured = "CHEVYPLAN S.A.",
                    IntermediaryTypeId = 1,
                    IntermediaryId = 7270,
                    Intermediary = "PUENTES VILLAMIL JORGE HUMBERTO",
                    CollectionDate = "16/07/2018",
                    CurrencyId = 1,
                    Currency = "PESOS",
                    Amount = 100,
                    ExchangeRate = 1,
                    LocalAmount = 100,
                    ApplicationReceiptNumber = 26171,
                    EntryNumber = 234,
                    MotherPolicyId = 4320
                });
            }


            /*
            CancellationProcess cancellationProcess = new CancellationProcess();

            cancellationProcess.Id = processNumber;
            cancellationProcess.User = new individuals.User()
            {
                Id = SessionHelper.GetUserId(),
                Nick = SessionHelper.GetUserName(),
                Name = tab
            };
            cancellationProcess.NumberPolicies = 0;

            var newCancellationProcess = cancellationPolicyService.GetCancellationProcess(cancellationProcess);

            if (tab.Equals("G"))
            {
                if (newCancellationProcess.CancellationPolicies != null)
                {
                    foreach (CancellationPolicy policyItem in newCancellationProcess.CancellationPolicies)
                    {
                        jsonData.Add(new
                        {
                            Id = policyItem.Id,
                            ProcessNumber = newCancellationProcess.Id,
                            Annul = policyItem.Policy.Product.Description,
                            CancellationMark = policyItem.Policy.Product.Id,
                            ExclusionReason = policyItem.Policy.BusinessType.Description,
                            ExclusionReasonCode = policyItem.Policy.BusinessType.Id,
                            QuoteDueDate = policyItem.Policy.DateAndTimeFrom == Convert.ToDateTime("01/01/0001 0:00:00") ? "" : policyItem.Policy.DateAndTimeFrom.ToString("dd/MM/yyyy"),
                            BranchId = policyItem.Policy.Branch.Id,
                            Branch = policyItem.Policy.Branch.Description,
                            SalePointId = policyItem.Policy.SalePoint.Id,
                            SalePoint = policyItem.Policy.SalePoint.Description,
                            PrefixId = policyItem.Policy.Prefix.Id,
                            Prefix = policyItem.Policy.Prefix.Description,
                            PolicyId = policyItem.Policy.Id,
                            Policy = policyItem.Policy.DocumentNumber,
                            InsuredId = policyItem.Policy.PolicyHolderPerson.Id,
                            Insured = policyItem.Policy.PolicyHolderPerson.Name,
                            IntermediaryTypeId = policyItem.Policy.Agent.AgentTypeCode,
                            IntermediaryId = policyItem.Policy.Agent.Id,
                            Intermediary = policyItem.Policy.Agent.Name,
                            IssueDate = policyItem.Policy.IssuanceDate.ToString("dd/MM/yyyy"),
                            IssueDateFrom = policyItem.Policy.CurrentFrom.ToString("dd/MM/yyyy"),
                            IssueDateTo = policyItem.Policy.CurrentTo.ToString("dd/MM/yyyy"),
                            CurrencyId = policyItem.Policy.Currency.Id,
                            Currency = policyItem.Policy.Currency.Description,
                            Amount = policyItem.Policy.Discount,
                            ExchangeRate = policyItem.Policy.ExchangeRate.BuyAmount,
                            LocalAmount = policyItem.Policy.Discount,
                            ProcessingMark = policyItem.Policy.EndorsementGroup.Id,
                            Processed = policyItem.Policy.EndorsementGroup.Description,
                            Observations = policyItem.Policy.PolicyHolderCompany.BusinessName,
                            EntryNumber = "",
                            ExclusionAllowsToCancel = policyItem.Policy.Risks[0].Id,
                            MotherPolicyId = policyItem.Policy.InitialPolicyId
                        });
                    }
                }
            }
            else if (tab.Equals("P"))
            {
                if (newCancellationProcess.CancellationPolicies != null)
                {
                    foreach (CancellationPolicy policyItem in newCancellationProcess.CancellationPolicies)
                    {
                        jsonData.Add(new
                        {
                            Id = policyItem.Id,
                            ProcessNumber = newCancellationProcess.Id,
                            AppliesCollection = policyItem.Policy.EndorsementType.Description,
                            AppliesCollectionMark = policyItem.Policy.EndorsementType.Id,
                            QuoteDueDate = policyItem.Policy.DateAndTimeFrom == Convert.ToDateTime("01/01/0001 0:00:00") ? "" : policyItem.Policy.DateAndTimeFrom.ToString("dd/MM/yyyy"),
                            BranchId = policyItem.Policy.Branch.Id,
                            Branch = policyItem.Policy.Branch.Description,
                            SalePointId = policyItem.Policy.SalePoint.Id,
                            SalePoint = policyItem.Policy.SalePoint.Description,
                            PrefixId = policyItem.Policy.Prefix.Id,
                            Prefix = policyItem.Policy.Prefix.Description,
                            PolicyId = policyItem.Policy.Id,
                            Policy = policyItem.Policy.DocumentNumber,
                            InsuredId = policyItem.Policy.PolicyHolderPerson.Id,
                            Insured = policyItem.Policy.PolicyHolderPerson.Name,
                            IntermediaryTypeId = policyItem.Policy.Agent.AgentTypeCode,
                            IntermediaryId = policyItem.Policy.Agent.Id,
                            Intermediary = policyItem.Policy.Agent.Name,
                            IssueDate = policyItem.Policy.IssuanceDate.ToString("dd/MM/yyyy"),
                            IssueDateFrom = policyItem.Policy.CurrentFrom.ToString("dd/MM/yyyy"),
                            IssueDateTo = policyItem.Policy.CurrentTo.ToString("dd/MM/yyyy"),
                            CurrencyId = policyItem.Policy.Currency.Id,
                            Currency = policyItem.Policy.Currency.Description,
                            Amount = policyItem.Policy.Discount,
                            ExchangeRate = policyItem.Policy.ExchangeRate.BuyAmount,
                            LocalAmount = policyItem.Policy.Discount,
                            ApplicationTemporaryId = policyItem.Policy.Risks[0].Description == "-1" ? "" : policyItem.Policy.Risks[0].Description,
                            MotherPolicyId = policyItem.Policy.InitialPolicyId
                        });
                    }
                }
            }
            else if (tab.Equals("R"))
            {
                if (newCancellationProcess.CancellationPolicies != null)
                {
                    foreach (CancellationPolicy policyItem in newCancellationProcess.CancellationPolicies)
                    {
                        jsonData.Add(new
                        {
                            Id = policyItem.Id,
                            ProcessNumber = newCancellationProcess.Id,
                            Reprocess = policyItem.Policy.PolicyHolderCompany.Email,
                            ReprocessMark = policyItem.Policy.PolicyHolderCompany.ProcessId,
                            QuoteDueDate = policyItem.Policy.DateAndTimeFrom == Convert.ToDateTime("01/01/0001 0:00:00") ? "" : policyItem.Policy.DateAndTimeFrom.ToString("dd/MM/yyyy"),
                            BranchId = policyItem.Policy.Branch.Id,
                            Branch = policyItem.Policy.Branch.Description,
                            SalePointId = policyItem.Policy.SalePoint.Id,
                            SalePoint = policyItem.Policy.SalePoint.Description,
                            PrefixId = policyItem.Policy.Prefix.Id,
                            Prefix = policyItem.Policy.Prefix.Description,
                            PolicyId = policyItem.Policy.Id,
                            Policy = policyItem.Policy.DocumentNumber,
                            InsuredId = policyItem.Policy.PolicyHolderPerson.Id,
                            Insured = policyItem.Policy.PolicyHolderPerson.Name,
                            IntermediaryTypeId = policyItem.Policy.Agent.AgentTypeCode,
                            IntermediaryId = policyItem.Policy.Agent.Id,
                            Intermediary = policyItem.Policy.Agent.Name,
                            IssueDate = policyItem.Policy.IssuanceDate.ToString("dd/MM/yyyy"),
                            IssueDateFrom = policyItem.Policy.CurrentFrom.ToString("dd/MM/yyyy"),
                            IssueDateTo = policyItem.Policy.CurrentTo.ToString("dd/MM/yyyy"),
                            CurrencyId = policyItem.Policy.Currency.Id,
                            Currency = policyItem.Policy.Currency.Description,
                            Amount = policyItem.Policy.Discount,
                            ExchangeRate = policyItem.Policy.ExchangeRate.BuyAmount,
                            LocalAmount = policyItem.Policy.Discount,
                            Description = policyItem.Policy.PolicyHolderCompany.BusinessName,
                            MotherPolicyId = policyItem.Policy.InitialPolicyId
                        });
                    }
                }
            }
            else if (tab.Equals("A"))
            {
                if (newCancellationProcess.CancellationPolicies != null)
                {
                    foreach (CancellationPolicy policyItem in newCancellationProcess.CancellationPolicies)
                    {
                        jsonData.Add(new
                        {
                            Id = policyItem.Id,
                            ProcessNumber = newCancellationProcess.Id,
                            BranchId = policyItem.Policy.Branch.Id,
                            Branch = policyItem.Policy.Branch.Description,
                            SalePointId = policyItem.Policy.SalePoint.Id,
                            SalePoint = policyItem.Policy.SalePoint.Description,
                            PrefixId = policyItem.Policy.Prefix.Id,
                            Prefix = policyItem.Policy.Prefix.Description,
                            PolicyId = policyItem.Policy.Id,
                            Policy = policyItem.Policy.DocumentNumber,
                            InsuredId = policyItem.Policy.PolicyHolderPerson.Id,
                            Insured = policyItem.Policy.PolicyHolderPerson.Name,
                            IntermediaryTypeId = policyItem.Policy.Agent.AgentTypeCode,
                            IntermediaryId = policyItem.Policy.Agent.Id,
                            Intermediary = policyItem.Policy.Agent.Name,
                            CollectionDate = policyItem.Policy.CurrentTo.ToString("dd/MM/yyyy"),
                            CurrencyId = policyItem.Policy.Currency.Id,
                            Currency = policyItem.Policy.Currency.Description,
                            Amount = policyItem.Policy.Discount,
                            ExchangeRate = policyItem.Policy.ExchangeRate.BuyAmount,
                            LocalAmount = policyItem.Policy.Discount,
                            ApplicationReceiptNumber = policyItem.Policy.Risks[0].Number,
                            EntryNumber = policyItem.Policy.Risks[0].BuildYear,
                            MotherPolicyId = policyItem.Policy.InitialPolicyId
                        });
                    }
                }
            }
            */
            return new UifTableResult(jsonData);
        }

        /// <summary>
        /// GetTotalRecords
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="salePointId"></param>
        /// <param name="prefixId"></param>
        /// <param name="policyNumber"></param>
        /// <param name="insuredId"></param>
        /// <param name="intermediaryId"></param>
        /// <param name="grouperId"></param>
        /// <param name="businessId"></param>
        /// <param name="cutDate"></param>
        /// <param name="issueDateFrom"></param>
        /// <param name="issueDateTo"></param>
        /// <param name="cancellationTypeId"></param>
        /// <returns>JsonResult</returns>
        public JsonResult GetTotalRecords(int branchId, int salePointId, int prefixId,
                                          string policyNumber, int insuredId, int intermediaryId,
                                          int grouperId, int businessId, string cutDate,
                                          string issueDateFrom, string issueDateTo, int cancellationTypeId)
        {
            try
            {
                List<object> jsonData = new List<object>();
                /*
                CancellationProcess cancellationProcess = new CancellationProcess();
                List<CancellationPolicy> policies = new List<CancellationPolicy>();
                CancellationPolicy cancellationPolicy = new CancellationPolicy();

                List<SalePoint> salePoints = new List<SalePoint>();
                salePoints.Add(new SalePoint() { Id = salePointId });
                Currency currency = new Currency() { Id = 0 };
                Branch branch = new Branch() { Id = branchId, SalePoints = salePoints };
                Prefix prefix = new Prefix() { Id = prefixId };
                individuals.Agent agent = new individuals.Agent() { Id = intermediaryId };
                PolicyBusinessType businessType = new PolicyBusinessType() { Id = businessId };
                EndorsementGroup group = new EndorsementGroup() { Id = grouperId };

                Policy policy = new Policy()
                {
                    Id = 0,
                    DocumentNumber = Convert.ToInt32(policyNumber),
                    Currency = currency,
                    Agent = agent,
                    Branch = branch,
                    BusinessType = businessType,
                    EffectiveEndDate = issueDateTo == "" ? Convert.ToDateTime("01/01/0001 0:00:00") : Convert.ToDateTime(issueDateTo),
                    EffectiveStartDate = issueDateFrom == "" ? Convert.ToDateTime("01/01/0001 0:00:00") : Convert.ToDateTime(issueDateFrom),
                    EndorsementGroup = group,
                    InitialPolicyId = cancellationTypeId,
                    InsuredId = insuredId,
                    Prefix = prefix
                };
                cancellationPolicy.Policy = policy;
                policies.Add(cancellationPolicy);

                individuals.User user = new individuals.User() { Id = SessionHelper.GetUserId(), Nick = SessionHelper.GetUserName() };

                cancellationProcess.CancellationPolicies = policies;
                cancellationProcess.Date = cutDate == "" ? Convert.ToDateTime("01/01/0001 0:00:00") : Convert.ToDateTime(cutDate);
                cancellationProcess.Id = -1;
                cancellationProcess.NumberPolicies = 0;
                cancellationProcess.User = user;

                var cancellation = cancellationPolicyService.SaveCancellationProcess(cancellationProcess);

                jsonData.Add(new
                {
                    TotalRecords = cancellation.NumberPolicies
                });
                */
                jsonData.Add(new
                {
                    TotalRecords = 10
                });
                return Json(jsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// GeneratePolicyAutomaticCancellation
        /// </summary>
        /// <param name="branchId"></param>
        /// <param name="salePointId"></param>
        /// <param name="prefixId"></param>
        /// <param name="policyNumber"></param>
        /// <param name="insuredId"></param>
        /// <param name="intermediaryId"></param>
        /// <param name="grouperId"></param>
        /// <param name="businessId"></param>
        /// <param name="cutDate"></param>
        /// <param name="issueDateFrom"></param>
        /// <param name="issueDateTo"></param>
        /// <param name="cancellationTypeId"></param>
        /// <returns>ActionResult</returns>
        public ActionResult GeneratePolicyAutomaticCancellation(int branchId, int salePointId, int prefixId,
                                                                string policyNumber, int insuredId, int intermediaryId,
                                                                int grouperId, int businessId, string cutDate,
                                                                string issueDateFrom, string issueDateTo, int cancellationTypeId)
        {
            try
            {
                List<object> data = new List<object>();
                /*
                CancellationProcess cancellationProcess = new CancellationProcess();
                List<CancellationPolicy> policies = new List<CancellationPolicy>();
                CancellationPolicy cancellationPolicy = new CancellationPolicy();

                List<SalePoint> salePoints = new List<SalePoint>();
                salePoints.Add(new SalePoint() { Id = salePointId });
                Currency currency = new Currency() { Id = 0 };
                Branch branch = new Branch() { Id = branchId, SalePoints = salePoints };
                Prefix prefix = new Prefix() { Id = prefixId };
                individuals.Agent agent = new individuals.Agent() { Id = intermediaryId };
                PolicyBusinessType businessType = new PolicyBusinessType() { Id = businessId };
                EndorsementGroup group = new EndorsementGroup() { Id = grouperId };

                Policy policy = new Policy()
                {
                    Id = 1,
                    DocumentNumber = Convert.ToInt32(policyNumber),
                    Currency = currency,
                    Agent = agent,
                    Branch = branch,
                    BusinessType = businessType,
                    EffectiveEndDate = issueDateTo == "" ? Convert.ToDateTime("01/01/0001 0:00:00") : Convert.ToDateTime(issueDateTo),
                    EffectiveStartDate = issueDateFrom == "" ? Convert.ToDateTime("01/01/0001 0:00:00") : Convert.ToDateTime(issueDateFrom),
                    EndorsementGroup = group,
                    InitialPolicyId = cancellationTypeId,
                    InsuredId = insuredId,
                    Prefix = prefix
                };

                cancellationPolicy.Policy = policy;
                policies.Add(cancellationPolicy);

                individuals.User user = new individuals.User() { Id = SessionHelper.GetUserId(), Nick = SessionHelper.GetUserName() };

                cancellationProcess.CancellationPolicies = policies;
                cancellationProcess.Date = cutDate == "" ? Convert.ToDateTime("01/01/0001 0:00:00") : Convert.ToDateTime(cutDate);
                cancellationProcess.Id = 0;
                cancellationProcess.NumberPolicies = 0;
                cancellationProcess.User = user;

                WorkerFactory.Instance.CreateWorker(cancellationProcess);
                */
                data.Add(new
                {
                    ProcessNumber = 1,
                    MessageError = "OK"
                });

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Permite descargar el archivo
        /// </summary>
        /// <param name="file">Nombre del Archivo</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [DeleteFileAttribute] //Action Filter, it will auto delete the file after download, 
        public ActionResult Download(string file)
        {
            // Get the temp folder and file path in server
            string fullPath = Path.Combine(Server.MapPath(PathTemp), file);

            // Return the file for download, this is an Excel 
            // so I set the file content type to "application/vnd.ms-excel"
            return File(fullPath, "application/vnd.ms-excel", file);
        }

        /// <summary>
        /// GenerateCancellationsToExcel
        /// </summary>
        /// <param name="processNumber">Número de proceso</param>
        /// <param name="processDate">Fecha de proceso</param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult GenerateCancellationsToExcel(int processNumber, string processDate)
        {
            List<PolicyCancellationModel> policies = new List<PolicyCancellationModel>();

            try
            {
                /*
                CancellationProcess cancellationProcess = new CancellationProcess();
                User user = new User()
                {
                    Id = SessionHelper.GetUserId(),
                    Nick = SessionHelper.GetUserName(),
                    Name = "P"
                };
                cancellationProcess.CancellationPolicies = null;
                cancellationProcess.Id = processNumber;
                cancellationProcess.NumberPolicies = -1;
                cancellationProcess.User = user;

                // Se obtiene la cabecera
                var header = cancellationPolicyService.GetCancellationProcess(cancellationProcess);

                // Se obtiene el detalle 
                cancellationProcess.NumberPolicies = 0;
                var newCancellationProcess = cancellationPolicyService.GetCancellationProcess(cancellationProcess);

                if (newCancellationProcess.CancellationPolicies != null)
                {
                    foreach (CancellationPolicy policyItem in newCancellationProcess.CancellationPolicies)
                    {
                        if (Convert.ToInt32(policyItem.Policy.Risks[0].Description) == -1)
                        {
                            policies.Add(new PolicyCancellationModel()
                            {
                                Amount = policyItem.Policy.Discount,
                                ApplicationTemporaryId = Convert.ToInt32(policyItem.Policy.Risks[0].Description),
                                AppliesCollection = policyItem.Policy.EndorsementType.Description,
                                BranchDescription = policyItem.Policy.Branch.Description,
                                BranchId = policyItem.Policy.Branch.Id,
                                CurrencyDescription = policyItem.Policy.Currency.Description,
                                CurrencyId = policyItem.Policy.Currency.Id,
                                ExchangeRate = policyItem.Policy.ExchangeRate.BuyAmount,
                                Id = policyItem.Id,
                                Insured = policyItem.Policy.PolicyHolderPerson.Name,
                                InsuredId = policyItem.Policy.PolicyHolderPerson.Id,
                                Intermediary = policyItem.Policy.Agent.Name,
                                IntermediaryId = policyItem.Policy.Agent.Id,
                                IntermediaryTypeId = policyItem.Policy.Agent.AgentTypeCode,
                                IssueDate = policyItem.Policy.IssuanceDate,
                                IssueDateFrom = policyItem.Policy.CurrentFrom,
                                IssueDateTo = policyItem.Policy.CurrentTo,
                                LocalAmount = policyItem.Policy.Discount,
                                MotherPolicyId = policyItem.Policy.InitialPolicyId,
                                Observations = policyItem.Policy.PolicyHolderCompany.BusinessName,
                                Policy = policyItem.Policy.DocumentNumber.ToString(),
                                PolicyId = policyItem.Policy.Id,
                                PrefixDescription = policyItem.Policy.Prefix.Description,
                                PrefixId = policyItem.Policy.Prefix.Id,
                                ProcessNumber = newCancellationProcess.Id,
                                QuoteDueDate = policyItem.Policy.DateAndTimeFrom,
                                SalePointDescription = policyItem.Policy.SalePoint.Description,
                                SalePointId = policyItem.Policy.SalePoint.Id,
                            });
                        }
                    }
                }
                */
                var fileName = "PólizasCanceladasProceso_" + processNumber.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";
                string fullPath = Path.Combine(Server.MapPath(PathTemp), fileName);
                MemoryStream dataStream = ExportCancellationsToExcel(/*header,*/ policies);

                using (var exportData = dataStream)
                {
                    FileStream file = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                    exportData.WriteTo(file);
                    file.Close();
                }

                // Return the Excel file name
                return Json(new { fileName = fileName, errorMessage = "" });
            }
            catch (Exception ex)
            {
                return Json(new { fileName = "", errorMessage = ex.Message });
            }
        }

        /// <summary>
        /// GenerateMistakesToExcel
        /// </summary>
        /// <param name="processNumber">Número de proceso</param>
        /// <param name="processDate">Fecha de proceso</param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult GenerateMistakesToExcel(int processNumber, string processDate)
        {
            List<PolicyCancellationModel> policies = new List<PolicyCancellationModel>();

            try
            {
                /*
                CancellationProcess cancellationProcess = new CancellationProcess();
                individuals.User user = new individuals.User()
                {
                    Id = SessionHelper.GetUserId(),
                    Nick = SessionHelper.GetUserName(),
                    Name = "R"
                };
                cancellationProcess.CancellationPolicies = null;
                cancellationProcess.Id = processNumber;
                cancellationProcess.NumberPolicies = -1;
                cancellationProcess.User = user;

                // Se obtiene la cabecera
                var header = cancellationPolicyService.GetCancellationProcess(cancellationProcess);

                // Se obtiene el detalle
                cancellationProcess.NumberPolicies = 0;
                var newCancellationProcess = cancellationPolicyService.GetCancellationProcess(cancellationProcess);

                if (newCancellationProcess.CancellationPolicies != null)
                {
                    foreach (CancellationPolicy policyItem in newCancellationProcess.CancellationPolicies)
                    {
                        policies.Add(new PolicyCancellationModel()
                        {
                            Amount = policyItem.Policy.Discount,
                            BranchDescription = policyItem.Policy.Branch.Description,
                            BranchId = policyItem.Policy.Branch.Id,
                            CancellationMark = policyItem.Policy.Product.Id,
                            CurrencyDescription = policyItem.Policy.Currency.Description,
                            CurrencyId = policyItem.Policy.Currency.Id,
                            EntryNumber = 0,
                            ExchangeRate = policyItem.Policy.ExchangeRate.BuyAmount,
                            ExclusionAllowsToCancel = policyItem.Policy.Risks[0].Id,
                            ExclusionReason = policyItem.Policy.BusinessType.Description,
                            ExclusionReasonCode = policyItem.Policy.BusinessType.Id,
                            Id = policyItem.Id,
                            Insured = policyItem.Policy.PolicyHolderPerson.Name,
                            InsuredId = policyItem.Policy.PolicyHolderPerson.Id,
                            Intermediary = policyItem.Policy.Agent.Name,
                            IntermediaryId = policyItem.Policy.Agent.Id,
                            IntermediaryTypeId = policyItem.Policy.Agent.AgentTypeCode,
                            IssueDate = policyItem.Policy.IssuanceDate,
                            IssueDateFrom = policyItem.Policy.CurrentFrom,
                            IssueDateTo = policyItem.Policy.CurrentTo,
                            LocalAmount = policyItem.Policy.Discount,
                            MotherPolicyId = policyItem.Policy.InitialPolicyId,
                            Observations = policyItem.Policy.PolicyHolderCompany.BusinessName,
                            Policy = policyItem.Policy.DocumentNumber.ToString(),
                            PolicyId = policyItem.Policy.Id,
                            PrefixDescription = policyItem.Policy.Prefix.Description,
                            PrefixId = policyItem.Policy.Prefix.Id,
                            Processed = policyItem.Policy.EndorsementGroup.Description,
                            ProcessingMark = policyItem.Policy.EndorsementGroup.Id,
                            ProcessNumber = newCancellationProcess.Id,
                            QuoteDueDate = policyItem.Policy.DateAndTimeFrom,
                            Reprocess = policyItem.Policy.Product.Description,
                            SalePointDescription = policyItem.Policy.SalePoint.Description,
                            SalePointId = policyItem.Policy.SalePoint.Id,
                        });
                    }
                }
                */
                var fileName = "ErrorAcancelarProceso_" + processNumber.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";
                string fullPath = Path.Combine(Server.MapPath(PathTemp), fileName);
                MemoryStream dataStream = ExportMistakesToExcel(/*header,*/ policies);

                using (var exportData = dataStream)
                {
                    FileStream file = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                    exportData.WriteTo(file);
                    file.Close();
                }

                // Return the Excel file name
                return Json(new { fileName = fileName, errorMessage = "" });
            }
            catch (Exception ex)
            {
                return Json(new { fileName = "", errorMessage = ex.Message });
            }
        }

        /// <summary>
        /// GenerateAppliedCancellationsToExcel
        /// </summary>
        /// <param name="processNumber">Número de proceso</param>
        /// <param name="processDate">Fecha de proceso</param>
        /// <returns>JsonResult</returns>
        [HttpPost]
        public JsonResult GenerateAppliedCancellationsToExcel(int processNumber, string processDate)
        {
            List<PolicyCancellationModel> policies = new List<PolicyCancellationModel>();

            try
            {
                /*
                CancellationProcess cancellationProcess = new CancellationProcess();
                individuals.User user = new individuals.User()
                {
                    Id = SessionHelper.GetUserId(),
                    Nick = SessionHelper.GetUserName(),
                    Name = "A"
                };
                cancellationProcess.CancellationPolicies = null;
                cancellationProcess.Id = processNumber;
                cancellationProcess.NumberPolicies = -1;
                cancellationProcess.User = user;

                // Se obtiene la cabecera
                var header = cancellationPolicyService.GetCancellationProcess(cancellationProcess);

                // Se obtiene el detalle 
                cancellationProcess.NumberPolicies = 0;
                var newCancellationProcess = cancellationPolicyService.GetCancellationProcess(cancellationProcess);

                if (newCancellationProcess.CancellationPolicies != null)
                {
                    foreach (CancellationPolicy policyItem in newCancellationProcess.CancellationPolicies)
                    {
                        policies.Add(new PolicyCancellationModel()
                        {
                            Amount = policyItem.Policy.Discount,
                            ApplicationReceiptNumber = policyItem.Policy.Risks[0].Number,
                            ApplicationTemporaryId = Convert.ToInt32(policyItem.Policy.Risks[0].Description),
                            AppliesCollection = policyItem.Policy.EndorsementType.Description,
                            BranchDescription = policyItem.Policy.Branch.Description,
                            BranchId = policyItem.Policy.Branch.Id,
                            CollectionDate = policyItem.Policy.EffectiveEndDate,
                            CurrencyDescription = policyItem.Policy.Currency.Description,
                            CurrencyId = policyItem.Policy.Currency.Id,
                            EntryNumber = policyItem.Policy.Risks[0].BuildYear,
                            ExchangeRate = policyItem.Policy.ExchangeRate.BuyAmount,
                            Id = policyItem.Id,
                            Insured = policyItem.Policy.PolicyHolderPerson.Name,
                            InsuredId = policyItem.Policy.PolicyHolderPerson.Id,
                            Intermediary = policyItem.Policy.Agent.Name,
                            IntermediaryId = policyItem.Policy.Agent.Id,
                            IntermediaryTypeId = policyItem.Policy.Agent.AgentTypeCode,
                            IssueDate = policyItem.Policy.IssuanceDate,
                            IssueDateFrom = policyItem.Policy.CurrentFrom,
                            IssueDateTo = policyItem.Policy.CurrentTo,
                            LocalAmount = policyItem.Policy.Discount,
                            MotherPolicyId = policyItem.Policy.InitialPolicyId,
                            Observations = policyItem.Policy.PolicyHolderCompany.BusinessName,
                            Policy = policyItem.Policy.DocumentNumber.ToString(),
                            PolicyId = policyItem.Policy.Id,
                            PrefixDescription = policyItem.Policy.Prefix.Description,
                            PrefixId = policyItem.Policy.Prefix.Id,
                            ProcessNumber = newCancellationProcess.Id,
                            QuoteDueDate = policyItem.Policy.DateAndTimeFrom,
                            SalePointDescription = policyItem.Policy.SalePoint.Description,
                            SalePointId = policyItem.Policy.SalePoint.Id,
                        });
                    }
                }
                */
                var fileName = "CancelacionesAplicadasProceso_" + processNumber.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";
                string fullPath = Path.Combine(Server.MapPath(PathTemp), fileName);
                MemoryStream dataStream = ExportAppliedCancellationsToExcel(/*header,*/ policies);

                using (var exportData = dataStream)
                {
                    FileStream file = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
                    exportData.WriteTo(file);
                    file.Close();
                }

                // Return the Excel file name
                return Json(new { fileName = fileName, errorMessage = "" });
            }
            catch (Exception ex)
            {
                return Json(new { fileName = "", errorMessage = ex.Message });
            }
        }

        /// <summary>
        /// GetPendingProcesses
        /// Obtiene los procesos pendientes de cancelación automática de pólizas
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult GetPendingProcesses()
        {
            List<object> jsonData = new List<object>();
            /*
            List<Policy> policies = new List<Policy>();
            Branch branch = new Branch() { Id = -1 };
            SalePoint salePoint = new SalePoint() { Id = -1 };
            Prefix prefix = new Prefix() { Id = -1 };
            individuals.Person insured = new individuals.Person();
            individuals.Person intermediary = new individuals.Person();
            CancellationPolicyType cancellationPolicyType = new CancellationPolicyType();

            policies = cancellationPolicyService.GetCancellationPolicies(branch, salePoint, prefix, null,
                                                                         insured, intermediary, SessionHelper.GetUserName(),
                                                                         -1, DateTime.Now, DateTime.Now,
                                                                         DateTime.Now, cancellationPolicyType);

            foreach (Policy policy in policies)
            {
                var progress = "";
                var finalized = "";

                if (Convert.ToDateTime(policy.CurrentTo) == Convert.ToDateTime("01/01/0001 0:00:00"))
                {
                    progress = @Global.InProcess;
                    finalized = "";
                }
                else
                {
                    progress = @Global.Finalized;
                    finalized = "T";
                }

                jsonData.Add(new
                {
                    Id = policy.Id,
                    ProcessDate = policy.DateAndTimeFrom.ToString("dd/MM/yyyy H:mm:ss"),
                    TotalRecords = policy.Product.Id,
                    TotalPremium = 0,
                    UserName = policy.Currency.Description,
                    IsActive = policy.PolicyHolderPerson.Id == 0 ? Global.Active : Global.Finalized,
                    Progress = progress,
                    Finalized = finalized
                });
            }
            */
            
            jsonData.Add(new
            {
                Id = 1,
                ProcessDate = DateTime.Now.ToString("dd/MM/yyyy H:mm:ss"),
                TotalRecords = 5,
                TotalPremium = 2516,
                UserName = User.Identity.Name, // SessionHelper.GetUserName(),
                IsActive = "Actived",
                Progress = "Finalizado",
                Finalized = "T"
            });

            return new UifTableResult(jsonData);
        }

        /// <summary>
        /// ReadPolicyFileInMemory
        /// Lee el registro en memoria
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="processNumber"></param>
        /// <param name="processDate"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ReadPolicyFileInMemory(HttpPostedFileBase uploadedFile, int? processNumber, string processDate)
        {
            string fileLocationName = "";
            fileLocationName = uploadedFile.FileName;
            string[] data = fileLocationName.Split(new char[] { '.' });

            if (data[1] == "xls" || data[1] == "xlsx")
            {
                return ReadPolicyExcelFileToStream(uploadedFile, Convert.ToInt32(processNumber), processDate);
            }
            else
            {
                fileLocationName = "BadFileExtension";
            }
            object[] jsonData = new object[2];

            jsonData[0] = fileLocationName;
            jsonData[1] = false;

            return Json(jsonData, JsonRequestBehavior.AllowGet);
            //return Json(fileLocationName, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ReadCancellationFileInMemory
        /// Lee el registro en memoria
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="processNumber"></param>
        /// <param name="processDate"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ReadCancellationFileInMemory(HttpPostedFileBase uploadedFile, int? processNumber, string processDate)
        {
            string fileLocationName = "";
            fileLocationName = uploadedFile.FileName;
            string[] data = fileLocationName.Split(new char[] { '.' });

            if (data[1] == "xls" || data[1] == "xlsx")
            {
                return ReadCancellationExcelFileToStream(uploadedFile, Convert.ToInt32(processNumber), processDate);
            }
            else
            {
                fileLocationName = "BadFileExtension";
            }

            object[] jsonData = new object[2];

            jsonData[0] = fileLocationName;
            jsonData[1] = false;

            return Json(jsonData, JsonRequestBehavior.AllowGet);
            //return Json(fileLocationName, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ReadMistakeFileInMemory
        /// Lee el registro en memoria
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="processNumber"></param>
        /// <param name="processDate"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ReadMistakeFileInMemory(HttpPostedFileBase uploadedFile, int? processNumber, string processDate)
        {
            string fileLocationName = "";
            fileLocationName = uploadedFile.FileName;
            string[] data = fileLocationName.Split(new char[] { '.' });

            if (data[1] == "xls" || data[1] == "xlsx")
            {
                return ReadMistakeExcelFileToStream(uploadedFile, Convert.ToInt32(processNumber), processDate);
            }
            else
            {
                fileLocationName = "BadFileExtension";
            }

            object[] jsonData = new object[2];

            jsonData[0] = fileLocationName;
            jsonData[1] = false;

            return Json(jsonData, JsonRequestBehavior.AllowGet);
            //return Json(fileLocationName, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// ExportCancellationsToExcel: Exporta a Excel el resultado de las cancelaciones procesadas
        /// </summary>
        /// <param name="header"></param>
        /// <param name="policies"></param>
        /// <returns>MemoryStream</returns>
        private MemoryStream ExportCancellationsToExcel(/*CancellationProcess header,*/ List<PolicyCancellationModel> policies)
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            var font = workbook.CreateFont();

            try
            {
                font.FontName = "Tahoma";
                font.FontHeightInPoints = 8;
                font.Boldweight = 3;
                font.Color = HSSFColor.White.Index;

                ICellStyle styleHeader = workbook.CreateCellStyle();
                styleHeader.SetFont(font);
                styleHeader.FillForegroundColor = HSSFColor.LightBlue.Index;
                styleHeader.FillPattern = FillPattern.SolidForeground;

                var fontDetail = workbook.CreateFont();
                fontDetail.FontName = "Tahoma";
                fontDetail.FontHeightInPoints = 8;
                fontDetail.Boldweight = 3;

                ICellStyle styleDetail = workbook.CreateCellStyle();
                styleDetail.SetFont(fontDetail);
                styleDetail.BottomBorderColor = HSSFColor.Black.Index;
                styleDetail.LeftBorderColor = HSSFColor.Black.Index;
                styleDetail.RightBorderColor = HSSFColor.Black.Index;
                styleDetail.TopBorderColor = HSSFColor.Black.Index;
                styleDetail.BorderBottom = BorderStyle.Thin;
                styleDetail.BorderLeft = BorderStyle.Thin;
                styleDetail.BorderRight = BorderStyle.Thin;
                styleDetail.BorderTop = BorderStyle.Thin;

                ICellStyle styleLine = workbook.CreateCellStyle();
                styleLine.SetFont(fontDetail);
                styleLine.BottomBorderColor = HSSFColor.Black.Index;
                styleLine.BorderBottom = BorderStyle.Thin;


                ICellStyle styleDoubleLine = workbook.CreateCellStyle();
                styleDoubleLine.SetFont(fontDetail);
                styleDoubleLine.BottomBorderColor = HSSFColor.Black.Index;
                styleDoubleLine.BorderBottom = BorderStyle.Double;

                ICellStyle styleLetter = workbook.CreateCellStyle();
                styleLetter.SetFont(fontDetail);

                // Cabecera
                var headerFile = sheet.CreateRow(0);

                headerFile.CreateCell(0).SetCellValue(@Global.ProcessNumber);
                headerFile.GetCell(0).CellStyle = styleHeader;
                //headerFile.CreateCell(1).SetCellValue(header.Id);
                headerFile.CreateCell(3).SetCellValue(@Global.ProcessDate);
                headerFile.GetCell(3).CellStyle = styleHeader;
                //headerFile.CreateCell(4).SetCellValue(header.Date.ToString("dd/MM/yyyy H:mm:ss"));

                // Detalle
                var headerRow = sheet.CreateRow(2);

                headerRow.CreateCell(0).SetCellValue(@Global.Id);
                headerRow.CreateCell(1).SetCellValue(@Global.AppliesCollection);
                headerRow.CreateCell(2).SetCellValue(@Global.QuoteDueDate);
                headerRow.CreateCell(3).SetCellValue(@Global.BranchId);
                headerRow.CreateCell(4).SetCellValue(@Global.Branch);
                headerRow.CreateCell(5).SetCellValue(@Global.SalePointId);
                headerRow.CreateCell(6).SetCellValue(@Global.SalePoint);
                headerRow.CreateCell(7).SetCellValue(@Global.PrefixId);
                headerRow.CreateCell(8).SetCellValue(@Global.Prefix);
                headerRow.CreateCell(9).SetCellValue(@Global.PolicyId);
                headerRow.CreateCell(10).SetCellValue(@Global.Policy);
                headerRow.CreateCell(11).SetCellValue(@Global.MotherPolicyId);
                headerRow.CreateCell(12).SetCellValue(@Global.InsuredId);
                headerRow.CreateCell(13).SetCellValue(@Global.Insured);
                headerRow.CreateCell(14).SetCellValue(@Global.IntermediaryTypeId);
                headerRow.CreateCell(15).SetCellValue(@Global.IntermediaryId);
                headerRow.CreateCell(16).SetCellValue(@Global.Intermediary);
                headerRow.CreateCell(17).SetCellValue(@Global.IssueDate);
                headerRow.CreateCell(18).SetCellValue(@Global.IssueDateFrom);
                headerRow.CreateCell(19).SetCellValue(@Global.IssueDateTo);
                headerRow.CreateCell(20).SetCellValue(@Global.CurrencyId);
                headerRow.CreateCell(21).SetCellValue(@Global.Currency);
                headerRow.CreateCell(22).SetCellValue(@Global.CancellationBalance);
                headerRow.CreateCell(23).SetCellValue(@Global.ApplicationTemporaryId);

                sheet.SetColumnWidth(0, 20 * 256);
                sheet.SetColumnWidth(1, 30 * 256);
                sheet.SetColumnWidth(2, 20 * 256);
                sheet.SetColumnWidth(3, 20 * 256);
                sheet.SetColumnWidth(4, 20 * 256);
                sheet.SetColumnWidth(5, 20 * 256);
                sheet.SetColumnWidth(6, 20 * 256);
                sheet.SetColumnWidth(7, 20 * 256);
                sheet.SetColumnWidth(8, 20 * 256);
                sheet.SetColumnWidth(9, 30 * 256);
                sheet.SetColumnWidth(10, 20 * 256);
                sheet.SetColumnWidth(11, 20 * 256);
                sheet.SetColumnWidth(12, 20 * 256);
                sheet.SetColumnWidth(13, 20 * 256);
                sheet.SetColumnWidth(14, 20 * 256);
                sheet.SetColumnWidth(15, 20 * 256);
                sheet.SetColumnWidth(16, 20 * 256);
                sheet.SetColumnWidth(17, 20 * 256);
                sheet.SetColumnWidth(18, 20 * 256);
                sheet.SetColumnWidth(19, 20 * 256);
                sheet.SetColumnWidth(20, 20 * 256);
                sheet.SetColumnWidth(21, 20 * 256);
                sheet.SetColumnWidth(22, 20 * 256);
                sheet.SetColumnWidth(23, 20 * 256);

                sheet.CreateFreezePane(0, 1, 0, 1);

                headerRow.GetCell(0).CellStyle = styleHeader;
                headerRow.GetCell(1).CellStyle = styleHeader;
                headerRow.GetCell(2).CellStyle = styleHeader;
                headerRow.GetCell(3).CellStyle = styleHeader;
                headerRow.GetCell(4).CellStyle = styleHeader;
                headerRow.GetCell(5).CellStyle = styleHeader;
                headerRow.GetCell(6).CellStyle = styleHeader;
                headerRow.GetCell(7).CellStyle = styleHeader;
                headerRow.GetCell(8).CellStyle = styleHeader;
                headerRow.GetCell(9).CellStyle = styleHeader;
                headerRow.GetCell(10).CellStyle = styleHeader;
                headerRow.GetCell(11).CellStyle = styleHeader;
                headerRow.GetCell(12).CellStyle = styleHeader;
                headerRow.GetCell(13).CellStyle = styleHeader;
                headerRow.GetCell(14).CellStyle = styleHeader;
                headerRow.GetCell(15).CellStyle = styleHeader;
                headerRow.GetCell(16).CellStyle = styleHeader;
                headerRow.GetCell(17).CellStyle = styleHeader;
                headerRow.GetCell(18).CellStyle = styleHeader;
                headerRow.GetCell(19).CellStyle = styleHeader;
                headerRow.GetCell(20).CellStyle = styleHeader;
                headerRow.GetCell(21).CellStyle = styleHeader;
                headerRow.GetCell(22).CellStyle = styleHeader;
                headerRow.GetCell(23).CellStyle = styleHeader;

                var rowNumber = 3;

                if (policies.Count > 0)
                {
                    foreach (var policy in policies)
                    {
                        var row = sheet.CreateRow(rowNumber++);

                        SetCellValue(0, row, policy.Id.ToString(), styleDetail);
                        SetCellValue(1, row, policy.AppliesCollection, styleDetail);
                        SetCellValue(2, row, policy.QuoteDueDate.ToString("dd/MM/yyyy"), styleDetail);
                        SetCellValue(3, row, policy.BranchId.ToString(), styleDetail);
                        SetCellValue(4, row, policy.BranchDescription, styleDetail);
                        SetCellValue(5, row, policy.SalePointId.ToString(), styleDetail);
                        SetCellValue(6, row, policy.SalePointDescription, styleDetail);
                        SetCellValue(7, row, policy.PrefixId.ToString(), styleDetail);
                        SetCellValue(8, row, policy.PrefixDescription, styleDetail);
                        SetCellValue(9, row, policy.PolicyId.ToString(), styleDetail);
                        SetCellValue(10, row, policy.Policy, styleDetail);
                        SetCellValue(11, row, policy.MotherPolicyId.ToString(), styleDetail);
                        SetCellValue(12, row, policy.InsuredId.ToString(), styleDetail);
                        SetCellValue(13, row, policy.Insured, styleDetail);
                        SetCellValue(14, row, policy.IntermediaryTypeId.ToString(), styleDetail);
                        SetCellValue(15, row, policy.IntermediaryId.ToString(), styleDetail);
                        SetCellValue(16, row, policy.Intermediary, styleDetail);
                        SetCellValue(17, row, policy.IssueDate.ToString("dd/MM/yyyy"), styleDetail);
                        SetCellValue(18, row, policy.IssueDateFrom.ToString("dd/MM/yyyy"), styleDetail);
                        SetCellValue(19, row, policy.IssueDateTo.ToString("dd/MM/yyyy"), styleDetail);
                        SetCellValue(20, row, policy.CurrencyId.ToString(), styleDetail);
                        SetCellValue(21, row, policy.CurrencyDescription, styleDetail);
                        SetCellValue(22, row, string.Format(new CultureInfo("en-US"), "{0:C}", policy.Amount), styleDetail);
                        SetCellValue(23, row, policy.ApplicationTemporaryId.ToString(), styleDetail);
                    }
                }

                HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                // Create the anchor
                HSSFClientAnchor anchor;
                anchor = new HSSFClientAnchor(0, 0, 0, 0, 2, 1, 0, 0);
                anchor.AnchorType = 5;
                MemoryStream memoryStream = new MemoryStream();
                workbook.Write(memoryStream);

                return memoryStream;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// SetCellValue
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="row"></param>
        /// <param name="value"></param>
        /// <param name="styleDetail"></param>
        private void SetCellValue(int columnIndex, IRow row, string value, ICellStyle styleDetail)
        {
            row.CreateCell(columnIndex).SetCellValue(value);
            row.GetCell(columnIndex).CellStyle = styleDetail;
        }

        /// <summary>
        /// ExportMistakesToExcel: Exporta a Excel el resultado de las cancelaciones procesadas
        /// </summary>
        /// <param name="header"></param>
        /// <param name="policies"></param>
        /// <returns>MemoryStream</returns>
        private MemoryStream ExportMistakesToExcel(/*CancellationProcess header,*/ List<PolicyCancellationModel> policies)
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            var font = workbook.CreateFont();

            try
            {

                font.FontName = "Tahoma";
                font.FontHeightInPoints = 8;
                font.Boldweight = 3;
                font.Color = HSSFColor.White.Index;

                ICellStyle styleHeader = workbook.CreateCellStyle();
                styleHeader.SetFont(font);
                styleHeader.FillForegroundColor = HSSFColor.LightBlue.Index;
                styleHeader.FillPattern = FillPattern.SolidForeground;

                var fontDetail = workbook.CreateFont();
                fontDetail.FontName = "Tahoma";
                fontDetail.FontHeightInPoints = 8;
                fontDetail.Boldweight = 3;

                ICellStyle styleDetail = workbook.CreateCellStyle();
                styleDetail.SetFont(fontDetail);
                styleDetail.BottomBorderColor = HSSFColor.Black.Index;
                styleDetail.LeftBorderColor = HSSFColor.Black.Index;
                styleDetail.RightBorderColor = HSSFColor.Black.Index;
                styleDetail.TopBorderColor = HSSFColor.Black.Index;
                styleDetail.BorderBottom = BorderStyle.Thin;
                styleDetail.BorderLeft = BorderStyle.Thin;
                styleDetail.BorderRight = BorderStyle.Thin;
                styleDetail.BorderTop = BorderStyle.Thin;

                ICellStyle styleLine = workbook.CreateCellStyle();
                styleLine.SetFont(fontDetail);
                styleLine.BottomBorderColor = HSSFColor.Black.Index;
                styleLine.BorderBottom = BorderStyle.Thin;


                ICellStyle styleDoubleLine = workbook.CreateCellStyle();
                styleDoubleLine.SetFont(fontDetail);
                styleDoubleLine.BottomBorderColor = HSSFColor.Black.Index;
                styleDoubleLine.BorderBottom = BorderStyle.Double;

                ICellStyle styleLetter = workbook.CreateCellStyle();
                styleLetter.SetFont(fontDetail);

                // Cabecera
                var headerFile = sheet.CreateRow(0);

                headerFile.CreateCell(0).SetCellValue(@Global.ProcessNumber);
                headerFile.GetCell(0).CellStyle = styleHeader;
                //headerFile.CreateCell(1).SetCellValue(header.Id);
                headerFile.CreateCell(3).SetCellValue(@Global.ProcessDate);
                headerFile.GetCell(3).CellStyle = styleHeader;
                //headerFile.CreateCell(4).SetCellValue(header.Date.ToString("dd/MM/yyyy H:mm:ss"));

                // Detalle
                var headerRow = sheet.CreateRow(2);

                headerRow.CreateCell(0).SetCellValue(@Global.Id);
                headerRow.CreateCell(1).SetCellValue(@Global.Reprocess);
                headerRow.CreateCell(2).SetCellValue(@Global.QuoteDueDate);
                headerRow.CreateCell(3).SetCellValue(@Global.BranchId);
                headerRow.CreateCell(4).SetCellValue(@Global.Branch);
                headerRow.CreateCell(5).SetCellValue(@Global.SalePointId);
                headerRow.CreateCell(6).SetCellValue(@Global.SalePoint);
                headerRow.CreateCell(7).SetCellValue(@Global.PrefixId);
                headerRow.CreateCell(8).SetCellValue(@Global.Prefix);
                headerRow.CreateCell(9).SetCellValue(@Global.PolicyId);
                headerRow.CreateCell(10).SetCellValue(@Global.Policy);
                headerRow.CreateCell(11).SetCellValue(@Global.MotherPolicyId);
                headerRow.CreateCell(12).SetCellValue(@Global.InsuredId);
                headerRow.CreateCell(13).SetCellValue(@Global.Insured);
                headerRow.CreateCell(14).SetCellValue(@Global.IntermediaryTypeId);
                headerRow.CreateCell(15).SetCellValue(@Global.IntermediaryId);
                headerRow.CreateCell(16).SetCellValue(@Global.Intermediary);
                headerRow.CreateCell(17).SetCellValue(@Global.IssueDate);
                headerRow.CreateCell(18).SetCellValue(@Global.IssueDateFrom);
                headerRow.CreateCell(19).SetCellValue(@Global.IssueDateTo);
                headerRow.CreateCell(20).SetCellValue(@Global.CurrencyId);
                headerRow.CreateCell(21).SetCellValue(@Global.Currency);
                headerRow.CreateCell(22).SetCellValue(@Global.TotalPremium);
                headerRow.CreateCell(23).SetCellValue(@Global.Description);

                sheet.SetColumnWidth(0, 20 * 256);
                sheet.SetColumnWidth(1, 30 * 256);
                sheet.SetColumnWidth(2, 20 * 256);
                sheet.SetColumnWidth(3, 20 * 256);
                sheet.SetColumnWidth(4, 20 * 256);
                sheet.SetColumnWidth(5, 20 * 256);
                sheet.SetColumnWidth(6, 20 * 256);
                sheet.SetColumnWidth(7, 20 * 256);
                sheet.SetColumnWidth(8, 20 * 256);
                sheet.SetColumnWidth(9, 30 * 256);
                sheet.SetColumnWidth(10, 20 * 256);
                sheet.SetColumnWidth(11, 20 * 256);
                sheet.SetColumnWidth(12, 20 * 256);
                sheet.SetColumnWidth(13, 20 * 256);
                sheet.SetColumnWidth(14, 20 * 256);
                sheet.SetColumnWidth(15, 20 * 256);
                sheet.SetColumnWidth(16, 30 * 256);
                sheet.SetColumnWidth(17, 20 * 256);
                sheet.SetColumnWidth(18, 20 * 256);
                sheet.SetColumnWidth(19, 20 * 256);
                sheet.SetColumnWidth(20, 20 * 256);
                sheet.SetColumnWidth(21, 20 * 256);
                sheet.SetColumnWidth(22, 20 * 256);
                sheet.SetColumnWidth(23, 20 * 256);

                sheet.CreateFreezePane(0, 1, 0, 1);

                headerRow.GetCell(0).CellStyle = styleHeader;
                headerRow.GetCell(1).CellStyle = styleHeader;
                headerRow.GetCell(2).CellStyle = styleHeader;
                headerRow.GetCell(3).CellStyle = styleHeader;
                headerRow.GetCell(4).CellStyle = styleHeader;
                headerRow.GetCell(5).CellStyle = styleHeader;
                headerRow.GetCell(6).CellStyle = styleHeader;
                headerRow.GetCell(7).CellStyle = styleHeader;
                headerRow.GetCell(8).CellStyle = styleHeader;
                headerRow.GetCell(9).CellStyle = styleHeader;
                headerRow.GetCell(10).CellStyle = styleHeader;
                headerRow.GetCell(11).CellStyle = styleHeader;
                headerRow.GetCell(12).CellStyle = styleHeader;
                headerRow.GetCell(13).CellStyle = styleHeader;
                headerRow.GetCell(14).CellStyle = styleHeader;
                headerRow.GetCell(15).CellStyle = styleHeader;
                headerRow.GetCell(16).CellStyle = styleHeader;
                headerRow.GetCell(17).CellStyle = styleHeader;
                headerRow.GetCell(18).CellStyle = styleHeader;
                headerRow.GetCell(19).CellStyle = styleHeader;
                headerRow.GetCell(20).CellStyle = styleHeader;
                headerRow.GetCell(21).CellStyle = styleHeader;
                headerRow.GetCell(22).CellStyle = styleHeader;
                headerRow.GetCell(23).CellStyle = styleHeader;

                var rowNumber = 3;

                if (policies.Count > 0)
                {
                    foreach (var policy in policies)
                    {
                        var row = sheet.CreateRow(rowNumber++);

                        SetCellValue(0, row, policy.Id.ToString(), styleDetail);
                        SetCellValue(1, row, policy.Reprocess, styleDetail);
                        SetCellValue(2, row, policy.QuoteDueDate.ToString("dd/MM/yyyy"), styleDetail);
                        SetCellValue(3, row, policy.BranchId.ToString(), styleDetail);
                        SetCellValue(4, row, policy.BranchDescription, styleDetail);
                        SetCellValue(5, row, policy.SalePointId.ToString(), styleDetail);
                        SetCellValue(6, row, policy.SalePointDescription, styleDetail);
                        SetCellValue(7, row, policy.PrefixId.ToString(), styleDetail);
                        SetCellValue(8, row, policy.PrefixDescription, styleDetail);
                        SetCellValue(9, row, policy.PolicyId.ToString(), styleDetail);
                        SetCellValue(10, row, policy.Policy, styleDetail);
                        SetCellValue(11, row, policy.MotherPolicyId.ToString(), styleDetail);
                        SetCellValue(12, row, policy.InsuredId.ToString(), styleDetail);
                        SetCellValue(13, row, policy.Insured, styleDetail);
                        SetCellValue(14, row, policy.IntermediaryTypeId.ToString(), styleDetail);
                        SetCellValue(15, row, policy.IntermediaryId.ToString(), styleDetail);
                        SetCellValue(16, row, policy.Intermediary, styleDetail);
                        SetCellValue(17, row, policy.IssueDate.ToString("dd/MM/yyyy"), styleDetail);
                        SetCellValue(18, row, policy.IssueDateFrom.ToString("dd/MM/yyyy"), styleDetail);
                        SetCellValue(19, row, policy.IssueDateTo.ToString("dd/MM/yyyy"), styleDetail);
                        SetCellValue(20, row, policy.CurrencyId.ToString(), styleDetail);
                        SetCellValue(21, row, policy.CurrencyDescription, styleDetail);
                        SetCellValue(22, row, string.Format(new CultureInfo("en-US"), "{0:C}", policy.Amount), styleDetail);
                        SetCellValue(23, row, policy.Observations, styleDetail);
                    }
                }

                HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                // Create the anchor
                HSSFClientAnchor anchor;
                anchor = new HSSFClientAnchor(0, 0, 0, 0, 2, 1, 0, 0);
                anchor.AnchorType = 5;
                MemoryStream memoryStream = new MemoryStream();
                workbook.Write(memoryStream);

                return memoryStream;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// ExportAppliedCancellationsToExcel: Exporta a Excel el resultado de las cancelaciones aplicadas
        /// </summary>
        /// <param name="header"></param>
        /// <param name="policies"></param>
        /// <returns>MemoryStream</returns>
        private MemoryStream ExportAppliedCancellationsToExcel(/*CancellationProcess header,*/ List<PolicyCancellationModel> policies)
        {
            var workbook = new HSSFWorkbook();
            var sheet = workbook.CreateSheet();
            var font = workbook.CreateFont();

            try
            {

                font.FontName = "Tahoma";
                font.FontHeightInPoints = 8;
                font.Boldweight = 3;
                font.Color = HSSFColor.White.Index;

                ICellStyle styleHeader = workbook.CreateCellStyle();
                styleHeader.SetFont(font);
                styleHeader.FillForegroundColor = HSSFColor.LightBlue.Index;
                styleHeader.FillPattern = FillPattern.SolidForeground;

                var fontDetail = workbook.CreateFont();
                fontDetail.FontName = "Tahoma";
                fontDetail.FontHeightInPoints = 8;
                fontDetail.Boldweight = 3;

                ICellStyle styleDetail = workbook.CreateCellStyle();
                styleDetail.SetFont(fontDetail);
                styleDetail.BottomBorderColor = HSSFColor.Black.Index;
                styleDetail.LeftBorderColor = HSSFColor.Black.Index;
                styleDetail.RightBorderColor = HSSFColor.Black.Index;
                styleDetail.TopBorderColor = HSSFColor.Black.Index;
                styleDetail.BorderBottom = BorderStyle.Thin;
                styleDetail.BorderLeft = BorderStyle.Thin;
                styleDetail.BorderRight = BorderStyle.Thin;
                styleDetail.BorderTop = BorderStyle.Thin;

                ICellStyle styleLine = workbook.CreateCellStyle();
                styleLine.SetFont(fontDetail);
                styleLine.BottomBorderColor = HSSFColor.Black.Index;
                styleLine.BorderBottom = BorderStyle.Thin;


                ICellStyle styleDoubleLine = workbook.CreateCellStyle();
                styleDoubleLine.SetFont(fontDetail);
                styleDoubleLine.BottomBorderColor = HSSFColor.Black.Index;
                styleDoubleLine.BorderBottom = BorderStyle.Double;

                ICellStyle styleLetter = workbook.CreateCellStyle();
                styleLetter.SetFont(fontDetail);

                // Cabecera
                var headerFile = sheet.CreateRow(0);

                headerFile.CreateCell(0).SetCellValue(@Global.ProcessNumber);
                headerFile.GetCell(0).CellStyle = styleHeader;
                //headerFile.CreateCell(1).SetCellValue(header.Id);
                headerFile.CreateCell(3).SetCellValue(@Global.ProcessDate);
                headerFile.GetCell(3).CellStyle = styleHeader;
                //headerFile.CreateCell(4).SetCellValue(header.Date.ToString("dd/MM/yyyy H:mm:ss"));

                // Detalle
                var headerRow = sheet.CreateRow(2);

                headerRow.CreateCell(0).SetCellValue(@Global.Id);
                headerRow.CreateCell(1).SetCellValue(@Global.BranchId);
                headerRow.CreateCell(2).SetCellValue(@Global.Branch);
                headerRow.CreateCell(3).SetCellValue(@Global.SalePointId);
                headerRow.CreateCell(4).SetCellValue(@Global.SalePoint);
                headerRow.CreateCell(5).SetCellValue(@Global.PrefixId);
                headerRow.CreateCell(6).SetCellValue(@Global.Prefix);
                headerRow.CreateCell(7).SetCellValue(@Global.PolicyId);
                headerRow.CreateCell(8).SetCellValue(@Global.Policy);
                headerRow.CreateCell(9).SetCellValue(@Global.MotherPolicyId);
                headerRow.CreateCell(10).SetCellValue(@Global.InsuredId);
                headerRow.CreateCell(11).SetCellValue(@Global.Insured);
                headerRow.CreateCell(12).SetCellValue(@Global.IntermediaryTypeId);
                headerRow.CreateCell(13).SetCellValue(@Global.IntermediaryId);
                headerRow.CreateCell(14).SetCellValue(@Global.Intermediary);
                headerRow.CreateCell(15).SetCellValue(@Global.CurrencyId);
                headerRow.CreateCell(16).SetCellValue(@Global.Currency);
                headerRow.CreateCell(17).SetCellValue(@Global.CollectionDate);
                headerRow.CreateCell(18).SetCellValue(@Global.TotalPremium);
                headerRow.CreateCell(19).SetCellValue(@Global.ApplicationReceiptNumber);
                headerRow.CreateCell(20).SetCellValue(@Global.JournalEntryNumber);

                sheet.SetColumnWidth(0, 20 * 256);
                sheet.SetColumnWidth(1, 30 * 256);
                sheet.SetColumnWidth(2, 20 * 256);
                sheet.SetColumnWidth(3, 20 * 256);
                sheet.SetColumnWidth(4, 20 * 256);
                sheet.SetColumnWidth(5, 20 * 256);
                sheet.SetColumnWidth(6, 20 * 256);
                sheet.SetColumnWidth(7, 20 * 256);
                sheet.SetColumnWidth(8, 20 * 256);
                sheet.SetColumnWidth(9, 30 * 256);
                sheet.SetColumnWidth(10, 20 * 256);
                sheet.SetColumnWidth(11, 20 * 256);
                sheet.SetColumnWidth(12, 20 * 256);
                sheet.SetColumnWidth(13, 20 * 256);
                sheet.SetColumnWidth(14, 20 * 256);
                sheet.SetColumnWidth(15, 20 * 256);
                sheet.SetColumnWidth(16, 20 * 256);
                sheet.SetColumnWidth(17, 30 * 256);
                sheet.SetColumnWidth(18, 20 * 256);
                sheet.SetColumnWidth(19, 20 * 256);
                sheet.SetColumnWidth(20, 20 * 256);

                sheet.CreateFreezePane(0, 1, 0, 1);

                headerRow.GetCell(0).CellStyle = styleHeader;
                headerRow.GetCell(1).CellStyle = styleHeader;
                headerRow.GetCell(2).CellStyle = styleHeader;
                headerRow.GetCell(3).CellStyle = styleHeader;
                headerRow.GetCell(4).CellStyle = styleHeader;
                headerRow.GetCell(5).CellStyle = styleHeader;
                headerRow.GetCell(6).CellStyle = styleHeader;
                headerRow.GetCell(7).CellStyle = styleHeader;
                headerRow.GetCell(8).CellStyle = styleHeader;
                headerRow.GetCell(9).CellStyle = styleHeader;
                headerRow.GetCell(10).CellStyle = styleHeader;
                headerRow.GetCell(11).CellStyle = styleHeader;
                headerRow.GetCell(12).CellStyle = styleHeader;
                headerRow.GetCell(13).CellStyle = styleHeader;
                headerRow.GetCell(14).CellStyle = styleHeader;
                headerRow.GetCell(15).CellStyle = styleHeader;
                headerRow.GetCell(16).CellStyle = styleHeader;
                headerRow.GetCell(17).CellStyle = styleHeader;
                headerRow.GetCell(18).CellStyle = styleHeader;
                headerRow.GetCell(19).CellStyle = styleHeader;
                headerRow.GetCell(20).CellStyle = styleHeader;

                var rowNumber = 3;

                if (policies.Count > 0)
                {
                    foreach (var policy in policies)
                    {
                        var row = sheet.CreateRow(rowNumber++);

                        SetCellValue(0, row, policy.Id.ToString(), styleDetail);
                        SetCellValue(1, row, policy.BranchId.ToString(), styleDetail);
                        SetCellValue(2, row, policy.BranchDescription, styleDetail);
                        SetCellValue(3, row, policy.SalePointId.ToString(), styleDetail);
                        SetCellValue(4, row, policy.SalePointDescription, styleDetail);
                        SetCellValue(5, row, policy.PrefixId.ToString(), styleDetail);
                        SetCellValue(6, row, policy.PrefixDescription, styleDetail);
                        SetCellValue(7, row, policy.PolicyId.ToString(), styleDetail);
                        SetCellValue(8, row, policy.Policy, styleDetail);
                        SetCellValue(9, row, policy.MotherPolicyId.ToString(), styleDetail);
                        SetCellValue(10, row, policy.InsuredId.ToString(), styleDetail);
                        SetCellValue(11, row, policy.Insured, styleDetail);
                        SetCellValue(12, row, policy.IntermediaryTypeId.ToString(), styleDetail);
                        SetCellValue(13, row, policy.IntermediaryId.ToString(), styleDetail);
                        SetCellValue(14, row, policy.Intermediary, styleDetail);
                        SetCellValue(15, row, policy.CurrencyId.ToString(), styleDetail);
                        SetCellValue(16, row, policy.CurrencyDescription, styleDetail);
                        SetCellValue(17, row, policy.CollectionDate.ToString("dd/MM/yyyy"), styleDetail);
                        SetCellValue(18, row, string.Format(new CultureInfo("en-US"), "{0:C}", policy.Amount), styleDetail);
                        SetCellValue(19, row, policy.ApplicationReceiptNumber.ToString(), styleDetail);
                        SetCellValue(20, row, policy.EntryNumber.ToString(), styleDetail);
                    }
                }

                HSSFPatriarch patriarch = (HSSFPatriarch)sheet.CreateDrawingPatriarch();
                // Create the anchor
                HSSFClientAnchor anchor;
                anchor = new HSSFClientAnchor(0, 0, 0, 0, 2, 1, 0, 0);
                anchor.AnchorType = 5;
                MemoryStream memoryStream = new MemoryStream();
                workbook.Write(memoryStream);

                return memoryStream;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// ReadPolicyExcelFileToStream
        /// Lee un documento excel
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="processNumber"></param>
        /// <param name="processDate"></param>
        /// <returns>JsonResult</returns>
        private JsonResult ReadPolicyExcelFileToStream(HttpPostedFileBase uploadedFile, int processNumber, string processDate)
        {
            bool successful = true;
            bool validateHeader = false;
            string fileLocationName = "";
            string message = "";
            Byte[] arrayContent;            

            fileLocationName = uploadedFile.FileName;
            string[] data = fileLocationName.Split(new char[] { '.' });

            // Convertir a Bytes
            var buffer = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(buffer, 0, (int)uploadedFile.InputStream.Length);

            // Lee el archivo y guarda en arreglo de tipo byte y este a su vez a arrayContent
            arrayContent = buffer;

            Stream stream = new MemoryStream(arrayContent);
            IExcelDataReader excelReader;

            try
            {
                if (data[1] == "xls")
                {
                    // 1. Lee desde binary Excel  ('97-2003 format; *.xls)
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else
                {
                    // 2. Lee desde binary OpenXml Excel file (2007 format; *.xlsx)
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                // 3. DataSet - El resultado sera creado en result.Tables
                DataSet dataSet = excelReader.AsDataSet();

                int recordNumber = dataSet.Tables[0].Rows.Count;
                //CancellationProcess updateCancellationProcess = new CancellationProcess();

                // Se lee la cabecera  
                if (recordNumber > 0)
                {
                    if (Convert.ToString(dataSet.Tables[0].Rows[0][1]) != "")
                    {
                        if (Convert.ToInt32(dataSet.Tables[0].Rows[0][1]) == processNumber)
                        {
                            validateHeader = true;
                        }
                        else
                        {
                            message = "NoProcessNumber";
                            validateHeader = false;
                        }
                    }
                    else
                    {
                        validateHeader = false;
                        message = "NoProcessNumber";
                    }
                    if (validateHeader)
                    {
                        if (Convert.ToString(dataSet.Tables[0].Rows[0][4]) != "")
                        {
                            if (Convert.ToString(dataSet.Tables[0].Rows[0][4]) == processDate)
                            {
                                validateHeader = true;
                            }
                            else
                            {
                                validateHeader = false;
                                message = "NoProcessDate";
                            }
                        }
                        else
                        {
                            validateHeader = false;
                            message = "NoProcessDate";
                        }
                    }
                }

                if (validateHeader)
                {
                    // Se lee el detalle
                    var rows = dataSet.Tables[0].Rows;

                    // Se valida la cabecera del detalle (Id)
                    if (Convert.ToString(rows[2][0]) != @Global.Id)
                    {
                        validateHeader = false;
                    }
                    // Anular
                    if (Convert.ToString(rows[2][1]) != @Global.Annul)
                    {
                        validateHeader = false;
                    }
                    // Id. Razón Exclusión
                    if (Convert.ToString(rows[2][2]) != @Global.ExclusionReasonId)
                    {
                        validateHeader = false;
                    }
                    // Motivo Exclusión
                    if (Convert.ToString(rows[2][3]) != @Global.ExclusionReason)
                    {
                        validateHeader = false;
                    }
                    // Fecha Vencimiento Cuota
                    if (Convert.ToString(rows[2][4]) != @Global.QuoteDueDate)
                    {
                        validateHeader = false;
                    }
                    // Id. Sucursal
                    if (Convert.ToString(rows[2][5]) != @Global.BranchId)
                    {
                        validateHeader = false;
                    }
                    // Sucursal
                    if (Convert.ToString(rows[2][6]) != @Global.Branch)
                    {
                        validateHeader = false;
                    }
                    // Id. Punto Venta
                    if (Convert.ToString(rows[2][7]) != @Global.SalePointId)
                    {
                        validateHeader = false;
                    }
                    // Punto de Venta
                    if (Convert.ToString(rows[2][8]) != @Global.SalePoint)
                    {
                        validateHeader = false;
                    }
                    // Id. Ramo
                    if (Convert.ToString(rows[2][9]) != @Global.PrefixId)
                    {
                        validateHeader = false;
                    }
                    // Ramo
                    if (Convert.ToString(rows[2][10]) != @Global.Prefix)
                    {
                        validateHeader = false;
                    }
                    // Id. Póliza
                    if (Convert.ToString(rows[2][11]) != @Global.PolicyId)
                    {
                        validateHeader = false;
                    }
                    // Póliza
                    if (Convert.ToString(rows[2][12]) != @Global.Policy)
                    {
                        validateHeader = false;
                    }
                    // Id. Póliza Madre
                    if (Convert.ToString(rows[2][13]) != @Global.MotherPolicyId)
                    {
                        validateHeader = false;
                    }
                    // Id. Asegurado
                    if (Convert.ToString(rows[2][14]) != @Global.InsuredId)
                    {
                        validateHeader = false;
                    }
                    // Asegurado
                    if (Convert.ToString(rows[2][15]) != @Global.Insured)
                    {
                        validateHeader = false;
                    }
                    // Id. Tipo Intermediario
                    if (Convert.ToString(rows[2][16]) != @Global.IntermediaryTypeId)
                    {
                        validateHeader = false;
                    }
                    // Id. Intermediario
                    if (Convert.ToString(rows[2][17]) != @Global.IntermediaryId)
                    {
                        validateHeader = false;
                    }
                    // Intermediario
                    if (Convert.ToString(rows[2][18]) != @Global.Intermediary)
                    {
                        validateHeader = false;
                    }
                    // Fecha Emisión
                    if (Convert.ToString(rows[2][19]) != @Global.IssueDate)
                    {
                        validateHeader = false;
                    }
                    // Fecha Emisión Desde
                    if (Convert.ToString(rows[2][20]) != @Global.IssueDateFrom)
                    {
                        validateHeader = false;
                    }
                    // Fecha Emisión Hasta
                    if (Convert.ToString(rows[2][21]) != @Global.IssueDateTo)
                    {
                        validateHeader = false;
                    }
                    // Id. Moneda
                    if (Convert.ToString(rows[2][22]) != @Global.CurrencyId)
                    {
                        validateHeader = false;
                    }
                    // Moneda
                    if (Convert.ToString(rows[2][23]) != @Global.Currency)
                    {
                        validateHeader = false;
                    }
                    // Prima Total
                    if (Convert.ToString(rows[2][24]) != @Global.TotalPremium)
                    {
                        validateHeader = false;
                    }
                    // Procesado
                    if (Convert.ToString(rows[2][25]) != @Global.Processed)
                    {
                        validateHeader = false;
                    }
                    // Observación
                    if (Convert.ToString(rows[2][26]) != @Global.Observations)
                    {
                        validateHeader = false;
                    }
                    // Exclusión Permite Cancelar
                    if (Convert.ToString(rows[2][27]) != @Global.ExclusionAllowsToCancel)
                    {
                        validateHeader = false;
                    }

                    if (validateHeader)
                    {
                        /*
                        CancellationProcess cancellationProcess = new CancellationProcess();
                        List<CancellationPolicy> policies = new List<CancellationPolicy>();

                        for (int index = 3; index < rows.Count; index++)
                        {
                            if (Convert.ToString(rows[index][0]) == "")
                            {
                                break; // Indica que no existe nro de registro en el archivo, se lo usa para fin de lectura de movimientos
                            }
                            else
                            {
                                var id = Convert.ToInt32(rows[index][0]);
                                var currencyId = Convert.ToInt32(rows[index][22]);
                                var branchId = Convert.ToInt32(rows[index][5]);
                                var salePointId = Convert.ToInt32(rows[index][7]);
                                var prefixId = Convert.ToInt32(rows[index][9]);
                                var policyNumber = Convert.ToInt32(rows[index][12]);

                                validateHeader = ValidatePolicyRowByProcessNumber(processNumber, id, branchId, salePointId, prefixId, policyNumber, "G");

                                if (validateHeader)
                                {
                                    CancellationPolicy cancellationPolicy = new CancellationPolicy();
                                    cancellationPolicy.Cancelled = false;
                                    cancellationPolicy.Id = id;
                                    Policy policy = new Policy();
                                    policy.Branch = new Branch() { Id = branchId };            // Id. Sucursal
                                    policy.BusinessType = new PolicyBusinessType()
                                    {
                                        Description = Convert.ToString(rows[index][3]),        // Motivo Exclusión
                                        Id = Convert.ToInt32(rows[index][2])                   // Id. Motivo Exclusión   
                                    };
                                    policy.Currency = new Currency() { Id = currencyId };      // Id. Moneda
                                    policy.DocumentNumber = policyNumber;                      // Nro. Póliza
                                    policy.Id = Convert.ToInt32(rows[index][11]);              // Id. Póliza
                                    policy.InitialPolicyId = Convert.ToInt32(rows[index][13]); // Id. Póliza Madre
                                    policy.EndorsementGroup = new EndorsementGroup()
                                    {
                                        Id = Convert.ToString(rows[index][27]) == "Si" ? 1 : 0 // Exclusión permite cancelar
                                    };

                                    policy.Prefix = new Prefix() { Id = prefixId };            // Id. Ramo
                                    policy.Product = new Product()
                                    {
                                        Description = Convert.ToString(rows[index][26]),       // Observación
                                        Id = Convert.ToString(rows[index][1]) == "Si" ? 1 : 0  // Anula
                                    };

                                    policy.SalePoint = new SalePoint() { Id = salePointId };   // Id. Pto. Venta

                                    cancellationPolicy.Policy = policy;

                                    policies.Add(cancellationPolicy);
                                }
                            }
                        }

                        cancellationProcess.CancellationPolicies = policies;
                        cancellationProcess.Date = Convert.ToDateTime(processDate);
                        cancellationProcess.Id = processNumber;
                        cancellationProcess.NumberPolicies = 1;
                        cancellationProcess.User = new User()
                        {
                            Id = _commonController.GetUserIdByName(User.Identity.Name), // SessionHelper.GetUserId(),
                            Nick = User.Identity.Name //SessionHelper.GetUserName()
                        };

                        updateCancellationProcess = cancellationPolicyService.SaveCancellationProcess(cancellationProcess);
                        */
                        successful = true;
                    }
                    else
                    {
                        message = "NoCorrespondColumns";
                        successful = false;
                    }

                }
                else
                {
                    successful = false;
                }
            }
            catch (FormatException)
            {
                message = "FormatException";
                successful = false;
            }
            catch (OverflowException)
            {
                message = "OverflowException";
                successful = false;
            }
            catch (IndexOutOfRangeException)
            {
                message = "IndexOutOfRangeException";
                successful = false;
            }
            catch (InvalidCastException)
            {
                message = "InvalidCastException";
                successful = false;
            }
            catch (Exception)
            {
                message = "Exception";
                successful = false;
            }

            stream.Close();
            buffer = null;
            arrayContent = null;

            object[] jsonData = new object[2];

            jsonData[0] = message;
            jsonData[1] = successful;

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ValidatePolicyRowByProcessNumber
        /// </summary>
        /// <param name="processNumber"></param>
        /// <param name="id"></param>
        /// <param name="branchId"></param>
        /// <param name="salePointId"></param>
        /// <param name="prefixId"></param>
        /// <param name="policyNumber"></param>
        /// <returns></returns>
        private bool ValidatePolicyRowByProcessNumber(int processNumber, int id, int branchId, int salePointId, int prefixId,
            int policyNumber, string tab)
        {
            bool exists = false;
            /*
            CancellationProcess cancellationProcess = new CancellationProcess();

            cancellationProcess.Id = processNumber;
            cancellationProcess.User = new User()
            {
                Id = SessionHelper.GetUserId(),
                Nick = SessionHelper.GetUserName(),
                Name = tab
            };
            cancellationProcess.NumberPolicies = 0;

            var newCancellationProcess = cancellationPolicyService.GetCancellationProcess(cancellationProcess);

            if (newCancellationProcess.CancellationPolicies != null)
            {
                foreach (CancellationPolicy policyItem in newCancellationProcess.CancellationPolicies)
                {
                    if (id == policyItem.Id && branchId == policyItem.Policy.Branch.Id && salePointId == policyItem.Policy.SalePoint.Id
                        && prefixId == policyItem.Policy.Prefix.Id && policyNumber == policyItem.Policy.DocumentNumber)
                    {
                        exists = true;
                        break;
                    }
                }
            }
            */
            return exists;
        }

        /// <summary>
        /// ReadCancellationExcelFileToStream
        /// Lee un documento excel
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="processNumber"></param>
        /// <param name="processDate"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ReadCancellationExcelFileToStream(HttpPostedFileBase uploadedFile, int processNumber, string processDate)
        {
            bool successful = true;
            bool validateHeader = false;
            string fileLocationName = "";
            string message = "";
            Byte[] arrayContent;
            
            fileLocationName = uploadedFile.FileName;
            string[] data = fileLocationName.Split(new char[] { '.' });

            // Convertir a Bytes
            var buffer = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(buffer, 0, (int)uploadedFile.InputStream.Length);

            // Lee el archivo y guarda en arreglo de tipo byte y este a su vez a arrayContent
            arrayContent = buffer;

            Stream stream = new MemoryStream(arrayContent);
            IExcelDataReader excelReader;

            try
            {
                if (data[1] == "xls")
                {
                    // 1. Lee desde binary Excel  ('97-2003 format; *.xls)
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else
                {
                    // 2. Lee desde binary OpenXml Excel file (2007 format; *.xlsx)
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                // 3. DataSet - El resultado sera creado en result.Tables
                DataSet dataSet = excelReader.AsDataSet();

                int recordNumber = dataSet.Tables[0].Rows.Count;
                //CancellationProcess updateCancellationProcess = new CancellationProcess();

                // Se lee la cabecera  
                if (recordNumber > 0)
                {
                    if (Convert.ToString(dataSet.Tables[0].Rows[0][1]) != "")
                    {
                        if (Convert.ToInt32(dataSet.Tables[0].Rows[0][1]) == processNumber)
                        {
                            validateHeader = true;
                        }
                        else
                        {
                            message = "NoProcessNumber";
                            validateHeader = false;
                        }
                    }
                    else
                    {
                        validateHeader = false;
                        message = "NoProcessNumber";
                    }
                    if (validateHeader)
                    {
                        if (Convert.ToString(dataSet.Tables[0].Rows[0][4]) != "")
                        {
                            if (Convert.ToString(dataSet.Tables[0].Rows[0][4]) == processDate)
                            {
                                validateHeader = true;
                            }
                            else
                            {
                                validateHeader = false;
                                message = "NoProcessDate";
                            }
                        }
                        else
                        {
                            validateHeader = false;
                            message = "NoProcessDate";
                        }
                    }
                }

                if (validateHeader)
                {
                    // Se lee el detalle
                    var rows = dataSet.Tables[0].Rows;

                    // Se valida la cabecera del detalle (Id)
                    if (Convert.ToString(rows[2][0]) != @Global.Id)
                    {
                        validateHeader = false;
                    }
                    // Aplica Cobro
                    if (Convert.ToString(rows[2][1]) != @Global.AppliesCollection)
                    {
                        validateHeader = false;
                    }
                    // Fecha Vencimiento Cuota
                    if (Convert.ToString(rows[2][2]) != @Global.QuoteDueDate)
                    {
                        validateHeader = false;
                    }
                    // Id. Sucursal
                    if (Convert.ToString(rows[2][3]) != @Global.BranchId)
                    {
                        validateHeader = false;
                    }
                    // Sucursal
                    if (Convert.ToString(rows[2][4]) != @Global.Branch)
                    {
                        validateHeader = false;
                    }
                    // Id. Punto Venta
                    if (Convert.ToString(rows[2][5]) != @Global.SalePointId)
                    {
                        validateHeader = false;
                    }
                    // Punto de Venta
                    if (Convert.ToString(rows[2][6]) != @Global.SalePoint)
                    {
                        validateHeader = false;
                    }
                    // Id. Ramo
                    if (Convert.ToString(rows[2][7]) != @Global.PrefixId)
                    {
                        validateHeader = false;
                    }
                    // Ramo
                    if (Convert.ToString(rows[2][8]) != @Global.Prefix)
                    {
                        validateHeader = false;
                    }
                    // Id. Póliza
                    if (Convert.ToString(rows[2][9]) != @Global.PolicyId)
                    {
                        validateHeader = false;
                    }
                    // Póliza
                    if (Convert.ToString(rows[2][10]) != @Global.Policy)
                    {
                        validateHeader = false;
                    }
                    // Id. Póliza Madre
                    if (Convert.ToString(rows[2][11]) != @Global.MotherPolicyId)
                    {
                        validateHeader = false;
                    }
                    // Id. Asegurado
                    if (Convert.ToString(rows[2][12]) != @Global.InsuredId)
                    {
                        validateHeader = false;
                    }
                    // Asegurado 
                    if (Convert.ToString(rows[2][13]) != @Global.Insured)
                    {
                        validateHeader = false;
                    }
                    // Id. Tipo Intermediario
                    if (Convert.ToString(rows[2][14]) != @Global.IntermediaryTypeId)
                    {
                        validateHeader = false;
                    }
                    // Id. Intermediario
                    if (Convert.ToString(rows[2][15]) != @Global.IntermediaryId)
                    {
                        validateHeader = false;
                    }
                    // Intermediario
                    if (Convert.ToString(rows[2][16]) != @Global.Intermediary)
                    {
                        validateHeader = false;
                    }
                    // Fecha Emisión
                    if (Convert.ToString(rows[2][17]) != @Global.IssueDate)
                    {
                        validateHeader = false;
                    }
                    // Fecha Emisión Desde
                    if (Convert.ToString(rows[2][18]) != @Global.IssueDateFrom)
                    {
                        validateHeader = false;
                    }
                    // Fecha Emisión Hasta
                    if (Convert.ToString(rows[2][19]) != @Global.IssueDateTo)
                    {
                        validateHeader = false;
                    }
                    // Id. Moneda
                    if (Convert.ToString(rows[2][20]) != @Global.CurrencyId)
                    {
                        validateHeader = false;
                    }
                    // Moneda
                    if (Convert.ToString(rows[2][21]) != @Global.Currency)
                    {
                        validateHeader = false;
                    }
                    // Saldo Cancelación
                    if (Convert.ToString(rows[2][22]) != @Global.CancellationBalance)
                    {
                        validateHeader = false;
                    }
                    // Id. Temporal Aplicación
                    if (Convert.ToString(rows[2][23]) != @Global.ApplicationTemporaryId)
                    {
                        validateHeader = false;
                    }

                    if (validateHeader)
                    {
                        /*
                        CancellationProcess cancellationProcess = new CancellationProcess();
                        List<CancellationPolicy> policies = new List<CancellationPolicy>();

                        for (int index = 3; index < rows.Count; index++)
                        {
                            if (Convert.ToString(rows[index][0]) == "")
                            {
                                break; // Indica que no existe nro de registro en el archivo, se lo usa para fin de lectura de movimientos
                            }
                            else
                            {
                                var id = Convert.ToInt32(rows[index][0]);
                                var currencyId = Convert.ToInt32(rows[index][20]);
                                var branchId = Convert.ToInt32(rows[index][3]);
                                var salePointId = Convert.ToInt32(rows[index][5]);
                                var prefixId = Convert.ToInt32(rows[index][7]);
                                var policyNumber = Convert.ToInt32(rows[index][10]);

                                validateHeader = ValidatePolicyRowByProcessNumber(processNumber, id, branchId, salePointId, prefixId, policyNumber, "P");

                                if (validateHeader)
                                {
                                    CancellationPolicy cancellationPolicy = new CancellationPolicy();
                                    cancellationPolicy.Cancelled = false;
                                    cancellationPolicy.Id = id;
                                    Policy policy = new Policy();
                                    policy.Branch = new Branch() { Id = branchId };            // Id. Sucursal
                                    policy.Currency = new Currency() { Id = currencyId };      // Id. Moneda
                                    policy.DocumentNumber = policyNumber;                      // Nro. Póliza
                                    policy.Id = Convert.ToInt32(rows[index][9]);               // Id. Póliza
                                    policy.InitialPolicyId = Convert.ToInt32(rows[index][11]); // Id. Póliza Madre
                                    policy.Prefix = new Prefix() { Id = prefixId };            // Id. Ramo
                                    policy.Product = new Product()
                                    {
                                        Id = Convert.ToString(rows[index][1]) == "Si" ? 1 : 0  // Aplica Cobro
                                    };

                                    policy.SalePoint = new SalePoint() { Id = salePointId };   // Id. Pto. Venta

                                    cancellationPolicy.Policy = policy;

                                    policies.Add(cancellationPolicy);
                                }
                            }
                        }

                        cancellationProcess.CancellationPolicies = policies;
                        cancellationProcess.Date = Convert.ToDateTime(processDate);
                        cancellationProcess.Id = processNumber;
                        cancellationProcess.NumberPolicies = 2;
                        cancellationProcess.User = new individuals.User()
                        {
                            Id = SessionHelper.GetUserId(),
                            Nick = SessionHelper.GetUserName()
                        };

                        updateCancellationProcess = cancellationPolicyService.SaveCancellationProcess(cancellationProcess);
                        */
                        successful = true;
                    }
                    else
                    {
                        message = "NoCorrespondColumns";
                        successful = false;
                    }

                }
                else
                {
                    successful = false;
                }
            }
            catch (FormatException)
            {
                message = "FormatException";
                successful = false;
            }
            catch (OverflowException)
            {
                message = "OverflowException";
                successful = false;
            }
            catch (IndexOutOfRangeException)
            {
                message = "IndexOutOfRangeException";
                successful = false;
            }
            catch (InvalidCastException)
            {
                message = "InvalidCastException";
                successful = false;
            }
            catch (Exception)
            {
                message = "Exception";
                successful = false;
            }

            stream.Close();
            //stream.Dispose();
            buffer = null;
            arrayContent = null;

            object[] jsonData = new object[2];

            jsonData[0] = message;
            jsonData[1] = successful;

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ReadMistakeExcelFileToStream
        /// Lee un documento excel
        /// </summary>
        /// <param name="uploadedFile"></param>
        /// <param name="processNumber"></param>
        /// <param name="processDate"></param>
        /// <returns>JsonResult</returns>
        public JsonResult ReadMistakeExcelFileToStream(HttpPostedFileBase uploadedFile, int processNumber, string processDate)
        {
            bool successful = true;
            bool validateHeader = false;
            string fileLocationName = "";
            string message = "";
            Byte[] arrayContent;
            
            fileLocationName = uploadedFile.FileName;
            string[] data = fileLocationName.Split(new char[] { '.' });

            // Convertir a Bytes
            var buffer = new byte[uploadedFile.InputStream.Length];
            uploadedFile.InputStream.Read(buffer, 0, (int)uploadedFile.InputStream.Length);

            // Lee el archivo y guarda en arreglo de tipo byte y este a su vez a arrayContent
            arrayContent = buffer;

            Stream stream = new MemoryStream(arrayContent);
            IExcelDataReader excelReader;

            try
            {
                if (data[1] == "xls")
                {
                    // 1. Lee desde binary Excel  ('97-2003 format; *.xls)
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else
                {
                    // 2. Lee desde binary OpenXml Excel file (2007 format; *.xlsx)
                    excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                // 3. DataSet - El resultado sera creado en result.Tables
                DataSet dataSet = excelReader.AsDataSet();

                int recordNumber = dataSet.Tables[0].Rows.Count;
                //CancellationProcess updateCancellationProcess = new CancellationProcess();

                // Se lee la cabecera  
                if (recordNumber > 0)
                {
                    if (Convert.ToString(dataSet.Tables[0].Rows[0][1]) != "")
                    {
                        if (Convert.ToInt32(dataSet.Tables[0].Rows[0][1]) == processNumber)
                        {
                            validateHeader = true;
                        }
                        else
                        {
                            message = "NoProcessNumber";
                            validateHeader = false;
                        }
                    }
                    else
                    {
                        validateHeader = false;
                        message = "NoProcessNumber";
                    }
                    if (validateHeader)
                    {
                        if (Convert.ToString(dataSet.Tables[0].Rows[0][4]) != "")
                        {
                            if (Convert.ToString(dataSet.Tables[0].Rows[0][4]) == processDate)
                            {
                                validateHeader = true;
                            }
                            else
                            {
                                validateHeader = false;
                                message = "NoProcessDate";
                            }
                        }
                        else
                        {
                            validateHeader = false;
                            message = "NoProcessDate";
                        }
                    }
                }

                if (validateHeader)
                {
                    // Se lee el detalle
                    var rows = dataSet.Tables[0].Rows;

                    // Se valida la cabecera del detalle (Id)
                    if (Convert.ToString(rows[2][0]) != @Global.Id)
                    {
                        validateHeader = false;
                    }
                    // Reprocesar
                    if (Convert.ToString(rows[2][1]) != @Global.Reprocess)
                    {
                        validateHeader = false;
                    }
                    // Fecha Vencimiento Cuota
                    if (Convert.ToString(rows[2][2]) != @Global.QuoteDueDate)
                    {
                        validateHeader = false;
                    }
                    // Id. Sucursal
                    if (Convert.ToString(rows[2][3]) != @Global.BranchId)
                    {
                        validateHeader = false;
                    }
                    // Sucursal
                    if (Convert.ToString(rows[2][4]) != @Global.Branch)
                    {
                        validateHeader = false;
                    }
                    // Id. Punto Venta
                    if (Convert.ToString(rows[2][5]) != @Global.SalePointId)
                    {
                        validateHeader = false;
                    }
                    // Punto de Venta
                    if (Convert.ToString(rows[2][6]) != @Global.SalePoint)
                    {
                        validateHeader = false;
                    }
                    // Id. Ramo
                    if (Convert.ToString(rows[2][7]) != @Global.PrefixId)
                    {
                        validateHeader = false;
                    }
                    // Ramo
                    if (Convert.ToString(rows[2][8]) != @Global.Prefix)
                    {
                        validateHeader = false;
                    }
                    // Id. Póliza
                    if (Convert.ToString(rows[2][9]) != @Global.PolicyId)
                    {
                        validateHeader = false;
                    }
                    // Póliza
                    if (Convert.ToString(rows[2][10]) != @Global.Policy)
                    {
                        validateHeader = false;
                    }
                    // Id. Póliza Madre
                    if (Convert.ToString(rows[2][11]) != @Global.MotherPolicyId)
                    {
                        validateHeader = false;
                    }
                    // Id. Asegurado
                    if (Convert.ToString(rows[2][12]) != @Global.InsuredId)
                    {
                        validateHeader = false;
                    }
                    // Asegurado 
                    if (Convert.ToString(rows[2][13]) != @Global.Insured)
                    {
                        validateHeader = false;
                    }
                    // Id. Tipo Intermediario
                    if (Convert.ToString(rows[2][14]) != @Global.IntermediaryTypeId)
                    {
                        validateHeader = false;
                    }
                    // Id. Intermediario
                    if (Convert.ToString(rows[2][15]) != @Global.IntermediaryId)
                    {
                        validateHeader = false;
                    }
                    // Intermediario
                    if (Convert.ToString(rows[2][16]) != @Global.Intermediary)
                    {
                        validateHeader = false;
                    }
                    // Fecha Emisión
                    if (Convert.ToString(rows[2][17]) != @Global.IssueDate)
                    {
                        validateHeader = false;
                    }
                    // Fecha Emisión Desde
                    if (Convert.ToString(rows[2][18]) != @Global.IssueDateFrom)
                    {
                        validateHeader = false;
                    }
                    // Fecha Emisión Hasta
                    if (Convert.ToString(rows[2][19]) != @Global.IssueDateTo)
                    {
                        validateHeader = false;
                    }
                    // Id. Moneda
                    if (Convert.ToString(rows[2][20]) != @Global.CurrencyId)
                    {
                        validateHeader = false;
                    }
                    // Moneda
                    if (Convert.ToString(rows[2][21]) != @Global.Currency)
                    {
                        validateHeader = false;
                    }
                    // Prima Total
                    if (Convert.ToString(rows[2][22]) != @Global.TotalPremium)
                    {
                        validateHeader = false;
                    }
                    // Descripción
                    if (Convert.ToString(rows[2][23]) != @Global.Description)
                    {
                        validateHeader = false;
                    }

                    if (validateHeader)
                    {
                        /*
                        CancellationProcess cancellationProcess = new CancellationProcess();
                        List<CancellationPolicy> policies = new List<CancellationPolicy>();

                        for (int index = 3; index < rows.Count; index++)
                        {
                            if (Convert.ToString(rows[index][0]) == "")
                            {
                                break; // Indica que no existe nro de registro en el archivo, se lo usa para fin de lectura de movimientos
                            }
                            else
                            {
                                var id = Convert.ToInt32(rows[index][0]);
                                var currencyId = Convert.ToInt32(rows[index][20]);
                                var branchId = Convert.ToInt32(rows[index][3]);
                                var salePointId = Convert.ToInt32(rows[index][5]);
                                var prefixId = Convert.ToInt32(rows[index][7]);
                                var policyNumber = Convert.ToInt32(rows[index][10]);

                                validateHeader = ValidatePolicyRowByProcessNumber(processNumber, id, branchId, salePointId, prefixId, policyNumber, "R");

                                if (validateHeader)
                                {
                                    CancellationPolicy cancellationPolicy = new CancellationPolicy();
                                    cancellationPolicy.Cancelled = false;
                                    cancellationPolicy.Id = id;
                                    Policy policy = new Policy();
                                    policy.Branch = new Branch() { Id = branchId };            // Id. Sucursal
                                    policy.Currency = new Currency() { Id = currencyId };      // Id. Moneda
                                    policy.DocumentNumber = policyNumber;                      // Nro. Póliza
                                    policy.Id = Convert.ToInt32(rows[index][9]);               // Id. Póliza
                                    policy.InitialPolicyId = Convert.ToInt32(rows[index][11]); // Id. Póliza Madre
                                    policy.Prefix = new Prefix() { Id = prefixId };            // Id. Ramo
                                    policy.Product = new Product()
                                    {
                                        Id = Convert.ToString(rows[index][1]) == "Si" ? 1 : 0  // Reprocesar
                                    };

                                    policy.SalePoint = new SalePoint() { Id = salePointId };   // Id. Pto. Venta

                                    cancellationPolicy.Policy = policy;

                                    policies.Add(cancellationPolicy);
                                }
                            }
                        }

                        cancellationProcess.CancellationPolicies = policies;
                        cancellationProcess.Date = Convert.ToDateTime(processDate);
                        cancellationProcess.Id = processNumber;
                        cancellationProcess.NumberPolicies = 3;
                        cancellationProcess.User = new individuals.User()
                        {
                            Id = SessionHelper.GetUserId(),
                            Nick = SessionHelper.GetUserName()
                        };

                        updateCancellationProcess = cancellationPolicyService.SaveCancellationProcess(cancellationProcess);
                        */
                        successful = true;
                    }
                    else
                    {
                        message = "NoCorrespondColumns";
                        successful = false;
                    }

                }
                else
                {
                    successful = false;
                }
            }
            catch (FormatException)
            {
                message = "FormatException";
                successful = false;
            }
            catch (OverflowException)
            {
                message = "OverflowException";
                successful = false;
            }
            catch (IndexOutOfRangeException)
            {
                message = "IndexOutOfRangeException";
                successful = false;
            }
            catch (InvalidCastException)
            {
                message = "InvalidCastException";
                successful = false;
            }
            catch (Exception)
            {
                message = "Exception";
                successful = false;
            }

            stream.Close();
            //stream.Dispose();
            buffer = null;
            arrayContent = null;

            object[] jsonData = new object[2];

            jsonData[0] = message;
            jsonData[1] = successful;

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}