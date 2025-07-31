using Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.Models;
namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Business
{
    public class HolderBusiness
    {
        /// <summary>
        /// Obtener dato basi tomador siempre debe tener el rol de asegurado
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public static IssuanceInsured GetHolderByIndividualId(int individualId)
        {
            HolderDAO holderDAO = new HolderDAO();
            return holderDAO.GetHolderByIndividualId(individualId);
        }
    }
}
