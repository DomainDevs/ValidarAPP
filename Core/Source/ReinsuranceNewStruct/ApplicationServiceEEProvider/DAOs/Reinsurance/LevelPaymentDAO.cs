using System.Collections.Generic;
//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
//Sistran Core
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Assemblers;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Models.Reinsurance;
using REINSEN = Sistran.Core.Application.Reinsurance.Entities;
using sp = Sistran.Core.Framework.DAF.Engine.StoredProcedure;
using System.Data;
using System;
using System.Linq;
using Sistran.Core.Framework.Views;
using Sistran.Co.Application.Data;
using Sistran.Core.Application.ReinsuranceServices.EEProvider.Enums;
using System.Xml.Linq;
using System.Globalization;
using System.Xml;
using Sistran.Core.Application.ReinsuranceServices.DTOs;
using Sistran.Core.Framework.Contexts;
using Sistran.Core.Application.CommonService.Models;

namespace Sistran.Core.Application.ReinsuranceServices.EEProvider.DAOs.Reinsurance
{

    internal class LevelPaymentDAO
    {
        #region Instance variables

        readonly internal static IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;
        private int RowsGrid = 200;
        private IDbConnection connection = null; //Defino la Conexión
        private IDbTransaction transaction = null; //Defino la Transacción
        private IDataFacade dataFacade;            //Defino el DataFacade   
        #endregion

        #region Save
        /// <summary>
        /// SaveLevelPayment
        /// </summary>
        /// <param name="levelPayment"></param>
        public void SaveLevelPayment(LevelPayment levelPayment)
        {
            // Convertir de model a entity
            REINSEN.LevelPayment entityLevel = EntityAssembler.CreateLevelPayment(levelPayment);
            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().InsertObject(entityLevel);
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateLevelPayment
        /// </summary>
        /// <param name="levelPayment"></param>
        public void UpdateLevelPayment(LevelPayment levelPayment)
        {
            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSEN.LevelPayment.CreatePrimaryKey(levelPayment.Id);
            // Encuentra el objeto en referencia a la llave primaria
            REINSEN.LevelPayment entityLevelPayment = (REINSEN.LevelPayment)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
            entityLevelPayment.LevelCode = levelPayment.Level.ContractLevelId;
            entityLevelPayment.PaymentNumber = levelPayment.Number;
            entityLevelPayment.Amount = levelPayment.Amount.Value;
            entityLevelPayment.PaymentDate = levelPayment.Date;
            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().UpdateObject(entityLevelPayment);
        }
        #endregion

        #region Delete
        /// <summary>
        /// DeleteLevelPayment
        /// </summary>
        /// <param name="levelPaymentId"></param>
        public void DeleteLevelPayment(int levelPaymentId)
        {

            // Crea la Primary key con el código de la entidad
            PrimaryKey primaryKey = REINSEN.LevelPayment.CreatePrimaryKey(levelPaymentId);
            // Realizar las operaciones con los entities utilizando DAF
            REINSEN.LevelPayment entityLevelPayment = (REINSEN.LevelPayment)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
            // Realizar las operaciones con los entities utilizando DAF
            _dataFacadeManager.GetDataFacade().DeleteObject(entityLevelPayment);

        }

        #endregion

        #region Get
        
        public LevelPayment GetLevelPayment(int levelPaymentId)
        {
            
            PrimaryKey primaryKey = REINSEN.LevelPayment.CreatePrimaryKey(levelPaymentId);
            REINSEN.LevelPayment entityLevelPayment = (REINSEN.LevelPayment)
                (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));
            return ModelAssembler.CreateLevelPayment(entityLevelPayment);
        }
        
        public List<LevelPayment> GetLevelPaymentsByLevelId(int levelId)
        {

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.LevelPayment.Properties.LevelCode, levelId);
            
            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(
                                                    typeof(REINSEN.LevelPayment), criteriaBuilder.GetPredicate()));
            
