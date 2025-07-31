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
    /// Definición de entidad ClaimNumber.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.ClaimServices.EEProvider.ClaimNumber.dict"),
    Serializable(),
    DescriptionKey("CLAIM_NUMBER_ENTITY"),
    TableMap(TableName = "CLAIM_NUMBER", Schema = "CLM"),
    ]
    public partial class ClaimNumber :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string PrefixCode = "PrefixCode";
            public static readonly string BranchCode = "BranchCode";
            public static readonly string ClaimNumberCode = "ClaimNumberCode";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="prefixCode">Propiedad clave PrefixCode.</param>
        /// <param name="branchCode">Propiedad clave BranchCode.</param>
        /// <param name="claimNumberCode">Propiedad clave ClaimNumberCode.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int prefixCode, int branchCode, int claimNumberCode)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.PrefixCode, prefixCode);
            keys.Add(Properties.BranchCode, branchCode);
            keys.Add(Properties.ClaimNumberCode, claimNumberCode);

            return new PrimaryKey<T>(keys);
        }

        protected static PrimaryKey InternalCreatePrimaryKey<T>()
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.PrefixCode, null);
            keys.Add(Properties.BranchCode, null);
            keys.Add(Properties.ClaimNumberCode, null);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="prefixCode">Propiedad clave PrefixCode.</param>
        /// <param name="branchCode">Propiedad clave BranchCode.</param>
        /// <param name="claimNumberCode">Propiedad clave ClaimNumberCode.</param>
        public static PrimaryKey CreatePrimaryKey(int prefixCode, int branchCode, int claimNumberCode)
        {
            return InternalCreatePrimaryKey<ClaimNumber>(prefixCode, branchCode, claimNumberCode);
        }

        public static PrimaryKey CreatePrimaryKey()
        {
            return InternalCreatePrimaryKey<ClaimNumber>();
        }
        #endregion

        //*** Object Attributes ********************************

        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="prefixCode">PrefixCode key property.</param>
        /// <param name="branchCode">BranchCode key property.</param>
        /// <param name="claimNumberCode">ClaimNumberCode key property.</param>
        public ClaimNumber(int prefixCode, int branchCode, int claimNumberCode) :
            this(ClaimNumber.CreatePrimaryKey(prefixCode, branchCode, claimNumberCode), null)
        {
        }

        public ClaimNumber() :
            this(ClaimNumber.CreatePrimaryKey(), null)
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
        public ClaimNumber(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        /*** Object Properties ********************************/
        /// <summary>
        /// Devuelve o setea el valor de la propiedad PrefixCode.
        /// </summary>
        /// <value>Propiedad PrefixCode.</value>
        [
        DescriptionKey("PREFIX_CODE_PROPERTY"),
        PersistentProperty(IsKey = true, KeyType = "None"),
        ColumnMap(ColumnName = "PREFIX_CD", DbType = System.Data.DbType.String),
        ]
        public int PrefixCode
        {
            get
            {
                return (int)this._primaryKey[Properties.PrefixCode];
            }
            set
            {
                this._primaryKey[Properties.PrefixCode] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad BranchCode.
        /// </summary>
        /// <value>Propiedad BranchCode.</value>
        [
        DescriptionKey("BRANCH_CODE_PROPERTY"),
        PersistentProperty(IsKey = true, KeyType = "None"),
        ColumnMap(ColumnName = "BRANCH_CD", DbType = System.Data.DbType.String),
        ]
        public int BranchCode
        {
            get
            {
                return (int)this._primaryKey[Properties.BranchCode];
            }
            set
            {
                this._primaryKey[Properties.BranchCode] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad ClaimNumberCode.
        /// </summary>
        /// <value>Propiedad ClaimNumberCode.</value>
        [
        DescriptionKey("CLAIM_NUMBER_CODE_PROPERTY"),
        PersistentProperty(IsKey = true, KeyType = "None"),
        ColumnMap(ColumnName = "CLAIM_NUMBER_CD", DbType = System.Data.DbType.String),
        ]
        public int ClaimNumberCode
        {
            get
            {
                return (int)this._primaryKey[Properties.ClaimNumberCode];
            }
            set
            {
                this._primaryKey[Properties.ClaimNumberCode] = value;
            }
        }

    }
}
