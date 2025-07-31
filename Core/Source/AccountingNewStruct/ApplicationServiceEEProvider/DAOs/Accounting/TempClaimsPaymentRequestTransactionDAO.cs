//System
using System;
using System.Collections.Generic;
using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
//Sistran Core
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Imputations;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs
{
    class TempClaimsPaymentRequestTransactionDAO
    {
        readonly TempClaimPaymentRequestDAO _tempClaimPaymentRequestDAO = new TempClaimPaymentRequestDAO();

        /// <summary>
        /// GetTempClaimsPaymentRequestTransactionByTempImputationId
        /// </summary>
        /// <param name="tempImputationId"></param>
        /// <param name="typePayment"></param>
        /// <returns>ClaimsPaymentRequestTransaction</returns>
        public ClaimsPaymentRequestTransaction GetTempClaimsPaymentRequestTransactionByTempImputationId(int tempImputationId, int typePayment)
        {
            decimal debits = 0;
            decimal credits = 0;

            ClaimsPaymentRequestTransaction tempClaimsPaymentRequestTransaction = new ClaimsPaymentRequestTransaction();
            tempClaimsPaymentRequestTransaction.Id = tempImputationId;

            List<ACCOUNTINGEN.TempClaimPaymentReqTrans> tempClaimPayments = _tempClaimPaymentRequestDAO.GetTempClaimPayment(0, 0, tempImputationId, typePayment);

            if (typePayment == 2) //Pagos no varios
            {
                foreach (ACCOUNTINGEN.TempClaimPaymentReqTrans tempClaimPaymentEntity in tempClaimPayments)
                {

                    switch (tempClaimPaymentEntity.RequestType)
                    {
                        case 1:
                            {
                                debits = debits + Convert.ToDecimal(tempClaimPaymentEntity.Amount);
                            }
                            break;
                        case 2:
                            {
                                credits = credits + Convert.ToDecimal(tempClaimPaymentEntity.Amount);
                            }
                            break;
                        case 3:
                            {
                                credits = credits + Convert.ToDecimal(tempClaimPaymentEntity.Amount);
                            }
                            break;
                    }
                }
            }
            if (typePayment == 1) // Pagos varios
            {
                foreach (ACCOUNTINGEN.TempClaimPaymentReqTrans tempClaimPaymentEntity in tempClaimPayments)
                {
                    if (tempClaimPaymentEntity.Amount < 0)  // Si es negativo es credito
                    {
                        credits = credits + (Convert.ToDecimal(tempClaimPaymentEntity.Amount) * -1);
                    }
                    else
                    {
                        debits = debits + Convert.ToDecimal(tempClaimPaymentEntity.Amount);
                    }
                }
            }

            tempClaimsPaymentRequestTransaction.TotalCredit = new Amount();
            tempClaimsPaymentRequestTransaction.TotalCredit.Value = credits;
            tempClaimsPaymentRequestTransaction.TotalDebit = new Amount();
            tempClaimsPaymentRequestTransaction.TotalDebit.Value = debits;

            return tempClaimsPaymentRequestTransaction;
        }
    }
}
