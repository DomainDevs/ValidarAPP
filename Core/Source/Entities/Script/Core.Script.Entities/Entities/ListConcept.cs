using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Sistran.Core.Application.Script.Entities
{
    /// <summary>
    /// Definición de entidad ListConcept.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.Script.ListConcept.dict"),
    Serializable(),
    DescriptionKey("LIST_CONCEPT_ENTITY"),
    TableMap(TableName = "LIST_CONCEPT", Schema = "SCR"),
    ]
    public partial class ListConcept :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string ConceptId = "ConceptId";
            public static readonly string EntityId = "EntityId";
            public static readonly string ListEntityCode = "ListEntityCode";
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
            return InternalCreatePrimaryKey<ListConcept>(conceptId, entityId);
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad ListEntityCode.
        /// </summary>
        private int _listEntityCode;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="conceptId">ConceptId key property.</param>
        /// <param name="entityId">EntityId key property.</param>
        public ListConcept(int conceptId, int entityId) :
            this(ListConcept.CreatePrimaryKey(conceptId, entityId), null)
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
        public ListConcept(PrimaryKey key, IDictionary initialValues) :
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
        /// Devuelve o setea el valor de la propiedad ListEntityCode.
        /// </summary>
        /// <value>Propiedad ListEntityCode.</value>
        [
        DescriptionKey("LIST_ENTITY_CODE_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "LIST_ENTITY_CD", DbType = System.Data.DbType.String),
        ]
        public int ListEntityCode
        {
            get
            {
                return this._listEntityCode;
            }
            set
            {
                this._listEntityCode = value;
            }
        }

    }
}