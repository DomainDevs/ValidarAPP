using System;
using System.Runtime.Serialization;


namespace Sistran.Company.Application.UnderwritingServices.Enums
{

    [Flags]
    public enum Parameter
    {
        /// <summary>
        /// Días de validez
        /// </summary>
        [EnumMember]
        SubscriptionDays = 503,      
        Daysdiscountedays = 2169,  
        MaxPercentejeToFinanceParameter = 2202,
        PercentageToFinanceParameter = 2203,
        RateFinanceParameter = 2204,
        ParameterId = 73
    }   

    [Flags]
    public enum PrefixType
    {
        [EnumMember]
        Manejo = 1,
        Surety = 30,
        Incendio = 3,
        Sustraccion = 4,
        Transportes = 5,
        Responsabilidad = 15,
        Rotura = 81,
        CorrienteDebil = 83,
        Automoviles = 7        

    }


}
