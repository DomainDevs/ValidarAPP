using System.Collections.Generic;
using System.Runtime.Serialization;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.Application.UniqueUserServices.Models;
using Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.Enum;

namespace Sistran.Company.Application.MassiveVehicleParamBusinessService.Model
{
    [DataContract]
    public class CompanyProcessFasecoldaMassiveLoad
    {
        [DataMember]
        public int Id { get; set; }
        
        [DataMember]
        public int ProcessId { get; set; }

        [DataMember]
        public int Pendings { get; set; }

        [DataMember]
        public int WithErrorsLoaded { get; set; }

        [DataMember]
        public int Loaded { get; set;}

        [DataMember]
        public int WithErrorsProcesseds { get; set; }

        [DataMember]
        public int TotalRowsProcesseds { get; set; }

        [DataMember]
        public int TotalRowsLoaded { get; set; }

        [DataMember]
        public int? StatusId { get; set; }

        [DataMember]
        public int TotalRows { get; set; }
        
        [DataMember]
        public User User { get; set; }

        [DataMember]
        public File File { get; set; }

        [DataMember]
        public int EnableProcessing { get; set; }

        [DataMember]
        public FileTypeFasecoldaEnum TypeFile { get; set; }

        [DataMember]

        public List<CompanyMassiveVehicleFasecoldaRow> Row { get; set; }
    }
}
