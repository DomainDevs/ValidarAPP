using System;
using System.Collections;
using System.Collections.Generic;
using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;

namespace Sistran.Company.Application.Common.Entities
{
    /// <summary>
    /// Definición de entidad MinPremiumRelation.
    /// </summary>
    [
    PersistentClass("Sistran.Company.Application.Common.MinPremiumRelation.dict"),
    Serializable(),
    DescriptionKey("MIN_PREMIUM_RELATION_ENTITY"),
    TableMap(TableName = "MIN_PREMIUM_RELATION", Schema = "COMM"),
    ]
    public partial class MinPremiumRelation :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string EndoTypeCode = "EndoTypeCode";
            public static readonly string Key2 = "Key2";
            public static readonly string PrefixCode = "PrefixCode";
            public static readonly string SubsMinPremium = "SubsMinPremium";
            public static readonly string MinPremiumRelId = "MinPremiumRelId";
            public static readonly string BranchCode = "BranchCode";
            public static readonly string RiskMinPremium = "RiskMinPremium";
            public static readonly string Key1 = "Key1";
            public static readonly string CurrencyCode = "CurrencyCode";
            public static readonly string CalculateProrate = "CalculateProrate";
            public static readonly string ValidityType = "ValidityType";

        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="minPremiumRelId">Propiedad clave MinPremiumRelId.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int minPremiumRelId)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.MinPremiumRelId, minPremiumRelId);

            return new PrimaryKey<T>(keys);
        }

        protected static PrimaryKey InternalCreatePrimaryKey<T>()
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.MinPremiumRelId, null);

            return new PrimaryKey<T>(keys);
        }


        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="minPremiumRelId">Propiedad clave MinPremiumRelId.</param>
        public static PrimaryKey CreatePrimaryKey(int minPremiumRelId)
        {
            return InternalCreatePrimaryKey<MinPremiumRelation>(minPremiumRelId);
        }

        public static PrimaryKey CreatePrimaryKey()
        {
            return InternalCreatePrimaryKey<MinPremiumRelation>();
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad EndoTypeCode.
        /// </summary>
        private int _endoTypeCode;
        /// <summary>
        /// Atributo para la propiedad Key2.
        /// </summary>
        private decimal? _key2 = null;
        /// <summary>
        /// Atributo para la propiedad PrefixCode.
        /// </summary>
        private int _prefixCode;
        /// <summary>
        /// Atributo para la propiedad SubsMinPremium.
        /// </summary>
        private decimal? _subsMinPremium = null;
        /// <summary>
        /// Atributo para la propiedad BranchCode.
        /// </summary>
        private int? _branchCode = null;
        /// <summary>
        /// Atributo para la propiedad RiskMinPremium.
        /// </summary>
        private decimal? _riskMinPremium = null;
        /// <summary>
        /// Atributo para la propiedad Key1.
        /// </summary>
        private decimal? _key1 = null;
        /// <summary>
        /// Atributo para la propiedad CurrencyCode.
        /// </summary>
        private int _currencyCode;

        /*<< Autor:Felipe Barbosa fecha: 19/11/2012  Asunto:  OT:97 - Propiedades para calculo a prorrata y tipo de vigencia compañia: 1*/
        /// <summary>
        /// Atributo para la propiedad CalculateProrate.
        /// </summary>
        private bool? _calculateProrate = null;

        /// <summary>
        /// Atributo para la propiedad ValidityType.
        /// </summary>
        private int? _validityType = null;
        /*TODO: Autor:Felipe Barbosa  fecha : 19/11/2012  >>  */
        //*** Object Constructors ********************************

        public MinPremiumRelation() :
       this(MinPremiumRelation.CreatePrimaryKey(), null)
        {
        }
        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="minPremiumRelId">MinPremiumRelId key property.</param>
        public MinPremiumRelation(int minPremiumRelId) :
            this(MinPremiumRelation.CreatePrimaryKey(minPremiumRelId), null)
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
        public MinPremiumRelation(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        /*** Object Properties ********************************/
        /// <summary>
        /// Devuelve o setea el valor de la propiedad EndoTypeCode.
        /// </summary>
        /// <value>Propiedad EndoTypeCode.</value>
        [
            DescriptionKey("ENDO_TYPE_CODE_PROPERTY"),
            PersistentProperty()
        ]
        public int EndoTypeCode
        {
            get
            {
                return this._endoTypeCode;
            }
            set
            {
                this._endoTypeCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Key2.
        /// </summary>
        /// <value>Propiedad Key2.</value>
        [
            DescriptionKey("KEY2_PROPERTY"),
            PersistentProperty(IsNullable = true)
        ]
        public decimal? Key2
        {
            get
            {
                return this._key2;
            }
            set
            {
                this._key2 = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad PrefixCode.
        /// </summary>
        /// <value>Propiedad PrefixCode.</value>
        [
            DescriptionKey("PREFIX_CODE_PROPERTY"),
            PersistentProperty()
        ]
        public int PrefixCode
        {
            get
            {
                return this._prefixCode;
            }
            set
            {
                this._prefixCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad SubsMinPremium.
        /// </summary>
        /// <value>Propiedad SubsMinPremium.</value>
        [
            DescriptionKey("SUBS_MIN_PREMIUM_PROPERTY"),
            PersistentProperty(IsNullable = true)
        ]
        public decimal? SubsMinPremium
        {
            get
            {
                return this._subsMinPremium;
            }
            set
            {
                this._subsMinPremium = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad MinPremiumRelId.
        /// </summary>
        /// <value>Propiedad MinPremiumRelId.</value>
        [
            DescriptionKey("MIN_PREMIUM_REL_ID_PROPERTY"),
            PersistentProperty(IsKey = true)
        ]
        public int MinPremiumRelId
        {
            get
            {
                return (int)this._primaryKey[Properties.MinPremiumRelId];
            }
            set
            {
                this._primaryKey[Properties.MinPremiumRelId] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad BranchCode.
        /// </summary>
        /// <value>Propiedad BranchCode.</value>
        [
            DescriptionKey("BRANCH_CODE_PROPERTY"),
            PersistentProperty(IsNullable = true)
        ]
        public int? BranchCode
        {
            get
            {
                return this._branchCode;
            }
            set
            {
                this._branchCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad RiskMinPremium.
        /// </summary>
        /// <value>Propiedad RiskMinPremium.</value>
        [
            DescriptionKey("RISK_MIN_PREMIUM_PROPERTY"),
            PersistentProperty(IsNullable = true)
        ]
        public decimal? RiskMinPremium
        {
            get
            {
                return this._riskMinPremium;
            }
            set
            {
                this._riskMinPremium = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Key1.
        /// </summary>
        /// <value>Propiedad Key1.</value>
        [
            DescriptionKey("KEY1_PROPERTY"),
            PersistentProperty(IsNullable = true)
        ]
        public decimal? Key1
        {
            get
            {
                return this._key1;
            }
            set
            {
                this._key1 = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad CurrencyCode.
        /// </summary>
        /// <value>Propiedad CurrencyCode.</value>
        [
            DescriptionKey("CURRENCY_CODE_PROPERTY"),
            PersistentProperty()
        ]
        public int CurrencyCode
        {
            get
            {
                return this._currencyCode;
            }
            set
            {
                this._currencyCode = value;
            }
        }

        /*<< Autor:Felipe Barbosa fecha: 19/11/2012  Asunto:  OT:97 - Propiedades para calculo a prorrata y tipo de vigencia compañia: 1*/
        /// <summary>
        /// Devuelve o setea el valor de la propiedad CalculateProrate.
        /// </summary>
        /// <value>Propiedad CalculateProrate.</value>
        [
            DescriptionKey("CALCULATE_PRORATE_PROPERTY"),
            PersistentProperty()
        ]
        public bool? CalculateProrate
        {
            get
            {
                return this._calculateProrate;
            }
            set
            {
                this._calculateProrate = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad CalculateProrate.
        /// </summary>
        /// <value>Propiedad CalculateProrate.</value>
        [
            DescriptionKey("VALIDITY_TYPE_CD_PROPERTY"),
            PersistentProperty()
        ]
        public int? ValidityType
        {
            get
            {
                return this._validityType;
            }
            set
            {
                this._validityType = value;
            }
        }

        /*TODO: Autor:Felipe Barbosa  fecha : 19/11/2012  >>  */


        /// <summary>
        /// Establecer los valores de las propiedades que no forman parte de la clave o identificador de la instancia.
        /// </summary>
        /// <param name="values">
        /// Valores para establecer el estado de la instancia.
        /// </param>
        public override void SetPropertyValues(IDictionary values)
        {
            // Set base properties
            base.SetPropertyValues(values);

            object value;
            value = values[Properties.EndoTypeCode];
            if (value == null)
            {
                throw new PropertyNotNullableException(this.GetType().FullName, Properties.EndoTypeCode);
            }
            this.EndoTypeCode = (int)value;

            value = values[Properties.Key2];
            if (value == null)
            {
                this.Key2 = null;
            }
            else
            {
                this.Key2 = (decimal)value;
            }

            value = values[Properties.PrefixCode];
            if (value == null)
            {
                throw new PropertyNotNullableException(this.GetType().FullName, Properties.PrefixCode);
            }
            this.PrefixCode = (int)value;

            value = values[Properties.SubsMinPremium];
            if (value == null)
            {
                this.SubsMinPremium = null;
            }
            else
            {
                this.SubsMinPremium = (decimal)value;
            }

            value = values[Properties.BranchCode];
            if (value == null)
            {
                this.BranchCode = null;
            }
            else
            {
                this.BranchCode = (int)value;
            }

            value = values[Properties.RiskMinPremium];
            if (value == null)
            {
                this.RiskMinPremium = null;
            }
            else
            {
                this.RiskMinPremium = (decimal)value;
            }

            value = values[Properties.Key1];
            if (value == null)
            {
                this.Key1 = null;
            }
            else
            {
                this.Key1 = (decimal)value;
            }

            value = values[Properties.CurrencyCode];
            if (value == null)
            {
                throw new PropertyNotNullableException(this.GetType().FullName, Properties.CurrencyCode);
            }
            this.CurrencyCode = (int)value;

            /*<< Autor:Felipe Barbosa fecha: 19/11/2012  Asunto:  OT:97 - Propiedades para calculo a prorrata y tipo de vigencia compañia: 1*/
            value = values[Properties.CalculateProrate];
            if (value == null)
            {
                this.CalculateProrate = null;
            }
            else
            {
                this.CalculateProrate = (bool)value;
            }

            value = values[Properties.ValidityType];
            if (value == null)
            {
                this._validityType = null;
            }
            else
            {
                this.CalculateProrate = (bool)value;
            }
            /*TODO: Autor:Felipe Barbosa  fecha : 19/11/2012  >>  */
        }
    }
}