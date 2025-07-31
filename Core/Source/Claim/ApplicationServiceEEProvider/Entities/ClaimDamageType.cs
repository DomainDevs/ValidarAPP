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
    /// Definición de entidad ClaimDamageType.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.ClaimServices.EEProvider.ClaimDamageType.dict"),
    Serializable(),
    DescriptionKey("CLAIM_DAMAGE_TYPE_ENTITY"),
    TableMap(TableName = "CLAIM_DAMAGE_TYPE", Schema = "CLM"),
    ]
    public partial class ClaimDamageType :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string ClaimDamageTypeId = "ClaimDamageTypeId";
            public static readonly string Description = "Description";
            public static readonly string Enabled = "Enabled";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="claimDamageTypeId">Propiedad clave ClaimDamageTypeId.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int claimDamageTypeId)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.ClaimDamageTypeId, claimDamageTypeId);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
		/// Crea una clave primaria para una clase concreta.
		/// </summary>
		/// <param name="concreteClass">Clase concreta.</param>
		/// <returns>Clave primaria.</returns>
	    protected static PrimaryKey InternalCreatePrimaryKey<T>()
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.ClaimDamageTypeId, null);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="claimDamageTypeId">Propiedad clave ClaimDamageTypeId.</param>
        public static PrimaryKey CreatePrimaryKey(int claimDamageTypeId)
        {
            return InternalCreatePrimaryKey<ClaimDamageType>(claimDamageTypeId);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        public static PrimaryKey CreatePrimaryKey()
        {
            return InternalCreatePrimaryKey<ClaimDamageType>();
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        private string _description = null;
        /// <summary>
        /// Atributo para la propiedad Enabled.
        /// </summary>
        private bool? _enabled = null;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="claimDamageTypeId">ClaimDamageTypeId key property.</param>
        public ClaimDamageType(int claimDamageTypeId) :
            this(ClaimDamageType.CreatePrimaryKey(claimDamageTypeId), null)
        {
        }

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        public ClaimDamageType() :
            this(ClaimDamageType.CreatePrimaryKey(), null)
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
        public ClaimDamageType(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        /*** Object Properties ********************************/
        /// <summary>
        /// Devuelve o setea el valor de la propiedad ClaimDamageTypeId.
        /// </summary>
        /// <value>Propiedad ClaimDamageTypeId.</value>
        [
        DescriptionKey("CLAIM_DAMAGE_TYPE_ID_PROPERTY"),
        PersistentProperty(IsKey = true, KeyType = "Identity"),
        ColumnMap(ColumnName = "CLAIM_DAMAGE_TYPE_ID", DbType = System.Data.DbType.String),
        ]
        public int ClaimDamageTypeId
        {
            get
            {
                return (int)this._primaryKey[Properties.ClaimDamageTypeId];
            }
            set
            {
                this._primaryKey[Properties.ClaimDamageTypeId] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Description.
        /// </summary>
        /// <value>Propiedad Description.</value>
        [
        DescriptionKey("DESCRIPTION_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "DESCRIPTION", DbType = System.Data.DbType.String),
        ]
        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                this._description = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Enabled.
        /// </summary>
        /// <value>Propiedad Enabled.</value>
        [
        DescriptionKey("ENABLED_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "ENABLED", DbType = System.Data.DbType.String),
        ]
        public bool? Enabled
        {
            get
            {
                return this._enabled;
            }
            set
            {
                this._enabled = value;
            }
        }

    }
}
