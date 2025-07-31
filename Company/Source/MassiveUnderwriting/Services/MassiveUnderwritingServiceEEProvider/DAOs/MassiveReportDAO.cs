using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Sistran.Core.Application.MassiveUnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Cache;


namespace Sistran.Company.Application.MassiveUnderwritingServices.EEProvider.DAOs
{
    public class MassiveReportDAO
    {
        public void LoadCommonCacheList( )
        {

            var documentTypes = InProcCache.Instance.GetCurrentDic("documentTypes_", "Report");
            if (documentTypes == null)
            {
                InProcCache.Instance.InsertCurrentDic("documentTypes_", "Report", DelegateService.uniquePersonService.GetDocumentTypes(3));
            }

            var deductibles = InProcCache.Instance.GetCurrentDic("deductibles_", "Report");
            if (deductibles == null)
            {
                InProcCache.Instance.InsertCurrentDic("deductibles_", "Report", DelegateService.underwritingService.GetDeductiblesAll());
            }
            var branches = InProcCache.Instance.GetCurrentDic("branches_", "Report");
            if (branches == null)
            {
                InProcCache.Instance.InsertCurrentDic("branches_", "Report", DelegateService.commonService.GetBranches());
            }

        }

        public object GetCacheList(string key, string hash)
        {

            return InProcCache.Instance.GetCurrentDic(key, hash);
        }

        public void  ClearCacheList()
        {
            InProcCache.Instance.Flush("branches_Report");
            InProcCache.Instance.Flush("deductibles_Report");
            InProcCache.Instance.Flush("documentTypes_Report");

        }
    }
}
