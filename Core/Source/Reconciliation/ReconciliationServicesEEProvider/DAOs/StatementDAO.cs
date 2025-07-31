//Sistran Core
using Sistran.Co.Application.Data;
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.ReconciliationServices.EEProvider.Assemblers;
using Sistran.Core.Application.ReconciliationServices.EEProvider.Entities;
using Sistran.Core.Application.ReconciliationServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;

//Sistran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Views;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Sistran.Core.Application.ReconciliationServices.EEProvider.DAOs
{
    public class StatementDAO
    {
        #region Instance Variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Public Methods

        #region Save

        /// <summary>
        /// SaveBankStatements
        /// Graba un extracto bancario
        /// </summary>
        /// <param name="bankStatements"></param>
        public void SaveBankStatements(List<Statement> bankStatements)
        {
            try
            {
                var parameters = new NameValue[16];

                foreach (Statement statement in bankStatements)
                {
                    parameters[0] = new NameValue("PROCESS_NUMBER", statement.Status);
                    parameters[1] = new NameValue("BANK_ACCOUNT_COMPANY_CD", statement.BankAccountCompany.Id);
                    parameters[2] = new NameValue("MOVEMENT_TYPE_CD", statement.ReconciliationMovementType.Id);
                    parameters[3] = new NameValue("BRANCH_CD", statement.Branch.Id);
                    parameters[4] = new NameValue("MOVEMENT_TYPE_DESCRIPTION", statement.ReconciliationMovementType.Description);
                    parameters[5] = new NameValue("BRANCH_DESCRIPTION", statement.Branch.Description);
                    parameters[6] = new NameValue("MOVEMENT_DATE", (String.Format("{0:dd/MM/yyyy HH:mm:ss}", statement.Date)));
                    parameters[7] = new NameValue("VOUCHER_NUMBER", Convert.ToDecimal(statement.DocumentNumber));
                    parameters[8] = new NameValue("MOVEMENT_AMOUNT", statement.LocalAmount.Value);
                    parameters[9] = new NameValue("MOVEMENT_DESCRIPTION", statement.Description);
                    parameters[10] = new NameValue("THIRD_PERSON", statement.ThirdPerson.FullName);
                    parameters[11] = new NameValue("PROCESS_DATE", (String.Format("{0:dd/MM/yyyy HH:mm:ss}", statement.ProcessDate)));
                    parameters[12] = new NameValue("OPERATION_TYPE", "I");
                    parameters[13] = new NameValue("BANK_STATEMENT_ID", statement.Id);
                    parameters[14] = new NameValue("ERROR_DESCRIPTION", statement.ThirdPerson.CustomerTypeDescription);
                    parameters[15] = new NameValue("USER_CD", statement.UserId);

                    using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                    {
                        dynamicDataAccess.ExecuteSPDataTable("GL.SAVE_BANK_STATEMENT", parameters);
                    }
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// SaveBankStatement
        /// Graba un extracto bancario
        /// </summary>
        /// <param name="bankStatements"></param>
        public void SaveBankStatement(List<Statement> bankStatements)
        {
            try
            {
                foreach (Statement statement in bankStatements)
                {
                    int processNumber = 0;
                    // Se obtiene la equivalencia entre códigos bancarios 
                    if (statement.ReconciliationMovementType.Id == 0)
                    {
                        statement.ReconciliationMovementType.Id = GetBankReconciliationIdByDescription(statement.ReconciliationMovementType.Description);
                    }

                    if (statement.Status == 0)
                    {
                        processNumber = GetProcessNumberByProcessDate(statement.ProcessDate);
                    }
                    else
                    {
                        processNumber = statement.Status;
                    }
                    statement.ProcessNumber = processNumber;

                    if (statement.ThirdPerson.CustomerTypeDescription != "")
                    {
                        SaveTempStatement(statement);
                        // Se elimina los extractos bancarios por proceso
                        DeleteBankStatementByProcessNumber(processNumber);
                    }
                    else
                    {
                        // Se elimina los extractos bancarios temporales por usuario
                        DeleteTempBankStatementByProcessNumberUserId(processNumber, statement.UserId);
                        //Se inserta duplicados
                        SaveDuplicateTempStatement(statement);

                        // Se valida la sucursal o el identificador de conciliación bancaria
                        if (statement.ReconciliationMovementType.Id == 0 || statement.Branch.Id == 0)
                        {
                            SaveFailureTempStatement(statement);
                        }
                        else
                        {
                            SaveStatement(statement);
                        }
                    }
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateBankStatement
        /// Actualiza un extracto bancario
        /// </summary>
        /// <param name="bankStatement"></param>
        public void UpdateBankStatement(Statement bankStatement)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(BankStatement.Properties.BankStatementId, bankStatement.Id);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(BankStatement.Properties.BankAccountCompanyCode, bankStatement.BankAccountCompany.Id);
                criteriaBuilder.And();
                criteriaBuilder.Property(BankStatement.Properties.ReconciliationDate).IsNull();

                var businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(BankStatement), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (BankStatement bankStatementEntity in businessCollection.OfType<BankStatement>())
                    {
                        UpdateBankStatementTransactionItem(bankStatement);
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Delete

        /// <summary>
        /// DeleteBankStatement
        /// Elimina un extracto bancario 
        /// </summary>
        /// <param name="bankStatement"></param>
        public void DeleteBankStatement(Statement bankStatement)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(BankStatement.Properties.BankStatementId, bankStatement.Id);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(BankStatement.Properties.BankAccountCompanyCode, bankStatement.BankAccountCompany.Id);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(BankStatement.Properties.BankReconciliationCode, bankStatement.ReconciliationMovementType.Id);
                criteriaBuilder.And();
                criteriaBuilder.Property(BankStatement.Properties.ReconciliationDate).IsNull();

                var businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(BankStatement), criteriaBuilder.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    foreach (BankStatement bankStatementEntity in businessCollection.OfType<BankStatement>())
                    {
                        DeleteBankStatementTransactionItem(bankStatementEntity.BankStatementId);
                    }
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion

        #region Get

        /// <summary>
        /// GetBankStatementsByAccountBank 
        /// Obtiene extractos bancarios por cuenta bancaria y fecha
        /// </summary>
        /// <param name="bankAccountCompany"></param>
        /// <param name="dateTo"></param>
        /// <returns> List<Statement></returns>
        public List<Statement> GetBankStatementsByAccountBank(BankAccountCompanyDTO bankAccountCompany, DateTime dateTo)
        {
            bool isDefault = bankAccountCompany.IsDefault;
            int rowsGrid;
            List<Statement> statements = new List<Statement>();
            UIView bankStatements;
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            if (!(isDefault))
            {
                criteriaBuilder.PropertyEquals(BankStatementView.Properties.BankAccountCompanyCode, bankAccountCompany.Id);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(BankStatementView.Properties.ProcessDate, dateTo);
                bankStatements = _dataFacadeManager.GetDataFacade().GetView("BankStatementView", criteriaBuilder.GetPredicate(), null, 0, 50, null, false, out rowsGrid);
            }
            else
            {
                if (dateTo == new DateTime(1900, 1, 1))
                {
                    criteriaBuilder.PropertyEquals(AccountBankExtractFormat.Properties.AccountBankCode, bankAccountCompany.Id);
                    bankStatements = _dataFacadeManager.GetDataFacade().GetView("BankExtractFormatHeaderView", criteriaBuilder.GetPredicate(), null, 0, 50, null, false, out rowsGrid);
                }
                else
                {
                    criteriaBuilder.PropertyEquals(BankStatementView.Properties.BankAccountCompanyCode, bankAccountCompany.Id);
                    bankStatements = _dataFacadeManager.GetDataFacade().GetView("BankStatementView", criteriaBuilder.GetPredicate(), null, 0, 50, null, false, out rowsGrid);

                    var dataAux = bankStatements.DefaultView.ToTable(true, "MovementDate", "ProcessDate", "ProcessNumber").AsEnumerable().Distinct(DataRowComparer.Default).ToList();
                    bankStatements.Clear();

                    foreach (DataRow row in dataAux)
                    {
                        bankStatements.ImportRow(row);
                    }
                }
            }

            if (bankStatements.Rows.Count > 0)
            {
                foreach (DataRow row in bankStatements.Rows)
                {
                    Statement statement = new Statement();

                    if (isDefault)
                    {
                        statement.BankAccountCompany = bankAccountCompany;
                        statement.Amount = new Amount() { Value = 0 };
                        statement.LocalAmount = new Amount() { Value = 0 };
                        statement.Branch = new Branch
                        {
                            Id = bankAccountCompany.Branch.Id,
                            Description = bankAccountCompany.Branch.Description
                        };
                        statement.ReconciliationMovementType = new ReconciliationMovementTypeDTO() { Id = 0 };
                        if (dateTo == new DateTime(1900, 1, 1))
                        {
                            statement.Date = (new DateTime(1900, 1, 1));
                            statement.ProcessDate = (new DateTime(1900, 1, 1));
                            statement.Status = Convert.ToInt32(row["Separator"] == DBNull.Value ? 0 : 1);
                        }
                        else
                        {
                            statement.Date = Convert.ToDateTime(row["MovementDate"]);
                            statement.ProcessDate = Convert.ToDateTime(row["ProcessDate"]);
                            statement.Status = Convert.ToInt32(row["ProcessNumber"] == DBNull.Value ? "0" : Convert.ToString(row["ProcessNumber"]));
                        }

                        statement.Description = "";
                        statement.DocumentNumber = "";
                        statement.Id = 0;
                        statement.ProcessNumber = 1;
                        statement.StatementType = StatementTypes.Bank;
                        statement.ThirdPerson = new Individual() { FullName = "" };
                    }
                    else
                    {
                        statement.BankAccountCompany = new BankAccountCompanyDTO();
                        statement.Amount = new Amount() { Value = Convert.ToDecimal(row["Amount"]) };
                        statement.LocalAmount = new Amount() { Value = Convert.ToDecimal(row["Amount"]) };
                        statement.Branch = new Branch()
                        {
                            Id = ReferenceEquals(row["SourceBranchCode"], DBNull.Value) ? 0 : Convert.ToInt32(row["SourceBranchCode"]),
                            Description = ReferenceEquals(row["BranchDescription"], DBNull.Value) ? "" : Convert.ToString(row["BranchDescription"])
                        };
                        statement.ReconciliationMovementType = new ReconciliationMovementTypeDTO()
                        {
                            Id = ReferenceEquals(row["BankReconciliationCode"], DBNull.Value) ? 0 : Convert.ToInt32(row["BankReconciliationCode"]),
                            Description = ReferenceEquals(row["MovementDescription"], DBNull.Value) ? "" : Convert.ToString(row["MovementDescription"])
                        };
                        statement.Date = Convert.ToDateTime(row["MovementDate"]);
                        statement.Description = ReferenceEquals(row["Description"], DBNull.Value) ? "" : Convert.ToString(row["Description"]);
                        statement.DocumentNumber = ReferenceEquals(row["VoucherNumber"], DBNull.Value) ? "" : Convert.ToString(row["VoucherNumber"]);
                        statement.Id = ReferenceEquals(row["BankStatementId"], DBNull.Value) ? 0 : Convert.ToInt32(row["BankStatementId"]);
                        statement.StatementType = StatementTypes.Bank;
                        statement.ThirdPerson = new Individual()
                        {
                            FullName = ReferenceEquals(row["ThirdDescription"], DBNull.Value) ? "" : Convert.ToString(row["ThirdDescription"])
                        };
                    }

                    statements.Add(statement);
                }
            }

            return statements;
        }

        /// <summary>
        /// GetFailedBankStatementsByAccountBank
        /// Obtiene extractos bancarios fallidos por cuenta bancaria y fecha
        /// </summary>
        /// <param name="bankAccountCompany"></param>
        /// <param name="dateTo"></param>
        /// <returns> List<Statement></returns>
        //public List<Statement> GetFailedBankStatementsByAccountBank(AccountBank accountBank, DateTime dateTo)
        public List<Statement> GetFailedBankStatementsByAccountBank(BankAccountCompanyDTO bankAccountCompany, DateTime dateTo)
        {
            int rowsGrid;
            List<Statement> statements = new List<Statement>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(TempBankStatementView.Properties.BankAccountCompanyCode, bankAccountCompany.Id);
                criteriaBuilder.And();
                criteriaBuilder.PropertyEquals(TempBankStatementView.Properties.ProcessDate, dateTo);
                UIView bankStatements = _dataFacadeManager.GetDataFacade().GetView("TempBankStatementView", criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out rowsGrid);

                if (bankStatements.Count > 0)
                {
                    foreach (DataRow item in bankStatements)
                    {
                        bankAccountCompany.Id = Convert.ToInt32(item["BankAccountCompanyCode"]);

                        Statement statement = new Statement();
                        statement.BankAccountCompany = bankAccountCompany;
                        statement.Amount = new Amount() { Value = Convert.ToDecimal(item["Amount"]) };
                        statement.LocalAmount = new Amount() { Value = Convert.ToDecimal(item["Amount"]) };

                        statement.Branch = new Branch() {
                            Id = ReferenceEquals(item["SourceBranchCode"], DBNull.Value) ? -1 : Convert.ToInt32(item["SourceBranchCode"]),
                            Description = GetErrorDescription(item, "-")
                        };


                        statement.ReconciliationMovementType = new ReconciliationMovementTypeDTO()
                        {
                            Id = ReferenceEquals(item["BankReconciliationCode"], DBNull.Value) ?  0 : Convert.ToInt32(item["BankReconciliationCode"]),
                            Description = GetErrorDescription(item, ":")
                        };
                        statement.Date = Convert.ToDateTime(item["MovementDate"]);

                        statement.Description = ReferenceEquals(item["MovementDescription"], DBNull.Value) ? "" : item["MovementDescription"].ToString();
                        statement.DocumentNumber = item["VoucherNumber"].ToString();
                        statement.Id = Convert.ToInt32(item["BankStatementId"]);
                        statement.ProcessDate = Convert.ToDateTime(item["ProcessDate"]);
                        statement.ProcessNumber = 1;
                        statement.StatementType = StatementTypes.Bank;
                        statement.Status = 0;
                        statement.ThirdPerson = new Individual()
                        {
                            CustomerTypeDescription = item["ErrorDescription"].ToString(),
                            FullName = item["ThirdDescription"].ToString()
                        };

                        statements.Add(statement);
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return statements;
        }
        #endregion

        #endregion 

        #region Private Methods

        /// <summary>
        /// SaveStatement
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        private void SaveStatement(Statement statement)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(BankStatement.Properties.BankReconciliationCode, statement.ReconciliationMovementType.Id).And();
                criteriaBuilder.PropertyEquals(BankStatement.Properties.VoucherNumber, statement.DocumentNumber).And();
                criteriaBuilder.PropertyEquals(BankStatement.Properties.MovementDate, statement.Date).And();
                criteriaBuilder.PropertyEquals(BankStatement.Properties.Amount, statement.LocalAmount.Value).And();
                criteriaBuilder.PropertyEquals(BankStatement.Properties.Description, statement.Description).And();
                criteriaBuilder.PropertyEquals(BankStatement.Properties.SourceBranchCode, statement.Branch.Id).And();
                criteriaBuilder.PropertyEquals(BankStatement.Properties.ThirdDescription, statement.ThirdPerson.FullName).And();
                criteriaBuilder.PropertyEquals(BankStatement.Properties.UserCode, statement.UserId);

                BusinessCollection businessCollections = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(BankStatement), criteriaBuilder.GetPredicate()));

                if (businessCollections.Count == 0)
                {
                    // Convertir de model a entity
                    BankStatement bankStatementEntity = EntityAssembler.CreateBankStatement(statement);

                    _dataFacadeManager.GetDataFacade().InsertObject(bankStatementEntity);

                    // Return del model
                    ModelAssembler.CreateBankStatement(bankStatementEntity);
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SaveTempStatement
        /// </summary>
        /// <param name="statement"></param>
        /// <returns></returns>
        private void SaveTempStatement(Statement statement)
        {
            try
            {
                // Convertir de model a entity
                TempBankStatement tempBankStatementEntity = EntityAssembler.CreateTempBankStatement(statement);

                _dataFacadeManager.GetDataFacade().InsertObject(tempBankStatementEntity);

                // Return del model
                ModelAssembler.CreateTempBankStatement(tempBankStatementEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SaveDuplicateTempStatement
        /// </summary>
        /// <param name="statment"></param>
        private void SaveDuplicateTempStatement(Statement statement)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(BankStatement.Properties.BankReconciliationCode, statement.ReconciliationMovementType.Id).And();
                criteriaBuilder.PropertyEquals(BankStatement.Properties.VoucherNumber, statement.DocumentNumber).And();
                criteriaBuilder.PropertyEquals(BankStatement.Properties.MovementDate, statement.Date).And();
                criteriaBuilder.PropertyEquals(BankStatement.Properties.Amount, statement.LocalAmount.Value).And();
                criteriaBuilder.PropertyEquals(BankStatement.Properties.Description, statement.Description).And();
                criteriaBuilder.PropertyEquals(BankStatement.Properties.SourceBranchCode, statement.Branch.Id).And();
                criteriaBuilder.PropertyEquals(BankStatement.Properties.ThirdDescription, statement.ThirdPerson.FullName).And();
                criteriaBuilder.PropertyEquals(BankStatement.Properties.UserCode, statement.UserId);

                BusinessCollection businessCollections = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(BankStatement), criteriaBuilder.GetPredicate()));

                if (businessCollections.Count > 0)
                {
                    statement.ThirdPerson.CustomerTypeDescription = "Duplicado";

                    // Convertir de model a entity
                    TempBankStatement tempBankStatementEntity = EntityAssembler.CreateTempBankStatement(statement);

                    _dataFacadeManager.GetDataFacade().InsertObject(tempBankStatementEntity);

                    // Return del model
                    ModelAssembler.CreateTempBankStatement(tempBankStatementEntity);
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// SaveFailureTempStatement
        /// </summary>
        /// <param name="statment"></param>
        private void SaveFailureTempStatement(Statement statement)
        {
            try
            {
                if (statement.ReconciliationMovementType.Id == 0)
                {
                    statement.ThirdPerson.CustomerTypeDescription = "El tipo de movimiento no esta parametrizado: " + statement.ReconciliationMovementType.Description;
                }
                if (statement.Branch.Id == 0)
                {
                    statement.ThirdPerson.CustomerTypeDescription = "La sucursal no esta registrada: " + statement.Branch.Description;
                }

                // Convertir de model a entity
                TempBankStatement tempBankStatementEntity = EntityAssembler.CreateTempBankStatement(statement);

                _dataFacadeManager.GetDataFacade().InsertObject(tempBankStatementEntity);

                // Return del model
                ModelAssembler.CreateTempBankStatement(tempBankStatementEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// UpdateBankStatementTransactionItem
        /// Actualiza la entidad de Extracto Bancario
        /// </summary>
        /// <param name="bankStatement"></param>
        /// <returns>bool</returns>
        private void UpdateBankStatementTransactionItem(Statement bankStatement)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = BankStatement.CreatePrimaryKey(bankStatement.Id);

                // Realizar las operaciones con los entities utilizando DAF
                BankStatement bankStatementEntity = (BankStatement)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                bankStatementEntity.VoucherNumber = bankStatement.DocumentNumber;
                bankStatementEntity.MovementDate = bankStatement.Date;
                bankStatementEntity.Amount = bankStatement.LocalAmount.Value;
                bankStatementEntity.Description = bankStatement.Description;
                bankStatementEntity.SourceBranchCode = bankStatement.Branch.Id;
                bankStatementEntity.ThirdDescription = bankStatement.ThirdPerson.FullName;
                bankStatementEntity.BankReconciliationCode = bankStatement.ReconciliationMovementType.Id;

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(bankStatementEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteBankStatementTransactionItem
        /// </summary>
        /// <param name="bankStatementId"></param>
        /// <returns>bool</returns>
        private void DeleteBankStatementTransactionItem(int bankStatementId)
        {
            try
            {
                // Crea la Primary key con el código de la entidad
                PrimaryKey primaryKey = BankStatement.CreatePrimaryKey(bankStatementId);

                // Realizar las operaciones con los entities utilizando DAF
                BankStatement bankStatementEntity = (BankStatement)
                    (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().DeleteObject(bankStatementEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// DeleteBankStatementByProcessNumber
        /// </summary>
        /// <param name="processNumber"></param>
        private void DeleteBankStatementByProcessNumber(int processNumber)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(BankStatement.Properties.ProcessNumber, processNumber);

                BusinessCollection businessCollections = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(BankStatement), criteriaBuilder.GetPredicate()));

                if (businessCollections.Count > 0)
                {
                    foreach (BankStatement bankStatementEntity in businessCollections.OfType<BankStatement>())
                    {
                        DeleteBankStatementTransactionItem(bankStatementEntity.BankStatementId);
                    }
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// DeleteTempBankStatementByProcessNumberUserId
        /// </summary>
        /// <param name="processNumber"></param>
        /// <param name="userId"></param>
        private void DeleteTempBankStatementByProcessNumberUserId(int processNumber, int userId)
        {
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.PropertyEquals(TempBankStatement.Properties.UserCode, userId).And();
                criteriaBuilder.Property(TempBankStatement.Properties.ProcessNumber);
                criteriaBuilder.Distinct();
                criteriaBuilder.Constant(processNumber);
             
                BusinessCollection businessCollections = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(TempBankStatement), criteriaBuilder.GetPredicate()));

                if (businessCollections.Count > 0)
                {
                    foreach (TempBankStatement tempBankStatementEntity in businessCollections.OfType<TempBankStatement>())
                    {
                        // Crea la Primary key con el código de la entidad
                        PrimaryKey primaryKey = TempBankStatement.CreatePrimaryKey(tempBankStatementEntity.BankStatementId);

                        // Realizar las operaciones con los entities utilizando DAF
                        TempBankStatement tempStatementEntity = (TempBankStatement)
                            (_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                        // Realizar las operaciones con los entities utilizando DAF
                        _dataFacadeManager.GetDataFacade().DeleteObject(tempStatementEntity);
                    }
                }
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        /// <summary>
        /// Obtiene la descripción de la sucursal ó codigo SISE si existe caso contrario llena con la
        /// descripción del error
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="separator"></param>
        /// <returns>string</returns>
        private string GetErrorDescription(DataRow dataRow, string separator)
        {
            var errorDescription = "";

            if (separator == "-")
            {
                if (dataRow["BranchDescription"] == null)
                {
                    errorDescription = dataRow["ErrorDescription"].ToString();
                    errorDescription = errorDescription.Substring(errorDescription.IndexOf(separator), 50);
                }
                else
                {
                    errorDescription = dataRow["BranchDescription"].ToString();
                }
            }
            else if (separator == ":")
            {
                if (dataRow["MovementDescription"] == null)
                {
                    errorDescription = dataRow["ErrorDescription"].ToString();
                    errorDescription = errorDescription.Substring(errorDescription.IndexOf(separator), 50);
                }
                else
                {
                    errorDescription = dataRow["MovementDescription"].ToString();
                }
            }

            return errorDescription;
        }

        /// <summary>
        /// GetProcessNumberByProcessDate
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private int GetProcessNumberByProcessDate(DateTime processDate)
        {
            int processNumber = 0;
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(BankStatement.Properties.ProcessDate, processDate);

            BusinessCollection businessCollections = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(BankStatement), criteriaBuilder.GetPredicate()));

            foreach (BankStatement bankStatementEntity in businessCollections.OfType<BankStatement>())
            {
                processNumber = Convert.ToInt32(bankStatementEntity.ProcessNumber);
            }

            return processNumber;
        }

        /// <summary>
        /// GetBankReconciliationIdByDescription
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        private int GetBankReconciliationIdByDescription(string description)
        {
            int bankReconciliationId = 0;
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(BankReconciliationBank.Properties.Description, description);

            BusinessCollection businessCollections = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(BankReconciliationBank), criteriaBuilder.GetPredicate()));

            foreach (BankReconciliationBank bankReconciliationBankEntity in businessCollections.OfType<BankReconciliationBank>())
            {
                bankReconciliationId = Convert.ToInt32(bankReconciliationBankEntity.BankReconciliationCode);
            }

            return bankReconciliationId;
        }


        #endregion

    }
}
