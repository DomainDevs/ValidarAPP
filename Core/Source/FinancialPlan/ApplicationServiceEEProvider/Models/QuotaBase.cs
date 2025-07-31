using Sistran.Core.Application.FinancialPlanServices.DTOs;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using System;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
namespace Sistran.Core.Application.FinancialPlanServices.EEProvider.Models
{
    /// <summary>
    /// Recuotificacion
    /// </summary>
    internal class QuotaBase
    {
        internal int InitialFee { get; set; }

        internal DateTime StartDate { get; set; }

        internal int PaymentPlanId { get; set; }

        internal List<ComponentTypeDTO> ComponentTypes { get; set; }

        internal List<QuotaPlanDTO> Quotas { get; set; }

        internal List<PaymentDistributionDTO> PaymentDistributions { get; set; }

        internal ComponentBaseDTO ComponentBaseDTO { get; set; }
    }
}
