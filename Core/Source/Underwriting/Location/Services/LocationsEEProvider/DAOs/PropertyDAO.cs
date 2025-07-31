using Sistran.Core.Application.Locations.EEProvider.Assemblers;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using Sistran.Core.Application.Utilities.DataFacade;
using UNDTO = Sistran.Core.Application.UnderwritingServices.DTOs;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using COEN = Sistran.Core.Application.Common.Entities;
using System.Linq;
using Sistran.Core.Application.Locations.EEProvider.Entities.Views;
using Sistran.Core.Framework.DAF.Engine;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.Locations.EEProvider.DAOs
{
    public class PropertyDAO
    {
        /// <summary>
        /// Obtiene polizas asociadas a la direccion
        /// </summary>
        /// <param name="street"></param>
        /// <returns></returns>
        public List<UNDTO.PolicyRiskDTO> GetPolicyRiskDTOsByStreet(string street)
        {
            List<UNDTO.PolicyRiskDTO> policyRiskDTOs = new List<UNDTO.PolicyRiskDTO>();
            List<UNDTO.PolicyRiskDTO> policyRiskDTOsLocations = new List<UNDTO.PolicyRiskDTO>();
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.RiskLocation.Properties.Street);
            filter.Equal();
            filter.Constant(street);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent);
            filter.Equal();
            filter.Constant(true);
            PolicyRiskLocationView view = new PolicyRiskLocationView();
            ViewBuilder builder = new ViewBuilder("PolicyRiskLocationView");
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            policyRiskDTOs = ModelAssembler.CreatePolicyRiskDTOs(view.Policies);
            List<COMMEN.Prefix> prefixes = view.Prefixes.Cast<COMMEN.Prefix>().ToList();
            List<COMMEN.Branch> branchs = view.Branches.Cast<COMMEN.Branch>().ToList();
            for (int i = 0; i < policyRiskDTOs.Count; i++)
            {
                policyRiskDTOs[i].PolicyId = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList()[i].PolicyId;
                policyRiskDTOs[i].EndorsementId = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList()[i].EndorsementId;
                policyRiskDTOs[i].RiskId = view.RiskLocations.Cast<ISSEN.RiskLocation>().ToList()[i].RiskId;
            }
            foreach (var riskLocation in policyRiskDTOs)
            {
                riskLocation.PrefixDescription = prefixes.Where(b => b.PrefixCode == riskLocation.PrefixId).FirstOrDefault().Description;
                riskLocation.BranchDescription = branchs.Where(b => b.BranchCode == riskLocation.BranchId).FirstOrDefault().Description;
                policyRiskDTOsLocations.Add(riskLocation);
            }
            if (policyRiskDTOsLocations.Count > 0)
            {
                policyRiskDTOsLocations = policyRiskDTOsLocations.GroupBy(b => new { b.BranchId, b.PrefixId, b.DocumentNumber }).Select(b => b.LastOrDefault()).ToList();
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.Locations.EEProvider.DAOs.PropertyDAO.GetPolicyRiskDTOsByStreet");
            return policyRiskDTOsLocations;

        }
    }
}
