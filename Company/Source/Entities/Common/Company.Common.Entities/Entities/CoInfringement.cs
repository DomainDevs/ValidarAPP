using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Sistran.Company.Application.Common.Entities
{
    /// <summary>
    /// Definición de entidad CoInfringement.
    /// </summary>
    [
    PersistentClass("Sistran.Company.Application.Common.CoInfringement.dict"),
    Serializable(),
    DescriptionKey("CO_INFRINGEMENT_ENTITY"),
    TableMap(TableName = "CO_INFRINGEMENT", Schema = "COMM"),
    ]
    public class CoInfringement : BusinessObject
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string InfringementCode = "InfringementCode";
            public static readonly string InfringementNewCode = "InfringementNewCode";
            public static readonly string InfringementPreviousCode = "InfringementPreviousCode";
            public static readonly string Description = "Description";
            public static readonly string GroupInfringementCode = "GroupInfringementCode";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="infringementCode">Propiedad clave InfringementCode.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int infringementCode)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.InfringementCode, infringementCode);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="infringementCode">Propiedad clave InfringementCode.</param>
        public static PrimaryKey CreatePrimaryKey(int infringementCode)
        {
            return InternalCreatePrimaryKey<CoInfringement>(infringementCode);
        }

        public static PrimaryKey CreatePrimaryKey()
        {
            ListDictionary keys = new ListDictionary();
            keys.Add(Properties.InfringementCode, null);
            return new PrimaryKey(typeof(CoInfringement), keys);
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad InfringementNewCode.
        /// </summary>
        private string _infringementNewCode = null;
        /// <summary>
        /// Atributo para la propiedad InfringementPreviousCode.
        /// </summary>
        private int? _infringementPreviousCode = null;
        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        private string _description = null;
        /// <summary>
        /// Atributo para la propiedad Group.
        /// </summary>
        private int? _groupInfringementCode = null;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="infringementCode">InfringementCode key property.</param>
        public CoInfringement(int infringementCode) :
            this(CoInfringement.CreatePrimaryKey(infringementCode), null)
        {
        }

        public CoInfringement() :
            base(CoInfringement.CreatePrimaryKey(), null)
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
        public CoInfringement(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        /*** Object Properties ********************************/
        /// <summary>
        /// Devuelve o setea el valor de la propiedad InfringementCode.
        /// </summary>
        /// <value>Propiedad InfringementCode.</value>
        [
        DescriptionKey("INFRINGEMENT_CODE_PROPERTY"),
        PersistentProperty(false, IsKey = true),
        ColumnMap(ColumnName = "INFRINGEMENT_CD", DbType = System.Data.DbType.String),
        ]
        public int InfringementCode
        {
            get
            {
                return (int)this._primaryKey[Properties.InfringementCode];
            }
            set
            {
                this._primaryKey[Properties.InfringementCode] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad InfringementNewCode.
        /// </summary>
        /// <value>Propiedad InfringementNewCode.</value>
        [
        DescriptionKey("INFRINGEMENT_NEW_CODE_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "INFRINGEMENT_NEW_CD", DbType = System.Data.DbType.String),
        ]
        public string InfringementNewCode
        {
            get
            {
                return this._infringementNewCode;
            }
            set
            {
                if (value == null)
                {
                    throw new PropertyNotNullableException(this.GetType().FullName, Properties.InfringementNewCode);
                }
                this._infringementNewCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad InfringementPreviousCode.
        /// </summary>
        /// <value>Propiedad InfringementPreviousCode.</value>
        [
        DescriptionKey("INFRINGEMENT_PREVIOUS_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "INFRINGEMENT_PREVIOUS_CD", DbType = System.Data.DbType.String),
        ]
        public int? InfringementPreviousCode
        {
            get
            {
                return this._infringementPreviousCode;
            }
            set
            {
                this._infringementPreviousCode = value;
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

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Group.
        /// </summary>
        /// <value>Propiedad Group.</value>
        [
        DescriptionKey("GROUP_INFRINGEMENT_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "GROUP_INFRINGEMENT_CD", DbType = System.Data.DbType.Int32),
        ]
        public int? GroupInfringementCode
        {
            get
            {
                return this._groupInfringementCode;
            }
            set
            {
                this._groupInfringementCode = value;
            }
        }
    }
}
