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
    /// Definición de entidad PendingOperations.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.ClaimServices.EEProvider.PendingOperations.dict"),
    Serializable(),
    DescriptionKey("PENDING_OPERATIONS_ENTITY"),
    TableMap(TableName = "PENDING_OPERATIONS", Schema = "CLM"),
    ]
    public partial class PendingOperations :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string Id = "Id";
            public static readonly string User = "User";
            public static readonly string CreationDate = "CreationDate";
            public static readonly string Operation = "Operation";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="id">Propiedad clave Id.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int id)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.Id, id);

            return new PrimaryKey<T>(keys);
        }

        protected static PrimaryKey InternalCreatePrimaryKey<T>()
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.Id, null);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="id">Propiedad clave Id.</param>
        public static PrimaryKey CreatePrimaryKey(int id)
        {
            return InternalCreatePrimaryKey<PendingOperations>(id);
        }

        public static PrimaryKey CreatePrimaryKey()
        {
            return InternalCreatePrimaryKey<PendingOperations>();
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad User.
        /// </summary>
        private int? _user = null;
        /// <summary>
        /// Atributo para la propiedad CreationDate.
        /// </summary>
        private DateTime _creationDate;
        /// <summary>
        /// Atributo para la propiedad Operation.
        /// </summary>
        private string _operation = null;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="id">Id key property.</param>
        public PendingOperations(int id) :
            this(PendingOperations.CreatePrimaryKey(id), null)
        {
        }

        public PendingOperations() :
          this(PendingOperations.CreatePrimaryKey(), null)
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
        public PendingOperations(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        /*** Object Properties ********************************/
        /// <summary>
        /// Devuelve o setea el valor de la propiedad Id.
        /// </summary>
        /// <value>Propiedad Id.</value>
        [
        DescriptionKey("ID_PROPERTY"),
        PersistentProperty(IsKey = true),
        ColumnMap(ColumnName = "ID", DbType = System.Data.DbType.String),
        ]
        public int Id
        {
            get
            {
                return (int)this._primaryKey[Properties.Id];
            }
            set
            {
                this._primaryKey[Properties.Id] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad User.
        /// </summary>
        /// <value>Propiedad User.</value>
        [
        DescriptionKey("USER_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "USER", DbType = System.Data.DbType.String),
        ]
        public int? User
        {
            get
            {
                return this._user;
            }
            set
            {
                this._user = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad CreationDate.
        /// </summary>
        /// <value>Propiedad CreationDate.</value>
        [
        DescriptionKey("CREATION_DATE_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "CREATION_DATE", DbType = System.Data.DbType.String),
        ]
        public DateTime CreationDate
        {
            get
            {
                return this._creationDate;
            }
            set
            {
                this._creationDate = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Operation.
        /// </summary>
        /// <value>Propiedad Operation.</value>
        [
        DescriptionKey("OPERATION_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "OPERATION", DbType = System.Data.DbType.String),
        ]
        public string Operation
        {
            get
            {
                return this._operation;
            }
            set
            {
                if (value == null)
                {
                    throw new PropertyNotNullableException(this.GetType().FullName, Properties.Operation);
                }
                this._operation = value;
            }
        }

    }
}