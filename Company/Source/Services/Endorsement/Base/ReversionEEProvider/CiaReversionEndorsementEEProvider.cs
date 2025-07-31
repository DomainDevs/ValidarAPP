

using System.Collections.Generic;
using Sistran.Company.Application.ReversionEndorsement.EEProvider.DAOs;
using Sistran.Core.Framework.BAF;
using System.ServiceModel;
using System;
using Sistran.Company.Application.Common.EndorsementWorkFlow.Entities;
using Sistran.Company.Application.ReversionEndorsement.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;

namespace Sistran.Company.Application.ReversionEndorsement.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CiaReversionEndorsementEEProvider : ICiaReversionEndorsement
    {

        public List<string> GetEndorsementWorkFlow(int PolyciId)
        {
            try
            {
                ReversionDAO EndorsementWorkFlow = new ReversionDAO();
                var result = EndorsementWorkFlow.GetEndorsementWorkFlow(PolyciId);
                return result;
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        public bool CreateEndorsementWorkFlow(int? PolyciId, int? EndorsementId, string filingNumber, DateTime filingDate)
        {
            try
            {
                ReversionDAO EndorsementWorkFlow = new ReversionDAO();
                bool result = EndorsementWorkFlow.CreateEndorsementWorkFlow(PolyciId, EndorsementId, filingNumber, filingDate);
                return true;
            }
            catch (Exception Ex)
            {
                return false;
            }
        }
    }
}
