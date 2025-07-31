namespace Sistran.Core.Application.AuditServices.Models
{
    using Sistran.Core.Application.AuditServices.Enums;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Modelo Auditoria
    /// </summary>
    [DataContract]
    public class Audit
    {
        /// <summary>
        /// Gets or sets the object identifier.
        /// </summary>
        /// <value>
        /// The object identifier.
        /// </value>
        [DataMember]
        public int ObjectId { get; set; }

        /// <summary>
        /// Nombre Objeto
        /// </summary>
        /// <value>
        /// Nombre Objeto
        /// </value>
        [DataMember]
        public string ObjectName { get; set; }

        /// <summary>
        /// Tipo de Operacion Crud
        /// </summary>
        /// <value>
        /// Tipo de Operacion Crud
        /// </value>
        [DataMember]
        public AudictType? ActionType { get; set; }

        /// <summary>
        /// Fecha Registro
        /// </summary>
        /// <value>
        /// The register date.
        /// </value>
        [DataMember]
        public DateTime RegisterDate { get; set; }


        /// <summary>
        /// Gets or sets the current to.
        /// </summary>
        /// <value>
        /// The current to.
        /// </value>
        [DataMember]
        public DateTime CurrentTo { get; set; }

        /// <summary>
        /// Cambios En los Objetos
        /// </summary>
        /// <value>
        /// Lista de Cambios
        /// </value>
        [DataMember]
        public List<AuditChange> Changes { get; set; }

        /// <summary>
        /// Obtiene el Usurio que genero la Operacion 
        /// </summary>
        /// <value>
        /// Usuario
        /// </value>
        [DataMember]
        public User User { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is serialize.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is serialize; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsSerialize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [package identifier].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [package identifier]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public Package Package { get; set; }

        /// <summary>
        /// Gets or sets the name of the colum.
        /// </summary>
        /// <value>
        /// The name of the colum.
        /// </value>
        [DataMember]
        public string ColumName { get; set; }

        /// <summary>
        /// Gets or sets the name of the colum.
        /// </summary>
        /// <value>
        /// The name of the colum.
        /// </value>
        [DataMember]
        public string ColumnDescription { get; set; }

        /// <summary>
        /// Gets or sets the name of the description.
        /// </summary>
        /// <value>
        /// The name of the description.
        /// </value>
        [DataMember]
        public IDictionary<string, object> Pk { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Audit"/> class.
        /// </summary>
        public Audit()
        {
            Changes = new List<AuditChange>();
        }
    }

    /// <summary>
    /// Cambios Objetos
    /// </summary>
    [DataContract]
    public class AuditChange
    {
        /// <summary>
        /// Obtiene o setea Nombre de la propiedad Modificada
        /// </summary>
        /// <value>
        ///  Nombre de la propiedad Modificada
        /// </value>
        [DataMember]
        public string Id { get; set; }

        /// <summary>
        ///Obtiene o setea el Valor Anterior del Objeto
        /// </summary>
        /// <value>
        ///Valor Anterior
        /// </value>
        [DataMember]
        public string ValueBefore { get; set; }

        /// <summary>
        /// Obtiene o setea el Valor Nuevo del Objeto
        /// </summary>
        /// <value>
        /// Valor Nuevo
        /// </value>
        [DataMember]
        public string ValueAfter { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is serialize.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is serialize; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool IsSerialize { get; set; }
    }
}
