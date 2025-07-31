namespace Sistran.Core.Application.FinancialPlanServices.EEProvider.Assemblers
{
    using Sistran.Core.Application.FinancialPlanServices.DTOs;
    using Sistran.Core.Application.FinancialPlanServices.EEProvider.Models;
    using System.Collections.Generic;
    using FPLANEN = Sistran.Core.Application.FinancialPlan.Entities;
    public class EntityAssembler
    {
        #region pagador

        /// <summary>
        /// Crear Una Entidad de Recuotificacion
        /// </summary>
        /// <param name="finPayerModel">The fin payer model.</param>
        /// <returns></returns>
        public static FPLANEN.FinPayer CreateFinPayer(FinPayerModel finPayerModel)
        {
            var Immaper = AutoMapperAssembler.CreateMapFinPayer();
            return Immaper.Map<FinPayerModel, FPLANEN.FinPayer>(finPayerModel);
        }
        #endregion pagador
        public static FPLANEN.FinPayerPayment CreateFinPayerPayment(FinPayerPaymentModel quotaPlan)
        {
            var Immaper = AutoMapperAssembler.CreateMapFinPayerPayment();
            return Immaper.Map<FinPayerPaymentModel, FPLANEN.FinPayerPayment>(quotaPlan);
        }
        public static List<FPLANEN.FinPayerPayment> CreateFinPayerPayments(List<FinPayerPaymentModel> quotaPlan)
        {
            var Immaper = AutoMapperAssembler.CreateMapFinPayerPayment();
            return Immaper.Map<List<FinPayerPaymentModel>, List<FPLANEN.FinPayerPayment>>(quotaPlan);
        }

        public static List<FPLANEN.FinPayerPaymentComp> CreateFinPayerPaymentComp(List<FinPayerPaymentCompModel> quotaPlan)
        {
            var Immaper = AutoMapperAssembler.CreateFinPayerPaymentComp();
            return Immaper.Map<List<FinPayerPaymentCompModel>, List<FPLANEN.FinPayerPaymentComp>>(quotaPlan);
        }

        public static List<FPLANEN.FinPayerPaymentCompLbsb> CreateFinPayerPaymentCompLbsb(List<FinPayerPaymentLbSbModel> quotaPlan)
        {
            var Immaper = AutoMapperAssembler.CreateFinPayerPaymentCompLbsb();
            return Immaper.Map<List<FinPayerPaymentLbSbModel>, List<FPLANEN.FinPayerPaymentCompLbsb>>(quotaPlan);
        }
    }
}
