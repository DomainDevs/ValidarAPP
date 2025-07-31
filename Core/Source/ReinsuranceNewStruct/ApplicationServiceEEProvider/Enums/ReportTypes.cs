using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums
{
    [Flags]
    public enum ReportTypes
    {
        #region INCONSISTENCIA DE PRIMA
        [EnumMember]
        ID_INCONSISTENCY_PREMIUM,
        [EnumMember]
        ID_FORMAT_INCONSISTENCY_PREMIUM,
        [EnumMember]
        PROCEDURE_INCONSISTENCY_PREMIUM,
        [EnumMember]
        TEMPLATE_NAME_INCONSISTENCY_PREMIUM,
        [EnumMember]
        PROCEDURE_GET_INCONSISTENCY_PREMIUM,
        #endregion

        #region BORDER AUX
        [EnumMember]
        ID_BORDER_AUX,
        [EnumMember]
        ID_FORMAT_BORDER_AUX,
        [EnumMember]
        PROCEDURE_BORDER_AUX,
        [EnumMember]
        TEMPLATE_NAME_BORDER_AUX,
        [EnumMember]
        PROCEDURE_GET_BORDER_AUX,
        #endregion

        #region TECHNICAL ACCOUNT 
        [EnumMember]
        ID_TECHNICAL_ACCOUNT,
        [EnumMember]
        ID_FORMAT_TECHNICAL_ACCOUNT,
        [EnumMember]
        PROCEDURE_NAME_TECHNICAL_ACCOUNT,
        [EnumMember]
        TEMPLATE_NAME_TECHNICAL_ACCOUNT,
        [EnumMember]
        PROCEDURE_GET_TECHNICAL_ACCOUNT,
        
        #endregion

        #region SINIESTROS
        [EnumMember]
        ID_SINISTER,
        [EnumMember]
        ID_FORMAT_SINISTER,
        [EnumMember]
        DESCRIPTION_SINISTER,
        [EnumMember]
        PROCEDURE_NAME_SINISTER,
        [EnumMember]
        TEMPLATE_NAME_SINISTER,
        [EnumMember]
        PROCEDURE_GET_SINISTER,
        #endregion

        #region PAGER
        [EnumMember]
        ID_PAGER,
        [EnumMember]
        ID_FORMAT_PAGER,
        [EnumMember]
        DESCRIPTION_PAGER,
        [EnumMember]
        TEMPLATE_NAME_PAGER,
        [EnumMember]
        PROCEDURE_NAME_PAGER,
        [EnumMember]
        PROCEDURE_GET_PAGER,

        #endregion

        #region SLIPS_FACULTATIVOS
        [EnumMember]
        ID_FACULTATIVE,
        [EnumMember]
        ID_FORMAT_FACULTATIVE,
        [EnumMember]
        DESCRIPTION_FACULTATIVE,
        [EnumMember]
        PROCEDURE_NAME_FACULTATIVE,
        [EnumMember]
        TEMPLATE_NAME_FACULTATIVE,
        [EnumMember]
        PROCEDURE_GET_FACULTATIVE,
        #endregion

        #region BORRADOR CUENTA TÉCNICA
        [EnumMember]
        ID_ACCOUNT_TECNICAL,
        [EnumMember]
        ID_FORMAT_DRAFT_TECHNICAL_ACCOUNT,
        [EnumMember]
        DESCRIPTION_DRAFT_TECHNICAL_ACCOUNT,
        [EnumMember]
        TEMPLATE_NAME_DRAFT_TECHNICAL_ACCOUNT,
        [EnumMember]
        PROCEDURE_NAME_DRAFT_TECHNICAL_ACCOUNT,
        [EnumMember]
        PROCEDURE_GET_DRAFT_TECHNICAL_ACCOUNT,
        #endregion

        #region BORDER AUX SINIESTRO
        [EnumMember]
        ID_BORDER,
        [EnumMember]
        ID_FORMAT_BORDER,
        [EnumMember]
        DESCRIPTION_BORDER,
        [EnumMember]
        TEMPLATE_NAME_BORDER,
        [EnumMember]
        PROCEDURE_NAME_BORDER,
        [EnumMember]
        PROCEDURE_GET_BORDER,
        #endregion
        
        #region BORDER AUX PAGOS
        [EnumMember]
        ID_BORDER_PAGER,
        [EnumMember]
        ID_FORMAT_BORDER_PAGER,
        [EnumMember]
        TEMPLATE_NAME_BORDER_PAGER,
        [EnumMember]
        PROCEDURE_NAME_BORDER_PAGER,
        [EnumMember]
        PROCEDURE_GET_BORDER_PAGER,
        #endregion

        #region BORRADOR CUENTA CORRIENTE
        [EnumMember]
        ID_CURRENT_ACCOUNT,
        [EnumMember]
        ID_FORMAT_CURRENT_ACCOUNT,
        [EnumMember]
        TEMPLATE_NAME_CURRENT_ACCOUNT,
        [EnumMember]
        PROCEDURE_NAME_CURRENT_ACCOUNT,
        [EnumMember]
        PROCEDURE_GET_CURRENT_ACCOUNT,
        #endregion

        #region CUENTA CORRIENTE
        [EnumMember]
        ID_ACCOUNT,
        [EnumMember]
        DESCRIPTION_ACCOUNT,
        [EnumMember]
        TEMPLATE_NAME_ACCOUNT,
        [EnumMember]
        PROCEDURE_NAME_ACCOUNT,
        [EnumMember]
        PROCEDURE_GET_ACCOUNT,
        [EnumMember]
        ID_FORMAT_ACCOUNT,        
        #endregion

        [EnumMember]
        MODULE_DATE_REINSURANCE,
        [EnumMember]
        PAGE_SIZE_REPORT,
        [EnumMember]
        ENABLED_GENERAL_LEDGER,
        [EnumMember]
        TRANSACTION_NUMBER,
        [EnumMember]
        REINSURANCE_MODULE,
        [EnumMember]
        REINSURANCE_CLOSING,
        [EnumMember]
        JOURNAL_ENTRY_TRANSACTION_NUMBER,
    }
}
