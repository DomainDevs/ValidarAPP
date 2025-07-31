using AutoMapper;
using Sistran.Core.Application.FinancialPlanServices.DTOs;
using Sistran.Core.Application.FinancialPlanServices.EEProvider.Models;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs;
using FPLANEN = Sistran.Core.Application.FinancialPlan.Entities;
namespace Sistran.Core.Application.FinancialPlanServices.EEProvider.Assemblers
{
    public class AutoMapperAssembler
    {
        #region  Plan financiero
        /// <summary>
        /// Creates the map financial plan.
        /// </summary>
        /// <returns></returns>
        public static IMapper CreateMapFinancialPlan()
        {
            IMapper config = MapperCache.GetMapper<FinancialPlanDTO, QuotaPlanDTO>(cfg =>
             {
                 cfg.CreateMap<FinancialPlanDTO, QuotaPlanDTO>()
                 .ForMember(d => d.OriginalStateQuota, o => o.MapFrom(c => c.StateQuota));
             });
            return config;
        }
        #endregion Plan financiero
        #region  Cuotas
        /// <summary>
        /// Creates the map financial plan.
        /// </summary>
        /// <returns></returns>
        public static IMapper CreateMapPayerPayment()
        {
            IMapper config = MapperCache.GetMapper<QuotaPlanDTO, FinPayerPaymentModel>(cfg =>
            {
                cfg.CreateMap<QuotaPlanDTO, FinPayerPaymentModel>()
                .ForMember(d => d.PaymentNumber, o => o.MapFrom(c => c.Number))
                .ForMember(d => d.PaymentState, o => o.MapFrom(c => c.StateQuota))
                .ForMember(d => d.OriginalPaymentState, o => o.MapFrom(c => c.OriginalStateQuota));
            });
            return config;
        }
        #endregion Cuotas

        #region  Plan de Pago
        /// <summary>
        /// Creates the map fin payer.
        /// </summary>
        /// <returns></returns>
        public static IMapper CreateMapFinPayer()
        {
            IMapper config = MapperCache.GetMapper<FinPayerModel, FPLANEN.FinPayer>(cfg =>
            {
                cfg.CreateMap<FinPayerModel, FPLANEN.FinPayer>()
                .ForMember(d => d.PayerId, o => o.MapFrom(c => c.IndividualId))
                .ForMember(d => d.IsByPaymentFinancial, o => o.MapFrom(c => c.isByPaymentUpdate))
                .ForMember(d => d.PaymentScheduleId, o => o.MapFrom(c => c.PaymentPlanId))
                .ForMember(d => d.PaymentMethodCode, o => o.MapFrom(c => c.PaymentMethodId))
                .ForMember(d => d.ReasonChange, o => o.MapFrom(c => c.ReasonforChange));
            });
            return config;
        }
        #endregion Plan de Pago
        #region FinPayerPayment
        /// <summary>
        /// Creates the map FinPayerPayment
        /// </summary>
        /// <returns></returns>
        public static IMapper CreateMapFinPayerPayment()
        {
            IMapper config = MapperCache.GetMapper<FinPayerPaymentModel, FPLANEN.FinPayerPayment>(cfg =>
            {
                cfg.CreateMap<FinPayerPaymentModel, FPLANEN.FinPayerPayment>()
                .ForMember(d => d.PaymentNum, o => o.MapFrom(c => c.PaymentNumber))
                .ForMember(d => d.PayExpDate, o => o.MapFrom(c => c.ExpirationDate))
                .ForMember(d => d.PaymentPercentage, o => o.MapFrom(c => c.PaymentPct));
            });
            return config;
        }
        #endregion FinPayerPayment
        #region CreateFinPayerPaymentComp
        public static IMapper CreateFinPayerPaymentComp()
        {
            IMapper config = MapperCache.GetMapper<FinPayerPaymentCompModel, FPLANEN.FinPayerPaymentComp>(cfg =>
            {
                cfg.CreateMap<FinPayerPaymentCompModel, FPLANEN.FinPayerPaymentComp>()
                .ForMember(d => d.PayerPaymentId, o => o.MapFrom(c => c.PaymentId))
                .ForMember(d => d.PaymentPercentage, o => o.MapFrom(c => c.PaymentPct))
                .ForMember(d => d.ComponentCode, o => o.MapFrom(c => c.ComponentId));
            });
            return config;
        }
        #endregion CreateFinPayerPaymentComp
        #region CreateFinPayerPaymentCompLbsb
        public static IMapper CreateFinPayerPaymentCompLbsb()
        {
            IMapper config = MapperCache.GetMapper<FinPayerPaymentLbSbModel, FPLANEN.FinPayerPaymentCompLbsb>(cfg =>
            {
                cfg.CreateMap<FinPayerPaymentLbSbModel, FPLANEN.FinPayerPaymentCompLbsb>()
                .ForMember(d => d.PayerPaymentId, o => o.MapFrom(c => c.PaymentId))
                 .ForMember(d => d.ComponentCode, o => o.MapFrom(c => c.ComponentId))
                 .ForMember(d => d.LineBusinessCode, o => o.MapFrom(c => c.LineBussinesId))
                 .ForMember(d => d.SubLineBusinessCode, o => o.MapFrom(c => c.SubLineBussinesId));
            });
            return config;
        }
        #endregion CreateFinPayerPaymentCompLbsb
        #region Plan base
        /// <summary>
        /// Creates the map fin payer.
        /// </summary>
        /// <returns></returns>
        public static IMapper CreateMapFinPayerModel()
        {
            IMapper config = MapperCache.GetMapper<FinanPlanModel, FinPayerModel>(cfg =>
             {
                 cfg.CreateMap<FinanPlanModel, FinPayerModel>();
             });
            return config;
        }
        #endregion Plan de Pago
    }
}
