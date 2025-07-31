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
    /// Definición de entidad ClaimNoticeLimit.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.ClaimServices.EEProvider.ClaimNoticeLimit.dict"),
    Serializable(),
    DescriptionKey("CLAIM_NOTICE_LIMIT_ENTITY"),
    TableMap(TableName = "CLAIM_NOTICE_LIMIT", Schema = "CLM"),
    ]
    public partial class ClaimNoticeLimit :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string LineBusinessCode = "LineBusinessCode";
            public static readonly string LimitDay = "LimitDay";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="lineBusinessCode">Propiedad clave LineBusinessCode.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int lineBusinessCode)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.LineBusinessCode, lineBusinessCode);

            return new PrimaryKey<T>(keys);
        }

        protected static PrimaryKey InternalCreatePrimaryKey<T>()
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.LineBusinessCode, null);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="lineBusinessCode">Propiedad clave LineBusinessCode.</param>
        public static PrimaryKey CreatePrimaryKey(int lineBusinessCode)
        {
            return InternalCreatePrimaryKey<ClaimNoticeLimit>(lineBusinessCode);
        }

        public static PrimaryKey CreatePrimaryKey()
        {
            return InternalCreatePrimaryKey<ClaimNoticeLimit>();
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad LimitDay.
        /// </summary>
        private int? _limitDay = null;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="lineBusinessCode">LineBusinessCode key property.</param>
        public ClaimNoticeLimit(int lineBusinessCode) :
            this(ClaimNoticeLimit.CreatePrimaryKey(lineBusinessCode), null)
        {
        }

        public ClaimNoticeLimit() :
            this(ClaimNoticeLimit.CreatePrimaryKey(), null)
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
        public ClaimNoticeLimit(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        /*** Object Properties ********************************/
        /// <summary>
        /// Devuelve o setea el valor de la propiedad LineBusinessCode.
        /// </summary>
        /// <value>Propiedad LineBusinessCode.</value>
        [
        DescriptionKey("LINE_BUSINESS_CODE_PROPERTY"),
        PersistentProperty(IsKey = true, KeyType = "None"),
        ColumnMap(ColumnName = "LINE_BUSINESS_CD", DbType = System.Data.DbType.String),
        ]
        public int LineBusinessCode
        {
            get
            {
                return (int)this._primaryKey[Properties.LineBusinessCode];
            }
            set
            {
                this._primaryKey[Properties.LineBusinessCode] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad LimitDay.
        /// </summary>
        /// <value>Propiedad LimitDay.</value>
        [
        DescriptionKey("LIMIT_DAY_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "LIMIT_DAY", DbType = System.Data.DbType.String),
        ]
        public int? LimitDay
        {
            get
            {
                return this._limitDay;
            }
            set
            {
                this._limitDay = value;
            }
        }

    }
}
