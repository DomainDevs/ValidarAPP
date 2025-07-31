using System;
using System.Collections;
using System.Collections.Specialized;
using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.CollectiveServices.EEProvider.Entities
{
    /// <summary>
    /// Definición de entidad MassiveLoad.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.CollectiveServices.EEProvider.RiskExclude.dict"),
    Serializable(),
    DescriptionKey("RISK_EXCLUDE_ENTITY")
    ]
    public class RiskExclude :
        BusinessObject
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public sealed class Properties
        {
            public static readonly string ConsecutiveCd = "ConsecutiveCd";
            public static readonly string MassiveId = "MassiveId";
            public static readonly string TempId = "TempId";
            public static readonly string LicensePlate = "LicensePlate";
            public static readonly string BeginFrom = "BeginFrom";
            public static readonly string RiskId = "RiskId";
            private Properties()
            {
            }
        }
        /// <summary>
        /// Crea las claves primarias para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <returns>Claves primarias.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey(Type concreteClass)
        {
            ListDictionary keys = new ListDictionary();
            keys.Add(Properties.ConsecutiveCd, null);

            return new PrimaryKey(concreteClass, keys);
        }

        /// <summary>
        /// Crea las clave primaria para esta clase.
        /// </summary>
        public static PrimaryKey CreatePrimaryKey()
        {
            return InternalCreatePrimaryKey(typeof(RiskExclude));
        }

        /// <summary>
        /// Crea las claves primarias para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="tempId">Propiedad clave tempId.</param>
        /// <param name="licensePlate">Propiedad clave licensePlate.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey(Type concreteClass, int tempId, string licensePlate, int consecutiveCd)
        {
            ListDictionary keys = new ListDictionary();
            keys.Add(Properties.ConsecutiveCd, consecutiveCd);

            return new PrimaryKey(concreteClass, keys);
        }

        /// <summary>
        /// Crea las claves primarias para esta clase.
        /// </summary>
        /// <param name="massiveLoadId">Propiedad clave MassiveLoadId.</param>
        public static PrimaryKey CreatePrimaryKey(int tempId, string licensePlate, int consecutiveCd)
        {
            return InternalCreatePrimaryKey(typeof(RiskExclude), tempId, licensePlate, consecutiveCd);
        }
        #endregion

        #region Attributes
        //*** Object Attributes ********************************
        /// <summary>
        /// Atributo para la propiedad TempId.
        /// </summary>
        private int _consecutiveCd;
        /// <summary>
        /// Atributo para la propiedad MassiveID.
        /// </summary>
        private int? _massiveId = null;
        /// <summary>
        /// Atributo para la propiedad TempId.
        /// </summary>
        private int _tempId;
        /// <summary>
        /// Atributo para la propiedad LicensePlate.
        /// </summary>
        private string _licensePlate;
        /// <summary>
        /// Atributo para la propiedad BeginFrom.
        /// </summary>
        private DateTime? _beginFrom = null;
        /// <summary>
        /// Atributo para la propiedad RiskId.
        /// </summary>
        private int? _riskId;
        #endregion

        #region Constructors
        //*** Object Constructors ********************************
        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves que no son autonumeradas.
        /// </summary>
        /// <returns>Primary key.</returns>
        public RiskExclude() :
            this(RiskExclude.CreatePrimaryKey(), null)
        {
        }

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="massiveLoadId">MassiveLoadId key property.</param>
        public RiskExclude(int tempId, string licensePlate, int consecutiveCd) :
            this(RiskExclude.CreatePrimaryKey(tempId, licensePlate, consecutiveCd), null)
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
        public RiskExclude(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        #endregion

        #region Object Properties

        /// <summary>
        /// Devuelve o setea el valor de la propiedad ConsecutiveCd.
        /// </summary>
        /// <value>Propiedad ConsecutiveCd.</value>
        [
        PersistentProperty(IsKey = true, IsAutomatic = true),
        DescriptionKey("CONSECUTIVE_CD_PROPERTY"),
       ]
        public int ConsecutiveCd
        {
            get
            {
                if (_primaryKey[Properties.ConsecutiveCd] == null)
                {
                    _primaryKey[Properties.ConsecutiveCd] = 0;
                }

                return (int)_primaryKey[Properties.ConsecutiveCd];
            }
            set
            {
                _primaryKey[Properties.ConsecutiveCd] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad MassiveFieldName.
        /// </summary>
        /// <value>Propiedad MassiveFieldName.</value>
        [
            DescriptionKey("MASSIVE_ID_PROPERTY"),
            PersistentProperty(IsNullable = true)
        ]
        public int? MassiveId
        {
            get
            {
                return this._massiveId;
            }
            set
            {
                this._massiveId = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad TempId.
        /// </summary>
        /// <value>Propiedad TempId.</value>
        [
            DescriptionKey("TEMP_ID_PROPERTY")
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
        /// Devuelve o setea el valor de la propiedad LicensePlate.
        /// </summary>
        /// <value>Propiedad LicensePlate.</value>
        [
            DescriptionKey("LICENSE_PLATE_PROPERTY")
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
        /// Devuelve o setea el valor de la propiedad BeginFrom.
        /// </summary>
        /// <value>Propiedad BeginFrom.</value>
        [
            DescriptionKey("BEGIN_FROM_PROPERTY"),
            PersistentProperty(IsNullable = true)
        ]
        public DateTime? BeginFrom
        {
            get
            {
                return this._beginFrom;
            }
            set
            {
                this._beginFrom = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad RiskId.
        /// </summary>
        /// <value>Propiedad RiskId.</value>
        [
            DescriptionKey("RISK_ID_PROPERTY"),
            PersistentProperty(IsNullable = true)
        ]
        public int? RiskId  
        {
            get
            {
                return this._riskId;
            }
            set
            {
                this._riskId = value;
            }
        }

        #endregion
    }
}

