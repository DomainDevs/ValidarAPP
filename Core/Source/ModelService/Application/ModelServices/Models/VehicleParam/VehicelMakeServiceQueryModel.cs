namespace Sistran.Core.Application.ModelServices.Models
{
    using System.Runtime.Serialization;
    [DataContract]
    public class VehicelMakeServiceQueryModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Description { get; set; }


    }
}
