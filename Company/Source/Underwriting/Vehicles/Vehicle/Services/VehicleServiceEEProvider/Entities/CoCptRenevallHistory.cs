using System;
using System.Collections;
using System.Collections.Generic;
using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;

namespace Sistran.Company.Application.Vehicles.VehicleServices.Entities
{
    /// <summary>
    /// Definición de entidad CoCptRenevallHistory.
    /// </summary>
    [
    PersistentClass("Sistran.Company.Application.Vehicles.VehicleServices.CoCptRenevallHistory.dict"),
    Serializable(),
    DescriptionKey("CO_CPT_RENEVALL_HISTORY_ENTITY"),
    TableMap(TableName = "CO_CPT_RENEVALL_HISTORY", Schema = "ISS"),
    ]
    public partial class CoCptRenevallHistory :
        BusinessObject2
    {
        #region static

        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string Id = "Id";
            public static readonly string KeyId = "KeyId";
            public static readonly string IdCardTypeCode = "IdCardTypeCode";
            public static readonly string IdCardNo = "IdCardNo";
            public static readonly string LicensePlate = "LicensePlate";
            public static readonly string CurrentFrom = "CurrentFrom";
            public static readonly string CurrentTo = "CurrentTo";
            public static readonly string EndoCurrentFrom = "EndoCurrentFrom";
            public static readonly string EndoCurrentTo = "EndoCurrentTo";
            public static readonly string NewRenewall = "NewRenewall";
            public static readonly string RenewallNum = "RenewallNum";
            public static readonly string Chanel = "Chanel";
            public static readonly string Distance = "Distance";
            public static readonly string DateDataIni = "DateDataIni";
            public static readonly string DateDataEnd = "DateDataEnd";
            public static readonly string LoadDate = "LoadDate";
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

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="id">Propiedad clave Id.</param>
        public static PrimaryKey CreatePrimaryKey(int id)
        {
            return InternalCreatePrimaryKey<CoCptRenevallHistory>(id);
        }

        #endregion static

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad KeyId.
        /// </summary>
        private string _keyId = null;

        /// <summary>
        /// Atributo para la propiedad IdCardTypeCode.
        /// </summary>
        private decimal? _idCardTypeCode = null;

        /// <summary>
        /// Atributo para la propiedad IdCardNo.
        /// </summary>
        private string _idCardNo = null;

        /// <summary>
        /// Atributo para la propiedad LicensePlate.
        /// </summary>
        private string _licensePlate = null;

        /// <summary>
        /// Atributo para la propiedad CurrentFrom.
        /// </summary>
        private DateTime? _currentFrom = null;

        /// <summary>
        /// Atributo para la propiedad CurrentTo.
        /// </summary>
        private DateTime? _currentTo = null;

        /// <summary>
        /// Atributo para la propiedad EndoCurrentFrom.
        /// </summary>
        private DateTime? _endoCurrentFrom = null;

        /// <summary>
        /// Atributo para la propiedad EndoCurrentTo.
        /// </summary>
        private DateTime? _endoCurrentTo = null;

        /// <summary>
        /// Atributo para la propiedad NewRenewall.
        /// </summary>
        private string _newRenewall = null;

        /// <summary>
        /// Atributo para la propiedad RenewallNum.
        /// </summary>
        private decimal? _renewallNum = null;

        /// <summary>
        /// Atributo para la propiedad Chanel.
        /// </summary>
        private string _chanel = null;

        /// <summary>
        /// Atributo para la propiedad Distance.
        /// </summary>
        private decimal? _distance = null;

        /// <summary>
        /// Atributo para la propiedad DateDataIni.
        /// </summary>
        private DateTime? _dateDataIni = null;

        /// <summary>
        /// Atributo para la propiedad DateDataEnd.
        /// </summary>
        private DateTime? _dateDataEnd = null;

        /// <summary>
        /// Atributo para la propiedad LoadDate.
        /// </summary>
        private DateTime? _loadDate = null;

        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="id">Id key property.</param>
        public CoCptRenevallHistory(int id) :
            this(CoCptRenevallHistory.CreatePrimaryKey(id), null)
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
        public CoCptRenevallHistory(PrimaryKey key, IDictionary initialValues) :
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
        /// Devuelve o setea el valor de la propiedad KeyId.
        /// </summary>
        /// <value>Propiedad KeyId.</value>
        [
        DescriptionKey("KEY_ID_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "KEY_ID", DbType = System.Data.DbType.String),
        ]
        public string KeyId
        {
            get
            {
                return this._keyId;
            }
            set
            {
                this._keyId = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad IdCardTypeCode.
        /// </summary>
        /// <value>Propiedad IdCardTypeCode.</value>
        [
        DescriptionKey("ID_CARD_TYPE_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "ID_CARD_TYPE_CD", DbType = System.Data.DbType.Decimal),
        ]
        public decimal? IdCardTypeCode
        {
            get
            {
                return this._idCardTypeCode;
            }
            set
            {
                this._idCardTypeCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad IdCardNo.
        /// </summary>
        /// <value>Propiedad IdCardNo.</value>
        [
        DescriptionKey("ID_CARD_NO_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "ID_CARD_NO", DbType = System.Data.DbType.String),
        ]
        public string IdCardNo
        {
            get
            {
                return this._idCardNo;
            }
            set
            {
                this._idCardNo = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad LicensePlate.
        /// </summary>
        /// <value>Propiedad LicensePlate.</value>
        [
        DescriptionKey("LICENSE_PLATE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "LICENSE_PLATE", DbType = System.Data.DbType.String),
        ]
        public string LicensePlate
        {
            get
            {
                return this._licensePlate;
            }
            set
            {
                this._licensePlate = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad CurrentFrom.
        /// </summary>
        /// <value>Propiedad CurrentFrom.</value>
        [
        DescriptionKey("CURRENT_FROM_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "CURRENT_FROM", DbType = System.Data.DbType.String),
        ]
        public DateTime? CurrentFrom
        {
            get
            {
                return this._currentFrom;
            }
            set
            {
                this._currentFrom = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad CurrentTo.
        /// </summary>
        /// <value>Propiedad CurrentTo.</value>
        [
        DescriptionKey("CURRENT_TO_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "CURRENT_TO", DbType = System.Data.DbType.String),
        ]
        public DateTime? CurrentTo
        {
            get
            {
                return this._currentTo;
            }
            set
            {
                this._currentTo = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad EndoCurrentFrom.
        /// </summary>
        /// <value>Propiedad EndoCurrentFrom.</value>
        [
        DescriptionKey("ENDO_CURRENT_FROM_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "ENDO_CURRENT_FROM", DbType = System.Data.DbType.String),
        ]
        public DateTime? EndoCurrentFrom
        {
            get
            {
                return this._endoCurrentFrom;
            }
            set
            {
                this._endoCurrentFrom = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad EndoCurrentTo.
        /// </summary>
        /// <value>Propiedad EndoCurrentTo.</value>
        [
        DescriptionKey("ENDO_CURRENT_TO_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "ENDO_CURRENT_TO", DbType = System.Data.DbType.String),
        ]
        public DateTime? EndoCurrentTo
        {
            get
            {
                return this._endoCurrentTo;
            }
            set
            {
                this._endoCurrentTo = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad NewRenewall.
        /// </summary>
        /// <value>Propiedad NewRenewall.</value>
        [
        DescriptionKey("NEW_RENEWALL_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "NEW_RENEWALL", DbType = System.Data.DbType.String),
        ]
        public string NewRenewall
        {
            get
            {
                return this._newRenewall;
            }
            set
            {
                this._newRenewall = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad RenewallNum.
        /// </summary>
        /// <value>Propiedad RenewallNum.</value>
        [
        DescriptionKey("RENEWALL_NUM_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "RENEWALL_NUM", DbType = System.Data.DbType.Decimal),
        ]
        public decimal? RenewallNum
        {
            get
            {
                return this._renewallNum;
            }
            set
            {
                this._renewallNum = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Chanel.
        /// </summary>
        /// <value>Propiedad Chanel.</value>
        [
        DescriptionKey("CHANEL_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "CHANEL", DbType = System.Data.DbType.String),
        ]
        public string Chanel
        {
            get
            {
                return this._chanel;
            }
            set
            {
                this._chanel = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Distance.
        /// </summary>
        /// <value>Propiedad Distance.</value>
        [
        DescriptionKey("DISTANCE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "DISTANCE", DbType = System.Data.DbType.Decimal),
        ]
        public decimal? Distance
        {
            get
            {
                return this._distance;
            }
            set
            {
                this._distance = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad DateDataIni.
        /// </summary>
        /// <value>Propiedad DateDataIni.</value>
        [
        DescriptionKey("DATE_DATA_INI_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "DATE_DATA_INI", DbType = System.Data.DbType.String),
        ]
        public DateTime? DateDataIni
        {
            get
            {
                return this._dateDataIni;
            }
            set
            {
                this._dateDataIni = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad DateDataEnd.
        /// </summary>
        /// <value>Propiedad DateDataEnd.</value>
        [
        DescriptionKey("DATE_DATA_END_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "DATE_DATA_END", DbType = System.Data.DbType.String),
        ]
        public DateTime? DateDataEnd
        {
            get
            {
                return this._dateDataEnd;
            }
            set
            {
                this._dateDataEnd = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad LoadDate.
        /// </summary>
        /// <value>Propiedad LoadDate.</value>
        [
        DescriptionKey("LOAD_DATE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "LOAD_DATE", DbType = System.Data.DbType.String),
        ]
        public DateTime? LoadDate
        {
            get
            {
                return this._loadDate;
            }
            set
            {
                this._loadDate = value;
            }
        }
    }

}
