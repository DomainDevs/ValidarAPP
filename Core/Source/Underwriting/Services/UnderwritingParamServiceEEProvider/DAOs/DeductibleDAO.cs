// -----------------------------------------------------------------------
// <copyright file="DeductibleDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using Sistran.Co.Application.Data;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs;    
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using COMMEN = Sistran.Core.Application.Common.Entities;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using PARAMEN = Sistran.Core.Application.Parameters.Entities;
    using QUOEN = Sistran.Core.Application.Quotation.Entities;
    using UTMO = Sistran.Core.Application.Utilities.Error;

    /// <summary>
    /// Clase pública deducibles
    /// </summary>
    public class DeductibleDAO
    {
        /// <summary>
        /// Obtene las unidades de los deducibles
        /// </summary>
        /// <returns>Unidades de deducibles</returns>
        public UTMO.Result<List<DeductibleUnit>, UTMO.ErrorModel> GetDeductibleUnits()
        {
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PARAMEN.DeductibleUnit)));
                List<DeductibleUnit> deductibleUnits = ModelAssembler.CreateDeductibleUnits(businessCollection);
                return new UTMO.ResultValue<List<DeductibleUnit>, UTMO.ErrorModel>(deductibleUnits);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add("Error");
                return new UTMO.ResultError<List<DeductibleUnit>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Obtene el asunto de los deducibles
        /// </summary>
        /// <returns>Unidades de deducibles</returns>    
        public UTMO.Result<List<DeductibleSubject>, UTMO.ErrorModel> GetDeductibleSubjects()
        {
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PARAMEN.DeductibleSubject)));
                List<DeductibleSubject> deductibleSubject = ModelAssembler.CreateDeductibleSubjects(businessCollection);
                return new UTMO.ResultValue<List<DeductibleSubject>, UTMO.ErrorModel>(deductibleSubject);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add("Error Creando");
                return new UTMO.ResultError<List<DeductibleSubject>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Obtiene los deducibles
        /// </summary>
        /// <returns>Listado de deducible</returns>
        public UTMO.Result<List<Deductible>, UTMO.ErrorModel> GetDeductibles()
        {
            List<string> listErrors = new List<string>();
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                ParamDeductibleView view = new ParamDeductibleView();
                ViewBuilder builder = new ViewBuilder("ParamDeductibleView");
                builder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                var deductibles = ModelAssembler.CreateDeductibles(view.Deductibles);

                foreach (var deductible in deductibles)
                {
                    deductible.DeductibleUnit.Description = (view.DeductibleUnits.FirstOrDefault(x => ((PARAMEN.DeductibleUnit)x).DeductUnitCode == deductible.DeductibleUnit.Id) as PARAMEN.DeductibleUnit)?.Description;
                    deductible.MinDeductibleUnit.Description = (view.MinimumDeductibleUnits.FirstOrDefault(x => ((PARAMEN.DeductibleUnit)x).DeductUnitCode == deductible.MinDeductibleUnit.Id) as PARAMEN.DeductibleUnit)?.Description;
                    deductible.DeductibleSubject.Description = (view.DeductibleSubjects.FirstOrDefault(x => ((PARAMEN.DeductibleSubject)x).DeductSubjectCode == deductible.DeductibleSubject.Id) as PARAMEN.DeductibleSubject)?.Description;
                    deductible.MinDeductibleSubject.Description = (view.MinimumDeductibleSubjects.FirstOrDefault(x => ((PARAMEN.DeductibleSubject)x).DeductSubjectCode == deductible.MinDeductibleSubject.Id) as PARAMEN.DeductibleSubject)?.Description;
                    deductible.LineBusiness.Description = (view.LineBusinesses?.FirstOrDefault(x => ((COMMEN.LineBusiness)x).LineBusinessCode == deductible.LineBusiness.Id) as COMMEN.LineBusiness)?.Description;
                    deductible.MaxDeductibleUnit.Description = (view.MaximumDeductibleUnits?.FirstOrDefault(x => ((PARAMEN.DeductibleUnit)x).DeductUnitCode == deductible.MaxDeductibleUnit.Id) as PARAMEN.DeductibleUnit)?.Description;
                    deductible.MaxDeductibleSubject.Description = (view.MaximumDeductibleSubjects?.FirstOrDefault(x => ((PARAMEN.DeductibleSubject)x).DeductSubjectCode == deductible.MaxDeductibleSubject.Id) as PARAMEN.DeductibleSubject)?.Description;
                    COMMEN.Currency currency = (COMMEN.Currency)view.Currencies.FirstOrDefault(x => ((COMMEN.Currency)x).CurrencyCode == deductible.Currency.Id);
                    if (currency != null)
                    {
                        deductible.Currency.Description = currency.Description;
                    }
                }

                return new UTMO.ResultValue<List<Deductible>, UTMO.ErrorModel>(deductibles);
            }
            catch (System.Exception ex)
            {
                listErrors.Add("Error Creando");
                return new UTMO.ResultError<List<Deductible>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Validación de deducibles
        /// </summary>
        /// <param name="deductibleId">Codigo de deducible</param>
        /// <returns>valor 1: tiene dependencias, 0: no tiene dependencias</returns> 
        public int ValidateDeductible(int deductibleId)
        {
            DataTable result;
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("@DEDUCTIBLE_ID", deductibleId);
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("QUO.VALIDATE_DEDUCTIBLE_PARAMETRIZATION", parameters);
            }

            return (int)result.Rows[0][0];
        }

        /// <summary>
        /// Genera archivo excel de coverturas
        /// </summary>
        /// <param name="deductibles">Listado de deducibles</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Modelo result</returns>
        public UTMO.Result<string, UTMO.ErrorModel> GenerateFileToDeductible(List<Deductible> deductibles, string fileName)
        {
            try
            {
                FileProcessValue fileProcessValue = new FileProcessValue()
                {
                    Key1 = (int)FileProcessType.ParametrizationDeductible
                };
                FileDAO fileDAO = new FileDAO();
                File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<Row> rows = new List<Row>();

                    foreach (Deductible item in deductibles)
                    {
                        var fields = file.Templates[0].Rows[0].Fields.Select(x => new Field
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

                        fields[0].Value = item.Id.ToString();
                        fields[1].Value = item.DeductValue.ToString();
                        fields[2].Value = item.DeductibleUnit.Description;
                        fields[3].Value = item.DeductibleSubject == null ? string.Empty : item.DeductibleSubject.Description;
                        fields[4].Value = item.MinDeductValue == null ? string.Empty : item.MinDeductValue.ToString();
                        fields[5].Value = item.MinDeductibleUnit.Description == null ? string.Empty : item.MinDeductibleUnit.Description.ToString();
                        fields[6].Value = item.MinDeductibleSubject.Description == null ? string.Empty : item.MinDeductibleSubject.Description.ToString();
                        fields[7].Value = item.MaxDeductValue == null ? string.Empty : item.MaxDeductValue.ToString();
                        fields[8].Value = item.MaxDeductibleUnit.Description == null ? string.Empty : item.MaxDeductibleUnit.Description;
                        fields[9].Value = item.MaxDeductibleSubject.Description == null ? string.Empty : item.MaxDeductibleSubject.Description;
                        fields[10].Value = item.LineBusiness.Description == null ? string.Empty : item.LineBusiness.Description.ToString();
                        fields[11].Value = item.Currency.Description == null ? string.Empty : item.Currency.Description.ToString();
                        if (item.RateType == RateType.FixedValue)
                        {
                            fields[12].Value = Resources.Errors.FixedValue;
                        }
                        else if (item.RateType ==RateType.Percentage)
                        {
                            fields[12].Value = Resources.Errors.Percentage;
                        }
                        else if (item.RateType == RateType.Permilage)
                        {
                            fields[12].Value = Resources.Errors.Permilage;
                        }

                        fields[13].Value = item.Rate.ToString();

                        rows.Add(new Row
                        {
                            Fields = fields
                        });
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                    var result = fileDAO.GenerateFile(file);
                    return new UTMO.ResultValue<string, UTMO.ErrorModel>(result);
                }
                else
                {
                    return new UTMO.ResultValue<string, UTMO.ErrorModel>(string.Empty);
                }
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add("Error Descargando excel");
                return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Obtiene los deducibles
        /// </summary>
        /// <returns>listado de deducibles</returns>
        public UTMO.Result<List<ParamDeductibleDesc>, UTMO.ErrorModel> GetParamDeductibleDescs()
        {
            try
            {
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.Deductible)));
                List<ParamDeductibleDesc> paramDeductibleDescs = ModelAssembler.CreateParamDeductibleDescs(businessCollection);
                return new UTMO.ResultValue<List<ParamDeductibleDesc>, UTMO.ErrorModel>(paramDeductibleDescs);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamDeductibleDesc>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorDBGetDeductible }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Obtiene listado relacionado de deducibles con Id de cobertura
        /// </summary>
        /// <param name="coverageId">id de cobertura</param>
        /// <returns>listado de deducibles</returns>
        public UTMO.Result<List<ParamDeductibleDesc>, UTMO.ErrorModel> GetParamDeductibleDescsByCoverageId(int coverageId)
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(QUOEN.CoverageDeductible.Properties.CoverageId, typeof(QUOEN.CoverageDeductible).Name);
                filter.Equal();
                filter.Constant(coverageId);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.CoverageDeductible), filter.GetPredicate()));
                List<ParamDeductibleDesc> paramDeductibleDescs = ModelAssembler.CreateParamDeductibleDescsRelation(businessCollection);
                return new UTMO.ResultValue<List<ParamDeductibleDesc>, UTMO.ErrorModel>(paramDeductibleDescs);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamDeductibleDesc>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorDBGetDeductibleRelation }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Crea el deducible
        /// </summary>
        /// <param name="paramDeductibleDesc">deducible a crear</param>
        /// <param name="coverageId">id de cobertura</param>
        /// <returns>deducible creado</returns>
        public UTMO.Result<ParamDeductibleDesc, UTMO.ErrorModel> CreateParamDeductibleDesc(ParamDeductibleDesc paramDeductibleDesc, int coverageId)
        {
            try
            {
                QUOEN.CoverageDeductible coverageDeductibleEntity = EntityAssembler.CreateCoverageDeductible(paramDeductibleDesc, coverageId);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(coverageDeductibleEntity);
                ParamDeductibleDesc result = ModelAssembler.CreateParamDeductibleDesc(coverageDeductibleEntity);
                return new UTMO.ResultValue<ParamDeductibleDesc, UTMO.ErrorModel>(result);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<ParamDeductibleDesc, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorDBCreateDeductibleRelation }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }
    }
}
