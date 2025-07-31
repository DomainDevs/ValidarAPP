using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.Utilities.DataFacade;
using System.Collections.Generic;
using CLMEN = Sistran.Core.Application.Claims.Entities;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class DamageDAO
    {
        public List<DamageType> GetDamageTypes()
        {
            return ModelAssembler.CreateDamageTypes(DataFacadeManager.GetObjects(typeof(CLMEN.ClaimDamageType)));
        }

        public List<DamageResponsibility> GetDamageResponsibilities()
        {
            return ModelAssembler.CreateDamageResponsibilities(DataFacadeManager.GetObjects(typeof(CLMEN.ClaimDamageResponsibility)));
        }
    }
}
