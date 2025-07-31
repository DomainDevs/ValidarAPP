//Sistran FWK
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
//Sistran.Core
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using REINSURANCEEN = Sistran.Core.Application.Reinsurance.Entities;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Data;
using System;
using Sistran.Core.Framework.Views;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{
    /// <summary>
    /// </summary>
    internal class TempAllocationDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        private int RowsGrid = 200;

        #endregion

        /// <summary>
        /// UpdateTempAllocation
        /// </summary>
        /// <param name="reinsuranceAllocation"></param>
        public void UpdateTempAllocation(ReinsuranceAllocation reinsuranceAllocation)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSURANCEEN.TempReinsuranceAllocation.CreatePrimaryKey(reinsuranceAllocation.ReinsuranceAllocationId);

            // Encuentra el objeto en referencia a la llave primaria
            REINSURANCEEN.TempReinsuranceAllocation entityReinsuranceAllocation = (REINSURANCEEN.TempReinsuranceAllocation)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            if (entityReinsuranceAllocation != null)
            {
                entityReinsuranceAllocation.Amount = reinsuranceAllocation.Amount.Value;
                entityReinsuranceAllocation.Premium = reinsuranceAllocation.Premium.Value;
            }

            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().UpdateObject(entityReinsuranceAllocation);
        }

        #region Get
        /// <summary>
        /// GetTempAllocationById
        /// </summary>
        /// <param name="tempIssueAllocationId"></param>
        /// <returns>ReinsuranceAllocation</returns>
        public ReinsuranceAllocation GetTempAllocationById(int tempIssueAllocationId)
        {

            PrimaryKey primaryKey = REINSURANCEEN.TempReinsuranceAllocation.CreatePrimaryKey(tempIssueAllocationId);

            // Realizar las operaciones con los entities utilizando DAF
            REINSURANCEEN.TempReinsuranceAllocation entityTempReinsuranceAllocation = (REINSURANCEEN.TempReinsuranceAllocation)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

            if (entityTempReinsuranceAllocation != null)
            {
                // Return del model
                return ModelAssembler.CreateTempAllocation(entityTempReinsuranceAllocation);
            }
            else
            {
                return new ReinsuranceAllocation();
            }
        }

        /// <summary>
        /// GetTempAllocation
        /// Método que obtiene datos de la vista REINS.TMP_ISSUE_ALLOCATION
        /// </summary>
        /// <param name="tempLayerLineId"></param>
        /// <returns>ReinsuranceLine</returns>
        public ReinsuranceLine GetTempAllocation(int tempLayerLineId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            ReinsuranceLine reinsuranceLine = new ReinsuranceLine();

            criteriaBuilder.Property(REINSURANCEEN.GetIssTmpAllocation.Properties.TempReinsuranceLineId);
            criteriaBuilder.Equal();
            criteriaBuilder.Constant(tempLayerLineId);

            UIView allocations = _dataFacadeManager.GetDataFacade().GetView("GetIssTmpAllocation",
                                                         criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out RowsGrid);

            List<ReinsuranceAllocation> reinsuranceAllocations = new List<ReinsuranceAllocation>();

            foreach (DataRow allocationEntity in allocations.Rows)
            {
                // CABECERA
                reinsuranceLine.ReinsuranceLineId = Convert.ToInt32(allocationEntity["TempReinsuranceLineId"]);

                // DETALLE
                ReinsuranceAllocation reinsuranceAllocation = new ReinsuranceAllocation();
                reinsuranceAllocation.ReinsuranceAllocationId = Convert.ToInt32(allocationEntity["TempReinsuranceAllocationId"]);
                reinsuranceAllocation.Contract = new Contract();
                reinsuranceAllocation.Contract.SmallDescription = allocationEntity["SmallDescription"].ToString();
                reinsuranceAllocation.Contract.ContractLevels = new List<Level>();
                Level level = new Level();
                level.Number = Convert.ToInt32(allocationEntity["LevelNumber"]);
                reinsuranceAllocation.Contract.ContractLevels.Add(level);
                reinsuranceAllocation.Amount = new CommonService.Models.Amount();
                reinsuranceAllocation.Amount.Value = allocationEntity["SumAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(allocationEntity["SumAmount"]); // SUM_AMOUNT
                reinsuranceAllocation.Premium = new CommonService.Models.Amount();
                reinsuranceAllocation.Premium.Value = allocationEntity["SumPremium"] == DBNull.Value ? 0 : Convert.ToDecimal(allocationEntity["SumPremium"]); // SUM_PREMIUM
                reinsuranceAllocation.Facultative = Convert.ToBoolean(allocationEntity["IsFacultative"]);

                reinsuranceAllocations.Add(reinsuranceAllocation);
            }

            reinsuranceLine.ReinsuranceAllocations = new List<ReinsuranceAllocation>();

            reinsuranceLine.ReinsuranceAllocations = reinsuranceAllocations;

            return reinsuranceLine;
        }


        /// <summary>
        /// GetClaimTempAllocations
        /// </summary>
        /// <param name="processId"></param> 
        /// <returns>List<ReinsuranceLine/></returns>
        public List<ClaimAllocation> GetClaimTempAllocations(int processId, int movemenSourceId, int claimCoverageCd)
        {
            List<ClaimAllocation> claimAllocations = new List<ClaimAllocation>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSURANCEEN.GetClmTmpAllocation.Properties.TempReinsuranceProcessCode, processId).And();
            criteriaBuilder.PropertyEquals(REINSURANCEEN.GetClmTmpAllocation.Properties.MovementSourceCode, movemenSourceId).And();
            criteriaBuilder.PropertyEquals(REINSURANCEEN.GetClmTmpAllocation.Properties.ClaimCoverageCode, claimCoverageCd);

            UIView claimAllocationsView = _dataFacadeManager.GetDataFacade().GetView("GetClmTmpAllocation", criteriaBuilder.GetPredicate(), null, 0, 50, null, false, out RowsGrid);

            foreach (DataRow row in claimAllocationsView.Rows)
            {

                ClaimAllocation claimAllocation = new ClaimAllocation()
                {
                    TmpReinsuranceProcessId = Convert.ToInt32(row["TempReinsuranceProcessCode"]),
                    TmpClaimReinsSourceId = Convert.ToInt32(row["TempClaimReinsSourceId"]),
                    LineId = Convert.ToInt32(row["LineCode"]),
                    CumulusKey = row["CumulusKey"].ToString(),
                    LayerNumber = Convert.ToInt32(row["LayerNumber"]),
                    Amount = Convert.ToDecimal(row["Amount"]),
                    LevelNumber = Convert.ToInt32(row["LevelNumber"]),
                    Contract = row["Contract"].ToString(),
                    MovementSource = row["MovementSource"].ToString(),
                    MovementSourceId = Convert.ToInt32(row["MovementSourceCode"])
                };

                claimAllocations.Add(claimAllocation);

            }

            return claimAllocations;
        }
        #endregion
    }
}