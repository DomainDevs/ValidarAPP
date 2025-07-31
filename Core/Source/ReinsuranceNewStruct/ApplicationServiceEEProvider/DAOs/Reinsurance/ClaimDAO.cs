using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using REINSEN = Sistran.Core.Application.Reinsurance.Entities;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using sp = Sistran.Core.Framework.DAF.Engine.StoredProcedure;
using Sistran.Core.Framework.Queries;
using System.Data;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.Contexts;
using Sistran.Co.Application.Data;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums;
using System.Xml.Linq;
using System.Xml;
using Sistran.Core.Application.TempCommonServices.Models;
using System.Globalization;
using Sistran.Core.Framework.Views;
using System.Configuration;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{
    internal class ClaimDAO
    {
        #region Instance variables
        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        private int RowsGrid = 200;
        private IDbConnection connection = null; //Defino la Conexión
        private IDbTransaction transaction = null; //Defino la Transacción
        private IDataFacade dataFacade;            //Defino el DataFacade   
        #endregion
        
        public List<Claim> GetClaims(int branchCode, int prefixCode, decimal policyNumber, int? claimNumber)
        {
            List<Claim> claims = new List<Claim>();
            CultureInfo cultureInfo = new CultureInfo(ConfigurationManager.AppSettings["DefaultCultureInfo"].ToString());
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.GetClaims.Properties.ClaimBranchCode, branchCode);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(REINSEN.GetClaims.Properties.PrefixCode, prefixCode);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(REINSEN.GetClaims.Properties.PolicyNumber, policyNumber);

            if (claimNumber > 0)
            {
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(REINSEN.GetClaims.Properties.ClaimNumber, claimNumber);
            }

            UIView claimRequests = _dataFacadeManager.GetDataFacade().GetView("GetClaims", criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out RowsGrid);

            foreach (DataRow claimRequest in claimRequests)
            {
                Claim claim = new Claim();
                claim.Modifications = new List<ClaimModify>()
                    {
                        new ClaimModify(){
                            Id = Convert.ToInt32(claimRequest["ClaimModifyCode"]),
                            Coverages = new List<ClaimCoverage>(),
                            RegistrationDate = Convert.ToDateTime(claimRequest["RegistrationDate"], cultureInfo),
                }
                    };
                ClaimCoverage claimCoverage = new ClaimCoverage();
                claimCoverage.Id = Convert.ToInt32(claimRequest["ClaimCoverageCd"]);
                claimCoverage.SubClaim = Convert.ToInt32(claimRequest["SubClaim"]);
                claimCoverage.Estimations = new List<Estimation>();
                claimCoverage.Estimations.Add(new Estimation()
                {
                    PaymentAmount = Convert.ToDecimal(claimRequest["EstimationAmount"]),
                    Type = new EstimationType()
                    {
                        Id = Convert.ToInt32(claimRequest["MovementSourceId"]),
                        Description = claimRequest["EstimationType"].ToString()
                    },
                    Version = Convert.ToInt32(claimRequest["VersionCd"])
                });

                Branch branch = new Branch() { Id = Convert.ToInt32(claimRequest["ClaimBranchCode"]) };
                Prefix prefix = new Prefix() { Id = Convert.ToInt32(claimRequest["PrefixCode"]) };

                claim.Modifications.First().Coverages = new List<ClaimCoverage>();
                claim.Modifications.First().Coverages.Add(claimCoverage);

                claim.Branch = branch;
                claim.OccurrenceDate = Convert.ToDateTime(claimRequest["ClaimDate"], cultureInfo);
                claim.Id = Convert.ToInt32(claimRequest["ClaimCode"]);
                claim.Number = Convert.ToInt32(claimRequest["ClaimNumber"]);
                claim.Prefix = prefix;
                claim.Endorsement = new ClaimEndorsement();
                claim.Endorsement.PolicyNumber = Convert.ToDecimal(claimRequest["PolicyNumber"]);
                claim.CoveredRiskType = CommonService.Enums.CoveredRiskType.Location; // 2
                claims.Add(claim);
            }
            return claims;
        }
        
        public List<ClaimDistribution> GetReinsuranceClaimDistributionByClaimCodeClaimModifyCode(int claimCode, int claimModifyCode, int movementSourceId, int claimCoverageCd)
        {
            List<ClaimDistribution> claimDistributions = new List<ClaimDistribution>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.GetClaimDistribution.Properties.ClaimCode, claimCode).And();
            criteriaBuilder.PropertyEquals(REINSEN.GetClaimDistribution.Properties.ClaimModifyCode, claimModifyCode).And();
            criteriaBuilder.PropertyEquals(REINSEN.GetClaimDistribution.Properties.MovementSourceId, movementSourceId).And();
            criteriaBuilder.PropertyEquals(REINSEN.GetClaimDistribution.Properties.ClaimCoverageCd, claimCoverageCd);


            UIView claimDistributionsView = _dataFacadeManager.GetDataFacade().GetView("GetClaimDistribution", criteriaBuilder.GetPredicate(), null, 0, 50, null, false, out RowsGrid);

            foreach (DataRow row in claimDistributionsView.Rows)
            {
                ClaimDistribution claimDistribution = new ClaimDistribution();

                claimDistribution.ClaimCode = Convert.ToInt32(row["ClaimCode"]);
                claimDistribution.ClaimModifyCode = Convert.ToInt32(row["ClaimModifyCode"]);
                claimDistribution.ReinsuranceNumber = Convert.ToInt32(row["ReinsuranceNumber"]);
                claimDistribution.LayerNumber = Convert.ToInt32(row["LayerNumber"]);
                claimDistribution.LineDescription = row["LineDescription"].ToString();
                claimDistribution.LineId = Convert.ToInt32(row["LineId"]);
                claimDistribution.CumulusKey = row["CumulusKey"].ToString();
                claimDistribution.Contract = row["Contract"].ToString();
                claimDistribution.LevelNumber = Convert.ToInt32(row["LevelNumber"]);
                claimDistribution.TradeName = row["TradeName"].ToString();
                claimDistribution.Amount = row["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(row["Amount"]);
                claimDistribution.MovementSource = row["MovementSource"].ToString();
                claimDistribution.MovementSourceId = Convert.ToInt32(row["MovementSourceId"]);
                claimDistributions.Add(claimDistribution);

            }

            return claimDistributions;
        }
        
        public Models.Reinsurance.Reinsurance ReinsuranceClaim(int claimCode, int claimModifyCode, int userId)
        {
            dataFacade = _dataFacadeManager.GetDataFacade();     
            this.connection = dataFacade.GetCurrentConnection(); 

            using (Context.Current)
            {
                try
                {
                    this.transaction = connection.BeginTransaction();
                    sp.StoredProcedure storedProcedure = new sp.StoredProcedure(connection, this.transaction);
                    NameValue[] parameters = new NameValue[3];
                    parameters[0] = new NameValue("CLAIM_CD", claimCode);
                    parameters[1] = new NameValue("CLAIM_MODIFY_CD", claimModifyCode);
                    parameters[2] = new NameValue("USER_ID", userId);

                    DataTable result;
                    using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                    {
                        result = dynamicDataAccess.ExecuteSPDataTable("REINS.CLAIMS_AUTOMATIC_PROCESS", parameters);
                    }

                    Models.Reinsurance.Reinsurance reinsurance = new Models.Reinsurance.Reinsurance();

                    foreach (DataRow process in result.Rows)
                    {
                        reinsurance.ReinsuranceId = int.Parse(process[0].ToString());
                        reinsurance.Number = int.Parse(process[2].ToString());
                    }

                    transaction.Commit();
                    return reinsurance;
                }
                catch (BusinessException ex)
                {
                    transaction.Rollback();
                    throw new BusinessException(ex.Message, ex);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }
        
        public Models.Reinsurance.Reinsurance LoadReinsuranceClaim(int? claimCode, int userId, int processType, DateTime dateFrom, DateTime dateTo, List<Prefix> prefixes)
        {
            NameValue[] parameters = new NameValue[8];

            parameters[0] = new NameValue("CLAIM_CD", claimCode); // DBNull.Value
            parameters[1] = new NameValue("DATE_FROM", dateFrom.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            parameters[2] = new NameValue("DATE_TO", dateTo.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            parameters[3] = new NameValue("PROCESS_TYPE", (int)ProcessTypes.Massive); //Proceso 3
            parameters[4] = new NameValue("USER_ID", userId);

            #region XmlPrefix


            XDocument xDocumentPrefix = new XDocument(new XElement("PrefixMassive", new XElement("fill", new XElement("option", 0))));

            if (processType == (int)ProcessTypes.Massive && prefixes.Count > 0)
            {
                foreach (Prefix prefix in prefixes)
                {
                    xDocumentPrefix.Element("PrefixMassive").Element("fill").AddAfterSelf(new XElement("Prefixes", new XElement("prefixCd", prefix.Id.ToString())));
                }
            }

            XmlDocument xmlDocumentPrefix = new XmlDocument();
            xmlDocumentPrefix.LoadXml(xDocumentPrefix.ToString());

            #endregion

            parameters[5] = new NameValue("PREFIX_XML", xmlDocumentPrefix.DocumentElement.OuterXml); // Lista de Ramo
            parameters[6] = new NameValue("SERVICE", 0);
            parameters[7] = new NameValue("CLAIM_MODIFY_CD", 0);


            DataTable processes;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                processes = dynamicDataAccess.ExecuteSPDataTable("REINS.CLM_AUTOPROC1_WORKTABLES_LOAD", parameters);
            }
            Models.Reinsurance.Reinsurance reinsurance = new Models.Reinsurance.Reinsurance();

            foreach (DataRow process in processes.Rows)
            {
                reinsurance.ReinsuranceId = int.Parse(process[0].ToString()); // Id correspondiente al reaseguro.
                reinsurance.Number = int.Parse(process[2].ToString());        // Reaseguro de Emisión
            }

            return reinsurance;
        }

        public DataTable WortableLoadClaims(int claimCd, int claimModifyCd, int userId, int service)
        {
            NameValue[] parameters = new NameValue[8];
            parameters[0] = new NameValue("CLAIM_CD", claimCd);
            parameters[1] = new NameValue("CLAIM_MODIFY_CD", claimModifyCd);
            parameters[2] = new NameValue("DATE_FROM", ""); 
            parameters[3] = new NameValue("DATE_TO", ""); 
            parameters[4] = new NameValue("PROCESS_TYPE", (int)ProcessTypes.Manual);
            parameters[5] = new NameValue("USER_ID", userId);
            parameters[6] = new NameValue("PREFIX_XML", "");
            parameters[7] = new NameValue("SERVICE", service);
            
            DataTable result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("REINS.CLM_AUTOPROC1_WORKTABLES_LOAD", parameters);
            }

            return result;
        }

        public Cumulus GetParametrizationClaimsCumulus(int claimCd, int claimsType)
        {
            Cumulus cumulus = new Cumulus();

            /*Cargar Objetos Cumulos----------------------------------------------------------------------------------*/
            NameValue[] parameters = new NameValue[2];
            parameters[0] = new NameValue("ID", claimCd);
            parameters[1] = new NameValue("CLAIM", 1);

            DataTable cumulusPayment;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                cumulusPayment = dynamicDataAccess.ExecuteSPDataTable("REINS.CLM_PYM_CUMULUS", parameters);
            }

            int movementId = 0;
            int claimsCumulusId = 0;
            List<IssueLayer> issueLayers = new List<IssueLayer>();

            foreach (DataRow process in cumulusPayment.Rows)
            {
                List<IssueAllocation> issueAllocation = new List<IssueAllocation>();
                List<Level> level = new List<Level>();
                List<IssueLayerLine> issueLayerLines = new List<IssueLayerLine>();
                Line line = new Line() { LineId = Convert.ToInt32(process[8]) }; //LINE_ID

                movementId = Convert.ToInt32(process[1]);                              //MOVEMENT_TYPE
                claimsCumulusId = Convert.ToInt32(process[2]);                         //TMP_CLAIM_CUMULUS_ID

                level.Add(new Level() { ContractLevelId = Convert.ToInt32(process[4]) }); //CONTRACT_LEVEL_ID
                Contract contract = new Contract() { ContractLevels = level };

                issueAllocation.Add(new IssueAllocation()
                {
                    Id = Convert.ToInt32(process[0]),                                  //TMP_CLAIM_ALLOCATION_ID
                    Currency = new Currency() { Id = Convert.ToInt32(process[5]) },    //CURRENCY_ID
                    Facultative = Convert.ToBoolean(process[3]),                       //IS_FACULTATIVE
                    Amount = new Amount() { Value = Convert.ToDecimal(process[6]) },     //CUMULUS_AMOUNT
                    ContractCompany = contract
                });

                issueLayerLines.Add(new IssueLayerLine()
                {
                    IssueAllocations = issueAllocation,
                    Line = line
                });
                issueLayers.Add(new IssueLayer() { IssueLayerLines = issueLayerLines });
            }

            cumulus.Id = claimsCumulusId;
            cumulus.MovementId = movementId;
            cumulus.IssueLayers = issueLayers;

            return cumulus;
        }

        public ReinsurancePaymentClaim GetReinsuranceClaim(int claimId)
        {
            List<ReinsuranceLine> reinsuranceLines = new List<ReinsuranceLine>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.GetClmTmpAllocation.Properties.ClaimCode, claimId);

            UIView data = _dataFacadeManager.GetDataFacade().GetView("GetClmTmpAllocation", criteriaBuilder.GetPredicate(), null, 0, 50, null, false, out RowsGrid);

            int tmpClaimLayerId = 0;
            int tmpClaimCumulusId = 0;
            int tmpClaimAllocationId = 0;
            int count = 0;

            ReinsurancePaymentClaim reinsurancePaymentClaim = new ReinsurancePaymentClaim();
            List<ReinsurancePaymentClaimLayer> reinsurancePaymentClaimsLayers = new List<ReinsurancePaymentClaimLayer>();
            List<ReinsurancePaymentClaimCumulus> reinsurancePaymentClaimsCumulus = new List<ReinsurancePaymentClaimCumulus>();
            List<ReinsurancePaymentClaimAllocation> reinsurancePaymentClaimsAllocation = new List<ReinsurancePaymentClaimAllocation>();

            ReinsurancePaymentClaimLayer reinsurancePaymentClaimLayer = new ReinsurancePaymentClaimLayer();
            ReinsurancePaymentClaimCumulus reinsurancePaymentClaimCumulus = new ReinsurancePaymentClaimCumulus();

            foreach (DataRow allocationEntity in data.Rows)
            {
                List<Level> level = new List<Level>();

                level.Add(new Level() { ContractLevelId = Convert.ToInt32(allocationEntity["LevelCode"]) });

                if (count == 0)
                {
                    tmpClaimLayerId = Convert.ToInt32(allocationEntity["TempClaimLayerId"]);
                    tmpClaimCumulusId = Convert.ToInt32(allocationEntity["TempClaimCumulusId"]);
                    tmpClaimAllocationId = Convert.ToInt32(allocationEntity["TempClaimAllocationId"]);
                }

                if (tmpClaimAllocationId != Convert.ToInt32(allocationEntity["TempClaimAllocationId"]))
                {
                    reinsurancePaymentClaimCumulus.ReinsurancePaymentClaimAllocation = reinsurancePaymentClaimsAllocation;
                    reinsurancePaymentClaimsAllocation = new List<ReinsurancePaymentClaimAllocation>();
                    reinsurancePaymentClaimsCumulus.Add(reinsurancePaymentClaimCumulus);
                    reinsurancePaymentClaimCumulus = new ReinsurancePaymentClaimCumulus();
                    tmpClaimAllocationId = Convert.ToInt32(allocationEntity["TempClaimAllocationId"]);
                }

                if (tmpClaimCumulusId != Convert.ToInt32(allocationEntity["TempClaimCumulusId"]))
                {
                    reinsurancePaymentClaimLayer.ReinsurancePaymentClaimCumulus = reinsurancePaymentClaimsCumulus;
                    reinsurancePaymentClaimsCumulus = new List<ReinsurancePaymentClaimCumulus>();
                    tmpClaimCumulusId = Convert.ToInt32(allocationEntity["TempClaimCumulusId"]);
                }
                if (tmpClaimLayerId != Convert.ToInt32(allocationEntity["TempClaimLayerId"]))
                {
                    reinsurancePaymentClaimsLayers.Add(reinsurancePaymentClaimLayer);
                    reinsurancePaymentClaimLayer = new ReinsurancePaymentClaimLayer();
                    tmpClaimLayerId = Convert.ToInt32(allocationEntity["TempClaimLayerId"]);
                }

                reinsurancePaymentClaimsAllocation.Add(new ReinsurancePaymentClaimAllocation()
                {
                    Id = Convert.ToInt32(allocationEntity["TempClaimAllocationId"]),                                            //TEMP_CLAIM_ALLOCATION_ID
                    ReinsuranceSourceId = Convert.ToInt32(allocationEntity["TempClaimReinsSourceId"]),                          //TEMP_CLAIM_REINS_SOURCE_ID
                    EstimationType = new EstimationType() { Id = Convert.ToInt32(allocationEntity["MovementSourceCode"]) },//MOVEMENT_SOURCE_CD
                    Currency = new Currency() { Id = Convert.ToInt32(allocationEntity["CurrencyCode"]) },                       //CURRENCY_CD
                    Facultative = Convert.ToBoolean(allocationEntity["IsFacultative"]),                                         //IS_FACULTATIVE
                    LevelCompanyId = 0,                                                                                         //PREGUNTAR
                    Amount = new Amount()
                    {
                        Value = allocationEntity["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(allocationEntity["Amount"])  //AMOUNT
                    }
                });

                reinsurancePaymentClaimCumulus.Id = Convert.ToInt32(allocationEntity["TempClaimCumulusId"]);                    //TEMP_CLAIM_CUMULUS_ID
                reinsurancePaymentClaimCumulus.Line = new Line();
                reinsurancePaymentClaimCumulus.Line.LineId = Convert.ToInt32(allocationEntity["LineCode"]);                     //LINE_CD
                reinsurancePaymentClaimCumulus.Line.ContractLines = new List<ContractLine>();
                reinsurancePaymentClaimCumulus.Line.ContractLines.Add(new ContractLine()
                {
                    Contract = new Contract() { ContractLevels = level }
                });
                reinsurancePaymentClaimCumulus.CumulusKey = allocationEntity["CumulusKey"].ToString();                          //CUMULUS_KEY

                reinsurancePaymentClaimLayer.Id = Convert.ToInt32(allocationEntity["TempClaimLayerId"]);                         //TEMP_CLAIM_LAYER_ID
                reinsurancePaymentClaimLayer.LayerNumber = Convert.ToInt32(allocationEntity["LayerNumber"]);                    //LAYER_NUMBER

                reinsurancePaymentClaim.Id = Convert.ToInt32(allocationEntity["TempReinsuranceProcessCode"]);                   //TEMP_REINSURANCE_PROCESS_CD
                reinsurancePaymentClaim.ReinsuranceNumber = Convert.ToInt32(allocationEntity["ReinsuranceNumber"]);             //REINSURANCE_NUMBER
                reinsurancePaymentClaim.Movement = Movements.Original;
                reinsurancePaymentClaim.ProcessDate = Convert.ToDateTime(allocationEntity["RegistrationDate"]);                 //REGISTRATION_DATE
                reinsurancePaymentClaim.ReinsuranceDate = Convert.ToDateTime(allocationEntity["ClaimDate"]);                    //CLAIM_DATE
                reinsurancePaymentClaim.IsAutomatic = false;
                count++;
            }
            reinsurancePaymentClaimCumulus.ReinsurancePaymentClaimAllocation = reinsurancePaymentClaimsAllocation;
            reinsurancePaymentClaimsCumulus.Add(reinsurancePaymentClaimCumulus);
            reinsurancePaymentClaimLayer.ReinsurancePaymentClaimCumulus = reinsurancePaymentClaimsCumulus;
            reinsurancePaymentClaimsLayers.Add(reinsurancePaymentClaimLayer);
            reinsurancePaymentClaim.ReinsurancePaymentClaimLayers = reinsurancePaymentClaimsLayers;

            return reinsurancePaymentClaim;
        }

        public List<ReinsuranceClaimLayer> GetClaimLayerByClaimIdClaimModifyId(int claimId, int claimModifyId, int movementSourceId, int claimCoverageCd)
        {
            List<ReinsuranceClaimLayer> reinsuranceClaimLayers = new List<ReinsuranceClaimLayer>();
            CultureInfo cultureInfo = new CultureInfo(ConfigurationManager.AppSettings["DefaultCultureInfo"].ToString());
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.ReinsuranceClaimLayerView.Properties.ClaimCode, claimId).And();
            criteriaBuilder.PropertyEquals(REINSEN.ReinsuranceClaimLayerView.Properties.ClaimModifyCode, claimModifyId).And();
            criteriaBuilder.PropertyEquals(REINSEN.ReinsuranceClaimLayerView.Properties.MovementSourceId, movementSourceId).And();
            criteriaBuilder.PropertyEquals(REINSEN.ReinsuranceClaimLayerView.Properties.ClaimCoverageCd, claimCoverageCd);

            UIView reinsuranceClaimLayerView = _dataFacadeManager.GetDataFacade().GetView("ReinsuranceClaimLayerView", criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out RowsGrid);

            if (reinsuranceClaimLayerView.Count > 0)
            {
                foreach (DataRow row in reinsuranceClaimLayerView)
                {
                    reinsuranceClaimLayers.Add(new ReinsuranceClaimLayer()
                    {
                        ClaimId = Convert.ToInt32(row["ClaimCode"]),
                        ClaimModifyId = Convert.ToInt32(row["ClaimModifyCode"]),
                        ClaimLayerId = Convert.ToInt32(row["ClaimLayerId"]),
                        ReinsuranceNumber = Convert.ToInt32(row["ReinsuranceNumber"]),
                        Description = row["Description"].ToString(),
                        ProcessDate = Convert.ToDateTime(row["ProcessDate"], cultureInfo),
                        RegistrationDate = Convert.ToDateTime(row["RegistrationDate"], cultureInfo),
                        IsAutomatic = Convert.ToString(row["IsAutomatic"]).ToUpper() == "TRUE" ? "SI" : "NO",
                        LayerNumber = Convert.ToInt32(row["LayerNumber"]),
                        Amount = Convert.ToDecimal(row["Amount"])
                    });
                }
            }
            return reinsuranceClaimLayers;
        }
        
        public List<ReinsuranceClaimDistribution> GetDistributionClaimByClaimLayerId(int claimLayerId, int movementSourceId)
        {
            List<ReinsuranceClaimDistribution> reinsuranceClaimDistributionDTOs = new List<ReinsuranceClaimDistribution>();
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.DistributionClaimLayerView.Properties.ClaimLayerId, claimLayerId);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(REINSEN.DistributionClaimLayerView.Properties.MovementSourceId, movementSourceId);

            UIView distributionsClaimLayer = _dataFacadeManager.GetDataFacade().GetView("DistributionClaimLayerView", criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out RowsGrid);

            if (distributionsClaimLayer.Count > 0)
            {
                foreach (DataRow itemDistribution in distributionsClaimLayer)
                {
                    reinsuranceClaimDistributionDTOs.Add(new ReinsuranceClaimDistribution()
                    {
                        ClaimLayerId = Convert.ToInt32(itemDistribution["ClaimLayerId"]),
                        Line = itemDistribution["Line"].ToString(),
                        CumulusKey = itemDistribution["CumulusKey"].ToString(),
                        Description = itemDistribution["MovementSource"].ToString(),
                        CurrencyDescription = itemDistribution["Currency"].ToString(),
                        IsFacultative = Convert.ToString(itemDistribution["IsFacultative"]).ToUpper() == "TRUE" ? "SI" : "NO",
                        Contract = itemDistribution["Contract"].ToString(),
                        LevelNumber = Convert.ToInt32(itemDistribution["LevelNumber"]),
                        Broker = itemDistribution["Broker"].ToString(),
                        Reinsurer = itemDistribution["Reinsurer"].ToString(),
                        Amount = itemDistribution["Amount"].ToString(),
                        MovementSourceId = Convert.ToInt32(itemDistribution["MovementSourceId"])
                    });
                }
            }

            return reinsuranceClaimDistributionDTOs;
        }

        public int SaveClaimReinsurance(int processId, int claimId, int claimModifyId, int userId)
        {
            int reinsuranceId = 0;
            NameValue[] parameters = new NameValue[5];

            parameters[0] = new NameValue("PROCESS_ID", processId);
            parameters[1] = new NameValue("IS_AUTOMATIC", 1);
            parameters[2] = new NameValue("USER_ID", userId);
            parameters[3] = new NameValue("CLAIM_CD", claimId);
            parameters[4] = new NameValue("CLAIM_MODIFY_CD", claimModifyId);

            DataTable result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("REINS.CLM_AUTOPROC3_RECORDING_FINAL_TABLES", parameters);
            }

            foreach (DataRow row in result.Rows)
            {
                reinsuranceId = int.Parse(row[0].ToString()); 
            }
            return reinsuranceId;
        }

        public bool ValidateClaimPaymentRequestReinsurance(int paymentRequestId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.GetPaymentDistribution.Properties.PaymentRequestCode, paymentRequestId);

            return _dataFacadeManager.GetDataFacade().GetView("GetPaymentDistribution", criteriaBuilder.GetPredicate(), null, 0, 50, null, false, out RowsGrid).Count > 0;
        }
    }
}
