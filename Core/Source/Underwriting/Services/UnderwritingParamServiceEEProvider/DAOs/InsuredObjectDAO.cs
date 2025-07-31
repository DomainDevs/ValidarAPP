// -----------------------------------------------------------------------
// <copyright file="InsuredObjectDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Heidy Pinto</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using Sistran.Co.Application.Data;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using COMMEN = Sistran.Core.Application.Common.Entities;
    using QUOEN = Sistran.Core.Application.Quotation.Entities;
    using UTIEN = Sistran.Core.Application.Utilities.Enums;
    using UTILEN = Sistran.Core.Services.UtilitiesServices.Enums;
    using UTILMO = Sistran.Core.Services.UtilitiesServices.Models;
    using UTIMO = Sistran.Core.Application.Utilities.Error;
    /// <summary>
    /// Acceso a DB de Objetos del seguro
    /// </summary>
    public class InsuredObjectDAO
    {
        /// <summary>
        /// Obtiene la informacion asociada a los objetos del seguro relacionado con el ramo tecnico
        /// </summary>
        /// <param name="lineBusinessId">id ramo tecnico</param>
        /// <returns>Amparos relacionados al ramo tecnico - MOD-B</returns>
        public UTIMO.Result<List<ParamInsuredObjectDesc>, UTIMO.ErrorModel> GetParamInsuredObjectDescsByLineBusinessId(int lineBusinessId)
        {
            try
            {
                List<ParamInsuredObjectDesc> paramInsuredObjectDescs = new List<ParamInsuredObjectDesc>();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(COMMEN.LineBusiness.Properties.LineBusinessCode, typeof(COMMEN.LineBusiness).Name, lineBusinessId);

                InsuredObjectLineBusinessView view = new InsuredObjectLineBusinessView();
                ViewBuilder viewBuilder = new ViewBuilder("InsuredObjectLineBusinessView");
                viewBuilder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, view);

                if (view.InsuredObjects.Count > 0)
                {
                    paramInsuredObjectDescs = ModelAssembler.CreateParamInsuredObjectDescs(view.InsuredObjects);
                }

                return new UTIMO.ResultValue<List<ParamInsuredObjectDesc>, UTIMO.ErrorModel>(paramInsuredObjectDescs);
            }
            catch (System.Exception ex)
            {
                return new UTIMO.ResultError<List<ParamInsuredObjectDesc>, UTIMO.ErrorModel>(UTIMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorDBGetInsuredObjectRelation }, UTIEN.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Obtener Objetos Del Seguro Por Descripción
        /// </summary>
        /// <param name="description">descripcion de objetos del seguro</param>
        /// <returns>Objetos Del Seguro</returns>
        public UTIMO.Result<List<Models.ParamInsuredObject>, UTIMO.ErrorModel> GetInsuredObjectsByDescription(string description)
        {
            List<Models.ParamInsuredObject> companyInsuredObjects = new List<Models.ParamInsuredObject>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(QUOEN.InsuredObject.Properties.Description, typeof(QUOEN.InsuredObject).Name).Like().Constant("%" + description + "%");
            filter.Or();
            filter.Property(QUOEN.InsuredObject.Properties.SmallDescription, typeof(QUOEN.InsuredObject).Name).Like().Constant("%" + description + "%");

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.InsuredObject), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                companyInsuredObjects = ModelAssembler.CreateCompanyInsuredObjects(businessCollection);
            }

            return new UTIMO.ResultValue<List<Models.ParamInsuredObject>, UTIMO.ErrorModel>(companyInsuredObjects);
        }

        /// <summary>
        /// Obtener Objetos Del Seguro 
        /// </summary>       
        /// <returns>Objetos Del Seguro</returns>
        public UTIMO.Result<List<Models.ParamInsuredObject>, UTIMO.ErrorModel> GetInsuredObjects()
        {
            List<Models.ParamInsuredObject> companyInsuredObjects = new List<Models.ParamInsuredObject>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.InsuredObject), filter.GetPredicate()));

            if (businessCollection.Count > 0)
            {
                companyInsuredObjects = ModelAssembler.CreateCompanyInsuredObjects(businessCollection);
            }

            return new UTIMO.ResultValue<List<Models.ParamInsuredObject>, UTIMO.ErrorModel>(companyInsuredObjects);
        }

        /// <summary>
        /// Genera archivo excel de objetos del seguro
        /// </summary>
        /// <param name="insuredObject">Listado de objectos de seguro</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Modelo result</returns>
        public UTIMO.Result<string, UTIMO.ErrorModel> GenerateFileToInsuredObject(List<Models.ParamInsuredObject> insuredObject, string fileName)
        {
            List<string> listErrors = new List<string>();
            listErrors.Add(Resources.Errors.ErrorDownloadingExcel);
            try
            {
                UTILMO.FileProcessValue fileProcessValue = new UTILMO.FileProcessValue()
                {
                    Key1 = (int)UTILEN.FileProcessType.ParametrizationInsuredObject
                };
                FileDAO fileDAO = new FileDAO();
                UTILMO.File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<UTILMO.Row> rows = new List<UTILMO.Row>();

                    foreach (Models.ParamInsuredObject item in insuredObject)
                    {
                        for (int i = 0; i < file.Templates[0].Rows.Count; i++)
                        {
                            var fields = file.Templates[0].Rows[i].Fields.Select(x => new UTILMO.Field
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

                            if (fields.Count > 1)
                            {
                                fields[0].Value = item.Id.ToString();
                                fields[1].Value = item.Description;
                                fields[2].Value = item.SmallDescription;
                                if (item.IsDeclarative)
                                {
                                    fields[3].Value = "SI";
                                }
                                else
                                {
                                    fields[3].Value = "NO";
                                }
                            }

                            rows.Add(new UTILMO.Row
                            {
                                Fields = fields
                            });
                        }
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                    var result = fileDAO.GenerateFile(file);
                    return new UTIMO.ResultValue<string, UTIMO.ErrorModel>(result);
                }
                else
                {
                    return new UTIMO.ResultValue<string, UTIMO.ErrorModel>(string.Empty);
                }
            }
            catch (System.Exception ex)
            {
                return new UTIMO.ResultError<string, UTIMO.ErrorModel>(UTIMO.ErrorModel.CreateErrorModel(listErrors, UTIEN.ErrorType.TechnicalFault, new System.ArgumentException(Resources.Errors.ErrorDownloadingExcel, "original")));
            }
        }

        /// <summary>
        /// Validación de objetos de seguro 
        /// </summary>
        /// <param name="insuredObjectId">Codigo de objeto de seguro </param>
        /// <returns>valor 1: tiene dependencias, 0: no tiene dependencias</returns> 
        public int ValidateInsuredObject(int insuredObjectId)
        {
            DataTable result;
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("@INSURED_OBJECT_ID", insuredObjectId);
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("QUO.VALIDATE_INSURED_OBJECT_PARAMETRIZATION", parameters);
            }

            return (int)result.Rows[0][0];
        }

        /// <summary>
        /// Obtiene la informacion asociada a los objetos del seguro relacionado con el ramo tecnico
        /// </summary>
        /// <returns>Amparos relacionados al ramo tecnico - MOD-B</returns>
        public UTIMO.Result<List<ParamInsuredObjectDesc>, UTIMO.ErrorModel> GetParamInsuredObjectDescs()
        {
            try
            {
                List<ParamInsuredObjectDesc> paramInsuredObjectDescs = new List<ParamInsuredObjectDesc>();
                //ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                //InsuredObjectLineBusinessView view = new InsuredObjectLineBusinessView();
                //ViewBuilder viewBuilder = new ViewBuilder("InsuredObjectLineBusinessView");
                //viewBuilder.Filter = filter.GetPredicate();

                //DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, view);


                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.InsuredObject)));



                paramInsuredObjectDescs = ModelAssembler.CreateParamInsuredObjectDescs(businessCollection);


                return new UTIMO.ResultValue<List<ParamInsuredObjectDesc>, UTIMO.ErrorModel>(paramInsuredObjectDescs);
            }
            catch (System.Exception ex)
            {
                return new UTIMO.ResultError<List<ParamInsuredObjectDesc>, UTIMO.ErrorModel>(UTIMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorGetInsuredObject }, UTIEN.ErrorType.TechnicalFault, ex));
            }
        }
    }
}
