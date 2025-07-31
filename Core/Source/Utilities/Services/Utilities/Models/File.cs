using Sistran.Core.Application.UtilitiesServices.Models.Base;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sistran.Core.Services.UtilitiesServices.Models
{
    [DataContract]
    public class File : BaseFile
    {
        /// <summary>
        /// Tipo De Archivo
        /// </summary>
        [DataMember]
        public FileType? FileType { get; set; }

        /// <summary>
        /// Plantillas
        /// </summary>
        [DataMember]
        public List<Template> Templates { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        [DataMember]
        public ParametrizationStatus? parametrizationStatus { get; set; }

        public static explicit operator File(BusinessObject v)
        {
            throw new NotImplementedException();
        }
    }
}