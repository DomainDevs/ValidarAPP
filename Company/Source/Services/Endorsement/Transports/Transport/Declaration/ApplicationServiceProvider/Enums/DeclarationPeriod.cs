using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.Transports.Endorsement.Declaration.ApplicationServices.EEProvider.Enums
{
    public enum DeclarationPeriod
    {
        /// <summary>
        /// Constante Intervalo Mensual en meses
        /// </summary>
        Monthly = 1,
        /// <summary>
        /// Constante Intervalo Bimensual en meses
        /// </summary>
        Bimonthly = 2,
        /// <summary>
        /// Constante Intervalo trimestral en meses
        /// </summary>
        Quaterly = 3,
        /// <summary>
        /// Constante Intervalo Cuatrimestral en meses
        /// </summary>
        FourtQuater = 4,
        /// <summary>
        /// Constante Intervalo Semestral en meses
        /// </summary>
        Biannual = 5,
        /// <summary>
        /// Constante Intervalo Anual en meses
        /// </summary>
        Annual = 6
    }
}
