using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using REINSEN = Sistran.Core.Application.Reinsurance.Entities;
using System.Linq;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Framework.Views;
using System.Data;
using System.Globalization;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{
    internal class DistributionDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        private int RowsGrid = 200;

        #endregion

        #region Get

        /// <summary>
        /// GetDistributionByReinsurance
        /// </summary>
        /// <param name="layerId"></param>
        /// <returns>List<ReinsuranceDistribution/></returns>
        public List<ReinsuranceDistribution> GetDistributionByReinsurance(int layerId)
        {
            List<ReinsuranceDistribution> reinsuranceDistributions = new List<ReinsuranceDistribution>();

            ObjectCriteriaBuilder criteriaBuilderRd = new ObjectCriteriaBuilder();
            criteriaBuilderRd.PropertyEquals(REINSEN.IssueLayerLine.Properties.IssueLayerId, layerId);
            criteriaBuilderRd.And();
            criteriaBuilderRd.PropertyEquals(REINSEN.IssueAllocation.Properties.IsFacultative, 0);

            UIView distributions = _dataFacadeManager.GetDataFacade().GetView("GetReinsuranceDistribution",
                            criteriaBuilderRd.GetPredicate(), null, 0, -1, null, false, out RowsGrid);
            bool isfacultative = false;

            foreach (DataRow distribution in distributions)
            {
                ReinsuranceDistribution reinsuranceDistributionDTO = ReinsuranceDistributions(distribution, isfacultative);
                reinsuranceDistributions.Add(reinsuranceDistributionDTO);
            }

            /*Consulta Facultativos*/
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.IssueLayerLine.Properties.IssueLayerId, layerId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(REINSEN.IssueAllocation.Properties.IsFacultative, 1);

            UIView distributionsFacultative = _dataFacadeManager.GetDataFacade().GetView("GetReinsuranceFacultativeDistribution",
                            criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out RowsGrid);

            isfacultative = true;

            foreach (DataRow distribution in distributionsFacultative)
            {
                ReinsuranceDistribution reinsuranceDistributionDTO = ReinsuranceDistributions(distribution, isfacultative);
                reinsuranceDistributions.Add(reinsuranceDistributionDTO);
            }

            return reinsuranceDistributions;
        }


        /// <summary>
        /// GetReinsuranceDistributionHeaders
        /// Método que obtiene datos de una vista que representa 
        /// los Reaseguros de Emisión dado el endoso
        /// </summary>
        /// <param name = "endorsementId" > Clave primaria del endoso</param>
        /// <returns>List<ReinsuranceDistributionHeaderDTO/></returns>
        public List<ReinsuranceDistributionHeader> GetReinsuranceDistributionHeaders(int? endorsementId)
        {
            List<ReinsuranceDistributionHeader> reinsuranceDistributionHeader = new List<ReinsuranceDistributionHeader>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.Property(REINSEN.GetIssDistribution.Properties.EndorsementId);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(endorsementId);

            UIView distributions = _dataFacadeManager.GetDataFacade().GetView("GetIssDistribution", criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out RowsGrid);

            foreach (DataRow reinsuranceRow in distributions.Rows)
            {
                ReinsuranceDistributionHeader reinsuranceDistribution = new ReinsuranceDistributionHeader();
                reinsuranceDistribution.EndorsementId = Convert.ToInt32(endorsementId);
                reinsuranceDistribution.LayerNumber = Convert.ToInt32(reinsuranceRow["LayerNumber"]);
                reinsuranceDistribution.LayerPercentage = Convert.ToDecimal((reinsuranceRow["LayerPercentage"]));
                reinsuranceDistribution.Line = reinsuranceRow["LineDescription"].ToString();
                reinsuranceDistribution.CumulusKey = reinsuranceRow["CumulusKey"].ToString();

                reinsuranceDistribution.ContractId = Convert.ToInt32(reinsuranceRow["ContractId"]);
                reinsuranceDistribution.ContractDescription = reinsuranceRow["ContractDescription"].ToString();

                reinsuranceDistribution.LevelNumber = Convert.ToInt32(reinsuranceRow["LevelNumber"]);
                reinsuranceDistribution.TradeName = reinsuranceRow["TradeName"].ToString();
                reinsuranceDistribution.PremiumSum = (reinsuranceRow["PremiumSum"] == DBNull.Value ? 0 : Convert.ToDecimal(reinsuranceRow["PremiumSum"])).ToString();
                reinsuranceDistribution.AmountSum = (reinsuranceRow["AmountSum"] == DBNull.Value ? 0 : Convert.ToDecimal(reinsuranceRow["AmountSum"])).ToString();

                reinsuranceDistributionHeader.Add(reinsuranceDistribution);
            }

            return reinsuranceDistributionHeader;
        }


        /// <summary>
        /// GetDistributionErrors
        /// Método que trae información cuando se produce un error 
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns>List<IssGetDistributionErrorsDTO/></returns>
        public List<IssGetDistributionErrors> GetDistributionErrors(int endorsementId)
        {
            List<IssGetDistributionErrors> distributionErrors = new List<IssGetDistributionErrors>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.PropertyEquals(REINSEN.GetDistributionErrors.Properties.EndorsementCode, endorsementId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(REINSEN.GetDistributionErrors.Properties.ModuleCode, Modules.Issuance);
            UIView distributionErrorsCollection = _dataFacadeManager.GetDataFacade().GetView("GetDistributionErrors",
                        criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out RowsGrid);

            foreach (DataRow distributionErrorsEntity in distributionErrorsCollection)
            {
                distributionErrors.Add(new IssGetDistributionErrors
                {
                    RiskNumber = Convert.ToInt32(distributionErrorsEntity["RiskNumber"]),
                    CoverageNumber = Convert.ToInt32(distributionErrorsEntity["CoverageNumber"]),
                    Description = distributionErrorsEntity["ErrorDescription"].ToString()
                });
            }

            List<IssGetDistributionErrors> distributionErrorsOrder = (from order in distributionErrors
                                                                      orderby
                                     order.RiskNumber, order.CoverageNumber
                                                                      select order).ToList();

            return distributionErrorsOrder;
        }

        public List<ReinsuranceDistributionHeader> GetReinsuranceDistributionByEndorsementId(int endorsementId)
        {
            List<ReinsuranceDistributionHeader> reinsuranceDistributionHeader = new List<ReinsuranceDistributionHeader>();
            reinsuranceDistributionHeader = GetReinsuranceDistributionHeaders(Convert.ToInt32(endorsementId));
            IEnumerable<ReinsuranceDistributionHeader> reinsurances = from reins in reinsuranceDistributionHeader
                                                                      orderby reins.LayerNumber, reins.CumulusKey, reins.LevelNumber, reins.ContractId ascending
                                                                      select reins;

            return reinsurances.ToList();
        }

        /// <summary>
        /// GetTempLayerDistribution
        /// Método que carga las capas Reaseguros provenientes de la vista
        /// REINS.GET_ISS_TMP_LAYER_DISTRIBUTION
        /// </summary>
        /// <param name="endorsementId"></param>
        /// <returns>List<ReinsuranceLayerIssuance/></returns>
        public List<ReinsuranceLayerIssuance> GetTempLayerDistribution(int endorsementId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            List<ReinsuranceLayerIssuance> reinsuranceLayerIssuances = new List<ReinsuranceLayerIssuance>();

            criteriaBuilder.Property(REINSEN.GetIssTmpLayerDistribution.Properties.EndorsementCode);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(endorsementId);

            UIView levelDistributions = _dataFacadeManager.GetDataFacade().GetView("GetIssTmpLayerDistribution", criteriaBuilder.GetPredicate(),
                                                                  null, 0, 50, null, true, out RowsGrid);
            foreach (DataRow reinsuranceLevelEntity in levelDistributions.Rows)
            {
                decimal layerPercentage = Convert.ToDecimal(reinsuranceLevelEntity["LayerPercentage"]);
                decimal layerAmount = Convert.ToDecimal(reinsuranceLevelEntity["SumAmount"]);

                decimal premiumPercentage = Convert.ToDecimal(reinsuranceLevelEntity["PremiumPercentage"]);
                decimal premiumAmount = Convert.ToDecimal(reinsuranceLevelEntity["SumPremium"]);

                reinsuranceLayerIssuances.Add(new ReinsuranceLayerIssuance
                {
                    TemporaryIssueId = Convert.ToInt32(reinsuranceLevelEntity["TempReinsuranceProcessCode"]),
                    LayerNumber = Convert.ToInt32(reinsuranceLevelEntity["LayerNumber"]),
                    ReinsuranceLayerId = Convert.ToInt32(reinsuranceLevelEntity["TempReinsLayerIssuanceId"]),
                    LayerPercentage = layerPercentage,
                    LayerAmount = layerAmount,
                    PremiumPercentage = premiumPercentage,
                    PremiumAmount = premiumAmount
                });
            }

            return reinsuranceLayerIssuances;
        }
        #endregion

        public bool ValidateEndorsementReinsurance(int endorsementId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(REINSEN.GetIssDistribution.Properties.EndorsementId);
            filter.Equal();
            filter.Constant(endorsementId);

            return _dataFacadeManager.GetDataFacade().GetView("GetIssDistribution", filter.GetPredicate(), null, 0, -1, null, true, out RowsGrid).Rows.Count > 0;
        }

        /// <summary>
        /// ReinsuranceDistributions
        /// </summary>
        /// <param name="distribution"></param>
        /// <param name="isfacultative"></param>
        /// <returns>ReinsuranceDistribution</returns>
        private ReinsuranceDistribution ReinsuranceDistributions(DataRow distribution, bool isfacultative)
        {
            ReinsuranceDistribution reinsuranceDistributions = new ReinsuranceDistribution()
            {
                IssueLayerId = Convert.ToInt32(distribution["IssueLayerId"]),
                Line = distribution["Line"].ToString(),
                CumulusKey = distribution["CumulusKey"].ToString(),
                IsFacultative = Convert.ToString(distribution["IsFacultative"]).ToUpper() == "TRUE" ? "SI" : "NO",
                Description = distribution["CurrencyDescription"].ToString(),
                Amount = distribution["Amount"].ToString(),
                Premium = distribution["Premium"].ToString(),
                Commission = distribution["Commission"].ToString(),
                Contract = isfacultative ? "F" + String.Format("{0:D9}", Convert.ToInt32(distribution["Contract"])) : distribution["Contract"].ToString(),
                LevelNumber = isfacultative ? 1 : Convert.ToInt32(distribution["LevelNumber"]),
                Broker = String.IsNullOrEmpty(distribution["Broker"].ToString()) ? "" : distribution["Broker"].ToString(),
                Reinsurer = String.IsNullOrEmpty(distribution["Reinsurer"].ToString()) ? "" : distribution["Reinsurer"].ToString(),
                ParticipationPercentage = distribution["ParticipationPercentage"].ToString()
            };
            return reinsuranceDistributions;
        }

    }
}
