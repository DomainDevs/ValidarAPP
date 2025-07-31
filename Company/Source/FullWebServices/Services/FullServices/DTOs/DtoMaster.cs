using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sistran.Co.Previsora.Application.FullServices.Models
{
    [DataContract]
    public class DtoMaster
    {
        //Edward Rubiano -- C11226 -- Validacion de datos basicos -- 10/05/2016
        [DataMember]
        public string GlobalOperation { get; set; }
        //Edward Rubiano -- C11226 -- Validacion de datos basicos -- 10/05/2016

        [DataMember]
        public string User { get; set; }

        [DataMember]
        public int IdApp { get; set; }

        [DataMember]
        public String cod_Rol { get; set; }

        [DataMember]
        public DtoInsured DtoInsured { get; set; }

        [DataMember]
        public DtoLawyer DtoLawyer { get; set; }

        [DataMember]
        public DtoUser DtoUser { get; set; }

        [DataMember]
        public DtoAgent DtoAgent { get; set; }

        [DataMember]
        public DtoAssigneed DtoAssigneed { get; set; }

        [DataMember]
        public DtoBeneficiary DtoBeneficiary { get; set; }

        [DataMember]
        public DtoPrincipalNational DtoPrincipalNational { get; set; }

        [DataMember]
        public DtoPrincipalComertial DtoPrincipalComertial { get; set; }

        [DataMember]
        public DtoTechnicalAssistant DtoTechnicalAssistant { get; set; }
        
        [DataMember]
        public DtoEmployee DtoEmployee { get; set; }

        [DataMember]
        public DtoProvider DtoProvider { get; set; }

        [DataMember]
        public DtoThird DtoThird { get; set; }
    }
}
