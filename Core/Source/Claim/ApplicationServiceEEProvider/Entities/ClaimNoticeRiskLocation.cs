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
    /// Definición de entidad ClaimNoticeRiskLocation.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.ClaimServices.EEProvider.ClaimNoticeRiskLocation.dict"),
    Serializable(),
    DescriptionKey("CLAIM_NOTICE_RISK_LOCATION_ENTITY"),
    TableMap(TableName = "CLAIM_NOTICE_RISK_LOCATION", Schema = "CLM"),
    ]
    public partial class ClaimNoticeRiskLocation :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string ClaimNoticeCode = "ClaimNoticeCode";
            public static readonly string Address = "Address";
            public static readonly string CountryCode = "CountryCode";
            public static readonly string StateCode = "StateCode";
            public static readonly string CityCode = "CityCode";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="claimNoticeCode">Propiedad clave ClaimNoticeCode.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int claimNoticeCode)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.ClaimNoticeCode, claimNoticeCode);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="claimNoticeCode">Propiedad clave ClaimNoticeCode.</param>
        public static PrimaryKey CreatePrimaryKey(int claimNoticeCode)
        {
            return InternalCreatePrimaryKey<ClaimNoticeRiskLocation>(claimNoticeCode);
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad Address.
        /// </summary>
        private string _address = null;
        /// <summary>
        /// Atributo para la propiedad CountryCode.
        /// </summary>
        private int? _countryCode = null;
        /// <summary>
        /// Atributo para la propiedad StateCode.
        /// </summary>
        private int? _stateCode = null;
        /// <summary>
        /// Atributo para la propiedad CityCode.
        /// </summary>
        private int? _cityCode = null;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="claimNoticeCode">ClaimNoticeCode key property.</param>
        public ClaimNoticeRiskLocation(int claimNoticeCode) :
            this(ClaimNoticeRiskLocation.CreatePrimaryKey(claimNoticeCode), null)
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
        public ClaimNoticeRiskLocation(PrimaryKey key, IDictionary initialValues) :
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
        /// Devuelve o setea el valor de la propiedad Address.
        /// </summary>
        /// <value>Propiedad Address.</value>
        [
        DescriptionKey("ADDRESS_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "ADDRESS", DbType = System.Data.DbType.String),
        ]
        public string Address
        {
            get
            {
                return this._address;
            }
            set
            {
                this._address = value;
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

    }
}
