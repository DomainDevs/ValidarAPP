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
    /// Definición de entidad ClaimNoticeContactInformation.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.ClaimServices.EEProvider.ClaimNoticeContactInformation.dict"),
    Serializable(),
    DescriptionKey("CLAIM_NOTICE_CONTACT_INFORMATION_ENTITY"),
    TableMap(TableName = "CLAIM_NOTICE_CONTACT_INFORMATION", Schema = "CLM"),
    ]
    public partial class ClaimNoticeContactInformation :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string ClaimNoticeCode = "ClaimNoticeCode";
            public static readonly string Name = "Name";
            public static readonly string Phone = "Phone";
            public static readonly string Mail = "Mail";
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
            return InternalCreatePrimaryKey<ClaimNoticeContactInformation>(claimNoticeCode);
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad Name.
        /// </summary>
        private string _name = null;
        /// <summary>
        /// Atributo para la propiedad Phone.
        /// </summary>
        private string _phone = null;
        /// <summary>
        /// Atributo para la propiedad Mail.
        /// </summary>
        private string _mail = null;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="claimNoticeCode">ClaimNoticeCode key property.</param>
        public ClaimNoticeContactInformation(int claimNoticeCode) :
            this(ClaimNoticeContactInformation.CreatePrimaryKey(claimNoticeCode), null)
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
        public ClaimNoticeContactInformation(PrimaryKey key, IDictionary initialValues) :
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
        /// Devuelve o setea el valor de la propiedad Name.
        /// </summary>
        /// <value>Propiedad Name.</value>
        [
        DescriptionKey("NAME_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "NAME", DbType = System.Data.DbType.String),
        ]
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (value == null)
                {
                    throw new PropertyNotNullableException(this.GetType().FullName, Properties.Name);
                }
                this._name = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Phone.
        /// </summary>
        /// <value>Propiedad Phone.</value>
        [
        DescriptionKey("PHONE_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "PHONE", DbType = System.Data.DbType.String),
        ]
        public string Phone
        {
            get
            {
                return this._phone;
            }
            set
            {
                if (value == null)
                {
                    throw new PropertyNotNullableException(this.GetType().FullName, Properties.Phone);
                }
                this._phone = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Mail.
        /// </summary>
        /// <value>Propiedad Mail.</value>
        [
        DescriptionKey("MAIL_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "MAIL", DbType = System.Data.DbType.String),
        ]
        public string Mail
        {
            get
            {
                return this._mail;
            }
            set
            {
                this._mail = value;
            }
        }

    }
}
