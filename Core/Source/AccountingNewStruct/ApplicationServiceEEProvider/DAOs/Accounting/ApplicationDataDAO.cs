using Sistran.Core.Application.AccountingServices.DTOs;
using Sistran.Core.Application.AccountingServices.DTOs.Filter;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.Queries;
using System;
using System.Data;
using System.Linq;
using ACCEN = Sistran.Core.Application.Accounting.Entities;
namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting
{
    public class ApplicationDataDAO
    {
        #region validacion temporales
        /// <summary>
        ///   Permite validar si la cuota a aplicar ya está siendo utilizada
        /// </summary>
        /// <returns></returns>
        public static TemporalPremiumDTO ValidatePremiumTemporal(PremiumFilterDTO premiumFilterDTO)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCEN.TempApplicationPremium.Properties.EndorsementCode, premiumFilterDTO.Id);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCEN.TempApplicationPremium.Properties.PaymentNum, premiumFilterDTO.Number);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(ACCEN.TempApplicationPremium.Properties.PayerCode, premiumFilterDTO.PayerId);
            SelectQuery selectQuery = new SelectQuery();
            Join join = new Join(new ClassNameTable(typeof(ACCEN.TempApplication), typeof(ACCEN.TempApplication).Name), new ClassNameTable(typeof(ACCEN.TempApplicationPremium), typeof(ACCEN.TempApplicationPremium).Name), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(ACCEN.TempApplication.Properties.TempAppCode, typeof(ACCEN.TempApplication).Name)
                .Equal()
                .Property(ACCEN.TempApplicationPremium.Properties.TempAppCode, typeof(ACCEN.TempApplicationPremium).Name)
                .GetPredicate());
            selectQuery.Table = join;
            selectQuery.Where = criteriaBuilder.GetPredicate();            
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.TempApplicationPremium.Properties.TempAppCode, typeof(ACCEN.TempApplicationPremium).Name), ACCEN.TempApplicationPremium.Properties.TempAppCode));
            TemporalPremiumDTO temporalPremiumDTO = null;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                temporalPremiumDTO = reader.SelectReader(read => new TemporalPremiumDTO { Id = Convert.ToInt32(read[ACCEN.TempApplicationPremium.Properties.TempAppCode]), IsTemporal = true }).ToList().FirstOrDefault();
            }
            if (temporalPremiumDTO == null)
            {
                if (premiumFilterDTO.IsReversion)
                {
                    temporalPremiumDTO = ValidateReversion(premiumFilterDTO);
                }
                if (temporalPremiumDTO == null)
                {
                    criteriaBuilder = new ObjectCriteriaBuilder();
                    criteriaBuilder.PropertyEquals(ACCEN.ApplicationPremium.Properties.EndorsementCode, premiumFilterDTO.Id);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCEN.ApplicationPremium.Properties.PaymentNum, premiumFilterDTO.Number);
                    criteriaBuilder.And();
                    criteriaBuilder.PropertyEquals(ACCEN.ApplicationPremium.Properties.PayerCode, premiumFilterDTO.Id);
                    if (premiumFilterDTO.IsReversion)
                    {
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCEN.ApplicationPremium.Properties.AppPremiumCode, premiumFilterDTO.PremiumId);
                        criteriaBuilder.And();
                        criteriaBuilder.PropertyEquals(ACCEN.ApplicationPremium.Properties.IsRev, true);
                    }
                    selectQuery = new SelectQuery();
                    selectQuery.Table = new ClassNameTable(typeof(ACCEN.ApplicationPremium), typeof(ACCEN.ApplicationPremium).Name);
                    selectQuery.Where = criteriaBuilder.GetPredicate();
                    using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                    {
                        temporalPremiumDTO = reader.SelectReader(read => new TemporalPremiumDTO { Id = Convert.ToInt32(read[ACCEN.TempApplicationPremium.Properties.TempAppCode]), IsTemporal = true }).ToList().FirstOrDefault();
                    }
                    if (temporalPremiumDTO == null)
                    {
                        temporalPremiumDTO = new TemporalPremiumDTO { Id = 0, IsTemporal = false };
                    }
                }

            }
            return temporalPremiumDTO;
        }

        private static TemporalPremiumDTO ValidateReversion(PremiumFilterDTO premiumFilterDTO)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCEN.TempApplicationPremiumRev.Properties.AppPremiumId, premiumFilterDTO.PremiumId);
            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(ACCEN.TempApplicationPremiumRev.Properties.TempAppId, typeof(ACCEN.TempApplicationPremiumRev).Name), ACCEN.TempApplicationPremiumRev.Properties.TempAppId));
            selectQuery.Table = new ClassNameTable(typeof(ACCEN.TempApplicationPremiumRev), typeof(ACCEN.TempApplicationPremiumRev).Name); ;
            selectQuery.Where = criteriaBuilder.GetPredicate();
            TemporalPremiumDTO temporalPremiumDTO = null;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                temporalPremiumDTO = reader.SelectReader(read => new TemporalPremiumDTO { Id = Convert.ToInt32(read[ACCEN.TempApplicationPremiumRev.Properties.TempAppId]), IsTemporal = true }).ToList().FirstOrDefault();
            }
            return temporalPremiumDTO;

        }
        #endregion
    }
}
