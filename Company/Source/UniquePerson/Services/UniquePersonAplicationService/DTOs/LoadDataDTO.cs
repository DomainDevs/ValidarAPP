using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonAplicationServices.DTOs
{
    [DataContract]
    public class LoadDataDTO
    {
        [DataMember]
        public List<GenderDTO> GenderTypes { get; set; }

        [DataMember]
        public List<MaritalStatusDTO> MaritalStatus { get; set; }

        [DataMember]
        public List<AddressTypeDTO> AddressTypes { get; set; }

        [DataMember]
        public List<AddressTypeDTO> AddressTypesbyEmail { get; set; }

        [DataMember]
        public List<AgentDeclinedTypeDTO> AgentDeclinedTypes { get; set; }

        [DataMember]
        public List<CurrencyDTO> Currencies { get; set; }

        [DataMember]
        public List<PhoneTypeDTO> PhoneTypes { get; set; }

        [DataMember]
        public List<EmailTypeDTO> EmailTypes { get; set; }

        [DataMember]
        public List<ExonerationTypeDTO> ExonerationTypes { get; set; }

        public LoadDataDTO ()
        {

        }
    }
}
