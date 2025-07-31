//Sistran Core
using Sistran.Co.Application.Data;
using Sistran.Core.Application.AccountingServices.DTOs.BankAccounts;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.GeneralLedgerServices.DTOs;
using Sistran.Core.Application.ReconciliationServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;

//Sitran FWK
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.DAF.Engine.Factories;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Views;

using System;
using System.Collections.Generic;
using System.Data;


namespace Sistran.Core.Application.ReconciliationServices.EEProvider.DAOs
{
    public class ReconciliationDAO
    {
        #region Instance Variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        readonly List<Statement> _statements = new List<Statement>();

        #endregion

        #region Public Methods

        #region Save

        /// <summary>
        /// SaveConciliations
        /// Graba la conciliación entre banco y compañía
        /// </summary>
        /// <param name="reconciliations"></param>
        /// <returns>Conciliation</returns>
        public Reconciliation SaveReconciliation(List<Reconciliation> reconciliations)
        {
            Reconciliation newReconciliation = new Reconciliation();
            List<Statement> statements = new List<Statement>();

            int reconciliationId = 0;
            int groupId = 0;
            string origin = "";
            string operationType = "";

            foreach (Reconciliation reconciliation in reconciliations)
            {
                foreach (Statement statement in reconciliation.BankStatements)
                {
                    origin = GetOrigin(statement.StatementType);

                    operationType = GetOperationType(statement.Description);

                    var parameters = new NameValue[9];

                    parameters[0] = new NameValue("RECONCILIATION_ID", reconciliationId);
                    parameters[1] = new NameValue("BANK_ACCOUNT_COMPANY_ID", statement.BankAccountCompany.Id);
                    parameters[2] = new NameValue("DATE_TO", reconciliation.Date);
                    parameters[3] = new NameValue("RECONCILIATION_DATE", reconciliation.Date);
                    parameters[4] = new NameValue("STATEMENT_ID", statement.Id);
                    parameters[5] = new NameValue("MOVEMENT_ORIGIN", origin);
                    parameters[6] = new NameValue("OPERATION_TYPE", operationType);
                    parameters[7] = new NameValue("GROUP_ID", groupId);
                    parameters[8] = new NameValue("USER_CD", reconciliation.UserId);

                    DataTable result;
                    using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                    {
                        result = dynamicDataAccess.ExecuteSPDataTable("GL.SAVE_RECONCILIATION", parameters);
                    }

                    if (result != null && result.Rows.Count > 0)
                    {
                        foreach (DataRow arrayItem in result.Rows)
                        {
                            reconciliationId = ReferenceEquals(arrayItem[0], DBNull.Value) ? 0 : Convert.ToInt32(arrayItem[0]);
                            groupId = ReferenceEquals(arrayItem[1], DBNull.Value) ? 0 : Convert.ToInt32(arrayItem[1]);
                        }
                    }
                }
            }

            newReconciliation.Id = reconciliationId;
            newReconciliation.BankStatements = statements;
            newReconciliation.ReconciliationType = ReconciliationTypes.Manual;

            return newReconciliation;
        }

