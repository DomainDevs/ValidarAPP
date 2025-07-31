using AutoMapper;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Linq;
using Sistran.Company.Application.Vehicles.VehicleCollectiveServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Vehicles.VehicleServices.Models;
using MSVEN = Sistran.Core.Application.Massive.Entities;
using TMPEN = Sistran.Core.Application.Temporary.Entities;
using COLEN = Sistran.Company.Application.CollectiveServices.EEProvider.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Company.Application.UnderwritingServices.Models;

namespace Sistran.Company.Application.Vehicles.VehicleCollectiveServices.EEProvider.DAOs
{
    public class VehicleMassiveDAO
    {
        /// <summary>
        /// Obtener temporal de autos
        /// </summary>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>Modelo temporal</returns>
        public CompanyPolicy GetCompanyVehiclePolicyByTemporalIdMassiveId(int temporalId, List<int> listMassiveLoad, bool withOutEvent)
        {


            Policy policy = DelegateService.collectiveService.GetTemporalPolicyByTemporalIdMassiveId(temporalId, listMassiveLoad);

            if (policy != null)
            {
                CompanyPolicy vehiclePolicy = new CompanyPolicy();
                Mapper.CreateMap(policy.GetType(), vehiclePolicy.GetType());
                Mapper.Map(policy, vehiclePolicy);
                //vehiclePolicy.CompanyVehicles = GetVehiclesByTemporalIdMassiveIdProductId(temporalId, listMassiveLoad, policy.Product.Id, withOutEvent);
                //if (vehiclePolicy.CompanyVehicles != null)
                //{
                //    foreach (CompanyVehicle risk in vehiclePolicy.CompanyVehicles)
                //    {
                //       risk.Coverages.ForEach(x => x.EndorsementType =  policy.Endorsement.EndorsementType);                      
                //    }
                //}
                return vehiclePolicy;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener riesgos de un temporal
        /// </summary>
        /// <param name="temporalId">Id temporal</param>
        /// <returns>Lista de riesgos</returns>
        public List<CompanyVehicle> GetVehiclesByTemporalIdMassiveIdProductId(int temporalId, List<int> listMassiveLoad, int productId, bool withOutEvent)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(TMPEN.TempRisk.Properties.TempId, typeof(TMPEN.TempRisk).Name);
            filter.Equal();
            filter.Constant(temporalId);


            if (listMassiveLoad.Count > 0)
            {
                filter.And();
                filter.Property(MSVEN.MassiveCollectiveDetail.Properties.IdMassive, typeof(MSVEN.MassiveCollectiveDetail).Name);
                filter.In();
                filter.ListValue();
                foreach (int massiveId in listMassiveLoad)
                {
                    filter.Constant(massiveId);
                }
                filter.EndList();
            }

            if (withOutEvent)
            {
                filter.And();
                filter.OpenParenthesis();
                filter.Property(MSVEN.MassiveCollectiveDetail.Properties.IsEvent, typeof(MSVEN.MassiveCollectiveDetail).Name);
                filter.IsNull();
                filter.Or();
                filter.Property(MSVEN.MassiveCollectiveDetail.Properties.IsEvent, typeof(MSVEN.MassiveCollectiveDetail).Name);
                filter.Equal();
                filter.Constant(false);
                filter.CloseParenthesis();
            }
   
            filter.And();
            filter.OpenParenthesis();
            filter.Property(MSVEN.MassiveCollectiveDetail.Properties.SnError, typeof(MSVEN.MassiveCollectiveDetail).Name);
            filter.IsNull();
            filter.Or();
            filter.Property(MSVEN.MassiveCollectiveDetail.Properties.SnError, typeof(MSVEN.MassiveCollectiveDetail).Name);
            filter.Equal();
            filter.Constant(false);
            filter.CloseParenthesis();

            COLEN.Views.TempRiskVehicleCollectiveView view = new COLEN.Views.TempRiskVehicleCollectiveView();
            ViewBuilder builder = new ViewBuilder("TempRiskVehicleCollectiveView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.TempRisks.Count > 0)
            {
                List<TMPEN.TempRisk> tempRisks = view.TempRisks.Cast<TMPEN.TempRisk>().ToList();
                List<TMPEN.CoTempRisk> coTempRisks = view.CoTempRisks.Cast<TMPEN.CoTempRisk>().ToList();
                List<TMPEN.TempRiskVehicle> tempRiskVehicles = view.TempRiskVehicles.Cast<TMPEN.TempRiskVehicle>().ToList();
                List<TMPEN.CoTempRiskVehicle> coTempRiskVehicles = view.CoTempRiskVehicles.Cast<TMPEN.CoTempRiskVehicle>().ToList();
                List<TMPEN.TempRiskBeneficiary> tempRiskBeneficiaries = view.TempRiskBeneficiaries.Cast<TMPEN.TempRiskBeneficiary>().ToList();
                List<TMPEN.TempRiskClause> tempRiskClauses = new List<TMPEN.TempRiskClause>();

                List<TMPEN.TempRiskCoverDetail> tempRiskCoverDetails = view.TempRiskCoverDetails.Cast<TMPEN.TempRiskCoverDetail>().ToList();
                List<TMPEN.TempRiskDetailAccessory> tempRiskDetailAccessories = view.TempRiskDetailAccessories.Cast<TMPEN.TempRiskDetailAccessory>().ToList();

                if (view.TempRiskClauses.Count > 0)
                {
                    tempRiskClauses = view.TempRiskClauses.Cast<TMPEN.TempRiskClause>().ToList();
                }

                List<CompanyVehicle> vehicles = new List<CompanyVehicle>();

                foreach (TMPEN.TempRisk item in tempRisks)
                {
                    CompanyVehicle vehicle = ModelAssembler.CreateTemporalVehicle(item,
                       coTempRisks.Where(x => x.TempId == item.TempId && x.RiskId == item.RiskId).First(),
                       tempRiskVehicles.Where(x => x.TempId == item.TempId && x.RiskId == item.RiskId).First(),
                       coTempRiskVehicles.Where(x => x.TempId == item.TempId && x.RiskId == item.RiskId).First());


                    vehicle.Beneficiaries = new List<Beneficiary>();
                    foreach (TMPEN.TempRiskBeneficiary tempRiskBeneficiary in tempRiskBeneficiaries.Where(x => x.TempId == item.TempId && x.RiskId == item.RiskId))
                    {
                        vehicle.Beneficiaries.Add(ModelAssembler.CreateTemporalBeneficiary(tempRiskBeneficiary));
                    }


                    vehicle.Clauses = new List<Clause>();
                    foreach (TMPEN.TempRiskClause tempRiskClause in tempRiskClauses.Where(x => x.TempId == item.TempId && x.RiskId == item.RiskId))
                    {
                        vehicle.Clauses.Add(ModelAssembler.CreateTempRiskClause(tempRiskClause));
                    }

                    vehicle.Coverages = DelegateService.underwritingService.GetCoveragesByTemporalIdRiskIdProductId(temporalId, item.RiskId, productId);


                    List<Accessory> accesories = new List<Accessory>();
                    foreach (TMPEN.TempRiskCoverDetail tempRiskCoverDetail in tempRiskCoverDetails.Where(x => x.RiskId == item.RiskId))
                    {
                        TMPEN.TempRiskDetailAccessory tempRiskDetailAccessory;
                        tempRiskDetailAccessory = tempRiskDetailAccessories.Where(x => x.RiskDetailId == tempRiskCoverDetail.RiskDetailId).FirstOrDefault();
                        Accessory accesory = ModelAssembler.CreateAccessoryTemp(tempRiskCoverDetail, tempRiskDetailAccessory);
                        accesories.Add(accesory);
                    }
                    vehicle.Accesories = accesories;

                    vehicles.Add(vehicle);

                }

                return vehicles;
            }
            else
            {
                return null;
            }
        }

    }
}
