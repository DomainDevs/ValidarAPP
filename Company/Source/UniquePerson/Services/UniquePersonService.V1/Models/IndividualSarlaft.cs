using System;
using System.Runtime.Serialization;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    /// <summary>
    /// Individuo Sarlaft
    /// </summary>
    /// <seealso cref="Sistran.Core.Application.UniquePersonService.V1.Models.Individual" />
    [DataContract]
    public class IndividualSarlaft : Sistran.Core.Application.UniquePersonService.V1.Models.Individual
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
        /// Gets or sets the form number.
        /// </summary>
        /// <value>
        /// The form number.
        /// </value>
        [DataMember]
        public string FormNum { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        [DataMember]
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the authorized by.
        /// </summary>
        /// <value>
        /// The authorized by.
        /// </value>
        [DataMember]
        public string AuthorizedBy { get; set; }

        /// <summary>
        /// Gets or sets the verifying employee.
        /// </summary>
        /// <value>
        /// The verifying employee.
        /// </value>
        [DataMember]
        public string VerifyingEmployee { get; set; }

        /// <summary>
        /// Gets or sets the name of the interviewer.
        /// </summary>
        /// <value>
        /// The name of the interviewer.
        /// </value>
        [DataMember]
        public string InterviewerName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [international operations].
        /// </summary>
        /// <value>
        /// <c>true</c> if [international operations]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool InternationalOperations { get; set; }

        /// <summary>
        /// Gets or sets the interviewer place.
        /// </summary>
        /// <value>
        /// The interviewer place.
        /// </value>
        [DataMember]
        public string InterviewerPlace { get; set; }

        /// <summary>
        /// Gets or sets the interview result code.
        /// </summary>
        /// <value>
        /// The interview result code.
        /// </value>
        [DataMember]
        public int InterviewResultCode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [pending events].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [pending events]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool PendingEvents { get; set; }

        /// <summary>
        /// Gets or sets the registration date.
        /// </summary>
        /// <value>
        /// The registration date.
        /// </value>
        [DataMember]
        public DateTime? RegistrationDate { get; set; }

        /// <summary>
        /// Gets or sets the branch code.
        /// </summary>
        /// <value>
        /// The branch code.
        /// </value>
        [DataMember]
        public int BranchCode { get; set; }

        /// <summary>
        /// Gets or sets the financial sarlaft.
        /// </summary>
        /// <value>
        /// The financial sarlaft.
        /// </value>
        [DataMember]
        public FinancialSarlaf FinancialSarlaft { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        [DataMember]
        public int UserId { get; set; }

    }
}
