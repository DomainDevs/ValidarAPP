using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Issuance.Entities
{
    [
      PersistentClass("Sistran.Core.Application.Issuance.PayerPaymentComponent.dict"),
      Serializable(),
      DescriptionKey("PAYER_PAYMENT_COMPONENT_ENTITY"),
      TableMap(TableName = "PAYER_PAYMENT_COMP", Schema = "ISS"),
      ]
    public partial class PayerPaymentComponent :
          BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string PayerPaymentCompId = "PayerPaymentCompId";
            public static readonly string PayerPaymentId = "PayerPaymentId";
            public static readonly string ComponentCode = "ComponentCd";
            public static readonly string PaymentCpt = "PaymentCpt";
            public static readonly string Amount = "Amount";
            public static readonly string LocalAmount = "LocalAmount";
            public static readonly string MainAmount = "MainAmount";
            public static readonly string MainLocalAmount = "MainLocalAmount";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="paymentScheduleId">Propiedad clave PaymentScheduleId.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>()
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.PayerPaymentCompId, null);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="payerPaymentCompId">Propiedad clave PaymentScheduleId.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int payerPaymentCompId)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.PayerPaymentCompId, payerPaymentCompId);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
		/// Crea una clave primaria para esta clase.
		/// </summary>
		/// <param name="paymentScheduleId">Propiedad clave PaymentScheduleId.</param>
		public static PrimaryKey CreatePrimaryKey()
        {
            return InternalCreatePrimaryKey<PayerPaymentComponent>();
        }
        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="payerPaymentCompId">Propiedad clave PaymentScheduleId.</param>
        public static PrimaryKey CreatePrimaryKey(int payerPaymentCompId)
        {
            return InternalCreatePrimaryKey<PayerPaymentComponent>(payerPaymentCompId);
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad PayerPaymentId.
        /// </summary>
        private int _payerPaymentId;
        /// <summary>
        /// Atributo para la propiedad ComponentCode.
        /// </summary>
        private int _componentCode;
        /// <summary>
        /// Atributo para la propiedad IsGreaterDate.
        /// </summary>
        private decimal _paymentCpt;
        /// <summary>
        /// Atributo para la propiedad IsIssueDate.
        /// </summary>
        private decimal _amount;
        /// <summary>
        /// Atributo para la propiedad FirstPayQuantity.
        /// </summary>
        private decimal _localAmount;
        /// <summary>
        /// Atributo para la propiedad PaymentQuantity.
        /// </summary>
        private decimal _mainAmount;
        /// <summary>
        /// Atributo para la propiedad GapUnitCode.
        /// </summary>
        private decimal _mainLocalAmount;
        
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="payerPaymentCompId">PaymentScheduleId key property.</param>
        public PayerPaymentComponent(int payerPaymentCompId) :
            this(PayerPaymentComponent.CreatePrimaryKey(payerPaymentCompId), null)
        {
        }

        /// <summary>
		/// Constructor de instancia de la clase en base a las propiedades claves.
		/// </summary>
		/// <param name="paymentScheduleId">PaymentScheduleId key property.</param>
	    public PayerPaymentComponent() :
            this(PayerPaymentComponent.CreatePrimaryKey(), null)
        {
        }

        /// <summary>
        /// Constructor de instancia de la clase en base a una clave primaria y a valores iniciales.
        /// </summary>
        /// <param name="key">
        /// Identificador de la instancia de la entidad.
        /// </param>
        /// <param name="initialValues">
        /// Valores para establecer el estado de la instancia.
        /// </param>
        public PayerPaymentComponent(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        /*** Object Properties ********************************/
        /// <summary>
        /// Devuelve o setea el valor de la propiedad PaymentScheduleId.
        /// </summary>
        /// <value>Propiedad PaymentScheduleId.</value>
        [
        DescriptionKey("PAYER_PAYMENT_COMP_ID_PROPERTY"),
        PersistentProperty(IsKey = true),
        ColumnMap(ColumnName = "PAYER_PAYMENT_COMP_ID", DbType = System.Data.DbType.String),
        ]
        public int PayerPaymentComponentId
        {
            get
            {
                return (int)this._primaryKey[Properties.PayerPaymentCompId];
            }
            set
            {
                this._primaryKey[Properties.PayerPaymentCompId] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Description.
        /// </summary>
        /// <value>Propiedad Description.</value>
        [
        DescriptionKey("PAYER_PAYMENT_ID_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "PAYER_PAYMENT_ID", DbType = System.Data.DbType.String),
        ]
        public int PayerPayment
        {
            get
            {
                return this._payerPaymentId;
            }
            set
            {
                this._payerPaymentId = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad SmallDescription.
        /// </summary>
        /// <value>Propiedad SmallDescription.</value>
        [
        DescriptionKey("COMPONENT_CD_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "COMPONENT_CD", DbType = System.Data.DbType.String),
        ]
        public int ComponentCode
        {
            get
            {
                return this._componentCode;
            }
            set
            {
                this._componentCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad IsGreaterDate.
        /// </summary>
        /// <value>Propiedad IsGreaterDate.</value>
        [
        DescriptionKey("PAYMENT_PCT_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "PAYMENT_PCT", DbType = System.Data.DbType.String),
        ]
        public decimal PaymentCpt
        {
            get
            {
                return this._paymentCpt;
            }
            set
            {
                this._paymentCpt = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad IsIssueDate.
        /// </summary>
        /// <value>Propiedad IsIssueDate.</value>
        [
        DescriptionKey("AMOUNT_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "AMOUNT", DbType = System.Data.DbType.String),
        ]
        public decimal Amount
        {
            get
            {
                return this._amount;
            }
            set
            {
                this._amount = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad FirstPayQuantity.
        /// </summary>
        /// <value>Propiedad FirstPayQuantity.</value>
        [
        DescriptionKey("LOCAL_AMOUNT_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "LOCAL_AMOUNT", DbType = System.Data.DbType.String),
        ]
        public decimal LocalAmount
        {
            get
            {
                return this._localAmount;
            }
            set
            {
                this._localAmount = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad PaymentQuantity.
        /// </summary>
        /// <value>Propiedad PaymentQuantity.</value>
        [
        DescriptionKey("MAIN_AMOUNT_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "MAIN_AMOUNT", DbType = System.Data.DbType.String),
        ]
        public decimal MainAmount
        {
            get
            {
                return this._mainAmount;
            }
            set
            {
                this._mainAmount = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad GapUnitCode.
        /// </summary>
        /// <value>Propiedad GapUnitCode.</value>
        [
        DescriptionKey("MAIN_LOCAL_AMOUNT_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "MAIN_LOCAL_AMOUNT", DbType = System.Data.DbType.String),
        ]
        public decimal MainLocalAmount
        {
            get
            {
                return this._mainLocalAmount;
            }
            set
            {
                this._mainLocalAmount = value;
            }
        }


    }
}
