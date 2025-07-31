using Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using PAYMEN = Sistran.Core.Application.Claims.Entities;
using INTEN = Sistran.Core.Application.Integration.Entities;
using CLMEN = Sistran.Core.Application.Claims.Entities;
using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using PAYMENMOD = Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework;
using Sistran.Core.Application.ClaimServices.EEProvider.Views.PaymentRequest;
using Sistran.Core.Framework.DAF.Engine;
using UTIENUM = Sistran.Core.Services.UtilitiesServices.Enums;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Application.ClaimServices.EEProvider.Enums;
using Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims;
using Sistran.Core.Application.ClaimServices.DTOs.Claims;
using Sistran.Core.Application.ClaimServices.EEProvider.Views.Claims;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Integration.TechnicalTransactionGeneratorServices.DTOs;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.PaymentRequest
{
    public class PaymentRequestDAO
    {
        private readonly int payment;

        public List<ConceptSource> GetPaymentSourcesByChargeRequest(bool isChargeRequest)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (isChargeRequest)
            {
                filter.PropertyEquals(PAYMEN.PaymentConceptSource.Properties.ChargeRequestEnable, typeof(PAYMEN.PaymentConceptSource).Name, isChargeRequest);
            }

            return ModelAssembler.CreatePaymentSourcesByChargeRequests(DataFacadeManager.GetObjects(typeof(PAYMEN.PaymentConceptSource), filter.GetPredicate()));
        }

        public List<Role> GetRoles()
        {
            return ModelAssembler.CreateRoles(DataFacadeManager.GetObjects(typeof(CLMEN.Role)));
        }

        public List<VoucherType> GetVoucherTypes()
        {
            return ModelAssembler.CreateVoucherTypes(DataFacadeManager.GetObjects(typeof(PARAMEN.VoucherType)));
        }

        public List<ClaimPaymentMethod> GetPaymentMethods()
        {
            return ModelAssembler.CreatePaymentMethods(DataFacadeManager.GetObjects(typeof(CLMEN.PaymentMethod)));
        }                

        public List<int> GetEstimationsTypesIdByMovementTypeId(int movementTypeId)
        {
            List<int> estimationsTypesId = new List<int>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.PropertyEquals(PAYMEN.RelationMovementEstimationType.Properties.MovementTypeCode, typeof(PAYMEN.RelationMovementEstimationType).Name, movementTypeId);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(PAYMEN.RelationMovementEstimationType), filter.GetPredicate());

            foreach (PAYMEN.RelationMovementEstimationType entityRelationMovementEstimationType in businessCollection)
            {
                estimationsTypesId.Add(entityRelationMovementEstimationType.EstimateTypeCode);
            }

            return estimationsTypesId;
        }


        public PAYMENMOD.PaymentRequest CreatePaymentRequest(PAYMENMOD.PaymentRequest paymentRequest)
        {
            PAYMEN.PaymentRequest entityPaymentRequest = EntityAssembler.CreatePaymentRequest(paymentRequest);
            DataFacadeManager.Insert(entityPaymentRequest);

            paymentRequest.Id = entityPaymentRequest.PaymentRequestCode;            

            return ModelAssembler.CreatePaymentRequest(entityPaymentRequest);
        }

        public void CreatePaymentRequestControl(Models.PaymentRequest.PaymentRequest paymentRequest)
        {
            #region PaymentRequestControl

            PaymentRequestControl paymentRequestControl = ModelAssembler.CreatePaymentRequestControl(paymentRequest);

            CreatePaymentRequestControl(paymentRequestControl);

            #endregion
        }

        public int GetPaymentRequestNumerByBranchId(int branchId)
        {
            try
            {
                if (Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_DEFAULT_PAYMENT_BRANCH)) > 0)
                {
                    ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(PAYMEN.PaymentRequestNumber.Properties.BranchCode, typeof(PAYMEN.PaymentRequestNumber).Name, Convert.ToInt32(EnumHelper.GetEnumParameterValue<ClaimsKeys>(ClaimsKeys.CLM_DEFAULT_PAYMENT_BRANCH)));

                    BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(PAYMEN.PaymentRequestNumber), filter.GetPredicate());
                    if (businessCollection.Count > 0)
                    {
                        PAYMEN.PaymentRequestNumber entityPaymentRequestNumber = (PAYMEN.PaymentRequestNumber)businessCollection.First();
                        entityPaymentRequestNumber.Number++;
                        DataFacadeManager.Update(entityPaymentRequestNumber);
                        return Convert.ToInt32(entityPaymentRequestNumber.Number);
                    }
                    else
                    {
                        throw new BusinessException(Resources.Resources.ErrorPaymentRequestNumberNotParameterized);
                    }
                }
                else
                {
                    throw new BusinessException(Resources.Resources.ErrorPaymentRequestNumberNotParameterized);
                }
            }
            catch (Exception ex)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(PAYMEN.PaymentRequestNumber.Properties.BranchCode, typeof(PAYMEN.PaymentRequestNumber).Name, branchId);

                BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(PAYMEN.PaymentRequestNumber), filter.GetPredicate());
                if (businessCollection.Count > 0)
                {
                    PAYMEN.PaymentRequestNumber entityPaymentRequestNumber = (PAYMEN.PaymentRequestNumber)businessCollection.First();
                    entityPaymentRequestNumber.Number++;
                    DataFacadeManager.Update(entityPaymentRequestNumber);
                    return Convert.ToInt32(entityPaymentRequestNumber.Number);
                }
                else
                {
                    throw new BusinessException(Resources.Resources.ErrorPaymentRequestNumberNotParameterized);
                }
            }
        }

        public PAYMENMOD.PaymentRequest SaveRequestCancellation(int paymentRequestId, bool IsChargeRequest)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            DateTime accountingDate = DelegateService.claimApplicationService.GetModuleDateByModuleTypeMovementDate(UTIENUM.ModuleType.Claim, DateTime.Today);

            //Se guarda la solicitud de pago
            PrimaryKey primaryKey = PAYMEN.PaymentRequest.CreatePrimaryKey(paymentRequestId);
            PAYMEN.PaymentRequest entityPaymentRequest = EntityAssembler.CreatePaymentRequest((PAYMEN.PaymentRequest)DataFacadeManager.GetObject(primaryKey));

            entityPaymentRequest.TypePaymentRequestCode = (int)UTIENUM.PaymentRequestType.Void;
            entityPaymentRequest.RegistrationDate = DateTime.Now;
            entityPaymentRequest.Description = string.Format(Resources.Resources.PaymentRequestCancelled, entityPaymentRequest.Number);
            entityPaymentRequest.Number = GetPaymentRequestNumerByBranchId(Convert.ToInt32(entityPaymentRequest.BranchCode));
            entityPaymentRequest.TotalAmount = entityPaymentRequest.TotalAmount * -1;
            entityPaymentRequest.PaymentDate = accountingDate.Date;
            entityPaymentRequest.EstimatedDate = accountingDate.Date;

            int newTechnicalTransaction = DelegateService.technicalTransactionIntegrationService.GetTechnicalTransaction(new TechnicalTransactionParameterDTO { BranchId = Convert.ToInt32(entityPaymentRequest.BranchCode) }).Id;
            int oldTechnicalTransaction = Convert.ToInt32(entityPaymentRequest.TechnicalTransaction);

            entityPaymentRequest.TechnicalTransaction = newTechnicalTransaction;

            DataFacadeManager.Insert(entityPaymentRequest);

            #region SavePaymentRequestCancellation

            DataFacadeManager.Insert(EntityAssembler.CreatePaymentRequestCancellation(paymentRequestId, entityPaymentRequest.PaymentRequestCode));

            #endregion

            #region IsChargeRequest

            //Si es una solicitud de cobro la que se cancela se guarda la información de asosiación al salvamento o recobro
            if (IsChargeRequest)
            {
                filter.PropertyEquals(PAYMEN.PaymentRecovery.Properties.PaymentRequestCode, typeof(PAYMEN.PaymentRecovery).Name, paymentRequestId);
                PAYMEN.PaymentRecovery entityPaymentRecovery = DataFacadeManager.GetObjects(typeof(PAYMEN.PaymentRecovery), filter.GetPredicate()).Cast<PAYMEN.PaymentRecovery>().First();

                entityPaymentRecovery.PaymentRequestCode = entityPaymentRequest.PaymentRequestCode;

                DataFacadeManager.Insert(entityPaymentRecovery);
            }

            #endregion

            #region CoInsurance

            filter.Clear();
            filter.PropertyEquals(PAYMEN.PaymentRequest.Properties.PaymentRequestCode, typeof(PAYMEN.PaymentRequest).Name, paymentRequestId);

            BusinessCollection coInsuranceAssigneds = DataFacadeManager.GetObjects(typeof(PAYMEN.PaymentRequestCoinsurance), filter.GetPredicate());

            foreach (PAYMEN.PaymentRequestCoinsurance entityPaymentRequestCoinsurance in coInsuranceAssigneds)
            {
                entityPaymentRequestCoinsurance.TypePaymentRequestCode = (int)UTIENUM.PaymentRequestType.Void;
                PAYMEN.PaymentRequestCoinsurance paymentRequestCoinsurance = (PAYMEN.PaymentRequestCoinsurance)DataFacadeManager.Insert(EntityAssembler.CreateCancellationPaymentRequestCoInsurance(entityPaymentRequestCoinsurance, entityPaymentRequest.PaymentRequestCode, entityPaymentRequestCoinsurance.CompanyCode));                 
            }

            #endregion

            filter.Clear();
            filter.PropertyEquals(PAYMEN.PaymentRequestClaim.Properties.PaymentRequestCode, typeof(PAYMEN.PaymentRequestClaim).Name, paymentRequestId);
                        
            List<PAYMEN.PaymentRequestClaim> entityPaymentRequestsClaim = DataFacadeManager.GetObjects(typeof(PAYMEN.PaymentRequestClaim), filter.GetPredicate()).Cast<PAYMEN.PaymentRequestClaim>().ToList();

            //Se guardan los vouchers
            BusinessCollection vouchers = DataFacadeManager.GetObjects(typeof(PAYMEN.PaymentVoucher), filter.GetPredicate());

            foreach (PAYMEN.PaymentVoucher entityPaymentVoucher in vouchers)
            {
                filter.Clear();
                filter.PropertyEquals(PAYMEN.PaymentVoucherConcept.Properties.PaymentVoucherCode, typeof(PAYMEN.PaymentVoucherConcept).Name, entityPaymentVoucher.PaymentVoucherCode);

                entityPaymentVoucher.Date = accountingDate.Date;

                //Se inserta la factura
                PAYMEN.PaymentVoucher voucher = (PAYMEN.PaymentVoucher)DataFacadeManager.Insert(EntityAssembler.CreateCancellationVoucher(entityPaymentVoucher, entityPaymentRequest.PaymentRequestCode));

                BusinessCollection voucherConcepts = DataFacadeManager.GetObjects(typeof(PAYMEN.PaymentVoucherConcept), filter.GetPredicate());

                //Se guardan los conceptos
                foreach (PAYMEN.PaymentVoucherConcept entityVoucherConcept in voucherConcepts)
                {
                    filter.Clear();
                    filter.PropertyEquals(PAYMEN.PaymentVoucherConceptTax.Properties.PaymentVoucherConceptCode, typeof(PAYMEN.PaymentVoucherConceptTax).Name, entityVoucherConcept.PaymentVoucherConceptCode);

                    //Se insertan los conceptos
                    PAYMEN.PaymentVoucherConcept voucherConcept = (PAYMEN.PaymentVoucherConcept)DataFacadeManager.Insert(EntityAssembler.CreateCancellationVoucherConcept(entityVoucherConcept, voucher.PaymentVoucherCode));

                    BusinessCollection voucherConceptTaxes = DataFacadeManager.GetObjects(typeof(PAYMEN.PaymentVoucherConceptTax), filter.GetPredicate());

                    //Se guardan los impuestos de los conceptos
                    foreach (PAYMEN.PaymentVoucherConceptTax entityPaymentVoucherConceptTax in voucherConceptTaxes)
                    {
                        PAYMEN.PaymentVoucherConceptTax paymentVoucherConceptTax = (PAYMEN.PaymentVoucherConceptTax)DataFacadeManager.Insert(EntityAssembler.CreateCancellationVoucherConceptTax(entityPaymentVoucherConceptTax, voucherConcept.PaymentVoucherConceptCode));
                    }

                    //Se guarda la asociación con la denuncia
                    DataFacadeManager.Insert(EntityAssembler.CreateClaimPaymentRequest(entityPaymentRequestsClaim.Where(x => x.PaymentVoucherConceptCode == entityVoucherConcept.PaymentVoucherCode).FirstOrDefault(), entityPaymentRequest, voucherConcept.PaymentVoucherConceptCode));
                }
            }

            #region PaymentRequestControl

            CreatePaymentRequestControl(ModelAssembler.CreatePaymentRequestControl(entityPaymentRequest));

            #endregion

            // Se reversa y se genera la contabilidad
            int journalEntryId = DelegateService.generalLedgerService.GetJournalEntryTechnicalTransaction(oldTechnicalTransaction, newTechnicalTransaction);

            return ModelAssembler.CreatePaymentRequest(entityPaymentRequest);
        }

        public PAYMENMOD.PaymentRequest GetPaymentRequestByPaymentRequestId(int paymentRequestId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PAYMEN.PaymentRequestClaim.Properties.PaymentRequestCode, typeof(PAYMEN.PaymentRequestClaim).Name, paymentRequestId);

            PaymentRequestClaimView paymentRequestClaimView = new PaymentRequestClaimView();
            ViewBuilder viewBuilder = new ViewBuilder("PaymentRequestClaimView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, paymentRequestClaimView);

            if (paymentRequestClaimView.PaymentRequests.Count > 0)
            {
                PAYMEN.PaymentRequest entityPaymentRequest = paymentRequestClaimView.PaymentRequests.Cast<PAYMEN.PaymentRequest>().First();                
                List<PAYMEN.PaymentRequestClaim> entityPaymentRequestClaims = paymentRequestClaimView.PaymentRequestClaims.Cast<PAYMEN.PaymentRequestClaim>().ToList();

                VoucherDAO voucherDAO = new VoucherDAO();
                ClaimDAO claimDAO = new ClaimDAO();

                PAYMENMOD.PaymentRequest paymentRequest = ModelAssembler.CreatePaymentRequest(entityPaymentRequest);
                List<Voucher> Vouchers = voucherDAO.GetVouchersByPaymentRequestId(paymentRequest.Id);
                paymentRequest.Claims = new List<Claim>();
                var claim = new Claim();
                var Vourcher = new Voucher();
                var Vourchers = new List<Voucher>();
                foreach (PAYMEN.PaymentRequestClaim paymentRequestClaim in entityPaymentRequestClaims)
                {                    
                    claim = claimDAO.GetClaimModifiesByClaimId(paymentRequestClaim.ClaimCode);

                    foreach (Voucher itemVou in Vouchers)
                    {
                        Vourcher = itemVou;
                        foreach (VoucherConcept voucherConcept in itemVou.Concepts)
                        {
                            if (voucherConcept.Id == paymentRequestClaim.PaymentVoucherConceptCode)
                            {
                                if (Vourchers.Count > 0)
                                {
                                    if (Vourchers[0].Concepts.Where(x => x.Id == voucherConcept.Id).Count() > 0)
                                    {
                                        Vourchers = new List<Voucher>();
                                        break;
                                    }
                                    else
                                    {
                                        Vourchers = new List<Voucher>();
                                        Vourchers.Add(Vourcher);
                                    }
                                }
                                else
                                {
                                    Vourchers.Add(Vourcher);
                                }
                                break;
                            }
                        }
                    }

                    if (Vourchers.Count > 0)
                    {
                        claim.Vouchers = Vourchers;                        
                        claim.Modifications = new List<ClaimModify>{
                            new ClaimModify
                            {
                                Coverages = new List<ClaimCoverage>
                                {                            
                                    new ClaimCoverage
                                    {
                                        Id = claim.Modifications.LastOrDefault().Coverages.Where(x => x.SubClaim == paymentRequestClaim.SubClaim).FirstOrDefault().Id,
                                        CoverageId = claim.Modifications.LastOrDefault().Coverages.Where(x => x.SubClaim == paymentRequestClaim.SubClaim).FirstOrDefault().CoverageId,
                                        SubClaim = paymentRequestClaim.SubClaim,
                                        Estimations = claim.Modifications.LastOrDefault().Coverages.Where(x => x.SubClaim == paymentRequestClaim.SubClaim).FirstOrDefault().Estimations.Where(z => z.Type.Id == paymentRequestClaim.EstimationTypeCode).ToList(),
                                        RiskId = claim.Modifications.LastOrDefault().Coverages.Where(x => x.SubClaim == paymentRequestClaim.SubClaim).FirstOrDefault().RiskId,
                                        Description = claim.Modifications.LastOrDefault().Coverages.Where(x => x.SubClaim == paymentRequestClaim.SubClaim).FirstOrDefault().Description,
                                        RiskDescription = claim.Modifications.LastOrDefault().Coverages.Where(x => x.SubClaim == paymentRequestClaim.SubClaim).FirstOrDefault().RiskDescription,
                                        IsInsured = claim.Modifications.LastOrDefault().Coverages.Where(x => x.SubClaim == paymentRequestClaim.SubClaim).FirstOrDefault().IsInsured,
                                    }
                            
                                },
                                RegistrationDate = claim.Modifications.LastOrDefault().RegistrationDate
                            }
                        };
                        paymentRequest.Claims.Add(claim);
                    }

                    if (Vouchers.Count == paymentRequest.Claims.Count)
                    {
                        break;
                    }
                }

                return paymentRequest;
            }

            return null;
        }

        public ChargeRequest GetChargeRequestByChargeRequestId(int chargeRequestId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PAYMEN.PaymentRequest.Properties.PaymentRequestCode, typeof(PAYMEN.PaymentRequest).Name, chargeRequestId);

            ChargeRequestClaimView chargeRequestClaimView = new ChargeRequestClaimView();
            ViewBuilder viewBuilder = new ViewBuilder("ChargeRequestClaimView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, chargeRequestClaimView);

            if (chargeRequestClaimView.PaymentRequests.Count > 0)
            {
                PAYMEN.PaymentRequest entityPaymentRequest = chargeRequestClaimView.PaymentRequests.Cast<PAYMEN.PaymentRequest>().First();
                CLMEN.Claim entityClaim = chargeRequestClaimView.Claims.Cast<CLMEN.Claim>().First();
                PAYMEN.PaymentRecovery entityPaymentRecovery = chargeRequestClaimView.PaymentRecovereis.Cast<PAYMEN.PaymentRecovery>().First();

                PAYMENMOD.ChargeRequest chargeRequest = ModelAssembler.CreateChargeRequest(entityPaymentRequest);
                chargeRequest.Claim = ModelAssembler.CreateClaim(entityClaim);
                chargeRequest.RecoveryId = Convert.ToInt32(entityPaymentRecovery.RecoveryCode);
                chargeRequest.SalvageId = Convert.ToInt32(entityPaymentRecovery.SalvageCode);

                VoucherDAO voucherDAO = new VoucherDAO();

                chargeRequest.Voucher = voucherDAO.GetVouchersByPaymentRequestId(chargeRequest.Id).First();

                return chargeRequest;
            }

            return null;
        }

        public PAYMENMOD.PaymentRequest GetReportPaymentRequestByPaymentRequestId(int paymentRequestId)
        {
            ClaimDAO claimDAO = new ClaimDAO();
            VoucherDAO voucherDAO = new VoucherDAO();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PAYMEN.PaymentRequest.Properties.PaymentRequestCode, typeof(PAYMEN.PaymentRequest).Name, paymentRequestId);

            PaymentRequestClaimReportView paymentRequestClaimReportView = new PaymentRequestClaimReportView();
            ViewBuilder viewBuilder = new ViewBuilder("PaymentRequestClaimReportView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, paymentRequestClaimReportView);

            if (paymentRequestClaimReportView.PaymentRequests.Count > 0)
            {
                PAYMEN.PaymentRequest entityPaymentRequest = paymentRequestClaimReportView.PaymentRequests.Cast<PAYMEN.PaymentRequest>().First();
                CLMEN.Claim entityClaim = paymentRequestClaimReportView.Claims.Cast<CLMEN.Claim>().First();                
                List<PAYMEN.PaymentRequestClaim> entitiesPaymentRequestClaim = paymentRequestClaimReportView.PaymentRequestClaims.Cast<PAYMEN.PaymentRequestClaim>().ToList();
                List<PAYMEN.PaymentRequestCoinsurance> entitypaymentRequestCoinsurances = paymentRequestClaimReportView.PaymentRequestCoinsurances.Cast<PAYMEN.PaymentRequestCoinsurance>().ToList();
                COMMEN.Currency entityCurrency = paymentRequestClaimReportView.Currencies.Cast<COMMEN.Currency>().First();
                PARAMEN.PersonType entityPersonType = paymentRequestClaimReportView.PersonTypes.Cast<PARAMEN.PersonType>().First();

                PAYMENMOD.PaymentRequest paymentRequest = ModelAssembler.CreatePaymentRequest(entityPaymentRequest);

                paymentRequest.Claims = new List<Claim>();
                paymentRequest.Currency = ModelAssembler.CreateCurrency(entityCurrency);
                paymentRequest.PersonType = ModelAssembler.CreatePersonType(entityPersonType);
                paymentRequest.Branch.Description = paymentRequestClaimReportView.Branches.Cast<COMMEN.Branch>().FirstOrDefault().Description;
                paymentRequest.Prefix.Description = paymentRequestClaimReportView.Prefixes.Cast<COMMEN.Prefix>().FirstOrDefault().Description;
                List<Voucher> Vouchers = voucherDAO.GetVouchersByPaymentRequestId(paymentRequest.Id);
                List<Voucher> Vourchers = new List<Voucher>();

                foreach (PAYMEN.PaymentRequestClaim entityPaymentRequestClaim in entitiesPaymentRequestClaim)
                {
                    Claim claim = claimDAO.GetClaimByClaimIdSubClaimEstimationTypeId(entityPaymentRequestClaim.ClaimCode, entityPaymentRequestClaim.SubClaim, entityPaymentRequestClaim.EstimationTypeCode);
                    claim.Inspection = claimDAO.GetInspectionByClaimId(claim.Id);

                    foreach (Voucher voucher in Vouchers)
                    {
                        foreach (VoucherConcept voucherConcept in voucher.Concepts)
                        {
                            if (voucherConcept.Id == entityPaymentRequestClaim.PaymentVoucherConceptCode)
                            {
                                if (Vourchers.Count > 0)
                                {
                                    if (Vourchers[0].Concepts.Where(x => x.Id == voucherConcept.Id).Count() > 0)
                                    {
                                        Vourchers = new List<Voucher>();
                                        break;
                                    }
                                    else
                                    {
                                        Vourchers = new List<Voucher>();
                                        Vourchers.Add(voucher);
                                    }
                                }
                                else
                                {
                                    Vourchers.Add(voucher);
                                }
                                break;
                            }
                        }
                    }

                    if (Vourchers.Count > 0)
                    {
                        claim.Vouchers = Vourchers;
                        paymentRequest.Claims.Add(claim);
                    }

                    if (Vouchers.Count == paymentRequest.Claims.Count)
                    {
                        break;
                    }
                }
                
                paymentRequest.CoInsurance = ModelAssembler.CreatePaymentRequestCoInsurances(entitypaymentRequestCoinsurances);

                paymentRequest.CoInsurance.ForEach(coInsurance =>
                {
                    PrimaryKey primaryKey = COMMEN.CoInsuranceCompany.CreatePrimaryKey(coInsurance.CompanyId);
                    COMMEN.CoInsuranceCompany coInsuranceCompany = (COMMEN.CoInsuranceCompany)DataFacadeManager.GetObject(primaryKey);
                    coInsurance.Company = coInsuranceCompany.Description;
                });

                if (paymentRequestClaimReportView.PaymentMethods.Count > 0)
                {
                    CLMEN.PaymentMethod entityPaymentMethod = paymentRequestClaimReportView.PaymentMethods.Cast<CLMEN.PaymentMethod>().First();
                    paymentRequest.PaymentMethod = ModelAssembler.CreatePaymentMethod(entityPaymentMethod);
                }

                return paymentRequest;
            }

            return null;
        }

        public ChargeRequest CreateChargeRequest(ChargeRequest chargeRequest)
        {
            PAYMEN.PaymentRequest entityPaymentRequest = EntityAssembler.CreateChargeRequest(chargeRequest);
            DataFacadeManager.Insert(entityPaymentRequest);

            chargeRequest.Id = entityPaymentRequest.PaymentRequestCode;

            VoucherDAO voucherDAO = new VoucherDAO();
            chargeRequest.Voucher.PaymentRequestId = chargeRequest.Id;
            chargeRequest.Voucher.Id = voucherDAO.CreateVoucher(chargeRequest.Voucher).Id;

            foreach (VoucherConcept voucherConcept in chargeRequest.Voucher.Concepts)
            {
                VoucherConceptDAO voucherConceptDAO = new VoucherConceptDAO();
                voucherConcept.Id = voucherConceptDAO.CreateVoucherConcept(voucherConcept, chargeRequest.Voucher.Id).Id;

                PAYMEN.PaymentRequestClaim entityPaymentRequestClaim = EntityAssembler.CreateClaimPaymentRequest(chargeRequest, chargeRequest.Voucher, voucherConcept.Id);
                DataFacadeManager.Insert(entityPaymentRequestClaim);
            }

            PAYMEN.PaymentRecovery entityPaymentRecovery = EntityAssembler.CreatePaymentRecovery(chargeRequest);
            DataFacadeManager.Insert(entityPaymentRecovery);

            return chargeRequest;
        }

        public PAYMENMOD.PaymentRequest GetPaymentRequestByPrefixIdBranchIdNumber(int prefixId, int branchId, int number)
        {
            PAYMENMOD.PaymentRequest paymentRequest = new PAYMENMOD.PaymentRequest();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PAYMEN.PaymentRequest.Properties.BranchCode, typeof(PAYMEN.PaymentRequest).Name, branchId);
            filter.And();
            filter.PropertyEquals(PAYMEN.PaymentRequest.Properties.PrefixCode, typeof(PAYMEN.PaymentRequest).Name, prefixId);
            filter.And();
            filter.PropertyEquals(PAYMEN.PaymentRequest.Properties.Number, typeof(PAYMEN.PaymentRequest).Name, number);
            filter.And();
            filter.PropertyEquals(PAYMEN.PaymentRequest.Properties.TypePaymentRequestCode, typeof(PAYMEN.PaymentRequest).Name, UTIENUM.PaymentRequestType.Payment);


            PAYMEN.PaymentRequest entityPaymentRequest = DataFacadeManager.GetObjects(typeof(PAYMEN.PaymentRequest), filter.GetPredicate()).Cast<PAYMEN.PaymentRequest>().FirstOrDefault();

            if (entityPaymentRequest == null)
                return null;

            List<PAYMEN.PaymentRequestCancellation> entityPaymentRequestCancellations = DataFacadeManager.GetObjects(typeof(PAYMEN.PaymentRequestCancellation)).Cast<PAYMEN.PaymentRequestCancellation>().ToList();

            if (entityPaymentRequestCancellations.Exists(x => x.PaymentRequestCode == entityPaymentRequest.PaymentRequestCode))
                throw new BusinessException(Resources.Resources.RequestAlreadyCanceled);

            return ModelAssembler.CreatePaymentRequest(entityPaymentRequest);
        }

        public ChargeRequest GetChargeRequestByPrefixIdBranchIdNumber(int prefixId, int branchId, int number)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PAYMEN.PaymentRequest.Properties.BranchCode, typeof(PAYMEN.PaymentRequest).Name, branchId);
            filter.And();
            filter.PropertyEquals(PAYMEN.PaymentRequest.Properties.PrefixCode, typeof(PAYMEN.PaymentRequest).Name, prefixId);
            filter.And();
            filter.PropertyEquals(PAYMEN.PaymentRequest.Properties.Number, typeof(PAYMEN.PaymentRequest).Name, number);
            filter.And();
            filter.PropertyEquals(PAYMEN.PaymentRequest.Properties.TypePaymentRequestCode, typeof(PAYMEN.PaymentRequest).Name, UTIENUM.PaymentRequestType.Recovery);

            PAYMEN.PaymentRequest entityPaymentRequest = DataFacadeManager.GetObjects(typeof(PAYMEN.PaymentRequest), filter.GetPredicate()).Cast<PAYMEN.PaymentRequest>().FirstOrDefault();

            if (entityPaymentRequest == null)
                return null;

            List<PAYMEN.PaymentRequestCancellation> entityPaymentRequestCancellations = DataFacadeManager.GetObjects(typeof(PAYMEN.PaymentRequestCancellation)).Cast<PAYMEN.PaymentRequestCancellation>().ToList();

            if (entityPaymentRequestCancellations.Exists(x => x.PaymentRequestCode == entityPaymentRequest.PaymentRequestCode))
                throw new BusinessException(Resources.Resources.RequestAlreadyCanceled);

            return ModelAssembler.CreateChargeRequest(entityPaymentRequest);
        }

        public List<PAYMENMOD.PaymentRequest> GetPaymentRequestClaimsByPaymentRequestId(int paymentRequestId)
        {
            List<PAYMENMOD.PaymentRequest> paymentRequests = new List<PAYMENMOD.PaymentRequest>();
            VoucherDAO voucherDAO = new VoucherDAO();
            ClaimDAO claimDAO = new ClaimDAO();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.PropertyEquals(PAYMEN.PaymentRequestClaim.Properties.PaymentRequestCode, typeof(PAYMEN.PaymentRequest).Name, paymentRequestId);

            PaymentRequestClaimView paymentRequestClaimView = new PaymentRequestClaimView();
            ViewBuilder viewBuilder = new ViewBuilder("PaymentRequestClaimView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, paymentRequestClaimView);

            if (paymentRequestClaimView.PaymentRequests.Count > 0)
            {
                paymentRequests = ModelAssembler.CreatePaymentRequests(paymentRequestClaimView.PaymentRequests);
                
                foreach (PAYMENMOD.PaymentRequest paymentRequest in paymentRequests)
                {                    
                    List<Voucher> Vouchers = voucherDAO.GetVouchersByPaymentRequestId(paymentRequest.Id);
                    List<Voucher> Vourchers = new List<Voucher>();
                    Claim claim;
                    paymentRequest.Claims = new List<Claim>();

                    List<PAYMEN.PaymentRequestClaim> entitiesPaymentRequestClaim = paymentRequestClaimView.PaymentRequestClaims.Cast<PAYMEN.PaymentRequestClaim>().ToList();                                        

                    foreach(PAYMEN.PaymentRequestClaim paymentRequestClaim in entitiesPaymentRequestClaim)
                    {                        
                        claim = claimDAO.GetClaimModifiesByClaimId(paymentRequestClaim.ClaimCode);

                        foreach (Voucher voucher in Vouchers)
                        {
                            foreach (VoucherConcept voucherConcept in voucher.Concepts)
                            {
                                if (voucherConcept.Id == paymentRequestClaim.PaymentVoucherConceptCode)
                                {
                                    if (Vourchers.Count > 0)
                                    {
                                        if (Vourchers[0].Concepts.Where(x => x.Id == voucherConcept.Id).Count() > 0)
                                        {
                                            Vourchers = new List<Voucher>();
                                            break;
                                        }
                                        else
                                        {
                                            Vourchers = new List<Voucher>();
                                            Vourchers.Add(voucher);
                                        }
                                    }
                                    else
                                    {
                                        Vourchers.Add(voucher);
                                    }
                                    break;
                                }

                            }

                        }

                        if (Vourchers.Count > 0)
                        {
                            claim.Vouchers = Vourchers;
                            claim.Modifications = new List<ClaimModify>{
                                new ClaimModify
                                {
                                    Coverages = new List<ClaimCoverage>
                                    {
                                        new ClaimCoverage
                                        {
                                            Id = claim.Modifications.LastOrDefault().Coverages.Where(x => x.SubClaim == paymentRequestClaim.SubClaim).FirstOrDefault().Id,
                                            CoverageId = claim.Modifications.LastOrDefault().Coverages.Where(x => x.SubClaim == paymentRequestClaim.SubClaim).FirstOrDefault().CoverageId,
                                            SubClaim = paymentRequestClaim.SubClaim,
                                            Estimations = claim.Modifications.LastOrDefault().Coverages.Where(x => x.SubClaim == paymentRequestClaim.SubClaim).FirstOrDefault().Estimations.Where(z => z.Type.Id == paymentRequestClaim.EstimationTypeCode).ToList(),
                                            RiskId = claim.Modifications.LastOrDefault().Coverages.Where(x => x.SubClaim == paymentRequestClaim.SubClaim).FirstOrDefault().RiskId,
                                            Description = claim.Modifications.LastOrDefault().Coverages.Where(x => x.SubClaim == paymentRequestClaim.SubClaim).FirstOrDefault().Description,
                                            RiskDescription = claim.Modifications.LastOrDefault().Coverages.Where(x => x.SubClaim == paymentRequestClaim.SubClaim).FirstOrDefault().RiskDescription,
                                            IsInsured = claim.Modifications.LastOrDefault().Coverages.Where(x => x.SubClaim == paymentRequestClaim.SubClaim).FirstOrDefault().IsInsured,
                                        }
                                    },
                                    RegistrationDate = claim.Modifications.LastOrDefault().RegistrationDate
                                }
                            };
                            paymentRequest.Claims.Add(claim);

                        }

                        if (Vouchers.Count == paymentRequest.Claims.Count)
                        {
                            break;
                        }
                    }

                    if (paymentRequestClaimView.Currencies.Count > 0)
                    {
                        paymentRequest.Currency.Description = paymentRequestClaimView.Currencies.Cast<COMMEN.Currency>().First(x => x.CurrencyCode == paymentRequest.Currency.Id).Description;
                    }

                    if (paymentRequestClaimView.PaymentMethods.Count > 0)
                    {
                        paymentRequest.PaymentMethod.Description = paymentRequestClaimView.PaymentMethods.Cast<CLMEN.PaymentMethod>().First(x => x.PaymentMethodCode == paymentRequest.PaymentMethod.Id).Description;
                    }
                }
            }

            return paymentRequests;
        }


        public List<PAYMENMOD.PaymentRequest> SearchPaymentRequests(PAYMENMOD.PaymentRequest paymentRequest)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            VoucherDAO voucherDAO = new VoucherDAO();
            ClaimDAO claimDAO = new ClaimDAO();
            var emptyFilter = true;

            if (paymentRequest.Number > 0)
            {
                filter.Property(PAYMEN.PaymentRequest.Properties.Number, typeof(PAYMEN.PaymentRequest).Name);
                filter.Equal();
                filter.Constant(paymentRequest.Number);
                emptyFilter = false;
            }

            if (paymentRequest.MovementType.Source.Id > 0)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(PAYMEN.PaymentRequest.Properties.PaymentSourceCode, typeof(PAYMEN.PaymentRequest).Name);
                filter.Equal();
                filter.Constant(paymentRequest.MovementType.Source.Id);
                emptyFilter = false;
            }

            if (paymentRequest.Branch.Id > 0)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(PAYMEN.PaymentRequest.Properties.BranchCode, typeof(PAYMEN.PaymentRequest).Name);
                filter.Equal();
                filter.Constant(paymentRequest.Branch.Id);
                emptyFilter = false;
            }

            if (paymentRequest.ClaimNumber > 0)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(PAYMEN.PaymentRequestClaim.Properties.ClaimNumber, typeof(PAYMEN.PaymentRequestClaim).Name);
                filter.Equal();
                filter.Constant(paymentRequest.ClaimNumber);
                emptyFilter = false;
            }

            if (paymentRequest.EstimatedDate != null && paymentRequest.EstimatedDate > DateTime.MinValue)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(PAYMEN.PaymentRequest.Properties.EstimatedDate, typeof(PAYMEN.PaymentRequest).Name);
                filter.Equal();
                filter.Constant(paymentRequest.EstimatedDate);
                emptyFilter = false;
            }

            if (paymentRequest.IndividualId > 0)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(PAYMEN.PaymentRequest.Properties.IndividualId, typeof(PAYMEN.PaymentRequest).Name);
                filter.Equal();
                filter.Constant(paymentRequest.IndividualId);
                emptyFilter = false;
            }

            List<PAYMEN.PaymentRequestCancellation> entityPaymentRequestCancellations = DataFacadeManager.GetObjects(typeof(PAYMEN.PaymentRequestCancellation)).Cast<PAYMEN.PaymentRequestCancellation>().ToList();

            if (entityPaymentRequestCancellations.Any())
            {
                filter.And();
                filter.Not();
                filter.Property(PAYMEN.PaymentRequest.Properties.PaymentRequestCode, typeof(PAYMEN.PaymentRequest).Name);
                filter.In();
                filter.ListValue();
                entityPaymentRequestCancellations.ForEach(x =>
                {
                    filter.Constant(x.PaymentRequestCode);
                    filter.Constant(x.PaymentRequestCancellationCode);
                });
                filter.EndList();
            }

            PaymentRequestSummaryView paymentRequestSummaryView = new PaymentRequestSummaryView();
            ViewBuilder viewBuilder = new ViewBuilder("PaymentRequestSummaryView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, paymentRequestSummaryView);

            List<PAYMENMOD.PaymentRequest> paymentRequests = new List<PAYMENMOD.PaymentRequest>();            
            List<Voucher> Vourchers = new List<Voucher>();
            Claim claim = new Claim();

            if (paymentRequestSummaryView.PaymentRequests.Count > 0)
            {                
                paymentRequests = ModelAssembler.CreatePaymentRequests(paymentRequestSummaryView.PaymentRequests);

                foreach (PAYMENMOD.PaymentRequest payment in paymentRequests)
                {
                    List<Voucher> Vouchers = voucherDAO.GetVouchersByPaymentRequestId(payment.Id);
                    List<PAYMEN.PaymentRequestClaim> entitiesPaymentRequestClaim = paymentRequestSummaryView.PaymentRequestsClaim.Cast<PAYMEN.PaymentRequestClaim>().ToList();
                    payment.Claims = new List<Claim>();

                    foreach (PAYMEN.PaymentRequestClaim entityPaymentRequestClaim in entitiesPaymentRequestClaim)
                    {
                        claim = claimDAO.GetClaimModifiesByClaimId(entityPaymentRequestClaim.ClaimCode);

                        if (paymentRequestSummaryView.Branches.Count > 0)
                        {
                            payment.Branch.Description = paymentRequestSummaryView.Branches.Cast<COMMEN.Branch>().First().Description;
                        }

                        if (paymentRequestSummaryView.Prefixes.Count > 0)
                        {
                            payment.Prefix.Description = paymentRequestSummaryView.Prefixes.Cast<COMMEN.Prefix>().First().Description;
                        }

                        foreach (Voucher voucher in Vouchers)
                        {
                            foreach (VoucherConcept voucherConcept in voucher.Concepts)
                            {
                                if (voucherConcept.Id == entityPaymentRequestClaim.PaymentVoucherConceptCode)
                                {
                                    if (Vourchers.Count > 0)
                                    {
                                        if (Vourchers[0].Concepts.Where(x => x.Id == voucherConcept.Id).Count() > 0)
                                        {
                                            Vourchers = new List<Voucher>();
                                            break;
                                        }
                                        else
                                        {
                                            Vourchers = new List<Voucher>();
                                            Vourchers.Add(voucher);
                                        }
                                    }
                                    else
                                    {
                                        Vourchers.Add(voucher);
                                    }
                                    break;
                                }
                            }
                        }

                        if (Vourchers.Count > 0)
                        {
                            claim.Vouchers = Vourchers;
                            payment.Claims.Add(claim);
                        }

                        if (Vouchers.Count == paymentRequest.Claims.Count)
                        {
                            break;
                        }
                    }
                }

                return paymentRequests;
            }
            else
            {
                return paymentRequests;
            }
        }

        public List<PAYMENMOD.ChargeRequest> SearchChargeRequests(PAYMENMOD.ChargeRequest chargeRequest)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            var emptyFilter = true;

            if (chargeRequest.Number > 0)
            {
                filter.Property(PAYMEN.PaymentRequest.Properties.Number, typeof(PAYMEN.PaymentRequest).Name);
                filter.Equal();
                filter.Constant(chargeRequest.Number);
                emptyFilter = false;
            }

            if (chargeRequest.MovementType.Source.Id > 0)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(PAYMEN.PaymentRequest.Properties.PaymentSourceCode, typeof(PAYMEN.PaymentRequest).Name);
                filter.Equal();
                filter.Constant(chargeRequest.MovementType.Source.Id);
                emptyFilter = false;
            }

            if (chargeRequest.Branch.Id > 0)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(PAYMEN.PaymentRequest.Properties.BranchCode, typeof(PAYMEN.PaymentRequest).Name);
                filter.Equal();
                filter.Constant(chargeRequest.Branch.Id);
                emptyFilter = false;
            }

            if (chargeRequest.IndividualId > 0)
            {
                if (!emptyFilter)
                    filter.And();
                filter.Property(PAYMEN.PaymentRequest.Properties.IndividualId, typeof(PAYMEN.PaymentRequest).Name);
                filter.Equal();
                filter.Constant(chargeRequest.IndividualId);
                emptyFilter = false;
            }

            List<PAYMEN.PaymentRequestCancellation> entityPaymentRequestCancellations = DataFacadeManager.GetObjects(typeof(PAYMEN.PaymentRequestCancellation)).Cast<PAYMEN.PaymentRequestCancellation>().ToList();

            if (entityPaymentRequestCancellations.Any())
            {
                filter.And();
                filter.Not();
                filter.Property(PAYMEN.PaymentRequest.Properties.PaymentRequestCode, typeof(PAYMEN.PaymentRequest).Name);
                filter.In();
                filter.ListValue();
                entityPaymentRequestCancellations.ForEach(x =>
                {
                    filter.Constant(x.PaymentRequestCode);
                    filter.Constant(x.PaymentRequestCancellationCode);
                });
                filter.EndList();
            }

            ChargeRequestSummaryView paymentRequestSummaryView = new ChargeRequestSummaryView();
            ViewBuilder viewBuilder = new ViewBuilder("ChargeRequestSummaryView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, paymentRequestSummaryView);

            List<ChargeRequest> chargeRequests = new List<ChargeRequest>();

            if (paymentRequestSummaryView.PaymentRequests.Count > 0)
            {
                chargeRequests = ModelAssembler.CreateChargeRequests(paymentRequestSummaryView.PaymentRequests);

                foreach (ChargeRequest charge in chargeRequests)
                {
                    charge.RecoveryOrSalvageAmount = CalculateRecoveryOrSalvageAmountByChargeRequestId(charge.Id);

                    PAYMEN.PaymentRequestClaim entityPaymentRequestClaim = paymentRequestSummaryView.PaymentRequestsClaim.Cast<PAYMEN.PaymentRequestClaim>().First(x => x.PaymentRequestCode == charge.Id);

                    if (paymentRequestSummaryView.Claims.Count > 0)
                    {
                        CLMEN.Claim entityCliam = paymentRequestSummaryView.Claims.Cast<CLMEN.Claim>().First(x => x.ClaimCode == entityPaymentRequestClaim.ClaimCode);
                        charge.Claim = ModelAssembler.CreateClaim(entityCliam);

                        filter.Clear();
                        filter.PropertyEquals(CLMEN.ClaimModify.Properties.ClaimCode, typeof(CLMEN.ClaimCoverage).Name, charge.Claim.Id);

                        string[] sortColumn = new string[1];
                        sortColumn[0] = "-" + CLMEN.ClaimModify.Properties.ClaimModifyCode;

                        CLMEN.ClaimModify entityClaimModify = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CLMEN.ClaimModify), filter.GetPredicate(), sortColumn, 1)).Cast<CLMEN.ClaimModify>().FirstOrDefault();

                        charge.Claim.Modifications = ModelAssembler.CreateClaimModifies(entityClaimModify);

                        filter.Clear();
                        filter.PropertyEquals(CLMEN.ClaimCoverage.Properties.ClaimModifyCode, typeof(CLMEN.ClaimCoverage).Name, charge.Claim.Modifications.First().Id);

                        PaymentRequestModificationView paymentRequestModificationView = new PaymentRequestModificationView();
                        ViewBuilder builder = new ViewBuilder("PaymentRequestModificationView");
                        builder.Filter = filter.GetPredicate();

                        DataFacadeManager.Instance.GetDataFacade().FillView(builder, paymentRequestModificationView);

                        if (paymentRequestModificationView.ClaimCoverages.Count > 0)
                        {
                            List<CLMEN.ClaimCoverage> entitiesClaimCoverage = paymentRequestModificationView.ClaimCoverages.Cast<CLMEN.ClaimCoverage>().Where(x => x.ClaimModifyCode == charge.Claim.Modifications.First().Id).ToList();

                            foreach (CLMEN.ClaimCoverage entityClaimCoverage in entitiesClaimCoverage)
                            {
                                charge.Claim.Modifications.First().Coverages.Add(ModelAssembler.CreateClaimCoverage(entityClaimCoverage));
                            }

                            if (paymentRequestModificationView.Coverages.Count > 0)
                            {
                                foreach (ClaimCoverage claimCoverage in charge.Claim.Modifications.First().Coverages)
                                {
                                    claimCoverage.Description = paymentRequestModificationView.Coverages.Cast<QUOEN.Coverage>().First(x => x.CoverageId == claimCoverage.CoverageId).PrintDescription;
                                }
                            }

                            if (paymentRequestModificationView.ClaimCoveragesAmount.Count > 0)
                            {
                                foreach (ClaimCoverage claimCoverage in charge.Claim.Modifications.First().Coverages)
                                {
                                    List<CLMEN.ClaimCoverageAmount> entitiesClaimCoverageAmount = paymentRequestModificationView.ClaimCoveragesAmount.Cast<CLMEN.ClaimCoverageAmount>().Where(x => x.ClaimCoverageCode == claimCoverage.Id).ToList();

                                    claimCoverage.Estimations = ModelAssembler.CreateEstimations(entitiesClaimCoverageAmount);

                                    if (paymentRequestModificationView.EstimationTypes.Count > 0)
                                    {
                                        foreach (Estimation estimation in claimCoverage.Estimations)
                                        {
                                            estimation.Type = ModelAssembler.CreateEstimationType(paymentRequestModificationView.EstimationTypes.Cast<PARAMEN.EstimationType>().First(x => x.EstimateTypeCode == estimation.Type.Id));
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (paymentRequestSummaryView.Branches.Count > 0)
                    {
                        charge.Claim.Branch.Description = paymentRequestSummaryView.ClaimBranches.Cast<COMMEN.Branch>().First(x => x.BranchCode == charge.Claim.Branch.Id).Description;
                        charge.Branch.Description = paymentRequestSummaryView.Branches.Cast<COMMEN.Branch>().First(x => x.BranchCode == charge.Branch.Id).Description;
                    }

                    if (paymentRequestSummaryView.Prefixes.Count > 0)
                    {
                        charge.Claim.Prefix.Description = paymentRequestSummaryView.Prefixes.Cast<COMMEN.Prefix>().First(x => x.PrefixCode == charge.Claim.Prefix.Id).Description;
                        charge.Prefix.Description = paymentRequestSummaryView.Prefixes.Cast<COMMEN.Prefix>().First(x => x.PrefixCode == charge.Prefix.Id).Description;
                    }

                    if (paymentRequestSummaryView.PaymentVouchers.Count > 0)
                    {
                        PAYMEN.PaymentVoucher entityPaymentVoucher = paymentRequestSummaryView.PaymentVouchers.Cast<PAYMEN.PaymentVoucher>().Where(x => x.PaymentRequestCode == charge.Id).First();

                        charge.Voucher = ModelAssembler.CreateVoucher(entityPaymentVoucher);

                        if (paymentRequestSummaryView.PaymentVoucherConcepts.Count > 0)
                        {
                            charge.Voucher.Concepts = new List<VoucherConcept>();

                            List<PAYMEN.PaymentVoucherConcept> entitiesPaymentVoucherConcept = paymentRequestSummaryView.PaymentVoucherConcepts.Cast<PAYMEN.PaymentVoucherConcept>().Where(x => x.PaymentVoucherCode == charge.Voucher.Id).ToList();
                            foreach (PAYMEN.PaymentVoucherConcept entityPaymentVoucherConcept in entitiesPaymentVoucherConcept)
                            {
                                charge.Voucher.Concepts.Add(ModelAssembler.CreateVoucherConcept(entityPaymentVoucherConcept));
                            }
                        }
                    }
                }

                return chargeRequests;
            }
            else
            {
                return chargeRequests;
            }
        }

        public List<ClaimPersonType> GetPersonTypes(bool isPaymentRequest)
        {

            List<ClaimPersonType> personTypes = new List<ClaimPersonType>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            if (isPaymentRequest)
            {
                filter.PropertyEquals(PAYMEN.ClaimPersonType.Properties.PaymentRequestEnable, typeof(PAYMEN.ClaimPersonType).Name, isPaymentRequest);
            }
            else
            {
                filter.PropertyEquals(PAYMEN.ClaimPersonType.Properties.ChargeRequestEnable, typeof(PAYMEN.ClaimPersonType).Name, !isPaymentRequest);
            }

            PaymentPersonTypeView paymentPersonTypeView = new PaymentPersonTypeView();
            ViewBuilder viewBuilder = new ViewBuilder("PaymentPersonTypeView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, paymentPersonTypeView);

            if (paymentPersonTypeView.PersonTypes.Count > 0)
            {
                personTypes = ModelAssembler.CreatePersonTypes(paymentPersonTypeView.PersonTypes);
            }

            return personTypes;
        }

        public List<ClaimPersonType> GetClaimSearchPersonType(int prefixId, int searchType)
        {
            List<ClaimPersonType> personTypes = new List<ClaimPersonType>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PAYMEN.ClaimSearchPersonType.Properties.SearchType, typeof(PAYMEN.ClaimSearchPersonType).Name, searchType);
            filter.And();
            filter.PropertyEquals(PAYMEN.ClaimSearchPersonType.Properties.PrefixCode, typeof(PAYMEN.ClaimSearchPersonType).Name, prefixId);
            SearchPersonTypeView searchPersonTypeView = new SearchPersonTypeView();
            ViewBuilder viewBuilder = new ViewBuilder("SearchPersonTypeView");
            viewBuilder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, searchPersonTypeView);
            if (searchPersonTypeView.PersonType.Count > 0)
            {
                personTypes = ModelAssembler.CreatePersonTypes(searchPersonTypeView.PersonType);
            }

            return personTypes;
        }

        public PaymentRequestCoInsurance SavePaymentRequestCoInsurance(PaymentRequestCoInsurance claimCoverageCoInsurance, int paymentRequestId, int CompanyId)
        {
            return ModelAssembler.CreatePaymentRequestCoInsurance((PAYMEN.PaymentRequestCoinsurance)DataFacadeManager.Insert(EntityAssembler.CreatePaymentRequestCoInsurance(claimCoverageCoInsurance, paymentRequestId, CompanyId)));
        }

        public PaymentRequestCoInsurance GetPaymentRequestCoInsuranceByPaymentRequestIdCompanyId(int paymentRequestId, int companyId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PAYMEN.PaymentRequestCoinsurance.Properties.PaymentRequestCode, typeof(PAYMEN.PaymentRequestCoinsurance).Name, paymentRequestId);
            filter.And();
            filter.PropertyEquals(PAYMEN.PaymentRequestCoinsurance.Properties.CompanyCode, typeof(PAYMEN.PaymentRequestCoinsurance).Name, companyId);

            return ModelAssembler.CreatePaymentRequestCoInsurance(DataFacadeManager.GetObjects(typeof(PAYMEN.PaymentRequestCoinsurance), filter.GetPredicate()).Cast<PAYMEN.PaymentRequestCoinsurance>().FirstOrDefault());
        }

        public decimal GetPaymentsValueByClaimIdSubClaimIdEstimationTypeIdCurrencyId(int claimId, int subClaimId, int? estimationTypeId = 0, int? currencyId = 0, DateTime? endDateClaimModify = null)
        {
            VoucherDAO voucherDAO = new VoucherDAO();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PAYMEN.PaymentRequestClaim.Properties.ClaimCode, typeof(PAYMEN.PaymentRequestClaim).Name, claimId);
            filter.And();
            filter.PropertyEquals(PAYMEN.PaymentRequestClaim.Properties.SubClaim, typeof(PAYMEN.PaymentRequestClaim).Name, subClaimId);

            if (estimationTypeId > 0)
            {
                filter.And();
                filter.PropertyEquals(PAYMEN.PaymentRequestClaim.Properties.EstimationTypeCode, typeof(PAYMEN.PaymentRequestClaim).Name, estimationTypeId);
            }

            if (endDateClaimModify != null)
            {
                filter.And();
                filter.Property(PAYMEN.PaymentRequest.Properties.RegistrationDate, typeof(PAYMEN.PaymentRequest).Name);
                filter.Less();
                filter.Constant(endDateClaimModify);
            }

            if (estimationTypeId != 7 && estimationTypeId != 2)
            {
                filter.And();
                filter.Property(PAYMEN.PaymentRequest.Properties.TypePaymentRequestCode, typeof(PAYMEN.PaymentRequest).Name);
                filter.Distinct();
                filter.Constant(UTIENUM.PaymentRequestType.Recovery);
            }

            List<PAYMEN.PaymentRequestCancellation> entityPaymentRequestCancellations = DataFacadeManager.GetObjects(typeof(PAYMEN.PaymentRequestCancellation)).Cast<PAYMEN.PaymentRequestCancellation>().ToList();

            if (entityPaymentRequestCancellations.Any())
            {
                filter.And();
                filter.Not();
                filter.Property(PAYMEN.PaymentRequest.Properties.PaymentRequestCode, typeof(PAYMEN.PaymentRequest).Name);
                filter.In();
                filter.ListValue();
                entityPaymentRequestCancellations.ForEach(x =>
                {
                    filter.Constant(x.PaymentRequestCode);
                    filter.Constant(x.PaymentRequestCancellationCode);
                });
                filter.EndList();
            }

            ClaimPaymentsView claimsPaymentView = new ClaimPaymentsView();
            ViewBuilder viewBuilder = new ViewBuilder("ClaimPaymentsView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, claimsPaymentView);

            if (claimsPaymentView.PaymentRequestClaims.Count > 0)
            {
                List<PAYMEN.PaymentRequest> paymentRequests = claimsPaymentView.PaymentRequests.Cast<PAYMEN.PaymentRequest>().ToList();
                List<PAYMEN.PaymentVoucherConcept> paymentVoucherConcepts = claimsPaymentView.PaymentVoucherConcepts.Cast<PAYMEN.PaymentVoucherConcept>().ToList();

                //Para calculo de la reserva en moneda de estimación
                paymentRequests.ForEach(paymentRequest =>
                {
                    if (paymentRequest.CurrencyCode != currencyId)
                    {
                        decimal SellAmount = DelegateService.commonServiceCore.GetExchangeRateByRateDateCurrencyId(Convert.ToDateTime(paymentRequest.RegistrationDate), Convert.ToInt32(currencyId)).SellAmount;

                        paymentVoucherConcepts.ForEach(paymentVoucherConcept =>
                        {
                            paymentVoucherConcept.Value /= SellAmount;
                        });
                    }
                });

                return paymentVoucherConcepts.Sum(x => Math.Abs(Convert.ToDecimal(x.Value)));
            }

            return 0;
        }

        public decimal GetRecoveryAmountByRecoveryIdOrSalvageId(int recoveryIdOrSalvageId, bool isRecovery)
        {
            decimal recoveryAmount = 0;

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            if (isRecovery)
                filter.PropertyEquals(PAYMEN.PaymentRecovery.Properties.RecoveryCode, typeof(PAYMEN.PaymentRecovery).Name, recoveryIdOrSalvageId);
            else
                filter.PropertyEquals(PAYMEN.PaymentRecovery.Properties.SalvageCode, typeof(PAYMEN.PaymentRecovery).Name, recoveryIdOrSalvageId);

            PaymentRecoveryView paymentRecoveryView = new PaymentRecoveryView();
            ViewBuilder viewBuilder = new ViewBuilder("PaymentRecoveryView");
            viewBuilder.Filter = filter.GetPredicate();

            List<PAYMEN.PaymentRequestCancellation> entityPaymentRequestCancellations = DataFacadeManager.GetObjects(typeof(PAYMEN.PaymentRequestCancellation)).Cast<PAYMEN.PaymentRequestCancellation>().ToList();

            if (entityPaymentRequestCancellations.Any())
            {
                filter.And();
                filter.Not();
                filter.Property(PAYMEN.PaymentRequest.Properties.PaymentRequestCode, typeof(PAYMEN.PaymentRequest).Name);
                filter.In();
                filter.ListValue();
                entityPaymentRequestCancellations.ForEach(x =>
                {
                    filter.Constant(x.PaymentRequestCode);
                    filter.Constant(x.PaymentRequestCancellationCode);
                });
                filter.EndList();
            }

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, paymentRecoveryView);

            if (paymentRecoveryView.PaymentRequests.Count > 0)
            {
                List<PAYMEN.PaymentRequest> paymentRequests = paymentRecoveryView.PaymentRequests.Cast<PAYMEN.PaymentRequest>().ToList();

                foreach (PAYMEN.PaymentRequest paymentRequest in paymentRequests)
                {
                    recoveryAmount += Convert.ToDecimal(Math.Abs(paymentRequest.TotalAmount));
                }
            }

            return recoveryAmount;
        }

        public PaymentRequestControl CreatePaymentRequestControl(PaymentRequestControl paymentRequestControl)
        {
            return ModelAssembler.CreatePaymentRequestControl((INTEN.ClmPaymentControl)DataFacadeManager.Insert(EntityAssembler.CreatePaymentRequestControl(paymentRequestControl)));
        }

        private decimal CalculateRecoveryOrSalvageAmountByChargeRequestId(int chargeRequestId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PAYMEN.PaymentRecovery.Properties.PaymentRequestCode, typeof(PAYMEN.PaymentRecovery).Name, chargeRequestId);

            SalvageRecoveryView salvageRecoveryView = new SalvageRecoveryView();
            ViewBuilder viewBuilder = new ViewBuilder("SalvageRecoveryView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, salvageRecoveryView);

            if (salvageRecoveryView.PaymentRecoveries.Any())
            {
                if (salvageRecoveryView.Recoveries.Any() || salvageRecoveryView.Salvages.Any())
                {
                    return Convert.ToDecimal(salvageRecoveryView.Recoveries.Cast<CLMEN.Recovery>().Sum(x => x.TotalAmount) + salvageRecoveryView.Salvages.Cast<CLMEN.Salvage>().Sum(x => x.EstimatedSale));
                }
            }

            return 0;
        }
    }
}
