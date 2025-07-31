using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UniquePersonService.V1.Models.Base
{
    [DataContract]
    public class BaseReInsurer : Extension
    {
        public int IndividualId { get; set; }
        public int ReinsuredCD { get; set; }
        public DateTime EnteredDate { get; set; }
        public DateTime? ModifyDate { get; set; }
        public DateTime? DeclinedDate { get; set; }
        public int? DeclaredTypeCD { get; set; }
        public string Annotations { get; set; }
        public bool IsActive { get; set; }


    }
}
