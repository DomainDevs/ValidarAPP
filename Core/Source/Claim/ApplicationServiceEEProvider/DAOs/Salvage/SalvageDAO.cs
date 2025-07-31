using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using System.Collections.Generic;
using System.Linq;
using CLMMOD = Sistran.Core.Application.ClaimServices.EEProvider.Models.Salvage;
using CLMEN = Sistran.Core.Application.Claims.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Application.ClaimServices.EEProvider.Views.Salvage;
using Sistran.Core.Framework.DAF.Engine;
using SLVMO = Sistran.Core.Application.ClaimServices.EEProvider.Models.Salvage;
using Sistran.Core.Application.CommonService.Models;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.ClaimServices.EEProvider.DAOs.PaymentRequest;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Salvage
{
    public class SalvageDAO
    {
        public CLMMOD.Salvage CreateSalvage(CLMMOD.Salvage salvage)
        {
            CLMEN.Salvage entitySalvage = EntityAssembler.CreateSalvage(salvage);

            DataFacadeManager.Insert(entitySalvage);
            salvage.Id = entitySalvage.SalvageCode;

            return salvage;
        }

        public CLMMOD.Salvage UpdateSalvage(CLMMOD.Salvage salvage)
        {
            CLMEN.Salvage entitySalvage = EntityAssembler.CreateSalvage(salvage);
            DataFacadeManager.Update(entitySalvage);
            return salvage;
        }

        public void DeleteSalvage(int salvageId)
        {
            PrimaryKey salvagePrimaryKey = CLMEN.Salvage.CreatePrimaryKey(salvageId);
            DataFacadeManager.Delete(salvagePrimaryKey);
        }

        public List<CLMMOD.Salvage> GetSalvagesByClaimId(int claimId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CLMEN.Salvage.Properties.ClaimCode, typeof(CLMEN.Salvage).Name, claimId);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(CLMEN.Salvage), filter.GetPredicate());

            return ModelAssembler.CreateSalvages(businessCollection);
        }

        public List<CLMMOD.Salvage> GetSalvagesByClaimIdSubClaimId(int claimId, int subClaimId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CLMEN.Salvage.Properties.ClaimCode, typeof(CLMEN.Salvage).Name, claimId);
            filter.And();
            filter.PropertyEquals(CLMEN.Salvage.Properties.SubclaimCode, typeof(CLMEN.Salvage).Name, subClaimId);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(CLMEN.Salvage), filter.GetPredicate());

            return ModelAssembler.CreateSalvages(businessCollection);
        }

        public CLMMOD.Salvage GetSalvageBySalvageId(int salvageId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(CLMEN.Salvage.Properties.SalvageCode, typeof(CLMEN.Salvage).Name, salvageId);

            PaymentRequestDAO paymentRequestDAO = new PaymentRequestDAO();
            SalvageSaleView view = new SalvageSaleView();
            ViewBuilder builder = new ViewBuilder("SalvageSaleView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.Salvage.Count > 0)
            {
                SLVMO.Salvage salvage = ModelAssembler.CreateSalvage(view.Salvage.Cast<CLMEN.Salvage>().First());
                salvage.Sales = ModelAssembler.CreateSales(view.Sales);
                
                salvage.RecoveryAmount = paymentRequestDAO.GetRecoveryAmountByRecoveryIdOrSalvageId(salvage.Id, false);

                return salvage;
            }
            else
            {
                return null;
            }
        }

        public int GetSalvageNumberByClaimIdSubClaimId(int claimId, int subClaimId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CLMEN.Salvage.Properties.ClaimCode, typeof(CLMEN.Salvage).Name);
            filter.Equal();
            filter.Constant(claimId);
            filter.And();
            filter.Property(CLMEN.Salvage.Properties.SubclaimCode, typeof(CLMEN.Salvage).Name);
            filter.Equal();
            filter.Constant(subClaimId);

            return DataFacadeManager.GetObjects(typeof(CLMEN.Salvage), filter.GetPredicate()).Count;
        }
        public List<PaymentClass> GetPaymentClasses()
        {
            return ModelAssembler.CreatePaymentClasses(DataFacadeManager.GetObjects(typeof(COMMEN.PaymentClass)));
        }
    }
}
