using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.AccountingServices.Enums
{
    [DataContract]
    [Flags]
    public enum MovementTypes
    {
        [EnumMember]
        [Description("Imputación")]
        Imputation = 0,
        [EnumMember]
        [Description("Primas por Cobrar")]
        PremiumReceivable = 1,
        [EnumMember]
        [Description("Primas en Depósito")]
        DepositPremium=2,
        [EnumMember]
        [Description("Comisiones Descontadas")]
        DiscountedCommission = 3,
        [EnumMember]
        [Description("Cta. Cte. Agentes")]
        BrokerCheckingAccount = 4,
        [EnumMember]
        [Description("Cta. Cte. Coaseguros")]
        CoinsuranceCheckingAccount = 5,
        [EnumMember]
        [Description("Cta. Cte. Reaseguros")]
        ReinsuranceCheckingAccount = 6,
        [EnumMember]
        [Description("Contabilidad")]
        DailyAccounting = 7,
        [EnumMember]
        [Description("Solicitud Pagos")]
        PaymentRequest = 8
    }
}
