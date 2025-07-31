using System.Runtime.Serialization;
namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class CompanyInsuredPremiumFinance
    {



        /// <summary>
        ///individual Id
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }


        /// <summary>
        ///Codigo del Asegurado
        /// </summary>
        [DataMember]
        public int InsuredCode { get; set; }



        /// <summary>
        /// Nombre Completo asegurado
        /// </summary>
        [DataMember]
        public string FullName { get; set; }
    }
}
