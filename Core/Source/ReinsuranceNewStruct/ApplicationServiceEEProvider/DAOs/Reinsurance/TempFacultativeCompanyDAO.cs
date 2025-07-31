using System;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using REINSEN = Sistran.Core.Application.Reinsurance.Entities;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using Sistran.Co.Application.Data;
using System.Data;
using System.Collections.Generic;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Views;
using System.Linq;
using System.Globalization;
using System.Configuration;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{
    /// <summary>
    /// </summary>
    internal class TempFacultativeCompanyDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        private int RowsGrid = 200;

        #endregion

        #region Save

        /// <summary>
        /// SaveTempFacultativeCompany 
        /// </summary>
        /// <param name="reinsuranceFacultative"></param>
        public void SaveTempFacultativeCompany(ReinsuranceAllocation reinsuranceFacultative)
        {
            // Convertir de model a entity
            REINSEN.TempInstallmentCompany entityTempInstallmentCompany = EntityAssembler.CreateTempFacultativeCompany(reinsuranceFacultative);

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().InsertObject(entityTempInstallmentCompany);
        }

        #endregion Save

        #region Update

        /// <summary>
        /// UpdateTempFacultativeCompany
        /// </summary>
        /// <param name="reinsuranceFacultative"></param>
        public void UpdateTempFacultativeCompany(ReinsuranceAllocation reinsuranceFacultative)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSEN.TempInstallmentCompany.CreatePrimaryKey(reinsuranceFacultative.ReinsuranceAllocationId);

            // Encuentra el objeto en referencia a la llave primaria
            REINSEN.TempInstallmentCompany entityTempInstallmentCompany = (REINSEN.TempInstallmentCompany)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            entityTempInstallmentCompany.BrokerReinsuranceCode = reinsuranceFacultative.Contract.ContractLevels[0].ContractLevelCompanies[0].Agent.IndividualId;
            entityTempInstallmentCompany.ReinsuranceCompanyCode = reinsuranceFacultative.Contract.ContractLevels[0].ContractLevelCompanies[0].Company.IndividualId;
            entityTempInstallmentCompany.CommissionPercentage = Convert.ToDecimal(reinsuranceFacultative.Contract.ContractLevels[0].ContractLevelCompanies[0].ComissionPercentage);
            entityTempInstallmentCompany.PremiumPercentage = Convert.ToDecimal(reinsuranceFacultative.Contract.ContractLevels[0].ContractLevelCompanies[0].GivenPercentage);
            entityTempInstallmentCompany.ParticipationPercentage = Convert.ToDecimal(reinsuranceFacultative.Contract.ContractLevels[0].AssignmentPercentage);

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().UpdateObject(entityTempInstallmentCompany);
        }

        #endregion Update

        /// <summary>
        /// LoadFacultative
        /// Carga facultativo
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="layerNumber"></param>
        /// <param name="lineId"></param>
        /// <param name="cumulusKey"></param>
        /// <param name="description"></param>
        /// <param name="sumPercentage"></param>
        /// <param name="premiumPercentage"></param>
        /// <param name="userId"></param>
        /// <returns>int</returns>
        public int LoadFacultative(int processId, int? layerNumber, int? lineId, string cumulusKey,
                                    string description, decimal sumPercentage, decimal premiumPercentage, int userId)
        {
            NameValue[] parameters = new NameValue[8];

            parameters[0] = new NameValue("PROCESS_ID", processId);
            parameters[1] = new NameValue("LAYER_NUMBER", layerNumber);
            parameters[2] = new NameValue("LINE_ID", lineId);
            if (String.IsNullOrEmpty(cumulusKey))
            {
                parameters[3] = new NameValue("CUMULUS_KEY", ""); //DBNull.Value   se cambia por sybase
            }
            else
            {
                parameters[3] = new NameValue("CUMULUS_KEY", cumulusKey);
            }
            parameters[4] = new NameValue("COMMENTS", description);

            parameters[5] = new NameValue("FACULTATIVE_PERCENTAGE", sumPercentage);
            parameters[6] = new NameValue("PREMIUM_PERCENTAGE", premiumPercentage);
            parameters[7] = new NameValue("USER_ID", userId);
            DataTable facultatives;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                facultatives = dynamicDataAccess.ExecuteSPDataTable("REINS.ISS_INSERT_FACULTATIVE", parameters);
            }

            int facultativeId = 0;

            foreach (DataRow facultative in facultatives.Rows)
            {
                facultativeId = int.Parse(facultative[0].ToString()); // El Id del Facultativo
            }

            return facultativeId;
        }

        /// <summary>
        /// CalculationValue
        /// Recupera valores de suma y prima para el facultativo
        /// </summary>
        /// <param name="processId"></param>
        /// <param name="layerNumber"></param>
        /// <param name="lineId"></param>
        /// <param name="cumulusKey"></param>
        /// <returns>decimal[]</returns>
        public decimal[] CalculationValue(int processId, int? layerNumber, int? lineId, string cumulusKey)
        {
            decimal[] values = new decimal[2];

            NameValue[] parameters = new NameValue[4];

            parameters[0] = new NameValue("PROCESS_ID", processId);
            parameters[1] = new NameValue("LAYER_NUMBER", Convert.ToInt32(layerNumber));
            parameters[2] = new NameValue("LINE_ID", Convert.ToInt32(lineId));
            if (String.IsNullOrEmpty(cumulusKey))
            {
                parameters[3] = new NameValue("CUMULUS_KEY", ""); //DBNull.Value  se cambia por sybase
            }
            else
            {
                parameters[3] = new NameValue("CUMULUS_KEY", cumulusKey);
            }
            DataTable calculationValues;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                calculationValues = dynamicDataAccess.ExecuteSPDataTable("REINS.ISSUE_CALCULATION_VALUE", parameters);
            }

            foreach (DataRow calculationValue in calculationValues.Rows)
            {
                values[0] = Convert.ToDecimal(calculationValue[0]); //sum
                values[1] = Convert.ToDecimal(calculationValue[1]); //prime
            }

            return values;
        }
        
        public List<ReinsuranceLayerIssuance> GetTempFacultativeCompanies(int endorsementId, int layerNumber, int? lineId, string cumulusKey)
        {
            List<ReinsuranceLayerIssuance> reinsuranceLayerIssuances = new List<ReinsuranceLayerIssuance>();
            List<ReinsuranceAllocation> reinsuranceAllocations;
            CultureInfo cultureInfo = new CultureInfo(ConfigurationManager.AppSettings["DefaultCultureInfo"].ToString());
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.GetIssTmpFacultativeCompany.Properties.EndorsementId, endorsementId);

            if (layerNumber != 0)
            {
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(REINSEN.GetIssTmpFacultativeCompany.Properties.LayerNumber, layerNumber);
            }

            if ((lineId != 0) && (cumulusKey != ""))
            {
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(REINSEN.GetIssTmpFacultativeCompany.Properties.LineId, lineId);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(REINSEN.GetIssTmpFacultativeCompany.Properties.CumulusKey, cumulusKey);
            }

            UIView getIssTmpFacultativeCompany = _dataFacadeManager.GetDataFacade().GetView("GetIssTmpFacultativeCompany", criteriaBuilder.GetPredicate(), null, 0, 50, null, true, out RowsGrid);

            foreach (DataRow row in getIssTmpFacultativeCompany.Rows)
            {
                ReinsuranceLine reinsuranceLine = new ReinsuranceLine()
                {
                    ReinsuranceAllocations = new List<ReinsuranceAllocation>()
                };

                reinsuranceAllocations = new List<ReinsuranceAllocation>();

                ReinsuranceLayerIssuance reinsuranceLayerIssuance = new ReinsuranceLayerIssuance();
                reinsuranceLayerIssuance.ReinsuranceLayerId = Convert.ToInt32(row["TempInstallementCompanyId"]);
                reinsuranceLayerIssuance.TemporaryIssueId = Convert.ToInt32(row["TempFacultativeCode"]);
                reinsuranceLayerIssuance.LayerPercentage = Convert.ToDecimal(row["ParticipationPercentage"] == DBNull.Value ? 0 : (float)Convert.ToDouble(row["ParticipationPercentage"]));
                reinsuranceLayerIssuance.LayerAmount = Convert.ToDecimal(row["SumValueParticipation"] == DBNull.Value ? 0 : Convert.ToDecimal(row["SumValueParticipation"]));
                reinsuranceLayerIssuance.PremiumPercentage = Convert.ToDecimal(row["PremiumPercentage"] == DBNull.Value ? 0 : (float)Convert.ToDouble(row["PremiumPercentage"]));
                reinsuranceLayerIssuance.PremiumAmount = Convert.ToDecimal(row["SumValuePremium"] == DBNull.Value ? 0 : Convert.ToDecimal(row["SumValuePremium"]));
                
                LevelCompany levelCompany = new LevelCompany();
                levelCompany.Company = new Company();
                levelCompany.Company.IndividualId = Convert.ToInt32(row["ReinsuranceCompanyCode"]);
                levelCompany.Company.FullName = row["DescriptionCompany"].ToString();
                levelCompany.ComissionPercentage = Convert.ToDecimal(row["CommissionPercentage"]);
                levelCompany.Agent = new Agent();
                levelCompany.Agent.IndividualId = Convert.ToInt32(row["BrokerReinsuranceId"]);
                levelCompany.Agent.FullName = row["DescriptionBroker"].ToString();
                levelCompany.DepositPercentage = Convert.ToDecimal(row["DepositPercentage"]);
                levelCompany.InterestOnReserve = Convert.ToDecimal(row["InterestOnReservePercentage"]);
                levelCompany.DepositReleaseDate = row["DepositReleaseDate"].ToString() == "" ? DateTime.Now : Convert.ToDateTime(Convert.ToDateTime(row["DepositReleaseDate"], cultureInfo));

                Level level = new Level();
                level.ContractLevelCompanies = new List<LevelCompany>();
                level.ContractLevelCompanies.Add(levelCompany);


                ReinsuranceAllocation reinsuranceAllocation = new ReinsuranceAllocation();
                reinsuranceAllocation.Contract = new Contract();
                reinsuranceAllocation.Contract.Description = row["Comments"].ToString();
                reinsuranceAllocation.Contract.FacultativePercentage = Convert.ToDecimal(row["FacultativePercentage"]);
                reinsuranceAllocation.Contract.FacultativePremiumPercentage = Convert.ToDecimal(row["FacultativePremiumPercentage"]);
                reinsuranceAllocation.Contract.ContractLevels = new List<Level>();
                reinsuranceAllocation.Contract.ContractLevels.Add(level);


                reinsuranceAllocations.Add(reinsuranceAllocation);
                reinsuranceLine.ReinsuranceAllocations.Add(reinsuranceAllocation);

                reinsuranceLayerIssuance.Lines = new List<ReinsuranceLine>();
                reinsuranceLayerIssuance.Lines.Add(reinsuranceLine);
                reinsuranceLayerIssuances.Add(reinsuranceLayerIssuance);
            }

            return reinsuranceLayerIssuances;
        }

        public int GetReinsuranceCompanyIdByFacultativeIdAndIndividualId(int facultativeId, int individualId)
        {
            int reinsuranceCompanyId = 0;

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.TempInstallmentCompany.Properties.TempFacultativeCode, facultativeId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(REINSEN.TempInstallmentCompany.Properties.ReinsuranceCompanyCode, individualId);
            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(REINSEN.TempInstallmentCompany), criteriaBuilder.GetPredicate()));

            foreach (REINSEN.TempInstallmentCompany tempInstallmentCompany in businessCollection.OfType<REINSEN.TempInstallmentCompany>())
            {
                reinsuranceCompanyId = Convert.ToInt32(tempInstallmentCompany.ReinsuranceCompanyCode);
            }

            return reinsuranceCompanyId;
        }

        public List<Slip> GetSlips(int processId, int endorsementId)
        {
            NameValue[] parameters = new NameValue[2];
            List<Slip> slipDTOs = new List<Slip>();
            parameters[0] = new NameValue("ENDORSEMENT_ID", endorsementId);
            parameters[1] = new NameValue("PROCESS_ID", processId);
            DataTable slips;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                slips = dynamicDataAccess.ExecuteSPDataTable("REINS.ISS_GET_SLIPS", parameters);
            }
            foreach (DataRow slip in slips.Rows)
            {
                Slip slipDto = new Slip
                {
                    FacultativeId  = int.Parse(slip[0].ToString()),
                    SlipNumber = slip[1].ToString()
                    };
                slipDTOs.Add(slipDto);
            }

            return slipDTOs;
        }
        public int ExpandFacultative(int processId, int endorsementId, int layerNumber, int facultativeId)
        {
            NameValue[] parameters = new NameValue[4];
            parameters[0] = new NameValue("PROCESS_ID", processId);
            parameters[1] = new NameValue("ENDORSEMENT_ID", endorsementId);
            parameters[2] = new NameValue("LAYER_NUMBER", layerNumber);
            parameters[3] = new NameValue("FACULTATIVE_ID", facultativeId);
           
            DataTable ExpandFacultatives;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                ExpandFacultatives = dynamicDataAccess.ExecuteSPDataTable("REINS.ISS_EXPAND_FACULTATIVE", parameters);
            }
            int expandFacultative = 0;

            foreach (DataRow row in ExpandFacultatives.Rows)
            {
                expandFacultative = int.Parse(row[0].ToString()); // El Id del Facultativo
            }
            return expandFacultative;
        }
    }
}