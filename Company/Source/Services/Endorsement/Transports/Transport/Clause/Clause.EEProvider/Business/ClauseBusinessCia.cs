using Sistran.Company.Application.UnderwritingServices.Models;
using System;


namespace Sistran.Company.Application.TransportClauseService.EEProvider.Business
{
    using Sistran.Company.Application.BaseEndorsementService.EEProvider.Business;
    using Sistran.Company.Application.TransportClauseService.EEProvider.Resources;
    using Sistran.Company.Application.Transports.TransportBusinessService.Models.Base;
    using Sistran.Company.Application.TransportClauseService.EEProvider.Services;
    using CTSE = Sistran.Company.Application.ClauseEndorsement.EEProvider;
    using CTBEE = Sistran.Company.Application.Transports.TransportBusinessService.EEProvider;
    using System.Collections.Generic;
    using System.Linq;
    using Sistran.Core.Application.UnderwritingServices.Enums;
    using Sistran.Core.Framework.BAF;

    class ClauseBusinessCia
    {

        BaseBusinessCia provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransportEndorsementBusinessCia" /> class.
        /// </summary>
        public ClauseBusinessCia()
        {
            provider = new BaseBusinessCia();
        }

        /// <summary>
        /// Creates the endorsement.
        /// </summary>
        /// <param name="companyEndorsement">The company endorsement.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Endoso Vacio</exception>
        //public CompanyPolicy CreateEndorsementClause(CompanyPolicy companyEndorsement)
        //{
        //    if (companyEndorsement == null)
        //    {
        //        throw new ArgumentException(Errors.ErrorEndorsement);
        //    }
        //    try
        //    {
        //        CompanyPolicy policy = new CompanyPolicy();
        //        CTSE.CiaClauseEndorsementEEProvider CiaClauseEndorsementEEProvider = new CTSE.CiaClauseEndorsementEEProvider();
        //        policy = CiaClauseEndorsementEEProvider.CreateCiaClause(companyEndorsement);
        //        if (policy != null)
        //        {
        //            List<CompanyTransport> companyTransports = null;
        //            companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyEndorsement.Endorsement.PolicyId);
        //            if (companyTransports != null && companyTransports.Count > 0)
        //            {
        //                List<CompanyRisk> risks = new List<CompanyRisk>();
        //                companyTransports.AsParallel().ForAll(item =>
        //                {
        //                    item.Risk.Policy = policy;
        //                    item.Risk.Status = RiskStatusType.Modified;
        //                    item = DelegateService.transportModificationService.GetDataModification(item, CoverageStatusType.Modified);
        //                    var transport = DelegateService.transportService.CreateCompanyTransportTemporal(item);
        //                });
        //                risks = companyTransports.Select(x => x.Risk).ToList();
        //                policy = provider.CalculatePolicyAmounts(policy, risks);
        //                policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
        //                //List<CompanyTransport> transports = DelegateService.transportService.CreateCompanyTransportTemporal(policy.Id);
        //                List<CompanyTransport> transports = DelegateService.transportService.GetCompanyTransportsByTemporalId(policy.Id);

        //                var message = DelegateService.baseEndorsementService.ValidateEndorsement(policy.Id);
        //                if (message != string.Empty)
        //                {
        //                    throw new Exception(message);
        //                }
        //                CTBEE.CompanyTransportBusinessServiceProvider companyTransportBusinessServiceProvider = new CTBEE.CompanyTransportBusinessServiceProvider();
        //                //TransportServiceEEProvider transportServiceEEProvider = new TransportServiceEEProvider();
        //                var companyPolicy = companyTransportBusinessServiceProvider.CreateEndorsement(policy, transports);
        //                CompanyPolicy newCompanyPolicy = new CompanyPolicy();
        //                newCompanyPolicy.Id = companyPolicy.Id;
        //                newCompanyPolicy.DocumentNumber = companyPolicy.DocumentNumber;
        //                newCompanyPolicy.Endorsement = companyPolicy.Endorsement;
        //                newCompanyPolicy.InfringementPolicies = companyPolicy.InfringementPolicies;
        //                return newCompanyPolicy;
        //            }
        //            else
        //            {
        //                throw new Exception(Errors.ErrorTransportNotFound);
        //            }
        //        }
        //        else
        //        {
        //            throw new Exception("Poliza no encontrada.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}
        public CompanyPolicyResult CreateEndorsementClause(CompanyPolicy companyEndorsement)
        {
            if (companyEndorsement == null)
            {
                throw new ArgumentException(Errors.ErrorEndorsement);
            }
            try
            {
                CompanyPolicy policy = new CompanyPolicy();
                CTSE.CiaClauseEndorsementEEProvider CiaClauseEndorsementEEProvider = new CTSE.CiaClauseEndorsementEEProvider();
                policy = CiaClauseEndorsementEEProvider.CreateCiaClause(companyEndorsement);
                policy.IssueDate = companyEndorsement.IssueDate;
                if (policy != null)
                {
                    List<CompanyTransport> companyTransports = null;
                    if (companyEndorsement.Endorsement.TemporalId == 0)
                    {
                        companyTransports = DelegateService.transportService.GetCompanyTransportsByPolicyId(companyEndorsement.Endorsement.PolicyId);
                    }
                    else
                    {
                        companyTransports = DelegateService.transportService.GetCompanyTransportsByTemporalId(policy.Id);
                        companyTransports.AsParallel().ForAll(
                      x =>
                      {
                          x.Risk.OriginalStatus = x.Risk.Status;
                          x.Risk.Status = RiskStatusType.NotModified;
                      });

                    }

                    if (companyTransports != null && companyTransports.Count > 0)
                    {
                        List<CompanyRisk> risks = new List<CompanyRisk>();
                        companyTransports.AsParallel().ForAll(item =>
                        {
                            item.Risk.Policy = policy;
                            item.Risk.Status = RiskStatusType.Modified;
                            item = DelegateService.transportModificationService.GetDataModification(item, CoverageStatusType.Modified);
                            var vehicle = DelegateService.transportService.CreateCompanyTransportTemporal(item);
                        });
                        risks = companyTransports.Select(x => x.Risk).ToList();
                        policy = provider.CalculatePolicyAmounts(policy, risks);
                        policy = DelegateService.underwritingService.CreatePolicyTemporal(policy, false);
                        policy.IssueDate = companyEndorsement.IssueDate;
                        List<CompanyTransport> vehicles = DelegateService.transportService.GetCompanyTransportsByTemporalId(policy.Id);

                        var message = DelegateService.baseEndorsementService.ValidateEndorsement(policy.Id);
                        if (message != string.Empty)
                        {
                            throw new Exception(message);
                        }
                        CTBEE.CompanyTransportBusinessServiceProvider transportBusinessServiceProvider = new CTBEE.CompanyTransportBusinessServiceProvider();
                        return transportBusinessServiceProvider.CreateCompanyPolicy(policy.Id, (int)policy.TemporalType, false);
                    }
                    else
                    {
                        throw new Exception(Errors.ErrorTransportNotFound);
                    }
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
    }
}
