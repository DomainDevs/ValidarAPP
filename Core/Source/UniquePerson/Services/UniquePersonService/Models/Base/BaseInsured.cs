using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.UniquePersonService.Models.Base
{
    [DataContract]
    public class BaseInsured : Individual
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
		[DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the insured identifier.
        /// </summary>
        /// <value>
        /// The insured identifier.
        /// </value>
        [DataMember]
        public int InsuredId { get; set; }

        /// <summary>
        /// Gets or sets the entered date.
        /// </summary>
        /// <value>
        /// The entered date.
        /// </value>
        [DataMember]
        public DateTime EnteredDate { get; set; }

        /// <summary>
        /// Gets or sets the update date.
        /// </summary>
        /// <value>
        /// The update date.
        /// </value>
        [DataMember]
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Gets or sets the declined date.
        /// </summary>
        /// <value>
        /// The declined date.
        /// </value>
        [DataMember]
        public DateTime? DeclinedDate { get; set; }

        /// <summary>
        /// Gets or sets the type of the ins declines.
        /// </summary>
        /// <value>
        /// The type of the ins declines.
        /// </value>
        [DataMember]
        public int? InsDeclinesType { get; set; }

        /// <summary>
        /// Gets or sets the annotations.
        /// </summary>
        /// <value>
        /// The annotations.
        /// </value>
        [DataMember]
        public string Annotations { get; set; }

        /// <summary>
        /// Gets or sets the profile.
        /// </summary>
        /// <value>
        /// The profile.
        /// </value>
        [DataMember]
        public int Profile { get; set; }

        /// <summary>
        /// Gets or sets the branch code.
        /// </summary>
        /// <value>
        /// The branch code.
        /// </value>
        [DataMember]
        public int BranchCode { get; set; }

        /// <summary>
        /// Gets or sets the modify date.
        /// </summary>
        /// <value>
        /// The modify date.
        /// </value>
        [DataMember]
        public DateTime? ModifyDate { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        [DataMember]
        public string Gender { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public string ReferedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsSMS { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsMailAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public bool IsComercialClient { get; set; }
    }
}
