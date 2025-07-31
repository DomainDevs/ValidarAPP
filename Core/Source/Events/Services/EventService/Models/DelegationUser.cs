
using System.Runtime.Serialization;

namespace Sistran.Core.Application.EventsServices.Models
{
    [DataContract]
    public class DelegationUser
    {
        /// <summary>
        /// Atributo para la propiedad AuthorizedInd.
        /// </summary>
        [DataMember]
        public bool AuthorizedInd { get; set; }

        /// <summary>
        /// Atributo para la propiedad NotificatedInd.
        /// </summary>
        [DataMember]
        public bool NotificatedInd { get; set; }

        /// <summary>
        /// Atributo para la propiedad NotificatedDefault.
        /// </summary>
        [DataMember]
        public bool NotificatedDefault { get; set; }

        /// <summary>
        /// Atributo para la propiedad UserId.
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Atributo para la propiedad Description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Atributo para la propiedad UserName.
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// Atributo para la propiedad Email.
        /// </summary>
        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public int PersonId { get; set; }
    }
}
