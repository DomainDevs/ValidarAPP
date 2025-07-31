using Sistran.Core.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseEmployeePerson:Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int? Id { get; set; }

        /// <summary>
        /// Descripcion
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Name del Empleado
        /// </summary>
        [DataMember]
        public string  Name { get; set; }

        /// <summary>
        /// Apellido del Empleado
        /// </summary>
        [DataMember]
        public string MotherLastName { get; set; }

        /// <summary>
        /// Numero de Documento del Empleado
        /// </summary>
        [DataMember]
        public string IdCardNo { get; set; }

    }
}
