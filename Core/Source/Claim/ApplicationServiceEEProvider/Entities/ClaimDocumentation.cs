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
    /// Definición de entidad ClaimDocumentation.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.ClaimServices.EEProvider.ClaimDocumentation.dict"),
    Serializable(),
    DescriptionKey("CLAIM_DOCUMENTATION_ENTITY"),
    TableMap(TableName = "CLAIM_DOCUMENTATION", Schema = "CLM"),
    ]
    public partial class ClaimDocumentation :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string ClaimCode = "ClaimCode";
            public static readonly string DocumentCode = "DocumentCode";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="claimCode">Propiedad clave ClaimCode.</param>
        /// <param name="documentCode">Propiedad clave DocumentCode.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int claimCode, int documentCode)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.ClaimCode, claimCode);
            keys.Add(Properties.DocumentCode, documentCode);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="claimCode">Propiedad clave ClaimCode.</param>
        /// <param name="documentCode">Propiedad clave DocumentCode.</param>
        public static PrimaryKey CreatePrimaryKey(int claimCode, int documentCode)
        {
            return InternalCreatePrimaryKey<ClaimDocumentation>(claimCode, documentCode);
        }
        #endregion

        //*** Object Attributes ********************************

        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="claimCode">ClaimCode key property.</param>
        /// <param name="documentCode">DocumentCode key property.</param>
        public ClaimDocumentation(int claimCode, int documentCode) :
            this(ClaimDocumentation.CreatePrimaryKey(claimCode, documentCode), null)
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
        public ClaimDocumentation(PrimaryKey key, IDictionary initialValues) :
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
