using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.OperationQuotaCompanyServices.EEProvider.Enums
{
    [DataContract]
    [Flags]
    public enum EnumIndicatorConcept
    {
        [EnumMember]
         CAPITAL_TRABAJO= 10,

        [EnumMember]
        RAZON_CORRIENTE = 11,

        [EnumMember]
        PRUEBA_ACIDA = 12,

        [EnumMember]
        NIVEL_ENDEUDAMIENTO = 13,

        [EnumMember]
        ENDEUDAMIENTO_FINANCIERO = 14,

        [EnumMember]
        APALANCAMIENTO = 15,

        [EnumMember]
        PATRIMONIO_ROE = 16,

        [EnumMember]
        ACTIVO_ROA = 17,

        [EnumMember]
        EBITDA = 18

    }
    
    [DataContract]
    [Flags]
    public enum EnumUtilityDetails
    {
        [EnumMember]
        EFECTIVO_EQUIVALENTES = 1,

        [EnumMember]
        INVENTARIOS = 2,

        [EnumMember]
        CUENTAS_POR_COBRAR = 3,

        [EnumMember]
        ACTIVO_CORRIENTE = 4,

        [EnumMember]
        PROPIEDAD_PLATA_EQUIPO = 5,

        [EnumMember]
        ACTIVO_NO_CORRIENTE = 6,

        [EnumMember]
        TOTAL_ACTIVO = 7,

        [EnumMember]
        OBLIGACIONES_CORTO_PLAZO = 8,

        [EnumMember]
        PROVEEDORES = 9,

        [EnumMember]
        PASIVO_CORRIENTE = 10,

        [EnumMember]
        OBLIGACIONES_LARGO_PLAZO = 11,

        [EnumMember]
        PASIVO_NO_CORRIENTE = 12,

        [EnumMember]
        TOTAL_PASIVO = 13,

        [EnumMember]
        CAPITAL_SOCIAL = 14,

        [EnumMember]
        RESERVAS_CAPITAL = 15,

        [EnumMember]
        RESULTADO_EJERCICIOS_ANTERIORES = 16,

        [EnumMember]
        TOTAL_PATRIMONIO = 17,

        [EnumMember]
        INGRESOS_OPERACIONALES = 18,

        [EnumMember]
        COSTOS = 19,

        [EnumMember]
        UTILIDAD_BRUTA = 20,

        [EnumMember]
        COSTOS_GASTOS_ADMINISTRACION = 21,

        [EnumMember]
        UTILIDAD_OPERACIONAL = 22,

        [EnumMember]
        INGRESOS_GASTOS_NO_OPERACIONALES = 23,

        [EnumMember]
        UTILIDAD_ANTES_IMPUESTOS = 24,

        [EnumMember]
        PROVISION_IMPUESTOS = 25,

       [EnumMember]
        UTILIDAD_NETA = 26

    }

    [DataContract]
    [Flags]
    public enum EnumAutomaticOperationType
    {
        [EnumMember]
        General = 1,

        [EnumMember]
        Terceros = 2,

        [EnumMember]
        Utilidades = 3

    }

    [DataContract]
    [Flags]
    public enum EnumIndicatorType
    {
        [EnumMember]
        Liquidez = 1,

        [EnumMember]
        Endeudamiento = 2,

        [EnumMember]
        Rentabilidad = 3,

        [EnumMember]
        Actividad = 4
    }
}