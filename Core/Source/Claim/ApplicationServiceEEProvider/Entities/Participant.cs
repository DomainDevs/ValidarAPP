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
    /// Definición de entidad Participant.
    /// </summary>
    [
    PersistentClass("Sistran.Core.Application.ClaimServices.EEProvider.Participant.dict"),
    Serializable(),
    DescriptionKey("PARTICIPANT_ENTITY"),
    TableMap(TableName = "PARTICIPANT", Schema = "CLM"),
    ]
    public partial class Participant :
        BusinessObject2
    {
        #region static
        /// <summary>
        /// Propiedades públicas de la entidad.
        /// </summary>
        public static class Properties
        {
            public static readonly string ParticipantCode = "ParticipantCode";
            public static readonly string Fullname = "Fullname";
            public static readonly string DocumentNumber = "DocumentNumber";
            public static readonly string DocumentTypeCode = "DocumentTypeCode";
            public static readonly string Address = "Address";
            public static readonly string Phone = "Phone";
        }

        /// <summary>
        /// Crea una clave primaria para una clase concreta.
        /// </summary>
        /// <param name="concreteClass">Clase concreta.</param>
        /// <param name="participantCode">Propiedad clave ParticipantCode.</param>
        /// <returns>Clave primaria.</returns>
        protected static PrimaryKey InternalCreatePrimaryKey<T>(int participantCode)
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.ParticipantCode, participantCode);

            return new PrimaryKey<T>(keys);
        }

        protected static PrimaryKey InternalCreatePrimaryKey<T>()
        {
            Dictionary<string, object> keys = new Dictionary<string, object>();
            keys.Add(Properties.ParticipantCode, null);

            return new PrimaryKey<T>(keys);
        }

        /// <summary>
        /// Crea una clave primaria para esta clase.
        /// </summary>
        /// <param name="participantCode">Propiedad clave ParticipantCode.</param>
        public static PrimaryKey CreatePrimaryKey(int participantCode)
        {
            return InternalCreatePrimaryKey<Participant>(participantCode);
        }

        public static PrimaryKey CreatePrimaryKey()
        {
            return InternalCreatePrimaryKey<Participant>();
        }
        #endregion

        //*** Object Attributes ********************************

        /// <summary>
        /// Atributo para la propiedad Fullname.
        /// </summary>
        private string _fullname = null;
        /// <summary>
        /// Atributo para la propiedad DocumentNumber.
        /// </summary>
        private string _documentNumber = null;
        /// <summary>
        /// Atributo para la propiedad DocumentTypeCode.
        /// </summary>
        private int? _documentTypeCode = null;
        /// <summary>
        /// Atributo para la propiedad Address.
        /// </summary>
        private string _address = null;
        /// <summary>
        /// Atributo para la propiedad Phone.
        /// </summary>
        private string _phone = null;
        //*** Object Constructors ********************************

        /// <summary>
        /// Constructor de instancia de la clase en base a las propiedades claves.
        /// </summary>
        /// <param name="participantCode">ParticipantCode key property.</param>
        public Participant(int participantCode) :
            this(Participant.CreatePrimaryKey(participantCode), null)
        {
        }

        public Participant() :
            this(Participant.CreatePrimaryKey(), null)
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
        public Participant(PrimaryKey key, IDictionary initialValues) :
            base(key, initialValues)
        {
        }

        /*** Object Properties ********************************/
        /// <summary>
        /// Devuelve o setea el valor de la propiedad ParticipantCode.
        /// </summary>
        /// <value>Propiedad ParticipantCode.</value>
        [
        DescriptionKey("PARTICIPANT_CODE_PROPERTY"),
        PersistentProperty(IsKey = true),
        ColumnMap(ColumnName = "PARTICIPANT_CD", DbType = System.Data.DbType.String),
        ]
        public int ParticipantCode
        {
            get
            {
                return (int)this._primaryKey[Properties.ParticipantCode];
            }
            set
            {
                this._primaryKey[Properties.ParticipantCode] = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Fullname.
        /// </summary>
        /// <value>Propiedad Fullname.</value>
        [
        DescriptionKey("FULLNAME_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "FULLNAME", DbType = System.Data.DbType.String),
        ]
        public string Fullname
        {
            get
            {
                return this._fullname;
            }
            set
            {
                this._fullname = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad DocumentNumber.
        /// </summary>
        /// <value>Propiedad DocumentNumber.</value>
        [
        DescriptionKey("DOCUMENT_NUMBER_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "DOCUMENT_NUMBER", DbType = System.Data.DbType.String),
        ]
        public string DocumentNumber
        {
            get
            {
                return this._documentNumber;
            }
            set
            {
                this._documentNumber = value;
            }
        }


        /// <summary>
        /// Devuelve o setea el valor de la propiedad DocumentTypeCode.
        /// </summary>
        /// <value>Propiedad DocumentTypeCode.</value>
        [
        DescriptionKey("DOCUMENT_TYPE_CODE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "DOCUMENT_TYPE_CD", DbType = System.Data.DbType.String),
        ]
        public int? DocumentTypeCode
        {
            get
            {
                return this._documentTypeCode;
            }
            set
            {
                this._documentTypeCode = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Address.
        /// </summary>
        /// <value>Propiedad Address.</value>
        [
        DescriptionKey("ADDRESS_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "ADDRESS", DbType = System.Data.DbType.String),
        ]
        public string Address
        {
            get
            {
                return this._address;
            }
            set
            {
                this._address = value;
            }
        }

        /// <summary>
        /// Devuelve o setea el valor de la propiedad Phone.
        /// </summary>
        /// <value>Propiedad Phone.</value>
        [
        DescriptionKey("PHONE_PROPERTY"),
        PersistentProperty(IsNullable = true),
        ColumnMap(ColumnName = "PHONE", DbType = System.Data.DbType.String),
        ]
        public string Phone
        {
            get
            {
                return this._phone;
            }
            set
            {
                this._phone = value;
            }
        }

    }
}