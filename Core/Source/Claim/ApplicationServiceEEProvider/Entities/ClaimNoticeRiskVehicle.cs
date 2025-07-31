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
    /// Definición de entidad ClaimNoticeRiskVehicle.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.ClaimServices.EEProvider.ClaimNoticeRiskVehicle.dict"),
    Serializable(),
    DescriptionKey("CLAIM_NOTICE_RISK_VEHICLE_ENTITY"),
    TableMap(TableName = "CLAIM_NOTICE_RISK_VEHICLE", Schema = "CLM"),
    ]
    public partial class ClaimNoticeRiskVehicle :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string ClaimNoticeCode = "ClaimNoticeCode";
            public static readonly string Plate = "Plate";
            public static readonly string VehicleMakeCode = "VehicleMakeCode";
            public static readonly string VehicleModelCode = "VehicleModelCode";
            public static readonly string VehicleVersionCode = "VehicleVersionCode";
            public static readonly string VehicleVersionYearCode = "VehicleVersionYearCode";
            public static readonly string VehicleColor = "VehicleColor";
            public static readonly string Driver = "Driver";
            public static readonly string ClaimDamageTypeCode = "ClaimDamageTypeCode";
            public static readonly string ClaimDamageResponsibilityCode = "ClaimDamageResponsibilityCode";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="claimNoticeCode">Propiedad clave ClaimNoticeCode.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int claimNoticeCode)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.ClaimNoticeCode, claimNoticeCode);

            return new PrimaryKey<T>(keys);
        }

        protected static PrimaryKey InternalCreatePrimaryKey<T>()
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.ClaimNoticeCode, null);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="claimNoticeCode">Propiedad clave ClaimNoticeCode.</param>
        public static PrimaryKey CreatePrimaryKey(int claimNoticeCode)
        {
            return InternalCreatePrimaryKey<ClaimNoticeRiskVehicle>(claimNoticeCode);
        }

        public static PrimaryKey CreatePrimaryKey()
        {
            return InternalCreatePrimaryKey<ClaimNoticeRiskVehicle>();
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad Plate.
        /// </summary>
        private string _plate = null;
        /// <summary>
        /// Atributo para la propiedad VehicleMakeCode.
        /// </summary>
        private int? _vehicleMakeCode = null;
        /// <summary>
        /// Atributo para la propiedad VehicleModelCode.
        /// </summary>
        private int? _vehicleModelCode = null;
        /// <summary>
        /// Atributo para la propiedad VehicleVersionCode.
        /// </summary>
        private int? _vehicleVersionCode = null;
        /// <summary>
        /// Atributo para la propiedad VehicleVersionYearCode.
        /// </summary>
        private int? _vehicleVersionYearCode = null;
        /// <summary>
        /// Atributo para la propiedad VehicleColor.
        /// </summary>
        private int? _vehicleColor = null;
        /// <summary>
        /// Atributo para la propiedad Driver.
        /// </summary>
        private string _driver = null;
        /// <summary>
        /// Atributo para la propiedad ClaimDamageTypeCode.
        /// </summary>
        private int? _claimDamageTypeCode = null;
        /// <summary>
        /// Atributo para la propiedad ClaimDamageResponsibilityCode.
        /// </summary>
        private int? _claimDamageResponsibilityCode = null;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="claimNoticeCode">ClaimNoticeCode key property.</param>
        public ClaimNoticeRiskVehicle(int claimNoticeCode) :
            this(ClaimNoticeRiskVehicle.CreatePrimaryKey(claimNoticeCode), null)
        {
        }

        public ClaimNoticeRiskVehicle() :
            this(ClaimNoticeRiskVehicle.CreatePrimaryKey(), null)
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
        public ClaimNoticeRiskVehicle(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        /*** Object Properties ********************************/
        /// <summary>
        /// Devuelve o setea el valor de la propiedad ClaimNoticeCode.
        /// </summary>
        /// <value>Propiedad ClaimNoticeCode.</value>
        [
        DescriptionKey("CLAIM_NOTICE_CODE_PROPERTY"),
        PersistentProperty(IsKey = true, KeyType = "None"),
        ColumnMap(ColumnName = "CLAIM_NOTICE_CD", DbType = System.Data.DbType.String),
        ]
        public int ClaimNoticeCode
        {
            get
            {
                return (int)this._primaryKey[Properties.ClaimNoticeCode];
            }
            set
            {
                this._primaryKey[Properties.ClaimNoticeCode] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Plate.
        /// </summary>
        /// <value>Propiedad Plate.</value>
        [
        DescriptionKey("PLATE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "PLATE", DbType = System.Data.DbType.String),
        ]
        public string Plate
        {
            get
            {
                return this._plate;
            }
            set
            {
                this._plate = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad VehicleMakeCode.
        /// </summary>
        /// <value>Propiedad VehicleMakeCode.</value>
        [
        DescriptionKey("VEHICLE_MAKE_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "VEHICLE_MAKE_CD", DbType = System.Data.DbType.String),
        ]
        public int? VehicleMakeCode
        {
            get
            {
                return this._vehicleMakeCode;
            }
            set
            {
                this._vehicleMakeCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad VehicleModelCode.
        /// </summary>
        /// <value>Propiedad VehicleModelCode.</value>
        [
        DescriptionKey("VEHICLE_MODEL_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "VEHICLE_MODEL_CD", DbType = System.Data.DbType.String),
        ]
        public int? VehicleModelCode
        {
            get
            {
                return this._vehicleModelCode;
            }
            set
            {
                this._vehicleModelCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad VehicleVersionCode.
        /// </summary>
        /// <value>Propiedad VehicleVersionCode.</value>
        [
        DescriptionKey("VEHICLE_VERSION_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "VEHICLE_VERSION_CD", DbType = System.Data.DbType.String),
        ]
        public int? VehicleVersionCode
        {
            get
            {
                return this._vehicleVersionCode;
            }
            set
            {
                this._vehicleVersionCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad VehicleVersionYearCode.
        /// </summary>
        /// <value>Propiedad VehicleVersionYearCode.</value>
        [
        DescriptionKey("VEHICLE_VERSION_YEAR_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "VEHICLE_VERSION_YEAR_CD", DbType = System.Data.DbType.String),
        ]
        public int? VehicleVersionYearCode
        {
            get
            {
                return this._vehicleVersionYearCode;
            }
            set
            {
                this._vehicleVersionYearCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad VehicleColor.
        /// </summary>
        /// <value>Propiedad VehicleColor.</value>
        [
        DescriptionKey("VEHICLE_COLOR_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "VEHICLE_COLOR", DbType = System.Data.DbType.String),
        ]
        public int? VehicleColor
        {
            get
            {
                return this._vehicleColor;
            }
            set
            {
                this._vehicleColor = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Driver.
        /// </summary>
        /// <value>Propiedad Driver.</value>
        [
        DescriptionKey("DRIVER_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "DRIVER", DbType = System.Data.DbType.String),
        ]
        public string Driver
        {
            get
            {
                return this._driver;
            }
            set
            {
                this._driver = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad ClaimDamageTypeCode.
        /// </summary>
        /// <value>Propiedad ClaimDamageTypeCode.</value>
        [
        DescriptionKey("CLAIM_DAMAGE_TYPE_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "CLAIM_DAMAGE_TYPE_CD", DbType = System.Data.DbType.String),
        ]
        public int? ClaimDamageTypeCode
        {
            get
            {
                return this._claimDamageTypeCode;
            }
            set
            {
                this._claimDamageTypeCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad ClaimDamageResponsibilityCode.
        /// </summary>
        /// <value>Propiedad ClaimDamageResponsibilityCode.</value>
        [
        DescriptionKey("CLAIM_DAMAGE_RESPONSIBILITY_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "CLAIM_DAMAGE_RESPONSIBILITY_CD", DbType = System.Data.DbType.String),
        ]
        public int? ClaimDamageResponsibilityCode
        {
            get
            {
                return this._claimDamageResponsibilityCode;
            }
            set
            {
                this._claimDamageResponsibilityCode = value;
            }
        }

    }
}
