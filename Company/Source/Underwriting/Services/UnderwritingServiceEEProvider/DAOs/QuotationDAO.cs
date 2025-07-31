using Newtonsoft.Json;
using Sistran.Company.Application.Common.Entities;
using Sistran.Company.Application.CommonServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Temporary.Entities;
using Sistran.Core.Application.UnderwritingServices.Enums;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using ISSEN = Sistran.Core.Application.Issuance.Entities;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.DAO
{
    public class QuotationDAO
    {
        /// <summary>
        /// Obtener Cotización
        /// </summary>
        /// <param name="quotationId">Id Cotización</param>
        /// <param name="version">Versión</param>
        /// <returns>Cotización</returns>
        public List<CompanyPolicy> GetPoliciesByQuotationIdVersionPrefixId(int operationId, int version, int prefixId, int branchId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<CompanyPolicy> policies = new List<CompanyPolicy>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(TempSubscription.Properties.OperationId, typeof(TempSubscription).Name);
            filter.Equal();
            filter.Constant(operationId);
            filter.And();
            filter.Property(TempSubscription.Properties.TemporalTypeCode, typeof(TempSubscription).Name);
            filter.Equal();
            filter.Constant(TemporalType.TempQuotation);

            if (version > 0)
            {
                filter.And();
                filter.Property(TempSubscription.Properties.QuotationVersion, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(version);
                filter.And();
                filter.Property(TempSubscription.Properties.PrefixCode, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(prefixId);
                filter.And();
                filter.Property(TempSubscription.Properties.BranchCode, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(branchId);
            }

            BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(TempSubscription), filter.GetPredicate());

            if (businessCollection.Count == 1)
            {
                int operationId2 = ((TempSubscription)businessCollection[0]).OperationId.GetValueOrDefault();
                int temporalId = ((TempSubscription)businessCollection[0]).TempId;

                PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(operationId);

                if (pendingOperation != null)
                {
                    CompanyPolicy policy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                    policy.Id = operationId2;
                    policy.Endorsement.TemporalId = temporalId;
                    pendingOperation.Operation = JsonConvert.SerializeObject(policy);
                    pendingOperation = DelegateService.utilitiesServiceCore.UpdatePendingOperation(pendingOperation);

                    policies.Add(policy);
                }
            }
            else if (businessCollection.Count > 1)
            {
                policies = businessCollectionPolicies(businessCollection);
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.QuotationServices.EEProvider.DAOs.GetPoliciesByQuotationIdVersionPrefixId");
            return policies;
        }

        public List<CompanyPolicy> businessCollectionPolicies(BusinessCollection businessCollection)
        {
            List<CompanyPolicy> policies = new List<CompanyPolicy>();
            int cont = 0;
            foreach (TempSubscription tempSubscription in businessCollection)
            {
                cont++;
                if (cont <= 20)
                {
                    int operationId = tempSubscription.OperationId.GetValueOrDefault();
                    int quotationId = tempSubscription.QuotationId.GetValueOrDefault();

                    PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(operationId);

                    if (pendingOperation != null && !string.IsNullOrEmpty(pendingOperation.Operation))
                    {
                        CompanyPolicy policyBusiness = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                        policyBusiness.Id = operationId;
                        policyBusiness.Endorsement.QuotationId = quotationId;
                        policyBusiness.Endorsement.TemporalId = tempSubscription.TempId;
                        policyBusiness.PolicyOrigin = pendingOperation.IsMassive ? PolicyOrigin.Massive : PolicyOrigin.Collective;

                        CompanyPolicy policy = GetPolicyByQuotationId(quotationId, policyBusiness.Prefix.Id, policyBusiness.Branch.Id);
                        if (policy != null)
                        {
                            policyBusiness.DocumentNumber = policy.DocumentNumber;
                        }
                        policies.Add(policyBusiness);
                    }
                }
            }
            return policies;
        }

        /// <summary>
        /// Obtener QuotationId por operationId
        /// </summary>
        /// <param name="OperationId">Id Cotización</param>
        /// <returns>Cotización</returns>
        public CompanyPolicy GetPolicyByPendingOperation(int OperationId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            CompanyPolicy policy = new CompanyPolicy();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(TempSubscription.Properties.OperationId, typeof(TempSubscription).Name);
            filter.Equal();
            filter.Constant(OperationId);
            filter.And();
            filter.Property(TempSubscription.Properties.TemporalTypeCode, typeof(TempSubscription).Name);
            filter.Equal();
            filter.Constant(TemporalType.TempQuotation);

            BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(TempSubscription), filter.GetPredicate());

            if (businessCollection.Count == 1)
            {
                int operationId = ((TempSubscription)businessCollection[0]).OperationId.GetValueOrDefault();
                int temporalId = ((TempSubscription)businessCollection[0]).TempId;
                int QuotationId = ((TempSubscription)businessCollection[0]).QuotationId == null ? 0 : (int)((TempSubscription)businessCollection[0]).QuotationId;


                policy.Id = operationId;
                policy.Endorsement = new CompanyEndorsement()
                {
                    QuotationId = QuotationId,
                    TemporalId = temporalId
                };

            }
            else
            {
                ObjectCriteriaBuilder filterII = new ObjectCriteriaBuilder();

                filterII.Property(TempSubscription.Properties.OperationId, typeof(TempSubscription).Name);
                filterII.Equal();
                filterII.Constant(OperationId);

                BusinessCollection businessCollectionII = DataFacadeManager.Instance.GetDataFacade().List(typeof(TempSubscription), filterII.GetPredicate());

                if (businessCollectionII.Count == 1)
                {
                    int TEMP_TYPE_CD = ((TempSubscription)businessCollectionII[0]).TemporalTypeCode ?? 1;

                    policy.TemporalType = (Sistran.Core.Application.UnderwritingServices.Enums.TemporalType)TEMP_TYPE_CD;

                }

            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.QuotationServices.EEProvider.DAOs.GetPoliciesByQuotationIdVersionPrefixId");
            return policy;
        }
        public CompanyPolicy GetPolicyByQuotationId(int quotationId, int prefixCd, int branchCd)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Endorsement.Properties.QuotationId, "e");
            filter.Equal();
            filter.Constant(quotationId);
            filter.And();
            filter.Property(ISSEN.Policy.Properties.PrefixCode, "p");
            filter.Equal();
            filter.Constant(prefixCd);
            filter.And();
            filter.Property(ISSEN.Policy.Properties.BranchCode, "p");
            filter.Equal();
            filter.Constant(branchCd);
            SelectQuery SelectQuery = new SelectQuery();
            #region Select
            SelectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.DocumentNumber, "p"), "DocumentNumber"));
            SelectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.PrefixCode, "p"), "PrefixCode"));
            SelectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.BranchCode, "p"), "BranchCode"));
            #endregion Select
            Join join = new Join(new ClassNameTable(typeof(ISSEN.Policy), "p"), new ClassNameTable(typeof(ISSEN.Endorsement), "e"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.Policy.Properties.PolicyId, "p")
                .Equal()
                .Property(ISSEN.Endorsement.Properties.PolicyId, "e")
                .GetPredicate());
            SelectQuery.Table = join;
            SelectQuery.Where = filter.GetPredicate();
            SelectQuery.GetFirstSelect();
            CompanyPolicy policy = null;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(SelectQuery))
            {
                while (reader.Read())
                {
                    policy = new CompanyPolicy();
                    policy.DocumentNumber = (decimal)reader["DocumentNumber"];
                    policy.Branch = new CommonServices.Models.CompanyBranch { Id = (int)reader["BranchCode"] };
                    policy.Prefix = new CommonServices.Models.CompanyPrefix { Id = (int)reader["PrefixCode"] };
                    break;
                }
            }
            return policy;
        }

        /// <summary>
        /// Crear Nueva Versión De Una Cotización
        /// </summary>
        /// <param name="quotationId">Id Cotización</param>
        /// <returns>Cotización</returns>
        public CompanyPolicy CreateNewVersionQuotation(int operationId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(operationId);

            if (pendingOperation != null)
            {
                CompanyPolicy policy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(TempSubscription.Properties.QuotationVersion, typeof(TempSubscription).Name);
                filter.IsNotNull();
                filter.And();
                filter.Property(TempSubscription.Properties.QuotationId, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(policy.Endorsement.QuotationId);
                filter.And();
                filter.Property(TempSubscription.Properties.TemporalTypeCode, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(TemporalType.Quotation);
                filter.And();
                filter.Property(TempSubscription.Properties.PrefixCode, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(policy.Prefix.Id);
                filter.And();
                filter.Property(TempSubscription.Properties.BranchCode, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(policy.Branch.Id);

                BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(TempSubscription), filter.GetPredicate());

                policy.Endorsement.TemporalId = 0;
                policy.Endorsement.QuotationVersion = 0;
                policy.TemporalType = TemporalType.TempQuotation;
                pendingOperation.Id = 0;
                pendingOperation.Operation = JsonConvert.SerializeObject(policy);
                pendingOperation = DelegateService.utilitiesServiceCore.CreatePendingOperation(pendingOperation);
                policy.Id = pendingOperation.Id;

                List<PendingOperation> pendingOperations = DelegateService.utilitiesServiceCore.GetPendingOperationsByParentId(operationId);

                foreach (PendingOperation item in pendingOperations)
                {
                    item.Id = 0;
                    item.ParentId = policy.Id;
                    DelegateService.utilitiesServiceCore.CreatePendingOperation(item);
                }
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.QuotationServices.EEProvider.DAOs.CreateNewVersionQuotation");
                return policy;
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.QuotationServices.EEProvider.DAOs.CreateNewVersionQuotation");
                return null;
            }
        }

        /// <summary>
        /// Obtener la fecha de cotización.
        /// </summary>
        /// <param name="moduleCode"></param>
        /// <param name="issueDate"></param>
        /// <returns>fecha de cotización</returns>
        public DateTime GetQuotationDate(int moduleCode, DateTime issueDate)
        {
            int month;
            int year;
            int day;
            int hour;
            int minute;
            DateTime closedDate;
            int diat;
            int mest;
            DateTime dateMonth;
            DateTime dateDeviationMinor;

            PrimaryKey key = CoModuleDate.CreatePrimaryKey(moduleCode);
            CoModuleDate coModuleDate = (CoModuleDate)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            if (coModuleDate == null)
            {
                return issueDate;
            }

            month = Convert.ToInt32(coModuleDate.CeilingMm);
            year = Convert.ToInt32(coModuleDate.CeilingYyyy);
            day = DateTime.DaysInMonth(DateTime.Today.Year, Convert.ToInt32(coModuleDate.CeilingMm));
            hour = issueDate.Hour;
            minute = issueDate.Minute;

            closedDate = new DateTime(year, month, day);
            dateMonth = closedDate.AddMonths(1);
            mest = dateMonth.Month;
            diat = DateTime.DaysInMonth(DateTime.Today.Year, mest);
            dateMonth = new DateTime(dateMonth.Year, dateMonth.Month, diat, hour, minute, 0);

            dateDeviationMinor = issueDate;

            if (dateDeviationMinor.CompareTo(closedDate) < 0)
            {
                dateDeviationMinor = closedDate;
            }

            if (issueDate.CompareTo(dateDeviationMinor) < 0)
            {
                issueDate = dateDeviationMinor;
            }

            if (issueDate.CompareTo(dateMonth) > 0)
            {
                issueDate = dateMonth;
            }

            if (issueDate.CompareTo(BusinessServices.GetDate()) > 0)
            {
                issueDate = BusinessServices.GetDate();
            }

            if (issueDate.CompareTo(closedDate) <= 0)
            {
                issueDate = new DateTime(dateMonth.Year, dateMonth.Month, 1);

                if (issueDate.CompareTo(dateMonth) < 0 && dateMonth.CompareTo(BusinessServices.GetDate()) < 0)
                {
                    issueDate = dateMonth;
                }
            }

            return issueDate;
        }
        #region Busq Avnz
        /// <summary>
        /// Obtener Cotización de busqueda avanzada
        /// </summary>
        /// <param name="policy"></param>
        /// <param name="issueFrom"></param>
        /// <param name="issueTo"></param>
        /// <returns>Cotizaciones</returns>
        public List<CompanyPolicy> GetPoliciesByPolicy(Policy policy)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            List<CompanyPolicy> policies = new List<CompanyPolicy>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            //filter.Property(TempSubscription.Properties.QuotationVersion, typeof(TempSubscription).Name);
            //filter.IsNotNull();
            //filter.And();
            filter.Property(TempSubscription.Properties.TemporalTypeCode, typeof(TempSubscription).Name);
            filter.Equal();
            filter.Constant((int)TemporalType.Quotation);
            if (policy.Endorsement != null && policy.Endorsement.QuotationId > 0)
            {
                filter.And();
                filter.Property(TempSubscription.Properties.QuotationId, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(policy.Endorsement.QuotationId);
            }

            if (policy.Holder != null && policy.Holder.IndividualId > 0)
            {
                filter.And();
                filter.Property(TempSubscription.Properties.PolicyHolderId, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(policy.Holder.IndividualId);
            }

            if (policy.UserId > 0)
            {
                filter.And();
                filter.Property(TempSubscription.Properties.UserId, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(policy.UserId);
            }

            if (policy.CurrentFrom > DateTime.MinValue)
            {
                filter.And();
                filter.Property(TempSubscription.Properties.IssueDate, typeof(TempSubscription).Name);
                filter.GreaterEqual();
                filter.Constant(policy.CurrentFrom);
            }

            if (policy.CurrentTo > DateTime.MinValue)
            {
                policy.CurrentTo = policy.CurrentTo.AddHours(23);
                filter.And();
                filter.Property(TempSubscription.Properties.IssueDate, typeof(TempSubscription).Name);
                filter.LessEqual();
                filter.Constant(policy.CurrentTo);
            }
            //segun proceso de R1 se debe mostrar cotizaciones con fecha de currentfrom no superior a 15 dias 
            DateTime fiteenyDays = DelegateService.commonService.GetModuleDateIssue(3, DateTime.Now);
            fiteenyDays = Convert.ToDateTime(fiteenyDays.ToString("dd-MM-yyyy 23:59:59"));

            filter.And();
            filter.Property(TempSubscription.Properties.IssueDate, typeof(TempSubscription).Name);
            filter.LessEqual();
            filter.Constant(fiteenyDays);

            fiteenyDays = DateTime.Now.AddDays(-Convert.ToDouble(DelegateService.commonService.GetParameterByDescription("QUOTATION_CURRENT_DAYS").NumberParameter));

            filter.And();
            filter.Property(TempSubscription.Properties.IssueDate, typeof(TempSubscription).Name);
            filter.GreaterEqual();
            filter.Constant(fiteenyDays);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(TempSubscription), filter.GetPredicate()));
            if (businessCollection.Count > 0)
            {
                policies = businessCollectionPoliciesc(businessCollection);
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.QuotationServices.EEProvider.DAOs.GetPoliciesByPolicy");

            return policies;
        }

        public List<CompanyPolicy> businessCollectionPoliciesc(BusinessCollection businessCollection)
        {
            List<CompanyPolicy> policies = new List<CompanyPolicy>();
            int cont = 0;
            foreach (TempSubscription tempSubscription in businessCollection)
            {
                cont++;
                if (cont <= 20)
                {
                    int operationId = tempSubscription.OperationId.GetValueOrDefault();
                    int quotationId = tempSubscription.QuotationId.GetValueOrDefault();

                    PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(operationId);

                    if (pendingOperation != null && !string.IsNullOrEmpty(pendingOperation.Operation))
                    {
                        CompanyPolicy policyBusiness = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);
                        policyBusiness.Id = operationId;
                        policyBusiness.Endorsement.QuotationId = quotationId;
                        policyBusiness.Endorsement.TemporalId = tempSubscription.TempId;

                        if (policyBusiness.Summary.RisksInsured != null)
                        {
                            foreach (var item in policyBusiness.Summary.RisksInsured)
                            {
                                foreach (var Individual in item.Beneficiaries)
                                {
                                    if (Individual.IndividualType == 0)
                                    {
                                        Individual.IndividualType = Core.Services.UtilitiesServices.Enums.IndividualType.Person;
                                    }
                                }
                            }
                        }

                        CompanyPolicy policy = GetPolicyByQuotationIdc(quotationId, policyBusiness.Prefix.Id, policyBusiness.Branch.Id);
                        if (policy != null)
                        {
                            policyBusiness.DocumentNumber = policy.DocumentNumber;
                        }


                        policies.Add(policyBusiness);
                    }
                }
            }
            return policies;
        }

        public CompanyPolicy GetPolicyByQuotationIdc(int quotationId, int prefixCd, int branchCd)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.Endorsement.Properties.QuotationId, "e");
            filter.Equal();
            filter.Constant(quotationId);
            filter.And();
            filter.Property(ISSEN.Policy.Properties.PrefixCode, "p");
            filter.Equal();
            filter.Constant(prefixCd);
            filter.And();
            filter.Property(ISSEN.Policy.Properties.BranchCode, "p");
            filter.Equal();
            filter.Constant(branchCd);
            SelectQuery SelectQuery = new SelectQuery();
            #region Select
            SelectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.DocumentNumber, "p"), "DocumentNumber"));
            SelectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.PrefixCode, "p"), "PrefixCode"));
            SelectQuery.AddSelectValue(new SelectValue(new Column(ISSEN.Policy.Properties.BranchCode, "p"), "BranchCode"));
            #endregion Select
            Join join = new Join(new ClassNameTable(typeof(ISSEN.Policy), "p"), new ClassNameTable(typeof(ISSEN.Endorsement), "e"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ISSEN.Policy.Properties.PolicyId, "p")
                .Equal()
                .Property(ISSEN.Endorsement.Properties.PolicyId, "e")
                .GetPredicate());
            SelectQuery.Table = join;
            SelectQuery.Where = filter.GetPredicate();
            SelectQuery.GetFirstSelect();
            CompanyPolicy policy = null;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(SelectQuery))
            {
                while (reader.Read())
                {
                    policy = new CompanyPolicy();
                    policy.DocumentNumber = (decimal)reader["DocumentNumber"];
                    policy.Branch = new CompanyBranch { Id = (int)reader["BranchCode"] };
                    policy.Prefix = new CompanyPrefix { Id = (int)reader["PrefixCode"] };
                    break;
                }
            }
            return policy;
        }
        #endregion

        public int GetVersionQuotation(int operationId, int QuotationId)
        {
            BusinessCollection businessCollection = null;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PendingOperation pendingOperation = DelegateService.utilitiesServiceCore.GetPendingOperationById(operationId);

            if (pendingOperation != null)
            {
                CompanyPolicy policy = JsonConvert.DeserializeObject<CompanyPolicy>(pendingOperation.Operation);

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                filter.Property(TempSubscription.Properties.QuotationVersion, typeof(TempSubscription).Name);
                filter.IsNotNull();
                filter.And();
                filter.Property(TempSubscription.Properties.QuotationId, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(QuotationId);
                filter.And();
                filter.Property(TempSubscription.Properties.TemporalTypeCode, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(TemporalType.Quotation);
                filter.And();
                filter.Property(TempSubscription.Properties.PrefixCode, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(policy.Prefix.Id);
                filter.And();
                filter.Property(TempSubscription.Properties.BranchCode, typeof(TempSubscription).Name);
                filter.Equal();
                filter.Constant(policy.Branch.Id);

                using (IDataFacade daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessCollection = daf.List(typeof(TempSubscription), filter.GetPredicate());
                }

                int QuotationVersion = 0;

                foreach (TempSubscription Temp in businessCollection)
                {
                    if (QuotationVersion < Temp.QuotationVersion.GetValueOrDefault())
                    {
                        QuotationVersion = Temp.QuotationVersion.GetValueOrDefault();
                    }

                }
                QuotationVersion += 1;
                return QuotationVersion;
            }
            else
            {
                return -1;
            }
        }
    }
}
