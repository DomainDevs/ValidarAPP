using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    /// <summary>
    /// Coasegurador
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.UniquePersonService.V1.Models.CoInsurerCompany" />
    [DataContract]
    public class CoInsurerCompany : Sistran.Core.Application.UniquePersonService.V1.Models.CoInsurerCompany
    {
        [DataMember]
        public decimal InsuranceCompanyId { get; set; }
        /// <summary>
        /// Atributo para la propiedad Description.
        /// </sumary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Atributo para la propiedad AddressTypeCode.
        /// </sumary>
        [DataMember]
        public int AddressTypeCode { get; set; }
        /// <summary>
        /// Atributo para la propiedad Street.
        /// </sumary>
        [DataMember]
        public string Street { get; set; }

        /// <summary>
        /// Atributo para la propiedad PostalCode.
        /// </sumary>
        [DataMember]
        public decimal? PostalCode { get; set; }

        /// <summary>
        /// Atributo para la propiedad AreaCode.
        /// </sumary>
        [DataMember]
        public decimal? AreaCode { get; set; }

        /// <summary>
        /// Atributo para la propiedad ColonyCode.
        /// </sumary>
        [DataMember]
        public decimal? ColonyCode { get; set; }

        /// <summary>
        /// Atributo para la propiedad CityCode.
        /// </sumary>
        [DataMember]
        public int CityCode { get; set; }

        /// <summary>
        /// Atributo para la propiedad StateCode.
        /// </sumary>
        [DataMember]
        public int StateCode { get; set; }

        /// <summary>
        /// Atributo para la propiedad CountryCode.
        /// </sumary>
        [DataMember]
        public int CountryCode { get; set; }

        /// <summary>
        /// Atributo para la propiedad PhoneTypeCode.
        /// </sumary>
        [DataMember]
        public int PhoneTypeCode { get; set; }

        /// <summary>
        /// Atributo para la propiedad PhoneNumber.
        /// </sumary>
        [DataMember]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Atributo para la propiedad IvaTypeCode.
        /// </sumary>
        [DataMember]
        public decimal IvaTypeCode { get; set; }

        /// <summary>
        /// Atributo para la propiedad TributaryIdNo.
        /// </sumary>
        [DataMember]
        public string TributaryIdNo { get; set; }

        /// <summary>
        /// Atributo para la propiedad PilotingSpendAmount.
        /// </sumary>
        [DataMember]
        public decimal PilotingSpendAmount { get; set; }

        /// <summary>
        /// Atributo para la propiedad YearMinSignInQuantity.
        /// </sumary>
        [DataMember]
        public decimal? YearMinSignInQuantity { get; set; }

        /// <summary>
        /// Atributo para la propiedad YearMaxSignInQuantity.
        /// </sumary>
        [DataMember]
        public decimal? YearMaxSignInQuantity { get; set; }

        /// <summary>
        /// Atributo para la propiedad YearMaxLongQuantity.
        /// </sumary>
        [DataMember]
        public decimal? YearMaxLongQuantity { get; set; }

        /// <summary>
        /// Atributo para la propiedad SmallDescription.
        /// </sumary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Atributo para la propiedad EnsureInd.
        /// </sumary>
        [DataMember]
        public bool EnsureInd { get; set; }

        /// <summary>
        /// Atributo para la propiedad EnteredDate.
        /// </sumary>
        [DataMember]
        public DateTime? EnteredDate { get; set; }

        /// <summary>
        /// Atributo para la propiedad ModifyDate.
        /// </sumary>
        [DataMember]
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// Atributo para la propiedad DeclinedDate.
        /// </sumary>
        [DataMember]
        public DateTime? DeclinedDate { get; set; }

        /// <summary>
        /// Atributo para la propiedad ComDeclinedTypeCode.
        /// </sumary>
        [DataMember]
        public int? ComDeclinedTypeCode { get; set; }

        /// <summary>
        /// Atributo para la propiedad Annotations.
        /// </sumary>
        [DataMember]
        public string Annotations { get; set; }
    }
}
