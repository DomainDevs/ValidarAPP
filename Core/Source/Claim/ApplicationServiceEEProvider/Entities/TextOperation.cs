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
    /// Definición de entidad TextOperation.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.ClaimServices.EEProvider.TextOperation.dict"),
    Serializable(),
    DescriptionKey("TEXT_OPERATION_ENTITY"),
    TableMap(TableName = "TEXT_OPERATION", Schema = "CLM"),
    ]
    public partial class TextOperation :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string Id = "Id";
            public static readonly string TextDescription = "TextDescription";
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
            return InternalCreatePrimaryKey<TextOperation>(id);
        }

        public static PrimaryKey CreatePrimaryKey()
        {
            return InternalCreatePrimaryKey<TextOperation>();
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad TextDescription.
        /// </summary>
        private string _textDescription = null;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="id">Id key property.</param>
        public TextOperation(int id) :
            this(TextOperation.CreatePrimaryKey(id), null)
        {
        }

        public TextOperation() :
           this(TextOperation.CreatePrimaryKey(), null)
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
        public TextOperation(PrimaryKey key, IDictionary initialValues) :
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
        /// Devuelve o setea el valor de la propiedad TextDescription.
        /// </summary>
        /// <value>Propiedad TextDescription.</value>
        [
        DescriptionKey("TEXT_DESCRIPTION_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "TEXT_DESCRIPTION", DbType = System.Data.DbType.String),
        ]
        public string TextDescription
        {
            get
            {
                return this._textDescription;
            }
            set
            {
                if (value == null)
                {
                    throw new PropertyNotNullableException(this.GetType().FullName, Properties.TextDescription);
                }
                this._textDescription = value;
            }
        }

    }
}
