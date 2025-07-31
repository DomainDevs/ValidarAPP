using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    /// <summary>
    /// Metodos de Pago
    /// </summary>
    public class PaymentMethodDAO
    {
        /// <summary>
        /// Obtener metodo de pago de un individuo
        /// </summary>
        /// <param name="individualId">Id individuo</param>
        /// <returns>Metodo de pago</returns>
        public Models.IndividualPaymentMethod GetPaymentMethodByIndividualId(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(IndividualPaymentMethod.Properties.IndividualId, typeof(IndividualPaymentMethod).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(IndividualPaymentMethod.Properties.Enabled, typeof(IndividualPaymentMethod).Name);
            filter.Equal();
            filter.Constant(1);
            BusinessCollection businessCollection = new BusinessCollection();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(IndividualPaymentMethod), filter.GetPredicate()));
            }
            if (businessCollection.Count > 0)
            {
                return ModelAssembler.CreatePaymentMethod((IndividualPaymentMethod)businessCollection[0]);
            }
            else
            {
                return new Models.IndividualPaymentMethod();
            }
        }

        /// <summary>
        /// Crear nuevo metodo de pago
        /// </summary>
        /// <param name="invidualPaymentMethod">Modelo invidualpaymentmethod</param>
        /// <param name="individualId">individualId</param>
        /// <returns></returns>
        public Models.IndividualPaymentMethod CreatePaymentMethod(Models.PaymentMethodAccount paymentMethodAccount, int individualId)
        {
            Models.IndividualPaymentMethod ipm = ModelAssembler.CreatePaymentMethodByPaymentAccount(paymentMethodAccount);
            ipm.IndividualId = individualId;
            IndividualPaymentMethod paymentMethodEntity = EntityAssembler.CreatePaymentMethod(ipm);
            paymentMethodEntity.IndividualId = individualId;
            DataFacadeManager.Instance.GetDataFacade().InsertObject(paymentMethodEntity);
            return ModelAssembler.CreatePaymentMethod(paymentMethodEntity);
        }

        /// <summary>
        /// Actualizar o Crear Metodos de Pago
        /// </summary>
        /// <param name="invidualPaymentMethod">Modelo invidualpaymentmethod</param>
        /// <param name="individualId">individualId</param>
        /// <returns></returns>
        public Models.IndividualPaymentMethod UpdatePaymentMethod(Models.PaymentMethodAccount paymentMethodAccount, int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(IndividualPaymentMethod.Properties.IndividualId, typeof(IndividualPaymentMethod).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(IndividualPaymentMethod.Properties.PaymentId, typeof(IndividualPaymentMethod).Name);
            filter.Equal();
            filter.Constant(paymentMethodAccount.Id);
            IndividualPaymentMethod PaymentMethod = null;
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                PaymentMethod = (IndividualPaymentMethod)daf.List(typeof(IndividualPaymentMethod), filter.GetPredicate()).FirstOrDefault();
            }
            if (PaymentMethod == null)
            {
                Models.IndividualPaymentMethod ipm = ModelAssembler.CreatePaymentMethodByPaymentAccount(paymentMethodAccount);
                ipm.IndividualId = individualId;
                IndividualPaymentMethod paymentMethodEntity = EntityAssembler.CreatePaymentMethod(ipm);
                paymentMethodEntity.IndividualId = individualId;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.InsertObject(paymentMethodEntity);
                }
                return ModelAssembler.CreatePaymentMethod(paymentMethodEntity);
            }
            else
            {
                return null;
            }
        }
    }
}
