using System;
using System.ComponentModel.DataAnnotations;

namespace Sistran.Core.Framework.UIF.Web.Areas.Audit.Models
{
    public class AuditModelView
    {

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Id de la SchemaId
        /// </summary>
        /// <value>
        /// The schema identifier.
        /// </value>
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public int? SchemaId { get; set; }


        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserId { get; set; }


        /// <summary>
        /// Gets or sets the transaccion identifier.
        /// </summary>
        /// <value>
        /// The transaccion identifier.
        /// </value>
        [Display(Name = "TypeTransaction", ResourceType = typeof(App_GlobalResources.Language))]
        public int? TransaccionId { get; set; }

        /// <summary>
        /// Id de la Objeto
        /// </summary>
        /// <value>
        /// The object identifier.
        /// </value>

        public int? ObjectId { get; set; }



        /// <summary>
        /// Gets or sets the name of the object.
        /// </summary>
        /// <value>
        /// The name of the object.
        /// </value>
        public string ObjectName { get; set; }



        /// <summary>
        /// Gets or sets the current from.
        /// </summary>
        /// <value>
        /// The current from.
        /// </value>
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "CurrentFrom", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public DateTime CurrentFrom { get; set; }


        /// <summary>
        /// Gets or sets the current to.
        /// </summary>
        /// <value>
        /// The current to.
        /// </value>
        [Display(Name = "CurrentTo", ResourceType = typeof(App_GlobalResources.Language))]
        [Required(ErrorMessageResourceType = typeof(App_GlobalResources.Language), ErrorMessageResourceName = "ErrorDocument")]
        public DateTime CurrentTo { get; set; }
    }
}