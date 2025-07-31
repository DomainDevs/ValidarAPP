using Sistran.Core.Application.Temporary.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public static class CoTempSubscription2gDAO
    {
        /// <summary>
        /// Gets the co temporary subscription2g by temporary identifier.
        /// </summary>
        /// <param name="tempId">The temporary identifier.</param>
        /// <returns></returns>
        public static CoTempSubscription2g GetCoTempSubscription2gByTempId(int tempId)
        {
            PrimaryKey key = CoTempSubscription2g.CreatePrimaryKey(tempId);
            return (CoTempSubscription2g)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        }
    }
}
