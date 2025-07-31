using Sistran.Core.Application.Finances.EEProvider.Assemblers;
using Sistran.Core.Application.Finances.Models;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;

namespace Sistran.Core.Application.Finances.EEProvider.Business
{
    public class FinancesBusiness
    {
        public List<IssuanceOccupation> GetOccupations()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Occupation)));
            return ModelAssembler.CreateOccupations(businessCollection);
        }
    }
}
