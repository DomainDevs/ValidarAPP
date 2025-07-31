using System;
using REINSEN = Sistran.Core.Application.Reinsurance.Entities;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers
{
    internal static class EntityAssembler
    {
        #region Contract

        #region Contract

        /// <summary>
        /// CreateContract
        /// </summary>
        /// <param name="contract"></param>
        /// <returns>Contract</returns>
        public static REINSEN.Contract CreateContract(Contract contract)
        {
            DateTime? estimated = null;

            if (contract.EstimatedDate != Convert.ToDateTime("01/01/0001"))
            {
                estimated = contract.EstimatedDate;
            }

            return new REINSEN.Contract(contract.ContractId)
            {
                ContractId = contract.ContractId,
                ContractTypeId = contract.ContractType.ContractTypeId,
                CurrencyId = contract.Currency.Id,
                DateFrom = contract.DateFrom,
                DateTo = contract.DateTo,
                Description = contract.Description,
                SmallDescription = contract.SmallDescription,
                Year = contract.Year,
                ReleaseTimeReserve = contract.ReleaseTimeReserve,
                AffectationTypeCode = contract.AffectationType.Id,
                ReestablishmentTypeCode = contract.ResettlementType.Id,
                Epi = contract.PremiumAmount,
                EpiTypeCode = contract.EPIType.Id,
                Status = contract.Enabled,
                Grouper = contract.GroupContract,
                CoinsurancePercentage = contract.CoInsurancePercentage,
                QuantityRisk = contract.RisksNumber,
                EstimatedDate = estimated,
                DepositPremiumAmount = contract.DepositPremiumAmount,
                PercentageDepositPremium = contract.DepositPercentageAmount

            };
        }

        #endregion Contract

        #region ContractLevel

        /// <summary>
        /// CreateLevel
        /// </summary>
        /// <param name="contractLevel"></param>
        /// <returns>ContractLevel</returns>
        public static REINSEN.Level CreateLevel(Level contractLevel)
        {
            return new REINSEN.Level(contractLevel.ContractLevelId)
            {
                AssignmentPercentage = contractLevel.AssignmentPercentage,
                ContractCode = contractLevel.Contract.ContractId,
                LevelId = contractLevel.ContractLevelId,
                LevelLimit = Convert.ToDecimal(contractLevel.ContractLimit),
                EventLimit = contractLevel.EventLimit,
                LevelNumber = contractLevel.Number,
                LinesNumber = Convert.ToDecimal(contractLevel.LinesNumber),
                RetentionLimit = contractLevel.RetentionLimit,
                AdjustmentPercentage = Convert.ToDecimal(contractLevel.AdjustmentPercentage),
                FixedRatePercentage = Convert.ToDecimal(contractLevel.FixedRatePercentage),
                MinimumRatePercentage = contractLevel.MinimumRatePercentage,
                MaximumRatePercentage = contractLevel.MaximumRatePercentage,
                LifeRate = contractLevel.LifeRate,
                CalculationType = Convert.ToInt32(contractLevel.CalculationType),
                ApplyOn = Convert.ToInt32(contractLevel.ApplyOnType),
                AnnualAddedLimit = contractLevel.AnnualAddedLimit,
                PremiumType = Convert.ToInt32(contractLevel.PremiumType)
            };
        }

        #endregion ContractLevel

        #region LevelPayment

        /// <summary>
        /// CreateLevelPayment
        /// </summary>
        /// <param name="levelPayment"></param>
        /// <returns>NonProportionalPayment</returns>
        public static REINSEN.LevelPayment CreateLevelPayment(LevelPayment levelPayment)
        {
            return new REINSEN.LevelPayment(levelPayment.Id)
            {
                LevelPaymentId = levelPayment.Id,
                LevelCode = levelPayment.Level.ContractLevelId,
                PaymentNumber = levelPayment.Number,
                Amount = levelPayment.Amount.Value,
                PaymentDate = levelPayment.Date
            };
        }

        #endregion LevelPayment

        #region LevelRestore

        /// <summary>
        /// CreateLevelRestore
        /// </summary>
        /// <param name="levelRestore"></param>
        /// <returns>Restablishment</returns>
        public static REINSEN.LevelRestore CreateLevelRestore(LevelRestore levelRestore)
        {
            return new REINSEN.LevelRestore(levelRestore.Id)
            {
                LevelRestoreId = levelRestore.Id,
                ContractLevelCode = levelRestore.Level.ContractLevelId,
                ReestablishmentNumber = levelRestore.Number,
                ReestablishmentPercentage = levelRestore.RestorePercentage,
                PointNoticePercentage = levelRestore.NoticePercentage
            };
        }

        #endregion LevelRestore

        #region ContractLevelCompany

        /// <summary>
        /// CreateLevelCompany
        /// </summary>
        /// <param name="contractLevelCompany"></param>
        /// <returns>ContractLevelCompany</returns>
        public static REINSEN.LevelCompany CreateLevelCompany(LevelCompany contractLevelCompany)
        {
            return new REINSEN.LevelCompany(contractLevelCompany.LevelCompanyId)
            {
                BrokerReinsuranceCompanyId = contractLevelCompany.Agent.IndividualId,
                CommissionPercentage = contractLevelCompany.ComissionPercentage,
                LevelCode = contractLevelCompany.ContractLevel.ContractLevelId,
                LevelCompanyId = contractLevelCompany.LevelCompanyId,
                ParticipationPercentage = contractLevelCompany.GivenPercentage,
                ReinsuranceCompanyId = contractLevelCompany.Company.IndividualId,
                ReservePremiumPercentage = contractLevelCompany.ReservePremiumPercentage,
                InterestReserveRelease = contractLevelCompany.InterestReserveRelease,
                AdditionalCommission = contractLevelCompany.AdditionalCommissionPercentage,
                DragLoss = Convert.ToInt32(contractLevelCompany.DragLossPercentage),
                ReinsurerExpenditur = contractLevelCompany.ReinsuranceExpensePercentage,
                ProfitSharingPercentage = contractLevelCompany.UtilitySharePercentage,
                Presentation = Convert.ToInt16(contractLevelCompany.PresentationInformationType),
                BrokerCommission = contractLevelCompany.IntermediaryCommission,
                LossCommissionPercentage = contractLevelCompany.ClaimCommissionPercentage,
                DifferentialCommissionPercentage = contractLevelCompany.DifferentialCommissionPercentage
            };
        }

        #endregion ContractLevelCompany

        #region EPITypes

        /// <summary>
        /// CreateEpiType
        /// </summary>
        /// <param name="epiType"></param>
        /// <returns>EpiType</returns>
        public static REINSEN.EpiType CreateEpiType(EPIType epiType)
        {
            return new REINSEN.EpiType(epiType.Id)
            {
                EpiTypeId = epiType.Id,
                Description = epiType.Description
            };
        }

        #endregion EPITypes

        #region AffectationType

        /// <summary>
        /// CreateAffectationType
        /// </summary>
        /// <param name="affectationType"></param>
        /// <returns>AffectationType</returns>
        public static REINSEN.AffectationType CreateAffectationType(AffectationType affectationType)
        {
            return new REINSEN.AffectationType(affectationType.Id)
            {
                AffectationTypeId = affectationType.Id,
                Description = affectationType.Description
            };
        }

        #endregion AffectationType

        #region ReestablishmentType

        /// <summary>
        /// CreateReestablishmentType
        /// </summary>
        /// <param name="resettlementType"></param>
        /// <returns>RestablishmentType</returns>
        public static REINSEN.ResettlementType CreateReestablishmentType(ResettlementType resettlementType)
        {
            return new REINSEN.ResettlementType(resettlementType.Id)
            {
                ResettlementTypeId = resettlementType.Id,
                Description = resettlementType.Description
            };
        }

        #endregion ReestablishmentType
        
        #region Line

        /// <summary>
        /// CreateLine
        /// </summary>
        /// <param name="line"></param>
        /// <returns>Line</returns>
        public static REINSEN.Line CreateLine(Line line)
        {
            return new REINSEN.Line(line.LineId)
            {
                LineId = line.LineId,
                CumulusTypeId = line.CumulusType.CumulusTypeId,
                Description = line.Description
            };
        }

        #endregion Line

        #region ContractLine

        /// <summary>
        /// CreateContractLine
        /// </summary>
        /// <param name="line"></param>
        /// <returns>ContractLine</returns>
        public static REINSEN.ContractLine CreateContractLine(Line line)
        {
            return new REINSEN.ContractLine(line.ContractLines[0].ContractLineId)
            {
                ContractId = line.ContractLines[0].Contract.ContractId,
                ContractLineId = line.ContractLines[0].ContractLineId,
                LineId = line.LineId,
                Priority = line.ContractLines[0].Priority
            };
        }

        #endregion ContractLine

        #endregion

        #region AssociationColumnValue

        /// <summary>
        /// CreateAssociationColumnValue
        /// </summary>
        /// <param name="associationColumnId"></param>
        /// <param name="associationLineId"></param>
        /// <param name="value"></param>
        /// <returns>AssociationColumnValue</returns>
        public static REINSEN.AssociationColumnValue CreateAssociationColumnValue(int associationLineId, int associationColumnId, int value)
        {
            return new REINSEN.AssociationColumnValue(0)
            {
                AssociationColumnValueId = 0,
                LineAssociationCode = associationLineId,
                AssociationColumnCode = associationColumnId,
                Value = value
            };
        }

        #endregion AssociationColumnValue

        #region AssociationLine

        /// <summary>
        /// CreateAssociationLine
        /// </summary>
        /// <param name="associationLine"></param>
        /// <returns>AssociationLine</returns>
        public static REINSEN.LineAssociation CreateAssociationLine(LineAssociation associationLine)
        {
            return new REINSEN.LineAssociation(associationLine.LineAssociationId)
            {
                LineAssociationId = 0,
                LineAssociationTypeCode = associationLine.AssociationType.LineAssociationTypeId,
                LineCode = associationLine.Line.LineId,
                DateFrom = Convert.ToDateTime(associationLine.DateFrom),
                DateTo = Convert.ToDateTime(associationLine.DateTo)
            };
        }

        #endregion

        #region ContractType

        /// <summary>
        /// CreateContractType
        /// </summary>
        /// <param name="contractType"></param>
        /// <returns>ContractType</returns>
        public static REINSEN.ContractType CreateContractType(ContractType contractType)
        {
            return new REINSEN.ContractType(contractType.ContractTypeId)
            {
                ContractFunctionalityId = contractType.ContractFunctionality.ContractFunctionalityId,
                ContractTypeId = contractType.ContractTypeId,
                Description = contractType.Description,
                Enabled = contractType.Enabled
            };
        }

        #endregion ContractType

        #region CumulusType

        /// <summary>
        /// CreateCumulusType
        /// </summary>
        /// <param name="cumulusType"></param>
        /// <returns>CumulusType</returns>
        public static REINSEN.CumulusType CreateCumulusType(CumulusType cumulusType)
        {
            return new REINSEN.CumulusType(cumulusType.CumulusTypeId)
            {
                CumulusTypeId = cumulusType.CumulusTypeId,
                Description = cumulusType.Description
            };
        }

        #endregion CumulusType

        #region IssueLayer

        /// <summary>
        /// CreateTempIssueLayer
        /// </summary>
        /// <param name="reinsuranceLayerIssuance"></param>
        /// <returns>TmpIssueLayer</returns>
        public static REINSEN.TempReinsLayerIssuance CreateTempIssueLayer(ReinsuranceLayerIssuance reinsuranceLayerIssuance)
        {
            return new REINSEN.TempReinsLayerIssuance(reinsuranceLayerIssuance.ReinsuranceLayerId)
            {
                TempReinsLayerIssuanceId = reinsuranceLayerIssuance.ReinsuranceLayerId,
                TempIssueCode = reinsuranceLayerIssuance.TemporaryIssueId,
                LayerNumber = reinsuranceLayerIssuance.LayerNumber,
                LayerPercentage = Convert.ToDecimal(reinsuranceLayerIssuance.LayerPercentage),
                PremiumPercentage = Convert.ToDecimal(reinsuranceLayerIssuance.PremiumPercentage)
            };
        }

        /// <summary>
        /// CreateTempFacultativeCompany
        /// </summary>
        /// <param name="reinsuranceFacultative"></param>
        /// <returns>TmpIssueLayer</returns>
        public static REINSEN.TempInstallmentCompany CreateTempFacultativeCompany(ReinsuranceAllocation reinsuranceFacultative)
        {
            return new REINSEN.TempInstallmentCompany(0)
            {
                TempInstallementCompanyId = 0,
                TempFacultativeCode = reinsuranceFacultative.MovementSourceId,
                BrokerReinsuranceCode = reinsuranceFacultative.Contract.ContractLevels[0].ContractLevelCompanies[0].Agent.IndividualId,
                ReinsuranceCompanyCode = reinsuranceFacultative.Contract.ContractLevels[0].ContractLevelCompanies[0].Company.IndividualId,
                CommissionPercentage = Convert.ToDecimal(reinsuranceFacultative.Contract.ContractLevels[0].ContractLevelCompanies[0].ComissionPercentage),
                PremiumPercentage = Convert.ToDecimal(reinsuranceFacultative.Contract.ContractLevels[0].ContractLevelCompanies[0].GivenPercentage),
                ParticipationPercentage = Convert.ToDecimal(reinsuranceFacultative.Contract.ContractLevels[0].AssignmentPercentage),
                PaymentDeadline = Convert.ToDateTime(reinsuranceFacultative.Contract.DateTo)
            };
        }

        /// <summary>
        /// CreateFacultativeCompany
        /// </summary>
        /// <param name="installment"></param>
        /// <returns>TmpFacultativeCompany</returns>
        public static REINSEN.TempInstallmentCompany CreateFacultativeCompany(Installment installment)
        {
            return new REINSEN.TempInstallmentCompany(0)
            {
                TempInstallementCompanyId = 0,
                TempFacultativeCode = installment.InstallmentNumber,
                BrokerReinsuranceCode = installment.LevelCompany.Agent.IndividualId,
                ReinsuranceCompanyCode = installment.LevelCompany.Company.IndividualId,
                CommissionPercentage = Convert.ToDecimal(installment.LevelCompany.ComissionPercentage),
                PremiumPercentage = Convert.ToDecimal(installment.LevelCompany.GivenPercentage),
                ParticipationPercentage = Convert.ToDecimal(installment.LevelCompany.ContractLevel.AssignmentPercentage),
                PaymentDeadline = DateTime.Now,
                ReservePremiumPercentage = Convert.ToDecimal(installment.LevelCompany.DepositPercentage),
                InterestReserveRelease = installment.LevelCompany.InterestOnReserve,
                DepositReleaseDate = installment.LevelCompany.DepositReleaseDate
            };
        }

        /// <summary>
        /// CreateTmpFacultativePayments
        /// </summary>
        /// <param name="installment"></param>
        /// <returns></returns>
        public static REINSEN.TempInstallmentPayment CreateTmpFacultativePayments(Installment installment)
        {
            return new REINSEN.TempInstallmentPayment(0)
            {
                TempInstallementCompanyCode = installment.LevelCompany.LevelCompanyId,
                FeeNumber = installment.InstallmentNumber,
                PaymentDate = installment.PaidDate,
                PaymentAmount = installment.PaidAmount.Value
            };
        }

        #endregion

        #region LineBusinessReinsurance

        /// <summary>
        /// CreateLineBusinessReinsurance
        /// </summary>
        /// <param name="reinsuranceprefix"></param>
        /// <returns>LineBusinessReinsurance</returns>
        public static REINSEN.ReinsurancePrefix CreateLineBusinessReinsurance(ReinsurancePrefix reinsuranceprefix)
        {
            return new REINSEN.ReinsurancePrefix(reinsuranceprefix.Id)
            {
                ReinsurancePrefixId = reinsuranceprefix.Id,
                LineBusinessCode = reinsuranceprefix.Prefix.Id,
                LineBusinessCumulusCode = reinsuranceprefix.PrefixCumulus.Id,
                TypeExerciceCode = (int)reinsuranceprefix.ExerciseType,
                Location = reinsuranceprefix.IsLocation
            };
        }

        #endregion

        #region PriorityRetention

        /// <summary>
        /// CreatePriorityRetention
        /// </summary>
        /// <param name="priorityRetention"></param>
        /// <returns></returns>
        public static REINSEN.PriorityRetention CreatePriorityRetention(PriorityRetention priorityRetention)
        {
            return new REINSEN.PriorityRetention(priorityRetention.Id)
            {
                PriorityRetentionId = priorityRetention.Id,
                ValidityTo = priorityRetention.ValidityTo,
                ValidityFrom = priorityRetention.ValidityFrom,
                PrefixCode = priorityRetention.Prefix.Id,
                PriorityRetentionAmount = priorityRetention.PriorityRetentionAmount,
                Enabled = priorityRetention.Enabled
            };
        }

        /// <summary>
        /// CreatePriorityRetentionDetail
        /// </summary>
        /// <param name="priorityRetentionDetail"></param>
        /// <returns></returns>
        public static REINSEN.PriorityRetentionDetail CreatePriorityRetentionDetail(PriorityRetentionDetail priorityRetentionDetail)
        {
            return new REINSEN.PriorityRetentionDetail()
            {
                PriorityRetentionId = priorityRetentionDetail.PriorityRetentionId,
                EndorsementId = priorityRetentionDetail.EndorsementId,
                PolicyId = priorityRetentionDetail.PolicyId,
                IssueDate = priorityRetentionDetail.IssueDate,
                PrefixCd = priorityRetentionDetail.PrefixCd,
                IndividualId = priorityRetentionDetail.IndividualId,
                RetentionCumulus = priorityRetentionDetail.RetentionCumulus,
                PriorityRetentionAmount = priorityRetentionDetail.PriorityRetentionAmount,
                ProcessDate = priorityRetentionDetail.ProcessDate,
                CurrentPriorityRetentionAmount = priorityRetentionDetail.CurrentPriorityRetentionAmount

            };
        }

        #endregion




    }
}