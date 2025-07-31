using System;
using System.Runtime.Serialization;

namespace Sistran.Core.Application.MassiveUnderwritingServices.Models
{
    /// <summary>
    /// Cabeceras del excel
    /// </summary>
    [DataContract]
    public class MassiveLoadHeader
    {
        /// <summary>
        /// Obtiene o establece el ID de cabecera
        /// </summary>
        [DataMember]
        public int HeaderId { get; set; }

        /// <summary>
        /// Obtiene o establece el ID de conjunto de datos
        /// </summary>
        [DataMember]
        public int FieldSetId { get; set; }

        /// <summary>
        /// Obtiene o establece el Titulo cabecera
        /// </summary>
        [DataMember]
        public String Text { get; set; }

        /// <summary>
        /// Obtiene o establece la Columna inicial
        /// </summary>
        [DataMember]
        public int BeginColumn { get; set; }

        /// <summary>
        /// Obtiene o establece las Columnas combinadas
        /// </summary>
        [DataMember]
        public int ColumnSpan { get; set; }

        /// <summary>
        /// Obtiene o establece la Fila inicial
        /// </summary>
        [DataMember]
        public int BeginRow { get; set; }

        /// <summary>
        /// Obtiene o establece las Filas combinadas
        /// </summary>
        [DataMember]
        public int RowSpan { get; set; }

        /// <summary>
        /// Obtiene o establece el Color de fondo
        /// </summary>
        [DataMember]
        public string ColorRgb { get; set; }

        /// <summary>
        /// Obtiene o establece el Color de titulo
        /// </summary>
        [DataMember]
        public string TextColorRgb { get; set; }

        /// <summary>
        /// Obtiene o establece el Tamaño de titulo
        /// </summary>
        [DataMember]
        public string TextSize { get; set; }

        /// <summary>
        /// Obtiene o establece la Alineación de titulo
        /// </summary>
        [DataMember]
        public string TextAling { get; set; }

        /// <summary>
        /// Obtiene o establece el titulo en negrita
        /// </summary>
        [DataMember]
        public bool TextBold { get; set; }
    }
}
