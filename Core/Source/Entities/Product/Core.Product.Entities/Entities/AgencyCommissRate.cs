using System;
using System.Collections;
using System.Collections.Generic;
using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.Product.Entities
{
    /// <summary>
    /// Definición de entidad AgencyCommissRate.
    /// </summary>
    [                     
    PersistentClass("Sistran.Core.Application.Product.AgencyCommissRate.dict"),
    Serializable(),
    DescriptionKey("AGENCY_COMMISS_RATE_ENTITY"),
    TableMap(TableName = "AGENCY_COMMISS_RATE", Schema = "PROD"),
    ]
    public partial class AgencyCommissRate :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string AgencyCommissRateId = "AgencyCommissRateId";
            public static readonly string IndividualId = "IndividualId";
            public static readonly string AgentAgencyId = "AgentAgencyId";
            public static readonly string PrefixCode = "PrefixCode";
            public static readonly string StCommissPercentage = "StCommissPercentage";
            public static readonly string LineBusinessCode = "LineBusinessCode";
            public static readonly string SubLineBusinessCode = "SubLineBusinessCode";
            public static readonly string AdditCommissPercentage = "AdditCommissPercentage";
            public static readonly string SchCommissPercentage = "SchCommissPercentage";
            public static readonly string StDisCommissPercentage = "StDisCommissPercentage";
            public static readonly string AdditDisCommissPercentage = "AdditDisCommissPercentage";
            public static readonly string IncCommissAdFacPercentage = "IncCommissAdFacPercentage";
            public static readonly string DimCommissAdFacPercentage = "DimCommissAdFacPercentage";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="agencyCommissRateId">Propiedad clave AgencyCommissRateId.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int agencyCommissRateId)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.AgencyCommissRateId, agencyCommissRateId);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="agencyCommissRateId">Propiedad clave AgencyCommissRateId.</param>
        public static PrimaryKey CreatePrimaryKey(int agencyCommissRateId)
        {
            return InternalCreatePrimaryKey<AgencyCommissRate>(agencyCommissRateId);
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad IndividualId.
        /// </summary>
        private int _individualId;
        /// <summary>
        /// Atributo para la propiedad AgentAgencyId.
        /// </summary>
        private int _agentAgencyId;
        /// <summary>
        /// Atributo para la propiedad PrefixCode.
        /// </summary>
        private int _prefixCode;
        /// <summary>
        /// Atributo para la propiedad StCommissPercentage.
        /// </summary>
        private decimal _stCommissPercentage;
        /// <summary>
        /// Atributo para la propiedad LineBusinessCode.
        /// </summary>
        private int? _lineBusinessCode = null;
        /// <summary>
        /// Atributo para la propiedad SubLineBusinessCode.
        /// </summary>
        private int? _subLineBusinessCode = null;
        /// <summary>
        /// Atributo para la propiedad AdditCommissPercentage.
        /// </summary>
        private decimal? _additCommissPercentage = null;
        /// <summary>
        /// Atributo para la propiedad SchCommissPercentage.
        /// </summary>
        private decimal? _schCommissPercentage = null;
        /// <summary>
        /// Atributo para la propiedad StDisCommissPercentage.
        /// </summary>
        private decimal? _stDisCommissPercentage = null;
        /// <summary>
        /// Atributo para la propiedad AdditDisCommissPercentage.
        /// </summary>
        private decimal? _additDisCommissPercentage = null;
        /// <summary>
        /// Atributo para la propiedad IncCommissAdFacPercentage.
        /// </summary>
        private decimal? _incCommissAdFacPercentage = null;
        /// <summary>
        /// Atributo para la propiedad DimCommissAdFacPercentage.
        /// </summary>
        private decimal? _dimCommissAdFacPercentage = null;
        
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="agencyCommissRateId">AgencyCommissRateId key property.</param>
        public AgencyCommissRate(int agencyCommissRateId) :
            this(AgencyCommissRate.CreatePrimaryKey(agencyCommissRateId), null)
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
        public AgencyCommissRate(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        /*** Object Properties ********************************/
        /// <summary>
        /// Devuelve o setea el valor de la propiedad AgencyCommissRateId.
        /// </summary>
        /// <value>Propiedad AgencyCommissRateId.</value>
        [
        DescriptionKey("AGENCY_COMMISS_RATE_ID_PROPERTY"),
        PersistentProperty(IsKey = true),
        ColumnMap(ColumnName = "AGENCY_COMMISS_RATE_ID", DbType = System.Data.DbType.String),
        ]
        public int AgencyCommissRateId
        {
            get
            {
                return (int)this._primaryKey[Properties.AgencyCommissRateId];
            }
            set
            {
                this._primaryKey[Properties.AgencyCommissRateId] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad IndividualId.
        /// </summary>
        /// <value>Propiedad IndividualId.</value>
        [
        DescriptionKey("INDIVIDUAL_ID_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "INDIVIDUAL_ID", DbType = System.Data.DbType.String),
        ]
        public int IndividualId
        {
            get
            {
                return this._individualId;
            }
            set
            {
                this._individualId = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad AgentAgencyId.
        /// </summary>
        /// <value>Propiedad AgentAgencyId.</value>
        [
        DescriptionKey("AGENT_AGENCY_ID_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "AGENT_AGENCY_ID", DbType = System.Data.DbType.String),
        ]
        public int AgentAgencyId
        {
            get
            {
                return this._agentAgencyId;
            }
            set
            {
                this._agentAgencyId = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad PrefixCode.
        /// </summary>
        /// <value>Propiedad PrefixCode.</value>
        [
        DescriptionKey("PREFIX_CODE_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "PREFIX_CD", DbType = System.Data.DbType.String),
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
        /// Devuelve o setea el valor de la propiedad StCommissPercentage.
        /// </summary>
        /// <value>Propiedad StCommissPercentage.</value>
        [
        DescriptionKey("ST_COMMISS_PERCENTAGE_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "ST_COMMISS_PCT", DbType = System.Data.DbType.Decimal),
        ]
        public decimal StCommissPercentage
        {
            get
            {
                return this._stCommissPercentage;
            }
            set
            {
                this._stCommissPercentage = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad LineBusinessCode.
        /// </summary>
        /// <value>Propiedad LineBusinessCode.</value>
        [
        DescriptionKey("LINE_BUSINESS_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "LINE_BUSINESS_CD", DbType = System.Data.DbType.String),
        ]
        public int? LineBusinessCode
        {
            get
            {
                return this._lineBusinessCode;
            }
            set
            {
                this._lineBusinessCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad SubLineBusinessCode.
        /// </summary>
        /// <value>Propiedad SubLineBusinessCode.</value>
        [
        DescriptionKey("SUB_LINE_BUSINESS_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "SUB_LINE_BUSINESS_CD", DbType = System.Data.DbType.String),
        ]
        public int? SubLineBusinessCode
        {
            get
            {
                return this._subLineBusinessCode;
            }
            set
            {
                this._subLineBusinessCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad AdditCommissPercentage.
        /// </summary>
        /// <value>Propiedad AdditCommissPercentage.</value>
        [
        DescriptionKey("ADDIT_COMMISS_PERCENTAGE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "ADDIT_COMMISS_PCT", DbType = System.Data.DbType.Decimal),
        ]
        public decimal? AdditCommissPercentage
        {
            get
            {
                return this._additCommissPercentage;
            }
            set
            {
                this._additCommissPercentage = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad SchCommissPercentage.
        /// </summary>
        /// <value>Propiedad SchCommissPercentage.</value>
        [
        DescriptionKey("SCH_COMMISS_PERCENTAGE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "SCH_COMMISS_PCT", DbType = System.Data.DbType.Decimal),
        ]
        public decimal? SchCommissPercentage
        {
            get
            {
                return this._schCommissPercentage;
            }
            set
            {
                this._schCommissPercentage = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad StDisCommissPercentage.
        /// </summary>
        /// <value>Propiedad StDisCommissPercentage.</value>
        [
        DescriptionKey("ST_DIS_COMMISS_PERCENTAGE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "ST_DIS_COMMISS_PCT", DbType = System.Data.DbType.Decimal),
        ]
        public decimal? StDisCommissPercentage
        {
            get
            {
                return this._stDisCommissPercentage;
            }
            set
            {
                this._stDisCommissPercentage = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad AdditDisCommissPercentage.
        /// </summary>
        /// <value>Propiedad AdditDisCommissPercentage.</value>
        [
        DescriptionKey("ADDIT_DIS_COMMISS_PERCENTAGE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "ADDIT_DIS_COMMISS_PCT", DbType = System.Data.DbType.Decimal),
        ]
        public decimal? AdditDisCommissPercentage
        {
            get
            {
                return this._additDisCommissPercentage;
            }
            set
            {
                this._additDisCommissPercentage = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad IncCommissAdFacPercentage.
        /// </summary>
        /// <value>Propiedad IncCommissAdFacPercentage.</value>
        [
        DescriptionKey("INC_COMMISS_AD_FAC_PERCENTAGE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "INC_COMMISS_AD_FAC_PCT", DbType = System.Data.DbType.Decimal),
        ]
        public decimal? IncCommissAdFacPercentage
        {
            get
            {
                return this._incCommissAdFacPercentage;
            }
            set
            {
                this._incCommissAdFacPercentage = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad DimCommissAdFacPercentage.
        /// </summary>
        /// <value>Propiedad DimCommissAdFacPercentage.</value>
        [
        DescriptionKey("DIM_COMMISS_AD_FAC_PERCENTAGE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "DIM_COMMISS_AD_FAC_PCT", DbType = System.Data.DbType.Decimal),
        ]
        public decimal? DimCommissAdFacPercentage
        {
            get
            {
                return this._dimCommissAdFacPercentage;
            }
            set
            {
                this._dimCommissAdFacPercentage = value;
            }
        }

       
        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        public AgencyCommissRate() :
          this(AgencyCommissRate.CreatePrimaryKey(), null)
        {
        }
        /// <summary>
		/// Crea una clave primaria para esta clase.
		/// </summary>
        public static PrimaryKey CreatePrimaryKey()
        {
            return InternalCreatePrimaryKey<AgencyCommissRate>();
        }
        /// <summary>
		/// Crea una clave primaria para una clase concreta.
		/// </summary>
		/// <param name="concreteClass">Clase concreta.</param>
		/// <param name="agencyCommissRateId">Propiedad clave AgencyCommissRateId.</param>
		/// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>()
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.AgencyCommissRateId, null);

            return new PrimaryKey<T>(keys);
        }
    }
}
