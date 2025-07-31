using Sistran.Core.Application.UniquePersonService.V1.Models.Base;
using System.Collections.Generic;

namespace Sistran.Company.Application.UniquePersonServices.V1.Models
{
    public class CompanyCoCompanyName : BaseCompanyName
    {

        /// <summary>
        /// Direccion
        /// </summary>    
        public CompanyAddress Address { get; set; }

        /// <summary>
        /// Teléfono 
        /// </summary>       
        public CompanyPhone Phone { get; set; }

        /// <summary>
        /// Correo 
        /// </summary>        
        public CompanyEmail Email { get; set; }        

    }
}
