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
    /// Definición de entidad ClaimNoticeCoverage.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.ClaimServices.EEProvider.ClaimNoticeCoverage.dict"),
    Serializable(),
    DescriptionKey("CLAIM_NOTICE_COVERAGE_ENTITY"),
    TableMap(TableName = "CLAIM_NOTICE_COVERAGE", Schema = "CLM"),
    ]
    public partial class ClaimNoticeCoverage :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string ClaimNoticeCode = "ClaimNoticeCode";
            public static readonly string RiskNum = "RiskNum";
            public static readonly string CoverNum = "CoverNum";
            public static readonly string CoverageId = "CoverageId";
            public static readonly string IndivididualId = "IndivididualId";
            public static readonly string IsInsured = "IsInsured";
            public static readonly string EstimateTypeCode = "EstimateTypeCode";
            public static readonly string EstimateAmount = "EstimateAmount";
            public static readonly string IsProspect = "IsProspect";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="claimNoticeCode">Propiedad clave ClaimNoticeCode.</param>
        /// <param name="riskNum">Propiedad clave RiskNum.</param>
        /// <param name="coverNum">Propiedad clave CoverNum.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int claimNoticeCode, int riskNum, int coverNum)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.ClaimNoticeCode, claimNoticeCode);
            keys.Add(Properties.RiskNum, riskNum);
            keys.Add(Properties.CoverNum, coverNum);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="claimNoticeCode">Propiedad clave ClaimNoticeCode.</param>
        /// <param name="riskNum">Propiedad clave RiskNum.</param>
        /// <param name="coverNum">Propiedad clave CoverNum.</param>
        public static PrimaryKey CreatePrimaryKey(int claimNoticeCode, int riskNum, int coverNum)
        {
            return InternalCreatePrimaryKey<ClaimNoticeCoverage>(claimNoticeCode, riskNum, coverNum);
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad CoverageId.
        /// </summary>
        private int? _coverageId = null;
        /// <summary>
        /// Atributo para la propiedad IndivididualId.
        /// </summary>
        private int? _individidualId = null;
        /// <summary>
        /// Atributo para la propiedad IsInsured.
        /// </summary>
        private bool? _isInsured = null;
        /// <summary>
        /// Atributo para la propiedad EstimateTypeCode.
        /// </summary>
        private int _estimateTypeCode;
        /// <summary>
        /// Atributo para la propiedad EstimateAmount.
        /// </summary>
        private decimal _estimateAmount;
        /// <summary>
        /// Atributo para la propiedad IsProspect.
        /// </summary>
        private bool? _isProspect = null;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="claimNoticeCode">ClaimNoticeCode key property.</param>
        /// <param name="riskNum">RiskNum key property.</param>
        /// <param name="coverNum">CoverNum key property.</param>
        public ClaimNoticeCoverage(int claimNoticeCode, int riskNum, int coverNum) :
            this(ClaimNoticeCoverage.CreatePrimaryKey(claimNoticeCode, riskNum, coverNum), null)
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
        public ClaimNoticeCoverage(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        /*** Object Properties ********************************/
        /// <summary>
        /// Devuelve o setea el valor de la propiedad ClaimNoticeCode.
        /// </summary>
        /// <value>Propiedad ClaimNoticeCode.</value>
        [
        DescriptionKey("CLAIM_NOTICE_CODE_PROPERTY"),
        PersistentProperty(IsKey = true),
        ColumnMap(ColumnName = "CLAIM_NOTICE_CD", DbType = System.Data.DbType.String),
        ]
        public int ClaimNoticeCode
        {
            get
            {
                return (int)this._primaryKey[Properties.ClaimNoticeCode];
            }
            set
            {
                this._primaryKey[Properties.ClaimNoticeCode] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad RiskNum.
        /// </summary>
        /// <value>Propiedad RiskNum.</value>
        [
        DescriptionKey("RISK_NUM_PROPERTY"),
        PersistentProperty(IsKey = true),
        ColumnMap(ColumnName = "RISK_NUM", DbType = System.Data.DbType.String),
        ]
        public int RiskNum
        {
            get
            {
                return (int)this._primaryKey[Properties.RiskNum];
            }
            set
            {
                this._primaryKey[Properties.RiskNum] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad CoverNum.
        /// </summary>
        /// <value>Propiedad CoverNum.</value>
        [
        DescriptionKey("COVER_NUM_PROPERTY"),
        PersistentProperty(IsKey = true),
        ColumnMap(ColumnName = "COVER_NUM", DbType = System.Data.DbType.String),
        ]
        public int CoverNum
        {
            get
            {
                return (int)this._primaryKey[Properties.CoverNum];
            }
            set
            {
                this._primaryKey[Properties.CoverNum] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad CoverageId.
        /// </summary>
        /// <value>Propiedad CoverageId.</value>
        [
        DescriptionKey("COVERAGE_ID_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "COVERAGE_ID", DbType = System.Data.DbType.String),
        ]
        public int? CoverageId
        {
            get
            {
                return this._coverageId;
            }
            set
            {
                this._coverageId = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad IndivididualId.
        /// </summary>
        /// <value>Propiedad IndivididualId.</value>
        [
        DescriptionKey("INDIVIDIDUAL_ID_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "INDIVIDIDUAL_ID", DbType = System.Data.DbType.String),
        ]
        public int? IndivididualId
        {
            get
            {
                return this._individidualId;
            }
            set
            {
                this._individidualId = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad IsInsured.
        /// </summary>
        /// <value>Propiedad IsInsured.</value>
        [
        DescriptionKey("IS_INSURED_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "IS_INSURED", DbType = System.Data.DbType.String),
        ]
        public bool? IsInsured
        {
            get
            {
                return this._isInsured;
            }
            set
            {
                this._isInsured = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad EstimateTypeCode.
        /// </summary>
        /// <value>Propiedad EstimateTypeCode.</value>
        [
        DescriptionKey("ESTIMATE_TYPE_CODE_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "ESTIMATE_TYPE_CD", DbType = System.Data.DbType.String),
        ]
        public int EstimateTypeCode
        {
            get
            {
                return this._estimateTypeCode;
            }
            set
            {
                this._estimateTypeCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad EstimateAmount.
        /// </summary>
        /// <value>Propiedad EstimateAmount.</value>
        [
        DescriptionKey("ESTIMATE_AMOUNT_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "ESTIMATE_AMOUNT", DbType = System.Data.DbType.Decimal),
        ]
        public decimal EstimateAmount
        {
            get
            {
                return this._estimateAmount;
            }
            set
            {
                this._estimateAmount = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad IsProspect.
        /// </summary>
        /// <value>Propiedad IsProspect.</value>
        [
        DescriptionKey("IS_PROSPECT_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "IS_PROSPECT", DbType = System.Data.DbType.String),
        ]
        public bool? IsProspect
        {
            get
            {
                return this._isProspect;
            }
            set
            {
                this._isProspect = value;
            }
        }

    }
}
