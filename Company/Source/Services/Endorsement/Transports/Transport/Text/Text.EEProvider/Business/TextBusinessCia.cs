using Sistran.Company.Application.UnderwritingServices;
using Sistran.Company.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.TransportTextService.EEProvider.Services;

namespace Sistran.Company.Application.TransportTextService.EEProvider.Business
{
    using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
    using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
    using Sistran.Company.Application.TransportTextService.EEProvider.Resources;
    using Sistran.Core.Application.UnderwritingServices.Enums;
    using System.Collections.Concurrent;
    using CTSE = Sistran.Company.Application.TextEndorsement.EEProvider;
    using CTSP = Sistran.Company.Application.Transports.TransportBusinessService;
    /// <summary>
    /// Endosos Autos
    /// </summary>
    public class TransportTextBusinessCia
    {
        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransportEndorsementBusinessCia" /> class.
        /// </summary>
        public TransportTextBusinessCia()
        {
            provider = new BaseBusinessCia();
        }

        /// <summary>
        /// Creates the endorsement.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Endoso Vacio</exception>
        //public CompanyPolicy CreateEndorsementText(CompanyEndorsement companyEndorsement)
        //{
        //    if (companyEndorsement == null)
        //    {
        //        throw new ArgumentException(Errors.ErrorEndorsement);
        //    }
        //    try
        //    {
        //        CompanyPolicy policy = new CompanyPolicy();
        //        CTSE.CiaTextEndorsementEEProvider CiaTextEndorsementEEProvider = new CTSE.CiaTextEndorsementEEProvider();
        //        policy = CiaTextEndorsementEEProvider.CreateCiaTexts(companyEndorsement);
        //        List<CompanyTransport> companyTransports = null;
        //        companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyEndorsement.PolicyId);
        //        if (companyTransports != null && companyTransports.Count > 0)
        //        {
        //            List<CompanyRisk> risks = new List<CompanyRisk>();
        //            companyTransports.AsParallel().ForAll(item =>
        //            {
        //                item.Risk.Policy = policy;
        //                item.Risk.Status = RiskStatusType.Modified;
        //                item = DelegateService.transportModificationService.GetDataModification(item, CoverageStatusType.Modified);
        //                var transport = DelegateService.transportService.CreateCompanyTransportTemporal(item);
        //            });
        //            risks = companyTransports.Select(x => x.Risk).ToList();
        //            policy = provider.CalculatePolicyAmounts(policy, risks);
        //            policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
        //            List<CompanyTransport> transports = DelegateService.transportService.GetCompanyTransportsByTemporalId(policy.Id);

        //            var message = DelegateService.baseEndorsementService.ValidateEndorsement(policy.Id);
        //            if (message != string.Empty)
        //            {
        //                throw new Exception(message);
        //            }
        //            /// CVSE.TransportServiceEEProvider transportServiceEEProvider = new CVSE.TransportServiceEEProvider();
        //            return DelegateService.transportService.CreateEndorsement(policy, transports);

        //        }
        //        else
        //        {
        //            throw new Exception(Errors.ErrorTransportNotFound);
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public CompanyPolicyResult CreateEndorsementText(CompanyEndorsement companyEndorsement)
        {
            if (companyEndorsement == null)
            {
                throw new ArgumentException(Errors.ErrorEndorsement);
            }
            try
            {
                CompanyPolicy policy = new CompanyPolicy();
                CTSE.CiaTextEndorsementEEProvider CiaTextEndorsementEEProvider = new CTSE.CiaTextEndorsementEEProvider();
                policy = CiaTextEndorsementEEProvider.CreateCiaTexts(companyEndorsement);
                List<CompanyTransport> companyTransport = null;
                if (companyEndorsement.TemporalId != 0)
                {
                    companyTransport = DelegateService.transportService.GetCompanyTransportsByTemporalId(companyEndorsement.TemporalId);
                    if (companyTransport == null || companyTransport.Count < 1)
                    {
                        companyTransport = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyEndorsement.PolicyId);
                        companyTransport.AsParallel().ForAll(
                           x =>
                           {
                               x.Risk.Id = 0;
                               x.Risk.OriginalStatus = x.Risk.Status;
                               x.Risk.Status = RiskStatusType.NotModified;
                           });
                    }
                }
                else
                {
                    companyTransport = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyEndorsement.PolicyId);
                    companyTransport.AsParallel().ForAll(
                       x =>
                       {
                           x.Risk.Id = 0;
                           x.Risk.OriginalStatus = x.Risk.Status;
                           x.Risk.Status = RiskStatusType.NotModified;
                       });
                }
                if (companyTransport != null && companyTransport.Count > 0)
                {
                    List<CompanyRisk> risks = new List<CompanyRisk>();
                    ConcurrentBag<string> errorsData = new ConcurrentBag<string>();
                    companyTransport.AsParallel().ForAll(item =>
                    {
                        try
                        {
                            item.Risk.Policy = policy;
                            item.Risk.Status = RiskStatusType.Modified;
                            item = DelegateService.transportModificationService.GetDataModification(item, CoverageStatusType.Modified);
                            var transport = DelegateService.transportService.CreateCompanyTransportTemporal(item);
                        }
                        catch (Exception)
                        {
                            errorsData.Add(Errors.ErrorCreateTransport);
                        }

                    });
                    risks = companyTransport.Select(x => x.Risk).ToList();
                    policy = provider.CalculatePolicyAmounts(policy, risks);
                    policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                    policy.IssueDate = companyEndorsement.IssueDate;
                    var message = DelegateService.baseEndorsementService.ValidateEndorsement(policy.Id);
                    if (message != string.Empty)
                    {
                        throw new Exception(message);
                    }
                    return DelegateService.transportService.CreateCompanyPolicy(policy.Id, (int)policy.TemporalType, false);
                }
                else
                {
                    throw new Exception(Errors.ErrorTemporalNotFound);
                }



            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
