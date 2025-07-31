using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class PrefixDAO
    {        

        public bool CreatePrefixByLineBusiness(PrefixLineBusiness prefixLineBusiness)
        {
            COMMEN.PrefixLineBusiness lineByPrexi = new COMMEN.PrefixLineBusiness(prefixLineBusiness.PrefixCode, prefixLineBusiness.LineBusinessCode);
            PrimaryKey keyUp = COMMEN.PrefixLineBusiness.CreatePrimaryKey(prefixLineBusiness.PrefixCode, lineByPrexi.LineBusinessCode);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(lineByPrexi);

            return true;
        }

        public bool DeletePrefixByLineBusiness(PrefixLineBusiness PrefixLineBusiness)
        {
            PrimaryKey key = COMMEN.PrefixLineBusiness.CreatePrimaryKey(PrefixLineBusiness.PrefixCode, PrefixLineBusiness.LineBusinessCode);
            COMMEN.PrefixLineBusiness lineByPrexi = new COMMEN.PrefixLineBusiness(PrefixLineBusiness.PrefixCode, PrefixLineBusiness.LineBusinessCode);
            lineByPrexi = (COMMEN.PrefixLineBusiness)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            DataFacadeManager.Instance.GetDataFacade().DeleteObject(lineByPrexi);

            return true;
        }

		public int GetPrefixCoveredRiskTypeByPrefixCode(int prefixCode)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(PARAMEN.HardRiskType.Properties.LineBusinessCode, typeof(PARAMEN.HardRiskType).Name, prefixCode);

            BusinessCollection businessCollection = DataFacadeManager.GetObjects(typeof(PARAMEN.HardRiskType), filter.GetPredicate());

            if (businessCollection.Count > 0)
            {
                return Convert.ToInt32(businessCollection.Cast<PARAMEN.HardRiskType>().First().CoveredRiskTypeCode);
            }
            else
            {
                return 0;
            }
        }
    }
}
