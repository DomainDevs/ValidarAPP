//Sistran Core
using Sistran.Core.Application.ReconciliationServices.EEProvider.Assemblers;
using Sistran.Core.Application.ReconciliationServices.Models;
using Sistran.Core.Application.ReportingServices.Models.Formats;
using CommonModels = Sistran.Core.Application.CommonService.Models;

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
    public class ReconciliationFormatDAO
    {
        #region Instance Variables

        internal static readonly IDataFacadeManager _dataFacadeManager = AppConfigDataFacadeManagerFactory.SingletonInstance;

        #endregion

        #region Public Methods

        #region Save

        /// <summary>
        /// SaveReconciliationFormat
        /// Graba la asignación del formato del extracto a la cuenta bancaria de la compañía
        /// </summary>
        /// <param name="reconciliationFormat"></param>
        public void SaveReconciliationFormat(ReconciliationFormat reconciliationFormat)
        {
            try
            {
                // Convertir de model a entity
                Entities.BankAccountCompanyFormat bankReconciliationEntity = EntityAssembler.CreateReconciliationFormat(reconciliationFormat);

                // Realizar las operaciones con los entities utilizando DAF
                _dataFacadeManager.GetDataFacade().InsertObject(bankReconciliationEntity);

                // Return del model
                ModelAssembler.CreateReconciliationFormat(bankReconciliationEntity);
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// UpdateReconciliationFormat
        /// Actualiza la asignación del formato de conciliación a la cuenta bancraia de la compañía
        /// </summary>
        /// <param name="reconciliationFormat"></param>
        public void UpdateReconciliationFormat(ReconciliationFormat reconciliationFormat)
        {
            try
            {
                // Crea el Primary key con el código de la entidad
                PrimaryKey primaryKey = Entities.BankAccountCompanyFormat.CreatePrimaryKey(reconciliationFormat.Id);

                // Encuentra el objeto en referencia a la llave primaria
                Entities.BankAccountCompanyFormat bankAccountCompanyFormatEntity = (Entities.BankAccountCompanyFormat)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                bankAccountCompanyFormatEntity.FormatCode = reconciliationFormat.Format.Id;

                // Realiza las operaciones con las entities utilizando DAF
                _dataFacadeManager.GetDataFacade().UpdateObject(bankAccountCompanyFormatEntity);

                // Return del model
                ModelAssembler.CreateReconciliationFormat(bankAccountCompanyFormatEntity);
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
        public void DeleteReconciliationFormat(int reconciliationFormatId)
        {
            try
            {
                // Crea el Primary key con el código de la entidad
                PrimaryKey primaryKey = Entities.BankAccountCompanyFormat.CreatePrimaryKey(reconciliationFormatId);

                // Realiza las operaciones con las entities utilizando DAF
                Entities.BankAccountCompanyFormat bankAccountCompanyFormatEntity = (Entities.BankAccountCompanyFormat)(_dataFacadeManager.GetDataFacade().GetObjectByPrimaryKey(primaryKey));

                _dataFacadeManager.GetDataFacade().DeleteObject(bankAccountCompanyFormatEntity);
            }
            catch (BusinessException exception)
            {
                throw new BusinessException(exception.Message);
            }
        }

        #endregion

        #region Get

        /// <summary>
        /// GetReconciliationFormats 
        /// Obtiene las asignaciones de formatos de conciliación
        /// </summary>
        /// <returns> List<Statement></returns>
        public List<ReconciliationFormat> GetReconciliationFormats()
        {
            int rowsGrid;
            List<ReconciliationFormat> reconciliationFormats = new List<ReconciliationFormat>();

            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();

                criteriaBuilder.Property(Entities.BankAccountCompanyFormat.Properties.BankAccountCompanyCode);
                criteriaBuilder.Distinct();
                criteriaBuilder.Constant(0);

                UIView formats = _dataFacadeManager.GetDataFacade().GetView("BankAccountCompanyFormatView", criteriaBuilder.GetPredicate(), null, 0, -1, null, false, out rowsGrid);

                if (formats.Count > 0)
                {
                    foreach (DataRow dataRow in formats)
                    {
                        ReconciliationFormat reconciliationFormat = new ReconciliationFormat();

                        string fileType = "";
                        FileTypes fileTypes;

                        if (Convert.ToInt32(dataRow["FileType"]) == 1)
                        {
                            fileType = "Texto";
                            fileTypes = FileTypes.Text;
                        }
                        else if (Convert.ToInt32(dataRow["FileType"]) == 2)
                        {
                            fileType = "Excel";
                            fileTypes = FileTypes.Excel;
                        }
                        else
                        {
                            fileType = "ExcelTemplate";
                            fileTypes = FileTypes.ExcelTemplate;
                        }
                        
                        reconciliationFormat.Bank = new CommonModels.Bank()
                        {
                            Description = dataRow["BankDescription"].ToString() + " - " + dataRow["AccountTypeDescription"].ToString() + " - " + dataRow["AccountNumber"].ToString(),
                            Id = ReferenceEquals(dataRow["BankAccountCompanyCode"], DBNull.Value) ? 0 : Convert.ToInt32(dataRow["BankAccountCompanyCode"])
                        };
                        reconciliationFormat.Format = new Format()
                        {
                            Description = dataRow["FormatDescription"].ToString() + " - " + fileType,
                            FileType = fileTypes,
                            Id = ReferenceEquals(dataRow["FormatCode"], DBNull.Value) ? 0 : Convert.ToInt32(dataRow["FormatCode"])
                        };
                        reconciliationFormat.Id = Convert.ToInt32(dataRow["BankAccountCompanyFormatId"]);

                        reconciliationFormats.Add(reconciliationFormat);
                    }
                }
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }

            return reconciliationFormats;
        }

        /// <summary>
        /// GetReconciliationFormat
        /// Obtiene extractos bancarios fallidos por cuenta bancaria y fecha
        /// </summary>
        /// <param name="bankAccountCompany"></param>
        /// <param name="dateTo"></param>
        /// <returns> List<Statement></returns>
        public int GetReconciliationFormat(int bankId)
        {
            int formatId = 0;
            try
            {
                ObjectCriteriaBuilder criteriaBuilder = new ObjectCriteriaBuilder();
                criteriaBuilder.PropertyEquals(Entities.BankAccountCompanyFormat.Properties.BankAccountCompanyCode, bankId);

                // Se asigna BusinessCollection a una lista
                BusinessCollection businessCollection = new BusinessCollection(_dataFacadeManager.GetDataFacade().SelectObjects(typeof(Entities.BankAccountCompanyFormat), criteriaBuilder.GetPredicate()));

                foreach (Entities.BankAccountCompanyFormat bankAccountCompanyFormatEntity in businessCollection.OfType<Entities.BankAccountCompanyFormat>())
                {
                    formatId = bankAccountCompanyFormatEntity.FormatCode;
                }

                // Return Lista
                return formatId;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #endregion 

    }
}
