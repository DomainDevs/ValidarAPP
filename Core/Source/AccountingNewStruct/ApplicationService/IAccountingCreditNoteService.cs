using System.Collections.Generic;
using System.ServiceModel;
using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.CreditNotes;
using Sistran.Core.Application.AccountingServices.DTOs.Search;

namespace Sistran.Core.Application.AccountingServices
{
    [ServiceContract]
    public interface IAccountingCreditNoteService
    {
        /// <summary>
        /// SaveCreditNote: Grabar Nota de Crédito
        /// </summary>
        /// <param name="operationType"></param>
        /// <param name="branch"></param>
        /// <param name="prefix"></param>
        /// <param name="policy"></param>
        /// <param name="insured"></param>
        /// <returns>CreditNote</returns>
        [OperationContract]
        CreditNoteDTO SaveCreditNote(int operationType, BranchDTO branch, PrefixDTO prefix, PolicyDTO policy, IndividualDTO insured);

        /// <summary>
        /// GetCreditNote: Obtiene Nota de Crédito
        /// </summary>
        /// <param name="creditNote"></param>
        /// <returns>CreditNote</returns>
        [OperationContract]
        CreditNoteDTO GetCreditNote(CreditNoteDTO creditNote);


        /// <summary>
        /// GetCreditNotes: Obtiene todas las Notas de Crédito
        /// </summary>
        /// <returns>List<CreditNote></returns>
        [OperationContract]
        List<CreditNoteDTO> GetCreditNotes();

        /// <summary>
        /// UpdateCreditNote: Actualiza la Nota de Credito
        /// </summary>
        /// <param name="creditNote"></param>
        /// <returns>CreditNote</returns>
        [OperationContract]
        CreditNoteDTO UpdateCreditNote(CreditNoteDTO creditNote);
    }
}
