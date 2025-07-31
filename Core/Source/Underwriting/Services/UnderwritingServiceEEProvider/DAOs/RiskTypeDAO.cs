using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Entities.Views;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class RiskTypeDAO
    {
        public List<RiskType> GetRiskTypeByPrefixId(int prefixId)
        {
            PrefixRiskTypeView view = null;
            try
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                view = new PrefixRiskTypeView();
                ViewBuilder builder = new ViewBuilder("PrefixRiskTypeView");
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(COMMEN.Prefix.Properties.PrefixCode, typeof(COMMEN.Prefix).Name);
                filter.Equal();
                filter.Constant(prefixId);
                builder.Filter = filter.GetPredicate();
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.FillView(builder, view);
                }
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.DAOs.GetRiskTypeByPrefixId");
                return ModelAssembler.CreateRiskTypes(view.CoveredRiskTypeList);

            }
            catch (Exception exc)
            {
                throw new BusinessException("Error en GetRiskTypeByPrefixId", exc);
            }
        }
    }
}
