using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sistran.Company.Application.Common.Entities
{
    /// <summary>
    /// Definición de entidad CoInfringementState.
    /// </summary>
    [
    PersistentClass("Sistran.Company.Application.Common.CoInfringementState.dict"),
    Serializable(),
    DescriptionKey("CO_INFRINGEMENT_STATE_ENTITY"),
    TableMap(TableName = "CO_INFRINGEMENT_STATE", Schema = "COMM"),
    ]
    public partial class CoInfringementState :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string InfringementStateCode = "InfringementStateCode";
            public static readonly string Description = "Description";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="infringementStateCode">Propiedad clave InfringementStateCode.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int infringementStateCode)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.InfringementStateCode, infringementStateCode);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="infringementStateCode">Propiedad clave InfringementStateCode.</param>
        public static PrimaryKey CreatePrimaryKey(int infringementStateCode)
        {
            return InternalCreatePrimaryKey<CoInfringementState>(infringementStateCode);
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        private string _description = null;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="infringementStateCode">InfringementStateCode key property.</param>
        public CoInfringementState(int infringementStateCode) :
            this(CoInfringementState.CreatePrimaryKey(infringementStateCode), null)
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
        public CoInfringementState(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        /*** Object Properties ********************************/
        /// <summary>
        /// Devuelve o setea el valor de la propiedad InfringementStateCode.
        /// </summary>
        /// <value>Propiedad InfringementStateCode.</value>
        [
        DescriptionKey("INFRINGEMENT_STATE_CODE_PROPERTY"),
        PersistentProperty(IsKey = true),
        ColumnMap(ColumnName = "INFRINGEMENT_STATE_CD", DbType = System.Data.DbType.String),
        ]
        public int InfringementStateCode
        {
            get
            {
                return (int)this._primaryKey[Properties.InfringementStateCode];
            }
            set
            {
                this._primaryKey[Properties.InfringementStateCode] = value;
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
