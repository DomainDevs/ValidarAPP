using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sistran.Core.Framework;
using Sistran.Core.Framework.DAF;

namespace Sistran.Company.Application.Vehicles.VehicleServices.EEProvider.Entities
{
    /// <summary>
    /// Definición de entidad CoTempCptGoodexpyears.
    /// </summary>
    [
    PersistentClass("Sistran.Company.Application.Vehicles.VehicleServices.CoTempCptGoodexpyears.dict"),
    Serializable(),
    DescriptionKey("CO_TEMP_CPT_GOODEXPYEARS_ENTITY"),
    TableMap(TableName = "CO_TEMP_CPT_GOODEXPYEARS", Schema = "TMP"),
    ]
    public partial class CoTempCptGoodexpyears :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string IdCardTypeCode = "IdCardTypeCode";
            public static readonly string IdCardNo = "IdCardNo";
            public static readonly string GoodExperienceNum = "GoodExperienceNum";
            public static readonly string GoodExpNumRate = "GoodExpNumRate";
            public static readonly string GoodExpNumPrinter = "GoodExpNumPrinter";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="idCardTypeCode">Propiedad clave IdCardTypeCode.</param>
        /// <param name="idCardNo">Propiedad clave IdCardNo.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int idCardTypeCode, string idCardNo)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.IdCardTypeCode, idCardTypeCode);
            keys.Add(Properties.IdCardNo, idCardNo);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="idCardTypeCode">Propiedad clave IdCardTypeCode.</param>
        /// <param name="idCardNo">Propiedad clave IdCardNo.</param>
        public static PrimaryKey CreatePrimaryKey(int idCardTypeCode, string idCardNo)
        {
            return InternalCreatePrimaryKey<CoTempCptGoodexpyears>(idCardTypeCode, idCardNo);
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad GoodExperienceNum.
        /// </summary>
        private int _goodExperienceNum;
        /// <summary>
        /// Atributo para la propiedad GoodExpNumRate.
        /// </summary>
        private string _goodExpNumRate = null;
        /// <summary>
        /// Atributo para la propiedad GoodExpNumPrinter.
        /// </summary>
        private int _goodExpNumPrinter;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="idCardTypeCode">IdCardTypeCode key property.</param>
        /// <param name="idCardNo">IdCardNo key property.</param>
        public CoTempCptGoodexpyears(int idCardTypeCode, string idCardNo) :
            this(CoTempCptGoodexpyears.CreatePrimaryKey(idCardTypeCode, idCardNo), null)
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
        public CoTempCptGoodexpyears(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        /*** Object Properties ********************************/
        /// <summary>
        /// Devuelve o setea el valor de la propiedad IdCardTypeCode.
        /// </summary>
        /// <value>Propiedad IdCardTypeCode.</value>
        [
        DescriptionKey("ID_CARD_TYPE_CODE_PROPERTY"),
        PersistentProperty(IsKey = true),
        ColumnMap(ColumnName = "ID_CARD_TYPE_CD", DbType = System.Data.DbType.String),
        ]
        public int IdCardTypeCode
        {
            get
            {
                return (int)this._primaryKey[Properties.IdCardTypeCode];
            }
            set
            {
                this._primaryKey[Properties.IdCardTypeCode] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad IdCardNo.
        /// </summary>
        /// <value>Propiedad IdCardNo.</value>
        [
        DescriptionKey("ID_CARD_NO_PROPERTY"),
        PersistentProperty(IsKey = true),
        ColumnMap(ColumnName = "ID_CARD_NO", DbType = System.Data.DbType.String),
        ]
        public string IdCardNo
        {
            get
            {
                return (string)this._primaryKey[Properties.IdCardNo];
            }
            set
            {
                this._primaryKey[Properties.IdCardNo] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad GoodExperienceNum.
        /// </summary>
        /// <value>Propiedad GoodExperienceNum.</value>
        [
        DescriptionKey("GOOD_EXPERIENCE_NUM_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "GOOD_EXPERIENCE_NUM", DbType = System.Data.DbType.String),
        ]
        public int GoodExperienceNum
        {
            get
            {
                return this._goodExperienceNum;
            }
            set
            {
                this._goodExperienceNum = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad GoodExpNumRate.
        /// </summary>
        /// <value>Propiedad GoodExpNumRate.</value>
        [
        DescriptionKey("GOOD_EXP_NUM_RATE_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "GOOD_EXP_NUM_RATE", DbType = System.Data.DbType.String),
        ]
        public string GoodExpNumRate
        {
            get
            {
                return this._goodExpNumRate;
            }
            set
            {
                if (value == null)
                {
                    throw new PropertyNotNullableException(this.GetType().FullName, Properties.GoodExpNumRate);
                }
                this._goodExpNumRate = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad GoodExpNumPrinter.
        /// </summary>
        /// <value>Propiedad GoodExpNumPrinter.</value>
        [
        DescriptionKey("GOOD_EXP_NUM_PRINTER_PROPERTY"),
        PersistentProperty(),
        ColumnMap(ColumnName = "GOOD_EXP_NUM_PRINTER", DbType = System.Data.DbType.String),
        ]
        public int GoodExpNumPrinter
        {
            get
            {
                return this._goodExpNumPrinter;
            }
            set
            {
                this._goodExpNumPrinter = value;
            }
        }

    }
}
