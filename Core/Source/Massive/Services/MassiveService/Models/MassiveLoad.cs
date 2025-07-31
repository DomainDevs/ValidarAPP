using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.MassiveServices.Enums;
using System.Collections.Generic;
using Sistran.Core.Application.UniqueUserServices.Models;
using System.Runtime.Serialization;
using Sistran.Core.Services.UtilitiesServices.Models;

namespace Sistran.Core.Application.MassiveServices.Models
{
    [DataContract]
    public class MassiveLoad
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
        /// Archivo
        /// </summary>
        [DataMember]
        public File File { get; set; }

        /// <summary>
        /// Usuario
        /// </summary>
        [DataMember]
        public User User { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        [DataMember]
        public MassiveLoadStatus? Status { get; set; }

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
        /// Tipo de Cargue
        /// </summary>
        [DataMember]
        public LoadType LoadType { get; set; }

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

        /// <summary>
        /// Logs
        /// </summary>
        [DataMember]
        public List<MassiveLoadLog> Logs { get; set; }
    }
}