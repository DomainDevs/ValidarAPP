using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.Utilities.Models
{
    public class EnumParameter 
    {
        /// <summary>
        /// llave con la cual se obtiene un valor de la parametrizacion
        /// </summary>
        public  string Key { get; set; }
        
        /// <summary>
        /// Valor obtenido
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// Columna de la Fuente de datos
        /// </summary>
        public string SourceColum { get; set; }

        /// <summary>
        /// Fuente de datos
        /// </summary>
        public string SourceTable { get; set; }

        /// <summary>
        /// Filtro de los datos
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Descripcion del parametro
        /// </summary>
        public string Description { get; set; }
    }
}
