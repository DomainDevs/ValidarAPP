using System;
using System.Collections;
using System.Collections.Generic;
using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.CollectiveServices.EEProvider.Entities
{
    /// <summary>
    /// Definición de entidad AdministratorUser.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.CollectiveServices.EEProvider.AdministratorUser.dict"),
    Serializable(),
    DescriptionKey("ADMINISTRATOR_USERS_TEMP_ENTITY"),
    TableMap(TableName = "ADMINISTRATOR_USERS_TEMP", Schema = "MSV"),
    ]
    public class AdministratorUser :
        BusinessObject
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string AdministratorId = "AdministratorId";
            public static readonly string TempId = "TempId";
            public static readonly string UserIdAuthorize = "UserIdAuthorize";
            public static readonly string DateUpdate = "DateUpdate";
            public static readonly string UserId = "UserId";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="administratorId">Propiedad clave AdministratorId.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int administratorId)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.AdministratorId, administratorId);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="administratorId">Propiedad clave AdministratorId.</param>
        public static PrimaryKey CreatePrimaryKey(int administratorId)
        {
            return InternalCreatePrimaryKey<AdministratorUser>(administratorId);
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad TempId.
        /// </summary>
        private int _tempId;
        /// <summary>
        /// Atributo para la propiedad UserIdAuthorize.
        /// </summary>
        private int _userIdAuthorize;
        /// <summary>
        /// Atributo para la propiedad DateUpdate.
        /// </summary>
        private DateTime _dateUpdate;
        /// <summary>
        /// Atributo para la propiedad UserId.
        /// </summary>
        private int _userId;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="administratorId">AdministratorId key property.</param>
        public AdministratorUser(int administratorId) :
            this(AdministratorUser.CreatePrimaryKey(administratorId), null)
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
        public AdministratorUser(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        /*** Object Properties ********************************/
        /// <summary>
        /// Devuelve o setea el valor de la propiedad AdministratorId.
        /// </summary>
        /// <value>Propiedad AdministratorId.</value>
        [
        DescriptionKey("ADMINISTRATOR_ID_PROPERTY"),
        PersistentProperty(IsKey = true),
        ColumnMap(ColumnName = "ADMINISTRATOR_ID", DbType = System.Data.DbType.String),
        ]
        public int AdministratorId
        {
            get
            {
                return (int)this._primaryKey[Properties.AdministratorId];
            }
            set
            {
                this._primaryKey[Properties.AdministratorId] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad TempId.
        /// </summary>
        /// <value>Propiedad TempId.</value>
        [
        DescriptionKey("TEMP_ID_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "TEMP_ID", DbType = System.Data.DbType.String),
        ]
        public int TempId
        {
            get
            {
                return this._tempId;
            }
            set
            {
                this._tempId = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad UserIdAuthorize.
        /// </summary>
        /// <value>Propiedad UserIdAuthorize.</value>
        [
        DescriptionKey("USER_ID_AUTHORIZE_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "USER_ID_AUTHORIZE", DbType = System.Data.DbType.String),
        ]
        public int UserIdAuthorize
        {
            get
            {
                return this._userIdAuthorize;
            }
            set
            {
                this._userIdAuthorize = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad DateUpdate.
        /// </summary>
        /// <value>Propiedad DateUpdate.</value>
        [
        DescriptionKey("DATE_UPDATE_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "DATE_UPDATE", DbType = System.Data.DbType.String),
        ]
        public DateTime DateUpdate
        {
            get
            {
                return this._dateUpdate;
            }
            set
            {
                this._dateUpdate = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad UserId.
        /// </summary>
        /// <value>Propiedad UserId.</value>
        [
        DescriptionKey("USER_ID_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "USER_ID", DbType = System.Data.DbType.String),
        ]
        public int UserId
        {
            get
            {
                return this._userId;
            }
            set
            {
                this._userId = value;
            }
        }

    }
}
