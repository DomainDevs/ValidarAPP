using Sistran.Core.Application.Extensions;
using System;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.PrintingServices.Models
{
    [DataContract]
    public class Printing : Extension
    {

        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Tipo de impresion
        /// </summary>
        [DataMember]
        public int PrintingTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public int KeyId { get; set; }

        /// <summary>
        /// Ruta donde esta la impresion 
        /// </summary>
        [DataMember]
        public string UrlFile { get; set; }
        /// <summary>
        /// Total de impresiones
        /// </summary>
        [DataMember]
        public int Total { get; set; }

        /// <summary>
        /// Fecha de inicio impresion
        /// </summary>
        [DataMember]
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// Fecha de fin impresion
        /// </summary>
        [DataMember]
        public DateTime FinishDate { get; set; }

        /// <summary>
        /// Codigo de usuario
        /// </summary>
        [DataMember]
        public int UserId { get; set; }

        /// <summary>
        /// Tiene error?
        /// </summary>
        [DataMember]
        public bool HasError { get; set; }

        /// <summary>
        /// Mensaje de error
        /// </summary>
        [DataMember]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Tiene error?
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }
    }
}
