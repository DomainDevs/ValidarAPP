using Sistran.Core.Application.Sureties.Models;
using Sistran.Core.Application.SuretiesEEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Vehicles.EEProvider.Entities.Views;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Entities = Sistran.Core.Application.Issuance.Entities;
using UniquePersonModel = Sistran.Core.Application.UniquePersonService.V1.Models;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;
namespace Sistran.Core.Application.SuretiesEEProvider.DAOs
{
    public class RiskSuretyGuaranteeDAO
    {
        /// <summary>
        /// Obtener  las contragarantias que han sido asignadas
        /// </summary>
        /// <param name="guarantees">Listado de contragarantias</param>
        /// <returns>Lista de contragarantías cumplimiento</returns>
        public List<RiskSuretyGuarantee> GetRiskSuretyGuaranteesByGuarantees(List<IssuanceGuarantee> guarantees)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Entities.RiskSuretyGuarantee.Properties.GuaranteeId, typeof(Entities.RiskSuretyGuarantee).Name);
            filter.In().ListValue();
            foreach (IssuanceGuarantee item in guarantees)
            {
                filter.Constant(item.Code);
            }
            filter.EndList();
            List<Entities.RiskSuretyGuarantee> riskSuretyGuarantees = new List<Entities.RiskSuretyGuarantee>();
            //using (var daf = DataFacadeManager.Instance.GetDataFacade())
            //{
            //    riskSuretyGuarantees = daf.List(typeof(Entities.RiskSuretyGuarantee), filter.GetPredicate()).Cast<Entities.RiskSuretyGuarantee>().ToList();
            //} 
            riskSuretyGuarantees = DataFacadeManager.GetObjects(typeof(Entities.RiskSuretyGuarantee), filter.GetPredicate()).Cast<Entities.RiskSuretyGuarantee>().ToList();

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.SuretiesEEProvider.DAOs.GetRiskSuretyGuaranteesByGuarantees");
            return ModelAssembler.CreateRiskSuretyGuarantees(riskSuretyGuarantees);
        }

        public List<IssuanceInsuredGuarantee> GetRiskSuretyGuaranteesByRiskId(int riskId)
        {
            List<IssuanceInsuredGuarantee> riskSuretyGuarantees = new List<IssuanceInsuredGuarantee>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(Entities.RiskSuretyGuarantee.Properties.RiskId, typeof(Entities.RiskSuretyGuarantee).Name, riskId);

            RiskSuretyGuaranteeView riskSuretyGuaranteeView = new RiskSuretyGuaranteeView();
            ViewBuilder viewBuilder = new ViewBuilder("RiskSuretyGuaranteeView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, riskSuretyGuaranteeView);

            if (riskSuretyGuaranteeView.InsuredGuarantees.Count > 0)
            {
                riskSuretyGuarantees = ModelAssembler.CreateIssuanceInsuredGuarantees(riskSuretyGuaranteeView.InsuredGuarantees.Cast<UPEN.InsuredGuarantee>().ToList());
            }

            return riskSuretyGuarantees;
        }
    }
}
