using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.CommonServices.Enums
{
    /// <summary>
    /// Id de valores de la tabla COMM.Parameter
    /// </summary>
    [Flags]
    public enum CompanyParameterType
    {
        /// <summary>
        /// Parametro de asistencia para hogar - ASISTENCIA HOGAR
        /// </summary>
        [EnumMember]
        AssistanceProperty = 1013,
        /// <summary>
        /// Parametro de asistencia para hogar - ASISTENCIA RC
        /// </summary>
        [EnumMember]
        AssistanceLiability = 0,

        /// <summary>
        /// Parametro de sin carroceria para autos - CARROCERIA VEHICULO
        /// </summary>
        [EnumMember]
        WithOutBodyVehicle = 10023,

        /// <summary>
        /// Parametro Cantidad Memor de Años de construcción - Validación Construcción
        /// </summary>
        [EnumMember]
        MinYearAllowed = 2011,

        /// <summary>
        /// Parametro Cantidad Mayor de número de Pisos - Validación Construcción
        /// </summary>
        [EnumMember]
        MaxFloorNumber = 2012,

        /// <summary>
        /// Parametro Cantidad Menor de número de Pisos - Validación Construcción
        /// </summary>
        [EnumMember]
        MinFloorNumber = 2013,

        /// <summary>
        /// Parametro validacion de celular
        /// </summary>
        [EnumMember]
        CellPhone = 2014,

        /// <summary>
        /// Parametro de asistencia para autos - ASISTENCIA EN VIAJES
        /// </summary>
        [EnumMember]
        AssistanceVehicle= 1014,

        /// <summary>
        /// Parametro de Años de buena expreriencia - AUTOS
        /// </summary>
        [EnumMember]
        GoodExpNumPrinter = 1015,
		
			/// <summary>
        /// Parametro Vehiculo de Reemplazo - AUTOS
        /// </summary>
        [EnumMember]
        VehicleReplacement = 10028,

        /// <summary>
        /// Cantidad máxima de participantes para coaseguro aceptado
        /// </summary>
        [EnumMember]
        MaxAgentCoinsuranceAccepted = 8807,

        /// <summary>
        /// Cantidad máxima de dias permitidos para Fecha de vigencia de la poliza
        /// </summary>
        [EnumMember]
        MaxDaysRenewPolicy = 1050

    }
}
