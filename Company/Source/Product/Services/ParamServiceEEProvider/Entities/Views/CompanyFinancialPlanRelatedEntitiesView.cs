using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Product.Entities;
using Sistran.Core.Application.Issuance.Entities;
namespace Sistran.Company.Application.ProductParamServices.EEProvider.Entities.Views
{
    /// <summary>
    /// Esta clase permite modelar una relación entre Sistran.Core.Aplication
    /// .Products.Entities.ProductFinancialPlan, Sistran.Core.Aplication
    /// .Products.Entities.PaymentSchdule, Sistran.Core.Aplication
    /// .Products.Entities.FinancialPlan y Sistran.Core.Aplication
    /// .Common.Entities.PaymentMethod.      
    /// </summary>
    [Serializable()]
	public class CompanyFinancialPlanRelatedEntitiesView : BusinessView
	{

		/// <summary>
		/// Constructor de instancias de la vista.
		/// </summary>
        public CompanyFinancialPlanRelatedEntitiesView()
		{}
        /// <summary>
        /// Colección de elementos de la entidad
        /// </summary>
        /// <value>
        /// Colección de FinancialPlan.
        /// </value>
        public BusinessCollection FinancialPlanList
        {
            get
            {
                return this["FinancialPlan"];
            }
        }

        /// <summary>
        /// Colección de elementos de la entidad <see 
        /// cref="Sistran.Core.Application.Common.Entities.Currency">
        /// Currency</see> relacionados con el ProductFinancialPlan.
        /// </summary>
        /// <value>
        /// Colección de Currency relacionados con ProductFinancialPlan.
        /// </value>
        public BusinessCollection CurrencyList
        {
            get
            {
                return this["Currency"];
            }
        }
	
		/// <summary>
		/// Colección de elementos de la entidad <see 
		/// cref="Sistran.Core.Application.Common.Entities.PaymentMethod">
		/// PaymentMethod</see> relacionados con el ProductFinancialPlan.
		/// </summary>
		/// <value>
		/// Colección de PaymentMethod relacionados con ProductFinancialPlan.
		/// </value>
		public BusinessCollection PaymentMethodList
		{
			get
			{
				return this["PaymentMethod"];
			}
		}
		

		/// <summary>
		/// Colección de elementos de la entidad
		/// </summary>
		/// <value>
		/// Colección de PaymentSchedule.
		/// </value>
		public BusinessCollection PaymentScheduleList
		{
			get
			{
				return this["PaymentSchedule"];
			}
		}

        /// <summary>
        /// Colección de elementos de la entidad
        /// </summary>
        /// <value>
        /// Colección de PaymentDistribution.
        /// </value>
        public BusinessCollection PaymentDistributionList
        {
            get
            {
                return this["PaymentDistribution"];
            }
        }
        
        /**********************************************************************
		* RELACIÓN : ProductFinancialPlan - FirstPayComponent
		**********************************************************************/
		/// <summary>
		/// Obtiene el objeto de la entidad ProductFinancialPlan relacionado a un
		/// objeto de la entidad FirstPayComponent.
		/// </summary>
		/// <param name="firstPayComponent">
		/// Objeto de la entidad FirstPayComponent a partir del que se desea
		/// obtener el objeto de la entidad ProductFinancialPlan asociado.
		/// </param>
		/// <returns>
		/// Objeto de la entidad ProductFinancialPlan.
		/// </returns>
		public ProductFinancialPlan GetProductFinancialPlanByFirstPayComponent(
            FirstPayComp firstPayComponent)
		{
			return (ProductFinancialPlan) this.GetObjectByRelation(
				"ProductFinancialPlanFirstPayComponent", firstPayComponent, true);
		}

		/// <summary>
		/// Obtiene una colección de objetos de la entidad FirstPayComponent
		/// relacionado a un objeto de la entidad ProductFinancialPlan.
		/// </summary>
		/// <param name="productFinancialPlan">
		/// Objeto de la entidad ProductFinancialPlan a partir del que se desea
		/// obtener una colección de objetos de la entidad FirstPayComponent
		/// asociado.
		/// </param>
		/// <returns>
		/// Colección de objetos de la entidad FirstPayComponent.
		/// </returns>
		public BusinessCollection GetFirstPayComponentListByProductFinancialPlan(
			ProductFinancialPlan productFinancialPlan)
		{
			return this.GetObjectsByRelation(
				"ProductFinancialPlanFirstPayComponent", productFinancialPlan, false);
		}

