using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UniquePersonServices.V1.Enums
{
    public enum Role
    {
        /// <summary>
        /// Accesso a Asegurado
        /// </summary>
        Asegurado=1,
        /// <summary>
        /// Acceso a Intermidario
        /// </summary>
        Intermidario = 2,
        /// <summary>
        /// Acceso a Empleado
        /// </summary>
        Empleado = 3,
        /// <summary>
        /// Acceso a beneficiario
        /// </summary>
        Benefeciario = 4,
        /// <summary>
        /// Acceso a director comercial / Nacional 
        /// </summary>
        DirectorComercialNacional = 5,
        /// <summary>
        /// Acceso a abogado 
        /// </summary>
        Abogado = 6,
        /// <summary>
        /// Acceso a provedor 
        /// </summary>
        Proveedor = 7,
        /// <summary>
        /// Acceso a cesionario
        /// </summary>
        Cesionario = 8,
        /// <summary>
        /// Acceso a tercero 
        /// </summary>
        Tercero = 9,
        /// <summary>
        /// Acceso a Tercero vinculado 
        /// </summary>
        TerceroVinculado = 10,
        /// <summary>
        /// Acceso a reaseguro
        /// </summary>
        Reasegurado = 11,
        /// <summary>
        /// Acceso a coasegurado
        /// </summary>
        Coasegurado = 12
    }
}