            return ModelAssembler.CreateLevelPayments(businessCollection);

        }

        
        public List<ReinsurancePaymentLayer> GetPaymentLayerByPaymentRequestId(int paymentRequestId, int voucherConceptCd, int claimCoverageCd)
        {
            List<ReinsurancePaymentLayer> reinsurancePaymentLayerDTO = new List<ReinsurancePaymentLayer>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.PaymentReinsurance.Properties.PaymentRequestCode, paymentRequestId).And();
            criteriaBuilder.PropertyEquals(REINSEN.PaymentReinsurance.Properties.VoucherConceptCd, voucherConceptCd).And();
            criteriaBuilder.PropertyEquals(REINSEN.PaymentReinsurance.Properties.ClaimCoverageCd, claimCoverageCd);

            UIView data = _dataFacadeManager.GetDataFacade().GetView("GetReinsuranceLayers",
                            criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out RowsGrid);

            foreach (DataRow reinsurance in data)
            {
                reinsurancePaymentLayerDTO.Add(new ReinsurancePaymentLayer()
                {
                    PaymentRequestId = Convert.ToInt32(reinsurance["PaymentRequestCode"]),
                    PaymentLayerId = Convert.ToInt32(reinsurance["PaymentLayerId"]),
                    ReinsuranceNumber = Convert.ToInt32(reinsurance["ReinsuranceNumber"]),
                    Description = reinsurance["MovementType"].ToString(),
                    ProcessDate = Convert.ToDateTime(reinsurance["ProcessDate"].ToString()),
                    RegistrationDate = Convert.ToDateTime(reinsurance["RegistrationDate"].ToString()),
                    IsAutomatic = Convert.ToString(reinsurance["IsAutomatic"]).ToUpper() == "TRUE" ? "SI" : "NO",
                    LayerNumber = Convert.ToInt32(reinsurance["LayerNumber"]),
                    Amount = Convert.ToDecimal(reinsurance["Amount"])
                });
            }

            List<ReinsurancePaymentLayer> reinsurancePaymentLayer = reinsurancePaymentLayerDTO
                                                                .GroupBy(g => new
                                                                {
                                                                    g.PaymentRequestId,
                                                                    g.PaymentLayerId,
                                                                    g.ReinsuranceNumber,
                                                                    g.Description,
                                                                    g.ProcessDate,
                                                                    g.RegistrationDate,
                                                                    g.IsAutomatic,
                                                                    g.UserName,
                                                                    g.LayerNumber
                                                                })
                                                            .Select(cl => new ReinsurancePaymentLayer
                                                            {
                                                                PaymentRequestId = cl.First().PaymentRequestId,
                                                                PaymentLayerId = cl.First().PaymentLayerId,
                                                                ReinsuranceNumber = cl.First().ReinsuranceNumber,
                                                                Description = cl.First().Description,
                                                                ProcessDate = cl.First().ProcessDate,
                                                                RegistrationDate = cl.First().RegistrationDate,
                                                                IsAutomatic = cl.First().IsAutomatic,
                                                                LayerNumber = cl.First().LayerNumber,
                                                                UserName = cl.First().UserName,
                                                                Amount = cl.Sum(c => c.Amount),
                                                            }).ToList();


            return reinsurancePaymentLayer;
        }

        
        public List<ReinsurancePaymentDistribution> GetDistributionPaymentByPaymentLayerId(int paymentLayerId)
        {
            List<ReinsurancePaymentDistribution> reinsurancePaymentDistribution = new List<ReinsurancePaymentDistribution>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.ReinsuranceDistributionView.Properties.PaymentLayerId, paymentLayerId);


            UIView data = _dataFacadeManager.GetDataFacade().GetView("ReinsPaymentDistributionView",
                                                criteriaBuilder.GetPredicate(), null, 0, 50, null, false, out RowsGrid);

            foreach (DataRow reinsurance in data)
            {
                reinsurancePaymentDistribution.Add(new ReinsurancePaymentDistribution()
                {
                    PaymentLayerId = Convert.ToInt32(reinsurance["PaymentLayerId"]),
                    Line = reinsurance["Line"].ToString(),
                    CumulusKey = reinsurance["CumulusKey"].ToString(),
                    Description = reinsurance["MovementSource"].ToString(),
                    CurrencyDescription = reinsurance["Currency"].ToString(),
                    IsFacultative = Convert.ToString(reinsurance["IsFacultative"]).ToUpper() == "TRUE" ? "SI" : "NO",
                    Contract = reinsurance["Contract"].ToString(),
                    LevelNumber = Convert.ToInt32(reinsurance["LevelNumber"]),
                    Broker = reinsurance["Broker"].ToString(),
                    Reinsurer = reinsurance["Reinsurer"].ToString(),
                    Amount = reinsurance["Amount"].ToString()
                });
            }

            return reinsurancePaymentDistribution;
        }

        public ReinsurancePaymentClaim GetReinsurancePayment(int paymentRequestId, int userId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.GetPymTmpAllocation.Properties.PaymentRequestCode, paymentRequestId);

            UIView allocations = _dataFacadeManager.GetDataFacade().GetView("GetPymTmpAllocation", criteriaBuilder.GetPredicate(),
                null, 0, 50, null, false, out RowsGrid);

            ReinsurancePaymentClaim resultReinsurancePaymentClaim = new ReinsurancePaymentClaim();

            foreach (DataRow allocationEntity in allocations.Rows)
            {

                List<ReinsurancePaymentClaimAllocation> paymentAllocations = new List<ReinsurancePaymentClaimAllocation>();
                ReinsurancePaymentClaimAllocation pymAllocation = new ReinsurancePaymentClaimAllocation();
                pymAllocation.Id = Convert.ToInt32(allocationEntity["TempPaymentAllocationId"]);
                pymAllocation.EstimationType = new EstimationType();
                pymAllocation.EstimationType.Id = Convert.ToInt32(allocationEntity["MovementSourceId"]);
                pymAllocation.ReinsuranceSourceId = Convert.ToInt32(allocationEntity["TempPaymentReinsSourceId"]);

                pymAllocation.Facultative = Convert.ToBoolean(allocationEntity["IsFacultative"]);
                pymAllocation.Currency = new Currency
                {
                    Id = Convert.ToInt32(allocationEntity["CurrencyCode"])
                };
                pymAllocation.LevelCompanyId = Convert.ToInt32(allocationEntity["LevelCode"]);
                pymAllocation.Amount = new Amount();
                pymAllocation.Amount.Value = allocationEntity["Amount"] == DBNull.Value ? 0 : Convert.ToDecimal(allocationEntity["Amount"]);//SUM_AMOUNT


                resultReinsurancePaymentClaim.ReinsurancePaymentClaimLayers = new List<ReinsurancePaymentClaimLayer>();
                ReinsurancePaymentClaimLayer layer = new ReinsurancePaymentClaimLayer();
                layer.Id = Convert.ToInt32(allocationEntity["TempPaymentLayerId"]);
                layer.LayerNumber = Convert.ToInt32(allocationEntity["LayerNumber"]);

                layer.ReinsurancePaymentClaimCumulus = new List<ReinsurancePaymentClaimCumulus>();
                ReinsurancePaymentClaimCumulus Cumulus = new ReinsurancePaymentClaimCumulus();
                Cumulus.Id = Convert.ToInt32(allocationEntity["TempPaymentCumulusCode"]);
                Cumulus.Line = new Line
                {
                    LineId = Convert.ToInt32(allocationEntity["LineId"]),
                    Description = allocationEntity["Description"].ToString()
                };
                Cumulus.CumulusKey = allocationEntity["CumulusKey"].ToString();

                paymentAllocations.Add(pymAllocation);
                Cumulus.ReinsurancePaymentClaimAllocation = paymentAllocations;
                layer.ReinsurancePaymentClaimCumulus.Add(Cumulus);
                resultReinsurancePaymentClaim.ReinsurancePaymentClaimLayers.Add(layer);

            }
            resultReinsurancePaymentClaim.UserId = userId;
            return resultReinsurancePaymentClaim;
        }

        public List<PaymentRequest> GetPaymentsRequest(int branchCode, int prefixCode, long policyNumber, long claimNumber, int? paymentRequestNumber)
        {
            List<PaymentRequest> paymentRequests = new List<PaymentRequest>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.GetPaymentRequest.Properties.BranchCode, branchCode);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(REINSEN.GetPaymentRequest.Properties.PrefixCode, prefixCode);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(REINSEN.GetPaymentRequest.Properties.PolicyNumber, policyNumber);
            criteriaBuilder.And();
            criteriaBuilder.PropertyEquals(REINSEN.GetPaymentRequest.Properties.ClaimNumber, claimNumber);
            if (paymentRequestNumber > 0)
            {
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(REINSEN.GetPaymentRequest.Properties.PaymentNumber, paymentRequestNumber);
            }

            BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(REINSEN.GetPaymentRequest), criteriaBuilder.GetPredicate()));

            foreach (REINSEN.GetPaymentRequest entityGetPaymentRequest in businessCollection.OfType<REINSEN.GetPaymentRequest>())
            {
                PaymentRequest paymentRequest = new PaymentRequest()
                {
                    BranchId = Convert.ToInt32(entityGetPaymentRequest.BranchCode),
                    ClaimNumber = Convert.ToInt32(entityGetPaymentRequest.ClaimNumber),
                    ConceptSourceDescription = entityGetPaymentRequest.Source,
                    PaymentDate = Convert.ToDateTime(entityGetPaymentRequest.PaymentDate),
                    PaymentRequestId = Convert.ToInt32(entityGetPaymentRequest.PaymentRequestCode),
                    PaymentRequestNumber = Convert.ToInt32(entityGetPaymentRequest.PaymentNumber),
                    PolicyNumber = entityGetPaymentRequest.PolicyNumber.ToString(),
                    PrefixId = Convert.ToInt32(entityGetPaymentRequest.PrefixCode),
                    TotalAmount = Convert.ToDecimal(entityGetPaymentRequest.PaymentAmount),
                    ConceptSourceId = entityGetPaymentRequest.SourceCode,
                    RiskNumber = entityGetPaymentRequest.RiskNum,
                    CoverageNumber = entityGetPaymentRequest.CoverageNum,
                    CurrencyId = Convert.ToInt32(entityGetPaymentRequest.CurrencyCode),
                    ClaimId = Convert.ToInt32(entityGetPaymentRequest.ClaimCode),
                    ClaimCoverageCd = Convert.ToInt32(entityGetPaymentRequest.ClaimCoverageCode),
                    PaymentVoucherConceptCd = Convert.ToInt32(entityGetPaymentRequest.PaymentVoucherConceptCode),
                    SubClaim = Convert.ToInt32(entityGetPaymentRequest.SubClaim),
                    Description = entityGetPaymentRequest.Description
                };

                paymentRequests.Add(paymentRequest);
            }

            return paymentRequests;
        }

        public List<PaymentDistribution> GetReinsurancePaymentDistributionsByPaymentRequestId(int paymentRequestId, int voucherConceptCd, int claimCoverageCd)
        {
            List<PaymentDistribution> paymentDistributions = new List<PaymentDistribution>();

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.GetPaymentDistribution.Properties.PaymentRequestCode, paymentRequestId).And();
            criteriaBuilder.PropertyEquals(REINSEN.GetPaymentDistribution.Properties.ClaimCoverageCd, claimCoverageCd).And();
            criteriaBuilder.PropertyEquals(REINSEN.GetPaymentDistribution.Properties.VoucherConceptCd, voucherConceptCd);

            UIView getPaymentDistributionView = _dataFacadeManager.GetDataFacade().GetView("GetPaymentDistribution", criteriaBuilder.GetPredicate(), null, 0, 50, null, false, out RowsGrid);

            foreach (DataRow row in getPaymentDistributionView)
            {
                PaymentDistribution paymentDistribution = new PaymentDistribution
                {
                    PaymentRequestCode = Convert.ToInt32(row["PaymentRequestCode"]),
                    ReinsuranceNumber = Convert.ToInt32(row["ReinsuranceNumber"]),
                    LayerNumber = Convert.ToInt32(row["LayerNumber"]),
                    Contract = row["Contract"].ToString(),
                    LevelNumber = Convert.ToInt32(row["LevelNumber"]),
                    TradeName = row["TradeName"].ToString(),
                    Amount = Convert.ToDecimal(row["Amount"]),
                    MovementSourceId = Convert.ToInt32(row["MovementSourceId"])
                };

                paymentDistributions.Add(paymentDistribution);
            }

            return paymentDistributions;
        }

        public List<PaymentAllocation> GetPaymentTempAllocations(int processId,int movementSourceId, int voucherConceptCd, int claimCoverageCd)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(REINSEN.GetPymTmpAllocation.Properties.TempReinsuranceProcessCode, processId).And();
            criteriaBuilder.PropertyEquals(REINSEN.GetPymTmpAllocation.Properties.MovementSourceId, movementSourceId).And();
            criteriaBuilder.PropertyEquals(REINSEN.GetPymTmpAllocation.Properties.VoucherConceptCode, voucherConceptCd).And();
            criteriaBuilder.PropertyEquals(REINSEN.GetPymTmpAllocation.Properties.ClaimCoverageCode, claimCoverageCd);
            
            UIView getPymTmpAllocationView = _dataFacadeManager.GetDataFacade().GetView("GetPymTmpAllocation", criteriaBuilder.GetPredicate(), null, 0, 50, null, false, out RowsGrid);

            List<PaymentAllocation> paymentAllocations = new List<PaymentAllocation>();

            foreach (DataRow row in getPymTmpAllocationView.Rows)
            {

                PaymentAllocation paymentAllocation = new PaymentAllocation()
                {
                    TmpPaymentReinsSourceId = Convert.ToInt32(row["TempPaymentReinsSourceId"]),
                    LineId = Convert.ToInt32(row["LineId"]),
                    Description = row["Description"].ToString(),
                    CumulusKey = row["CumulusKey"].ToString(),
                    LayerNumber = Convert.ToInt32(row["LayerNumber"]),
                    SmallDescription = row["SmallDescription"].ToString(),
                    LevelNumber = Convert.ToInt32(row["LevelNumber"]),
                    Amount = Convert.ToDecimal(row["Amount"]),
                    MovementSourceId = Convert.ToInt32(row["MovementSourceId"]),
                    ClaimCoverageCd = Convert.ToInt32(row["ClaimCoverageCode"]),
                    VoucherConceptCd = Convert.ToInt32(row["VoucherConceptCode"])
                };

                paymentAllocations.Add(paymentAllocation);
            }

            return paymentAllocations;
        }



        #endregion Get
        
        public Models.Reinsurance.Reinsurance LoadReinsurancePayment(int userId, DateTime dateFrom, DateTime dateTo, List<Prefix> prefixes)
        {
            NameValue[] parameters = new NameValue[7];

            parameters[0] = new NameValue("PAYMENT_REQUEST_CD", 0);  // DBNull.Value
            parameters[1] = new NameValue("DATE_FROM", dateFrom.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            parameters[2] = new NameValue("DATE_TO", dateTo.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            parameters[3] = new NameValue("PROCESS_TYPE", (int)ProcessTypes.Massive);//proceso 3
            parameters[4] = new NameValue("USER_ID", userId);

            #region XmlPrefix


            XDocument xDocumentPrefix = new XDocument(new XElement("PrefixMassive", new XElement("fill", new XElement("option", 0))));

            if (prefixes.Count > 0)
            {
                foreach (Prefix _prefixes in prefixes)
                {
                    xDocumentPrefix.Element("PrefixMassive").Element("fill").AddAfterSelf(new XElement("Prefixes", new XElement("prefixCd", _prefixes.Id.ToString())));
                }
            }

            XmlDocument xmlDocumentPrefix = new XmlDocument();
            xmlDocumentPrefix.LoadXml(xDocumentPrefix.ToString());
            #endregion

            parameters[5] = new NameValue("PREFIX_XML", xmlDocumentPrefix.DocumentElement.OuterXml); // Lista de Ramo
            parameters[6] = new NameValue("SERVICE", 0);

            DataTable processes;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                processes = dynamicDataAccess.ExecuteSPDataTable("REINS.PYM_AUTOPROC1_WORKTABLES_LOAD", parameters);
                // ISS.VALIDATE_LICENSE_PLATE
            }

            Models.Reinsurance.Reinsurance reinsurance = new Models.Reinsurance.Reinsurance();

            foreach (DataRow process in processes.Rows)
            {
                reinsurance.ReinsuranceId = int.Parse(process[0].ToString()); // Id correspondiente al reaseguro.                                        
            }

            return reinsurance;
        }


        public int SavePaymentReinsurance(int processId, int paymentRequestId, int userId)
        {
            int reinsuranceId = 0;

            NameValue[] parameters = new NameValue[4];

            parameters[0] = new NameValue("PROCESS_ID", processId);
            parameters[1] = new NameValue("IS_AUTOMATIC", 1);
            parameters[2] = new NameValue("USER_ID", userId);
            parameters[3] = new NameValue("PAYMENT_REQUEST_CD", paymentRequestId);

            DataTable result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("REINS.PYM_AUTOPROC3_RECORDING_FINAL_TABLES", parameters);
            }

            foreach (DataRow row in result.Rows)
            {
               

                if (int.Parse(row[1].ToString()) == 0)
                {
                    reinsuranceId = 0;
                }
                else
                {
                    reinsuranceId = Convert.ToInt32(row[0].ToString());
                }
                
            }

            return reinsuranceId;
        }


        public DataTable CalculationReinsurancePayment(int requestPaymentCode, int userId, int service)
        {
            ReinsurancePaymentClaim resultReinsurancePaymentClaim = new ReinsurancePaymentClaim();
            NameValue[] parameters = new NameValue[7];
            parameters[0] = new NameValue("PAYMENT_REQUEST_CD", requestPaymentCode);
            parameters[1] = new NameValue("DATE_FROM", "");
            parameters[2] = new NameValue("DATE_TO", "");
            parameters[3] = new NameValue("PROCESS_TYPE", (int)ProcessTypes.Manual);
            parameters[4] = new NameValue("USER_ID", userId);
            parameters[5] = new NameValue("PREFIX_XML", "");
            parameters[6] = new NameValue("SERVICE", service);

            DataTable result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("REINS.PYM_AUTOPROC1_WORKTABLES_LOAD", parameters);
            }

            return result;

        }
        
        public Models.Reinsurance.Reinsurance ReinsurancePayment(int paymentRequestId, int userId)
        {
            dataFacade = _dataFacadeManager.GetDataFacade();     
            this.connection = dataFacade.GetCurrentConnection(); 

            using (Context.Current)
            {
                try
                {
                    this.transaction = connection.BeginTransaction();
                    sp.StoredProcedure storedProcedure = new sp.StoredProcedure(connection, this.transaction);

                    NameValue[] parameters = new NameValue[2];
                    parameters[0] = new NameValue("PAYMENT_REQUEST_CD", paymentRequestId);
                    parameters[1] = new NameValue("USER_ID", userId);

                    DataTable result;
                    using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                    {
                        result = dynamicDataAccess.ExecuteSPDataTable("REINS.PAYMENTS_AUTOMATIC_PROCESS", parameters);
                    }

                    Models.Reinsurance.Reinsurance reinsurance = new Models.Reinsurance.Reinsurance();

                    foreach (DataRow row in result.Rows)
                    {
                        if (Convert.ToBoolean(row[1]))
                        {
                            reinsurance.ReinsuranceId = int.Parse(row[0].ToString());        
                        }
                        else
                        {
                            reinsurance.ReinsuranceId = 0;
                        }
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

        public Models.Reinsurance.Reinsurance ReinsuranceCancellationPayment(int paymentRequestId, int userId)
        {
            dataFacade = _dataFacadeManager.GetDataFacade();
            this.connection = dataFacade.GetCurrentConnection();

            using (Context.Current)
            {
                try
                {
                    this.transaction = connection.BeginTransaction();
                    sp.StoredProcedure storedProcedure = new sp.StoredProcedure(connection, this.transaction);

                    NameValue[] parameters = new NameValue[2];
                    parameters[0] = new NameValue("PAYMENT_REQUEST_CD", paymentRequestId);
                    parameters[1] = new NameValue("USER_ID", userId);

                    DataTable result;
                    using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                    {
                        result = dynamicDataAccess.ExecuteSPDataTable("REINS.CANCEL_PAYMENT_REINSURANCE_PROCESS", parameters);
                    }

                    Models.Reinsurance.Reinsurance reinsurance = new Models.Reinsurance.Reinsurance();

                    foreach (DataRow row in result.Rows)
                    {
                        if (Convert.ToBoolean(row[1]))
                        {
                            reinsurance.ReinsuranceId = int.Parse(row[0].ToString());
                        }
                        else
                        {
                            reinsurance.ReinsuranceId = 0;
                        }
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

    }
}