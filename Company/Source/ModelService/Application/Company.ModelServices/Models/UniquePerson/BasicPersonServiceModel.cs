// -----------------------------------------------------------------------
// <copyright file="PersonBasicServiceModel.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>David S. Niño T.</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.ModelServices.Models
{
    using Sistran.Core.Application.ModelServices.Models.Param;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Datos Basicos de Persona
    /// </summary>
    [DataContract]
    public class BasicPersonServiceModel 
    {
        /// <summary>
        /// Obtiene o establece el Id de la Persona
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo de Documento
        /// </summary>   
        [DataMember]
        public int DocumentType { get; set; }


        /// <summary>
        /// Obtiene o establece el Numero de Documento
        /// </summary>   
        [DataMember]
        public string DocumentNumber { get; set; }


        /// <summary>
        /// Obtiene o establece el Código de Persona
        /// </summary>   
        [DataMember]
        public int PersonCode { get; set; }

        /// <summary>
        /// Obtiene o establece el Primer Apellido
        /// </summary>   
        [DataMember]
        public string FirstName { get; set; }

        /// <summary>
        /// Obtiene o establece el Segundo Apellido
        /// </summary>   
        [DataMember]
        public string LastName { get; set; }

        /// <summary>
        /// Obtiene o establece el Nombre
        /// </summary>   
        [DataMember]
        public string Name { get; set; }


        /// <summary>
        /// Obtiene o establece el Genero
        /// </summary>   
        [DataMember]
        public string Gender { get; set; }

        /// <summary>
        /// Obtiene o establece el Estado Civil
        /// </summary>   
        [DataMember]
        public int MaritalStatus { get; set; }

        /// <summary>
        /// Obtiene o establece la Fecha de Nacimiento
        /// </summary>   
        [DataMember]
        public DateTime Birthdate { get; set; }

        /// <summary>
        /// Obtiene o establece Lugar de Nacimiento
        /// </summary>   
        [DataMember]
        public string BirthPlace { get; set; }

        /// <summary>
        /// Obtiene o establece la Fecha última actualización
        /// </summary>           
        [DataMember]
        public DateTime? LastUpdate { get; set; }

        /// <summary>
        /// Obtiene o establece Lugar de Nacimiento
        /// </summary>   
        [DataMember]
        public string UpdateBy { get; set; }

        /// <summary>
        /// Obtiene o establece si es tomador de una poliza
        /// </summary>   
        [DataMember]
        public bool Policy { get; set; }

        /// <summary>
        /// Obtiene o establece si es asegurado de una poliza
        /// </summary>   
        [DataMember]
        public bool Insured { get; set; }

        /// <summary>
        /// Obtiene o establece si es beneficiario de una poliza
        /// </summary>   
        [DataMember]
        public bool Beneficiary { get; set; }
    }
}
