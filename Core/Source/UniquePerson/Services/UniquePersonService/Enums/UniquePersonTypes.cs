using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Enums
{
    /// <summary>
    /// Zonas
    /// </summary>
    [Flags]
    public enum AddressZoneType
    {
        /// <summary>
        /// Urbano
        /// </summary>
        [EnumMember]
        Urban = 1,
        /// <summary>
        /// Rural
        /// </summary>
        [EnumMember]
        Rural = 2
    }

    /// <summary>
    /// Tipo Cliente
    /// </summary>
    [Flags]
    public enum CustomerType
    {
        /// <summary>
        /// Constante Individual
        /// </summary>
        [EnumMember]
        Individual = 1,
        /// <summary>
        /// Constante Prospecto
        /// </summary>
        [EnumMember]
        Prospect = 2
    }

    /// <summary>
    /// Genero
    /// </summary>
    [Flags]
    public enum GenderType
    {
        /// <summary>
        /// Masculino
        /// </summary>
        [EnumMember]
        Male = 1,
        /// <summary>
        /// Femenino
        /// </summary>
        [EnumMember]
        Female = 2

    }

    /// <summary>
    /// Tipos de Contragarantia
    /// </summary>
    [Flags]
    public enum GuaranteeType
    {
        /// <summary>
        /// Pagare
        /// </summary>
        [EnumMember]
        PromissoryNote = 1,
        /// <summary>
        /// Cdt
        /// </summary>
        [EnumMember]
        Cdt = 2,
        /// <summary>
        /// Banco
        /// </summary>
        [EnumMember]
        AcceptingBank = 3,
        /// <summary>
        /// Hipoteca
        /// </summary>
        [EnumMember]
        Mortgage = 4,
        /// <summary>
        /// Promesa de pago
        /// </summary>
        [EnumMember]
        PromiseToPay = 5,
        /// <summary>
        /// Promesa 
        /// </summary>
        [EnumMember]
        Pledge = 6,
        /// <summary>
        /// Otros 
        /// </summary>
        [EnumMember]
        Other = 7,
        /// <summary>
        /// Exonerado 
        /// </summary>
        [EnumMember]
        Exonerated = 8
    }

    /// <summary>
    /// Tipos de Riesgo Fuerte
    /// </summary>
    [Flags]
    public enum HardRiskTypes
    {
        /// <summary>
        /// Auto 
        /// </summary>
        [EnumMember]
        Auto = 1,
        /// <summary>
        /// Fianza 
        /// </summary>
        [EnumMember]
        Bail = 7
    }

    /// <summary>
    /// Busqueda por tipo de Individuo
    /// </summary>
    [Flags]
    public enum IndividualSearchType
    {
        /// <summary>
        /// Persona 
        /// </summary>
        [EnumMember]
        Person = 1,
        /// <summary>
        /// Compañia 
        /// </summary>
        [EnumMember]
        Company = 2,
        /// <summary>
        /// Persona Prospecto 
        /// </summary>
        [EnumMember]
        ProspectusPerson = 3,
        /// <summary>
        /// Compañia Prospecto 
        /// </summary>
        [EnumMember]
        ProspectusCompany = 4,
        /// <summary>
        /// Todos 
        /// </summary>
        [EnumMember]
        All = 5
    }

    /// <summary>
    /// Tipos de Individuo
    /// </summary>
    [Flags]
    public enum IndividualType
    {
        /// <summary>
        /// Persona 
        /// </summary>
        [EnumMember]
        Person = 1,
        /// <summary>
        /// Compañia 
        /// </summary>
        [EnumMember]
        LegalPerson = 2,
        /// <summary>
        /// Prospecto Natural 
        /// </summary>
        [EnumMember]
        Naturalprospectus = 3,
        /// <summary>
        /// Prospecto Compañia  
        /// </summary>
        [EnumMember]
        Legalprospectus = 4
    }

    /// <summary>
    /// Tipo de Busqueda Asegurado
    /// </summary>
    [Flags]
    public enum InsuredSearchType
    {
        /// <summary>
        /// Numero documento
        /// </summary>
        [EnumMember]
        DocumentNumber = 1,
        /// <summary>
        /// Individuo
        /// </summary>
        [EnumMember]
        IndividualId = 2

    }

    /// <summary>
    /// Datos de la Tabla Parametrica COMM.PARAMETER
    /// </summary>
    public enum ParameterType
    {
        /// <summary>
        /// Tomar Consecutivo del Asegurado
        /// </summary>
        Insured = 20,
        /// <summary>
        /// Tomar Consecutivo de la Contragarantia
        /// </summary>
        Guarantee = 2149,
        /// <summary>
        /// Tomar Consecutivo del Contragarantia Nota
        /// </summary>
        GuaranteeNote = 2210
    }
    /// <summary>
    /// Tipo de Pago
    /// </summary>
    public enum PaymentMethodType
    {

        /// <summary>
        /// Efectivo
        /// </summary>
        Cash = 1
    }
    /// <summary>
    /// Roles de la Persona o Compañia
    /// </summary>
    [Flags]
    public enum RolesType
    {
        /// <summary>
        /// Asegurado
        /// </summary>
        [EnumMember]
        Insured = 1,
        /// <summary>
        /// Intermediario
        /// </summary>
        [EnumMember]
        Intermediary = 2,
        /// <summary>
        /// Empleado
        /// </summary>
        [EnumMember]
        Employed = 3,
        /// <summary>
        /// Beneficiario
        /// </summary>
        [EnumMember]
        Beneficiary = 4

    }
    /// <summary>
    /// Tipos de Direccion
    /// </summary>
    [DataContract]
    public enum AddressTypes
    {
        /// <summary>
        /// Casa
        /// </summary>
        [EnumMember]
        Home = 1,
        /// <summary>
        /// Oficina
        /// </summary>
        [EnumMember]
        OfficeCommercial = 2,
        /// <summary>
        /// otros
        /// </summary>
        [EnumMember]
        Other = 7,
        /// <summary>
        /// Centro Comercial
        /// </summary>
        [EnumMember]
        CenterCommercial = 9,
        /// <summary>
        /// Edificio
        /// </summary>
        [EnumMember]
        Edifice = 10,
        /// <summary>
        /// Condominio
        /// </summary>
        [EnumMember]
        Condominium = 11,
        /// <summary>
        /// Banco
        /// </summary>
        [EnumMember]
        BancasSurance = 12,
        /// <summary>
        /// Correo
        /// </summary>
        [EnumMember]
        Email = 13
    }

    /// <summary>
    /// Tipo de Asociacion
    /// </summary>
    public enum AssociationTypes
    {
        /// <summary>
        /// Individual
        /// </summary>
        Individual = 1,
        /// <summary>
        /// Consorcio
        /// </summary>
        Consortium = 2,
        /// <summary>
        /// Union Temporal
        /// </summary>
        TemporaryUnion = 3,
        /// <summary>
        /// Compañia Futura
        /// </summary>
        FutureCompany = 4

    }


}
