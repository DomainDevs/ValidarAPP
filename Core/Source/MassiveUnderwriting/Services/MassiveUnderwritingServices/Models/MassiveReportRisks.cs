using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.MassiveUnderwritingServices.Models
{
    /// <summary>
    /// Informacion de los asegurados, beneficiarios y riesgos
    /// </summary>
    [DataContract]
    public class MassiveReportRisks
    {
        /// <summary>
        /// Obtiene o establece el ID de riesgo
        /// </summary>
        [DataMember]
        public int RiskId { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo de individuo asegurado
        /// </summary>
        [DataMember]
        public string InsuredIndividualType { get; set; }

        /// <summary>
        /// Obtiene o establece el Primer apellido asegurado
        /// </summary>
        [DataMember]
        public string InsuredSurname { get; set; }

        /// <summary>
        /// Obtiene o establece el Segundo apellido asegurado
        /// </summary>
        [DataMember]
        public string InsuredMotherLastName { get; set; }

        /// <summary>
        /// Obtiene o establece el Nombre asegurado
        /// </summary>
        [DataMember]
        public string InsuredName { get; set; }

        /// <summary>
        /// Obtiene o establece el Genero asegurado
        /// </summary>
        [DataMember]
        public string InsuredGender { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo de documento asegurado
        /// </summary>
        [DataMember]
        public string InsuredIdCardType { get; set; }

        /// <summary>
        /// Obtiene o establece el Documento asegurado
        /// </summary>
        [DataMember]
        public string InsuredIdCard { get; set; }

        /// <summary>
        /// Obtiene o establece el Estado civil asegurado
        /// </summary>
        [DataMember]
        public string InsuredMaritalStatus { get; set; }

        /// <summary>
        /// Obtiene o establece la Fecha de nacimiento asegurado
        /// </summary>
        [DataMember]
        public string InsuredBirthDate { get; set; }

        /// <summary>
        /// Obtiene o establece la Ocupacion asegurado
        /// </summary>
        [DataMember]
        public string InsuredOccupationType { get; set; }

        /// <summary>
        /// Obtiene o establece la Razon social asegurado
        /// </summary>
        [DataMember]
        public string InsuredTradeName { get; set; }

        /// <summary>
        /// Obtiene o establece el ID de tipo tributario asegurado
        /// </summary>
        [DataMember]
        public string InsuredTributaryIdType { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo tributario asegurado
        /// </summary>
        [DataMember]
        public string InsuredTributaryId { get; set; }

        /// <summary>
        /// Obtiene o establece el Pais empresa asegurado
        /// </summary>
        [DataMember]
        public string InsuredCompanyCountry { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo de empresa asegurado
        /// </summary>
        [DataMember]
        public string InsuredCompanyType { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo de direccion asegurado
        /// </summary>
        [DataMember]
        public string InsuredAddressType { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo de direccion asegurado
        /// </summary>
        [DataMember]
        public string InsuredAddressStreetType { get; set; }

        /// <summary>
        /// Obtiene o establece la Direccion asegurado
        /// </summary>
        [DataMember]
        public string InsuredAddressStreet { get; set; }

        /// <summary>
        /// Obtiene o establece la Ciudad asegurado
        /// </summary>
        [DataMember]
        public string InsuredAddressCity { get; set; }

        /// <summary>
        /// Obtiene o establece el Pais asegurado
        /// </summary>
        [DataMember]
        public string InsuredAddressCountry { get; set; }

        /// <summary>
        /// Obtiene o establece el Departamento asegurado
        /// </summary>
        [DataMember]
        public string InsuredAddressState { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo de telefono asegurado
        /// </summary>
        [DataMember]
        public string InsuredPhoneType { get; set; }

        /// <summary>
        /// Obtiene o establece el Telefono asegurado
        /// </summary>
        [DataMember]
        public string InsuredPhone { get; set; }

        /// <summary>
        /// Obtiene o establece el telefono adicional del asegurado
        /// </summary>
        [DataMember]
        public string AdditionalInsured { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo de individuo beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryIndividualType { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo de beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryType { get; set; }

        /// <summary>
        /// Obtiene o establece el Primer apellido beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiarySurname { get; set; }

        /// <summary>
        /// Obtiene o establece el Segundo apellido beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryMotherLastName { get; set; }

        /// <summary>
        /// Obtiene o establece el Nombre beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryName { get; set; }

        /// <summary>
        /// Obtiene o establece el Genero del beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryGender { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo de documento beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryIdCardType { get; set; }

        /// <summary>
        /// Obtiene o establece el Documento del beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryIdCard { get; set; }

        /// <summary>
        /// Obtiene o establece el Estado civil del beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryMaritalStatus { get; set; }

        /// <summary>
        /// Obtiene o establece la Fecha de nacimiento beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryBirthDate { get; set; }

        /// <summary>
        /// Obtiene o establece la Ocupacion del beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryOccupationType { get; set; }

        /// <summary>
        /// Obtiene o establece la Razon social del beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryTradeName { get; set; }

        /// <summary>
        /// Obtiene o establece el ID tipo tributario del beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryTributaryIdType { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo tributario del beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryTributaryId { get; set; }

        /// <summary>
        /// Obtiene o establece el Pais empresa beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryCompanyCountry { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo de empresa beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryCompanyType { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo de direccion beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryAddressType { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo de calle beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryAddressStreetType { get; set; }

        /// <summary>
        /// Obtiene o establece la Direccion beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryAddressStreet { get; set; }

        /// <summary>
        /// Obtiene o establece el Ciudad beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryAddressCity { get; set; }

        /// <summary>
        /// Obtiene o establece el Pais beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryAddressCountry { get; set; }

        /// <summary>
        /// Obtiene o establece el Departamento beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryAddressState { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo de telefono beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryPhoneType { get; set; }

        /// <summary>
        /// Obtiene o establece el Telefono beneficiario
        /// </summary>
        [DataMember]
        public string BeneficiaryPhone { get; set; }

        /// <summary>
        /// Obtiene o establece el identificación de covertura del grupo
        /// </summary>
        [DataMember]
        public string CoverGroupId { get; set; }

        /// <summary>
        /// Obtiene o establece el Calculo de prima minima
        /// </summary>
        [DataMember]
        public string CalculateMinPremium { get; set; }

        /// <summary>
        /// Obtiene o establece el CurrentFrom Fecha DESDE
        /// </summary>
        [DataMember]
        public string CurrentFrom { get; set; }

        /// <summary>
        /// Obtiene o establece el CurrentTo Fecha HASTA
        /// </summary>
        [DataMember]
        public string CurrentTo { get; set; }


        /// <summary>
        /// Obtiene o establece la Zona de tarificacion
        /// </summary>
        [DataMember]
        public string RatingZone { get; set; }

        /// <summary>
        /// Obtiene o establece el Limite RC
        /// </summary>
        [DataMember]
        public string LimitsRC { get; set; }

        /// <summary>
        /// Obtiene o establece el Incluye covertura
        /// </summary>
        [DataMember]
        public string IncludesCoverage { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo de asistencia
        /// </summary>
        [DataMember]
        public string Assitance { get; set; }

        /// <summary>
        /// Obtiene o establece el Codigo fasecolda
        /// </summary>
        [DataMember]
        public string Fasecolda { get; set; }

        /// <summary>
        /// Obtiene o establece la Marca vehiculo
        /// </summary>
        [DataMember]
        public string VehicleMake { get; set; }

        /// <summary>
        /// Obtiene o establece el Año vehiculo
        /// </summary>
        [DataMember]
        public string VehicleYear { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo vehiculo
        /// </summary>
        [DataMember]
        public string VehicleType { get; set; }

        /// <summary>
        /// Obtiene o establece el Uso vehiculo
        /// </summary>
        [DataMember]
        public string VehicleUse { get; set; }

        /// <summary>
        /// Obtiene o establece la Carroceria vehiculo
        /// </summary>
        [DataMember]
        public string VehicleBody { get; set; }

        /// <summary>
        /// Obtiene o establece el Precio vehiculo
        /// </summary>
        [DataMember]
        public string VehiclePrice { get; set; }

        /// <summary>
        /// Obtiene o establece el Vehiculo nuevo
        /// </summary>
        [DataMember]
        public string VehicleNew { get; set; }

        /// <summary>
        /// Obtiene o establece la Placa vehiculo
        /// </summary>
        [DataMember]
        public string VehicleLicensePlate { get; set; }

        /// <summary>
        /// Obtiene o establece el Serial motor vehiculo
        /// </summary>
        [DataMember]
        public string VehicleEngineSerial { get; set; }

        /// <summary>
        /// Obtiene o establece el Serial chasis vehiculo
        /// </summary>
        [DataMember]
        public string VehicleChassisSerial { get; set; }

        /// <summary>
        /// Obtiene o establece el Color vehiculo
        /// </summary>
        [DataMember]
        public string VehicleColor { get; set; }

        /// <summary>
        /// Obtiene o establece los Pasajeros vehiculo
        /// </summary>
        [DataMember]
        public string VehiclePassengers { get; set; }

        /// <summary>
        /// Obtiene o establece el Trailers vehiculo
        /// </summary>
        [DataMember]
        public string VehicleTrailers { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo de carga vehiculo
        /// </summary>
        [DataMember]
        public string VehicleLoadType { get; set; }

        /// <summary>
        /// Obtiene o establece el Combustible vehiculo
        /// </summary>
        [DataMember]
        public string VehicleFuel { get; set; }

        /// <summary>
        /// Obtiene o establece el Años sin reclamacion
        /// </summary>
        [DataMember]
        public string GoodExperience { get; set; }

        /// <summary>
        /// Obtiene o establece el Tipo de trabajador
        /// </summary>
        [DataMember]
        public string WorkerType { get; set; }

        /// <summary>
        /// Obtiene o establece la Fecha de nacimiento hijo
        /// </summary>
        [DataMember]
        public string SonBirthDate { get; set; }

        /// <summary>
        /// Obtiene o establece la tarifa plana PCT.
        /// </summary>
        [DataMember]
        public string FlatRatePCT { get; set; }

        /// <summary>
        /// Obtiene o establece la facultad
        /// </summary>
        [DataMember]
        public string Facultative { get; set; }

        /// <summary>
        /// Obtiene o establece la fecha desde actual
        /// </summary>
        [DataMember]
        public string DateCurrentFrom { get; set; }

        /// <summary>
        /// Obtiene o establece la Lista de accesorios
        /// </summary>
        [DataMember]
        public List<MassiveReportAccessories> Accesories { get; set; }

        /// <summary>
        /// Tipo de reportes
        /// </summary>
        [DataMember]
        public int ReportType { get; set; }

        /// <summary>
        /// Detalle de error por riesgo.
        /// </summary>
        [DataMember]
        public string DetailLogError { get; set; }

        /// <summary>
        /// Description del cargue.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Codigo del trayecto.
        /// </summary>
        [DataMember]
        public string ShuttleCd { get; set; }

        /// <summary>
        /// Codigo de deducible.
        /// </summary>
        [DataMember]
        public string DeductId { get; set; }

        /// <summary>
        /// Codigo de tipo de servicio.
        /// </summary>
        [DataMember]
        public string ServiceTypeCd { get; set; }

        /// <summary>
        /// Asegurado adicionales.
        /// </summary>
        [DataMember]
        public string ExtraInsured { get; set; }

        /// <summary>
        /// Tasa unica.
        /// </summary>
        [DataMember]
        public string UniqueRate { get; set; }

        /// <summary>
        /// Prima.
        /// </summary>
        [DataMember]
        public string Premium { get; set; }

        [DataMember]
        public string VehicleVersion { get; set; }
        [DataMember]
        public string VehicleModel { get; set; }


        ///Se agregan campos para consulta de excel de autos y rc con y sin agrupadora

        /// <summary>
        /// Obtiene o establece los intermediarios
        /// </summary>
        [DataMember]
        public List<MassiveReportAgents> Agents { get; set; }

        [DataMember]
        public string AllianceBranch { get; set; }
        [DataMember]
        public string AllianceCode { get; set; }
        [DataMember]
        public string AllianceSalesPoint { get; set; }
        [DataMember]
        public string BilingCode { get; set; }
        [DataMember]
        public string CoinsuranceCompany { get; set; }
        [DataMember]
        public string CoinsuranceCompanyPercentage { get; set; }
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public string CompanyPercentage { get; set; }
        [DataMember]
        public string Currency { get; set; }
        [DataMember]
        public string Deductible { get; set; }
        [DataMember]
        public string EndorsementNumber { get; set; }
        [DataMember]
        public string ErrorMessage { get; set; }
        [DataMember]
        public string ExchangeRate { get; set; }
        [DataMember]
        public string Expenses { get; set; }
        [DataMember]
        public string ExternalPolicyNumber { get; set; }
        [DataMember]
        public string HolderAccountNumber { get; set; }
        [DataMember]
        public string HolderAccountType { get; set; }
        [DataMember]
        public string HolderAddressCity { get; set; }
        [DataMember]
        public string HolderAddressCountry { get; set; }
        [DataMember]
        public string HolderAddressState { get; set; }
        [DataMember]
        public string HolderAddressStreet { get; set; }
        [DataMember]
        public string HolderAddressStreetType { get; set; }
        [DataMember]
        public string HolderAddressType { get; set; }
        [DataMember]
        public string HolderBank { get; set; }
        [DataMember]
        public string HolderBirthDate { get; set; }
        [DataMember]
        public string HolderBranch { get; set; }
        [DataMember]
        public string HolderCardNumber { get; set; }
        [DataMember]
        public string HolderCompanyCountry { get; set; }
        [DataMember]
        public string HolderCompanyType { get; set; }
        [DataMember]
        public string HolderCurrency { get; set; }
        [DataMember]
        public string HolderCurrentFrom { get; set; }
        [DataMember]
        public string HolderCurrentTo { get; set; }
        [DataMember]
        public string HolderElectronicAccount { get; set; }
        [DataMember]
        public string HolderGender { get; set; }
        [DataMember]
        public string HolderIdCard { get; set; }
        [DataMember]
        public string HolderIdCardType { get; set; }
        [DataMember]
        public string HolderIndividualType { get; set; }
        [DataMember]
        public string HolderMaritalStatus { get; set; }
        [DataMember]
        public string HolderMotherLastName { get; set; }
        [DataMember]
        public string HolderName { get; set; }
        [DataMember]
        public string HolderOccupationType { get; set; }
        [DataMember]
        public string HolderPaymentMethod { get; set; }
        [DataMember]
        public string HolderPaymentSchedule { get; set; }
        [DataMember]
        public string HolderPhone { get; set; }
        [DataMember]
        public string HolderPhoneType { get; set; }
        [DataMember]
        public string HolderSecurityNumber { get; set; }
        [DataMember]
        public string HolderSurname { get; set; }
        [DataMember]
        public string HolderTradeName { get; set; }
        [DataMember]
        public string HolderTributaryId { get; set; }
        [DataMember]
        public string HolderTributaryIdType { get; set; }
        [DataMember]
        public string PercentageCommission { get; set; }
        [DataMember]
        public string PolicyNumber { get; set; }
        [DataMember]
        public string PolicyOperationType { get; set; }
        [DataMember]
        public string PolicyType { get; set; }
        [DataMember]
        public string Product { get; set; }
        [DataMember]
        public string RateType { get; set; }
        [DataMember]
        public string ServiceType { get; set; }
        [DataMember]
        public string Shuttle { get; set; }
        [DataMember]
        public string Sinister { get; set; }
        [DataMember]
        public string UsePercentageCommissionProduct { get; set; }

        [DataMember]
        public string IncludesAssitanceCoverage { get; set; }

        [DataMember]
        public string InsuredValue { get; set; }
        
        [DataMember]
        public string Rate { get; set; }

        [DataMember]
        public string Commission { get; set; }

        [DataMember]
        public string TempId { get; set; }

        [DataMember]
        public string MassiveId { get; set; }

        [DataMember]
        public string PolicyBranch { get; set; }

        [DataMember]
        public string PolicyPrefix { get; set; }

        [DataMember]
        public string PolicyId { get; set; }
    }
}