		/**********************************************************************
		* RELACIÓN : ProductFinancialPlan - FinancialPlan
		**********************************************************************/
		/// <summary>
		/// Obtiene el objeto de la entidad ProductFinancialPlan relacionado a un objeto de
		/// la entidad FinancialPlan.
		/// </summary>
		/// <param name="financialPlan">
		/// Objeto de la entidad FinancialPlan a partir del que se desea 
		/// obtener el objeto de la entidad ProductFinancialPlan asociado.
		/// </param>
		/// <returns>
		/// Objeto de la entidad ProductFinancialPlan.
		/// </returns>
		public ProductFinancialPlan GetProductFinancialPlanByFinancialPlan(
			FinancialPlan financialPlan)
		{
			return (ProductFinancialPlan) this.GetObjectByRelation(
				"ProductFinancialPlanFinancialPlan", financialPlan, true);
		}
		
		/// <summary>
		/// Obtiene una colección de objetos de la entidad FinancialPlan 
		/// relacionado a un objeto de la entidad ProductFinancialPlan.
		/// </summary>
		/// <param name="productFinancialPlan">
		/// Objeto de la entidad ProductFinancialPlan a partir del que se 
		/// desea obtener una colección de objetos de la entidad 
		/// FinancialPlan asociado.
		/// </param>
		/// <returns>
		/// Colección de objetos de la entidad FinancialPlan.
		/// </returns>
		public BusinessCollection GetFinancialPlanListByProductFinancialPlan(
			ProductFinancialPlan productFinancialPlan)
		{
			return this.GetObjectsByRelation(
				"ProductFinancialPlanFinancialPlan",
				productFinancialPlan,
				false);
		}

		/**********************************************************************
		* RELACIÓN : PaymentMethod - FinancialPlan
		**********************************************************************/
		/// <summary>
		/// Obtiene el objeto de la entidad PaymentMethod relacionado a un 
		/// objeto de la entidad FinancialPlan.
		/// </summary>
		/// <param name="financialPlan">
		/// Objeto de la entidad FinancialPlan a partir del que se desea 
		/// obtener el objeto de la entidad PaymentMethod asociado.
		/// </param>
		/// <returns>
		/// Objeto de la entidad PaymentMethod.
		/// </returns>
		public PaymentMethod GetPaymentMethodByFinancialPlan(
			FinancialPlan financialPlan)
		{
			return (PaymentMethod) this.GetObjectByRelation(
				"PaymentMethodFinancialPlan", financialPlan, false);
		}

		/// <summary>
		/// Obtiene una colección de objetos de la entidad FinancialPlan 
		/// relacionado a un objeto de la entidad PaymentMethod.
		/// </summary>
		/// <param name="paymentMethod">
		/// Objeto de la entidad PaymentMethod a partir del que se desea 
		/// obtener una colección de objetos de la entidad FinancialPlan 
		/// asociado.
		/// </param>
		/// <returns>
		/// Colección de objetos de la entidad FinancialPlan.
		/// </returns>
		public BusinessCollection GetFinancialPlanListByPaymentMethod(
			PaymentMethod paymentMethod)
		{
			return this.GetObjectsByRelation(
				"PaymentMethodFinancialPlan", paymentMethod, false);
		}

		/**********************************************************************
		* RELACIÓN : PaymentSchedule - FinancialPlan
		**********************************************************************/
		/// <summary>
		/// Obtiene una colección de la entidad PaymentSchedule relacionado a un 
		/// objeto de la entidad FinancialPlan.
		/// </summary>
		/// <param name="financialPlan">
		/// Objeto de la entidad FinancialPlan a partir del que se desea 
		/// obtener el objeto de la entidad PaymentSchedule asociado.
		/// </param>
		/// <returns>
		/// Objeto de la entidad PaymentSchedule.
		/// </returns>
		public BusinessCollection GetPaymentScheduleListByFinancialPlan(
			FinancialPlan financialPlan)
		{
			return this.GetObjectsByRelation(
				"PaymentScheduleFinancialPlan", financialPlan, false);
		}

