using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    public static class PrefixTypeDAO
    {
        public static PARAMEN.PrefixType GetPrefixTypeByPrefixTypeId(int prefixTypeId)
        {
            PrimaryKey key = PARAMEN.PrefixType.CreatePrimaryKey(prefixTypeId);
            return (PARAMEN.PrefixType)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        }

        
    }
}
