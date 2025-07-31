using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.SecurityServices.EEProvider.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class Module
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        [DataMember]
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Module"/> is disabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disabled; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool Disabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is search enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is search enabled; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool isSearchEnabled
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        [DataMember]
        public int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        [DataMember]
        public string Image
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>
        /// The path.
        /// </value>
        [DataMember]
        public string Path
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the sub modules.
        /// </summary>
        /// <value>
        /// The sub modules.
        /// </value>
        [DataMember]
        public List<Module> SubModules
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember]
        public string Title
        {
            get;
            set;
        }
    }
}
