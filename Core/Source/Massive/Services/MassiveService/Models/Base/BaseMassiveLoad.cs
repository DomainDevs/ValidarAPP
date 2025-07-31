using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.MassiveServices.Models.Base
{
    [DataContract]
    public class BaseMassiveLoad : Extension
    {
        /// <summary>
        /// Identificador
        /// </summary>
        [DataMember]
        public int Id { get; set; }

        /// <summary>
        /// Descripción
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Descripción Estado
        /// </summary>
        [DataMember]
        public string StatusDescription { get; set; }

        /// <summary>
        /// Tiene Error?
        /// </summary>
        [DataMember]
        public bool HasError { get; set; }

        /// <summary>
        /// Descripción Error
        /// </summary>
        [DataMember]
        public string ErrorDescription { get; set; }

        /// <summary>
        /// Total Registros
        /// </summary>
        [DataMember]
        public int TotalRows { get; set; }

        /// <summary>
        /// Descripción Tipo de Cargue
        /// </summary>
        [DataMember]
        public string LoadTypeDescription { get; set; }

        /// <summary>
        /// Procesados
        /// </summary>
        [DataMember]
        public int Processeds { get; set; }

        /// <summary>
        /// Pendientes
        /// </summary>
        [DataMember]
        public int Pendings { get; set; }

        /// <summary>
        /// Con Eventos
        /// </summary>
        [DataMember]
        public int WithEvents { get; set; }

        /// <summary>
        /// Con Error
        /// </summary>
        [DataMember]
        public int WithErrors { get; set; }

        /// <summary>
        /// Tarifados
        /// </summary>
        [DataMember]
        public int Tariffed { get; set; }

        /// <summary>
        /// Por tarifar
        /// </summary>
        [DataMember]
        public int ForRate { get; set; }

        /// <summary>
        /// Emitidos
        /// </summary>
        [DataMember]
        public int Issued { get; set; }

        /// <summary>
        /// Por emitir
        /// </summary>
        [DataMember]
        public int ForIssue { get; set; }
    }
}
