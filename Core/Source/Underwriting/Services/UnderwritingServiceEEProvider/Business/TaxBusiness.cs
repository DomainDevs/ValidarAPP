using Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using MODPA = Sistran.Core.Application.ModelServices.Models.Param;
using UTIMO = Sistran.Core.Application.Utilities.Error;
using MODEN = Sistran.Core.Application.ModelServices.Enums;
    using UTILMO = Sistran.Core.Services.UtilitiesServices.Models;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.Business
{
    public class TaxBusiness
    {
        #region TaxMethods
        public ParamTax CreateTax(ParamTax paramTax)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.CreateTax(paramTax);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public ParamTax UpdateTax(ParamTax paramTax)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.UpdateTax(paramTax);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<ParamTax> GetByDescriptionTax(string description)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.GetByDescriptionTax(description);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<ParamTax> GetByTaxIdAndDescription(int taxId, string taxDescription)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.GetByTaxIdAndDescription(taxId, taxDescription);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        /// <summary>
        /// Genera el reporte de impuestos
        /// </summary>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Ruta del archivo</returns>
        public MODPA.ExcelFileServiceModel GenerateTaxFileReport(int taxId)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();

                MODPA.ExcelFileServiceModel excelFileServiceModel = new MODPA.ExcelFileServiceModel();
                UTIMO.Result<string, UTIMO.ErrorModel> result = taxDAO.GenerateTaxFileReport(taxId);

                if (result is UTIMO.ResultError<string, UTIMO.ErrorModel>)
                {
                    UTIMO.ErrorModel errorModelResult = (result as UTIMO.ResultError<string, UTIMO.ErrorModel>).Message;
                    excelFileServiceModel.ErrorTypeService = (MODEN.ErrorTypeService)errorModelResult.ErrorType;
                    excelFileServiceModel.ErrorDescription = errorModelResult.ErrorDescription;
                }
                else if (result is UTIMO.ResultValue<string, UTIMO.ErrorModel>)
                {
                    excelFileServiceModel.FileData = (result as UTIMO.ResultValue<string, UTIMO.ErrorModel>).Value;
                }

                return excelFileServiceModel;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        #endregion

        #region TaxRate Methods

        public ParamTaxRate CreateTaxRate(ParamTaxRate paramTaxRate)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.CreateTaxRate(paramTaxRate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public ParamTaxRate UpdateTaxRate(ParamTaxRate paramTaxRate)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.UpdateTaxRate(paramTaxRate);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<ParamTaxRate> GetTaxRatesByTaxId(int taxId)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.GetTaxRatesByTaxId(taxId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        public ParamTaxRate GetBusinessTaxRateByTaxIdbyAttributes(int taxId, int? taxConditionId, int? taxCategoryId, int? countryCode, int? stateCode, int? cityCode, int? economicActivityCode, int? prefixId, int? coverageId, int? technicalBranchId)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.GetBusinessTaxRateByTaxIdbyAttributes(taxId, taxConditionId, taxCategoryId, countryCode, stateCode, cityCode, economicActivityCode, prefixId, coverageId, technicalBranchId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public ParamTaxRate GetBusinessTaxRateById(int taxRateId)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.GetBusinessTaxRateById(taxRateId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        

        #endregion

        #region TaxCategory Methods

        public ParamTaxCategory CreateTaxCategory(ParamTaxCategory paramTaxCategory)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.CreateTaxCategory(paramTaxCategory);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public ParamTaxCategory UpdateTaxCategory(ParamTaxCategory paramTaxCategory)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.UpdateTaxCategory(paramTaxCategory);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<ParamTaxCategory> GetTaxCategoriesByTaxId(int taxId)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.GetTaxCategoriesByTaxId(taxId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public bool DeleteTaxCategoriesByTaxId(int categoryId, int taxId)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.DeleteTaxCategoriesByTaxId(categoryId, taxId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        #endregion

        #region TaxCondition Methods

        public ParamTaxCondition CreateTaxCondition(ParamTaxCondition paramTaxCondition)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.CreateTaxCondition(paramTaxCondition);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public ParamTaxCondition UpdateTaxCondition(ParamTaxCondition paramTaxCondition)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.UpdateTaxCondition(paramTaxCondition);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public List<ParamTaxCondition> GetTaxConditionsByTaxId(int taxId)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.GetTaxConditionsByTaxId(taxId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }

        public bool DeleteTaxConditionsByTaxId(int conditionId, int taxId)
        {
            try
            {
                TaxDAO taxDAO = new TaxDAO();
                return taxDAO.DeleteTaxConditionsByTaxId(conditionId, taxId);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message);
            }
        }
        #endregion
    }
}
