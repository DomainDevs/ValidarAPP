namespace Sistran.Core.Application.CommonService.Enums
{
    /// <summary>
    /// Id de valores de la tabla COMM.Parameter
    /// </summary>
    public enum ParametersTypes
    {
        /// <summary>
        /// Parametro de coverturas para tasa de accesorios - TASA ACCESORIOS
        /// </summary>
        RateAccessories = 2201,
        /// <summary>
        /// Parametro de cobertura - PORCENTAJE PROTEC PATRIMONIAL
        /// </summary>
        CoverageHeritage = 2188,
        /// <summary>
        /// Parametro de Terceros afectados - Para RC y rc pasajeros 
        /// </summary>
        ThirdPartiesAffected = 10003,
        /// <summary>
        /// Parametro de Paises 
        /// </summary>
        Country = 10005,
        /// <summary>
        /// Parametro de Moneda 
        /// </summary>
        Currency = 10006,
        /// <summary>
        /// Parametro de Tipo de Persona por Defecto 
        /// </summary>
        PersonType = 10013,
        /// <summary>
        /// Parametro de Tipo de Persona por Defecto 
        /// </summary>
        FutureSociety = 1009,
        /// <summary>
        /// Parametro de cancelacion corto plazo  
        /// </summary>
        ShortTermCancellation = 10019,
        /// <summary>
        /// Parámetro de concepto de la prima mínima
        /// </summary>
        MinimumPremiumConcept = 10020,
        /// <summary>
        /// Parámetro de concepto de la prima mínima a Prorrata
        /// </summary>
        ProrateConcept = 10021,

        /// <summary>
        /// Tiempo que la contraseña del usuario va a durar bloqueada
        /// </summary>
        TimeUserBlocked = 2151,
        /// <summary>
        /// Cantidad de intentos para mostrar advertencia
        /// </summary>
        WarningAttempts = 2152,
        /// <summary>
        /// Cantidad de intentos para que se bloquee la contraseña
        /// </summary>
        MaxAttempts = 2153,
        /// <summary>
        /// Secuencias no permitidas en la contraseña
        /// </summary>
        NotAllowedSecuences = 2190,
        /// <summary>
        /// Porcentaje de coincidencia de la contraseña
        /// </summary>
        KeyCoincidencePercentage = 2191,
        /// <summary>
        /// Cantidad de contraseñas anteriores a validar
        /// </summary>
        MaxRecordsHistory = 2192,
        /// <summary>
        /// Longitud mínima de la contraseña
        /// </summary>
        MinKeyLeng = 2212,
        /// <summary>
        /// Días en que alerta que se le vencerá la contraseña, previos a la expiración de la contraseña
        /// </summary>
        DaysBeforePasswordExpires = 2225,
        /// <summary>
        /// Días en que expirará la contraseña
        /// </summary>
        DAYS_EXPIRATION_PASSWORD = 2223,
        /// <summary>
        /// Cantidad mínima de números que debe tener la contraseña
        /// </summary>
        MinNumbersAmount = 10014,
        /// <summary>
        /// Cantidad mínima de minúsculas que debe tener la contraseña
        /// </summary>
        MinLowerAmount = 10015,
        /// <summary>
        /// Cantidad mínima de mayúsculas que debe tener la contraseña
        /// </summary>
        MinUpperAmount = 10016,
        /// <summary>
        /// Cantidad mínima de caracteres especiales que debe tener la contraseña
        /// </summary>
        MinSpecialsAmount = 10017,
        /// <summary>
        /// Cantidad de caracteres que se verificarán en la secuencia
        /// </summary>
        CharactersToValidateInSecuence = 10018,
        /// <summary>
        /// Ramo de la poliza correlativa de ubicación
        /// </summary>
        LiabilityCorrelativePrefixCode = 10022,
        /// <summary>
        /// Incluye asistencia para prima minima
        /// </summary>
        IncludeAssistanceInPremiumMin = 2010,
        /// <summary>
        /// Campos que permiten Id 0
        /// </summary>
        FieldAllowZero = 10024,
        /// <summary>
        /// Cantidad de dias de validez
        /// </summary>
        DaysValidity = 1027,
         /// <summary>
         /// Deducible dinámico
         /// </summary>
        DeductibleDynamic = 522,
        /// <summary>
        /// Concepto que establece si se está usando R2
        /// </summary>
        ConceptAppSourceR2 = 10034,
        /// <summary>
        /// Pais por defecto a cargar
        /// </summary>
        EmisionDefaultCountry = 12155
    }
}
