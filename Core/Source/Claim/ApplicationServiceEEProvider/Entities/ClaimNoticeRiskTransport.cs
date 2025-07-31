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
    /// Definición de entidad ClaimNoticeRiskTransport.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.ClaimServices.EEProvider.ClaimNoticeRiskTransport.dict"),
    Serializable(),
    DescriptionKey("CLAIM_NOTICE_RISK_TRANSPORT_ENTITY"),
    TableMap(TableName = "CLAIM_NOTICE_RISK_TRANSPORT", Schema = "CLM"),
    ]
    public partial class ClaimNoticeRiskTransport :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string ClaimNoticeCode = "ClaimNoticeCode";
            public static readonly string TransportCargoType = "TransportCargoType";
            public static readonly string TransportPackagingType = "TransportPackagingType";
            public static readonly string Source = "Source";
            public static readonly string Destiny = "Destiny";
            public static readonly string TransportViaType = "TransportViaType";
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
            return InternalCreatePrimaryKey<ClaimNoticeRiskTransport>(claimNoticeCode);
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad TransportCargoType.
        /// </summary>
        private string _transportCargoType = null;
        /// <summary>
        /// Atributo para la propiedad TransportPackagingType.
        /// </summary>
        private string _transportPackagingType = null;
        /// <summary>
        /// Atributo para la propiedad Source.
        /// </summary>
        private string _source = null;
        /// <summary>
        /// Atributo para la propiedad Destiny.
        /// </summary>
        private string _destiny = null;
        /// <summary>
        /// Atributo para la propiedad TransportViaType.
        /// </summary>
        private string _transportViaType = null;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="claimNoticeCode">ClaimNoticeCode key property.</param>
        public ClaimNoticeRiskTransport(int claimNoticeCode) :
            this(ClaimNoticeRiskTransport.CreatePrimaryKey(claimNoticeCode), null)
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
        public ClaimNoticeRiskTransport(PrimaryKey key, IDictionary initialValues) :
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
        /// Devuelve o setea el valor de la propiedad TransportCargoType.
        /// </summary>
        /// <value>Propiedad TransportCargoType.</value>
        [
        DescriptionKey("TRANSPORT_CARGO_TYPE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "TRANSPORT_CARGO_TYPE", DbType = System.Data.DbType.String),
        ]
        public string TransportCargoType
        {
            get
            {
                return this._transportCargoType;
            }
            set
            {
                this._transportCargoType = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad TransportPackagingType.
        /// </summary>
        /// <value>Propiedad TransportPackagingType.</value>
        [
        DescriptionKey("TRANSPORT_PACKAGING_TYPE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "TRANSPORT_PACKAGING_TYPE", DbType = System.Data.DbType.String),
        ]
        public string TransportPackagingType
        {
            get
            {
                return this._transportPackagingType;
            }
            set
            {
                this._transportPackagingType = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Source.
        /// </summary>
        /// <value>Propiedad Source.</value>
        [
        DescriptionKey("SOURCE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "SOURCE", DbType = System.Data.DbType.String),
        ]
        public string Source
        {
            get
            {
                return this._source;
            }
            set
            {
                this._source = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Destiny.
        /// </summary>
        /// <value>Propiedad Destiny.</value>
        [
        DescriptionKey("DESTINY_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "DESTINY", DbType = System.Data.DbType.String),
        ]
        public string Destiny
        {
            get
            {
                return this._destiny;
            }
            set
            {
                this._destiny = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad TransportViaType.
        /// </summary>
        /// <value>Propiedad TransportViaType.</value>
        [
        DescriptionKey("TRANSPORT_VIA_TYPE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "TRANSPORT_VIA_TYPE", DbType = System.Data.DbType.String),
        ]
        public string TransportViaType
        {
            get
            {
                return this._transportViaType;
            }
            set
            {
                this._transportViaType = value;
            }
        }

    }
}
