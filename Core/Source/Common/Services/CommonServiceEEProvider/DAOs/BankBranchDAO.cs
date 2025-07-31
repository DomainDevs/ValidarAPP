using Sistran.Core.Application.CommonServices.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using COMMEN = Sistran.Core.Application.Common.Entities;
using COMMML = Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    public class BankBranchDAO
    {
        public List<COMMML.BankBranch> GetBankBranches(int bankId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.BankBranch.Properties.BankCode, typeof(COMMEN.BankBranch).Name);
            filter.Equal();
            filter.Constant(bankId);


            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.BankBranch), filter.GetPredicate()));
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetBankBranches");
            return ModelAssembler.CreateBankBranches(businessCollection);
        }
    }
}
