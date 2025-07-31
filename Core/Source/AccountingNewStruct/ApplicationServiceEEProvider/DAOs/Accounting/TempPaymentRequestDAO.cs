using Sistran.Core.Application.AccountingServices.EEProvider.Assemblers;
using AccountPayableModel = Sistran.Core.Application.AccountingServices.EEProvider.Models.AccountsPayables;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims;
using PAYMEN = Sistran.Core.Application.Claims.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Claims.PaymentRequest;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class TempPaymentRequestDAO
    {
        #region Instance variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
              

        #endregion

        #region Public Methods

        /// <summary>
        /// SaveTempPaymentRequest
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns>int</returns>
        public int SaveTempPaymentRequest(AccountPayableModel.PaymentRequest paymentRequest)
        {
            try
            {
                PaymentRequest newPaymentRequest = new PaymentRequest();
                PaymentRequest newPaymentRequestFinal = new PaymentRequest();
                newPaymentRequestFinal.Claim = new Claim();
                PaymentRequest newPaymentRequestCommon = new PaymentRequest();
                Voucher newVoucherCommon = new Voucher();
                VoucherConcept newVoucherConceptCommon = new VoucherConcept();
                AccountPayableModel.PaymentRequest headerPaymentRequest = new AccountPayableModel.PaymentRequest(); 

                int indexClaim = 0;
                int indexCoverage = 0;
                int indexAmount = 0;
                int indexVoucher = 0;

                if (paymentRequest.Id == 0)
                {
                    // Graba cabecera TempPaymentRequest
                    headerPaymentRequest = SaveClaimTempPaymentRequest(paymentRequest);
                    headerPaymentRequest.Id = newPaymentRequestCommon.Number;
                }
                else
                {
                    UpdateTotalAmountTempPaymentRequest(paymentRequest.Id, Convert.ToDecimal(paymentRequest.TotalAmount.Value));
                }
                /*TODO LFREIRE Hasta definir o implementar ClaimsService
                // Graba los detalles
                foreach (Claim claim in paymentRequest.PaymentClaim)
                {
                    Claim newClaim = new Claim();

                    // Viene de una solicitud de pagos varios (Accounting)
                    if (paymentRequest.MovementType.ConceptSource.Id != Convert.ToInt32(PaymentRequestTypes.Payment))
                    {
                        newClaim.ClaimId = claim.ClaimId;
                        newClaim = _claimService.GetClaim(newClaim);
                        claim.Branch = newClaim.Branch;
                        claim.Policy = new Policy();
                        claim.Policy.Prefix = newClaim.Policy.Prefix;
                        claim.Number = newClaim.Number;
                    }
                    else if (paymentRequest.MovementType.ConceptSource.Id == Convert.ToInt32(PaymentRequestTypes.Payment))
                    {
                        claim.Branch = new Branch() { Id = 0 };
                        claim.Policy = new Policy()
                        {
                            Prefix = new Prefix() { Id = 0 }
                        };
                        claim.Number = 0;
                    }

                    foreach (ClaimCoverage coverage in claim.Coverages)
                    {
                        foreach (EstimationType estimationType in coverage.EstimationMovementType.Estimation)
                        {
                            foreach (claims.VoucherConcept vouchers in estimationType.Estimation.VoucherConcepts)
                            {
                                //Graba TempVoucher
                                vouchers.Voucher.Id = 0;
                                newVoucherCommon = SaveTempVoucher(vouchers.Voucher);

                                //Graba TempVoucherConcepts
                                vouchers.Voucher.Id = newVoucherCommon.Id;
                                newVoucherConceptCommon = SaveTempVoucherConcept(vouchers, paymentRequest.Id);

                                if (paymentRequest.PaymentRequestInfo.PayToInfo.TaxConditions.Count > 0)
                                {
                                    int lineBusinessCode = 0;
                                    //TaxCategories
                                    if (paymentRequest.PaymentRequestInfo.Source.Id != Convert.ToInt32(CommonModelPayments.PaymentSources.PaymentRequest))  //para pagos varios no se aplica linea de negocio por lo que se pasa parámentro 0
                                    {
                                        lineBusinessCode = GetLineBusinessByCoverageId(coverage.Id);
                                    }

                                    int conditionCode = paymentRequest.PaymentRequestInfo.PayToInfo.TaxConditions[0].Taxes[0].TaxConditions[0].Id;
                                    
                                    Dictionary<int, int> taxCategories = new Dictionary<int, int>();

                                    foreach (CommonService.Models.Taxes.TaxCategory taxCategory in paymentRequest.PaymentRequestInfo.PayToInfo.TaxConditions[0].Taxes[0].TaxCategories)
                                    {
                                        foreach (CommonService.Models.Taxes.Tax tax in paymentRequest.PaymentRequestInfo.PayToInfo.TaxConditions[0].Taxes)
                                        {
                                            int key = taxCategories.Where(p => p.Key == tax.Id).FirstOrDefault().Key;

                                            if (taxCategory.Id > 0) 
                                            {
                                                // taxCategories.Add(tax.Id, tax.TaxCategories[0].Id);
                                                if (key != tax.Id)//temporal hasta analizar lo del common
                                                {
                                                   taxCategories.Add(tax.Id, taxCategory.Id);
                                                }
                                             }

                                            key = 0;
                                        }
                                    }

                                    int numbersTax = SaveTempTaxDetail(paymentRequest.Beneficiary.IndividualId,
                                                                       conditionCode,
                                                                       taxCategories,
                                                                       paymentRequest.Branch.Id,
                                                                       lineBusinessCode,
                                                                       vouchers.Voucher.ExchangeRate,
                                                                       vouchers.Value,
                                                                       paymentRequest.Id,
                                                                       vouchers.Voucher.Id
                                                                      );
                                }

                                //Graba TempPaymentRequestClaim
                                paymentRequest.PaymentClaim[indexClaim].Coverages[indexCoverage].EstimationMovementType.Estimation[indexAmount].Estimation.VoucherConcepts[indexVoucher].Id = newVoucherConceptCommon.Id;

                                newPaymentRequest = _tempPaymentRequestClaimDAO.SaveTempPaymentRequestClaim(paymentRequest, paymentRequest.PaymentClaim[indexClaim], indexCoverage, indexAmount, indexVoucher);

                                indexVoucher += 1;
                            }

                            // Grabado de Payment Recovery / Salvage
                            //////////////////////////////////////////////////////////////////
                            int[] paymentRecoveryRow = new int[3];

                            if (estimationType.Estimation.Recoveries != null)
                            {
                                foreach (var recovery in estimationType.Estimation.Recoveries)
                                {
                                    paymentRecoveryRow[0] = 0;
                                    paymentRecoveryRow[1] = paymentRequest.Id;
                                    paymentRecoveryRow[2] = recovery.Id;
                                }
                                SavePaymentRecovery(paymentRecoveryRow);
                            }

                            if (estimationType.Estimation.Salvages != null)
                            {
                                foreach (var salvage in estimationType.Estimation.Salvages)
                                {
                                    paymentRecoveryRow[0] = 0;
                                    paymentRecoveryRow[1] = paymentRequest.Id;
                                    paymentRecoveryRow[2] = salvage.Id;
                                }
                                SavePaymentSalvage(paymentRecoveryRow);
                            }

                            ///////////////////////////////////////////////////////////////////
                            indexVoucher = 0;
                            indexAmount += 1;
                        }
                        indexAmount = 0;
                        indexCoverage += 1;
                    }
                    indexCoverage = 0;
                    indexClaim += 1;
                }
                */

                return newPaymentRequest.Number;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// ConvertTempPaymentRequestToPaymentRequest
        /// </summary>
        /// <param name="tempPaymentRequestId"></param>
        /// <returns>int[]</returns>
        public int[] ConvertTempPaymentRequestToPaymentRequest(int tempPaymentRequestId)
        {
            try
            {
                /*TODO LFREIRE Haasta definir o implementar ClaimsService/
                claims.PaymentRequest paymentRequest = new claims.PaymentRequest();
                claims.PaymentRequest newPaymentRequest = new claims.PaymentRequest();

                List<CommonModelPayments.Voucher> newVoucherCommonList = new List<CommonModelPayments.Voucher>();
                CommonModelPayments.Voucher newVoucherCommon = new CommonModelPayments.Voucher();

                List<CommonModelPayments.VoucherConcept> newVoucherConceptCommonList = new List<CommonModelPayments.VoucherConcept>();
                CommonModelPayments.VoucherConcept newVoucherConceptCommon = new CommonModelPayments.VoucherConcept();

                List<CommonModelPayments.PaymentRequestTax> newTempPaymentRequestTaxList = new List<CommonModelPayments.PaymentRequestTax>();

                CommonModelPayments.PaymentRequestTax newTempPaymentRequestTax = new CommonModelPayments.PaymentRequestTax();

                CommonModelPayments.PaymentRequest commonTempPaymentRequest = new CommonModelPayments.PaymentRequest();

                // Recupero TempPaymentRequest
                commonTempPaymentRequest.Id = tempPaymentRequestId;
                paymentRequest.PaymentRequestInfo = _commonService.GetTempPaymentRequest(commonTempPaymentRequest);

                // Recupera PaymentRequestNumber
                CommonModelPayments.PaymentRequestNumber paymentRequestNumber = new CommonModelPayments.PaymentRequestNumber();
                paymentRequestNumber.Branch = new Branch() { Id = paymentRequest.PaymentRequestInfo.Branch.Id };
                paymentRequestNumber = _commonService.GetPaymentRequestNumber(paymentRequestNumber);

                // Comienza las grabaciones
                if (paymentRequestNumber != null)
                {
                    paymentRequest.PaymentRequestInfo.Number.Number = paymentRequestNumber.Number;

                    paymentRequest.PaymentRequestInfo.Id = 0;

                    // Graba cabecera en PaymentRequest
                    newPaymentRequest.PaymentRequestInfo = _commonService.SavePaymentRequest(paymentRequest.PaymentRequestInfo);

                    paymentRequest.PaymentRequestInfo.Id = tempPaymentRequestId;

                    // Recupera VoucherConcepts
                    newVoucherConceptCommonList = _commonService.GetTempVoucherConceptByTempPaymentRequestId(paymentRequest.PaymentRequestInfo.Id);

                    int startVoucher = newVoucherConceptCommonList[0].Voucher.Id;
                    int endVoucher = newVoucherConceptCommonList[newVoucherConceptCommonList.Count - 1].Voucher.Id;

                    // Recupera Vouchers
                    newVoucherCommonList = _commonService.GetTempVoucherByRangeVoucherId(startVoucher, endVoucher);

                    int index = 0;

                    List<int> voucherConceptIds = new List<int>();

                    foreach (CommonModelPayments.VoucherConcept voucherConcept in newVoucherConceptCommonList)
                    {
                        newVoucherCommonList[index].Id = 0;
                        // Graba Voucher en Reales
                        newVoucherCommon = _commonService.SaveVoucher(newVoucherCommonList[index]);

                        voucherConcept.Voucher.Id = Convert.ToInt32(newVoucherCommon.Id);

                        // Graba VoucherConcepts en Reales

                        voucherConcept.Id = 0;
                        newVoucherConceptCommon = _commonService.SaveVoucherConcept(voucherConcept, newPaymentRequest.PaymentRequestInfo.Id);

                        voucherConceptIds.Add(newVoucherConceptCommon.Id);

                        index++;
                    }

                    // Recupero TempPaymentRequestClaim 
                    ArrayList paymentRequestClaim = _tempPaymentRequestClaimDAO.GetTempPaymentRequestClaimByPaymentRequestId(paymentRequest.PaymentRequestInfo.Id);

                    index = 0;

                    // Graba detalles en PaymentRequestClaim
                    foreach (PAYMEN.TempPaymentRequestClaim paymentRequestClaimRow in paymentRequestClaim)
                    {
                        int[] paymentRequestRow = new int[9];
                        paymentRequestRow[0] = newPaymentRequest.PaymentRequestInfo.Id;
                        paymentRequestRow[1] = paymentRequestClaimRow.ClaimCode;
                        paymentRequestRow[2] = paymentRequestClaimRow.SubClaim;
                        paymentRequestRow[3] = paymentRequestClaimRow.EstimationTypeCode;
                        paymentRequestRow[4] = voucherConceptIds[index];


                        // Validación para cuando venga de una solicitud de pago vario poner nulos 
                        if (paymentRequest.PaymentRequestInfo.Source.Id == Convert.ToInt16(CommonModelPayments.PaymentSources.PaymentRequest))
                        {
                            paymentRequestRow[5] = -1;
                            paymentRequestRow[6] = -1;
                            paymentRequestRow[7] = -1;
                            paymentRequestRow[8] = -1;
                        }
                        else
                        {
                            paymentRequestRow[5] = Convert.ToInt32(paymentRequestClaimRow.PaymentTypeCode);
                            paymentRequestRow[6] = Convert.ToInt32(paymentRequestClaimRow.BranchCode);
                            paymentRequestRow[7] = Convert.ToInt32(paymentRequestClaimRow.PrefixCode);
                            paymentRequestRow[8] = Convert.ToInt32(paymentRequestClaimRow.ClaimNumber);
                        }

                        _paymentRequestClaimDAO.SavePaymentRequestClaim(paymentRequestRow);

                        index++;
                    }

                    // Recupera TempPaymentRequestTax
                    newTempPaymentRequestTaxList = _commonService.GetTempPaymentRequestTaxByTempPaymentRequestId(paymentRequest.PaymentRequestInfo.Id);

                    // Graba TempPaymentRequestTax
                    foreach (PaymentRequestTax tempPaymentRequestTax in newTempPaymentRequestTaxList)
                    {
                        tempPaymentRequestTax.PaymentRequestId = newPaymentRequest.PaymentRequestInfo.Id;
                        newTempPaymentRequestTax = _commonService.SavePaymentRequestTax(tempPaymentRequestTax);
                    }

                    //Actualiza PaymentRequestNumber
                    paymentRequestNumber.Number++;
                    paymentRequestNumber = _commonService.UpdatePaymentRequestNumber(paymentRequestNumber);

                    // Función que permite empatar El Recobro/Salvamento con el Id del Pago real para que pueda ser encontrado 
                    // desde la búsqueda de Solicitud de Pago Siniestros del proyecto Billing
                    //21/08/2013
                    //DDV

                    UpdatePaymentRecoveryByPaymentRequestId(tempPaymentRequestId, newPaymentRequest.PaymentRequestInfo.Id);

                    //UpdatePaymentRecoveryByPaymentRequestId si devuelve true es porque el pago tuvo un recobro o un salvamento
                    //que actualizar si es false no tuvo ningun recobro/salvamento esa solicitud de cobro

                    // Borra Temporales
                    DeleteTempPaymentRequest(tempPaymentRequestId);

                    int[] result = { newPaymentRequest.PaymentRequestInfo.Id, newPaymentRequest.PaymentRequestInfo.Number.Number, paymentRequest.PaymentRequestInfo.Source.Id };
                    return result;
                }
                else // si paymentRequestNumber es null quiere decir que la sucursal no está parametrizada en PAYM.PAYMENT_REQUEST_NUMBER
                {
                    return null;
                }
                */
                return new int[3];
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #region PaymentRecovery

        /// <summary>
        /// UpdatePaymentRecoveryByPaymentRequestId
        /// Función que permite empatar el recbro/salvamento con el Id del Pago real para que pueda ser encontrado 
        /// desde la búsqueda de Solicitud de Pago Siniestros del proyecto Accounting
        /// </summary>
        /// <param name="tempPaymentRequestId"></param>
        /// <param name="paymentRequestId"></param>
        /// <returns>bool</returns>
        private bool UpdatePaymentRecoveryByPaymentRequestId(int tempPaymentRequestId, int paymentRequestId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(PAYMEN.PaymentRecovery.Properties.PaymentRequestCode, tempPaymentRequestId);

                BusinessCollection businessCollection = new
                    BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects
                                          (typeof(PAYMEN.PaymentRecovery), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0) // Lo encuentra
                {
                    foreach (PAYMEN.PaymentRecovery paymentRecovery in businessCollection.OfType<PAYMEN.PaymentRecovery>())
                    {
                        paymentRecovery.PaymentRequestCode = paymentRequestId;
                        // Realizar las operaciones con los entities utilizando DAF
                        _dataFacadeManager.GetDataFacade().UpdateObject(paymentRecovery);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (BusinessException)
            {
                return false;
            }
        }

        /// <summary>
        /// SavePaymentRecovery
        /// </summary>
        /// <param name="paymentRecovery"></param>
        /// <returns></returns>
        private static void SavePaymentRecovery(Array paymentRecovery)
        {
            try
            {
                PAYMEN.PaymentRecovery paymentRecoveryEntity = Assemblers.EntityAssembler.CreatePaymentRecovery(paymentRecovery);

                _dataFacadeManager.GetDataFacade().InsertObject(paymentRecoveryEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SavePaymentRecovery
        /// </summary>
        /// <param name="paymentRecovery"></param>
        /// <returns></returns>
        private static void SavePaymentSalvage(Array paymentRecovery)
        {
            try
            {
                PAYMEN.PaymentRecovery paymentSalvageEntity = Assemblers.EntityAssembler.CreatePaymentSalvage(paymentRecovery);

                _dataFacadeManager.GetDataFacade().InsertObject(paymentSalvageEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteTempPaymentRequest
        /// </summary>
        /// <param name="tempPaymentRequestId"></param>
        /// <returns></returns>
        private void DeleteTempPaymentRequest(int tempPaymentRequestId)
        {
            try
            {
                /*TODO LFREIRE Haasta definir o implementar ClaimsService
                List<AccountPayableModel.Voucher> vouchers = new List<AccountPayableModel.Voucher>();
                List<AccountPayableModel.VoucherConcept> voucherConcepts = new List<AccountPayableModel.VoucherConcept>();

                // Borra TempPaymentRequest
                _commonService.DeleteTempPaymentRequest(tempPaymentRequestId);

                // Borra TempPaymentRequestClaim
                _tempPaymentRequestClaimDAO.DeleteTempPaymentRequestClaim(tempPaymentRequestId);

                // Recupera VoucherConcepts
                voucherConcepts = _commonService.GetTempVoucherConceptByTempPaymentRequestId(tempPaymentRequestId);

                // Borra TempVoucherConcepts
                new AccountsPayableServiceEEProvider().DeleteTempVoucherConceptByTempPaymentRequest(tempPaymentRequestId);

                int startVoucher = 1;
                int endVoucher = 1;

                //int startVoucher = newVoucherConceptCommonList[0].Voucher.Id; //TODO ACE-650 'VoucherConcept' does not contain a definition for 'Voucher'
                //int endVoucher = newVoucherConceptCommonList[newVoucherConceptCommonList.Count - 1].Voucher.Id; //TODO ACE-650 'VoucherConcept' does not contain a definition for 'Voucher'

                // Recupera TempVouchers
                vouchers = new AccountsPayableServiceEEProvider().GetTempVoucherByRangeVoucherId(startVoucher, endVoucher);

                // Borra TempVouchers
                foreach (AccountPayableModel.Voucher voucher in vouchers)
                {
                    new AccountsPayableServiceEEProvider().DeleteTempVoucherByRangeVoucherId(startVoucher, endVoucher);
                }

                // Borra TempPaymentRequestTax
                _commonService.DeleteTempPaymentRequestTaxByTempPaymentRequest(tempPaymentRequestId);
                */
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// GetLineBusinessByCoverageId
        /// </summary>
        /// <param name="coverageId"></param>
        /// <returns></returns>
        private int GetLineBusinessByCoverageId(int coverageId)
        {
            /*TODO: LFREIRE No existe el método GetLineBusinessByCoverageId
            return _underwritingService.GetLineBusinessByCoverageId(coverage.Id);
            */
            return 0;
        }

        /// <summary>
        /// SaveClaimTempPaymentRequest
        /// </summary>
        /// <param name="paymentRequest"></param>
        /// <returns></returns>
        private AccountPayableModel.PaymentRequest SaveClaimTempPaymentRequest(AccountPayableModel.PaymentRequest paymentRequest)
        {
            try
            {
                /*TODO LFREIRE Haasta definir o implementar ClaimsService
                PAYMEN.TempPaymentRequest tempPaymentRequestEntity;

                //Validación para cuando no venga de una solicitud de pago vario
                if (paymentRequest.MovementType.ConceptSource.Id != Convert.ToInt16(AccountPayableModel.PaymentRequestTypes.Payment))
                {
                    paymentRequest.Company = new Company() { IndividualId = 0 };
                    paymentRequest.SalePoint = new SalePoint() { Id = 0 };

                    //convertir de model a entity
                    tempPaymentRequestEntity = EntityAssembler.CreateTempPaymentRequest(paymentRequest);

                    tempPaymentRequestEntity.CompanyCode = null;
                    tempPaymentRequestEntity.SalePointCode = null;
                }
                else
                {
                    tempPaymentRequestEntity = EntityAssembler.CreateTempPaymentRequest(paymentRequest);
                }

                //realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(tempPaymentRequestEntity);

                //return del model
                return ModelAssembler.CreateTempPaymentRequest(tempPaymentRequestEntity);
                */
                return new AccountPayableModel.PaymentRequest();
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateTotalAmountTempPaymentRequest
        /// </summary>
        /// <param name="paymentRequestId"></param>
        /// <param name="amount"></param>
        public void UpdateTotalAmountTempPaymentRequest(int paymentRequestId, decimal amount)
        {
            try
            {
                /*TODO LFREIRE Haasta definir o implementar ClaimsService
                //Crea la Primary key con el codigo de la entidad
                PrimaryKey primaryKey = PAYMEN.TempPaymentRequest.CreatePrimaryKey(paymentRequestId);

                //Encuentra el objeto en referencia a la llave primaria
                PAYMEN.TempPaymentRequest tempPaymentRequestEntity = (PAYMEN.TempPaymentRequest)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                tempPaymentRequestEntity.TotalAmount = tempPaymentRequestEntity.TotalAmount + Convert.ToDecimal(amount);

                //realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(tempPaymentRequestEntity);
                */
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SaveTempVoucher
        /// </summary>
        /// <param name="voucher"></param>
        /// <returns>Voucher</returns>
        private AccountPayableModel.Voucher SaveTempVoucher(AccountPayableModel.Voucher voucher)
        {
            try
            {
                COMMEN.TempVoucher tempVoucherEntity = EntityAssembler.CreateTempVoucher(voucher);

                _dataFacadeManager.GetDataFacade().InsertObject(tempVoucherEntity);

                return ModelAssembler.CreateTempVoucher(tempVoucherEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SaveTempVoucherConcept
        /// </summary>
        /// <param name="voucherConcept"></param>
        /// <param name="tempPaymentRequestId"></param>
        /// <returns>VoucherConcept</returns>
        public AccountPayableModel.VoucherConcept SaveTempVoucherConcept(AccountPayableModel.VoucherConcept voucherConcept, int tempPaymentRequestId)
        {
            try
            {
                /*TODO LFREIRE Haasta definir o implementar ClaimsService
                PAYMEN.TempVoucherConcept voucherConceptEntity = EntityAssembler.CreateTempVoucherConcept(voucherConcept, tempPaymentRequestId);

                _dataFacadeManager.GetDataFacade().InsertObject(voucherConceptEntity);

                return ModelAssembler.CreateTempVoucherConcept(voucherConceptEntity);
                */
                return new AccountPayableModel.VoucherConcept();
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SaveTempTaxDetail
        /// </summary>
        /// <param name="individualId">individualId</param>
        /// <param name="branchCode">branchCode</param>
        /// <param name="conditionCode">conditionCode</param>
        /// <param name="categoryCode">categoryCode</param>
        /// <param name="lineBusinessCode">lineBusinessCode</param>
        /// <param name="exchangeRate">exchangeRate</param>
        /// <param name="amount">amount</param>
        /// <param name="paymentRequestCode">paymentRequestCode</param>
        /// <param name="voucherConceptCode">voucherConceptCode</param>
        /// <returns>int</returns>
        private int SaveTempTaxDetail(int individualId, int conditionCode, Dictionary<int, int> categories, int branchCode, int lineBusinessCode,
                                      decimal exchangeRate, decimal amount, int tempPaymentRequestCode, int voucherConceptCode)
        {
            try
            {
                /*TODO LFREIRE Haasta definir o implementar ClaimsService
                TaxProvider taxProvider = new TaxProvider(RequestProcessor);
                DataTable dt = taxProvider.GetTax(individualId, conditionCode, categories, branchCode, lineBusinessCode, exchangeRate, amount);

                PaymentRequestTax tempPaymentRequestTax = new PaymentRequestTax();
                tempPaymentRequestTax.Id = 0;
                int taxCategoryCode = 0;
                int taxConditionCode = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    taxCategoryCode = 0;
                    if (dr["TaxCategoryCode"] != System.DBNull.Value)
                    {
                        taxCategoryCode = Convert.ToInt32(dr["TaxCategoryCode"]);
                    }

                    taxConditionCode = 0;
                    if (dr["TaxConditionCode"] != System.DBNull.Value)
                    {
                        taxConditionCode = Convert.ToInt32(dr["TaxConditionCode"]);
                    }


                    tempPaymentRequestTax = SaveTempPaymentRequestTax(tempPaymentRequestCode,
                                                                Convert.ToInt32(dr["TaxCode"]),
                                                                taxCategoryCode,
                                                                taxConditionCode,
                                                                voucherConceptCode,
                                                                Convert.ToDecimal(dr["Rate"]),
                                                                Convert.ToDecimal(dr["TaxValue"]),
                                                                Convert.ToDecimal(dr["TaxAmountBase"])
                                                                );
                }

                return dt.Rows.Count;
                */
                return 0;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion Private Methods

    }
}
