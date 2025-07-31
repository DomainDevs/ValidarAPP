using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.ClaimServices.EEProvider.Views.Claims
{
    [Serializable()]
    public class ClaimNoticeView : BusinessView
    {
        public BusinessCollection ClaimNotices
        {
            get
            {
                return this["ClaimNotice"];
            }
        }
        public BusinessCollection ClaimNoticeDocumentations
        {
            get
            {
                return this["ClaimNoticeDocumentation"];
            }
        }
        public BusinessCollection ClaimNoticeCoverages
        {
            get
            {
                return this["ClaimNoticeCoverage"];
            }
        }
        public BusinessCollection ClaimNoticeContactInformations
        {
            get
            {
                return this["ClaimNoticeContactInformation"];
            }
        }
    }
}
