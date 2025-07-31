using Sistran.Core.Application.Extensions;
using System.Runtime.Serialization;
namespace Sistran.Core.Application.UtilitiesServices.Models.Base
{
    [DataContract]
    public class BaseField : Extension
    {
        /// <summary>
        /// Identificador del campo en la plantilla
        /// </summary>
        [DataMember]
        public int TemplateFieldId { get; set; }

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
        /// Abreviatura
        /// </summary>
        [DataMember]
        public string SmallDescription { get; set; }

        /// <summary>
        /// Esta Activo?
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Es Obligatorio?
        /// </summary>
        [DataMember]
        public bool IsMandatory { get; set; }

        /// <summary>
        /// Posición Columna
        /// </summary>
        [DataMember]
        public int Order { get; set; }

        /// <summary>
        /// Cantidad De Columnas
        /// </summary>
        [DataMember]
        public int ColumnSpan { get; set; }

        /// <summary>
        /// Posición Fila
        /// </summary>
        [DataMember]
        public int RowPosition { get; set; }

        /// <summary>
        /// Propiedad
        /// </summary>
        [DataMember]
        public string PropertyName { get; set; }

        /// <summary>
        /// Longitud
        /// </summary>
        [DataMember]
        public string PropertyLength { get; set; }

        /// <summary>
        /// Valor
        /// </summary>
        [DataMember]
        public string Value { get; set; }
    }
}
