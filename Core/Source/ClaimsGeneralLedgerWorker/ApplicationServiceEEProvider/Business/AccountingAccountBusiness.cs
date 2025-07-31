using GLMO = Sistran.Core.Application.GeneralLedgerServices.DTOs;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using Sistran.Core.Framework.BAF;

namespace Sistran.Core.Application.ClaimsGeneralLedgerWorkerServices.EEProvider.Business
{
    public static class AccountingAccountBusiness
    {
        internal static GLMO.AccountingAccountDTO ValidateTextReplaceInAccountingAccountNumber(this GLMO.AccountingAccountDTO accountingAccountDTO, int prefixId)
        {
            if (Regex.Matches(accountingAccountDTO.Number, @"[a-zA-Z]").Count > 0)
            {
                if (accountingAccountDTO.Number.Contains("RRR"))
                {
                    string numberAccount;
                    CommonService.Models.LineBusiness lineBusiness = DelegateService.commonServiceCore.GetLineBusinessByPrefixId(prefixId).FirstOrDefault();

                    if (lineBusiness != null)
                    {
                        string lineBusinessCode = Convert.ToString(lineBusiness.Id);
                        lineBusinessCode = lineBusinessCode.PadLeft(3,'0');
                        
                        accountingAccountDTO.Description = string.Empty;
                        accountingAccountDTO.Number = accountingAccountDTO.Number.Replace("RRR", lineBusinessCode);
                    }

                    numberAccount = accountingAccountDTO.Number;

                    accountingAccountDTO = DelegateService.generalLedgerService.GetAccountingAccountsByNumberDescription(accountingAccountDTO).FirstOrDefault();

                    if (accountingAccountDTO == null)
                    {
                        // No encontró la cuenta contable
                        throw new BusinessException(string.Format(Resources.Resources.AccountingAccountNotFound, numberAccount));
                    }
                }
            }

            return accountingAccountDTO;
        }
    }
}
