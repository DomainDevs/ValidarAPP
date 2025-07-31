using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Sistran.Core.Application.Utilities.Entities
{
    /// <summary>
    /// Definición de entidad PositionEntity.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.Utilities.PositionEntity.dict"),
    Serializable(),
    DescriptionKey("POSITION_ENTITY_ENTITY"),
    TableMap(TableName = "POSITION_ENTITY", Schema = "SCR"),
    ]
    public partial class PositionEntity :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string EntityId = "EntityId";
            public static readonly string LevelId = "LevelId";
            public static readonly string PackageId = "PackageId";
            public static readonly string Position = "Position";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="entityId">Propiedad clave EntityId.</param>
        /// <param name="levelId">Propiedad clave LevelId.</param>
        /// <param name="packageId">Propiedad clave PackageId.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int entityId, int levelId, int packageId)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.EntityId, entityId);
            keys.Add(Properties.LevelId, levelId);
            keys.Add(Properties.PackageId, packageId);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="entityId">Propiedad clave EntityId.</param>
        /// <param name="levelId">Propiedad clave LevelId.</param>
        /// <param name="packageId">Propiedad clave PackageId.</param>
        public static PrimaryKey CreatePrimaryKey(int entityId, int levelId, int packageId)
        {
            return InternalCreatePrimaryKey<PositionEntity>(entityId, levelId, packageId);
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad Position.
        /// </summary>
        private int _position;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="entityId">EntityId key property.</param>
        /// <param name="levelId">LevelId key property.</param>
        /// <param name="packageId">PackageId key property.</param>
        public PositionEntity(int entityId, int levelId, int packageId) :
            this(PositionEntity.CreatePrimaryKey(entityId, levelId, packageId), null)
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
        public PositionEntity(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        /*** Object Properties ********************************/
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
        /// Devuelve o setea el valor de la propiedad LevelId.
        /// </summary>
        /// <value>Propiedad LevelId.</value>
        [
        DescriptionKey("LEVEL_ID_PROPERTY"),
        PersistentProperty(IsKey = true),
        ColumnMap(ColumnName = "LEVEL_ID", DbType = System.Data.DbType.String),
        ]
        public int LevelId
        {
            get
            {
                return (int)this._primaryKey[Properties.LevelId];
            }
            set
            {
                this._primaryKey[Properties.LevelId] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad PackageId.
        /// </summary>
        /// <value>Propiedad PackageId.</value>
        [
        DescriptionKey("PACKAGE_ID_PROPERTY"),
        PersistentProperty(IsKey = true),
        ColumnMap(ColumnName = "PACKAGE_ID", DbType = System.Data.DbType.String),
        ]
        public int PackageId
        {
            get
            {
                return (int)this._primaryKey[Properties.PackageId];
            }
            set
            {
                this._primaryKey[Properties.PackageId] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Position.
        /// </summary>
        /// <value>Propiedad Position.</value>
        [
        DescriptionKey("POSITION_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "POSITION", DbType = System.Data.DbType.String),
        ]
        public int Position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
            }
        }

    }
}