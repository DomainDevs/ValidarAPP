using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.Location.PropertyServices.EEProvider.Views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.Location.PropertyServices.Models;
using Sistran.Core.Application.Location.PropertyServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.Enums;

namespace Sistran.Core.Application.Location.PropertyServices.EEProvider.DAOs
{
    public class PropertyDAO
    {
        public List<PropertyRisk> GetRiskPropertiesByAddress(string address)
        {
            List<PropertyRisk> propertyRisks = new List<PropertyRisk>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.RiskLocation.Properties.Street, typeof(ISSEN.RiskLocation).Name);
            filter.Like();
            filter.Constant(address + "%");

            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);

            ClaimRiskLocationView riskLocationView = new ClaimRiskLocationView();
            ViewBuilder viewBuilder = new ViewBuilder("ClaimRiskLocationView");
            viewBuilder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, riskLocationView);

            if (riskLocationView.RiskLocations.Count > 0)
            {
                List<ISSEN.EndorsementRisk> entityEndorsementRisks = riskLocationView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.Policy> entityPolicies = riskLocationView.Policies.Cast<ISSEN.Policy>().ToList();

                foreach (ISSEN.RiskLocation entityRiskLocation in riskLocationView.RiskLocations)
                {
                    PropertyRisk propertyRisk = ModelAssembler.CreateRiskLocation(entityRiskLocation);
                    ISSEN.Risk entityRisk = riskLocationView.Risks.Cast<ISSEN.Risk>().FirstOrDefault(x => x.RiskId == propertyRisk.Risk.RiskId);
                    COMMEN.City entityCity = riskLocationView.Cities.Cast<COMMEN.City>().FirstOrDefault(x => x.CityCode == propertyRisk.City.Id);
                    COMMEN.State entityState = riskLocationView.States.Cast<COMMEN.State>().FirstOrDefault(x => x.StateCode == propertyRisk.City.State.Id);
                    COMMEN.Country entityCountry = riskLocationView.Countries.Cast<COMMEN.Country>().FirstOrDefault(x => x.CountryCode == propertyRisk.City.State.Country.Id);
                    propertyRisk.Risk = new Risk
                    {
                        RiskId = entityRiskLocation.RiskId,
                        MainInsured = new IssuanceInsured
                        {
                            IndividualId = entityRisk.InsuredId
                        },
                        Number = Convert.ToInt32(entityEndorsementRisks.Where(X => X.RiskId == propertyRisk.Risk.RiskId).FirstOrDefault()?.RiskNum),
                        CoveredRiskType = (CoveredRiskType)entityRisk.CoveredRiskTypeCode,
                        Policy = new Policy
                        {
                            Id = entityEndorsementRisks.FirstOrDefault(X => X.RiskId == propertyRisk.Risk.RiskId).PolicyId,
                            Endorsement = new Endorsement
                            {
                                Id = entityEndorsementRisks.FirstOrDefault(X => X.RiskId == propertyRisk.Risk.RiskId).EndorsementId
                            }
                        },
                    };

                    propertyRisk.City.Description = entityCity?.Description;
                    propertyRisk.City.State.Description = entityState?.Description;
                    propertyRisk.City.State.Country.Description = entityCountry?.Description;

                    propertyRisk.Risk.Policy.DocumentNumber = entityPolicies.FirstOrDefault(X => X.PolicyId == propertyRisk.Risk.Policy.Id).DocumentNumber;

                    propertyRisks.Add(propertyRisk);
                }
            }

            return propertyRisks;
        }

        public List<PropertyRisk> GetRiskPropertiesByEndorsementId(int endorsementId)
        {
            List<PropertyRisk> propertyRisks = new List<PropertyRisk>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(ISSEN.EndorsementRisk.Properties.EndorsementId, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(endorsementId);
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.RiskStatusCode, typeof(ISSEN.EndorsementRisk).Name);
            filter.Distinct();
            filter.Constant(RiskStatusType.Excluded);

            ClaimRiskLocationView view = new ClaimRiskLocationView();
            ViewBuilder builder = new ViewBuilder("ClaimRiskLocationView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.Risks.Count > 0)
            {
                List<ISSEN.EndorsementRisk> entityEndorsementRisks = view.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.Policy> entityPolicies = view.Policies.Cast<ISSEN.Policy>().ToList();

                foreach (ISSEN.RiskLocation entityRiskLocation in view.RiskLocations)
                {
                    PropertyRisk propertyRisk = ModelAssembler.CreateRiskLocation(entityRiskLocation);
                    ISSEN.Risk entityRisk = view.Risks.Cast<ISSEN.Risk>().FirstOrDefault(x => x.RiskId == propertyRisk.Risk.RiskId);
                    COMMEN.City entityCity = view.Cities.Cast<COMMEN.City>().FirstOrDefault(x => x.CityCode == propertyRisk.City.Id);
                    COMMEN.State entityState = view.States.Cast<COMMEN.State>().FirstOrDefault(x => x.StateCode == propertyRisk.City.State.Id);
                    COMMEN.Country entityCountry = view.Countries.Cast<COMMEN.Country>().FirstOrDefault(x => x.CountryCode == propertyRisk.City.State.Country.Id);

                    ObjectCriteriaBuilder filterSumAssured = new ObjectCriteriaBuilder();
                    filterSumAssured.Property(ISSEN.EndorsementRiskCoverage.Properties.EndorsementId, typeof(ISSEN.EndorsementRiskCoverage).Name);
                    filterSumAssured.Equal();
                    filterSumAssured.Constant(endorsementId);
                    ClaimLocationSumAssuredView assuredView = new ClaimLocationSumAssuredView();
                    ViewBuilder builderAssured = new ViewBuilder("ClaimLocationSumAssuredView");
                    builderAssured.Filter = filterSumAssured.GetPredicate();
                    DataFacadeManager.Instance.GetDataFacade().FillView(builderAssured, assuredView);

                    decimal insuredAmount = 0;

                    foreach (ISSEN.RiskCoverage entityRiskCoverage in assuredView.RiskCoverages.Cast<ISSEN.RiskCoverage>().ToList())
                    {
                        insuredAmount += entityRiskCoverage.LimitAmount;
                    }

                    propertyRisk.Risk = new Risk
                    {
                        RiskId = entityRiskLocation.RiskId,
                        MainInsured = new IssuanceInsured
                        {
                            IndividualId = entityRisk.InsuredId
                        },
                        Number = Convert.ToInt32(entityEndorsementRisks.Where(X => X.RiskId == propertyRisk.Risk.RiskId).FirstOrDefault()?.RiskNum),
                        CoveredRiskType = (CoveredRiskType)entityRisk.CoveredRiskTypeCode,
                        Policy = new Policy
                        {
                            Id = entityEndorsementRisks.FirstOrDefault(X => X.RiskId == propertyRisk.Risk.RiskId).PolicyId,
                            Endorsement = new Endorsement
                            {
                                Id = entityEndorsementRisks.FirstOrDefault(X => X.RiskId == propertyRisk.Risk.RiskId).EndorsementId
                            }
                        },
                        AmountInsured = insuredAmount
                    };

                    propertyRisk.City.Description = entityCity?.Description;
                    propertyRisk.City.State.Description = entityState?.Description;
                    propertyRisk.City.State.Country.Description = entityCountry?.Description;

                    propertyRisk.Risk.Policy.DocumentNumber = entityPolicies.FirstOrDefault(X => X.PolicyId == propertyRisk.Risk.Policy.Id).DocumentNumber;

                    propertyRisks.Add(propertyRisk);
                }
            }

            return propertyRisks;
        }

        public List<PropertyRisk> GetRiskPropertiesByInsuredId(int insuredId)
        {
            List<PropertyRisk> propertyRisks = new List<PropertyRisk>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.Risk.Properties.InsuredId, typeof(ISSEN.Risk).Name);
            filter.Equal();
            filter.Constant(insuredId);
            
            filter.And();
            filter.Property(ISSEN.EndorsementRisk.Properties.IsCurrent, typeof(ISSEN.EndorsementRisk).Name);
            filter.Equal();
            filter.Constant(true);

            ClaimRiskLocationView riskLocationView = new ClaimRiskLocationView();
            ViewBuilder builder = new ViewBuilder("ClaimRiskLocationView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, riskLocationView);

            if (riskLocationView.RiskLocations.Count > 0)
            {
                List<ISSEN.EndorsementRisk> entityEndorsementRisks = riskLocationView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.Policy> entityPolicies = riskLocationView.Policies.Cast<ISSEN.Policy>().ToList();

                foreach (ISSEN.RiskLocation entityRiskLocation in riskLocationView.RiskLocations)
                {
                    PropertyRisk propertyRisk = ModelAssembler.CreateRiskLocation(entityRiskLocation);
                    ISSEN.Risk entityRisk = riskLocationView.Risks.Cast<ISSEN.Risk>().FirstOrDefault(x => x.RiskId == propertyRisk.Risk.RiskId);
                    COMMEN.City entityCity = riskLocationView.Cities.Cast<COMMEN.City>().FirstOrDefault(x => x.CityCode == propertyRisk.City.Id);
                    COMMEN.State entityState = riskLocationView.States.Cast<COMMEN.State>().FirstOrDefault(x => x.StateCode == propertyRisk.City.State.Id);
                    COMMEN.Country entityCountry = riskLocationView.Countries.Cast<COMMEN.Country>().FirstOrDefault(x => x.CountryCode == propertyRisk.City.State.Country.Id);
                    propertyRisk.Risk = new Risk
                    {
                        RiskId = entityRiskLocation.RiskId,
                        MainInsured = new IssuanceInsured
                        {
                            IndividualId = entityRisk.InsuredId
                        },
                        Number = Convert.ToInt32(entityEndorsementRisks.Where(X => X.RiskId == propertyRisk.Risk.RiskId).FirstOrDefault()?.RiskNum),
                        CoveredRiskType = (CoveredRiskType)entityRisk.CoveredRiskTypeCode,
                        Policy = new Policy
                        {
                            Id = entityEndorsementRisks.FirstOrDefault(X => X.RiskId == propertyRisk.Risk.RiskId).PolicyId,
                            Endorsement = new Endorsement
                            {
                                Id = entityEndorsementRisks.FirstOrDefault(X => X.RiskId == propertyRisk.Risk.RiskId).EndorsementId
                            }
                        },
                    };

                    propertyRisk.City.Description = entityCity?.Description;
                    propertyRisk.City.State.Description = entityState?.Description;
                    propertyRisk.City.State.Country.Description = entityCountry?.Description;

                    propertyRisk.Risk.Policy.DocumentNumber = entityPolicies.FirstOrDefault(X => X.PolicyId == propertyRisk.Risk.Policy.Id).DocumentNumber;

                    propertyRisks.Add(propertyRisk);
                }
            }

            return propertyRisks;
        }

        public PropertyRisk GetRiskPropertyByRiskId(int riskId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(ISSEN.RiskLocation.Properties.RiskId, typeof(ISSEN.RiskLocation).Name);
            filter.Equal();
            filter.Constant(riskId);

            ClaimRiskLocationView riskLocationView = new ClaimRiskLocationView();
            ViewBuilder builder = new ViewBuilder("ClaimRiskLocationView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, riskLocationView);

            if (riskLocationView.RiskLocations.Count > 0)
            {
                ISSEN.RiskLocation entityRiskLocation = riskLocationView.RiskLocations.Cast<ISSEN.RiskLocation>().First();
                List<ISSEN.EndorsementRisk> entityEndorsementRisks = riskLocationView.EndorsementRisks.Cast<ISSEN.EndorsementRisk>().ToList();
                List<ISSEN.Policy> entityPolicies = riskLocationView.Policies.Cast<ISSEN.Policy>().ToList();

                PropertyRisk propertyRisk = ModelAssembler.CreateRiskLocation(entityRiskLocation);
                ISSEN.Risk entityRisk = riskLocationView.Risks.Cast<ISSEN.Risk>().FirstOrDefault(x => x.RiskId == propertyRisk.Risk.RiskId);
                propertyRisk.Risk = new Risk
                {
                    RiskId = entityRiskLocation.RiskId,
                    MainInsured = new IssuanceInsured
                    {
                        IndividualId = entityRisk.InsuredId
                    },
                    Number = Convert.ToInt32(entityEndorsementRisks.Where(X => X.RiskId == propertyRisk.Risk.RiskId).FirstOrDefault()?.RiskNum),
                    CoveredRiskType = (CoveredRiskType)entityRisk.CoveredRiskTypeCode,
                    Policy = new Policy
                    {
                        Id = entityEndorsementRisks.FirstOrDefault(X => X.RiskId == propertyRisk.Risk.RiskId).PolicyId,
                        Endorsement = new Endorsement
                        {
                            Id = entityEndorsementRisks.FirstOrDefault(X => X.RiskId == propertyRisk.Risk.RiskId).EndorsementId
                        }
                    },
                };

                propertyRisk.Risk.Policy.DocumentNumber = entityPolicies.FirstOrDefault(X => X.PolicyId == propertyRisk.Risk.Policy.Id).DocumentNumber;

                return propertyRisk;
            }
            else
            {
                return null;
            }
        }
    }
}
