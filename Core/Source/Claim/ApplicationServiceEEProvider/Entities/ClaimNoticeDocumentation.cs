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
    /// Definición de entidad ClaimNoticeDocumentation.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.ClaimServices.EEProvider.ClaimNoticeDocumentation.dict"),
    Serializable(),
    DescriptionKey("CLAIM_NOTICE_DOCUMENTATION_ENTITY"),
    TableMap(TableName = "CLAIM_NOTICE_DOCUMENTATION", Schema = "CLM"),
    ]
    public partial class ClaimNoticeDocumentation :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string ClaimNoticeCode = "ClaimNoticeCode";
            public static readonly string DocumentCode = "DocumentCode";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="claimNoticeCode">Propiedad clave ClaimNoticeCode.</param>
        /// <param name="documentCode">Propiedad clave DocumentCode.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int claimNoticeCode, int documentCode)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.ClaimNoticeCode, claimNoticeCode);
            keys.Add(Properties.DocumentCode, documentCode);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="claimNoticeCode">Propiedad clave ClaimNoticeCode.</param>
        /// <param name="documentCode">Propiedad clave DocumentCode.</param>
        public static PrimaryKey CreatePrimaryKey(int claimNoticeCode, int documentCode)
        {
            return InternalCreatePrimaryKey<ClaimNoticeDocumentation>(claimNoticeCode, documentCode);
        }
        #endregion

        //*** Object Attributes ********************************

        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="claimNoticeCode">ClaimNoticeCode key property.</param>
        /// <param name="documentCode">DocumentCode key property.</param>
        public ClaimNoticeDocumentation(int claimNoticeCode, int documentCode) :
            this(ClaimNoticeDocumentation.CreatePrimaryKey(claimNoticeCode, documentCode), null)
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
        public ClaimNoticeDocumentation(PrimaryKey key, IDictionary initialValues) :
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
        /// Devuelve o setea el valor de la propiedad DocumentCode.
        /// </summary>
        /// <value>Propiedad DocumentCode.</value>
        [
        DescriptionKey("DOCUMENT_CODE_PROPERTY"),
        PersistentProperty(IsKey = true),
        ColumnMap(ColumnName = "DOCUMENT_CD", DbType = System.Data.DbType.String),
        ]
        public int DocumentCode
        {
            get
            {
                return (int)this._primaryKey[Properties.DocumentCode];
            }
            set
            {
                this._primaryKey[Properties.DocumentCode] = value;
            }
        }

    }
}
