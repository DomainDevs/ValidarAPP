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
    /// Definición de entidad Claim.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.ClaimServices.EEProvider.Claim.dict"),
    Serializable(),
    DescriptionKey("CLAIM_ENTITY"),
    TableMap(TableName = "CLAIM", Schema = "CLM"),
    ]
    public partial class Claim :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string ClaimCode = "ClaimCode";
            public static readonly string OccurrenceDate = "OccurrenceDate";
            public static readonly string NoticeDate = "NoticeDate";
            public static readonly string ClaimNoticeCode = "ClaimNoticeCode";
            public static readonly string ClaimBranchCode = "ClaimBranchCode";
            public static readonly string PolicyId = "PolicyId";
            public static readonly string EndorsementId = "EndorsementId";
            public static readonly string IndividualId = "IndividualId";
            public static readonly string BusinessTypeCode = "BusinessTypeCode";
            public static readonly string Number = "Number";
            public static readonly string PrefixCode = "PrefixCode";
            public static readonly string DocumentNumber = "DocumentNumber";
            public static readonly string ClaimDamageTypeCode = "ClaimDamageTypeCode";
            public static readonly string ClaimDamageResponsibilityCode = "ClaimDamageResponsibilityCode";
            public static readonly string Location = "Location";
            public static readonly string CountryCode = "CountryCode";
            public static readonly string CityCode = "CityCode";
            public static readonly string StateCode = "StateCode";
            public static readonly string CauseId = "CauseId";
            public static readonly string TextOperationCode = "TextOperationCode";
            public static readonly string IsTotalParticipation = "IsTotalParticipation";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="claimCode">Propiedad clave ClaimCode.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int claimCode)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.ClaimCode, claimCode);

            return new PrimaryKey<T>(keys);
        }

        protected static PrimaryKey InternalCreatePrimaryKey<T>()
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.ClaimCode, null);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="claimCode">Propiedad clave ClaimCode.</param>
        public static PrimaryKey CreatePrimaryKey(int claimCode)
        {
            return InternalCreatePrimaryKey<Claim>(claimCode);
        }

        public static PrimaryKey CreatePrimaryKey()
        {
            return InternalCreatePrimaryKey<Claim>();
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad OccurrenceDate.
        /// </summary>
        private DateTime? _occurrenceDate = null;
        /// <summary>
        /// Atributo para la propiedad NoticeDate.
        /// </summary>
        private DateTime? _noticeDate = null;
        /// <summary>
        /// Atributo para la propiedad ClaimNoticeCode.
        /// </summary>
        private int? _claimNoticeCode = null;
        /// <summary>
        /// Atributo para la propiedad ClaimBranchCode.
        /// </summary>
        private int? _claimBranchCode = null;
        /// <summary>
        /// Atributo para la propiedad PolicyId.
        /// </summary>
        private int? _policyId = null;
        /// <summary>
        /// Atributo para la propiedad EndorsementId.
        /// </summary>
        private int? _endorsementId = null;
        /// <summary>
        /// Atributo para la propiedad IndividualId.
        /// </summary>
        private int? _individualId = null;
        /// <summary>
        /// Atributo para la propiedad BusinnesTypeCode.
        /// </summary>
        private int? _businessTypeCode = null;
        /// <summary>
        /// Atributo para la propiedad Number.
        /// </summary>
        private int? _number = null;
        /// <summary>
        /// Atributo para la propiedad PrefixCode.
        /// </summary>
        private int? _prefixCode = null;
        /// <summary>
        /// Atributo para la propiedad DocumentNumber.
        /// </summary>
        private string _documentNumber = null;
        /// <summary>
        /// Atributo para la propiedad ClaimDamageTypeCode.
        /// </summary>
        private int? _claimDamageTypeCode = null;
        /// <summary>
        /// Atributo para la propiedad ClaimDamageResponsibilityCode.
        /// </summary>
        private int? _claimDamageResponsibilityCode = null;
        /// <summary>
        /// Atributo para la propiedad Location.
        /// </summary>
        private string _location = null;
        /// <summary>
        /// Atributo para la propiedad CountryCode.
        /// </summary>
        private int? _countryCode = null;
        /// <summary>
        /// Atributo para la propiedad CityCode.
        /// </summary>
        private int? _cityCode = null;
        /// <summary>
        /// Atributo para la propiedad StateCode.
        /// </summary>
        private int? _stateCode = null;
        /// <summary>
        /// Atributo para la propiedad CauseId.
        /// </summary>
        private int? _causeId = null;
        /// <summary>
		/// Atributo para la propiedad Description.
		/// </summary>
        private int? _textOperationCode = null;
        /// <summary>
        /// Atributo para la propiedad IsTotalParticipation.
        /// </summary>
        private bool _isTotalParticipation;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="claimCode">ClaimCode key property.</param>
        public Claim(int claimCode) :
            this(Claim.CreatePrimaryKey(claimCode), null)
        {
        }

        public Claim() :
            this(Claim.CreatePrimaryKey(), null)
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
        public Claim(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        /*** Object Properties ********************************/
        /// <summary>
        /// Devuelve o setea el valor de la propiedad ClaimCode.
        /// </summary>
        /// <value>Propiedad ClaimCode.</value>
        [
        DescriptionKey("CLAIM_CODE_PROPERTY"),
        PersistentProperty(IsKey = true),
        ColumnMap(ColumnName = "CLAIM_CD", DbType = System.Data.DbType.String),
        ]
        public int ClaimCode
        {
            get
            {
                return (int)this._primaryKey[Properties.ClaimCode];
            }
            set
            {
                this._primaryKey[Properties.ClaimCode] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad OccurrenceDate.
        /// </summary>
        /// <value>Propiedad Date.</value>
        [
        DescriptionKey("OCCURRENCE_DATE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "OCCURRENCE_DATE", DbType = System.Data.DbType.String),
        ]
        public DateTime? OccurrenceDate
        {
            get
            {
                return this._occurrenceDate;
            }
            set
            {
                this._occurrenceDate = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad NoticeDate.
        /// </summary>
        /// <value>Propiedad NoticeDate.</value>
        [
        DescriptionKey("NOTICE_DATE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "NOTICE_DATE", DbType = System.Data.DbType.String),
        ]
        public DateTime? NoticeDate
        {
            get
            {
                return this._noticeDate;
            }
            set
            {
                this._noticeDate = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad ClaimNoticeCode.
        /// </summary>
        /// <value>Propiedad ClaimNoticeCode.</value>
        [
        DescriptionKey("CLAIM_NOTICE_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "CLAIM_NOTICE_CD", DbType = System.Data.DbType.String),
        ]
        public int? ClaimNoticeCode
        {
            get
            {
                return this._claimNoticeCode;
            }
            set
            {
                this._claimNoticeCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad ClaimBranchCode.
        /// </summary>
        /// <value>Propiedad ClaimBranchCode.</value>
        [
        DescriptionKey("CLAIM_BRANCH_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "CLAIM_BRANCH_CD", DbType = System.Data.DbType.String),
        ]
        public int? ClaimBranchCode
        {
            get
            {
                return this._claimBranchCode;
            }
            set
            {
                this._claimBranchCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad PolicyId.
        /// </summary>
        /// <value>Propiedad PolicyId.</value>
        [
        DescriptionKey("POLICY_ID_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "POLICY_ID", DbType = System.Data.DbType.String),
        ]
        public int? PolicyId
        {
            get
            {
                return this._policyId;
            }
            set
            {
                this._policyId = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad EndorsementId.
        /// </summary>
        /// <value>Propiedad EndorsementId.</value>
        [
        DescriptionKey("ENDORSEMENT_ID_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "ENDORSEMENT_ID", DbType = System.Data.DbType.String),
        ]
        public int? EndorsementId
        {
            get
            {
                return this._endorsementId;
            }
            set
            {
                this._endorsementId = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad IndividualId.
        /// </summary>
        /// <value>Propiedad IndividualId.</value>
        [
        DescriptionKey("INDIVIDUAL_ID_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "INDIVIDUAL_ID", DbType = System.Data.DbType.String),
        ]
        public int? IndividualId
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
        /// Devuelve o setea el valor de la propiedad BusinnesTypeCode.
        /// </summary>
        /// <value>Propiedad BusinnesTypeCode.</value>
        [
        DescriptionKey("BUSINESS_TYPE_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "BUSINESS_TYPE_CD", DbType = System.Data.DbType.String),
        ]
        public int? BusinessTypeCode
        {
            get
            {
                return this._businessTypeCode;
            }
            set
            {
                this._businessTypeCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Number.
        /// </summary>
        /// <value>Propiedad Number.</value>
        [
        DescriptionKey("NUMBER_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "NUMBER", DbType = System.Data.DbType.String),
        ]
        public int? Number
        {
            get
            {
                return this._number;
            }
            set
            {
                this._number = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad PrefixCode.
        /// </summary>
        /// <value>Propiedad PrefixCode.</value>
        [
        DescriptionKey("PREFIX_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "PREFIX_CD", DbType = System.Data.DbType.String),
        ]
        public int? PrefixCode
        {
            get
            {
                return this._prefixCode;
            }
            set
            {
                this._prefixCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad DocumentNumber.
        /// </summary>
        /// <value>Propiedad DocumentNumber.</value>
        [
        DescriptionKey("DOCUMENT_NUMBER_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "DOCUMENT_NUMBER", DbType = System.Data.DbType.String),
        ]
        public string DocumentNumber
        {
            get
            {
                return this._documentNumber;
            }
            set
            {
                this._documentNumber = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad ClaimDamageTypeCode.
        /// </summary>
        /// <value>Propiedad ClaimDamageTypeCode.</value>
        [
        DescriptionKey("CLAIM_DAMAGE_TYPE_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "CLAIM_DAMAGE_TYPE_CD", DbType = System.Data.DbType.String),
        ]
        public int? ClaimDamageTypeCode
        {
            get
            {
                return this._claimDamageTypeCode;
            }
            set
            {
                this._claimDamageTypeCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad ClaimDamageResponsibilityCode.
        /// </summary>
        /// <value>Propiedad ClaimDamageResponsibilityCode.</value>
        [
        DescriptionKey("CLAIM_DAMAGE_RESPONSIBILITY_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "CLAIM_DAMAGE_RESPONSIBILITY_CD", DbType = System.Data.DbType.String),
        ]
        public int? ClaimDamageResponsibilityCode
        {
            get
            {
                return this._claimDamageResponsibilityCode;
            }
            set
            {
                this._claimDamageResponsibilityCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Location.
        /// </summary>
        /// <value>Propiedad Location.</value>
        [
        DescriptionKey("LOCATION_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "LOCATION", DbType = System.Data.DbType.String),
        ]
        public string Location
        {
            get
            {
                return this._location;
            }
            set
            {
                this._location = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad CountryCode.
        /// </summary>
        /// <value>Propiedad CountryCode.</value>
        [
        DescriptionKey("COUNTRY_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "COUNTRY_CD", DbType = System.Data.DbType.String),
        ]
        public int? CountryCode
        {
            get
            {
                return this._countryCode;
            }
            set
            {
                this._countryCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad CityCode.
        /// </summary>
        /// <value>Propiedad CityCode.</value>
        [
        DescriptionKey("CITY_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "CITY_CD", DbType = System.Data.DbType.String),
        ]
        public int? CityCode
        {
            get
            {
                return this._cityCode;
            }
            set
            {
                this._cityCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad StateCode.
        /// </summary>
        /// <value>Propiedad StateCode.</value>
        [
        DescriptionKey("STATE_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "STATE_CD", DbType = System.Data.DbType.String),
        ]
        public int? StateCode
        {
            get
            {
                return this._stateCode;
            }
            set
            {
                this._stateCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad CauseId.
        /// </summary>
        /// <value>Propiedad CauseId.</value>
        [
        DescriptionKey("CAUSE_ID_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "CAUSE_ID", DbType = System.Data.DbType.String),
        ]
        public int? CauseId
        {
            get
            {
                return this._causeId;
            }
            set
            {
                this._causeId = value;
            }
        }

        /// <summary>
		/// Devuelve o setea el valor de la propiedad Description.
		/// </summary>
		/// <value>Propiedad Description.</value>
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

        /// <summary>
		/// Devuelve o setea el valor de la propiedad IsTotalParticipation.
		/// </summary>
		/// <value>Propiedad IsTotalParticipation.</value>
	    [
        DescriptionKey("IS_TOTAL_PARTICIPATION_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "IS_TOTAL_PARTICIPATION", DbType = System.Data.DbType.String),
        ]
        public bool IsTotalParticipation
        {
            get
            {
                return this._isTotalParticipation;
            }
            set
            {
                this._isTotalParticipation = value;
            }
        }
    }
}
