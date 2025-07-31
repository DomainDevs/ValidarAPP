using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Framework.Transactions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using TAXEN = Sistran.Core.Application.Tax.Entities;
using TAMOD = Sistran.Core.Application.TaxServices.Models;
using QUOENT = Sistran.Core.Application.Quotation.Entities;
using MODPA = Sistran.Core.Application.ModelServices.Models.Param;
using UTMO = Sistran.Core.Application.Utilities.Error;
using UTILMO = Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.Application.CommonService.Models;
using UTILEN = Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Framework.DAF.Engine;
using ENUMUT = Sistran.Core.Application.Utilities.Enums;
using Sistran.Core.Application.UnderwritingServices.EEProvider.Resources;
using Sistran.Co.Application.Data;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class TaxDAO
    {
        #region TaxMethods
        public ParamTax CreateTax(ParamTax paramTax)
        {

            using (Transaction transaction = new Transaction())
            {
                try
                {
                    paramTax.Id = GetMaxTaxId() + 1;
                    TAXEN.Tax entityTax = EntityAssembler.CreateParamTax(paramTax);
                    DataFacadeManager.Insert(entityTax);

                    if (paramTax.TaxAttributes.Count > 0)
                    {
                        foreach (TaxAttribute item in paramTax.TaxAttributes)
                        {
                            InsertTaxAttributes(item.Id, entityTax.TaxCode);
                        }
                    }

                    if (paramTax.TaxRoles.Count > 0)
                    {
                        foreach (TaxRole item in paramTax.TaxRoles)
                        {
                            InsertTaxRoles(item.Id, entityTax.TaxCode);
                        }
                    }
                    transaction.Complete();

                    ParamTax paramTaxReturned = new ParamTax();
                    paramTaxReturned = ModelAssembler.CreateParamTax(entityTax);
                    paramTaxReturned.TaxAttributes = paramTax.TaxAttributes;
                    paramTaxReturned.TaxRoles = paramTax.TaxRoles;

                    return paramTaxReturned;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    throw new BusinessException(ex.Message, ex);
                }
            }
           
        }

        public ParamTax UpdateTax(ParamTax paramTax)
        {
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    TAXEN.Tax entityTax = EntityAssembler.CreateParamTax(paramTax);
                    DataFacadeManager.Update(entityTax);

                    DeleteTaxAttributes(paramTax.Id);
                    DeleteTaxRoles(paramTax.Id);

                    if (paramTax.TaxAttributes.Count > 0)
                    {
                        foreach (TaxAttribute item in paramTax.TaxAttributes)
                        {
                            InsertTaxAttributes(item.Id, entityTax.TaxCode);
                        }
                    }

                    if (paramTax.TaxRoles.Count > 0)
                    {
                        foreach (TaxRole item in paramTax.TaxRoles)
                        {
                            InsertTaxRoles(item.Id, entityTax.TaxCode);
                        }
                    }

                    transaction.Complete();
                    return ModelAssembler.CreateParamTax(entityTax);
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        internal List<ParamTax> GetByDescriptionTax(string description)
        {
            ObjectCriteriaBuilder filterTax = new ObjectCriteriaBuilder();
            if (!string.IsNullOrEmpty(description))
            {
                filterTax.Property(TAXEN.Tax.Properties.Description, typeof(TAXEN.Tax).Name);
                filterTax.Like();
                filterTax.Constant("%" + description + "%"); 
            }
            BusinessCollection businessCollectionTax = DataFacadeManager.GetObjects(typeof(TAXEN.Tax), filterTax.GetPredicate());

            List<ParamTax> TaxList = ModelAssembler.CreateTaxesSearched(businessCollectionTax);

            //Attributes
            LoadAttributes(TaxList, description);

            //Roles
            LoadRoles(TaxList, description);

            //Rates
            LoadTaxRates(TaxList, description);

            //TaxCategories
            LoadTaxCategories(TaxList, description);

            //TaxCondtions
            LoadTaxConditions(TaxList, description);


            return TaxList;
        }

        internal List<ParamTax> GetByTaxIdAndDescription(int taxId, string description)
        {
            ObjectCriteriaBuilder filterTax = new ObjectCriteriaBuilder();
            if (taxId != 0)
            {
                filterTax.Property(TAXEN.Tax.Properties.TaxCode, typeof(TAXEN.Tax).Name);
                filterTax.Equal();
                filterTax.Constant(taxId);
            }
            BusinessCollection businessCollectionTax = DataFacadeManager.GetObjects(typeof(TAXEN.Tax), filterTax.GetPredicate());

            List<ParamTax> TaxList = ModelAssembler.CreateTaxesSearched(businessCollectionTax);

            //Attributes
            LoadAttributes(TaxList, description);

            //Roles
            LoadRoles(TaxList, description);

            //Rates
            LoadTaxRates(TaxList, description);

            //TaxCategories
            LoadTaxCategories(TaxList, description);

            //TaxCondtions
            LoadTaxConditions(TaxList, description);


            return TaxList;
        }


        private int GetMaxTaxId()
        {
            try
            {
                int taxId = 0;
                SelectQuery selectQuery = new SelectQuery();

                Function function = new Function(FunctionType.Max);

                function.AddParameter(new Column(TAXEN.Tax.Properties.TaxCode, typeof(TAXEN.Tax).Name));

                selectQuery.Table = new ClassNameTable(typeof(TAXEN.Tax), typeof(TAXEN.Tax).Name);
                selectQuery.AddSelectValue(new SelectValue(function, typeof(TAXEN.Tax).Name));


                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        taxId = Convert.ToInt32(reader[0]);
                    }
                }

                return taxId;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }


        public UTMO.Result<string, UTMO.ErrorModel> GenerateTaxFileReport(int taxId)
        {
            List<string> errorModelListDescription = new List<string>();
            try
            {
                //TaxTemplate
                List<TaxExcelReport> listTaxExcelReport = new List<TaxExcelReport>();

                DataTable resultTax;
                NameValue[] parametersTax = new NameValue[1];

                parametersTax[0] = new NameValue("@TAX_CODE", taxId);

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    resultTax = dynamicDataAccess.ExecuteSPDataTable("REPORT.TAX_GET_TAX_REPORT", parametersTax);
                }

                if (resultTax != null)
                {
                    foreach (DataRow row in resultTax.Rows)
                    {
                        TaxExcelReport taxExcelReport = new TaxExcelReport();

                        taxExcelReport.TaxCode = row[0].ToString() == "" ? 0 : Convert.ToInt32(row[0].ToString());
                        taxExcelReport.Description = row[1].ToString();
                        taxExcelReport.SmallDescription = row[2].ToString();
                        taxExcelReport.CurrentFrom = row[3].ToString();
                        taxExcelReport.RateType = row[4].ToString();
                        taxExcelReport.AditionalRateType = row[5].ToString();
                        taxExcelReport.BaseContitionTax = row[6].ToString();
                        taxExcelReport.IsEarned = (bool)row[7] == true ? "SI" : "NO";
                        taxExcelReport.IsSurplus = (bool)row[8] == true ? "SI" : "NO";
                        taxExcelReport.IsAditionalSurplus = (bool)row[9] == true ? "SI" : "NO";
                        taxExcelReport.Enabled = (bool)row[10] == true ? "SI" : "NO";
                        taxExcelReport.IsRetention = (bool)row[11] == true ? "SI" : "NO";
                        taxExcelReport.Retention = row[12].ToString();
                        taxExcelReport.TaxAttributes = row[13].ToString();
                        taxExcelReport.TaxRoles = row[14].ToString();

                        listTaxExcelReport.Add(taxExcelReport);
                    }
                }

                //TaxRate Template
                List<TaxRateExcelReport> listTaxRateExcelReport = new List<TaxRateExcelReport>();
                DataTable resultTaxRate;
                NameValue[] parametersTaxRate = new NameValue[1];
                parametersTaxRate[0] = new NameValue("@TAX_CODE", taxId);

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    resultTaxRate = dynamicDataAccess.ExecuteSPDataTable("REPORT.TAX_GET_TAXRATE_REPORT", parametersTaxRate);
                }

                if (resultTaxRate != null)
                {                    
                    if (resultTaxRate.Rows.Count > 0)
                    {
                        foreach (DataRow row in resultTaxRate.Rows)
                        {
                            TaxRateExcelReport taxRateExcelReport = new TaxRateExcelReport();

                            taxRateExcelReport.TaxCode = row[0].ToString() == "" ? 0 : Convert.ToInt32(row[0].ToString());
                            taxRateExcelReport.TaxRateCode = row[1].ToString() == "" ? 0 : Convert.ToInt32(row[1].ToString());
                            taxRateExcelReport.CurrentFrom = row[2].ToString();
                            taxRateExcelReport.TaxRateCondition = string.IsNullOrEmpty(row[3].ToString()) ? "" : row[3].ToString();
                            taxRateExcelReport.TaxRateCategory = string.IsNullOrEmpty(row[4].ToString()) ? "" : row[4].ToString();
                            taxRateExcelReport.TaxRateLineBusiness = string.IsNullOrEmpty(row[5].ToString()) ? "" : row[5].ToString();
                            taxRateExcelReport.TaxRateState = string.IsNullOrEmpty(row[6].ToString()) ? "" : row[6].ToString();
                            taxRateExcelReport.TaxRateCountry = string.IsNullOrEmpty(row[7].ToString()) ? "" : row[7].ToString();
                            taxRateExcelReport.TaxRateBranch = string.IsNullOrEmpty(row[8].ToString()) ? "" : row[8].ToString();
                            taxRateExcelReport.TaxRateRate = row[9].ToString();
                            taxRateExcelReport.TaxRateAdditionalRate = row[10].ToString();
                            taxRateExcelReport.TaxRateMinBaseAmount = row[11].ToString();
                            taxRateExcelReport.TaxRateMinAdditionalBaseAmount = row[12].ToString();
                            taxRateExcelReport.TaxRateMinTaxAmount = row[13].ToString();
                            taxRateExcelReport.TaxRateMinAdditionalTaxAmount = row[14].ToString();
                            taxRateExcelReport.TaxRateBaseTaxInc = (bool)row[15] == true ? "SI" : "NO";

                            listTaxRateExcelReport.Add(taxRateExcelReport);
                        }
                    }
                    else
                    {
                        TaxRateExcelReport taxRateExcelReport = new TaxRateExcelReport();

                        taxRateExcelReport.TaxCode = 0;
                        taxRateExcelReport.TaxRateCode = 0;
                        taxRateExcelReport.CurrentFrom = "";
                        taxRateExcelReport.TaxRateCondition = "";
                        taxRateExcelReport.TaxRateCategory = "";
                        taxRateExcelReport.TaxRateLineBusiness = "" ;
                        taxRateExcelReport.TaxRateState = "";
                        taxRateExcelReport.TaxRateCountry = "" ;
                        taxRateExcelReport.TaxRateBranch = "";
                        taxRateExcelReport.TaxRateRate = "";
                        taxRateExcelReport.TaxRateAdditionalRate = "";
                        taxRateExcelReport.TaxRateMinBaseAmount = "";
                        taxRateExcelReport.TaxRateMinAdditionalBaseAmount = "";
                        taxRateExcelReport.TaxRateMinTaxAmount = "";
                        taxRateExcelReport.TaxRateMinAdditionalTaxAmount ="";
                        taxRateExcelReport.TaxRateBaseTaxInc = "";

                        listTaxRateExcelReport.Add(taxRateExcelReport);
                    }               
                }

                //TaxCategory Template
                List<TaxCategoryExcelReport> listTaxCategoryExcelReport = new List<TaxCategoryExcelReport>();
                DataTable resultTaxCategory;
                NameValue[] parametersTaxCategory = new NameValue[1];
                parametersTaxCategory[0] = new NameValue("@TAX_CODE", taxId);

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    resultTaxCategory = dynamicDataAccess.ExecuteSPDataTable("REPORT.TAX_GET_TAXCATEGORY_REPORT", parametersTaxCategory);
                }

                if (resultTaxCategory != null)
                {
                    if (resultTaxCategory.Rows.Count > 0)
                    {
                        foreach (DataRow row in resultTaxCategory.Rows)
                        {
                            TaxCategoryExcelReport taxCategoryExcelReport = new TaxCategoryExcelReport();

                            taxCategoryExcelReport.TaxCode = row[0].ToString() == "" ? 0 : Convert.ToInt32(row[0].ToString());
                            taxCategoryExcelReport.TaxCategoryCode = row[1].ToString() == "" ? 0 : Convert.ToInt32(row[1].ToString());
                            taxCategoryExcelReport.Description = row[2].ToString();

                            listTaxCategoryExcelReport.Add(taxCategoryExcelReport);
                        }
                    }
                    else
                    {
                        TaxCategoryExcelReport taxCategoryExcelReport = new TaxCategoryExcelReport();

                        taxCategoryExcelReport.TaxCode = 0;
                        taxCategoryExcelReport.TaxCategoryCode = 0;
                        taxCategoryExcelReport.Description = "";

                        listTaxCategoryExcelReport.Add(taxCategoryExcelReport);
                    }                  
                }

                //TaxCondition Template
                List<TaxConditionExcelReport> listTaxConditionExcelReport = new List<TaxConditionExcelReport>();

                DataTable resultTaxCondition;
                NameValue[] parametersTaxCondition = new NameValue[1];
                parametersTaxCondition[0] = new NameValue("@TAX_CODE", taxId);

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
                {
                    resultTaxCondition = dynamicDataAccess.ExecuteSPDataTable("REPORT.TAX_GET_TAXCONDITION_REPORT", parametersTaxCategory);
                }
                if (resultTaxCondition != null)
                {
                    if (resultTaxCondition.Rows.Count>0)
                    {
                        foreach (DataRow row in resultTaxCondition.Rows)
                        {
                            TaxConditionExcelReport taxConditionExcelReport = new TaxConditionExcelReport();

                            taxConditionExcelReport.TaxCode = row[0].ToString() == "" ? 0 : Convert.ToInt32(row[0].ToString());
                            taxConditionExcelReport.TaxConditionCode = row[1].ToString() == "" ? 0 : Convert.ToInt32(row[1].ToString());
                            taxConditionExcelReport.Description = row[2].ToString();
                            taxConditionExcelReport.HasNationalRate = (bool)row[3] == true ? "SI" : "NO";
                            taxConditionExcelReport.IsIndependent = (bool)row[4] == true ? "SI" : "NO";
                            taxConditionExcelReport.IsDefault = (bool)row[5] == true ? "SI" : "NO";

                            listTaxConditionExcelReport.Add(taxConditionExcelReport);
                        }
                    }
                    else
                    {
                        TaxConditionExcelReport taxConditionExcelReport = new TaxConditionExcelReport();

                        taxConditionExcelReport.TaxCode = 0;
                        taxConditionExcelReport.TaxConditionCode = 0;
                        taxConditionExcelReport.Description = "";
                        taxConditionExcelReport.HasNationalRate = "";
                        taxConditionExcelReport.IsIndependent = "";
                        taxConditionExcelReport.IsDefault = "";

                        listTaxConditionExcelReport.Add(taxConditionExcelReport);
                    }
                }

                FileDAO fileDAO = new FileDAO();
                FileProcessValue fileProcessValue = new FileProcessValue()
                {
                    Key1 = (int)FileProcessType.ParametrizationTaxReport
                };

                UTILMO.File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = "Reporte de impuestos";

                    //Tax
                    List<UTILMO.Row> rowsTax = new List<UTILMO.Row>();
                    foreach (TaxExcelReport taxExcelReport in listTaxExcelReport)
                    {
                        var fields = file.Templates[0].Rows[0].Fields.Select(x => new UTILMO.Field
                        {
                            ColumnSpan = x.ColumnSpan,
                            Description = x.Description,
                            FieldType = x.FieldType,
                            Id = x.Id,
                            IsEnabled = x.IsEnabled,
                            IsMandatory = x.IsMandatory,
                            Order = x.Order,
                            RowPosition = x.RowPosition,
                            SmallDescription = x.SmallDescription
                        }).ToList();


                        fields[0].Value = taxExcelReport.TaxCode.ToString();
                        fields[1].Value = taxExcelReport.Description;
                        fields[2].Value = taxExcelReport.SmallDescription;
                        fields[3].Value = taxExcelReport.CurrentFrom;
                        fields[4].Value = taxExcelReport.RateType;
                        fields[5].Value = taxExcelReport.AditionalRateType;
                        fields[6].Value = taxExcelReport.BaseContitionTax;
                        fields[7].Value = taxExcelReport.IsSurplus;
                        fields[8].Value = taxExcelReport.IsAditionalSurplus;
                        fields[9].Value = taxExcelReport.Enabled;
                        fields[10].Value = taxExcelReport.IsEarned;
                        fields[11].Value = taxExcelReport.IsRetention;
                        fields[12].Value = taxExcelReport.Retention;
                        fields[13].Value = taxExcelReport.TaxAttributes;
                        fields[14].Value = taxExcelReport.TaxRoles;

                        rowsTax.Add(new UTILMO.Row
                        {
                            Fields = fields
                        });

                    }
                    file.Templates[0].Rows = rowsTax;

                    //TaxRate
                    List<UTILMO.Row> rowsTaxRate = new List<UTILMO.Row>();
                    foreach (TaxRateExcelReport taxRateExcelReport in listTaxRateExcelReport)
                    {
                        var fields = file.Templates[1].Rows[0].Fields.Select(x => new UTILMO.Field
                        {
                            ColumnSpan = x.ColumnSpan,
                            Description = x.Description,
                            FieldType = x.FieldType,
                            Id = x.Id,
                            IsEnabled = x.IsEnabled,
                            IsMandatory = x.IsMandatory,
                            Order = x.Order,
                            RowPosition = x.RowPosition,
                            SmallDescription = x.SmallDescription
                        }).ToList();


                        fields[0].Value = taxRateExcelReport.TaxCode.ToString();
                        fields[1].Value = taxRateExcelReport.TaxRateCode.ToString();
                        fields[2].Value = taxRateExcelReport.CurrentFrom;
                        fields[3].Value = taxRateExcelReport.TaxRateCondition;
                        fields[4].Value = taxRateExcelReport.TaxRateCategory;
                        fields[5].Value = taxRateExcelReport.TaxRateLineBusiness;
                        fields[6].Value = taxRateExcelReport.TaxRateState;
                        fields[7].Value = taxRateExcelReport.TaxRateCountry;
                        fields[8].Value = taxRateExcelReport.TaxRateBranch;
                        fields[9].Value = taxRateExcelReport.TaxRateRate;
                        fields[10].Value = taxRateExcelReport.TaxRateAdditionalRate;
                        fields[11].Value = taxRateExcelReport.TaxRateMinBaseAmount;
                        fields[12].Value = taxRateExcelReport.TaxRateMinAdditionalBaseAmount;
                        fields[13].Value = taxRateExcelReport.TaxRateMinTaxAmount;
                        fields[14].Value = taxRateExcelReport.TaxRateMinAdditionalTaxAmount;
                        fields[15].Value = taxRateExcelReport.TaxRateBaseTaxInc;

                        rowsTaxRate.Add(new UTILMO.Row
                        {
                            Fields = fields
                        });

                    }
                    file.Templates[1].Rows = rowsTaxRate;


                    //TaxCategory
                    List<UTILMO.Row> rowsTaxCategory = new List<UTILMO.Row>();
                    foreach (TaxCategoryExcelReport taxCategoryExcelReport in listTaxCategoryExcelReport)
                    {
                        var fields = file.Templates[2].Rows[0].Fields.Select(x => new UTILMO.Field
                        {
                            ColumnSpan = x.ColumnSpan,
                            Description = x.Description,
                            FieldType = x.FieldType,
                            Id = x.Id,
                            IsEnabled = x.IsEnabled,
                            IsMandatory = x.IsMandatory,
                            Order = x.Order,
                            RowPosition = x.RowPosition,
                            SmallDescription = x.SmallDescription
                        }).ToList();


                        fields[0].Value = taxCategoryExcelReport.TaxCode.ToString();
                        fields[1].Value = taxCategoryExcelReport.TaxCategoryCode.ToString();
                        fields[2].Value = taxCategoryExcelReport.Description;

                        rowsTaxCategory.Add(new UTILMO.Row
                        {
                            Fields = fields
                        });

                    }
                    file.Templates[2].Rows = rowsTaxCategory;

                    //TaxCondition
                    List<UTILMO.Row> rowsTaxCondition = new List<UTILMO.Row>();
                    foreach (TaxConditionExcelReport taxConditionExcelReport in listTaxConditionExcelReport)
                    {
                        var fields = file.Templates[3].Rows[0].Fields.Select(x => new UTILMO.Field
                        {
                            ColumnSpan = x.ColumnSpan,
                            Description = x.Description,
                            FieldType = x.FieldType,
                            Id = x.Id,
                            IsEnabled = x.IsEnabled,
                            IsMandatory = x.IsMandatory,
                            Order = x.Order,
                            RowPosition = x.RowPosition,
                            SmallDescription = x.SmallDescription
                        }).ToList();


                        fields[0].Value = taxConditionExcelReport.TaxCode.ToString();
                        fields[1].Value = taxConditionExcelReport.TaxConditionCode.ToString();
                        fields[2].Value = taxConditionExcelReport.Description;
                        fields[3].Value = taxConditionExcelReport.HasNationalRate;
                        fields[4].Value = taxConditionExcelReport.IsIndependent;
                        fields[5].Value = taxConditionExcelReport.IsDefault;

                        rowsTaxCondition.Add(new UTILMO.Row
                        {
                            Fields = fields
                        });

                    }
                    file.Templates[3].Rows = rowsTaxCondition;
               
                    file.Name = string.Format(file.Name + "-" + DateTime.Now.ToString("dd_MM_yyyy")+"-"+ DateTime.Now.Hour.ToString()+ "_"+ DateTime.Now.Minute.ToString()+"_" + DateTime.Now.Second.ToString());
                    return new UTMO.ResultValue<string, UTMO.ErrorModel>(fileDAO.GenerateFile(file));
                }
                else
                {
                    return new UTMO.ResultValue<string, UTMO.ErrorModel>(string.Empty);
                }
            }
            catch (System.Exception ex)
            {
                if (ex.Message == "La secuencia no contiene elementos")
                {
                    errorModelListDescription.Add("Resources.Errors.ErrorProductionReport");
                    return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.NotFound, ex));
                }
                else
                {
                    errorModelListDescription.Add("Resources.Errors.ErrorProductionReport");
                    return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(errorModelListDescription, ENUMUT.ErrorType.TechnicalFault, ex));
                }
            }
        }

        #endregion

        #region TaxRate Methods

        public ParamTaxRate CreateTaxRate(ParamTaxRate paramTaxRate)
        {

            using (Transaction transaction = new Transaction())
            {
                try
                {
                    paramTaxRate.Id = GetMaxTaxRateId() + 1;
                    TAXEN.TaxRate entityTaxRate = EntityAssembler.CreateParamTaxRate(paramTaxRate);
                    DataFacadeManager.Insert(entityTaxRate);

                    //TaxPeriodRate
                    TAMOD.TaxPeriodRate taxPeriodRateReturned = new TAMOD.TaxPeriodRate();
                    if (paramTaxRate.TaxPeriodRate != null)
                    {
                        paramTaxRate.TaxPeriodRate.Id = GetMaxTaxPeriodRateId() + 1;
                        TAXEN.TaxPeriodRate entityTaxPeriodRate = EntityAssembler.CreateParamTaxPeriodRate(paramTaxRate.TaxPeriodRate);
                        DataFacadeManager.Insert(entityTaxPeriodRate);
                        taxPeriodRateReturned = ModelAssembler.CreateParamTaxPeriodRate(entityTaxPeriodRate);
                    }

                    transaction.Complete();

                    ParamTaxRate paramTaxRateReturned = new ParamTaxRate();
                    paramTaxRateReturned = ModelAssembler.CreateParamTaxRate(entityTaxRate);
                    paramTaxRateReturned.TaxPeriodRate = taxPeriodRateReturned;

                    return paramTaxRateReturned;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        public ParamTaxRate UpdateTaxRate(ParamTaxRate paramTaxRate)
        {
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    TAXEN.TaxRate entityTaxRate = EntityAssembler.CreateParamTaxRate(paramTaxRate);
                    DataFacadeManager.Update(entityTaxRate);


                    //Borrar en Tax.TaxPeriodRate
                    DeleteTaxPeriodRate(paramTaxRate.Id);



                    //TaxPeriodRate
                    TAMOD.TaxPeriodRate taxPeriodRateReturned = new TAMOD.TaxPeriodRate();
                    if (paramTaxRate.TaxPeriodRate != null)
                    {
                        TAXEN.TaxPeriodRate entityTaxPeriodRate = EntityAssembler.CreateParamTaxPeriodRate(paramTaxRate.TaxPeriodRate);
                        entityTaxPeriodRate.TaxRateId = entityTaxRate.TaxRateId;
                        DataFacadeManager.Insert(entityTaxPeriodRate);
                        taxPeriodRateReturned = ModelAssembler.CreateParamTaxPeriodRate(entityTaxPeriodRate);
                    }


                    transaction.Complete();

                    ParamTaxRate paramTaxRateReturned = new ParamTaxRate();
                    paramTaxRateReturned = ModelAssembler.CreateParamTaxRate(entityTaxRate);
                    paramTaxRateReturned.TaxPeriodRate = taxPeriodRateReturned;

                    return paramTaxRateReturned;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        internal List<ParamTaxRate> GetTaxRatesByTaxId(int taxId)
        {
            ObjectCriteriaBuilder filterTaxRate = new ObjectCriteriaBuilder();
            filterTaxRate.Property(TAXEN.TaxRate.Properties.TaxCode, typeof(TAXEN.TaxRate).Name);
            filterTaxRate.Equal();
            filterTaxRate.Constant(taxId);
            BusinessCollection businessCollectionTax = DataFacadeManager.GetObjects(typeof(TAXEN.TaxRate), filterTaxRate.GetPredicate());

            List<ParamTaxRate> TaxRateList = ModelAssembler.CreateTaxRatesSearched(businessCollectionTax);

            //PeriodRate
            LoadTaxPeriodRates(TaxRateList);

            return TaxRateList;
        }

        internal ParamTaxRate GetBusinessTaxRateByTaxIdbyAttributes(int taxId, int? taxConditionId, int? taxCategoryId, int? countryCode, int? stateCode, int? cityCode, int? economicActivityCode, int? prefixId, int? coverageId, int? technicalBranchId)
        {
            ObjectCriteriaBuilder filterTaxRate = new ObjectCriteriaBuilder();
            filterTaxRate.PropertyEquals(TAXEN.TaxRate.Properties.TaxCode, typeof(TAXEN.TaxRate).Name, taxId);
         

            if (taxConditionId != null)
            {
                filterTaxRate.And();
                filterTaxRate.PropertyEquals(TAXEN.TaxRate.Properties.TaxConditionCode, typeof(TAXEN.TaxRate).Name, taxConditionId);
            }

            if (taxCategoryId != null)
            {
                filterTaxRate.And();
                filterTaxRate.PropertyEquals(TAXEN.TaxRate.Properties.TaxCategoryCode, typeof(TAXEN.TaxRate).Name, taxCategoryId);
            }

            if (countryCode != null)
            {
                filterTaxRate.And();
                filterTaxRate.PropertyEquals(TAXEN.TaxRate.Properties.CountryCode, typeof(TAXEN.TaxRate).Name, countryCode);
            }

            if (stateCode != null)
            {
                filterTaxRate.And();
                filterTaxRate.PropertyEquals(TAXEN.TaxRate.Properties.StateCode, typeof(TAXEN.TaxRate).Name, stateCode);
            }
            if (cityCode != null)
            {
                filterTaxRate.And();
                filterTaxRate.PropertyEquals(TAXEN.TaxRate.Properties.CityCode, typeof(TAXEN.TaxRate).Name, cityCode);
            }
            if (economicActivityCode != null)
            {
                filterTaxRate.And();
                filterTaxRate.PropertyEquals(TAXEN.TaxRate.Properties.EconomicActivityTaxCode, typeof(TAXEN.TaxRate).Name, economicActivityCode);
            }

            if (prefixId != null)
            {
                filterTaxRate.And();
                filterTaxRate.PropertyEquals(TAXEN.TaxRate.Properties.LineBusinessCode, typeof(TAXEN.TaxRate).Name, prefixId);
            }

            if (coverageId != null)
            {
                filterTaxRate.And();
                filterTaxRate.PropertyEquals(TAXEN.TaxRate.Properties.CoverageId, typeof(TAXEN.TaxRate).Name, coverageId);
            }

            if (technicalBranchId != null)
            {
                filterTaxRate.And();
                filterTaxRate.PropertyEquals(TAXEN.TaxRate.Properties.BranchCode, typeof(TAXEN.TaxRate).Name, technicalBranchId);
            }
            BusinessCollection businessCollectionTax = DataFacadeManager.GetObjects(typeof(TAXEN.TaxRate), filterTaxRate.GetPredicate());
            return ModelAssembler.CreateTaxRate(businessCollectionTax);
        }

        internal ParamTaxRate GetBusinessTaxRateById(int taxRateId)
        {
            ObjectCriteriaBuilder filterTaxRate = new ObjectCriteriaBuilder();
            filterTaxRate.PropertyEquals(TAXEN.TaxRate.Properties.TaxRateId, typeof(TAXEN.TaxRate).Name, taxRateId);
            BusinessCollection businessCollectionTax = DataFacadeManager.GetObjects(typeof(TAXEN.TaxRate), filterTaxRate.GetPredicate());
            return ModelAssembler.CreateTaxRate(businessCollectionTax);
        }

        private void LoadTaxRates(List<ParamTax> TempTaxList, string description)
        {
            foreach (ParamTax TempTaxItem in TempTaxList)
            {
                #region Filters
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                List<ParamTaxRate> tempTaxRateList = new List<ParamTaxRate>();
                TempTaxItem.TaxRates = new List<ParamTaxRate>();
                List<Dictionary<string, dynamic>> dictionaryTaxRates = new List<Dictionary<string, dynamic>>();
                bool emptyFilter = true;
                if (!string.IsNullOrEmpty(description))
                {
                    filter.Property(TAXEN.TaxRate.Properties.TaxCode, typeof(TAXEN.TaxRate).Name);
                    filter.Equal();
                    filter.Constant(TempTaxItem.Id);
                }

                #endregion
                #region Selects
                SelectQuery selectQuery = new SelectQuery();

                //TaxRate
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxRate.Properties.TaxRateId, typeof(TAXEN.TaxRate).Name), "TaxRateId"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxRate.Properties.BranchCode, typeof(TAXEN.TaxRate).Name), "BranchCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxRate.Properties.CityCode, typeof(TAXEN.TaxRate).Name), "CityCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxRate.Properties.CountryCode, typeof(TAXEN.TaxRate).Name), "CountryCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxRate.Properties.CoverageId, typeof(TAXEN.TaxRate).Name), "CoverageId"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxRate.Properties.EconomicActivityTaxCode, typeof(TAXEN.TaxRate).Name), "EconomicActivityCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxRate.Properties.LineBusinessCode, typeof(TAXEN.TaxRate).Name), "LineBusinessCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxRate.Properties.StateCode, typeof(TAXEN.TaxRate).Name), "StateCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxRate.Properties.TaxCategoryCode, typeof(TAXEN.TaxRate).Name), "TaxCategoryCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxRate.Properties.TaxCode, typeof(TAXEN.TaxRate).Name), "TaxCode"));
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxRate.Properties.TaxConditionCode, typeof(TAXEN.TaxRate).Name), "TaxConditionCode"));

                //TaxCondition
                if (TempTaxItem.TaxAttributes.Where(x => x.Id == 1).FirstOrDefault() != null)
                {
                    selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxCondition.Properties.Description, typeof(TAXEN.TaxCondition).Name), "TaxConditionDescription"));
                }
                //TaxCategory
                if (TempTaxItem.TaxAttributes.Where(x => x.Id == 2).FirstOrDefault() != null)
                {
                    selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.TaxCategory.Properties.Description, typeof(TAXEN.TaxCategory).Name), "TaxCategoryDescription"));
                }

                //State
                if (TempTaxItem.TaxAttributes.Where(x => x.Id == 3).FirstOrDefault() != null)
                {
                    selectQuery.AddSelectValue(new SelectValue(new Column(COMMEN.State.Properties.Description, typeof(COMMEN.State).Name), "StateDescription"));
                }

                //Country
                if (TempTaxItem.TaxAttributes.Where(x => x.Id == 4).FirstOrDefault() != null)
                {
                    selectQuery.AddSelectValue(new SelectValue(new Column(COMMEN.Country.Properties.Description, typeof(COMMEN.Country).Name), "CountryDescription"));
                }

                //EconomicActivity
                if (TempTaxItem.TaxAttributes.Where(x => x.Id == 5).FirstOrDefault() != null)
                {
                    selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.EconomicActivityTax.Properties.Description, typeof(TAXEN.EconomicActivityTax).Name), "EconomicActivityDescription"));
                }
                
                //Branch = 6
                if (TempTaxItem.TaxAttributes.Where(x=>x.Id == 6).FirstOrDefault() != null)
                { 
                    selectQuery.AddSelectValue(new SelectValue(new Column(COMMEN.Branch.Properties.Description, typeof(COMMEN.Branch).Name), "BranchDescription"));
                }

                //LineBusiness
                if (TempTaxItem.TaxAttributes.Where(x => x.Id == 7).FirstOrDefault() != null)
                {
                    selectQuery.AddSelectValue(new SelectValue(new Column(COMMEN.LineBusiness.Properties.Description, typeof(COMMEN.LineBusiness).Name), "LineBusinessDescription"));
                }

                //Coverage
                if (TempTaxItem.TaxAttributes.Where(x => x.Id == 8).FirstOrDefault() != null)
            {
                    selectQuery.AddSelectValue(new SelectValue(new Column(QUOENT.Coverage.Properties.PrintDescription, typeof(QUOENT.Coverage).Name), "CoverageDescription"));
                }

                //City
                if (TempTaxItem.TaxAttributes.Where(x => x.Id == 9).FirstOrDefault() != null)
            {
                    selectQuery.AddSelectValue(new SelectValue(new Column(COMMEN.City.Properties.Description, typeof(COMMEN.City).Name), "CityDescription"));
                }
                //Tax
                selectQuery.AddSelectValue(new SelectValue(new Column(TAXEN.Tax.Properties.Description, typeof(TAXEN.Tax).Name), "TaxDescription"));
                
                #endregion
                #region Joins

                Join join = new Join(new ClassNameTable(typeof(TAXEN.TaxRate), typeof(TAXEN.TaxRate).Name), new ClassNameTable(typeof(TAXEN.Tax), typeof(TAXEN.Tax).Name), JoinType.Inner);
                join.Criteria = (new ObjectCriteriaBuilder()
                    .Property(TAXEN.TaxRate.Properties.TaxCode, typeof(TAXEN.TaxRate).Name)
                    .Equal()
                    .Property(TAXEN.Tax.Properties.TaxCode, typeof(TAXEN.Tax).Name))
                    .GetPredicate();

                //Join TaxCondition

                if (TempTaxItem.TaxAttributes.Where(x => x.Id == 1).FirstOrDefault() != null)
                {
                    join = new Join(join, new ClassNameTable(typeof(TAXEN.TaxCondition), typeof(TAXEN.TaxCondition).Name), JoinType.Inner);
                    join.Criteria = (new ObjectCriteriaBuilder()
                        .Property(TAXEN.TaxRate.Properties.TaxConditionCode, typeof(TAXEN.TaxRate).Name)
                        .Equal()
                        .Property(TAXEN.TaxCondition.Properties.TaxConditionCode, typeof(TAXEN.TaxCondition).Name))
                        .GetPredicate();
                }

                //Join TaxCategory
                if (TempTaxItem.TaxAttributes.Where(x => x.Id == 2).FirstOrDefault() != null)
                {
                    join = new Join(join, new ClassNameTable(typeof(TAXEN.TaxCategory), typeof(TAXEN.TaxCategory).Name), JoinType.Inner);
                    join.Criteria = (new ObjectCriteriaBuilder()
                        .Property(TAXEN.TaxRate.Properties.TaxCategoryCode, typeof(TAXEN.TaxRate).Name)
                        .Equal()
                        .Property(TAXEN.TaxCategory.Properties.TaxCategoryCode, typeof(TAXEN.TaxCategory).Name))
                        .GetPredicate();
                }

                //Join State
                if (TempTaxItem.TaxAttributes.Where(x => x.Id == 3).FirstOrDefault() != null)
                {
                    join = new Join(join, new ClassNameTable(typeof(COMMEN.State), typeof(COMMEN.State).Name), JoinType.Inner);
                    join.Criteria = (new ObjectCriteriaBuilder()
                        .Property(TAXEN.TaxRate.Properties.StateCode, typeof(TAXEN.TaxRate).Name)
                        .Equal()
                        .Property(COMMEN.State.Properties.StateCode, typeof(COMMEN.State).Name)
                        .And()
                        .Property(TAXEN.TaxRate.Properties.CountryCode, typeof(TAXEN.TaxRate).Name)
                        .Equal()
                        .Property(COMMEN.State.Properties.CountryCode, typeof(COMMEN.State).Name))
                        .GetPredicate();
                }
                //Join Country
                if (TempTaxItem.TaxAttributes.Where(x => x.Id == 4).FirstOrDefault() != null)
                {
                    join = new Join(join, new ClassNameTable(typeof(COMMEN.Country), typeof(COMMEN.Country).Name), JoinType.Inner);
                    join.Criteria = (new ObjectCriteriaBuilder()
                        .Property(TAXEN.TaxRate.Properties.CountryCode, typeof(TAXEN.TaxRate).Name)
                        .Equal()
                        .Property(COMMEN.Country.Properties.CountryCode, typeof(COMMEN.Country).Name))
                        .GetPredicate();
                }

                //Join EconomicActivity
                if (TempTaxItem.TaxAttributes.Where(x => x.Id == 5).FirstOrDefault() != null)
                {
                    join = new Join(join, new ClassNameTable(typeof(TAXEN.EconomicActivityTax), typeof(TAXEN.EconomicActivityTax).Name), JoinType.Inner);
                    join.Criteria = (new ObjectCriteriaBuilder()
                        .Property(TAXEN.TaxRate.Properties.EconomicActivityTaxCode, typeof(TAXEN.TaxRate).Name)
                        .Equal()
                        .Property(TAXEN.EconomicActivityTax.Properties.EconomicActivityTaxId, typeof(TAXEN.EconomicActivityTax).Name))
                        .GetPredicate();
                }

                //Join Branch
                if (TempTaxItem.TaxAttributes.Where(x => x.Id == 6).FirstOrDefault() != null)
                {
                    join = new Join(join, new ClassNameTable(typeof(COMMEN.Branch), typeof(COMMEN.Branch).Name), JoinType.Inner);
                    join.Criteria = (new ObjectCriteriaBuilder()
                        .Property(TAXEN.TaxRate.Properties.BranchCode, typeof(TAXEN.TaxRate).Name)
                        .Equal()
                        .Property(COMMEN.Branch.Properties.BranchCode, typeof(COMMEN.Branch).Name))
                        .GetPredicate();
                }

                //Join LineBusiness
                if (TempTaxItem.TaxAttributes.Where(x => x.Id == 7).FirstOrDefault() != null)
                {
                    join = new Join(join, new ClassNameTable(typeof(COMMEN.LineBusiness), typeof(COMMEN.LineBusiness).Name), JoinType.Inner);
                    join.Criteria = (new ObjectCriteriaBuilder()
                        .Property(TAXEN.TaxRate.Properties.LineBusinessCode, typeof(TAXEN.TaxRate).Name)
                        .Equal()
                        .Property(COMMEN.LineBusiness.Properties.LineBusinessCode, typeof(COMMEN.LineBusiness).Name))
                        .GetPredicate();
                }

                //Join Coverage
                if (TempTaxItem.TaxAttributes.Where(x => x.Id == 8).FirstOrDefault() != null)
                {
                    join = new Join(join, new ClassNameTable(typeof(QUOENT.Coverage), typeof(QUOENT.Coverage).Name), JoinType.Inner);
                    join.Criteria = (new ObjectCriteriaBuilder()
                        .Property(TAXEN.TaxRate.Properties.CoverageId, typeof(TAXEN.TaxRate).Name)
                        .Equal()
                        .Property(QUOENT.Coverage.Properties.CoverageId, typeof(QUOENT.Coverage).Name))
                        .GetPredicate();
                }

                //Join City
                if (TempTaxItem.TaxAttributes.Where(x => x.Id == 9).FirstOrDefault() != null)
                {
                    join = new Join(join, new ClassNameTable(typeof(COMMEN.City), typeof(COMMEN.City).Name), JoinType.Inner);
                    join.Criteria = (new ObjectCriteriaBuilder()
                        .Property(TAXEN.TaxRate.Properties.CityCode, typeof(TAXEN.TaxRate).Name)
                        .Equal()
                        .Property(COMMEN.City.Properties.CityCode, typeof(COMMEN.City).Name)
                        .And()
                        .Property(TAXEN.TaxRate.Properties.StateCode, typeof(TAXEN.TaxRate).Name)
                        .Equal()
                        .Property(COMMEN.City.Properties.StateCode, typeof(COMMEN.City).Name)
                        .And()
                        .Property(TAXEN.TaxRate.Properties.CountryCode, typeof(TAXEN.TaxRate).Name)
                        .Equal()
                        .Property(COMMEN.City.Properties.CountryCode, typeof(COMMEN.City).Name))
                        .GetPredicate();
                }
                #endregion

                selectQuery.Table = join;
                selectQuery.Where = filter.GetPredicate();



                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        Dictionary<string, dynamic> dictionarytaxRate = new Dictionary<string, dynamic>();

                        dictionarytaxRate.Add("TaxRateId", Convert.ToInt32(reader["TaxRateId"]));
                        dictionarytaxRate.Add("TaxCode", Convert.ToInt32(reader["TaxCode"]));
                        dictionarytaxRate.Add("TaxDescription", Convert.ToString(reader["TaxDescription"]));

                        if (TempTaxItem.TaxAttributes.Where(x => x.Id == 1).FirstOrDefault() != null)
                        {
                            dictionarytaxRate.Add("TaxConditionDescription", Convert.ToString(reader["TaxConditionDescription"]));
                            dictionarytaxRate.Add("TaxConditionCode", Convert.ToInt32(reader["TaxConditionCode"]));
                        }

                        if (TempTaxItem.TaxAttributes.Where(x => x.Id == 2).FirstOrDefault() != null)
                        {
                            dictionarytaxRate.Add("TaxCategoryCode", Convert.ToInt32(reader["TaxCategoryCode"]));
                            dictionarytaxRate.Add("TaxCategoryDescription", Convert.ToString(reader["TaxCategoryDescription"]));
                        }

                        if (TempTaxItem.TaxAttributes.Where(x => x.Id == 3).FirstOrDefault() != null)
                        {
                            dictionarytaxRate.Add("StateCode", Convert.ToInt32(reader["StateCode"]));
                            dictionarytaxRate.Add("StateDescription", Convert.ToString(reader["StateDescription"]));
                        }

                        if (TempTaxItem.TaxAttributes.Where(x => x.Id == 4).FirstOrDefault() != null)
                        {
                            dictionarytaxRate.Add("CountryCode", Convert.ToInt32(reader["CountryCode"]));
                            dictionarytaxRate.Add("CountryDescription", Convert.ToString(reader["CountryDescription"]));
                        }
                        if (TempTaxItem.TaxAttributes.Where(x => x.Id == 5).FirstOrDefault() != null)
                        {
                            dictionarytaxRate.Add("EconomicActivityDescription", Convert.ToString(reader["EconomicActivityDescription"]));
                            dictionarytaxRate.Add("EconomicActivityCode", Convert.ToInt32(reader["EconomicActivityCode"]));
                        }

                        if (TempTaxItem.TaxAttributes.Where(x => x.Id == 6).FirstOrDefault() != null)
                        {
                            dictionarytaxRate.Add("BranchCode", Convert.ToInt32(reader["BranchCode"]));
                            dictionarytaxRate.Add("BranchDescription", Convert.ToString(reader["BranchDescription"]));
                        }
                        if (TempTaxItem.TaxAttributes.Where(x => x.Id == 7).FirstOrDefault() != null)
                        {
                            dictionarytaxRate.Add("LineBusinessCode", Convert.ToInt32(reader["LineBusinessCode"]));
                            dictionarytaxRate.Add("LineBusinessDescription", Convert.ToString(reader["LineBusinessDescription"]));
                        }
                        if (TempTaxItem.TaxAttributes.Where(x => x.Id == 8).FirstOrDefault() != null)
                        {
                            dictionarytaxRate.Add("CoverageId", Convert.ToInt32(reader["CoverageId"]));
                            dictionarytaxRate.Add("CoverageDescription", Convert.ToString(reader["CoverageDescription"]));
                        }
                        if (TempTaxItem.TaxAttributes.Where(x => x.Id == 9).FirstOrDefault() != null)
                        {
                            dictionarytaxRate.Add("CityCode", Convert.ToInt32(reader["CityCode"]));
                            dictionarytaxRate.Add("CityDescription", Convert.ToString(reader["CityDescription"]));
                        }
                        dictionaryTaxRates.Add(dictionarytaxRate);
                    }

                    foreach (var dictionaryTaxRate in dictionaryTaxRates)
                    {
                        ParamTaxRate paramTaxRate = new ParamTaxRate();
                        paramTaxRate.TaxState = new TAMOD.TaxState();
                        paramTaxRate.TaxState = new TAMOD.TaxState();
                        paramTaxRate.TaxCondition = new TAMOD.TaxCondition();
                        paramTaxRate.TaxCategory = new TAMOD.TaxCategory();
                        paramTaxRate.EconomicActivity = new EconomicActivity();
                        paramTaxRate.Branch = new Branch();
                        paramTaxRate.LineBusiness = new LineBusiness();
                        paramTaxRate.Coverage = new Coverage();

                        paramTaxRate.IdTax = dictionaryTaxRate["TaxCode"];
                        paramTaxRate.Id = dictionaryTaxRate["TaxRateId"];

                        //Set TaxCondition
                        if (TempTaxItem.TaxAttributes.Where(x => x.Id == 1).FirstOrDefault() != null)
                        {
                            paramTaxRate.TaxCondition = new TAMOD.TaxCondition
                            {
                                Id = dictionaryTaxRate["TaxConditionCode"],
                                Description = dictionaryTaxRate["TaxConditionDescription"],
                            };
                        }
                        //Set TaxCategory
                        if (TempTaxItem.TaxAttributes.Where(x => x.Id == 2).FirstOrDefault() != null)
                        {
                            paramTaxRate.TaxCategory = new TAMOD.TaxCategory
                            {
                                Id = dictionaryTaxRate["TaxCategoryCode"],
                                Description = dictionaryTaxRate["TaxCategoryDescription"]
                            };
                        }

                        //Set State
                        if (TempTaxItem.TaxAttributes.Where(x => x.Id == 3).FirstOrDefault() != null)
                        {
                            paramTaxRate.TaxState.IdState = dictionaryTaxRate["StateCode"];
                            paramTaxRate.TaxState.DescriptionState = dictionaryTaxRate["StateDescription"];

                        }
                        //Set Country
                        if (TempTaxItem.TaxAttributes.Where(x => x.Id == 4).FirstOrDefault() != null)
                        {
                            paramTaxRate.TaxState.IdCountry = dictionaryTaxRate["CountryCode"];
                            paramTaxRate.TaxState.DescriptionCountry = dictionaryTaxRate["CountryDescription"];
                        }
                        //Set EconomicActivity
                        if (TempTaxItem.TaxAttributes.Where(x => x.Id == 5).FirstOrDefault() != null)
                        {
                            paramTaxRate.EconomicActivity = new EconomicActivity
                            {
                                Id = dictionaryTaxRate["EconomicActivityCode"],
                                Description = dictionaryTaxRate["EconomicActivityDescription"]
                            };
                        }
                        //Set Branch
                        if (TempTaxItem.TaxAttributes.Where(x => x.Id == 6).FirstOrDefault() != null)
                        {
                            paramTaxRate.Branch = new Branch
                            {
                                Id = dictionaryTaxRate["BranchCode"],
                                Description = dictionaryTaxRate["BranchDescription"]
                            };
                        }
                        //Set LineBusiness
                        if (TempTaxItem.TaxAttributes.Where(x => x.Id == 7).FirstOrDefault() != null)
                        {
                            paramTaxRate.LineBusiness = new LineBusiness
                            {
                                Id = dictionaryTaxRate["LineBusinessCode"],
                                Description = dictionaryTaxRate["LineBusinessDescription"]
                            };
                        }
                        //Set Coverage
                        if (TempTaxItem.TaxAttributes.Where(x => x.Id == 8).FirstOrDefault() != null)
                        {
                            paramTaxRate.Coverage = new Coverage()
                            {
                                Id = dictionaryTaxRate["CoverageId"],
                                Description = dictionaryTaxRate["CoverageDescription"],
                            };
                        }

                        //Set CityCode
                        if (TempTaxItem.TaxAttributes.Where(x => x.Id == 9).FirstOrDefault() != null)
                        {
                            paramTaxRate.TaxState.IdCity = dictionaryTaxRate["CityCode"];
                            paramTaxRate.TaxState.DescriptionCity = dictionaryTaxRate["CityDescription"];
                        }

                        tempTaxRateList.Add(paramTaxRate);
                    }
                    LoadTaxPeriodRates(tempTaxRateList);
                    TempTaxItem.TaxRates.AddRange(tempTaxRateList);
                }
            }




            //foreach (ParamTax TempTaxItem in TempTaxList)
            //{
            //    List<ParamTaxRate> tempTaxRateList = new List<ParamTaxRate>();
            //    TempTaxItem.TaxRates = new List<ParamTaxRate>();
            //    ObjectCriteriaBuilder filterAttribute = new ObjectCriteriaBuilder();
            //    if (!string.IsNullOrEmpty(description))
            //    {
            //        filterAttribute.Property(TAXEN.TaxRate.Properties.TaxCode, typeof(TAXEN.TaxRate).Name);
            //        filterAttribute.Equal();
            //        filterAttribute.Constant(TempTaxItem.Id);
            //    }
            //    BusinessCollection businessCollectionRate = DataFacadeManager.GetObjects(typeof(TAXEN.TaxRate), filterAttribute.GetPredicate());
            //
            //    foreach (TAXEN.TaxRate tempTaxRate in businessCollectionRate)
            //    {
            //
            //        tempTaxRateList.Add(ModelAssembler.CreateParamTaxRate(tempTaxRate));
            //
            //    }
            //
            //    LoadTaxPeriodRates(tempTaxRateList);
            //
            //    TempTaxItem.TaxRates.AddRange(tempTaxRateList);
            //}
        }

        private int GetMaxTaxRateId()
        {
            try
            {
                int taxRateId = 0;
                SelectQuery selectQuery = new SelectQuery();

                Function function = new Function(FunctionType.Max);

                function.AddParameter(new Column(TAXEN.TaxRate.Properties.TaxRateId, typeof(TAXEN.TaxRate).Name));

                selectQuery.Table = new ClassNameTable(typeof(TAXEN.TaxRate), typeof(TAXEN.TaxRate).Name);
                selectQuery.AddSelectValue(new SelectValue(function, typeof(TAXEN.TaxRate).Name));


                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        taxRateId = Convert.ToInt32(reader[0]);
                    }
                }

                return taxRateId;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        

        #endregion


        #region TaxPeriodRate Methods

        private void LoadTaxPeriodRates(List<ParamTaxRate> TempTaxRateList)
        {
            if (TempTaxRateList.Any(x => x.Id != 0))
            {
                ObjectCriteriaBuilder filterTaxPeriodRate = new ObjectCriteriaBuilder();
                filterTaxPeriodRate.Property(TAXEN.TaxPeriodRate.Properties.TaxRateId, typeof(TAXEN.TaxPeriodRate).Name);
                filterTaxPeriodRate.In();
                filterTaxPeriodRate.ListValue();
                TempTaxRateList.Where(x => x.Id != 0).ToList().ForEach(x => { filterTaxPeriodRate.Constant(x.Id); x.TaxPeriodRate = new TAMOD.TaxPeriodRate(); });
                filterTaxPeriodRate.EndList();

                var entityTaxAttributes = DataFacadeManager.GetObjects(typeof(TAXEN.TaxPeriodRate), filterTaxPeriodRate.GetPredicate()).Cast<TAXEN.TaxPeriodRate>();

                TempTaxRateList.ForEach(x =>
                {
                    var entity = entityTaxAttributes.FirstOrDefault(y => y.TaxRateId == x.Id);
                    x.TaxPeriodRate = entity == null ? null : ModelAssembler.CreateParamTaxPeriodRate(entity);
                });
            }
        }

        private int GetMaxTaxPeriodRateId()
        {
            try
            {
                int taxPeriodRateId = 0;
                SelectQuery selectQuery = new SelectQuery();

                Function function = new Function(FunctionType.Max);

                function.AddParameter(new Column(TAXEN.TaxPeriodRate.Properties.TaxRateId, typeof(TAXEN.TaxPeriodRate).Name));

                selectQuery.Table = new ClassNameTable(typeof(TAXEN.TaxPeriodRate), typeof(TAXEN.TaxPeriodRate).Name);
                selectQuery.AddSelectValue(new SelectValue(function, typeof(TAXEN.TaxPeriodRate).Name));


                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        taxPeriodRateId = Convert.ToInt32(reader[0]);
                    }
                }

                return taxPeriodRateId;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        private void DeleteTaxPeriodRate(int id)
        {
            var filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(TAXEN.TaxPeriodRate.Properties.TaxRateId, id);
            DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(TAXEN.TaxPeriodRate), filter.GetPredicate());
        }

        #endregion


        #region TaxRoles Methods

        private void LoadRoles(List<ParamTax> TempTaxList, string description)
        {
            if (!string.IsNullOrEmpty(description) && TempTaxList.Any())
            {
                ObjectCriteriaBuilder filterRole = new ObjectCriteriaBuilder();

                filterRole.Property(TAXEN.TaxRole.Properties.TaxCode, typeof(TAXEN.TaxRole).Name);
                filterRole.In();
                filterRole.ListValue();
                TempTaxList.ForEach(x => { filterRole.Constant(x.Id); x.TaxRoles = new List<TaxRole>(); });
                filterRole.EndList();

                var taxRole = DataFacadeManager.GetObjects(typeof(TAXEN.TaxRole), filterRole.GetPredicate()).Cast<TAXEN.TaxRole>().ToList();

                TempTaxList.ForEach(x => taxRole.Where(z => z.TaxCode == x.Id).ToList().ForEach(y => x.TaxRoles.Add(ModelAssembler.CreateParamTaxRole(y))));
            }
        }

        private void InsertTaxRoles(int id, int taxCode)
        {
            TAXEN.TaxRole entityTaxRole = new TAXEN.TaxRole(id, taxCode);
            DataFacadeManager.Insert(entityTaxRole);
        }

        private void DeleteTaxRoles(int id)
        {
            var filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(TAXEN.TaxRole.Properties.TaxCode, id);
            DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(TAXEN.TaxRole), filter.GetPredicate());
        }

        #endregion


        #region TaxAttributes Methods

        private void LoadAttributes(List<ParamTax> TempTaxList, string description)
        {
            if (!string.IsNullOrEmpty(description) && TempTaxList.Any())
            {
                ObjectCriteriaBuilder filterAttribute = new ObjectCriteriaBuilder();
                filterAttribute.Property(TAXEN.TaxAttribute.Properties.TaxCode, typeof(TAXEN.TaxAttribute).Name);
                filterAttribute.In();
                filterAttribute.ListValue();
                TempTaxList.ForEach(x => { filterAttribute.Constant(x.Id); x.TaxAttributes = new List<TaxAttribute>(); });
                filterAttribute.EndList();

                List<TAXEN.TaxAttribute> entityTaxAttributes = DataFacadeManager.GetObjects(typeof(TAXEN.TaxAttribute), filterAttribute.GetPredicate()).Cast<TAXEN.TaxAttribute>().ToList();

                TempTaxList.ForEach(x => entityTaxAttributes.Where(y => y.TaxCode == x.Id).ToList().ForEach(z => x.TaxAttributes.Add(ModelAssembler.CreateParamTaxAttribute(z))));
            }
        }

        private void InsertTaxAttributes(int id, int taxCode)
        {
            TAXEN.TaxAttribute entityTaxAttribute = new TAXEN.TaxAttribute(taxCode, id);
            entityTaxAttribute.TaxCode = taxCode;
            DataFacadeManager.Insert(entityTaxAttribute);
        }

        private void DeleteTaxAttributes(int id)
        {
            var filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(TAXEN.TaxAttribute.Properties.TaxCode, id);
            DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(TAXEN.TaxAttribute), filter.GetPredicate());
        }


        #endregion


        #region TaxCategory Methods

        public ParamTaxCategory CreateTaxCategory(ParamTaxCategory paramTaxCategory)
        {

            using (Transaction transaction = new Transaction())
            {
                try
                {
                    paramTaxCategory.Id = GetMaxTaxCategoryId() + 1;
                    TAXEN.TaxCategory entityTaxCategory = EntityAssembler.CreateParamTaxCategory(paramTaxCategory);
                    DataFacadeManager.Insert(entityTaxCategory);

                    transaction.Complete();

                    ParamTaxCategory paramTaxCategoryReturned = new ParamTaxCategory();
                    paramTaxCategoryReturned = ModelAssembler.CreateParamTaxCategory(entityTaxCategory);

                    return paramTaxCategoryReturned;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        public ParamTaxCategory UpdateTaxCategory(ParamTaxCategory paramTaxCategory)
        {
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    TAXEN.TaxCategory entityTaxCategory = EntityAssembler.CreateParamTaxCategory(paramTaxCategory);
                    DataFacadeManager.Update(entityTaxCategory);

                    transaction.Complete();

                    ParamTaxCategory paramTaxCategoryReturned = new ParamTaxCategory();
                    paramTaxCategoryReturned = ModelAssembler.CreateParamTaxCategory(entityTaxCategory);

                    return paramTaxCategoryReturned;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        internal List<ParamTaxCategory> GetTaxCategoriesByTaxId(int taxId)
        {
            ObjectCriteriaBuilder filterTaxRate = new ObjectCriteriaBuilder();
            filterTaxRate.Property(TAXEN.TaxCategory.Properties.TaxCode, typeof(TAXEN.TaxCategory).Name);
            filterTaxRate.Equal();
            filterTaxRate.Constant(taxId);
            BusinessCollection businessCollectionTaxCategories = DataFacadeManager.GetObjects(typeof(TAXEN.TaxCategory), filterTaxRate.GetPredicate());

            List<ParamTaxCategory> TaxCategoriesList = ModelAssembler.CreateTaxCategoriesSearched(businessCollectionTaxCategories);

            return TaxCategoriesList;
        }

        private void LoadTaxCategories(List<ParamTax> TempTaxList, string description)
        {
            if (!string.IsNullOrEmpty(description) && TempTaxList.Any())
            {
                ObjectCriteriaBuilder filterAttribute = new ObjectCriteriaBuilder();
                filterAttribute.Property(TAXEN.TaxCategory.Properties.TaxCode, typeof(TAXEN.TaxCategory).Name);
                filterAttribute.In();
                filterAttribute.ListValue();
                TempTaxList.ForEach(x => { filterAttribute.Constant(x.Id); x.TaxCategories = new List<ParamTaxCategory>(); });
                filterAttribute.EndList();

                var entity = DataFacadeManager.GetObjects(typeof(TAXEN.TaxCategory), filterAttribute.GetPredicate()).Cast<TAXEN.TaxCategory>().ToList();

                TempTaxList.ForEach(x => entity.Where(z => z.TaxCode == x.Id).ToList().ForEach(y => x.TaxCategories.Add(ModelAssembler.CreateParamTaxCategory(y))));
            }
        }

        public bool DeleteTaxCategoriesByTaxId(int categoryId, int idTax)
        {

            using (Transaction transaction = new Transaction())
            {
                try
                {

                    var filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(TAXEN.TaxCategory.Properties.TaxCode, idTax);
                    filter.And();
                    filter.PropertyEquals(TAXEN.TaxCategory.Properties.TaxCategoryCode, categoryId);

                    int CategoriesDeleted = 0;
                    CategoriesDeleted = DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(TAXEN.TaxCategory), filter.GetPredicate());

                    transaction.Complete();

                    if (CategoriesDeleted > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        private int GetMaxTaxCategoryId()
        {
            try
            {
                int taxCategoryId = 0;
                SelectQuery selectQuery = new SelectQuery();

                Function function = new Function(FunctionType.Max);

                function.AddParameter(new Column(TAXEN.TaxCategory.Properties.TaxCategoryCode, typeof(TAXEN.TaxCategory).Name));

                selectQuery.Table = new ClassNameTable(typeof(TAXEN.TaxCategory), typeof(TAXEN.TaxCategory).Name);
                selectQuery.AddSelectValue(new SelectValue(function, typeof(TAXEN.TaxCategory).Name));


                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        taxCategoryId = Convert.ToInt32(reader[0]);
                    }
                }

                return taxCategoryId;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }

        #endregion


        #region TaxCondition Methods

        public ParamTaxCondition CreateTaxCondition(ParamTaxCondition paramTaxCondition)
        {

            using (Transaction transaction = new Transaction())
            {
                try
                {
                    paramTaxCondition.Id = GetMaxTaxConditionId() + 1;
                    TAXEN.TaxCondition entityTaxCondition = EntityAssembler.CreateParamTaxCondition(paramTaxCondition);
                    DataFacadeManager.Insert(entityTaxCondition);

                    transaction.Complete();

                    ParamTaxCondition paramTaxConditionReturned = new ParamTaxCondition();
                    paramTaxConditionReturned = ModelAssembler.CreateParamTaxCondition(entityTaxCondition);

                    return paramTaxConditionReturned;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        public ParamTaxCondition UpdateTaxCondition(ParamTaxCondition paramTaxCondition)
        {
            using (Transaction transaction = new Transaction())
            {
                try
                {
                    TAXEN.TaxCondition entityTaxCondition = EntityAssembler.CreateParamTaxCondition(paramTaxCondition);
                    DataFacadeManager.Update(entityTaxCondition);

                    transaction.Complete();

                    ParamTaxCondition paramTaxConditionReturned = new ParamTaxCondition();
                    paramTaxConditionReturned = ModelAssembler.CreateParamTaxCondition(entityTaxCondition);

                    return paramTaxConditionReturned;
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        internal List<ParamTaxCondition> GetTaxConditionsByTaxId(int taxId)
        {
            ObjectCriteriaBuilder filterTaxRate = new ObjectCriteriaBuilder();
            filterTaxRate.Property(TAXEN.TaxCondition.Properties.TaxCode, typeof(TAXEN.TaxCondition).Name);
            filterTaxRate.Equal();
            filterTaxRate.Constant(taxId);
            BusinessCollection businessCollectionTaxConditions = DataFacadeManager.GetObjects(typeof(TAXEN.TaxCondition), filterTaxRate.GetPredicate());

            List<ParamTaxCondition> TaxConditionsList = ModelAssembler.CreateTaxConditionsSearched(businessCollectionTaxConditions);

            return TaxConditionsList;
        }

        private void LoadTaxConditions(List<ParamTax> TempTaxList, string description)
        {
            if (!string.IsNullOrEmpty(description) && TempTaxList.Any())
            {
                ObjectCriteriaBuilder filterAttribute = new ObjectCriteriaBuilder();
                filterAttribute.Property(TAXEN.TaxCondition.Properties.TaxCode, typeof(TAXEN.TaxCondition).Name);
                filterAttribute.In();
                filterAttribute.ListValue();
                TempTaxList.ForEach(x => { filterAttribute.Constant(x.Id); x.TaxConditions = new List<ParamTaxCondition>(); });
                filterAttribute.EndList();

                var entity = DataFacadeManager.GetObjects(typeof(TAXEN.TaxCondition), filterAttribute.GetPredicate()).Cast<TAXEN.TaxCondition>().ToList();

                TempTaxList.ForEach(x => entity.Where(z => z.TaxCode == x.Id).ToList().ForEach(y => x.TaxConditions.Add(ModelAssembler.CreateParamTaxCondition(y))));
            }
        }

        public bool DeleteTaxConditionsByTaxId(int conditionId, int idTax)
        {

            using (Transaction transaction = new Transaction())
            {
                try
                {

                    var filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(TAXEN.TaxCondition.Properties.TaxCode, idTax);
                    filter.And();
                    filter.PropertyEquals(TAXEN.TaxCondition.Properties.TaxConditionCode, conditionId);

                    int ConditionsDeleted = 0;
                    ConditionsDeleted = DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(TAXEN.TaxCondition), filter.GetPredicate());

                    transaction.Complete();

                    if (ConditionsDeleted > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Dispose();
                    throw new BusinessException(ex.Message, ex);
                }
            }
        }

        private int GetMaxTaxConditionId()
        {
            try
            {
                int taxCondtionId = 0;
                SelectQuery selectQuery = new SelectQuery();

                Function function = new Function(FunctionType.Max);

                function.AddParameter(new Column(TAXEN.TaxCondition.Properties.TaxConditionCode, typeof(TAXEN.TaxCondition).Name));

                selectQuery.Table = new ClassNameTable(typeof(TAXEN.TaxCondition), typeof(TAXEN.TaxCondition).Name);
                selectQuery.AddSelectValue(new SelectValue(function, typeof(TAXEN.TaxCondition).Name));


                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        taxCondtionId = Convert.ToInt32(reader[0]);
                    }
                }

                return taxCondtionId;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
        #endregion
    }
}
