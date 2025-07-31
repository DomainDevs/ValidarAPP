using Sistran.Core.Application.UnderwritingServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Sistran.Company.Application.UnderwritingServices.Models
{
    [DataContract]
    public class FirstPayComponent
    {
        /// <summary>
        /// Plan Financiero
        /// </summary>
        [DataMember]
        public FinancialPlan FinancialPlan { get; set; }

		/// <summary>
        /// Componente
        /// </summary>
        [DataMember]
        public Component Component { get; set; }
    }
}
