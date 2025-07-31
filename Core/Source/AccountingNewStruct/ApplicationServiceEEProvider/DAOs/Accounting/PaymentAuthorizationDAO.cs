using ACCOUNTINGEN = Sistran.Core.Application.Accounting.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;

namespace Sistran.Core.Application.AccountingServices.EEProvider.DAOs.Accounting
{
    internal class PaymentAuthorizationDAO
    {
        /// <summary>
        /// Saves the payment authorization.
        /// </summary>
        /// <param name="ballotNumber">The ballot number.</param>
        /// <returns></returns>
        internal static long SavePaymentAuthorization(string ballotNumber)
        {
            ACCOUNTINGEN.CiaPaymentAuthorizationNumber entityPaymentAuthorizationNumber = new ACCOUNTINGEN.CiaPaymentAuthorizationNumber { BallotNumber = ballotNumber };
            DataFacadeManager.Instance.GetDataFacade().InsertObject(entityPaymentAuthorizationNumber);
            return entityPaymentAuthorizationNumber.Id;
        }

        /// <summary>
        /// Saves the payment authorization.
        /// </summary>
        /// <param name="ballotNumber">The ballot number.</param>
        /// <returns></returns>
        internal static long UpdatePaymentAuthorization(long paymentAuthorizationId, int technicalTransaction)
        {
            PrimaryKey primaryKey = ACCOUNTINGEN.CiaPaymentAuthorizationNumber.CreatePrimaryKey(paymentAuthorizationId);

            ACCOUNTINGEN.CiaPaymentAuthorizationNumber entityPaymentAuthorization = (ACCOUNTINGEN.CiaPaymentAuthorizationNumber)
                (DataFacadeManager.GetObject(primaryKey));

            if (entityPaymentAuthorization != null)
            {
                entityPaymentAuthorization.TechnicalTransaction = technicalTransaction;
                DataFacadeManager.Update(entityPaymentAuthorization);
                return entityPaymentAuthorization.Id;
            }
            return -1;
        }

        /// <summary>
        /// Saves the payment authorization.
        /// </summary>
        /// <param name="ballotNumber">The ballot number.</param>
        /// <returns></returns>
        internal static bool ExistsPaymentAuthorization(string paymentBallot)
        {

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(ACCOUNTINGEN.CiaPaymentAuthorizationNumber.Properties.BallotNumber, paymentBallot);
            criteriaBuilder.And();
            criteriaBuilder.Property(ACCOUNTINGEN.CiaPaymentAuthorizationNumber.Properties.TechnicalTransaction);
            criteriaBuilder.IsNotNull();

            BusinessCollection businessCollection =
                    DataFacadeManager.GetObjects(typeof(ACCOUNTINGEN.CiaPaymentAuthorizationNumber), criteriaBuilder.GetPredicate());

            return businessCollection != null && businessCollection.Count > 0;
        }
    }
}
