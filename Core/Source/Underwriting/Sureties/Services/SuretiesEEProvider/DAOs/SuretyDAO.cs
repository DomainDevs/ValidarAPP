using Sistran.Core.Application.SuretiesEEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Vehicles.EEProvider.Entities.Views;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using UNDTO = Sistran.Core.Application.UnderwritingServices.DTOs;


namespace Sistran.Core.Application.SuretiesEEProvider.DAOs
{
    public class SuretyDAO
    {
        /// <summary>
        /// Consulta los datos basicos de las polizas que esten asociadas al afianzado
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns>
        /// poliza con datos basicos
        /// </returns>
        public List<UNDTO.PolicyRiskDTO> GetPolicyRiskDTOsByIndividualId(int individualId)
        {
            List<UNDTO.PolicyRiskDTO> policyRiskDTOs = new List<UNDTO.PolicyRiskDTO>();
            List<UNDTO.PolicyRiskDTO> policyRiskDTOsSureties = new List<UNDTO.PolicyRiskDTO>();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.RiskSurety.Properties.IndividualId);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent);
            filter.Equal();
            filter.Constant(true);
            PolicyRiskSuretyView view = new PolicyRiskSuretyView();
            ViewBuilder builder = new ViewBuilder("PolicyRiskSuretyView");
            builder.Filter = filter.GetPredicate();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                daf.FillView(builder, view);
            }
            policyRiskDTOs = ModelAssembler.CreatePolicyRiskDTOs(view.Policies.Cast<ISSEN.Policy>().ToList());
            List<COMMEN.Prefix> prefixes = view.Prefixes.Cast<COMMEN.Prefix>().ToList();
            List<COMMEN.Branch> branchs = view.Branches.Cast<COMMEN.Branch>().ToList();
            object objlock = new object();
            Parallel.For(0, policyRiskDTOs.Count, i =>
            {
                policyRiskDTOs[i].PolicyId = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList()[i].PolicyId;
                policyRiskDTOs[i].RiskId = view.RiskSureties.Cast<ISSEN.RiskSurety>().ToList()[i].RiskId;
                policyRiskDTOs[i].EndorsementId = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList()[i].EndorsementId;
                policyRiskDTOs[i].PrefixDescription = prefixes.Where(b => b.PrefixCode == policyRiskDTOs[i].PrefixId).FirstOrDefault().Description;
                policyRiskDTOs[i].BranchDescription = branchs.Where(b => b.BranchCode == policyRiskDTOs[i].BranchId).FirstOrDefault().Description;
                lock (objlock)
                {
                    policyRiskDTOsSureties.Add(policyRiskDTOs[i]);
                }
            });         
            if (policyRiskDTOsSureties.Count > 0)
            {
                policyRiskDTOsSureties = policyRiskDTOsSureties.GroupBy(b => new { b.BranchId, b.PrefixId, b.DocumentNumber }).Select(b => b.LastOrDefault()).ToList();
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "namespace Sistran.Core.Application.SuretiesEEProvider.DAOs.GetPolicyRiskDTOsByIndividualId");
            return policyRiskDTOsSureties;
        }
    }
}
