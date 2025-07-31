using System.Runtime.Serialization;
using Sistran.Co.Previsora.Application.FullServices.Models;
using System.Collections.Generic;

namespace Sistran.Co.Previsora.Application.FullServices.DTOs
{
    [DataContract]
    public class DtoTechnicalCard
    {
        [DataMember]
        public TECHNICAL_CARD TECHNICAL_CARD { get; set; }

        [DataMember]
        public List<BOARD_DIRECTORS> List_BOARD_DIRECTORS { get; set; }

        [DataMember]
        public List<FINANCIAL_STATEMENTS> List_FINANCIAL_STATEMENTS { get; set; }

        [DataMember]
        public List<TECHNICAL_CARD_DESCRIPTION> List_TECHNICAL_CARD_DESCRIPTION { get; set; }
    }
}