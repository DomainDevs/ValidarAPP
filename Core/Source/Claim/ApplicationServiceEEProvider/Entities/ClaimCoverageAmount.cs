using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Entities

{
    /// <summary>
    /// Definición de entidad ClaimCoverageAmount.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.ClaimServices.EEProvider.ClaimCoverageAmount.dict"),
    Serializable(),
    DescriptionKey("CLAIM_COVERAGE_AMOUNT_ENTITY"),
    TableMap(TableName = "CLAIM_COVERAGE_AMOUNT", Schema = "CLM"),
    ]
    public partial class ClaimCoverageAmount :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string ClaimCoverageCode = "ClaimCoverageCode";
            public static readonly string EstimationTypeCode = "EstimationTypeCode";
            public static readonly string EstimationTypeStatusCode = "EstimationTypeStatusCode";
            public static readonly string EstimationTypeStatusReasonCode = "EstimationTypeStatusReasonCode";
            public static readonly string EstimationAmount = "EstimationAmount";
            public static readonly string DeductibleAmount = "DeductibleAmount";
            public static readonly string VersionCode = "VersionCode";
            public static readonly string Date = "Date";
            public static readonly string CurrencyCode = "CurrencyCode";
            public static readonly string EstimateAmountAccumulate = "EstimateAmountAccumulate";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="claimCoverageCode">Propiedad clave ClaimCoverageCode.</param>
        /// <param name="estimationTypeCode">Propiedad clave EstimationTypeCode.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int claimCoverageCode, int estimationTypeCode)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.ClaimCoverageCode, claimCoverageCode);
            keys.Add(Properties.EstimationTypeCode, estimationTypeCode);

            return new PrimaryKey<T>(keys);
        }

        protected static PrimaryKey InternalCreatePrimaryKey<T>()
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.ClaimCoverageCode, null);
            keys.Add(Properties.EstimationTypeCode, null);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="claimCoverageCode">Propiedad clave ClaimCoverageCode.</param>
        /// <param name="estimationTypeCode">Propiedad clave EstimationTypeCode.</param>
        public static PrimaryKey CreatePrimaryKey(int claimCoverageCode, int estimationTypeCode)
        {
            return InternalCreatePrimaryKey<ClaimCoverageAmount>(claimCoverageCode, estimationTypeCode);
        }

        public static PrimaryKey CreatePrimaryKey()
        {
            return InternalCreatePrimaryKey<ClaimCoverageAmount>();
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad EstimationTypeStatusCode.
        /// </summary>
        private int? _estimationTypeStatusCode = null;
        /// <summary>
        /// Atributo para la propiedad EstimationTypeStatusReasonCode.
        /// </summary>
        private int? _estimationTypeStatusReasonCode = null;
        /// <summary>
        /// Atributo para la propiedad EstimationAmount.
        /// </summary>
        private decimal? _estimationAmount = null;
        /// <summary>
        /// Atributo para la propiedad DeductibleAmount.
        /// </summary>
        private decimal? _deductibleAmount = null;
        /// <summary>
        /// Atributo para la propiedad VersionCode.
        /// </summary>
        private int? _versionCode = null;
        /// <summary>
        /// Atributo para la propiedad Date.
        /// </summary>
        private DateTime? _date = null;
        /// <summary>
        /// Atributo para la propiedad CurrencyCode.
        /// </summary>
        private int? _currencyCode = null;
        /// <summary>
        /// Atributo para la propiedad EstimateAmountAccumulate.
        /// </summary>
        private decimal? _estimateAmountAccumulate = null;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="claimCoverageCode">ClaimCoverageCode key property.</param>
        /// <param name="estimationTypeCode">EstimationTypeCode key property.</param>
        public ClaimCoverageAmount(int claimCoverageCode, int estimationTypeCode) :
            this(ClaimCoverageAmount.CreatePrimaryKey(claimCoverageCode, estimationTypeCode), null)
        {
        }

        public ClaimCoverageAmount() :
            this(ClaimCoverageAmount.CreatePrimaryKey(), null)
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
        public ClaimCoverageAmount(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        /*** Object Properties ********************************/
        /// <summary>
        /// Devuelve o setea el valor de la propiedad ClaimCoverageCode.
        /// </summary>
        /// <value>Propiedad ClaimCoverageCode.</value>
        [
        DescriptionKey("CLAIM_COVERAGE_CODE_PROPERTY"),
        PersistentProperty(IsKey = true, KeyType = "None"),
        ColumnMap(ColumnName = "CLAIM_COVERAGE_CD", DbType = System.Data.DbType.String),
        ]
        public int ClaimCoverageCode
        {
            get
            {
                return (int)this._primaryKey[Properties.ClaimCoverageCode];
            }
            set
            {
                this._primaryKey[Properties.ClaimCoverageCode] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad EstimationTypeCode.
        /// </summary>
        /// <value>Propiedad EstimationTypeCode.</value>
        [
        DescriptionKey("ESTIMATION_TYPE_CODE_PROPERTY"),
        PersistentProperty(IsKey = true),
        ColumnMap(ColumnName = "ESTIMATION_TYPE_CD", DbType = System.Data.DbType.String),
        ]
        public int EstimationTypeCode
        {
            get
            {
                return (int)this._primaryKey[Properties.EstimationTypeCode];
            }
            set
            {
                this._primaryKey[Properties.EstimationTypeCode] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad EstimationTypeStatusCode.
        /// </summary>
        /// <value>Propiedad EstimationTypeStatusCode.</value>
        [
        DescriptionKey("ESTIMATION_TYPE_STATUS_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "ESTIMATION_TYPE_STATUS_CD", DbType = System.Data.DbType.String),
        ]
        public int? EstimationTypeStatusCode
        {
            get
            {
                return this._estimationTypeStatusCode;
            }
            set
            {
                this._estimationTypeStatusCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad EstimationTypeStatusReasonCode.
        /// </summary>
        /// <value>Propiedad EstimationTypeStatusReasonCode.</value>
        [
        DescriptionKey("ESTIMATION_TYPE_STATUS_REASON_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "ESTIMATION_TYPE_STATUS_REASON_CD", DbType = System.Data.DbType.String),
        ]
        public int? EstimationTypeStatusReasonCode
        {
            get
            {
                return this._estimationTypeStatusReasonCode;
            }
            set
            {
                this._estimationTypeStatusReasonCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad EstimationAmount.
        /// </summary>
        /// <value>Propiedad EstimationAmount.</value>
        [
        DescriptionKey("ESTIMATION_AMOUNT_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "ESTIMATION_AMOUNT", DbType = System.Data.DbType.Decimal),
        ]
        public decimal? EstimationAmount
        {
            get
            {
                return this._estimationAmount;
            }
            set
            {
                this._estimationAmount = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad DeductibleAmount.
        /// </summary>
        /// <value>Propiedad DeductibleAmount.</value>
        [
        DescriptionKey("DEDUCTIBLE_AMOUNT_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "DEDUCTIBLE_AMOUNT", DbType = System.Data.DbType.Decimal),
        ]
        public decimal? DeductibleAmount
        {
            get
            {
                return this._deductibleAmount;
            }
            set
            {
                this._deductibleAmount = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad VersionCode.
        /// </summary>
        /// <value>Propiedad VersionCode.</value>
        [
        DescriptionKey("VERSION_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "VERSION_CD", DbType = System.Data.DbType.String),
        ]
        public int? VersionCode
        {
            get
            {
                return this._versionCode;
            }
            set
            {
                this._versionCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Date.
        /// </summary>
        /// <value>Propiedad Date.</value>
        [
        DescriptionKey("DATE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "DATE", DbType = System.Data.DbType.String),
        ]
        public DateTime? Date
        {
            get
            {
                return this._date;
            }
            set
            {
                this._date = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad CurrencyCode.
        /// </summary>
        /// <value>Propiedad CurrencyCode.</value>
        [
        DescriptionKey("CURRENCY_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "CURRENCY_CD", DbType = System.Data.DbType.String),
        ]
        public int? CurrencyCode
        {
            get
            {
                return this._currencyCode;
            }
            set
            {
                this._currencyCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad EstimateAmountAccumulate.
        /// </summary>
        /// <value>Propiedad EstimateAmountAccumulate.</value>
        [
        DescriptionKey("ESTIMATE_AMOUNT_ACCUMULATE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "ESTIMATE_AMOUNT_ACCUMULATE", DbType = System.Data.DbType.String),
        ]
        public decimal? EstimateAmountAccumulate
        {
            get
            {
                return this._estimateAmountAccumulate;
            }
            set
            {
                this._estimateAmountAccumulate = value;
            }
        }

    }
}
