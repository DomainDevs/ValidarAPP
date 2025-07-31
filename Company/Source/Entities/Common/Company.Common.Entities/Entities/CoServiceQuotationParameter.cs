using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;

namespace Sistran.Company.Application.Common.Entities
{
    /// <summary>
    /// Definición de entidad CoServiceQuotationParameter.
    /// </summary>
    [PersistentClass("Sistran.Company.Application.Common.CoServiceQuotationParameter.dict"),
    Serializable(), DescriptionKey("CO_SERVICE_QUOTATION_PARAMETER_ENTITY"),
    TableMap(TableName = "CO_SERVICE_QUOTATION_PARAMETER", Schema = "COMM"),
    ]
    public partial class CoServiceQuotationParameter :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string CoQuotationId = "CoQuotationId";
            public static readonly string SourceCode = "SourceCode";
            public static readonly string EntityParameter = "EntityParameter";
            public static readonly string ParameterValue = "ParameterValue";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="coQuotationId">Propiedad clave CoQuotationId.</param>
        /// <param name="sourceCode">Propiedad clave SourceCode.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int coQuotationId, int sourceCode)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.CoQuotationId, coQuotationId);
            keys.Add(Properties.SourceCode, sourceCode);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="coQuotationId">Propiedad clave CoQuotationId.</param>
        /// <param name="sourceCode">Propiedad clave SourceCode.</param>
        public static PrimaryKey CreatePrimaryKey(int coQuotationId, int sourceCode)
        {
            return InternalCreatePrimaryKey<CoServiceQuotationParameter>(coQuotationId, sourceCode);
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad EntityParameter.
        /// </summary>
        private string _entityParameter = null;
        /// <summary>
        /// Atributo para la propiedad ParameterValue.
        /// </summary>
        private string _parameterValue = null;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="coQuotationId">CoQuotationId key property.</param>
        /// <param name="sourceCode">SourceCode key property.</param>
        public CoServiceQuotationParameter(int coQuotationId, int sourceCode) :
            this(CoServiceQuotationParameter.CreatePrimaryKey(coQuotationId, sourceCode), null)
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
        public CoServiceQuotationParameter(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        /*** Object Properties ********************************/
        /// <summary>
        /// Devuelve o setea el valor de la propiedad CoQuotationId.
        /// </summary>
        /// <value>Propiedad CoQuotationId.</value>
        [
        DescriptionKey("CO_QUOTATION_ID_PROPERTY"),
        PersistentProperty(IsKey = true),
        ColumnMap(ColumnName = "CO_QUOTATION_ID", DbType = System.Data.DbType.String),
        ]
        public int CoQuotationId
        {
            get
            {
                return (int)this._primaryKey[Properties.CoQuotationId];
            }
            set
            {
                this._primaryKey[Properties.CoQuotationId] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad SourceCode.
        /// </summary>
        /// <value>Propiedad SourceCode.</value>
        [
        DescriptionKey("SOURCE_CODE_PROPERTY"),
        PersistentProperty(IsKey = true),
        ColumnMap(ColumnName = "SOURCE_CD", DbType = System.Data.DbType.String),
        ]
        public int SourceCode
        {
            get
            {
                return (int)this._primaryKey[Properties.SourceCode];
            }
            set
            {
                this._primaryKey[Properties.SourceCode] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad EntityParameter.
        /// </summary>
        /// <value>Propiedad EntityParameter.</value>
        [
        DescriptionKey("ENTITY_PARAMETER_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "ENTITY_PARAMETER", DbType = System.Data.DbType.String),
        ]
        public string EntityParameter
        {
            get
            {
                return this._entityParameter;
            }
            set
            {
                this._entityParameter = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad ParameterValue.
        /// </summary>
        /// <value>Propiedad ParameterValue.</value>
        [
        DescriptionKey("PARAMETER_VALUE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "PARAMETER_VALUE", DbType = System.Data.DbType.String),
        ]
        public string ParameterValue
        {
            get
            {
                return this._parameterValue;
            }
            set
            {
                this._parameterValue = value;
            }
        }

    }


}
