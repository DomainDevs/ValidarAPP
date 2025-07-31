using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.Utilities.DataFacade;
using System.Collections.Generic;
using PARAMEN = Sistran.Core.Application.Parameters.Entities;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class CancellationReasonDAO
    {
        public List<ClaimCancellationReason> GetCancellationReasons()
        {
            return ModelAssembler.CreateCancellationReasons(DataFacadeManager.GetObjects(typeof(PARAMEN.CancellationReason)));
        }
    }
}
