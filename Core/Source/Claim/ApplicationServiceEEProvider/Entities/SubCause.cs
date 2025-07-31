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
    /// Definición de entidad SubCause.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.ClaimServices.EEProvider.SubCause.dict"),
    Serializable(),
    DescriptionKey("SUB_CAUSE_ENTITY"),
    TableMap(TableName = "SUB_CAUSE", Schema = "CLM"),
    ]
    public partial class SubCause :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string SubcauseId = "SubcauseId";
            public static readonly string Description = "Description";
            public static readonly string CauseCode = "CauseCode";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="subcauseId">Propiedad clave SubcauseId.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int subcauseId)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.SubcauseId, subcauseId);

            return new PrimaryKey<T>(keys);
        }

        protected static PrimaryKey InternalCreatePrimaryKey<T>()
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.SubcauseId, null);

            return new PrimaryKey<T>(keys);
        }
        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="subcauseId">Propiedad clave SubcauseId.</param>
        public static PrimaryKey CreatePrimaryKey(int subcauseId)
        {
            return InternalCreatePrimaryKey<SubCause>(subcauseId);
        }

        public static PrimaryKey CreatePrimaryKey()
        {
            return InternalCreatePrimaryKey<SubCause>();
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        private string _description = null;
        /// <summary>
        /// Atributo para la propiedad CauseCode.
        /// </summary>
        private int? _causeCode = null;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="subcauseId">SubcauseId key property.</param>
        public SubCause(int subcauseId) :
            this(SubCause.CreatePrimaryKey(subcauseId), null)
        {
        }

        public SubCause() :
           this(SubCause.CreatePrimaryKey(), null)
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
        public SubCause(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        /*** Object Properties ********************************/
        /// <summary>
        /// Devuelve o setea el valor de la propiedad SubcauseId.
        /// </summary>
        /// <value>Propiedad SubcauseId.</value>
        [
        DescriptionKey("SUBCAUSE_ID_PROPERTY"),
        PersistentProperty(IsKey = true),
        ColumnMap(ColumnName = "SUBCAUSE_ID", DbType = System.Data.DbType.String),
        ]
        public int SubcauseId
        {
            get
            {
                return (int)this._primaryKey[Properties.SubcauseId];
            }
            set
            {
                this._primaryKey[Properties.SubcauseId] = value;
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
        /// Devuelve o setea el valor de la propiedad CauseCode.
        /// </summary>
        /// <value>Propiedad CauseCode.</value>
        [
        DescriptionKey("CAUSE_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "CAUSE_CD", DbType = System.Data.DbType.String),
        ]
        public int? CauseCode
        {
            get
            {
                return this._causeCode;
            }
            set
            {
                this._causeCode = value;
            }
        }

    }
}
