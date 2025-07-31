using Sistran.Core.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Marines.MarineBusinessService.Models.Base
{
    [DataContract]
    public class BaseMarine : Extension
    {
        /// <summary>
        /// Año de fabricación
        /// </summary>
        [DataMember]
        public int CurrentManufacturing { get; set; }

        /// <summary>
        /// Nombre de Barco
        /// </summary>
        [DataMember]
        public string BoatName { get; set; }

        /// <summary>
        /// Id uso de aeronave
        /// </summary>
        [DataMember]
        public int UseId { get; set; }
        /// <summary>
        /// Descripcion uso aeronave
        /// </summary>
        [DataMember]
        public string UseDescription { get; set; }

        /// <summary>
        /// Número de Matricula
        /// </summary>
        [DataMember]
        public string NumberRegister { get; set; }

        /// <summary>
        /// Explotador de la aeronave
        /// </summary>
        [DataMember]
        public int OperatorId { get; set; }
       
    }
}

