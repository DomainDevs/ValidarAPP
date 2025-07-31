//Sistran.Core
using AutoMapper;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ReinsuranceServices.DTOs;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using Sistran.Core.Application.ReinsuranceServices.Models.LineAssociationTypes;
using Sistran.Core.Application.UniquePerson.IntegrationService.Models;
using Sistran.Core.Application.Utilities.Cache;
//FWK
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using REINSEN = Sistran.Core.Application.Reinsurance.Entities;
using TEMPINTDTOs = Sistran.Core.Integration.TempCommonService.DTOs;


namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers
{
    internal static class ModelAssembler
    {

        #region Contract

        /// <summary>
        /// CreateContract
        /// </summary>
        /// <param name="contract"></param>
        /// <returns>Contract</returns>
        internal static Contract CreateContract(REINSEN.Contract contract)
        {
            var currency = new Currency { Id = contract.CurrencyId };
            return new Contract
            {
                ContractId = contract.ContractId,
                ContractType = new ContractType { ContractTypeId = contract.ContractTypeId },
                Currency = currency,
                DateFrom = contract.DateFrom,
                DateTo = contract.DateTo,
                Description = contract.Description,
                SmallDescription = contract.SmallDescription,
                Year = contract.Year,
                ReleaseTimeReserve = Convert.ToInt32(contract.ReleaseTimeReserve),
                AffectationType = new AffectationType { Id = Convert.ToInt32(contract.AffectationTypeCode) },
                ResettlementType = new ResettlementType { Id = Convert.ToInt32(contract.ReestablishmentTypeCode) },
                PremiumAmount = Convert.ToDecimal(contract.Epi),
                EPIType = new EPIType { Id = Convert.ToInt32(contract.EpiTypeCode) },
                Enabled = Convert.ToBoolean(contract.Status),
                GroupContract = contract.Grouper,
                CoInsurancePercentage = Convert.ToDecimal(contract.CoinsurancePercentage),
                RisksNumber = Convert.ToInt32(contract.QuantityRisk),
                EstimatedDate = Convert.ToDateTime(contract.EstimatedDate),
                DepositPremiumAmount = Convert.ToDecimal(contract.DepositPremiumAmount),
                DepositPercentageAmount = Convert.ToDecimal(contract.PercentageDepositPremium)
            };
        }

        /// <summary>
        /// CreateContracts
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>Contract</returns>
        internal static List<Contract> CreateContracts(BusinessCollection businessCollection)
        {
            var contracts = new List<Contract>();

            foreach (BusinessObject businessObject in businessCollection)
            {
                REINSEN.Contract contractEntity = (REINSEN.Contract)businessObject;
                contracts.Add(CreateContract(contractEntity));
            }
            return contracts;
        }

        #endregion Contract

        #region ContractLevel

        /// <summary>
        /// CreateLevel
        /// </summary>
        /// <param name="contractLevel"></param>
        /// <returns>Level</returns>
        internal static Level CreateLevel(REINSEN.Level contractLevel)
        {
            return new Level()
            {
                AssignmentPercentage = Convert.ToDecimal((contractLevel.AssignmentPercentage)),
                Contract = new Contract { ContractId = contractLevel.ContractCode },
                ContractLevelId = contractLevel.LevelId,
                ContractLimit = contractLevel.LevelLimit,
                EventLimit = contractLevel.EventLimit,
                Number = contractLevel.LevelNumber,
                LinesNumber = Convert.ToDecimal(contractLevel.LinesNumber),
                RetentionLimit = contractLevel.RetentionLimit,
                AdjustmentPercentage = Convert.ToDecimal(contractLevel.AdjustmentPercentage),
                FixedRatePercentage = Convert.ToDecimal(contractLevel.FixedRatePercentage),
                MinimumRatePercentage = Convert.ToDecimal(contractLevel.MinimumRatePercentage),
                MaximumRatePercentage = Convert.ToDecimal(contractLevel.MaximumRatePercentage),
                LifeRate = Convert.ToDecimal(contractLevel.LifeRate),
                CalculationType = contractLevel.CalculationType == null ? 0 : (CalculationTypes)contractLevel.CalculationType,
                ApplyOnType = contractLevel.ApplyOn == null ? 0 : (ApplyOnTypes)contractLevel.ApplyOn,
                AnnualAddedLimit = Convert.ToDecimal(contractLevel.AnnualAddedLimit),
                PremiumType = contractLevel.PremiumType == null ? 0 : (PremiumTypes)contractLevel.PremiumType
            };
        }

        /// <summary>
        /// CreateLevels
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<models.Level/></returns>
        internal static List<Level> CreateLevels(BusinessCollection businessCollection)
        {
            return (from REINSEN.Level contractLevelEntity in businessCollection select CreateLevel(contractLevelEntity)).ToList();
        }

        #endregion ContractLevel

        #region LevelPayment

        /// <summary>
        /// CreateLevelPayment
        /// </summary>
        /// <param name="levelPayment"></param>
        /// <returns>LevelPayment</returns>
        internal static LevelPayment CreateLevelPayment(REINSEN.LevelPayment levelPayment)
        {
            return new LevelPayment
            {
                Id = levelPayment.LevelPaymentId,
                Level = new Level { ContractLevelId = Convert.ToInt32(levelPayment.LevelCode) },
                Number = levelPayment.PaymentNumber,
                Amount = new Amount { Value = Convert.ToDecimal(levelPayment.Amount) },
                Date = levelPayment.PaymentDate
            };
        }

        /// <summary>
        /// CreateLevelPayments
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<models.LevelPayment></returns>
        internal static List<LevelPayment> CreateLevelPayments(BusinessCollection businessCollection)
        {
            List<LevelPayment> levelPayments = new List<LevelPayment>();

            foreach (REINSEN.LevelPayment levelPayment in businessCollection.OfType<REINSEN.LevelPayment>())
            {
                levelPayments.Add(CreateLevelPayment(levelPayment));
            }
            return levelPayments;
        }

        #endregion LevelPayment

        #region LevelRestore

        /// <summary>
        /// CreateLevelRestore
        /// </summary>
        /// <param name="restablishment"></param>
        /// <returns>LevelRestore</returns>
        internal static LevelRestore CreateLevelRestore(REINSEN.LevelRestore reestablishment)
        {
            return new LevelRestore
            {
                Id = reestablishment.LevelRestoreId,
                Level = new Level { ContractLevelId = Convert.ToInt32(reestablishment.ContractLevelCode) },
                Number = reestablishment.ReestablishmentNumber,
                RestorePercentage = Convert.ToDecimal(reestablishment.ReestablishmentPercentage),
                NoticePercentage = Convert.ToDecimal(reestablishment.PointNoticePercentage)
            };
        }

        /// <summary>
        /// CreateLevelRestores
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<models.LevelRestore></returns>
        internal static List<LevelRestore> CreateLevelRestores(BusinessCollection businessCollection)
        {
            List<LevelRestore> levelRestores = new List<LevelRestore>();

            foreach (REINSEN.LevelRestore reestablishmentEntity in businessCollection.OfType<REINSEN.LevelRestore>())
            {
                levelRestores.Add(CreateLevelRestore(reestablishmentEntity));
            }
            return levelRestores;
        }

        #endregion LevelRestore

        #region ContractLevelCompany

        /// <summary>
        /// CreateLevelCompany
        /// </summary>
        /// <param name="contractLevelCompany"></param>
        /// <returns>LevelCompany</returns>
        internal static LevelCompany CreateLevelCompany(REINSEN.LevelCompany contractLevelCompany)
        {
            var agent = new Agent { IndividualId = contractLevelCompany.BrokerReinsuranceCompanyId };
            var company = new Company { IndividualId = contractLevelCompany.ReinsuranceCompanyId };

            PresentationInformationTypes presentationInformationTypes = PresentationInformationTypes.Annual;
            if (contractLevelCompany.Presentation == 1)
            {
                presentationInformationTypes = PresentationInformationTypes.Monthly;
            }
            if (contractLevelCompany.Presentation == 2)
            {
                presentationInformationTypes = PresentationInformationTypes.Quarterly;
            }
            if (contractLevelCompany.Presentation == 3)
            {
                presentationInformationTypes = PresentationInformationTypes.Biannual;
            }
            if (contractLevelCompany.Presentation == 4)
            {
                presentationInformationTypes = PresentationInformationTypes.Annual;
            }

            return new LevelCompany()
            {
                LevelCompanyId = contractLevelCompany.LevelCompanyId,
                Agent = agent,
                Company = company,
                GivenPercentage = Convert.ToDecimal(contractLevelCompany.ParticipationPercentage),
                ComissionPercentage = contractLevelCompany.CommissionPercentage,
                ContractLevel = new Level() { ContractLevelId = contractLevelCompany.LevelCode },
                InterestReserveRelease = contractLevelCompany.InterestReserveRelease,
                ReservePremiumPercentage = contractLevelCompany.ReservePremiumPercentage,
                AdditionalCommissionPercentage = Convert.ToDecimal(contractLevelCompany.AdditionalCommission),
                DragLossPercentage = Convert.ToDecimal(contractLevelCompany.DragLoss),
                ReinsuranceExpensePercentage = Convert.ToDecimal(contractLevelCompany.ReinsurerExpenditur),
                UtilitySharePercentage = Convert.ToDecimal(contractLevelCompany.ProfitSharingPercentage),
                PresentationInformationType = presentationInformationTypes,
                IntermediaryCommission = Convert.ToBoolean(contractLevelCompany.BrokerCommission),
                ClaimCommissionPercentage = Convert.ToDecimal(contractLevelCompany.LossCommissionPercentage),
                DifferentialCommissionPercentage = Convert.ToDecimal(contractLevelCompany.DifferentialCommissionPercentage)
            };
        }

        /// <summary>
        /// CreateLevelCompanys
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<LevelCompany></returns>
        internal static List<LevelCompany> CreateLevelCompanys(BusinessCollection businessCollection)
        {
            var listLevelCompany = new List<LevelCompany>();

            foreach (REINSEN.LevelCompany levelCompanyEntity in businessCollection.OfType<REINSEN.LevelCompany>())
            {
                listLevelCompany.Add(CreateLevelCompany(levelCompanyEntity));
            }
            return listLevelCompany;
        }

        /// <summary>
        /// CreateInstallment
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<Installment/></returns>
        internal static List<Installment> CreateInstallment(BusinessCollection businessCollection)
        {

            List<Installment> installments = new List<Installment>();

            LevelCompany levelcompany = new LevelCompany();
            foreach (REINSEN.TempInstallmentPayment tmpFacultativePayments in businessCollection.OfType<REINSEN.TempInstallmentPayment>())
            {
                Amount paidAmount = new Amount() { Value = tmpFacultativePayments.PaymentAmount };

                levelcompany.LevelCompanyId = tmpFacultativePayments.TempInstallementCompanyCode;

                installments.Add(new Installment
                {
                    Id = tmpFacultativePayments.TempInstallementPaymentsId,
                    LevelCompany = levelcompany,
                    InstallmentNumber = tmpFacultativePayments.FeeNumber,
                    PaidAmount = paidAmount,
                    PaidDate = tmpFacultativePayments.PaymentDate
                });
            }
            return installments;
        }
        #endregion

        #region Line

        /// <summary>
        /// CreateLine
        /// </summary>
        /// <param name="associationLine"></param>
        /// <returns>Line</returns>
        internal static Line CreateLine(REINSEN.Line lineEntity)
        {
            if (lineEntity != null)
            {
                return new Line
                {
                    LineId = lineEntity.LineId,
                    Description = lineEntity?.Description,
                    CumulusType = new CumulusType
                    {
                        CumulusTypeId = lineEntity.CumulusTypeId
                    }
                };
            }
            else
            {
                return new Line();
            }

        }

        /// <summary>
        /// CreateLines
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<Line/></returns>
        internal static List<Line> CreateLines(BusinessCollection businessCollection)
        {
            return (from REINSEN.Line lineEntity in businessCollection select CreateLine(lineEntity)).ToList();
        }

        #endregion

        #region ContractLine

        /// <summary>
        /// CreateContractLine
        /// </summary>
        /// <param name="contractLine"></param>
        /// <returns>ContractLine</returns>
        internal static ContractLine CreateContractLine(REINSEN.ContractLine contractLine)
        {
            return new ContractLine
            {
                Contract = new Contract { ContractId = contractLine.ContractId },
                ContractLineId = contractLine.ContractLineId,
                Priority = contractLine.Priority
            };
        }

        /// <summary>
        /// CreateContractLines
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<ContractLine/></returns>
        internal static List<ContractLine> CreateContractLines(BusinessCollection businessCollection)
        {
            return (from REINSEN.ContractLine contractLineEntity in businessCollection select CreateContractLine(contractLineEntity)).ToList();
        }

        #endregion ContractLine

        #region LineAssociation

        internal static List<LineAssociation> CreateAssociationLines(BusinessCollection businessObjects)
        {
            List<LineAssociation> associationLines = new List<LineAssociation>();
            foreach (REINSEN.LineAssociation associationLine in businessObjects)
            {
                associationLines.Add(CreateAssociationLine(associationLine));
            }

            return associationLines;
        }

        /// <summary>
        /// CreateAssociationLine
        /// </summary>
        /// <param name="associationLine"></param>
        /// <returns>LineAssociation</returns>
        internal static LineAssociation CreateAssociationLine(REINSEN.LineAssociation associationLine)
        {
            return new LineAssociation()
            {
                AssociationType = new LineAssociationType { LineAssociationTypeId = associationLine.LineAssociationTypeCode },
                LineAssociationId = associationLine.LineAssociationId,
                Line = new Line { LineId = associationLine.LineCode },
                DateFrom = associationLine.DateFrom,
                DateTo = associationLine.DateTo
            };
        }

        /// <summary>
        /// CreateAssociationColumnValue
        /// </summary>
        /// <param name="associationColumnValue"></param>
        /// <returns>ByLineBusiness</returns>
        internal static ByLineBusiness CreateAssociationColumnValue(REINSEN.AssociationColumnValue associationColumnValue)
        {
            return new ByLineBusiness
            {
                LineAssociationTypeId = associationColumnValue.AssociationColumnValueId,
            };
        }

        #endregion LineAssociation

        #region AssociationType

        /// <summary>
        /// CreateAssociationType
        /// </summary>
        /// <param name="associationType"></param>
        /// <returns>LineAssociationType</returns>
        internal static LineAssociationType CreateAssociationType(REINSEN.LineAssociationType associationType)
        {
            return new LineAssociationType
            {
                LineAssociationTypeId = associationType.LineAssociationTypeId,
                Description = associationType.Description,
                Priority = associationType.Priority,
                Enabled = (Boolean)associationType.Enabled
            };
        }

        /// <summary>
        /// CreateAssociationTypes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<AssociationType/></returns>
        internal static List<LineAssociationType> CreateAssociationTypes(BusinessCollection businessCollection)
        {
            var listAssociationType = new List<LineAssociationType>();

            foreach (REINSEN.LineAssociationType associationTypeEntity in businessCollection.OfType<REINSEN.LineAssociationType>())
            {
                listAssociationType.Add(CreateAssociationType(associationTypeEntity));
            }
            return listAssociationType;
        }

        #region ContractType

        /// <summary>
        /// CreateContractType
        /// </summary>
        /// <param name="contractType"></param>
        /// <returns>ContractType</returns>
        internal static ContractType CreateContractType(REINSEN.ContractType contractType)
        {
            return new ContractType
            {
                ContractFunctionality = new ContractFunctionality { ContractFunctionalityId = Convert.ToInt32(contractType.ContractFunctionalityId) },
                ContractTypeId = contractType.ContractTypeId,
                Description = contractType.Description,
                Enabled = contractType.Enabled
            };
        }

        /// <summary>
        /// CreateContractTypes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<ContractType/></returns>
        internal static List<ContractType> CreateContractTypes(BusinessCollection businessCollection)
        {
            return (from REINSEN.ContractType contractTypeEntity in businessCollection select CreateContractType(contractTypeEntity)).ToList();
        }

        #endregion ContractType

        #region CumulusType

        /// <summary>
        /// CreateCumulusType
        /// </summary>
        /// <param name="cumulusType"></param>
        /// <returns>Models.CumulusType</returns>
        internal static CumulusType CreateCumulusType(REINSEN.CumulusType cumulusType)
        {
            return new CumulusType
            {
                CumulusTypeId = cumulusType.CumulusTypeId,
                Description = cumulusType.Description
            };
        }

        /// <summary>
        /// CreateCumulusTypes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<CumulusType/></returns>
        internal static List<CumulusType> CreateCumulusTypes(BusinessCollection businessCollection)
        {
            var listCumulusType = new List<CumulusType>();
            foreach (REINSEN.CumulusType cumulusTypeEntity in businessCollection.OfType<REINSEN.CumulusType>())
            {
                listCumulusType.Add(CreateCumulusType(cumulusTypeEntity));
            }
            return listCumulusType;
        }

        #endregion

        #endregion REINS

        #region ReinsModification

        /// <summary>
        /// CreateTempIssueLayer
        /// </summary>
        /// <param name="tmpIssueLayer"></param>
        /// <returns>ReinsuranceLayerIssuance</returns>
        internal static ReinsuranceLayerIssuance CreateTempIssueLayer(REINSEN.TempReinsLayerIssuance tempReinsLayerIssuance)
        {
            decimal layerPercentage = tempReinsLayerIssuance.LayerPercentage;
            decimal premiumPercentage = tempReinsLayerIssuance.PremiumPercentage;

            return new ReinsuranceLayerIssuance()
            {
                LayerPercentage = layerPercentage,
                LayerAmount = 0,
                PremiumPercentage = premiumPercentage,
                PremiumAmount = 0,
                LayerNumber = tempReinsLayerIssuance.LayerNumber,
                TemporaryIssueId = tempReinsLayerIssuance.TempIssueCode,
                ReinsuranceLayerId = tempReinsLayerIssuance.TempReinsLayerIssuanceId
            };
        }

        /// <summary>
        /// CreateTempLayerLine
        /// </summary>
        /// <param name="tempLayerLine"></param>
        /// <returns>ReinsuranceLine</returns>
        internal static ReinsuranceLine CreateTempLayerLine(REINSEN.TempReinsuranceLine tempLayerLine)
        {
            Line line = new Line();
            line.LineId = Convert.ToInt32(tempLayerLine.LineCode);

            return new ReinsuranceLine()
            {
                ReinsuranceLineId = tempLayerLine.TempReinsuranceLineId,
                Line = line
            };
        }

        /// <summary>
        /// CreateTempAllocation
        /// </summary>
        /// <param name="tempAllocation"></param>
        /// <returns>ReinsuranceAllocation</returns>
        internal static ReinsuranceAllocation CreateTempAllocation(REINSEN.TempReinsuranceAllocation tempAllocation)
        {
            Amount amount = new Amount();
            amount.Value = Convert.ToDecimal(tempAllocation.Amount);

            Amount premium = new Amount();
            premium.Value = Convert.ToDecimal(tempAllocation.Premium);

            return new ReinsuranceAllocation()
            {
                ReinsuranceAllocationId = tempAllocation.TempReinsuranceAllocationId,
                Amount = amount,
                Premium = premium
            };
        }

        /// <summary>
        /// CreateTempFacultativeCompany
        /// </summary>
        /// <param name="tempFacultativeCompany"></param>
        /// <returns>ReinsuranceAllocation</returns>
        internal static ReinsuranceAllocation CreateTempFacultativeCompany(REINSEN.TempInstallmentCompany tempFacultativeCompany)
        {
            Contract contract = new Contract();

            contract.DateTo = tempFacultativeCompany.PaymentDeadline;

            Level level = new Level();
            level.AssignmentPercentage = tempFacultativeCompany.ParticipationPercentage;

            LevelCompany levelCompany = new LevelCompany();
            levelCompany.Agent = new Agent();
            levelCompany.Agent.IndividualId = tempFacultativeCompany.BrokerReinsuranceCode;
            levelCompany.Company = new Company();
            levelCompany.Company.IndividualId = Convert.ToInt32(tempFacultativeCompany.ReinsuranceCompanyCode);
            levelCompany.ComissionPercentage = tempFacultativeCompany.CommissionPercentage;
            levelCompany.GivenPercentage = tempFacultativeCompany.PremiumPercentage;
            level.ContractLevelCompanies = new List<LevelCompany>();
            level.ContractLevelCompanies.Add(levelCompany);
            contract.ContractLevels = new List<Level>();
            contract.ContractLevels.Add(level);

            return new ReinsuranceAllocation()
            {
                ReinsuranceAllocationId = tempFacultativeCompany.TempInstallementCompanyId,
                MovementSourceId = tempFacultativeCompany.TempFacultativeCode,
                Contract = contract
            };
        }

        #endregion

        #region EPITypes

        /// <summary>
        /// CreateEpiType
        /// </summary>
        /// <param name="epiType"></param>
        /// <returns>EPIType</returns>
        internal static EPIType CreateEpiType(REINSEN.EpiType epiType)
        {
            return new EPIType
            {
                Id = epiType.EpiTypeId,
                Description = epiType.Description
            };
        }

        /// <summary>
        /// CreateEpiTypes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<models.EPIType/></returns>
        internal static List<EPIType> CreateEpiTypes(BusinessCollection businessCollection)
        {
            List<EPIType> epiTypes = new List<EPIType>();

            foreach (REINSEN.EpiType epiTypesEntity in businessCollection.OfType<REINSEN.EpiType>())
            {
                epiTypes.Add(CreateEpiType(epiTypesEntity));
            }
            return epiTypes;
        }

        #endregion

        #region AffectationType

        /// <summary>
        /// CreateAffectationType
        /// </summary>
        /// <param name="epiType"></param>
        /// <returns>AffectationType</returns>
        internal static AffectationType CreateAffectationType(REINSEN.AffectationType epiType)
        {
            return new AffectationType
            {
                Id = epiType.AffectationTypeId,
                Description = epiType.Description
            };
        }

        /// <summary>
        /// CreateAffectationTypes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<models.AffectationType/></returns>
        internal static List<AffectationType> CreateAffectationTypes(BusinessCollection businessCollection)
        {
            List<AffectationType> affectationTypes = new List<AffectationType>();

            foreach (REINSEN.AffectationType affectationTypesEntity in businessCollection.OfType<REINSEN.AffectationType>())
            {
                affectationTypes.Add(CreateAffectationType(affectationTypesEntity));
            }
            return affectationTypes;
        }

        #endregion

        #region ReestablishmentType

        /// <summary>
        /// CreateReestablishmentType
        /// </summary>
        /// <param name="restablishmentType"></param>
        /// <returns>ResettlementType</returns>
        internal static ResettlementType CreateReestablishmentType(REINSEN.ResettlementType restablishmentType)
        {
            return new ResettlementType
            {
                Id = restablishmentType.ResettlementTypeId,
                Description = restablishmentType.Description
            };
        }

        /// <summary>
        /// CreateReestablishmentTypes
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<ResettlementType/></returns>
        internal static List<ResettlementType> CreateReestablishmentTypes(BusinessCollection businessCollection)
        {
            List<ResettlementType> restablishmentTypes = new List<ResettlementType>();

            foreach (REINSEN.ResettlementType restablishmentTypeEntity in businessCollection.OfType<REINSEN.ResettlementType>())
            {
                restablishmentTypes.Add(CreateReestablishmentType(restablishmentTypeEntity));
            }
            return restablishmentTypes;
        }

        #endregion

        #region LineBusinessReinsurance

        /// <summary>
        /// CreateLineBusinessReinsurance
        /// </summary>
        /// <param name="linebusinessreinsurance"></param>
        /// <returns>ReinsurancePrefix</returns>
        internal static ReinsurancePrefix CreateLineBusinessReinsurance(REINSEN.ReinsurancePrefix lineBusinessReinsurance)
        {
            return new ReinsurancePrefix
            {
                Id = lineBusinessReinsurance.ReinsurancePrefixId,
                ExerciseType = (ExerciseTypes)lineBusinessReinsurance.TypeExerciceCode,
                IsLocation = lineBusinessReinsurance.Location,
                Prefix = new Prefix() { Id = lineBusinessReinsurance.LineBusinessCode },
                PrefixCumulus = new Prefix() { Id = lineBusinessReinsurance.LineBusinessCumulusCode }
            };
        }

        /// <summary>
        /// CreateLinesBusinessReinsurance
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<models.ReinsurancePrefix/></returns>
        internal static List<ReinsurancePrefix> CreateLinesBusinessReinsurance(BusinessCollection businessCollection)
        {
            var reinsurancePrefixes = new List<ReinsurancePrefix>();

            foreach (BusinessObject businessObject in businessCollection)
            {
                REINSEN.ReinsurancePrefix reinsurancePrefixtEntity = (REINSEN.ReinsurancePrefix)businessObject;
                reinsurancePrefixes.Add(CreateLineBusinessReinsurance(reinsurancePrefixtEntity));
            }
            return reinsurancePrefixes;
        }

        #endregion

        #region PriorityRetention
        /// <summary>
        /// CreateLinesBusinessReinsurance
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<models.ReinsurancePrefix/></returns>
        internal static List<PriorityRetention> CreatePriorityRetentions(BusinessCollection businessCollection)
        {
            List<PriorityRetention> priorityRetentions = new List<PriorityRetention>();

            foreach (BusinessObject businessObject in businessCollection)
            {
                REINSEN.PriorityRetention entityPriorityRetention = (REINSEN.PriorityRetention)businessObject;
                priorityRetentions.Add(CreatePriorityRetention(entityPriorityRetention));
            }
            return priorityRetentions;
        }
        internal static PriorityRetention CreatePriorityRetention(REINSEN.PriorityRetention entityPriorityRetention)
        {
            return new PriorityRetention
            {
                Id = entityPriorityRetention.PriorityRetentionId,
                Prefix = new Prefix
                {
                    Id = entityPriorityRetention.PrefixCode,
                    Description = ""
                },
                PriorityRetentionAmount = entityPriorityRetention.PriorityRetentionAmount,
                ValidityFrom = entityPriorityRetention.ValidityFrom,
                ValidityTo = entityPriorityRetention.ValidityTo,
                Enabled = entityPriorityRetention.Enabled
            };
        }

        internal static List<PriorityRetentionDetail> CreatePriorityRetentionDetails(BusinessCollection businessCollection)
        {
            List<PriorityRetentionDetail> priorityRetentionDetails = new List<PriorityRetentionDetail>();

            foreach (REINSEN.PriorityRetentionDetail businessObject in businessCollection)
            {
                priorityRetentionDetails.Add(CreatePriorityRetentionDetail(businessObject));
            }
            return priorityRetentionDetails;
        }

        internal static PriorityRetentionDetail CreatePriorityRetentionDetail(REINSEN.PriorityRetentionDetail entityPriorityRetentionDetail)
        {
            return new PriorityRetentionDetail
            {
                Id = entityPriorityRetentionDetail.PriorityRetentionDetailId,
                PriorityRetentionId = entityPriorityRetentionDetail.PriorityRetentionId
            };
        }

        #endregion

        /// <summary>
        /// CreatePrefix
        /// </summary>
        /// <param name="prefixDTO"></param>
        /// <returns></returns>
        internal static Prefix CreatePrefix(PrefixDTO prefixDTO)
        {
            return new Prefix
            {
                Id = prefixDTO.Id
            };
        }

        /// <summary>
        /// CreatePrefixs
        /// </summary>
        /// <param name="prefixDTO"></param>
        /// <returns></returns>
        internal static List<Prefix> CreatePrefixes(List<PrefixDTO> prefixDTO)
        {
            List<Prefix> prefixs = new List<Prefix>();

            foreach (PrefixDTO tempPrefix in prefixDTO)
            {
                prefixs.Add(CreatePrefix(tempPrefix));
            }

            return prefixs;
        }

        /// <summary>
        /// CreatePolicy
        /// </summary>
        /// <param name="policyDTO"></param>
        /// <returns></returns>
        internal static TempCommonServices.Models.Policy CreatePolicy(PolicyDTO policyDTO)
        {

            return new TempCommonServices.Models.Policy
            {
                Id = policyDTO.PolicyId,
                Endorsement = new TempCommonServices.Models.Endorsement()
                {
                    Id = policyDTO.EndorsmentId,
                }
            };
        }

        /// <summary>
        /// CreateLine
        /// </summary>
        /// <param name="lineDTO"></param>
        /// <returns></returns>
        internal static Line CreateLine(LineDTO lineDTO)
        {
            List<ContractLine> contractLines = new List<ContractLine>();

            if (lineDTO.ContractLines != null)
            {
                contractLines = CreateContractLines(lineDTO.ContractLines);
            }


            CumulusType cumulusType = new CumulusType();

            if (lineDTO.CumulusType != null)
            {
                cumulusType.CumulusTypeId = lineDTO.CumulusType.CumulusTypeId;
            }


            return new Line
            {
                LineId = lineDTO.LineId,
                Description = lineDTO.Description,
                CumulusType = cumulusType,
                ContractLines = contractLines
            };
        }

        /// <summary>
        /// CreateContractLines
        /// </summary>
        /// <param name="ContractLineDTOs"></param>
        /// <returns>List<ContractLine></returns>
        internal static List<ContractLine> CreateContractLines(List<ContractLineDTO> ContractLineDTOs)
        {
            List<ContractLine> contractLines = new List<ContractLine>();

            foreach (ContractLineDTO tempContractLineDTO in ContractLineDTOs)
            {
                contractLines.Add(CreateContractLine(tempContractLineDTO));
            }
            return contractLines;
        }

        /// <summary>
        /// CreateContractLine
        /// </summary>
        /// <param name="contractLineDTO"></param>
        /// <returns>ContractLine</returns>
        internal static ContractLine CreateContractLine(ContractLineDTO contractLineDTO)
        {
            List<Level> contractLevels = new List<Level>();
            if (contractLineDTO.Contract.ContractLevels != null)
            {
                contractLevels = CreateLevels(contractLineDTO.Contract.ContractLevels);
            }

            return new ContractLine
            {
                ContractLineId = contractLineDTO.ContractLineId,
                Priority = contractLineDTO.Priority,
                Contract = new Contract
                {
                    ContractId = contractLineDTO.Contract.ContractId,
                    ContractLevels = contractLevels
                }
            };
        }

        /// <summary>
        /// CreateLevel
        /// </summary>
        /// <param name="levelDTO"></param>
        /// <returns></returns>
        internal static Level CreateLevel(LevelDTO levelDTO)
        {
            List<LevelCompany> levelCompanies = new List<LevelCompany>();

            if (levelDTO.ContractLevelCompanies != null)
            {
                levelCompanies = CreateLevelCompanies(levelDTO.ContractLevelCompanies);
            }

            return new Level
            {
                ContractLevelId = levelDTO.ContractLevelId,
                AssignmentPercentage = levelDTO.AssignmentPercentage,
                Contract = new Contract
                {
                    ContractId = levelDTO.Contract.ContractId
                },
                ContractLimit = levelDTO.ContractLimit,
                EventLimit = levelDTO.EventLimit,
                Number = levelDTO.Number,
                LinesNumber = levelDTO.LinesNumber,
                RetentionLimit = levelDTO.RetentionLimit,
                AdjustmentPercentage = levelDTO.AdjustmentPercentage,
                FixedRatePercentage = levelDTO.FixedRatePercentage,
                MinimumRatePercentage = levelDTO.MinimumRatePercentage,
                MaximumRatePercentage = levelDTO.MaximumRatePercentage,
                LifeRate = levelDTO.LifeRate,
                CalculationType = (Enums.CalculationTypes)levelDTO.CalculationType,
                ApplyOnType = (Enums.ApplyOnTypes)levelDTO.ApplyOnType,
                AnnualAddedLimit = levelDTO.AnnualAddedLimit,
                PremiumType = (Enums.PremiumTypes)levelDTO.PremiumType,
                ContractLevelCompanies = levelCompanies

            };
        }

        /// <summary>
        /// CreateLevels
        /// </summary>
        /// <param name="levelDTOs"></param>
        /// <returns>List<Level></returns>
        internal static List<Level> CreateLevels(List<LevelDTO> levelDTOs)
        {
            List<Level> Levels = new List<Level>();

            foreach (LevelDTO tempLevelDTO in levelDTOs)
            {
                Levels.Add(CreateLevel(tempLevelDTO));
            }
            return Levels;
        }

        /// <summary>
        /// CreateLevelCompany
        /// </summary>
        /// <param name="levelCompanyDTO"></param>
        /// <returns>LevelCompany</returns>
        internal static LevelCompany CreateLevelCompany(LevelCompanyDTO levelCompanyDTO)
        {

            Agent agent = new Agent()
            {
                IndividualId = levelCompanyDTO.Agent.IndividualId,
                FullName = levelCompanyDTO.Agent.FullName
            };

            Company company = new Company()
            {
                IndividualId = levelCompanyDTO.Company.IndividualId,
                FullName = levelCompanyDTO.Company.Name
            };

            return new LevelCompany
            {
                LevelCompanyId = levelCompanyDTO.LevelCompanyId,
                Agent = agent,
                Company = company,
                GivenPercentage = levelCompanyDTO.GivenPercentage,
                ComissionPercentage = levelCompanyDTO.ComissionPercentage,
                ContractLevel = new Level()
                {
                    ContractLevelId = levelCompanyDTO.ContractLevel.ContractLevelId
                },
                InterestReserveRelease = levelCompanyDTO.InterestReserveRelease,
                ReservePremiumPercentage = levelCompanyDTO.ReservePremiumPercentage,
                AdditionalCommissionPercentage = levelCompanyDTO.AdditionalCommissionPercentage,
                DragLossPercentage = levelCompanyDTO.DragLossPercentage,
                ReinsuranceExpensePercentage = levelCompanyDTO.ReinsuranceExpensePercentage,
                UtilitySharePercentage = levelCompanyDTO.UtilitySharePercentage,
                PresentationInformationType = (Enums.PresentationInformationTypes)levelCompanyDTO.PresentationInformationType,
                IntermediaryCommission = levelCompanyDTO.IntermediaryCommission,
                ClaimCommissionPercentage = levelCompanyDTO.ClaimCommissionPercentage,
                DifferentialCommissionPercentage = levelCompanyDTO.DifferentialCommissionPercentage

            };
        }

        /// <summary>
        /// CreateLevelCompanies
        /// </summary>
        /// <param name="LevelCompanyDTOs"></param>
        /// <returns>List<LevelCompany></returns>
        internal static List<LevelCompany> CreateLevelCompanies(List<LevelCompanyDTO> LevelCompanyDTOs)
        {
            List<LevelCompany> levelCompanies = new List<LevelCompany>();

            foreach (LevelCompanyDTO tempLevelCompanyDTO in LevelCompanyDTOs)
            {
                levelCompanies.Add(CreateLevelCompany(tempLevelCompanyDTO));
            }
            return levelCompanies;
        }

        /// <summary>
        /// CreateContract
        /// </summary>
        /// <param name="contractDTO"></param>
        /// <returns>Contract</returns>
        internal static Contract CreateContract(ContractDTO contractDTO)
        {

            return new Contract
            {

                ContractId = contractDTO.ContractId,
                ContractType = new ContractType()
                {
                    ContractTypeId = contractDTO.ContractType.ContractTypeId
                },
                Currency = new Currency()
                {
                    Id = contractDTO.CurrencyId
                },

                DateFrom = contractDTO.DateFrom,
                DateTo = contractDTO.DateTo,
                Description = contractDTO.Description,
                SmallDescription = contractDTO.SmallDescription,
                Year = contractDTO.Year,
                ReleaseTimeReserve = contractDTO.ReleaseTimeReserve,
                AffectationType = new AffectationType()
                {
                    Id = Convert.ToInt32(contractDTO.AffectationType?.Id)
                },
                ResettlementType = new ResettlementType()
                {
                    Id = Convert.ToInt32(contractDTO.ResettlementType?.Id)
                },
                PremiumAmount = contractDTO.PremiumAmount,
                EPIType = new EPIType()
                {
                    Id = Convert.ToInt32(contractDTO.EPIType?.Id)
                },
                Enabled = contractDTO.Enabled,
                GroupContract = contractDTO.GroupContract,
                CoInsurancePercentage = contractDTO.CoInsurancePercentage,
                RisksNumber = contractDTO.RisksNumber,
                EstimatedDate = contractDTO.EstimatedDate
            };
        }

        /// <summary>
        /// CreateLevelPayment
        /// </summary>
        /// <param name="levelPaymentDTO"></param>
        /// <returns>LevelPayment</returns>
        internal static LevelPayment CreateLevelPayment(LevelPaymentDTO levelPaymentDTO)
        {
            Level level = new Level();
            level.ContractLevelId = levelPaymentDTO.Level.ContractLevelId;

            return new LevelPayment
            {
                Id = levelPaymentDTO.Id,
                Level = level,
                Number = levelPaymentDTO.Number,
                Amount = new Amount { Value = Convert.ToDecimal(levelPaymentDTO.Amount) },
                Date = levelPaymentDTO.Date
            };
        }

        /// <summary>
        /// CreateLevelRestore
        /// </summary>
        /// <param name="levelRestoreDTO"></param>
        /// <returns>LevelRestore</returns>
        internal static LevelRestore CreateLevelRestore(LevelRestoreDTO levelRestoreDTO)
        {
            Level level = new Level();
            level.ContractLevelId = levelRestoreDTO.Level.ContractLevelId;

            return new LevelRestore
            {
                Id = levelRestoreDTO.Id,
                Level = level,
                Number = levelRestoreDTO.Number,
                RestorePercentage = levelRestoreDTO.RestorePercentage,
                NoticePercentage = levelRestoreDTO.NoticePercentage
            };
        }

        /// <summary>
        /// CreateInstallment
        /// </summary>
        /// <param name="installmentDTO"></param>
        /// <returns>Installment</returns>
        internal static Installment CreateInstallment(InstallmentDTO installmentDTO)
        {
            LevelCompany levelCompany = new LevelCompany();

            if (installmentDTO.LevelCompany != null)
            {
                levelCompany.LevelCompanyId = installmentDTO.LevelCompany.LevelCompanyId;

                levelCompany.Agent = new Agent()
                {
                    IndividualId = installmentDTO.LevelCompany.Agent.IndividualId,
                    FullName = installmentDTO.LevelCompany.Agent.FullName
                };

                levelCompany.Company = new Company()
                {
                    IndividualId = installmentDTO.LevelCompany.Company.IndividualId,
                    FullName = installmentDTO.LevelCompany.Company.Name
                };

                levelCompany.GivenPercentage = installmentDTO.LevelCompany.GivenPercentage;
                levelCompany.ComissionPercentage = installmentDTO.LevelCompany.ComissionPercentage;

                if (installmentDTO.LevelCompany.ContractLevel != null)
                {
                    levelCompany.ContractLevel = new Level()
                    {
                        //ContractLevelId = levelCompanyDTO.ContractLevel.ContractLevelId
                        AssignmentPercentage = installmentDTO.LevelCompany.ContractLevel.AssignmentPercentage
                    };

                }

                levelCompany.DepositPercentage = installmentDTO.LevelCompany.DepositPercentage;
                levelCompany.InterestOnReserve = installmentDTO.LevelCompany.InterestOnReserve;
                levelCompany.DepositReleaseDate = installmentDTO.LevelCompany.DepositReleaseDate;
            }


            return new Installment
            {
                Id = installmentDTO.Id,
                InstallmentNumber = installmentDTO.InstallmentNumber,
                PaidAmount = new Amount() { Value = installmentDTO.PaidAmount.Value },
                PaidDate = installmentDTO.PaidDate,
                LevelCompany = levelCompany
            };
        }

        internal static TempLayerDistributions CreateTempLayerDistributionsByLayerIssuance(ReinsuranceLayerIssuanceDTO reinsuranceLayerIssuanceDTO)
        {
            return new TempLayerDistributions()
            {
                TmpReinsuranceProcessId = reinsuranceLayerIssuanceDTO.TemporaryIssueId,
                LayerNumber = reinsuranceLayerIssuanceDTO.LayerNumber,
                LayerPercentage = reinsuranceLayerIssuanceDTO.LayerPercentage,
                SumAmount = Convert.ToDecimal(reinsuranceLayerIssuanceDTO.LayerAmount.ToString() == "" ? "0" : reinsuranceLayerIssuanceDTO.LayerAmount.ToString()),
                PremiumPercentage = reinsuranceLayerIssuanceDTO.PremiumPercentage,
                PremiumAmount = Convert.ToDecimal(reinsuranceLayerIssuanceDTO.PremiumAmount.ToString() == "" ? "0" : reinsuranceLayerIssuanceDTO.PremiumAmount.ToString()),
                TempIssueLayerId = reinsuranceLayerIssuanceDTO.ReinsuranceLayerId
            };
        }

        internal static TempLineCumulusIssuance CreateTempLineCumulusIssuance(ReinsuranceLineDTO reinsuranceLineDTO)
        {
            return new TempLineCumulusIssuance()
            {
                TempLayerLineId = reinsuranceLineDTO.ReinsuranceLineId,
                LineId = reinsuranceLineDTO.Line.LineId,
                LineDescription = reinsuranceLineDTO.Line.Description,
                CumulusType = reinsuranceLineDTO.Line.CumulusType.Description,
                CumulusKey = reinsuranceLineDTO.CumulusKey,
                RetainedSum = reinsuranceLineDTO.ReinsuranceAllocations[0].Amount.Value,
                GivenSum = reinsuranceLineDTO.ReinsuranceAllocations[0].Premium.Value
            };
        }

        internal static TempReinsuranceProcess CreateTempReinsuranceProcess(TempReinsuranceProcessDTO tempReinsuranceProcessDTO, double progress, double elapsed, double minElapsed)
        {
            return new TempReinsuranceProcess()
            {
                TempReinsuranceProcessId = tempReinsuranceProcessDTO.TempReinsuranceProcessId,
                Description = tempReinsuranceProcessDTO.Description,
                RecordsNumber = tempReinsuranceProcessDTO.RecordsNumber,
                RecordsProcessed = tempReinsuranceProcessDTO.RecordsProcessed,
                RecordsFailed = tempReinsuranceProcessDTO.RecordsFailed,
                Progress = progress.ToString("P", CultureInfo.InvariantCulture),
                Elapsed = Math.Truncate(elapsed) + " h " + Math.Truncate(minElapsed) + " m",
                Status = tempReinsuranceProcessDTO.Status
            };
        }
        
        internal static TempAllocation CreateTempAllocation(ReinsuranceAllocationDTO reinsuranceAllocationDTO, ReinsuranceLineDTO reinsuranceLineDTO, decimal totSum, decimal totPremium)
        {
            return new TempAllocation()
            {
                TmpLayerLineId = reinsuranceLineDTO.ReinsuranceLineId,
                TmpIssueAllocationId = reinsuranceAllocationDTO.ReinsuranceAllocationId,
                ContractDescription = reinsuranceAllocationDTO.Contract.SmallDescription,
                Layer = reinsuranceAllocationDTO.Contract.ContractLevels[0].Number,
                Sum = Convert.ToDecimal(reinsuranceAllocationDTO.Amount.Value.ToString() == "" ? "0" : reinsuranceAllocationDTO.Amount.Value.ToString()),
                Premium = Convert.ToDecimal(reinsuranceAllocationDTO.Premium.Value.ToString() == "" ? "0" : reinsuranceAllocationDTO.Premium.Value.ToString()),
                TotSum = totSum,
                TotPremium = totPremium,
                Facultative = reinsuranceAllocationDTO.Facultative
            };
        }

        internal static List<ModuleDateDTO> CreateModuleDTOIntegration(List<TEMPINTDTOs.ModuleDateDTO> moduleDateDTO)
        {
            List<ModuleDateDTO> moduleDateDTOs = new List<ModuleDateDTO>();
            foreach (TEMPINTDTOs.ModuleDateDTO item in moduleDateDTO)
            {
                moduleDateDTOs.Add(new ModuleDateDTO
                {
                    Id = item.Id,
                    Description = item.Description,
                    CeilingYyyy = item.CeilingYyyy,
                    CeilingMm = item.CeilingMm,
                    LastClosingYyyy = item.LastClosingYyyy,
                    LastClosingMm = item.LastClosingMm,
                    OfflineCeilingYyyy = item.OfflineCeilingYyyy,
                    OfflineCeilingMm = item.OfflineCeilingMm
                });
            }

            return moduleDateDTOs;
        }

        internal static List<EndorsementDTO> CreateEndorsementDTOsByTEMPIntegrationEndorsementDTOs(List<TEMPINTDTOs.EndorsementDTO> integrationEndorsementDTOs)
        {
            List<EndorsementDTO> endorsementDTOs = new List<EndorsementDTO>();

            foreach (TEMPINTDTOs.EndorsementDTO endorsementDTO in integrationEndorsementDTOs)
            {
                endorsementDTOs.Add(CreateEndorsementDTOByTEMPIntegrationEndorsementDTO(endorsementDTO));
            }
            return endorsementDTOs;
        }

        internal static EndorsementDTO CreateEndorsementDTOByTEMPIntegrationEndorsementDTO(TEMPINTDTOs.EndorsementDTO integrationEndorsementDTO)
        {
            return new EndorsementDTO
            {
                Currency = integrationEndorsementDTO.Currency,
                CurrentFrom = Convert.ToDateTime(integrationEndorsementDTO.CurrentFrom),
                CurrentTo = Convert.ToDateTime(integrationEndorsementDTO.CurrentTo),
                Description = integrationEndorsementDTO.Description,
                EndorsementId = integrationEndorsementDTO.EndorsementId,
                EndorsementNumber = integrationEndorsementDTO.EndorsementNumber,
                InsuredAmount = integrationEndorsementDTO.InsuredAmount,
                InsuredCd = integrationEndorsementDTO.InsuredCd,
                InsuredName = integrationEndorsementDTO.InsuredName,
                IssueDate = Convert.ToDateTime(integrationEndorsementDTO.IssueDate),
                OperationType = integrationEndorsementDTO.OperationType,
                PolicyId = integrationEndorsementDTO.PolicyId,
                Prime = integrationEndorsementDTO.Prime,
                ResponsibilityMaximumAmount = integrationEndorsementDTO.ResponsibilityMaximumAmount
            };
        }

        internal static DTOs.InsuredDTO CreateInsuredByIndividualId(IntegrationInsured integrationInsured)
        {
            return new DTOs.InsuredDTO()
            {
                IndividualId = integrationInsured.IndividualId,
                FullName = integrationInsured.FullName,
                IdentificationNumber = integrationInsured.IdentificationNumber,
                CustomerType = Convert.ToInt32(integrationInsured.CustomerType),
                DeclinedDate = integrationInsured.DeclinedDate
            };
        }

        internal static List<DTOs.IndividualDTO> CreateMapIndividualIntegration(List<TEMPINTDTOs.IndividualDTO> individualDTOs)
        {
            List<DTOs.IndividualDTO> individualDTOS = new List<DTOs.IndividualDTO>();
            foreach (TEMPINTDTOs.IndividualDTO individualDTO in individualDTOs)
            {
                individualDTOS.Add(new DTOs.IndividualDTO
                {
                    IndividualId = individualDTO.IndividualId,
                    IndividualTypeId = individualDTO.IndividualTypeId,
                    DocumentNumber = individualDTO.DocumentNumber,
                    DocumentTypeId = individualDTO.DocumentTypeId,
                    Name = individualDTO.Name,
                });
            }

            return individualDTOS;
        }
        
        internal static IMapper CreateMapIssueAllocationRiskCoverage()
        {
            var config = MapperCache.GetMapper<BusinessCollection, IssueAllocationRiskCover>(cfg =>
            {
                cfg.CreateMap<REINSEN.IssueAllocationRiskCover, IssueAllocationRiskCover>()
                .ForMember(dest => dest.IndividualCd, opt => opt.MapFrom(src => src.IndividualCode))
                .ForMember(dest => dest.InsuredCd, opt => opt.MapFrom(src => src.InsuredCode))
                .ForMember(dest => dest.LineBusinessCd, opt => opt.MapFrom(src => src.LineBusinessCode))
                .ForMember(dest => dest.SubLineBusinessCd, opt => opt.MapFrom(src => src.SubLineBusinessCode))
                .ForMember(dest => dest.ContractCurrencyCd, opt => opt.MapFrom(src => src.ContractCurrencyCode))
                .ForMember(dest => dest.PrefixCd, opt => opt.MapFrom(src => src.PrefixCode))
                .ForMember(dest => dest.BranchCd, opt => opt.MapFrom(src => src.BranchCode));
            });
            return config;
        }

        internal static IssueAllocationRiskCover CreateIssueAllocationRiskCoverage(this REINSEN.IssueAllocationRiskCover entityIssueAllocationRiskCover)
        {
            var config = CreateMapIssueAllocationRiskCoverage();
            return config.Map<REINSEN.IssueAllocationRiskCover, IssueAllocationRiskCover>(entityIssueAllocationRiskCover);
        }

        internal static List<IssueAllocationRiskCover> CreateIssueAllocationRiskCoverages(this BusinessCollection entityIssueAllocationRiskCovers)
        {
            var config = CreateMapIssueAllocationRiskCoverage();
            return config.Map<BusinessCollection, List<IssueAllocationRiskCover>>(entityIssueAllocationRiskCovers);
        }


        internal static List<TempRiskCoverage> CreateTempRisksCoverages(BusinessCollection businessObjects)
        {
            List<TempRiskCoverage> tempRiskCoverages = new List<TempRiskCoverage>();

            foreach (REINSEN.TempRiskCoverage entityTempRiskCoverage in businessObjects)
            {
                tempRiskCoverages.Add(CreateTempRiskCoverage(entityTempRiskCoverage));
            }

            return tempRiskCoverages;

        }

        internal static TempRiskCoverage CreateTempRiskCoverage(REINSEN.TempRiskCoverage entityTempRiskCoverage)
        {
            return new TempRiskCoverage
            {
                CoverageId = entityTempRiskCoverage.CoverageId,
                RiskCode = Convert.ToInt32(entityTempRiskCoverage.RiskCode),
                IndividualCode = entityTempRiskCoverage.IndividualCode,
                LineBusinessCode = entityTempRiskCoverage.LineBusinessCode
            };
        }

        internal static List<TempIssue> CreateTempIssues(BusinessCollection businessObjects)
        {
            List<TempIssue> tempIssues = new List<TempIssue>();

            foreach (REINSEN.TempIssue entityTempIssue in businessObjects)
            {
                tempIssues.Add(CreateTempIssue(entityTempIssue));
            }

            return tempIssues;
        }

        internal static TempIssue CreateTempIssue(REINSEN.TempIssue entityTempIssue)
        {
            return new TempIssue
            {
                ProcessId = entityTempIssue.TempReinsuranceProcessCode,
                PolicyId = Convert.ToInt32(entityTempIssue.PolicyId),
                EndorsementId = entityTempIssue.EndorsementCode,
            };
        }

        internal static List<ReportType> CreateReportTypes(BusinessCollection businessObjects)
        {
            List<ReportType> reportTypes = new List<ReportType>();
            foreach (REINSEN.ReportType reportType in businessObjects)
            {
                reportTypes.Add(CreateReportType(reportType));
            }

            return reportTypes;
        }

        internal static ReportType CreateReportType(REINSEN.ReportType entityReportType)
        {
            return new ReportType
            {
                Id = entityReportType.ReportTypeId,
                Description = entityReportType.Description,
                Enable = entityReportType.Enable
            };
        }
    }
}

