using Sistran.Core.Application.AccountingServices.Assemblers;
using Sistran.Core.Application.AccountingServices.EEProvider.DAOs;
using Sistran.Core.Application.AccountingServices.EEProvider.Enums;
using Sistran.Core.Application.AccountingServices.EEProvider.Models;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Collect;
using Sistran.Core.Application.AccountingServices.EEProvider.Models.Payments;
using System;

namespace Sistran.Core.Application.AccountingServices.EEProvider.Business
{
    public class AccountingPaymentBusiness
    {
        public BillReport GetBillReport(int technicalTransaction)
        {
            AccountingPaymentDAO accountingPaymentDAO = new AccountingPaymentDAO();
            BillReport billReport = accountingPaymentDAO.GetBillReport(technicalTransaction);
            if (billReport != null && billReport.BranchName != "")
            {
                billReport.ClientDocumentNumber = CommonBusiness.GetTextParameter(Enums.AccountingKeys.ACC_BILL_CLIENT_DOC_NUMBER);
            }
            return billReport;
        }

        public Message ProcessAccountingCheck(Collect collect, DateTime accountingDate, int userId, int oldTechnicalTransaction, int paymentId, int statusTypeId, int bridgeAccountId)
        {
            SaveBillParameter saveBillParametersModel = new SaveBillParameter()
            {
                Collect = new Collect
                {
                    Id = collect.Id,
                    Transaction = collect.Transaction,
                    Date = accountingDate,
                    Payments = collect.Payments
                },
                TypeId = statusTypeId,
                UserId = userId,
                TechnicalTransaction = oldTechnicalTransaction,
                PaymentCode = paymentId,
                BridgeAccoutingId = bridgeAccountId,
                BridgePackageCode = CommonBusiness.GetStringParameter(AccountingKeys.ACC_RULE_PACKAGE_COLLECT)
            };

            int entryNumber = DelegateService.accountingAccountService.AccountingChecks(saveBillParametersModel.ToDTO());
            Message message = new Message()
            {
                GeneralLedgerSuccess = false
            };

            if (entryNumber > 0)
            {
                message.GeneralLedgerSuccess = true;
                message.Info = Resources.Resources.IntegrationSuccessMessage + " " + entryNumber;
            }
            if (entryNumber == 0)
            {
                message.Info = Resources.Resources.AccountingIntegrationUnbalanceEntry;
            }
            if (entryNumber == -2)
            {
                message.Info = Resources.Resources.EntryRecordingError;
            }
            return message;
        }
    }
}
