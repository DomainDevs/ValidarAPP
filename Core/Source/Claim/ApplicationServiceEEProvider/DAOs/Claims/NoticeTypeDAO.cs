using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class NoticeTypeDAO
    {
        public List<NoticeType> GetNoticeTypes()
        {
            return ModelAssembler.CreateNoticeTypes(DataFacadeManager.GetObjects(typeof(PARAMEN.ClaimNoticeType)));
        }
    }
}
