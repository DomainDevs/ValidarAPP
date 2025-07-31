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
    /// Definición de entidad ClaimCoverage.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.ClaimServices.EEProvider.ClaimCoverage.dict"),
    Serializable(),
    DescriptionKey("CLAIM_COVERAGE_ENTITY"),
    TableMap(TableName = "CLAIM_COVERAGE", Schema = "CLM"),
    ]
    public partial class ClaimCoverage :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string ClaimCoverageCode = "ClaimCoverageCode";
            public static readonly string SubClaim = "SubClaim";
            public static readonly string ClaimModifyCode = "ClaimModifyCode";
            public static readonly string RiskNum = "RiskNum";
            public static readonly string CoverageNum = "CoverageNum";
            public static readonly string IndividualId = "IndividualId";
            public static readonly string RiskId = "RiskId";
            public static readonly string CoverageId = "CoverageId";
            public static readonly string IsInsured = "IsInsured";
            public static readonly string IsProspect = "IsProspect";
            public static readonly string TextOperationCode = "TextOperationCode";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="claimCoverageCode">Propiedad clave ClaimCoverageCode.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int claimCoverageCode)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.ClaimCoverageCode, claimCoverageCode);

            return new PrimaryKey<T>(keys);
        }

        protected static PrimaryKey InternalCreatePrimaryKey<T>()
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.ClaimCoverageCode, null);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="claimCoverageCode">Propiedad clave ClaimCoverageCode.</param>
        public static PrimaryKey CreatePrimaryKey(int claimCoverageCode)
        {
            return InternalCreatePrimaryKey<ClaimCoverage>(claimCoverageCode);
        }

        public static PrimaryKey CreatePrimaryKey()
        {
            return InternalCreatePrimaryKey<ClaimCoverage>();
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad SubClaim.
        /// </summary>
        private int? _subClaim = null;
        /// <summary>
        /// Atributo para la propiedad ClaimCode.
        /// </summary>
        private int _claimModifyCode;
        /// <summary>
        /// Atributo para la propiedad RiskNum.
        /// </summary>
        private int _riskNum;
        /// <summary>
        /// Atributo para la propiedad CoverageNum.
        /// </summary>
        private int _coverageNum;
        /// <summary>
        /// Atributo para la propiedad IndividualId.
        /// </summary>
        private int _individualId;
        /// <summary>
        /// Atributo para la propiedad RiskId.
        /// </summary>
        private int? _riskId = null;
        /// <summary>
        /// Atributo para la propiedad CoverageId.
        /// </summary>
        private int? _coverageId = null;
        /// <summary>
        /// Atributo para la propiedad IsInsured.
        /// </summary>
        private bool? _isInsured = null;
        /// <summary>
        /// Atributo para la propiedad IsProspect.
        /// </summary>
        private bool? _isProspect = null;
        /// <summary>
        /// Atributo para la propiedad TextOperationCode.
        /// </summary>
        private int? _textOperationCode = null;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="claimCoverageCode">ClaimCoverageCode key property.</param>
        public ClaimCoverage(int claimCoverageCode) :
            this(ClaimCoverage.CreatePrimaryKey(claimCoverageCode), null)
        {
        }
        public ClaimCoverage() :
            this(ClaimCoverage.CreatePrimaryKey(), null)
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
        public ClaimCoverage(PrimaryKey key, IDictionary initialValues) :
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
        PersistentProperty(IsKey = true, KeyType = "Identity"),
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
        /// Devuelve o setea el valor de la propiedad SubClaim.
        /// </summary>
        /// <value>Propiedad SubClaim.</value>
        [
        DescriptionKey("SUB_CLAIM_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "SUB_CLAIM", DbType = System.Data.DbType.String),
        ]
        public int? SubClaim
        {
            get
            {
                return this._subClaim;
            }
            set
            {
                this._subClaim = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad ClaimCode.
        /// </summary>
        /// <value>Propiedad ClaimCode.</value>
        [
        DescriptionKey("CLAIM_MODIFY_CODE_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "CLAIM_MODIFY_CD", DbType = System.Data.DbType.String),
        ]
        public int ClaimModifyCode
        {
            get
            {
                return this._claimModifyCode;
            }
            set
            {
                this._claimModifyCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad RiskNum.
        /// </summary>
        /// <value>Propiedad RiskNum.</value>
        [
        DescriptionKey("RISK_NUM_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "RISK_NUM", DbType = System.Data.DbType.String),
        ]
        public int RiskNum
        {
            get
            {
                return this._riskNum;
            }
            set
            {
                this._riskNum = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad CoverageNum.
        /// </summary>
        /// <value>Propiedad CoverageNum.</value>
        [
        DescriptionKey("COVERAGE_NUM_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "COVERAGE_NUM", DbType = System.Data.DbType.String),
        ]
        public int CoverageNum
        {
            get
            {
                return this._coverageNum;
            }
            set
            {
                this._coverageNum = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad IndividualId.
        /// </summary>
        /// <value>Propiedad IndividualId.</value>
        [
        DescriptionKey("INDIVIDUAL_ID_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "INDIVIDUAL_ID", DbType = System.Data.DbType.String),
        ]
        public int IndividualId
        {
            get
            {
                return this._individualId;
            }
            set
            {
                this._individualId = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad RiskId.
        /// </summary>
        /// <value>Propiedad RiskId.</value>
        [
        DescriptionKey("RISK_ID_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "RISK_ID", DbType = System.Data.DbType.String),
        ]
        public int? RiskId
        {
            get
            {
                return this._riskId;
            }
            set
            {
                this._riskId = value;
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

        /// <summary>
        /// Devuelve o setea el valor de la propiedad TextOperationCode.
        /// </summary>
        /// <value>Propiedad TextOperationCode.</value>
        [
        DescriptionKey("TEXT_OPERATION_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "TEXT_OPERATION_CD", DbType = System.Data.DbType.String),
        ]
        public int? TextOperationCode
        {
            get
            {
                return this._textOperationCode;
            }
            set
            {
                this._textOperationCode = value;
            }
        }

    }
}
