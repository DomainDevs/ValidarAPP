using System;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework;
using System.Collections.Specialized;
using System.Collections;

namespace Sistran.Core.Application.CollectiveServices.EEProvider.Entities
{
    /// <summary>
    /// Definición de la entidad MassiveLoadFields
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.CollectiveServices.EEProvider.MassiveLoadFields.dict"),
    Serializable(),
    DescriptionKey("MASSIVE_LOAD_FIELD_ENTITY")
    ]
    public class MassiveLoadFields : BusinessObject
    {
        #region static

        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string MassiveFieldId = "MassiveFieldId";
            public static readonly string MassiveFieldName = "MassiveFieldName";
            public static readonly string MassiveFieldDescription = "MassiveFieldDescription";
            public static readonly string FieldLong = "FieldLong";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey(Type concreteClass)
        {
            ListDictionary keys = new ListDictionary();
            keys.Add(Properties.MassiveFieldId, null);

            return new PrimaryKey(concreteClass, keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        public static PrimaryKey CreatePrimaryKey()
        {
            return InternalCreatePrimaryKey(typeof(MassiveLoadFields));
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="massiveFieldId">Propiedad clave MassiveFieldId.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey(Type concreteClass, int massiveFieldId)
        {
            ListDictionary keys = new ListDictionary();
            keys.Add(Properties.MassiveFieldId, massiveFieldId);

            return new PrimaryKey(concreteClass, keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="massiveFieldId">Propiedad clave MassiveFieldId.</param>
        public static PrimaryKey CreatePrimaryKey(int massiveFieldId)
        {
            return InternalCreatePrimaryKey(typeof(MassiveLoadFields), massiveFieldId);
        }

        #endregion

        #region Object Attributes

        /// <summary>
        /// Atributo para la propiedad MassiveFieldName.
        /// </summary>
        private string _massiveFieldName = null;

        /// <summary>
        /// Atributo para la propiedad MassiveFieldDescription.
        /// </summary>
        private string _massiveFieldDescription = null;

        /// <summary>
        /// Atributo para la propiedad FieldLong.
        /// </summary>
        private int? _fieldLong = null;

        #endregion

        #region Object Constructors

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves que no son autonumeradas.
        /// </summary>
        /// <returns>Primary key.</returns>
        public MassiveLoadFields() :
            this(MassiveLoadFields.CreatePrimaryKey(), null)
        {
        }

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="massiveFieldId">MassiveFieldId key property.</param>
        public MassiveLoadFields(int massiveFieldId) :
            this(MassiveLoadFields.CreatePrimaryKey(massiveFieldId), null)
        {
        }

        /// <summary>
        /// Constructor de instancia de la clase en base a una clave primaria y a valores iniciales.
        /// </summary>
        /// <param name="key"> Identificador de la instancia de la entidad.</param>
        /// <param name="initialValues"> Valores para establecer el estado de la instancia.</param>
        public MassiveLoadFields(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        #endregion

        #region Object Properties

        /// <summary>
        /// Devuelve o setea el valor de la propiedad MassiveFieldId.
        /// </summary>
        /// <value>Propiedad MassiveFieldId.</value>
        [
            DescriptionKey("MASSIVE_FIELD_ID_PROPERTY"),
            PersistentProperty(IsKey = true, IsAutomatic = true)
        ]
        public int MassiveFieldId
        {
            get
            {
                return (int)this._primaryKey[Properties.MassiveFieldId];
            }
            set
            {
                this._primaryKey[Properties.MassiveFieldId] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad MassiveFieldName.
        /// </summary>
        /// <value>Propiedad MassiveFieldName.</value>
        [
            DescriptionKey("MASSIVE_FIELD_NAME_PROPERTY"),
            PersistentProperty()
        ]
        public string MassiveFieldName
        {
            get
            {
                return this._massiveFieldName;
            }
            set
            {
                this._massiveFieldName = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad MassiveFieldDescription.
        /// </summary>
        /// <value>Propiedad MassiveFieldDescription.</value>
        [
            DescriptionKey("MASSIVE_FIELD_DESCRIPTION_PROPERTY"),
            PersistentProperty()
        ]
        public string MassiveFieldDescription
        {
            get
            {
                return this._massiveFieldDescription;
            }
            set
            {
                this._massiveFieldDescription = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad FieldLong.
        /// </summary>
        /// <value>Propiedad FieldLong.</value>
        [
            DescriptionKey("FIELD_LONG_PROPERTY"),
            PersistentProperty()
        ]
        public int? FieldLong
        {
            get
            {
                return this._fieldLong;
            }
            set
            {
                this._fieldLong = value;
            }
        }


        /// <summary>
        /// Establecer los valores de las propiedades que no forman parte de la clave o identificador de la instancia.
        /// </summary>
        /// <param name="values"> Valores para establecer el estado de la instancia. </param>
        public override void SetPropertyValues(IDictionary values)
        {
            base.SetPropertyValues(values);

            object value;

            value = values[Properties.MassiveFieldName];
            this.MassiveFieldName = (string)value;

            value = values[Properties.MassiveFieldDescription];
            this.MassiveFieldDescription = (string)values[Properties.MassiveFieldDescription];

            value = values[Properties.FieldLong];
            if (value == null)
            {
                this.FieldLong = null;
            }
            else
            {
                this.FieldLong = (int)value;
            }
        }

        /// <summary>
        /// Obtiene el estado a partir de los valores de la instancia de la entidad.
        /// </summary>
        /// <returns>
        /// Diccionario con los valores de las propiedades.
        /// </returns>
        public override IDictionary GetPropertyValues()
        {
            IDictionary values = base.GetPropertyValues();

            values.Add(Properties.MassiveFieldName, this.MassiveFieldName);
            values.Add(Properties.MassiveFieldDescription, this.MassiveFieldDescription);
            if (!this.FieldLong.HasValue)
            {
                values.Add(Properties.FieldLong, null);
            }
            else
            {
                values.Add(Properties.FieldLong, this.FieldLong.Value);
            }
            return values;
        }
        #endregion
    }
}
