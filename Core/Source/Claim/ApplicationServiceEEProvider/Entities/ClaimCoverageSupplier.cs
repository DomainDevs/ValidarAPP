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
    /// Definición de entidad ClaimCoverageSupplier.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.ClaimServices.EEProvider.ClaimCoverageSupplier.dict"),
    Serializable(),
    DescriptionKey("CLAIM_COVERAGE_SUPPLIER_ENTITY"),
    TableMap(TableName = "CLAIM_COVERAGE_SUPPLIER", Schema = "CLM"),
    ]
    public partial class ClaimCoverageSupplier :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string ClaimCoverageCode = "ClaimCoverageCode";
            public static readonly string AdjusterCode = "AdjusterCode";
            public static readonly string AnalizerCode = "AnalizerCode";
            public static readonly string ResearcherCode = "ResearcherCode";
            public static readonly string DateInspection = "DateInspection";
            public static readonly string AffectedProperty = "AffectedProperty";
            public static readonly string LossDescription = "LossDescription";
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
            return InternalCreatePrimaryKey<ClaimCoverageSupplier>(claimCoverageCode);
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad AdjusterCode.
        /// </summary>
        private int? _adjusterCode = null;
        /// <summary>
        /// Atributo para la propiedad AnalizerCode.
        /// </summary>
        private int? _analizerCode = null;
        /// <summary>
        /// Atributo para la propiedad ResearcherCode.
        /// </summary>
        private int? _researcherCode = null;
        /// <summary>
        /// Atributo para la propiedad DateInspection.
        /// </summary>
        private DateTime? _dateInspection = null;
        /// <summary>
        /// Atributo para la propiedad AffectedProperty.
        /// </summary>
        private string _affectedProperty = null;
        /// <summary>
        /// Atributo para la propiedad LossDescription.
        /// </summary>
        private string _lossDescription = null;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="claimCoverageCode">ClaimCoverageCode key property.</param>
        public ClaimCoverageSupplier(int claimCoverageCode) :
            this(ClaimCoverageSupplier.CreatePrimaryKey(claimCoverageCode), null)
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
        public ClaimCoverageSupplier(PrimaryKey key, IDictionary initialValues) :
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
        /// Devuelve o setea el valor de la propiedad AdjusterCode.
        /// </summary>
        /// <value>Propiedad AdjusterCode.</value>
        [
        DescriptionKey("ADJUSTER_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "ADJUSTER_CD", DbType = System.Data.DbType.String),
        ]
        public int? AdjusterCode
        {
            get
            {
                return this._adjusterCode;
            }
            set
            {
                this._adjusterCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad AnalizerCode.
        /// </summary>
        /// <value>Propiedad AnalizerCode.</value>
        [
        DescriptionKey("ANALIZER_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "ANALIZER_CD", DbType = System.Data.DbType.String),
        ]
        public int? AnalizerCode
        {
            get
            {
                return this._analizerCode;
            }
            set
            {
                this._analizerCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad ResearcherCode.
        /// </summary>
        /// <value>Propiedad ResearcherCode.</value>
        [
        DescriptionKey("RESEARCHER_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "RESEARCHER_CD", DbType = System.Data.DbType.String),
        ]
        public int? ResearcherCode
        {
            get
            {
                return this._researcherCode;
            }
            set
            {
                this._researcherCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad DateInspection.
        /// </summary>
        /// <value>Propiedad DateInspection.</value>
        [
        DescriptionKey("DATE_INSPECTION_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "DATE_INSPECTION", DbType = System.Data.DbType.String),
        ]
        public DateTime? DateInspection
        {
            get
            {
                return this._dateInspection;
            }
            set
            {
                this._dateInspection = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad AffectedProperty.
        /// </summary>
        /// <value>Propiedad AffectedProperty.</value>
        [
        DescriptionKey("AFFECTED_PROPERTY_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "AFFECTED_PROPERTY", DbType = System.Data.DbType.String),
        ]
        public string AffectedProperty
        {
            get
            {
                return this._affectedProperty;
            }
            set
            {
                this._affectedProperty = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad LossDescription.
        /// </summary>
        /// <value>Propiedad LossDescription.</value>
        [
        DescriptionKey("LOSS_DESCRIPTION_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "LOSS_DESCRIPTION", DbType = System.Data.DbType.String),
        ]
        public string LossDescription
        {
            get
            {
                return this._lossDescription;
            }
            set
            {
                this._lossDescription = value;
            }
        }

    }
}
