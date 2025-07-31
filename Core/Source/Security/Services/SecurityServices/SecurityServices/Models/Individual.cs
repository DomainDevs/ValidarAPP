using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.SecurityServices.Models
{
    [DataContract]
    [Flags]
    public enum CustomerType
    {
        [EnumMember]
        Individual = 1,
        [EnumMember]
        Prospect = 2
    }    
    [DataContract]
    [Flags]
    public enum IndividualType : int
    {
        [EnumMember]
        Person = 1,
        [EnumMember]
        Company = 2
    };
    [DataContract]
    public class Individual
    {
        /// <summary>
        /// IndividualId.
        /// </summary>
        [DataMember]
        public int IndividualId { get; set; }
        /// <summary>
        /// Name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }
        /// <summary>
        /// CustomerType.
        /// </summary>
        [DataMember]
        public CustomerType CustomerType { get; set; }
        /// <summary>
        /// InsuredType.
        /// </summary>
        [DataMember]
        public IndividualType InsuredType { get; set; }
    }
}
