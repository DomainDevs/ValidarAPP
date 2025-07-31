// -----------------------------------------------------------------------
// <copyright file="PerilDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gómez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Sistran.Co.Application.Data;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using COMMEN = Sistran.Core.Application.Common.Entities;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using UTMO = Sistran.Core.Application.Utilities.Error;
    using PARUPSM = Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using QUOEN = Sistran.Core.Application.Quotation.Entities;
    using UNDMO = Sistran.Core.Application.UnderwritingServices.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using Sistran.Core.Services.UtilitiesServices.Enums;


    /// <summary>
    /// Clase pública amparos
    /// </summary>
    public class PerilDAO
    {
        /// <summary>
        /// Validación de amparos
        /// </summary>
        /// <param name="perilId">Codigo de amparos</param>
        /// <returns>valor 1: tiene dependencias, 0: no tiene dependencias</returns> 
        public int ValidatePeril(int perilId)
        {
            DataTable result;
            NameValue[] parameters = new NameValue[1];
            parameters[0] = new NameValue("@PERIL_ID", perilId);
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("QUO.VALIDATE_PERIL_PARAMETRIZATION", parameters);
            }

            return (int)result.Rows[0][0];
        }

        /// <summary>
        /// Genera archivo excel de amparos
        /// </summary>
        /// <param name="perils">Listado de amparos</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Modelo result</returns>
        public UTMO.Result<string, UTMO.ErrorModel> GenerateFileToPeril(List<Peril> perils, string fileName)
        {
            try
            {
                FileProcessValue fileProcessValue = new FileProcessValue()
                {
                    Key1 = (int)FileProcessType.ParametrizationPeril
                };
                FileDAO fileDAO = new FileDAO();
                File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    file.Templates[0].Rows = this.AssignValues(perils, file);
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
                listErrors.Add("Error Creando");
                return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Asigna los valores de las filas
        /// </summary>
        /// <param name="perils">Listado de Peril</param>
        /// <param name="file">Configuración archivo</param>
        /// <returns>Listado filas</returns>
        private List<Row> AssignValues(List<Peril> perils, File file)
        {
            List<Row> rows = new List<Row>();
            foreach (Peril item in perils)
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
                fields[1].Value = item.Description.ToString();
                fields[2].Value = item.SmallDescription.ToString();

                rows.Add(new Row
                {
                    Fields = fields
                });
            }

            return rows;
        }

        /// <summary>
        /// Obtiene la informacion asociada a los amparos relacionado con el ramo tecnico
        /// </summary>
        /// <param name="lineBusinessId">id ramo tecnico</param>
        /// <returns>Amparos relacionados al ramo tecnico - MOD-B</returns>
        public UTMO.Result<List<ParamPeril>, UTMO.ErrorModel> GetPerilsByLineBusinessId(int lineBusinessId)
        {
            try
            {
                var viewBuilder = new ViewBuilder("PerilLineBusinessView");
                var filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(COMMEN.LineBusiness.Properties.LineBusinessCode, typeof(COMMEN.LineBusiness).Name, lineBusinessId);
                viewBuilder.Filter = filter.GetPredicate();
                PerilLineBusinessView view = new PerilLineBusinessView();
                DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, view);
                List<ParamPeril> perils = view.Perils.Select(x => ModelAssembler.CreateParamPeril((QUOEN.Peril)x)).ToList();
                return new UTMO.ResultValue<List<ParamPeril>, UTMO.ErrorModel>(perils);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamPeril>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorGetPerils }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Obtiene la informacion asociada a los amparos 
        /// </summary>
        /// <returns>Lista de amparos</returns>
        public UTMO.Result<List<ParamPeril>, UTMO.ErrorModel> GetPerils()
        {
            try
            {
                //var viewBuilder = new ViewBuilder("PerilLineBusinessView");
                //var filter = new ObjectCriteriaBuilder();
                //viewBuilder.Filter = filter.GetPredicate();
                //PerilLineBusinessView view = new PerilLineBusinessView();
                //DataFacadeManager.Instance.GetDataFacade().FillView(viewBuilder, view);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(QUOEN.Peril)));
                List<ParamPeril> perils = businessCollection.Select(x => ModelAssembler.CreateParamPeril((QUOEN.Peril)x)).ToList();
                return new UTMO.ResultValue<List<ParamPeril>, UTMO.ErrorModel>(perils);
            }
            catch (System.Exception ex)
            {
                return new UTMO.ResultError<List<ParamPeril>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(new List<string>() { Resources.Errors.ErrorGetPerils }, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }
    }
}