        /// <summary>
        /// Reconcile
        /// Ejecuta el proceso de conciliación bancaria 
        /// </summary>
        /// <param name="bankAccountCompany"></param>
        /// <param name="dateTo"></param>
        /// <param name="dateConciliation"></param>
        /// <param name="getBanks"></param>
        /// <param name="getCentralAccountings"></param>
        /// <param name="getDailyAccountings"></param>
        /// <param name="byType"></param>
        /// <param name="byMonth"></param>
        /// <param name="byDocumentNumber"></param>
        /// <param name="byDate"></param>
        /// <param name="byBranch"></param>
        /// <param name="userId"></param>
        /// <returns>List<Conciliation></returns>
        public List<Reconciliation> Reconcile(BankAccountCompanyDTO bankAccountCompany, DateTime dateTo, DateTime reconciliationDate,
                                             bool getBanks, bool getCentralAccountings, bool getDailyAccountings,
                                             bool byType, bool byMonth, bool byDocumentNumber, bool byDate, bool byBranch, int userId)
        {
            var parameters = new NameValue[13];

            parameters[0] = new NameValue("BANK_ACCOUNT_COMPANY_ID", bankAccountCompany.Id);
            parameters[1] = new NameValue("BANK", getBanks ? 1 : 0);
            parameters[2] = new NameValue("LEDGER_ENTRY", getCentralAccountings ? 1 : 0);
            parameters[3] = new NameValue("JOURNAL_ENTRY", getDailyAccountings ? 1 : 0);
            parameters[4] = new NameValue("DATE_TO", dateTo);
            parameters[5] = new NameValue("RECONCILIATION_DATE", reconciliationDate);
            parameters[6] = new NameValue("BY_TYPE", byType ? 0 : 1);
            parameters[7] = new NameValue("BY_MONTH", byMonth ? 0 : 1);
            parameters[8] = new NameValue("BY_DATE", byDate ? 0 : 1);
            parameters[9] = new NameValue("BY_VOUCHER", byDocumentNumber ? 0 : 1);
            parameters[10] = new NameValue("BY_BRANCH", byBranch ? 0 : 1);
            parameters[11] = new NameValue("PARAMETER_ID", bankAccountCompany.BankAccountType.Id);
            parameters[12] = new NameValue("USER_ID", userId);

            DataTable result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("GL.GENERATE_RECONCILIATION", parameters);
            }

            List<Reconciliation> reconciliations = new List<Reconciliation>();

            if (result != null && result.Rows.Count > 0)
            {
                foreach (DataRow arrayItem in result.Rows)
                {
                    List<Statement> statements = new List<Statement>();

                    Reconciliation reconciliation = new Reconciliation();
                    reconciliation.ReconciliationType = ReconciliationTypes.Automatic;
                    reconciliation.Date = reconciliationDate;
                    reconciliation.Id = DBNull.ReferenceEquals(arrayItem[0], DBNull.Value) ? 0 : Convert.ToInt32(arrayItem[0]);
                    reconciliation.UserId = userId;

                    Statement statement = new Statement();

                    statement.BankAccountCompany = new BankAccountCompanyDTO()
                    {
                        Id = bankAccountCompany.Id
                    };

                    statement.Amount = new Amount() { Value = Convert.ToDecimal(arrayItem[6]) };      // importe compañía
                    statement.LocalAmount = new Amount() { Value = Convert.ToDecimal(arrayItem[5]) }; // importe banco

                    statement.Branch = new Branch() { Id = 0 };

                    ReconciliationMovementTypeDTO reconciliationMovementType = new ReconciliationMovementTypeDTO();
                    int tempReconciliationMovementTypeId = ReferenceEquals(arrayItem[1], DBNull.Value) ? 0 : Convert.ToInt32(arrayItem[1]);
                    reconciliationMovementType.Id = arrayItem[1].Equals("**") ? 0 : tempReconciliationMovementTypeId;
                    /*
                    if (arrayItem[1].Equals("**"))
                    {
                        reconciliationMovementType.Id = 0;
                    }
                    else
                    {
                        reconciliationMovementType.Id = ReferenceEquals(arrayItem[1], DBNull.Value) ? 0 : Convert.ToInt32(arrayItem[1]);
                    }
                    */
                    reconciliationMovementType.Description = ReferenceEquals(arrayItem[2], DBNull.Value) ? "" : Convert.ToString(arrayItem[2]);

                    statement.ReconciliationMovementType = reconciliationMovementType;
                    statement.Date = arrayItem[4].Equals("**") ? Convert.ToDateTime("01/01/0001 0:00:00") : Convert.ToDateTime(arrayItem[4]);
                    /*
                    if (arrayItem[4].Equals("**"))
                    {
                        statement.Date = Convert.ToDateTime("01/01/0001 0:00:00");
                    }
                    else
                    {
                        statement.Date = Convert.ToDateTime(arrayItem[4]);
                    }
                    */

                    statement.Description = ReferenceEquals(arrayItem[7], DBNull.Value) ? "" : Convert.ToString(arrayItem[7]); // diferencia
                    statement.DocumentNumber = ReferenceEquals(arrayItem[3], DBNull.Value) ? "" : Convert.ToString(arrayItem[3]);
                    statement.Id = ReferenceEquals(arrayItem[0], DBNull.Value) ? 0 : Convert.ToInt32(arrayItem[0]);
                    statement.StatementType = StatementTypes.Bank;

                    statements.Add(statement);
                    reconciliation.BankStatements = statements;
                    reconciliations.Add(reconciliation);
                }
            }
            return reconciliations;
        }

