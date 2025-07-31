using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using PAYMEN = Sistran.Core.Application.Claims.Entities;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.PaymentRequest;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using Sistran.Core.Application.ClaimServices.EEProvider.Views.PaymentRequest;
using Sistran.Core.Framework.DAF.Engine;
using TAXEN = Sistran.Core.Application.Tax.Entities;
using System.Linq;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.PaymentRequest
{
    public class VoucherConceptDAO
    {
        public VoucherConcept CreateVoucherConcept(VoucherConcept voucherConcept, int voucherId)
        {
            PAYMEN.PaymentVoucherConcept entityVoucherConcept = EntityAssembler.CreateVoucherConcept(voucherConcept, voucherId);
            DataFacadeManager.Insert(entityVoucherConcept);
            voucherConcept.Id = entityVoucherConcept.PaymentVoucherConceptCode;

            foreach (VoucherConceptTax voucherConceptTax in voucherConcept.VoucherConceptTaxes)
            {
                PAYMEN.PaymentVoucherConceptTax entityPaymentRequestTax = EntityAssembler.CreateVoucherConceptTax(voucherConceptTax, voucherConcept.Id);
                DataFacadeManager.Insert(entityPaymentRequestTax);

                voucherConceptTax.Id = entityPaymentRequestTax.PaymentVoucherConceptTaxCode;
            }

            return voucherConcept;
        }

        public List<VoucherConcept> GetVoucherConceptsByVoucherId(int voucherId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PAYMEN.PaymentVoucherConcept.Properties.PaymentVoucherCode, typeof(PAYMEN.PaymentVoucherConcept).Name, voucherId);

            List<VoucherConcept> voucherConcepts = ModelAssembler.CreateVoucherConcepts(DataFacadeManager.GetObjects(typeof(PAYMEN.PaymentVoucherConcept), filter.GetPredicate()));

            foreach (VoucherConcept voucherConcept in voucherConcepts)
            {
                filter.Clear();
                filter.PropertyEquals(PAYMEN.PaymentVoucherConceptTax.Properties.PaymentVoucherConceptCode, typeof(PAYMEN.PaymentVoucherConceptTax).Name, voucherConcept.Id);

                PaymentVoucherConceptTaxesView paymentVoucherConceptTaxesView = new PaymentVoucherConceptTaxesView();
                ViewBuilder viewBuilder = new ViewBuilder("PaymentVoucherConceptTaxesView");
                viewBuilder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, paymentVoucherConceptTaxesView);

                if (paymentVoucherConceptTaxesView.PaymentVoucherConceptTaxes.Count > 0)
                {
                    voucherConcept.VoucherConceptTaxes = ModelAssembler.CreateVoucherConceptTaxesByVoucherConceptId(paymentVoucherConceptTaxesView.PaymentVoucherConceptTaxes);

                    List<TAXEN.Tax> entityTaxes = paymentVoucherConceptTaxesView.Taxes.Cast<TAXEN.Tax>().ToList();
                    List<TAXEN.TaxCategory> entityTaxCategories = paymentVoucherConceptTaxesView.TaxCategories.Cast<TAXEN.TaxCategory>().ToList();
                    List<TAXEN.TaxCondition> entityTaxConditions = paymentVoucherConceptTaxesView.TaxConditions.Cast<TAXEN.TaxCondition>().ToList();

                    foreach (VoucherConceptTax voucherConceptTax in voucherConcept.VoucherConceptTaxes)
                    {
                        voucherConceptTax.TaxDescription = entityTaxes.FirstOrDefault(x => x.TaxCode == voucherConceptTax.TaxId)?.Description;
                        voucherConceptTax.CategoryDescription = entityTaxCategories.FirstOrDefault(x => x.TaxCode == voucherConceptTax.TaxId && x.TaxCategoryCode == voucherConceptTax.CategoryId)?.Description;
                        voucherConceptTax.ConditionDescription = entityTaxConditions.FirstOrDefault(x => x.TaxCode == voucherConceptTax.TaxId && x.TaxConditionCode == voucherConceptTax.ConditionId)?.Description;
                    }
                }
            }

            return voucherConcepts;
        }
    }
}