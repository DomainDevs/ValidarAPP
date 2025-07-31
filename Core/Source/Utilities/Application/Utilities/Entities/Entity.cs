using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Sistran.Core.Application.Utilities.Entities
{
    /// <summary>
    /// Definición de entidad Entity.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.Utilities.Entity.dict"),
    Serializable(),
    DescriptionKey("ENTITY_ENTITY"),
    TableMap(TableName = "ENTITY", Schema = "PARAM"),
    ]
    public partial class Entity : BusinessObject2
    {
        public enum EntityTypes
        {
            FacadeGeneral = 83,
            FacadeCoverage = 84,
            FacadeRisk = 85,
            FacadeComponent = 86,
            FacadeCommission = 87,
            FacadeRiskEndorsement = 119,
            FacadeClaim = 323,
			FacadeEstimation = 328,
			FacadePaymentRequest = 329,
            FacadeGeneralAutomaticQuota = 724,
            FacadeThirdAutomaticQuota = 725,
            FacadeBusinessAutomaticQuota =726
        }

        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string EntityId = "EntityId";
            public static readonly string Description = "Description";
            public static readonly string EntityName = "EntityName";
            public static readonly string LevelId = "LevelId";
            public static readonly string PackageId = "PackageId";
            public static readonly string ConfigFile = "ConfigFile";
            public static readonly string PropertySearch = "PropertySearch";
            public static readonly string BusinessView = "BusinessView";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="entityId">Propiedad clave EntityId.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int entityId)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.EntityId, entityId);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="entityId">Propiedad clave EntityId.</param>
        public static PrimaryKey CreatePrimaryKey(int entityId)
        {
            return InternalCreatePrimaryKey<Entity>(entityId);
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        private string _description = null;
        /// <summary>
        /// Atributo para la propiedad EntityName.
        /// </summary>
        private string _entityName = null;
        /// <summary>
        /// Atributo para la propiedad LevelId.
        /// </summary>
        private int _levelId;
        /// <summary>
        /// Atributo para la propiedad PackageId.
        /// </summary>
        private int _packageId;
        /// <summary>
        /// Atributo para la propiedad ConfigFile.
        /// </summary>
        private string _configFile = null;
        /// <summary>
        /// 
        /// </summary>
        private string _propertySearch;
        /// <summary>
        /// 
        /// </summary>
        private string _businessView;

        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="entityId">EntityId key property.</param>
        public Entity(int entityId) :
            this(Entity.CreatePrimaryKey(entityId), null)
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
        public Entity(PrimaryKey key, IDictionary initialValues) :
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
        /// Devuelve o setea el valor de la propiedad EntityName.
        /// </summary>
        /// <value>Propiedad EntityName.</value>
        [
        DescriptionKey("ENTITY_NAME_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "ENTITY_NAME", DbType = System.Data.DbType.String),
        ]
        public string EntityName
        {
            get
            {
                return this._entityName;
            }
            set
            {
                if (value == null)
                {
                    throw new PropertyNotNullableException(this.GetType().FullName, Properties.EntityName);
                }
                this._entityName = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad LevelId.
        /// </summary>
        /// <value>Propiedad LevelId.</value>
        [
        DescriptionKey("LEVEL_ID_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "LEVEL_ID", DbType = System.Data.DbType.String),
        ]
        public int LevelId
        {
            get
            {
                return this._levelId;
            }
            set
            {
                this._levelId = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad PackageId.
        /// </summary>
        /// <value>Propiedad PackageId.</value>
        [
        DescriptionKey("PACKAGE_ID_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "PACKAGE_ID", DbType = System.Data.DbType.String),
        ]
        public int PackageId
        {
            get
            {
                return this._packageId;
            }
            set
            {
                this._packageId = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad ConfigFile.
        /// </summary>
        /// <value>Propiedad ConfigFile.</value>
        [
        DescriptionKey("CONFIG_FILE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "CONFIG_FILE", DbType = System.Data.DbType.String),
        ]
        public string ConfigFile
        {
            get
            {
                return this._configFile;
            }
            set
            {
                this._configFile = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad PropertySearh.
        /// </summary>
        /// <value>Propiedad PackageId.</value>
        [
            DescriptionKey("PROPERTY_SEARCH_PROPERTY"),
            PersistentProperty()
        ]
        public string PropertySearch
        {
            get
            {
                return this._propertySearch;
            }
            set
            {
                this._propertySearch = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad PropertyView.
        /// </summary>
        /// <value>Propiedad PackageId.</value>
        [
            DescriptionKey("BUSINESS_VIEW_PROPERTY"),
            PersistentProperty()
        ]
        public string BusinessView
        {
            get
            {
                return this._businessView;
            }
            set
            {
                this._businessView = value;
            }
        }


        /// <summary>
        /// Validación de propiedades obligatorias para la persistencia de 
        /// instancias de de la entidad. Se valida que las propiedades 
        /// obligatorias no estén sin valor asignado (null).
        /// </summary>
        /// <exception cref = "Sistran.Core.Framework.PropertyNotNullableException">
        /// En caso que alguna propiedad obligatoria no tenga valor asignado o tenga el valor null.
        /// </exception>
        public override void Validate()
        {
            StringCollection propertyNames = new StringCollection();
            if (this._entityName == null)
            {
                propertyNames.Add(Properties.EntityName);
            }
            if (this._description == null)
            {
                propertyNames.Add(Properties.Description);
            }
            if (propertyNames.Count > 0)
            {
                throw new PropertyNotNullableException(this.GetType().FullName, propertyNames);
            }
        }


    }
}