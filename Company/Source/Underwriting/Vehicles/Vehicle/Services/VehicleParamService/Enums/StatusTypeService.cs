using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Vehicles.VehicleApplicationService.Enums
{
    [DataContract]
    public enum StatusTypeService
    {
        /// <summary>
        /// Estado original
        /// </summary>
        [EnumMember]
        Original = 1,

        /// <summary>
        /// Creación del objeto
        /// </summary>
        [EnumMember]
        Create = 2,

        /// <summary>
        /// Actualización del objeto
        /// </summary>
        [EnumMember]
        Update = 3,

        /// <summary>
        /// Eliminación del objeto
        /// </summary>
        [EnumMember]
        Delete = 4,

        /// <summary>
        /// Error en el proceso
        /// </summary>
        [EnumMember]
        Error = 5
    }
}
