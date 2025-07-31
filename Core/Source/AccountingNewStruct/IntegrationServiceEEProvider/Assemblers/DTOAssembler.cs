using Sistran.Core.Integration.AccountingServices.DTOs.Accounting;
using System.Collections.Generic;

namespace Sistran.Core.Integration.AccountingServices.EEProvider.Assemblers
{
    public class DTOAssembler
    {
        /// <summary>
        /// Creates the components.
        /// </summary>
        /// <param name="components">The components.</param>
        /// <returns></returns>
        public static List<PremiumBaseDTO> CreatePaymentAppliedDTO(List<PaymentAppliedDTO> components)
        {
            var mapper = AutoMapperAssembler.CreateMapPaymentAppliedDTO();
            return mapper.Map<List<PaymentAppliedDTO>, List<PremiumBaseDTO>>(components);
        }
    }
}
