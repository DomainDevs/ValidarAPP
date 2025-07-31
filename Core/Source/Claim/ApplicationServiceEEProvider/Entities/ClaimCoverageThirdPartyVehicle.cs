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
    /// Definición de entidad ClaimCoverageThirdPartyVehicle.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.ClaimServices.EEProvider.ClaimCoverageThirdPartyVehicle.dict"),
    Serializable(),
    DescriptionKey("CLAIM_COVERAGE_THIRD_PARTY_VEHICLE_ENTITY"),
    TableMap(TableName = "CLAIM_COVERAGE_THIRD_PARTY_VEHICLE", Schema = "CLM"),
    ]
    public partial class ClaimCoverageThirdPartyVehicle :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string ClaimCoverageCode = "ClaimCoverageCode";
            public static readonly string Plate = "Plate";
            public static readonly string VehicleMake = "VehicleMake";
            public static readonly string VehicleModel = "VehicleModel";
            public static readonly string VehicleYear = "VehicleYear";
            public static readonly string VehicleColorCode = "VehicleColorCode";
            public static readonly string ChasisNumber = "ChasisNumber";
            public static readonly string EngineNumber = "EngineNumber";
            public static readonly string VinCode = "VinCode";
            public static readonly string Description = "Description";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="claimCoverageCode">Propiedad clave ClaimCoverageCode.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int claimCoverageCode)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.ClaimCoverageCode, claimCoverageCode);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="claimCoverageCode">Propiedad clave ClaimCoverageCode.</param>
        public static PrimaryKey CreatePrimaryKey(int claimCoverageCode)
        {
            return InternalCreatePrimaryKey<ClaimCoverageThirdPartyVehicle>(claimCoverageCode);
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad Plate.
        /// </summary>
        private string _plate = null;
        /// <summary>
        /// Atributo para la propiedad VehicleMake.
        /// </summary>
        private string _vehicleMake = null;
        /// <summary>
        /// Atributo para la propiedad VehicleModel.
        /// </summary>
        private string _vehicleModel = null;
        /// <summary>
        /// Atributo para la propiedad VehicleYear.
        /// </summary>
        private int? _vehicleYear = null;
        /// <summary>
        /// Atributo para la propiedad VehicleColorCode.
        /// </summary>
        private int? _vehicleColorCode = null;
        /// <summary>
        /// Atributo para la propiedad ChasisNumber.
        /// </summary>
        private string _chasisNumber = null;
        /// <summary>
        /// Atributo para la propiedad EngineNumber.
        /// </summary>
        private string _engineNumber = null;
        /// <summary>
        /// Atributo para la propiedad VinCode.
        /// </summary>
        private string _vinCode = null;
        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        private string _description = null;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="claimCoverageCode">ClaimCoverageCode key property.</param>
        public ClaimCoverageThirdPartyVehicle(int claimCoverageCode) :
            this(ClaimCoverageThirdPartyVehicle.CreatePrimaryKey(claimCoverageCode), null)
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
        public ClaimCoverageThirdPartyVehicle(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        /*** Object Properties ********************************/
        /// <summary>
        /// Devuelve o setea el valor de la propiedad ClaimCoverageCode.
        /// </summary>
        /// <value>Propiedad ClaimCoverageCode.</value>
        [
        DescriptionKey("CLAIM_COVERAGE_CODE_PROPERTY"),
        PersistentProperty(IsKey = true),
        ColumnMap(ColumnName = "CLAIM_COVERAGE_CD", DbType = System.Data.DbType.String),
        ]
        public int ClaimCoverageCode
        {
            get
            {
                return (int)this._primaryKey[Properties.ClaimCoverageCode];
            }
            set
            {
                this._primaryKey[Properties.ClaimCoverageCode] = value;
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
        /// Devuelve o setea el valor de la propiedad VehicleMake.
        /// </summary>
        /// <value>Propiedad VehicleMake.</value>
        [
        DescriptionKey("VEHICLE_MAKE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "VEHICLE_MAKE", DbType = System.Data.DbType.String),
        ]
        public string VehicleMake
        {
            get
            {
                return this._vehicleMake;
            }
            set
            {
                this._vehicleMake = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad VehicleModel.
        /// </summary>
        /// <value>Propiedad VehicleModel.</value>
        [
        DescriptionKey("VEHICLE_MODEL_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "VEHICLE_MODEL", DbType = System.Data.DbType.String),
        ]
        public string VehicleModel
        {
            get
            {
                return this._vehicleModel;
            }
            set
            {
                this._vehicleModel = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad VehicleYear.
        /// </summary>
        /// <value>Propiedad VehicleYear.</value>
        [
        DescriptionKey("VEHICLE_YEAR_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "VEHICLE_YEAR", DbType = System.Data.DbType.String),
        ]
        public int? VehicleYear
        {
            get
            {
                return this._vehicleYear;
            }
            set
            {
                this._vehicleYear = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad VehicleColorCode.
        /// </summary>
        /// <value>Propiedad VehicleColorCode.</value>
        [
        DescriptionKey("VEHICLE_COLOR_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "VEHICLE_COLOR_CD", DbType = System.Data.DbType.String),
        ]
        public int? VehicleColorCode
        {
            get
            {
                return this._vehicleColorCode;
            }
            set
            {
                this._vehicleColorCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad ChasisNumber.
        /// </summary>
        /// <value>Propiedad ChasisNumber.</value>
        [
        DescriptionKey("CHASIS_NUMBER_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "CHASIS_NUMBER", DbType = System.Data.DbType.String),
        ]
        public string ChasisNumber
        {
            get
            {
                return this._chasisNumber;
            }
            set
            {
                this._chasisNumber = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad EngineNumber.
        /// </summary>
        /// <value>Propiedad EngineNumber.</value>
        [
        DescriptionKey("ENGINE_NUMBER_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "ENGINE_NUMBER", DbType = System.Data.DbType.String),
        ]
        public string EngineNumber
        {
            get
            {
                return this._engineNumber;
            }
            set
            {
                this._engineNumber = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad VinCode.
        /// </summary>
        /// <value>Propiedad VinCode.</value>
        [
        DescriptionKey("VIN_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "VIN_CODE", DbType = System.Data.DbType.String),
        ]
        public string VinCode
        {
            get
            {
                return this._vinCode;
            }
            set
            {
                this._vinCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Description.
        /// </summary>
        /// <value>Propiedad Description.</value>
        [
        DescriptionKey("DESCRIPTION_PROPERTY"),
        PersistentProperty(IsNullable = true),
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
                this._description = value;
            }
        }

    }
}
