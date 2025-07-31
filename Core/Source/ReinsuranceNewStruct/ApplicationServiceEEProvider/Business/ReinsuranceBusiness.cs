using Sistran.Core.Application.ReinsuranceServices.Assemblers;
using Sistran.Core.Application.ReinsuranceServices.DTOs;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using Sistran.Core.Application.TempCommonServices.Models;
using Sistran.Core.Application.Utilities.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Business
{
    public class ReinsuranceBusiness
    {
        //***************************************************************************************************************************************
        //**   M � T O D O S P � B L I C O S D E P R O C E S O     R E A S E G U R O S
        //***************************************************************************************************************************************

        #region Par�metros_Contabilizaci�n        

        #endregion AccountingParameters

        //***************************************************************************************************************************************
        //        **   M � T O D O S P � B L I C O S D E C O N S U L T  A R E A S E  G U R O S
        //***************************************************************************************************************************************

        #region ConsultarReaseguro_Endoso
        /// <summary>            
        /// ValidateLineCumulusKeyWithLine
        /// Valida que el total por L�nea del l�mite de contrato sea >= que el valor de la L�nea C�mulo     
        /// </summary>
        /// <returns>bool</returns>
        public bool ValidateLineCumulusKeyWithLine(List<LineCumulusKey> lineCumulusKeys, List<LineDTO> lines)
        {

            bool isValid = false;
            decimal contractLimit = 0;
            List<LineContractLimit> lineContractLimits = new List<LineContractLimit>();

            //Totaliza L�mite de contrato por L�nea
            foreach (LineDTO line in lines)
            {
                foreach (ContractLineDTO contractLine in line.ContractLines)
                {
                    foreach (LevelDTO level in contractLine.Contract.ContractLevels)
                    {
                        contractLimit = contractLimit + Convert.ToDecimal(level.ContractLimit);
                    }
                }

                LineContractLimit lineContractLimit = new LineContractLimit();
                lineContractLimit.LineId = line.LineId;
                lineContractLimit.contractLimit = contractLimit;
                lineContractLimits.Add(lineContractLimit);
                contractLimit = 0;
            }

            //Compara L�neas totalizadas vs. L�neaC�mulo
            foreach (LineContractLimit lineContractLimit in lineContractLimits)
            {
                foreach (LineCumulusKey lineCumulusKey in lineCumulusKeys)
                {
                    if (lineContractLimit.LineId == lineCumulusKey.Line.LineId)
                    {
                        if (lineContractLimit.contractLimit >= lineCumulusKey.Amount.Value)
                        {
                            isValid = true;
                        }
                        else
                        {
                            isValid = false;
                        }
                    }
                }
            }
            return isValid;
        }


        #endregion
        
        public List<TempAllocation> GetReinsuranceAllocationBytempLayerLineId(int tempLayerLineId, ReinsuranceLineDTO reinsuranceLineDTO)
        {

            List<TempAllocation> tempAllocations = new List<TempAllocation>();
            decimal totSum = 0;
            decimal totPremium = 0;

            foreach (ReinsuranceAllocationDTO tempReinsuranceAllocation in reinsuranceLineDTO.ReinsuranceAllocations)
            {
                totSum = totSum + tempReinsuranceAllocation.Amount.Value;
                totPremium = totPremium + tempReinsuranceAllocation.Premium.Value;
            }

            foreach (ReinsuranceAllocationDTO reinsuranceAllocationDTO in reinsuranceLineDTO.ReinsuranceAllocations)
            {
                tempAllocations.Add(ModelAssembler.CreateTempAllocation(reinsuranceAllocationDTO, reinsuranceLineDTO, totSum, totPremium));
            }

            return tempAllocations;
        }

        public Contract AddContract(int contractId, int contractYear, ContractTypeDTO contractTypeDTO)
        {
            Contract contract = new Contract();
            if (contractId.Equals(0))
            {
                contract.ContractId = contractId;
                contract.ContractType = contractTypeDTO.ToModel();
                contract.Year = Convert.ToInt32(contractYear);
                contract.DateFrom = DateTime.Now;
                contract.DateTo = DateTime.Now.AddYears(1);
                contract.Enabled = true;
                contract.EstimatedDate = DateTime.Now;
            }
            return contract;
        }

        public List<Contract> GetContractsByYearAndContractTypeId(int year, int contractTypeId, List<ContractDTO> contractsDTO, ContractTypeDTO contractTypeDTO)
        {
            List<Contract> contracts = new List<Contract>();

            if (Convert.ToInt32(year) > 0 && Convert.ToInt32(contractTypeId) > 0)
            {
                foreach (ContractDTO contractDTO in contractsDTO)
                {
                    contractDTO.ContractType = contractTypeDTO;
                    contracts.Add(contractDTO.ToModel());
                }
            }
            return contracts;
        }

        public List<ContractType> GetContractTypeEnabled(List<ContractTypeDTO> contractTypes)
        {
            List<ContractType> contractTypesResult = new List<ContractType>();
            foreach (ContractTypeDTO contractType in contractTypes)
            {
                if (contractType.Enabled)
                {
                    contractTypesResult.Add(contractType.ToModel());
                }
            }
            return contractTypesResult;
        }

        public List<ContractType> GetContractFuncionalityId(int contractTypeId, List<ContractTypeDTO> contractTypes)
        {
            return contractTypes.Where(ct => ct.ContractTypeId == contractTypeId).ToModels().ToList();
        }

        public List<Contract> GetCurrentPeriodContracts(int year, List<ContractDTO> contracts)
        {
            return contracts.Where(ct => ct.Year >= year).ToModels().ToList();
        }

        public int ValidateBeforeDeleteContract(int contractId, List<LevelDTO> levelsDTO, List<ContractLineDTO> contractLinesDTO)
        {
            int result = 0;
            result = levelsDTO.Count;
            if (levelsDTO.Count == 0)
            {
                foreach (ContractLineDTO contractLineDTO in contractLinesDTO)
                {
                    if (contractLineDTO.Contract.ContractId == contractId)
                    {
                        result = 1;
                    }
                }
            }
            return result;
        }

        public int GetNextLevelNumberByLevelId(int levelId, List<LevelPaymentDTO> levelPaymentsDTO)
        {
            int levelPaymentNumber = 0;
            if (levelPaymentsDTO.Count != 0)
            {
                levelPaymentNumber = levelPaymentsDTO.Where(lp => lp.Level.ContractLevelId == levelId)
              .OrderByDescending(t => t.Number).FirstOrDefault().Number;
            }

            levelPaymentNumber = levelPaymentNumber + 1;
            return levelPaymentNumber;
        }

        public int GetNextNumberRestoreByLevelId(int levelId, List<LevelRestoreDTO> levelRestoresDTO)
        {
            int levelPaymentNumber = 0;
            if (levelRestoresDTO.Count != 0)
            {
                levelPaymentNumber = levelRestoresDTO.Where(lp => lp.Level.ContractLevelId == levelId)
               .OrderByDescending(t => t.Number).FirstOrDefault().Number;
            }

            levelPaymentNumber = levelPaymentNumber + 1;
            return levelPaymentNumber;
        }

        public Line AddLine(int lineId, List<ModuleDateDTO> moduleDates, LineDTO lineDTO)
        {
            Line line = new Line();
            if (lineId.Equals(0))
            {
                line.LineId = 0;
            }
            else
            {
                line.LineId = lineDTO.LineId;
                line.CumulusType = new CumulusType()
                {
                    CumulusTypeId = lineDTO.CumulusType.CumulusTypeId
                };
                line.Description = lineDTO.Description;
            }
            return line;
        }

        public Line AddContractLine(int contractLineId, int lineId, ContractLineDTO contractLineDTO, ContractDTO contractDTO)
        {
            Line line = new Line();
            line.LineId = lineId;
            line.ContractLines = new List<ContractLine>();
            contractLineDTO.Contract = new ContractDTO();
            contractLineDTO.Contract.ContractType = new ContractTypeDTO();

            if (!contractLineId.Equals(0))
            {
                contractLineDTO.Contract.Year = contractDTO.Year;
                contractLineDTO.Contract.ContractType = new ContractTypeDTO();
                contractLineDTO.Contract.ContractType.ContractTypeId = contractDTO.ContractType.ContractTypeId;
            }

            line.ContractLines.Add(contractLineDTO.ToModel());
            return line;
        }
        
        public List<TempLayerDistributions> GetTempLayerDistributionByEndorsementId(int endorsementId, List<ReinsuranceLayerIssuanceDTO> layerIssuancesDTOs)
        {
            List<TempLayerDistributions> result = new List<TempLayerDistributions>();

            foreach (ReinsuranceLayerIssuanceDTO layerIssuancesDTO in layerIssuancesDTOs)
            {
                result.Add(ModelAssembler.CreateTempLayerDistributionsByLayerIssuance(layerIssuancesDTO));
            }
            return result;
        }

        public List<TempLineCumulusIssuance> GetTempLineeCumulusByIssuance(int tempIssueLayerId, ReinsuranceLayerIssuanceDTO reinsuranceLineDTO)
        {
            List<TempLineCumulusIssuance> result = new List<TempLineCumulusIssuance>();
            foreach (ReinsuranceLineDTO tempReinsuranceLines in reinsuranceLineDTO.Lines)
            {
                result.Add(ModelAssembler.CreateTempLineCumulusIssuance(tempReinsuranceLines));
            }
            return result;
        }

        public List<SelectDTO> GetContractYear()
        {
            List<SelectDTO> selectDTO = new List<SelectDTO>();
            var numberPreviousYear = (int)ContractYear.QuantityContractYear - 2;

            for (int i = DateTime.Now.Year - numberPreviousYear; i <= DateTime.Now.Year + 1; i++)
            {
                selectDTO.Add(new SelectDTO { Id = i, Name = i.ToString() });
            }

            return selectDTO;
        }

        public LevelDTO AddContractLevel(int contractId, int contractLevelId, int number, ContractTypeDTO contractTypeDTO)
        {
            LevelDTO levelDTO = new LevelDTO();

            levelDTO.ContractLevelId = 0;
            levelDTO.Contract = new ContractDTO { ContractId = contractId };
            levelDTO.Number = number;
            levelDTO.RetentionLimit = 0;
            levelDTO.EventLimit = 0;
            levelDTO.Contract.ContractType = contractTypeDTO;

            if (levelDTO.Contract.ContractType.ContractTypeId == 6) // Retenci�n
            {
                levelDTO.LinesNumber = 0;
            }
            else if (levelDTO.Contract.ContractType.ContractTypeId == 2 || levelDTO.Contract.ContractType.ContractTypeId==4) // Excedente
            {
                levelDTO.LinesNumber = 1;
            }
            else
            {
                levelDTO.LinesNumber = null;
            }

            // Ocultar y grabar NULL para contratos Excedentes=2, Excesos de p�rdida=3 y Catastr�ficos=5 
            if (levelDTO.Contract.ContractType.ContractTypeId == 2 || levelDTO.Contract.ContractType.ContractTypeId == 3 || levelDTO.Contract.ContractType.ContractTypeId == 5 || levelDTO.Contract.ContractType.ContractTypeId == 6)
            {
                levelDTO.AssignmentPercentage = 0;
            }
            else
            {
                levelDTO.AssignmentPercentage = 100;
            }
            levelDTO.ContractLimit = 0;
            levelDTO.AdjustmentPercentage = 0;
            levelDTO.FixedRatePercentage = 0;
            levelDTO.MaximumRatePercentage = 0;
            levelDTO.MinimumRatePercentage = 0;
            levelDTO.LifeRate = 0;
            levelDTO.AnnualAddedLimit = 0;

            return levelDTO;
        }

        public List<int> ValidateBeforeDeleteContractLevel(int contractLevelId, List<LevelCompanyDTO> levelCompaniesDTOs)
        {
            List<int> result = new List<int>();
            int levelCompanyId = 0;
            int recordNumber = 0;

            recordNumber = levelCompaniesDTOs.Count;

            if (recordNumber > 0)
            {
                levelCompanyId = levelCompaniesDTOs[0].LevelCompanyId;
            }

            result.Add(recordNumber);
            result.Add(levelCompanyId);
            return result;
        }

        public List<LevelPayment> GetLevelPaymentsByLevelIdByLevelId(int levelId, List<LevelPaymentDTO> levelPaymentDTOs)
        {
            List<LevelPayment> levelPayments = new List<LevelPayment>();

            foreach (LevelPaymentDTO itemsTypes in levelPaymentDTOs)
            {
                levelPayments.Add(itemsTypes.ToModel());
            }

            return levelPayments;
        }

        public List<ContractLine> GetContractLineByLine(LineDTO lineDTO)
        {
            List<ContractLine> contractLines = new List<ContractLine>();

            foreach (ContractLineDTO contractLine in lineDTO.ContractLines)
            {
                contractLines.Add(contractLine.ToModel());
            }

            return contractLines;
        }

        public List<AssociationLine> GetAssociationLineByTypeLineYear(int year, int associationTypeId, int associationLineId, List<AssociationLineDTO> associationLineDTOs)
        {
            List<AssociationLine> associationLinesModel = new List<AssociationLine>();

            if (associationLineId == 0)
            {
                List<AssociationLineDTO> associationLinesDistinct = new List<AssociationLineDTO>();
                associationLinesDistinct = associationLineDTOs.DistinctBy(x => x.AssociationLineId).OrderBy(x => x.AssociationLineId).ToList();

                foreach (AssociationLineDTO associationLineDTO in associationLinesDistinct)
                {
                    associationLinesModel.Add(associationLineDTO.ToModel());
                }
            }
            else
            {
                foreach (AssociationLineDTO associationLineDTO in associationLineDTOs)
                {
                    associationLinesModel.Add(associationLineDTO.ToModel());
                }
            }

            return associationLinesModel;
        }

        #region Process Controller
        public List<ReinsuranceAllocation> GetTempAllocationByLayerLineId(int tempLayerLineId, ReinsuranceLine reinsuranceLine)
        {
            List<ReinsuranceAllocation> reinsuranceAllocations = new List<ReinsuranceAllocation>();
            reinsuranceAllocations = reinsuranceLine.ReinsuranceAllocations;

            reinsuranceAllocations.ForEach(
              x =>
              {
                  x.TotalSum = reinsuranceAllocations.Sum(y => y.Amount.Value);
                  x.TotalPremium = reinsuranceAllocations.Sum(y => y.Premium.Value);
              }
            );

            return reinsuranceAllocations;
        }

        public List<ReinsuranceAllocation> GetTotSumPrimeAllocation(int tempLayerLineId, ReinsuranceLine reinsuranceLine)
        {
            List<ReinsuranceAllocation> reinsuranceAllocations = new List<ReinsuranceAllocation>();
            reinsuranceAllocations = reinsuranceLine.ReinsuranceAllocations;

            reinsuranceAllocations.ForEach(
              x =>
              {
                  x.TotalSum = reinsuranceAllocations.Sum(y => y.Amount.Value);
                  x.TotalPremium = reinsuranceAllocations.Sum(y => y.Premium.Value);
              }
            );

            return reinsuranceAllocations;
        }

        public ReinsuranceAllocation ModificationReinsuranceContractDialog(int tempIssueAllocationId, ReinsuranceAllocation reinsuranceAllocation)
        {
            ReinsuranceAllocation result = new ReinsuranceAllocation();
            result.TotalSum = reinsuranceAllocation.Amount.Value;
            result.TotalPremium = reinsuranceAllocation.Premium.Value;
            result.ReinsuranceAllocationId = reinsuranceAllocation.ReinsuranceAllocationId;
            return result;
        }

        public List<TempReinsuranceProcess> GetTempReinsuranceProcessByProcessId(int tempReinsuranceProcessId, TempReinsuranceProcessDTO tempReinsuranceProcessDTO)
        {
            List<TempReinsuranceProcess> tempReinsuranceProcess = new List<TempReinsuranceProcess>();
            if (tempReinsuranceProcessDTO.TempReinsuranceProcessId > 0)
            {
                double progress;
                progress = (Convert.ToDouble(tempReinsuranceProcessDTO.RecordsProcessed) + Convert.ToDouble(tempReinsuranceProcessDTO.RecordsFailed)) / Convert.ToDouble(tempReinsuranceProcessDTO.RecordsNumber);               
                double elapsed = Math.Round((DateTime.Now.TimeOfDay.TotalHours - tempReinsuranceProcessDTO.StartDate.TimeOfDay.TotalHours), 2);
                double minElapsed = elapsed - Math.Truncate(elapsed);
                minElapsed = minElapsed * 60;
                tempReinsuranceProcess.Add(ModelAssembler.CreateTempReinsuranceProcess(tempReinsuranceProcessDTO, progress, elapsed, minElapsed));
            }
            return tempReinsuranceProcess;
        }
        
        public List<PlanFacultativeDTO> GetTempFacultativePayment(int levelCompanyId, List<InstallmentDTO> installmentDTOs)
        {
            List<PlanFacultativeDTO> planFacultativeDTO = new List<PlanFacultativeDTO>();
            foreach (InstallmentDTO installmentDTO in installmentDTOs)
            {
                planFacultativeDTO.Add(new PlanFacultativeDTO
                {
                    FacultativePaymentsId = installmentDTO.Id,
                    QuotaNumber = installmentDTO.InstallmentNumber,
                    DueDate = installmentDTO.PaidDate.ToString("dd/MM/yyyy"),
                    Amount = installmentDTO.PaidAmount,
                    TempCompanyId = installmentDTO.LevelCompany.LevelCompanyId
                });
            }
            return planFacultativeDTO;
        }
        
        public List<TempReinsuranceProcess> LoadProcessMassiveDetailsReport(int tempReinsuranceProcessId, List<TempReinsuranceProcessDTO> tempReinsuranceProcessDTOs)
        {
            List<TempReinsuranceProcess> tempReinsuranceProcesses = new List<TempReinsuranceProcess>();
            foreach (TempReinsuranceProcessDTO tempReinsuranceProcessDTO in tempReinsuranceProcessDTOs)
            {
                tempReinsuranceProcesses.Add(tempReinsuranceProcessDTO.ToModel());
            }
            return tempReinsuranceProcesses;
        }
        #endregion
    }
}