        #endregion

        #region Get

        /// <summary>
        /// GetReconciliationsByStatementTypes
        /// Obtiene conciliaciones por tipo de extracto
        /// </summary>
        /// <param name="bankAccountCompany"></param>
        /// <param name="dateTo"></param>
        /// <param name="getBanks"></param>
        /// <param name="getCentralAccountings"></param>
        /// <param name="getDailyAccountings"></param>
        /// <param name="userId"></param>
        /// <returns>List<Conciliation></returns>
        public List<Reconciliation> GetReconciliationsByStatementTypes(BankAccountCompanyDTO bankAccountCompany, DateTime dateTo,
                                                bool getBanks, bool getCentralAccountings, bool getDailyAccountings, int userId)
        {
            List<Reconciliation> reconciliations = new List<Reconciliation>();

            try
            {
                var parameters = new NameValue[6];

                parameters[0] = new NameValue("BANK_ACCOUNT_COMPANY_ID", bankAccountCompany.Id);
                parameters[1] = new NameValue("BANK", getBanks ? 1 : 0);
                parameters[2] = new NameValue("LEDGER_ENTRY", getCentralAccountings ? 1 : 0);
                parameters[3] = new NameValue("JOURNAL_ENTRY", getDailyAccountings ? 1 : 0);
                parameters[4] = new NameValue("DATE_TO", dateTo);
                parameters[5] = new NameValue("USER_CD", userId);

                DataTable result;
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("GL.GET_RECONCILE_PENDING_MOVEMENT", parameters);
                }

                if (result != null && result.Rows.Count > 0)
                {
                    Reconciliation reconciliation = new Reconciliation();
                    reconciliation.ReconciliationType = ReconciliationTypes.Manual;
                    reconciliation.Date = DateTime.Now;
                    reconciliation.Id = 0;

                    List<Statement> statements = new List<Statement>();

                    foreach (DataRow arrayItem in result.Rows)
                    {
                        Statement statement = new Statement();

                        statement.BankAccountCompany = new BankAccountCompanyDTO()
                        {
                            Id = ReferenceEquals(arrayItem[1], DBNull.Value) ? 0 : Convert.ToInt32(arrayItem[1])
                        };
                        statement.Amount = new Amount() { Value = Convert.ToDecimal(arrayItem[7]) };
                        statement.LocalAmount = new Amount() { Value = Convert.ToDecimal(arrayItem[7]) };
                        statement.Branch = new Branch() { Id = ReferenceEquals(arrayItem[9], DBNull.Value) ? 0 : Convert.ToInt32(arrayItem[9]) };
                        statement.ReconciliationMovementType = new ReconciliationMovementTypeDTO()
                        {
                            Id = ReferenceEquals(arrayItem[3], DBNull.Value) ? 0 : Convert.ToInt32(arrayItem[3]),
                            Description = ReferenceEquals(arrayItem[4], DBNull.Value) ? "" : Convert.ToString(arrayItem[4])
                        };
                        statement.Date = Convert.ToDateTime(arrayItem[6]);
                        statement.Description = ReferenceEquals(arrayItem[8], DBNull.Value) ? "" : Convert.ToString(arrayItem[8]);
                        statement.DocumentNumber = ReferenceEquals(arrayItem[5], DBNull.Value) ? "" : Convert.ToString(arrayItem[5]);
                        statement.Id = ReferenceEquals(arrayItem[0], DBNull.Value) ? 0 : Convert.ToInt32(arrayItem[0]);
                        string origin = Convert.ToString(arrayItem[2]);
                        statement.StatementType = SetttingStatementTypeByOrigin(origin);
                        /*
                        if (origin.Equals("B"))
                        {
                            statement.StatementType = StatementTypes.Bank;
                        }
                        if (origin.Equals("C"))
                        {
                            statement.StatementType = StatementTypes.CentralAccounting;
                        }
                        if (origin.Equals("D"))
                        {
                            statement.StatementType = StatementTypes.DailyAccounting;
                        }
                        */
                        statement.ThirdPerson = new Individual()
                        {
                            FullName = ReferenceEquals(arrayItem[11], DBNull.Value) ? "" : Convert.ToString(arrayItem[11])
                        };

                        statements.Add(statement);
                    }

                    reconciliation.BankStatements = statements;
                    reconciliations.Add(reconciliation);
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
            return reconciliations;
        }

        /// <summary>
        /// GetReconciliationsByReconciliationId
        /// Obtiene una conciliación por su identificativo
        /// </summary>
        /// <param name="reconciliationId"></param>
        /// <returns>List<Conciliation></returns>
        public List<Reconciliation> GetReconciliationsByReconciliationId(int reconciliationId)
        {
            int rowsGrid;
            List<Reconciliation> reconciliations = new List<Reconciliation>();
            UIView bankMovements;
            UIView journalEntryMovements;
            UIView ledgerEntryMovements;

            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
            criteriaBuilder.PropertyEquals(Entities.TempReconciliation.Properties.ReconciliationCode, reconciliationId);

            Reconciliation reconciliation = new Reconciliation();
            reconciliation.ReconciliationType = ReconciliationTypes.Manual;
            reconciliation.Date = DateTime.Now;
            reconciliation.UserId = -1;

            int pageSize = GetTotalRecordTempReconciliation(reconciliationId);

            if (pageSize > 0) //Existe en temporales
            {
                bankMovements = _dataFacadeManager.GetDataFacade().GetView("BankTempReconciliationView",
                criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rowsGrid);

                /*Contabilidad Diaria*/
                journalEntryMovements = _dataFacadeManager.GetDataFacade().GetView("JournalEntryTempReconciliationView",
                criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rowsGrid);

                /*Contabilidad Central*/
                ledgerEntryMovements = _dataFacadeManager.GetDataFacade().GetView("LedgerEntryTempReconciliationView",
                criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rowsGrid);

                foreach (DataRow dataRow in bankMovements)
                {
                    reconciliation.Id = ReferenceEquals(dataRow["ReconciliationCode"], DBNull.Value) ? 0 : Convert.ToInt32(dataRow["ReconciliationCode"]);
                    SettingStatements(dataRow, "B");
                }

                foreach (DataRow dataRow in journalEntryMovements)
                {
                    reconciliation.Id = ReferenceEquals(dataRow["ReconciliationCode"], DBNull.Value) ? 0 : Convert.ToInt32(dataRow["ReconciliationCode"]);
                    SettingStatements(dataRow, "D");
                }
                foreach (DataRow dataRow in ledgerEntryMovements)
                {
                    reconciliation.Id = ReferenceEquals(dataRow["ReconciliationCode"], DBNull.Value) ? 0 : Convert.ToInt32(dataRow["ReconciliationCode"]);
                    SettingStatements(dataRow, "C");
                }
            }
            else
            {
                pageSize = GetTotalRecordReconciliation(reconciliationId);

                if (pageSize > 0)
                {
                    bankMovements = _dataFacadeManager.GetDataFacade().GetView("BankReconciliationView",
                                       criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rowsGrid);

                    /*Contabilidad Diaria*/
                    journalEntryMovements = _dataFacadeManager.GetDataFacade().GetView("JournalEntryReconciliationView",
                                            criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rowsGrid);


                    /*Contabilidad Central*/
                    ledgerEntryMovements = _dataFacadeManager.GetDataFacade().GetView("LedgerEntryReconciliationView",
                                                 criteriaBuilder.GetPredicate(), null, 0, -1, null, true, out rowsGrid);

                    foreach (DataRow dataRow in bankMovements)
                    {
                        reconciliation.Id = ReferenceEquals(dataRow["ReconciliationCode"], DBNull.Value) ? 0 : Convert.ToInt32(dataRow["ReconciliationCode"]);
                        SettingStatements(dataRow, "B");
                    }

                    foreach (DataRow dataRow in journalEntryMovements)
                    {
                        reconciliation.Id = ReferenceEquals(dataRow["ReconciliationCode"], DBNull.Value) ? 0 : Convert.ToInt32(dataRow["ReconciliationCode"]);
                        SettingStatements(dataRow, "D");
                    }
                    foreach (DataRow dataRow in ledgerEntryMovements)
                    {
                        reconciliation.Id = ReferenceEquals(dataRow["ReconciliationCode"], DBNull.Value) ? 0 : Convert.ToInt32(dataRow["ReconciliationCode"]);
                        SettingStatements(dataRow, "C");
                    }
                }
            }

            reconciliation.BankStatements = _statements;
            reconciliations.Add(reconciliation);

            return reconciliations;
        }

        #endregion

        #region Reverse

        /// <summary>
        /// ReverseConciliation
        /// Reversa una conciliación generada
        /// </summary>
        /// <param name="reconciliation"></param>
        /// <returns></returns>
        public int ReverseReconciliation(Reconciliation reconciliation)
        {
            var parameters = new NameValue[4];

            parameters[0] = new NameValue("RECONCILIATION_ID", reconciliation.Id);
            parameters[1] = new NameValue("REVERSE_DATE", reconciliation.Date);
            parameters[2] = new NameValue("OPERATION_TYPE", "U");
            parameters[3] = new NameValue("USER_CD", reconciliation.UserId);

            DataTable result;
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                dynamicDataAccess.ExecuteSPDataTable("GL.REVERSE_RECONCILIATION", parameters);
            }

            return 0;
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// SettingStatements
        /// Union de las consultas Banco, contabilidad diaria y central
        /// </summary>
        /// <param name="dataRow"></param>
        /// <param name="origin"></param>
        private void SettingStatements(DataRow dataRow, string origin)
        {
            int banckAccountCompanyCode;

            Statement statement = new Statement();

            if (origin == "B")
            {
                banckAccountCompanyCode = ReferenceEquals(dataRow["BankAccountCompanyCode"], DBNull.Value) ? 0 : Convert.ToInt32(dataRow["BankAccountCompanyCode"]);

                statement.Amount = new Amount()
                {
                    Value = Convert.ToDecimal(dataRow["LocalAmount"])
                };
                statement.LocalAmount = new Amount()
                {
                    Value = Convert.ToDecimal(dataRow["LocalAmount"])
                };
                statement.Branch = new Branch()
                {
                    Id = ReferenceEquals(dataRow["SourceBranchCode"], DBNull.Value) ? 0 : Convert.ToInt32(dataRow["SourceBranchCode"])
                };
                statement.ReconciliationMovementType = new ReconciliationMovementTypeDTO()
                {
                    Id = ReferenceEquals(dataRow["BankReconciliationCode"], DBNull.Value) ? 0 : Convert.ToInt32(dataRow["BankReconciliationCode"]),
                    Description = ReferenceEquals(dataRow["ShortDescription"], DBNull.Value) ? "" : Convert.ToString(dataRow["ShortDescription"])
                };
                statement.Date = Convert.ToDateTime(dataRow["MovementDate"]);
                statement.Description = ReferenceEquals(dataRow["Description"], DBNull.Value) ? "" : Convert.ToString(dataRow["Description"]);
                statement.DocumentNumber = ReferenceEquals(dataRow["VoucherNumber"], DBNull.Value) ? "" : Convert.ToString(dataRow["VoucherNumber"]);
                statement.Id = ReferenceEquals(dataRow["BankStatementCode"], DBNull.Value) ? 0 : Convert.ToInt32(dataRow["BankStatementCode"]);
                statement.ThirdPerson = new Individual()
                {
                    FullName = ReferenceEquals(dataRow["ThirdDescription"], DBNull.Value) ? "" : Convert.ToString(dataRow["ThirdDescription"])
                };
            }
            else
            {
                banckAccountCompanyCode = 0;

                statement.Amount = new Amount()
                {
                    Value = Convert.ToDecimal(dataRow.ItemArray[6]) //LocalAmount
                };
                statement.LocalAmount = new Amount()
                {
                    Value = Convert.ToDecimal(dataRow.ItemArray[6]) //LocalAmount
                };
                statement.Branch = new Branch()
                {
                    Id = ReferenceEquals(dataRow.ItemArray[8], DBNull.Value) ? 0 : Convert.ToInt32(dataRow.ItemArray[8])//SourceBranchCode
                };
                statement.ReconciliationMovementType = new ReconciliationMovementTypeDTO()
                {
                    Id = ReferenceEquals(dataRow.ItemArray[2], DBNull.Value) ? 0 : Convert.ToInt32(dataRow.ItemArray[2]),//SiseCode
                    Description = DBNull.ReferenceEquals(dataRow.ItemArray[3], DBNull.Value) ? "" : Convert.ToString(dataRow.ItemArray[3])//ShortDescription
                };
                statement.Date = Convert.ToDateTime(dataRow.ItemArray[5]);//MovementDate
                statement.Description = ReferenceEquals(dataRow.ItemArray[7], DBNull.Value) ? "" : Convert.ToString(dataRow.ItemArray[7]);//Description
                statement.DocumentNumber = ReferenceEquals(dataRow.ItemArray[4], DBNull.Value) ? "" : Convert.ToString(dataRow.ItemArray[4]);//Voucher
                statement.Id = ReferenceEquals(dataRow.ItemArray[1], DBNull.Value) ? 0 : Convert.ToInt32(dataRow.ItemArray[1]);//CheckPaymentOrderCode

                statement.ThirdPerson = new Individual()
                {
                    FullName = ""
                };
            }

            statement.BankAccountCompany = new BankAccountCompanyDTO()
            {
                Id = banckAccountCompanyCode
            };

            if (origin.Equals("B"))
            {
                statement.StatementType = StatementTypes.Bank;
            }
            if (origin.Equals("C"))
            {
                statement.StatementType = StatementTypes.CentralAccounting;
            }
            if (origin.Equals("D"))
            {
                statement.StatementType = StatementTypes.DailyAccounting;
            }

            _statements.Add(statement);
        }

        /// <summary>
        /// GetTotalRecordTempReconciliation
        /// Recupera el total de los registros del TempReconciliation
        /// </summary>
        /// <param name="reconciliationId"></param>
        /// <returns>int</returns>
        private int GetTotalRecordTempReconciliation(int reconciliationId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.PropertyEquals(Entities.TempReconciliation.Properties.ReconciliationCode, reconciliationId);
            BusinessCollection businessCollections = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.TempReconciliation), criteriaBuilder.GetPredicate()));

            return businessCollections.Count;
        }

        /// <summary>
        /// GetTotalRecordReconciliation
        /// Recupera el total de los registros del Reconciliation
        /// </summary>
        /// <param name="reconciliationId"></param>
        /// <returns>int</returns>
        private int GetTotalRecordReconciliation(int reconciliationId)
        {
            ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

            criteriaBuilder.PropertyEquals(Entities.Reconciliation.Properties.ReconciliationCode, reconciliationId);
            BusinessCollection businessCollections = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.Reconciliation), criteriaBuilder.GetPredicate()));

            return businessCollections.Count;
        }

        /// <summary>
        /// GetOrigin
        /// </summary>
        /// <param name="statementTypes"></param>
        /// <returns></returns>
        private string GetOrigin(StatementTypes statementTypes)
        {
            string origin = "";

            if (statementTypes == StatementTypes.Bank)
            {
                origin = "B";
            }
            if (statementTypes == StatementTypes.CentralAccounting)
            {
                origin = "C";
            }
            if (statementTypes == StatementTypes.DailyAccounting)
            {
                origin = "D";
            }

            return origin;
        }

        /// <summary>
        /// GetOperationType
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        private string GetOperationType(string description)
        {
            string operationType = "";

            if (description == "U")
            {
                operationType = "U";
            }
            else
            {
                operationType = "I";
            }

            return operationType;
        }

        /// <summary>
        /// SetttingStatementTypeByOrigin
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        private StatementTypes SetttingStatementTypeByOrigin(string origin)
        {
            StatementTypes statementTypes = StatementTypes.Bank;

            if (origin.Equals("B"))
            {
                statementTypes = StatementTypes.Bank;
            }
            if (origin.Equals("C"))
            {
                statementTypes = StatementTypes.CentralAccounting;
            }
            if (origin.Equals("D"))
            {
                statementTypes = StatementTypes.DailyAccounting;
            }

            return statementTypes;
        }

        #endregion


    }
}
