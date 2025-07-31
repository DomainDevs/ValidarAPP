using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class ScoreCredit
    {
        /// <summary>
        /// Identificador de Score Credit
        /// </summary>
        [DataMember]
        public int ScoreCreditId { get; set; }

        /// <summary>
        /// Identificador de Individual
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Identificador de Tipo de Identificación
        /// </summary>
        [DataMember]
        public int IdCardTypeCode { get; set; }

        /// <summary>
        /// Identificación
        /// </summary>
        [DataMember]
        public string IdCardNo { get; set; }

        /// <summary>
        /// Valor del score
        /// </summary>
        [DataMember]
        public string Score { get; set; }

        /// <summary>
        /// Código de Respuesta
        /// </summary>
        [DataMember]
        public int ResponseCode { get; set; }

        /// <summary>
        /// Respuesta
        /// </summary>
        [DataMember]
        public string Response { get; set; }

        /// <summary>
        /// Fecha de petición
        /// </summary>
        [DataMember]
        public DateTime DateRequest { get; set; }

        /// <summary>
        /// Es valor por defecto
        /// </summary>
        [DataMember]
        public bool? IsDefaultValue { get; set; }

        /// <summary>
        /// Identificador del usuario
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Propiedad A1
        /// </summary>
        [DataMember]
        public string A1 { get; set; }

        /// <summary>
        /// Propiedad A2
        /// </summary>
        [DataMember]
        public string A2 { get; set; }

        /// <summary>
        /// Propiedad A3
        /// </summary>
        [DataMember]
        public string A3 { get; set; }

        /// <summary>
        /// Propiedad A4
        /// </summary>
        [DataMember]
        public string A4 { get; set; }

        /// <summary>
        /// Propiedad A5
        /// </summary>
        [DataMember]
        public string A5 { get; set; }

        /// <summary>
        /// Propiedad A6
        /// </summary>
        [DataMember]
        public string A6 { get; set; }

        /// <summary>
        /// Propiedad A7
        /// </summary>
        [DataMember]
        public string A7 { get; set; }

        /// <summary>
        /// Propiedad A8
        /// </summary>
        [DataMember]
        public string A8 { get; set; }

        /// <summary>
        /// Propiedad A9
        /// </summary>
        [DataMember]
        public string A9 { get; set; }

        /// <summary>
        /// Propiedad A10
        /// </summary>
        [DataMember]
        public string A10 { get; set; }

        /// <summary>
        /// Propiedad A11
        /// </summary>
        [DataMember]
        public string A11 { get; set; }

        /// <summary>
        /// Propiedad A12
        /// </summary>
        [DataMember]
        public string A12 { get; set; }

        /// <summary>
        /// Propiedad A13
        /// </summary>
        [DataMember]
        public string A13 { get; set; }

        /// <summary>
        /// Propiedad A14
        /// </summary>
        [DataMember]
        public string A14 { get; set; }

        /// <summary>
        /// Propiedad A15
        /// </summary>
        [DataMember]
        public string A15 { get; set; }

        /// <summary>
        /// Propiedad A16
        /// </summary>
        [DataMember]
        public string A16 { get; set; }

        /// <summary>
        /// Propiedad A17
        /// </summary>
        [DataMember]
        public string A17 { get; set; }

        /// <summary>
        /// Propiedad A18
        /// </summary>
        [DataMember]
        public string A18 { get; set; }

        /// <summary>
        /// Propiedad A19
        /// </summary>
        [DataMember]
        public string A19 { get; set; }

        /// <summary>
        /// Propiedad A20
        /// </summary>
        [DataMember]
        public string A20 { get; set; }

        /// <summary>
        /// Propiedad A21
        /// </summary>
        [DataMember]
        public string A21 { get; set; }

        /// <summary>
        /// Propiedad A22
        /// </summary>
        [DataMember]
        public string A22 { get; set; }

        /// <summary>
        /// Propiedad A23
        /// </summary>
        [DataMember]
        public string A23 { get; set; }

        /// <summary>
        /// Propiedad A24
        /// </summary>
        [DataMember]
        public string A24 { get; set; }

        /// <summary>
        /// Propiedad A25
        /// </summary>
        [DataMember]
        public string A25 { get; set; }

        /// <summary>
        /// Respuesta del servicio
        /// </summary>
        [DataMember]
        public string Request { get; set; }

        /// <summary>
        /// Is Score
        /// </summary>
        [DataMember]
        public bool IsScore { get; set; }
    }
}
