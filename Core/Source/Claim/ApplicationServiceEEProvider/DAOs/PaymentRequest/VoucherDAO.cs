using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest;
using Sistran.Core.Application.ClaimServices.EEProvider.Views.PaymentRequest;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Linq;
using PAYMEN = Sistran.Core.Application.Claims.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.PaymentRequest
{
    public class VoucherDAO
    {
        public List<Voucher> CreateVouchers(Models.Claim.Claim claim, Models.PaymentRequest.PaymentRequest paymentRequest)
        {
            List<PAYMEN.PaymentVoucher> paymentVouchers = new List<PAYMEN.PaymentVoucher>();
            foreach (Voucher voucher in claim.Vouchers)
            {                
                voucher.PaymentRequestId = paymentRequest.Id;
                PAYMEN.PaymentVoucher entityVoucher = EntityAssembler.CreateVoucher(voucher);
                DataFacadeManager.Insert(entityVoucher);

                voucher.Id = entityVoucher.PaymentVoucherCode;

                foreach (VoucherConcept voucherConcept in voucher.Concepts)
                {
                    VoucherConceptDAO voucherConceptBusiness = new VoucherConceptDAO();
                    voucherConcept.Id = voucherConceptBusiness.CreateVoucherConcept(voucherConcept, voucher.Id).Id;

                    PAYMEN.PaymentRequestClaim entityPaymentRequestClaim = EntityAssembler.CreateClaimPaymentRequest(paymentRequest, claim, voucher, voucherConcept.Id);

                    DataFacadeManager.Insert(entityPaymentRequestClaim);
                }

                paymentVouchers.Add(entityVoucher);
            }

            return ModelAssembler.CreateVouchers(paymentVouchers);
        }
        public Voucher CreateVoucher(Voucher voucher)
        {
            PAYMEN.PaymentVoucher entityVoucher = EntityAssembler.CreateVoucher(voucher);
            DataFacadeManager.Insert(entityVoucher);

            return ModelAssembler.CreateVoucher(entityVoucher);
        }

        public List<Voucher> GetVouchersByPaymentRequestId(int paymentRequestId)
        {
            List<Voucher> vouchers = new List<Voucher>();
            VoucherConceptDAO voucherConceptDAO = new VoucherConceptDAO();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PAYMEN.PaymentVoucher.Properties.PaymentRequestCode, typeof(PAYMEN.PaymentVoucher).Name, paymentRequestId);

            PaymentVoucherView paymentVoucherView = new PaymentVoucherView();
            ViewBuilder viewBuilder = new ViewBuilder("PaymentVoucherView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, paymentVoucherView);

            if (paymentVoucherView.PaymentVouchers.Any())
            {
                vouchers = ModelAssembler.CreateVouchers(paymentVoucherView.PaymentVouchers);

                foreach (Voucher voucher in vouchers)
                {
                    voucher.Currency = ModelAssembler.CreateCurrency(paymentVoucherView.Currencies.Cast<COMMEN.Currency>().First(x => x.CurrencyCode == voucher.Currency.Id));
                    voucher.VoucherType = ModelAssembler.CreateVoucherType(paymentVoucherView.VoucherTypes.Cast<PARAMEN.VoucherType>().First(x => x.VoucherTypeCode == voucher.VoucherType.Id));
                    voucher.Concepts = voucherConceptDAO.GetVoucherConceptsByVoucherId(voucher.Id);
                }
            }

            return vouchers;
        }
    }
}