		/// <summary>
		/// Obtiene una colección de objetos de la entidad FinancialPlan 
		/// relacionado a un objeto de la entidad PaymentSchedule.
		/// </summary>
		/// <param name="paymentSchedule">
		/// Objeto de la entidad PaymentSchedule a partir del que se desea 
		/// obtener una colección de objetos de la entidad FinancialPlan 
		/// asociado.
		/// </param>
		/// <returns>
		/// Colección de objetos de la entidad FinancialPlan.
		/// </returns>
		public FinancialPlan GetFinancialPlanByPaymentSchedule(
			PaymentSchedule paymentSchedule)
		{
            return (FinancialPlan) this.GetObjectByRelation(
				"PaymentScheduleFinancialPlan", paymentSchedule, true);
		}

		/// <summary>
		/// Obtiene una colección de objetos de la entidad FinancialPlan 
		/// relacionado a un objeto de la entidad PaymentSchedule.
		/// </summary>
		/// <param name="paymentSchedule">
		/// Objeto de la entidad PaymentSchedule a partir del que se desea 
		/// obtener una colección de objetos de la entidad FinancialPlan 
		/// asociado.
		/// </param>
		/// <returns>
		/// Colección de objetos de la entidad FinancialPlan.
		/// </returns>
        public FinancialPlan GetFinancialPlanByPaymentScheduleId(
            int paymentScheduleId)
        {
            PaymentSchedule paymentSchedule =
                (PaymentSchedule)PaymentScheduleList[PaymentSchedule.CreatePrimaryKey(paymentScheduleId)];

            return (FinancialPlan)this.GetObjectByRelation(
                "PaymentScheduleFinancialPlan", paymentSchedule, true);
        }

        /// <summary>
        /// Obtiene el objeto de la entidad PaymentSchedule relacionado a un 
        /// objeto de la entidad FinancialPlan.
        /// </summary>
        /// <param name="financialPlan">
        /// Objeto de la entidad FinancialPlan a partir del que se desea 
        /// obtener el objeto de la entidad PaymentSchedule asociado.
        /// </param>
        /// <returns>
        /// Objeto de la entidad PaymentSchedule.
        /// </returns>
        public PaymentSchedule GetPaymentScheduleByFinancialPlan(
            FinancialPlan financialPlan)
        {
            BusinessCollection paymentScheduleList = this.GetPaymentScheduleListByFinancialPlan(financialPlan);

            return (PaymentSchedule)paymentScheduleList[0];
        }
        /**********************************************************************
        * RELACIÓN : FinancialPlan - Currency
        **********************************************************************/
        /// <summary>
        /// Obtiene el objeto de la entidad Currency relacionado a un objeto de
        /// la entidad FinancialPlan.
        /// </summary>
        /// <param name="financialPlan">
        /// Objeto de la entidad FinancialPlan a partir del que se desea 
        /// obtener el objeto de la entidad Currency asociado.
        /// </param>
        /// <returns>
        /// Objeto de la entidad Currency.
        /// </returns>
        public Currency GetCurrencyByFinancialPlan(FinancialPlan financialPlan)
        {
            return (Currency)this.GetObjectByRelation(
                "FinancialPlanCurrency", financialPlan, false);
        }

        /// <summary>
        /// Obtiene una colección de objetos de la entidad FinancialPlan 
        /// relacionado a un objeto de la entidad Currency.
        /// </summary>
        /// <param name="currency">
        /// Objeto de la entidad Currency a partir del que se 
        /// desea obtener una colección de objetos de la entidad 
        /// FinancialPlan asociado.
        /// </param>
        /// <returns>
        /// Colección de objetos de la entidad FinancialPlan.
        /// </returns>
        public BusinessCollection GetFinancialPlanListByCurrency(
            Currency currency)
        {
            return this.GetObjectsByRelation(
                "FinancialPlanCurrency",
                currency,
                true);
        }

	}
}
