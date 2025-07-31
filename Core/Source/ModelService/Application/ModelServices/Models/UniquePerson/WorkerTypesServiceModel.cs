using System.Collections.Generic;

namespace Sistran.Core.Application.ModelServices.Models.UniquePerson
{
    using Param;
    using System.Runtime.Serialization;

    [DataContract]
    public class WorkerTypesServiceModel : ErrorServiceModel
    {
        [DataMember]
        public List<WorkerTypeServiceModel> WorkerTypeServiceModel { get; set; }


    }
}
