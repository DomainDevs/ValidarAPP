// -----------------------------------------------------------------------
// <copyright file="ParamPersonBasic.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>David S. Niño T.</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.UniquePersonParamService.Models
{
    using Sistran.Core.Application.Utilities.Error;
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Datos Basicos de Persona
    /// </summary>
    public class ParamBasicPerson
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


        /// <summary>
        /// Objeto que obtiene la Información Basica de la Persona.
        /// </summary>
        /// <param name="ParamBasicPerson">Objeto ParamBasicPerson.</param>
        /// <returns>Retorna el modelo de negocio o un error.</returns>
        public static Result<ParamBasicPerson, ErrorModel> GetParameterBasicPerson(int IndividualId, int DocumentType, string DocumentNumber,
             int PersonCode, string FirstName, string LastName, string Name, string Gender, int MaritalStatus, DateTime Birthdate,
             string BirthPlace, DateTime? LastUpdate, string UpdateBy, bool Policy, bool Insured, bool Beneficiary)
        {
            return new ResultValue<ParamBasicPerson, ErrorModel>(new ParamBasicPerson()
            {

                IndividualId = IndividualId,
                DocumentType = DocumentType,
                DocumentNumber = DocumentNumber,
                PersonCode = PersonCode,
                FirstName = FirstName,
                LastName = LastName,
                Name = Name,
                Gender = Gender,
                MaritalStatus = MaritalStatus,
                Birthdate = Birthdate,
                BirthPlace = BirthPlace,
                LastUpdate = LastUpdate,
                UpdateBy = UpdateBy,
                Policy = Policy,
                Insured = Insured,
                Beneficiary = Beneficiary
            });
        }
    }
}
