namespace Sistran.Core.Application.ModelServices.Models.UniquePerson
{
    using Param;
    using System.Runtime.Serialization;

    public class WorkerTypeServiceModel : ParametricServiceModel
    {

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string SmallDescription { get; set; }

        [DataMember]
        public bool IsEnabled { get; set; }

    }
}
