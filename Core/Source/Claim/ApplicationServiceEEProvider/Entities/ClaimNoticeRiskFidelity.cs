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
    /// Definición de entidad ClaimNoticeRiskFidelity.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.ClaimServices.EEProvider.ClaimNoticeRiskFidelity.dict"),
    Serializable(),
    DescriptionKey("CLAIM_NOTICE_RISK_FIDELITY_ENTITY"),
    TableMap(TableName = "CLAIM_NOTICE_RISK_FIDELITY", Schema = "CLM"),
    ]
    public partial class ClaimNoticeRiskFidelity :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string ClaimNoticeCode = "ClaimNoticeCode";
            public static readonly string RiskCommercialClassCode = "RiskCommercialClassCode";
            public static readonly string OccupationCode = "OccupationCode";
            public static readonly string DiscoveryDate = "DiscoveryDate";
            public static readonly string Description = "Description";
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

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="claimNoticeCode">Propiedad clave ClaimNoticeCode.</param>
        public static PrimaryKey CreatePrimaryKey(int claimNoticeCode)
        {
            return InternalCreatePrimaryKey<ClaimNoticeRiskFidelity>(claimNoticeCode);
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad RiskCommercialClassCode.
        /// </summary>
        private int _riskCommercialClassCode;
        /// <summary>
        /// Atributo para la propiedad OccupationCode.
        /// </summary>
        private int _occupationCode;
        /// <summary>
        /// Atributo para la propiedad DiscoveryDate.
        /// </summary>
        private DateTime _discoveryDate;
        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        private string _description = null;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="claimNoticeCode">ClaimNoticeCode key property.</param>
        public ClaimNoticeRiskFidelity(int claimNoticeCode) :
            this(ClaimNoticeRiskFidelity.CreatePrimaryKey(claimNoticeCode), null)
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
        public ClaimNoticeRiskFidelity(PrimaryKey key, IDictionary initialValues) :
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
        PersistentProperty(IsKey = true),
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
        /// Devuelve o setea el valor de la propiedad RiskCommercialClassCode.
        /// </summary>
        /// <value>Propiedad RiskCommercialClassCode.</value>
        [
        DescriptionKey("RISK_COMMERCIAL_CLASS_CODE_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "RISK_COMMERCIAL_CLASS_CD", DbType = System.Data.DbType.String),
        ]
        public int RiskCommercialClassCode
        {
            get
            {
                return this._riskCommercialClassCode;
            }
            set
            {
                this._riskCommercialClassCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad OccupationCode.
        /// </summary>
        /// <value>Propiedad OccupationCode.</value>
        [
        DescriptionKey("OCCUPATION_CODE_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "OCCUPATION_CD", DbType = System.Data.DbType.String),
        ]
        public int OccupationCode
        {
            get
            {
                return this._occupationCode;
            }
            set
            {
                this._occupationCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad DiscoveryDate.
        /// </summary>
        /// <value>Propiedad DiscoveryDate.</value>
        [
        DescriptionKey("DISCOVERY_DATE_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "DISCOVERY_DATE", DbType = System.Data.DbType.String),
        ]
        public DateTime DiscoveryDate
        {
            get
            {
                return this._discoveryDate;
            }
            set
            {
                this._discoveryDate = value;
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

    }
}
