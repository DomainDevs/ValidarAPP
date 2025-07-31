using System.Collections.Generic;
using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using CLMMOD=Sistran.Core.Application.ClaimServices.EEProvider.Models.Recovery;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using CLMEN = Sistran.Core.Application.Claims.Entities;
using PAYMEN = Sistran.Core.Application.Claims.Entities;
using Sistran.Core.Framework.DAF.Engine;
using System.Linq;
using Sistran.Core.Application.ClaimServices.EEProvider.Views.Recovery;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;
using Sistran.Core.Application.ClaimServices.EEProvider.Business.Claims;
using INTEN = Sistran.Core.Application.Integration.Entities;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Recovery
{
    public class RecoveryDAO
    {
        public CLMMOD.Recovery CreateRecovery(CLMMOD.Recovery recovery)
        {
            CLMEN.Recovery entityRecovery = EntityAssembler.CreateRecovery(recovery);

            if (entityRecovery.DebtorCode == 0)
                entityRecovery.DebtorCode = null;

            DataFacadeManager.Insert(entityRecovery);
            recovery.Id = entityRecovery.RecoveryCode;

            #region RecoveryControl
            CLMMOD.RecoveryControl recoveryControl = new CLMMOD.RecoveryControl();
            recoveryControl.Action = "I";
            recoveryControl.RecoveryId = recovery.Id;            
            recoveryControl.PaymentPlanId = recovery.PaymentPlan.Id;
            CreateRecoveryControl(recoveryControl);
            #endregion

            return recovery;
        }

        public CLMMOD.Recovery UpdateRecovery(CLMMOD.Recovery recovery)
        {
            CLMEN.Recovery entityRecovery = EntityAssembler.CreateRecovery(recovery);

            if (recovery.Debtor.Id == 0 && recovery.Debtor.DocumentNumber == null)
            {
                entityRecovery.DebtorCode = null;
            }

            DataFacadeManager.Update(entityRecovery);

            return ModelAssembler.CreateRecovery(entityRecovery);
        }

        public List<CLMMOD.RecoveryType> GetRecoveryTypesById(int recoveryTypeId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PARAMEN.RecoveryType.Properties.RecoveryTypeCode, typeof(PARAMEN.RecoveryType).Name, recoveryTypeId);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(PARAMEN.RecoveryType), filter.GetPredicate());
            return ModelAssembler.CreateRecoveryTypes(businessCollection);
        }

        public List<CLMMOD.Recovery> GetRecoveriesByClaimIdSubClaimId(int claimId, int subClaimId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CLMEN.Recovery.Properties.ClaimCode, typeof(CLMEN.Recovery).Name, claimId);
            filter.And();
            filter.PropertyEquals(CLMEN.Recovery.Properties.SubClaimCode, typeof(CLMEN.Recovery).Name, subClaimId);

            List<CLMMOD.Recovery> recoveries = new List<CLMMOD.Recovery>();

            RecoveryView recoveryView = new RecoveryView();
            ViewBuilder viewBuilder = new ViewBuilder("RecoveryView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, recoveryView);

            if (recoveryView.Recoveries.Count > 0)
            {
                recoveries = ModelAssembler.CreateRecoveries(recoveryView.Recoveries);
                if (recoveryView.PaymentPlans.Count > 0)
                {
                    foreach (CLMMOD.Recovery recovery in recoveries)
                    {
                        recovery.PaymentPlan = ModelAssembler.CreatePaymentPlan(recoveryView.PaymentPlans.Cast<PAYMEN.PaymentPlan>().FirstOrDefault(x => x.PaymentPlanCode == recovery.PaymentPlan.Id));
                    }
                }
            }

            return recoveries;
        }

        public List<CLMMOD.Recovery> GetRecoveriesByClaimId(int claimId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CLMEN.Recovery.Properties.ClaimCode, typeof(CLMEN.Recovery).Name, claimId);

            List<CLMMOD.Recovery> recoveries = new List<CLMMOD.Recovery>();

            RecoveryView recoveryView = new RecoveryView();
            ViewBuilder viewBuilder = new ViewBuilder("RecoveryView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, recoveryView);

            if (recoveryView.Recoveries.Count > 0)
            {
                recoveries = ModelAssembler.CreateRecoveries(recoveryView.Recoveries);
                if (recoveryView.PaymentPlans.Count > 0)
                {
                    foreach (CLMMOD.Recovery recovery in recoveries)
                    {
                        recovery.PaymentPlan = ModelAssembler.CreatePaymentPlan(recoveryView.PaymentPlans.Cast<PAYMEN.PaymentPlan>().FirstOrDefault(x => x.PaymentPlanCode == recovery.PaymentPlan.Id));
                    }
                }
            }

            return recoveries;
        }

        public List<CLMMOD.RecoveryType> GetRecoveryTypes()
        {
            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(PARAMEN.RecoveryType));
            return ModelAssembler.CreateRecoveryTypes(businessCollection);
        }

        public CLMMOD.Recovery GetRecoveryByRecoveryId(int recoveryId)
        {
            CLMMOD.Recovery recovery = new CLMMOD.Recovery();
            RecuperatorBusiness recuperatorBusiness = new RecuperatorBusiness();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CLMEN.Recovery.Properties.RecoveryCode, typeof(CLMEN.Recovery).Name);
            filter.Equal();
            filter.Constant(recoveryId);

            RecoveryPaymentPlanView recoveryPaymentPlanView = new RecoveryPaymentPlanView();
            ViewBuilder viewBuilder = new ViewBuilder("RecoveryPaymentPlanView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, recoveryPaymentPlanView);

            if (recoveryPaymentPlanView.Recoveries.Count > 0)
            {
                recovery = ModelAssembler.CreateRecovery((CLMEN.Recovery)recoveryPaymentPlanView.Recoveries.First());

                if (recoveryPaymentPlanView.PaymentPlans.Count > 0)
                {
                    recovery.PaymentPlan = ModelAssembler.CreatePaymentPlan((PAYMEN.PaymentPlan)recoveryPaymentPlanView.PaymentPlans.First());
                    recovery.PaymentPlan.PaymentQuotas = ModelAssembler.CreatePaymentQuotas(recoveryPaymentPlanView.PaymentSchedules);
                }

                return recovery;
            }
            else
            {
                return null;
            }
        }

        public int GetRecoveryNumberByClaimIdSubClaimId(int claimId, int subClaimId)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CLMEN.Recovery.Properties.ClaimCode, typeof(CLMEN.Recovery).Name);
            filter.Equal();
            filter.Constant(claimId);
            filter.And();
            filter.Property(CLMEN.Recovery.Properties.SubClaimCode, typeof(CLMEN.Recovery).Name);
            filter.Equal();
            filter.Constant(subClaimId);

            return DataFacadeManager.GetObjects(typeof(CLMEN.Recovery), filter.GetPredicate()).Count;
        }


        private CLMMOD.RecoveryControl CreateRecoveryControl(CLMMOD.RecoveryControl recoveryControl)
        {
            return ModelAssembler.CreateRecoveryControl((INTEN.ClmRecoveryControl)DataFacadeManager.Insert(EntityAssembler.CreateRecoveryControl(recoveryControl)));
        }
    }
}
