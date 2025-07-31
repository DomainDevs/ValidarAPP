using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingBusinessServiceProvider.Entities.Views
{
    [Serializable()]
    public class SurchargeView: BusinessView
    {
        /// <summary>
        /// Obtiene recargos
        /// </summary>
        /// <returns> retorna el modelo de componentes </returns>
        public BusinessCollection Surcharge
        {
            get
            {
                return this["Surcharge"];
            }
        }

        /// <summary>
        /// Obtiene componentes 
        /// </summary>
        /// <returns> retorna el modelo de componentes </returns>
        public BusinessCollection Component
        {
            get
            {
                return this["Component"];
            }
        }

        /// <summary>
        /// Obtiene tipo de tasa
        /// </summary>
        /// <returns> retorna el modelo de componentes </returns>
        public BusinessCollection RateType
        {
            get
            {
                return this["RateType"];
            }
        }
    }
}
