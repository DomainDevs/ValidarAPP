using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class EmployeePersonDTO
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
        public string Name { get; set; }

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
