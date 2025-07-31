namespace Sistran.Core.Application.ModelServices.Models.VehicleParam
{
    using Param;
    using System.Runtime.Serialization;
    [DataContract]
    public class VehicleModelServiceModel : ParametricServiceModel
    {
        [DataMember]
        public int Id { get; set; }


        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string SmallDescription { get; set; }

        [DataMember]
        public VehicelMakeServiceQueryModel VehicelMakeServiceQueryModel { get; set; }



    }
}
