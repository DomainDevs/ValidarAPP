using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Sistran.Core.Application.Utilities.Entities
{
    /// <summary>
    /// Definición de entidad Concept.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.Utilities.Concept.dict"),
    Serializable(),
    DescriptionKey("CONCEPT_ENTITY"),
    TableMap(TableName = "CONCEPT", Schema = "SCR"),
    ]
    public partial class Concept : BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string ConceptId = "ConceptId";
            public static readonly string EntityId = "EntityId";
            public static readonly string Description = "Description";
            public static readonly string ConceptName = "ConceptName";
            public static readonly string ConceptTypeCode = "ConceptTypeCode";
            public static readonly string KeyOrder = "KeyOrder";
            public static readonly string IsStatic = "IsStatic";
            public static readonly string ConceptControlCode = "ConceptControlCode";
            public static readonly string IsReadOnly = "IsReadOnly";
            public static readonly string IsVisible = "IsVisible";
            public static readonly string IsNullable = "IsNullable";
            public static readonly string IsPersistible = "IsPersistible";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="conceptId">Propiedad clave ConceptId.</param>
        /// <param name="entityId">Propiedad clave EntityId.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int conceptId, int entityId)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.ConceptId, conceptId);
            keys.Add(Properties.EntityId, entityId);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="conceptId">Propiedad clave ConceptId.</param>
        /// <param name="entityId">Propiedad clave EntityId.</param>
        public static PrimaryKey CreatePrimaryKey(int conceptId, int entityId)
        {
            return InternalCreatePrimaryKey<Concept>(conceptId, entityId);
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        private string _description = null;
        /// <summary>
        /// Atributo para la propiedad ConceptName.
        /// </summary>
        private string _conceptName = null;
        /// <summary>
        /// Atributo para la propiedad ConceptTypeCode.
        /// </summary>
        private int _conceptTypeCode;
        /// <summary>
        /// Atributo para la propiedad KeyOrder.
        /// </summary>
        private int _keyOrder;
        /// <summary>
        /// Atributo para la propiedad IsStatic.
        /// </summary>
        private bool _isStatic;
        /// <summary>
        /// Atributo para la propiedad ConceptControlCode.
        /// </summary>
        private int _conceptControlCode;
        /// <summary>
        /// Atributo para la propiedad IsReadOnly.
        /// </summary>
        private bool _isReadOnly;
        /// <summary>
        /// Atributo para la propiedad IsVisible.
        /// </summary>
        private bool _isVisible;
        /// <summary>
        /// Atributo para la propiedad IsNullable.
        /// </summary>
        private bool _isNullable;
        /// <summary>
        /// Atributo para la propiedad IsPersistible.
        /// </summary>
        private bool _isPersistible;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="conceptId">ConceptId key property.</param>
        /// <param name="entityId">EntityId key property.</param>
        public Concept(int conceptId, int entityId) :
            this(Concept.CreatePrimaryKey(conceptId, entityId), null)
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
        public Concept(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        /*** Object Properties ********************************/
        /// <summary>
        /// Devuelve o setea el valor de la propiedad ConceptId.
        /// </summary>
        /// <value>Propiedad ConceptId.</value>
        [
        DescriptionKey("CONCEPT_ID_PROPERTY"),
        PersistentProperty(IsKey = true),
        ColumnMap(ColumnName = "CONCEPT_ID", DbType = System.Data.DbType.String),
        ]
        public int ConceptId
        {
            get
            {
                return (int)this._primaryKey[Properties.ConceptId];
            }
            set
            {
                this._primaryKey[Properties.ConceptId] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad EntityId.
        /// </summary>
        /// <value>Propiedad EntityId.</value>
        [
        DescriptionKey("ENTITY_ID_PROPERTY"),
        PersistentProperty(IsKey = true),
        ColumnMap(ColumnName = "ENTITY_ID", DbType = System.Data.DbType.String),
        ]
        public int EntityId
        {
            get
            {
                return (int)this._primaryKey[Properties.EntityId];
            }
            set
            {
                this._primaryKey[Properties.EntityId] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Description.
        /// </summary>
        /// <value>Propiedad Description.</value>
        [
        DescriptionKey("DESCRIPTION_PROPERTY"),
        PersistentProperty(),
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
                if (value == null)
                {
                    throw new PropertyNotNullableException(this.GetType().FullName, Properties.Description);
                }
                this._description = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad ConceptName.
        /// </summary>
        /// <value>Propiedad ConceptName.</value>
        [
        DescriptionKey("CONCEPT_NAME_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "CONCEPT_NAME", DbType = System.Data.DbType.String),
        ]
        public string ConceptName
        {
            get
            {
                return this._conceptName;
            }
            set
            {
                if (value == null)
                {
                    throw new PropertyNotNullableException(this.GetType().FullName, Properties.ConceptName);
                }
                this._conceptName = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad ConceptTypeCode.
        /// </summary>
        /// <value>Propiedad ConceptTypeCode.</value>
        [
        DescriptionKey("CONCEPT_TYPE_CODE_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "CONCEPT_TYPE_CD", DbType = System.Data.DbType.String),
        ]
        public int ConceptTypeCode
        {
            get
            {
                return this._conceptTypeCode;
            }
            set
            {
                this._conceptTypeCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad KeyOrder.
        /// </summary>
        /// <value>Propiedad KeyOrder.</value>
        [
        DescriptionKey("KEY_ORDER_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "KEY_ORDER", DbType = System.Data.DbType.String),
        ]
        public int KeyOrder
        {
            get
            {
                return this._keyOrder;
            }
            set
            {
                this._keyOrder = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad IsStatic.
        /// </summary>
        /// <value>Propiedad IsStatic.</value>
        [
        DescriptionKey("IS_STATIC_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "IS_STATIC", DbType = System.Data.DbType.String),
        ]
        public bool IsStatic
        {
            get
            {
                return this._isStatic;
            }
            set
            {
                this._isStatic = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad ConceptControlCode.
        /// </summary>
        /// <value>Propiedad ConceptControlCode.</value>
        [
        DescriptionKey("CONCEPT_CONTROL_CODE_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "CONCEPT_CONTROL_CD", DbType = System.Data.DbType.String),
        ]
        public int ConceptControlCode
        {
            get
            {
                return this._conceptControlCode;
            }
            set
            {
                this._conceptControlCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad IsReadOnly.
        /// </summary>
        /// <value>Propiedad IsReadOnly.</value>
        [
        DescriptionKey("IS_READ_ONLY_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "IS_READ_ONLY", DbType = System.Data.DbType.String),
        ]
        public bool IsReadOnly
        {
            get
            {
                return this._isReadOnly;
            }
            set
            {
                this._isReadOnly = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad IsVisible.
        /// </summary>
        /// <value>Propiedad IsVisible.</value>
        [
        DescriptionKey("IS_VISIBLE_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "IS_VISIBLE", DbType = System.Data.DbType.String),
        ]
        public bool IsVisible
        {
            get
            {
                return this._isVisible;
            }
            set
            {
                this._isVisible = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad IsNullable.
        /// </summary>
        /// <value>Propiedad IsNullable.</value>
        [
        DescriptionKey("IS_NULLABLE_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "IS_NULLABLE", DbType = System.Data.DbType.String),
        ]
        public bool IsNullable
        {
            get
            {
                return this._isNullable;
            }
            set
            {
                this._isNullable = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad IsPersistible.
        /// </summary>
        /// <value>Propiedad IsPersistible.</value>
        [
        DescriptionKey("IS_PERSISTIBLE_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "IS_PERSISTIBLE", DbType = System.Data.DbType.String),
        ]
        public bool IsPersistible
        {
            get
            {
                return this._isPersistible;
            }
            set
            {
                this._isPersistible = value;
            }
        }

    }
}