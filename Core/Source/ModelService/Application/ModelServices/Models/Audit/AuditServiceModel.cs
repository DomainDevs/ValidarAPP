using Sistran.Core.Application.AuditServices.Enums;
using Sistran.Core.Application.ModelServices.Models.Param;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.ModelServices.Models.Audit
{
    /// <summary>
    /// Auditoria
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.ModelServices.Models.Param.ParametricServiceModel" />
    [DataContract]
    public class AuditServiceModel : ParametricServiceModel
    {

        /// <summary>
        /// Codigo Entidad
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
        public AudictTypeService? ActionType { get; set; }

        /// <summary>
        /// Nombre Operacion 
        /// </summary>
        /// <value>
        ///  Nombre Operacion 
        /// </value>
        [DataMember]
        public string ActionTypeName { get; set; }

        /// <summary>
        /// Fecha Registro
        /// </summary>
        /// <value>
        /// The register date.
        /// </value>
        [DataMember]
        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// Fecha Hasta Registro
        /// </summary>
        /// <value>
        /// The register date.
        /// </value>
        [DataMember]
        public DateTime CurrentTo { get; set; }

        /// <summary>
        /// Gets or sets the name of the description.
        /// </summary>
        /// <value>
        /// The name of the description.
        /// </value>
        [DataMember]
        public string ColumnDescription { get; set; }

        /// <summary>
        /// Gets or sets the name of the colum.
        /// </summary>
        /// <value>
        /// The name of the colum.
        /// </value>
        [DataMember]
        public string ColumName { get; set; }

        /// <summary>
        /// Gets or sets the name of the description.
        /// </summary>
        /// <value>
        /// The name of the description.
        /// </value>
        [DataMember]
        public List<PrimaryKeyModel> Pk { get; set; }



        /// <summary>
        /// Gets or sets the package identifier.
        /// </summary>
        /// <value>
        /// The package identifier.
        /// </value>
        [DataMember]
        public PackageServiceModel Package { get; set; }

        /// <summary>
        /// Cambios En los Objetos
        /// </summary>
        /// <value>
        /// Lista de Cambios
        /// </value>
        [DataMember]
        public List<AuditChangeServiceModel> Changes { get; set; }

        /// <summary>
        /// Obtiene el Usurio que genero la Operacion 
        /// </summary>
        /// <value>
        /// Usuario
        /// </value>
        [DataMember]
        public UserServiceModel User { get; set; }

        public AuditServiceModel()
        {
            Changes = new List<AuditChangeServiceModel>();
        }
    }


}


