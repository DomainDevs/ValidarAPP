// -----------------------------------------------------------------------
// <copyright file="SubLineBusinessDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Camila Vergara</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{

    using COMMEN = Sistran.Core.Application.Common.Entities;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Sistran.Core.Application.CommonService.Models;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.DAOs;    
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Entities.Views;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.DAF.Engine;
    using Sistran.Core.Framework.Queries;
    using Sistran.Core.Application.UnderwritingParamServices.EEProvider.Assemblers;
    using UTMO = Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using Sistran.Core.Services.UtilitiesServices.Enums;


    /// <summary>
    /// Clase para el DAO SubRamoTecnico
    /// </summary>
    public class SubLineBusinessDAO
    { 
        /// <summary>
        /// Metodo para consultar SubRamo Tecnico
        /// </summary>
        /// <returns>Retorna lista SubRamo Tecnico</returns>
        public UTMO.Result<List<SubLineBusiness>, UTMO.ErrorModel> GetSubLinesBusiness()
        {
            SubLineBusinessParametrizationView view = new SubLineBusinessParametrizationView();
            ViewBuilder builder = new ViewBuilder("SubLineBusinessParametrizationView");
            List<SubLineBusiness> sublineBusinessSets = new List<SubLineBusiness>();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            if (view.SubLineBusiness.Count > 0)
            {
                List<SubLineBusiness> subLineBusiness = ModelAssembler.CreateSubLinesBusiness(view.SubLineBusiness);
                List<COMMEN.LineBusiness> entityLineBusiness = view.LineBusiness.Cast<COMMEN.LineBusiness>().ToList();

                foreach (SubLineBusiness subLineBusinessList in subLineBusiness)
                {
                    subLineBusinessList.LineBusinessDescription = entityLineBusiness.First(X => X.LineBusinessCode == subLineBusinessList.LineBusinessId).Description;
                    subLineBusinessList.LineBusinessId = entityLineBusiness.First(X => X.LineBusinessCode == subLineBusinessList.LineBusinessId).LineBusinessCode;
                    sublineBusinessSets.Add(subLineBusinessList);
                }
            }

            return new UTMO.ResultValue<List<SubLineBusiness>, UTMO.ErrorModel>(sublineBusinessSets);
        }

        /// <summary>
        /// Consulta SubRamo Tecnico por nombre 
        /// </summary>
        /// <param name="name">Recibe parametro nombre</param>
        /// <returns>Retorna listado de la consulta</returns>
        public UTMO.Result<List<SubLineBusiness>, UTMO.ErrorModel> GetSubLineBusinessByNameAndTitle(string name)
        {
            SubLineBusinessParametrizationView view = new SubLineBusinessParametrizationView();
            ViewBuilder builder = new ViewBuilder("SubLineBusinessParametrizationView");
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.SubLineBusiness.Properties.Description, typeof(COMMEN.SubLineBusiness).Name);
            filter.Like();
            filter.Constant("%" + name + "%");
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);
            List<SubLineBusiness> sublineBusinessSets = new List<SubLineBusiness>();
          
            if (view.SubLineBusiness.Count > 0)
            {
                List<SubLineBusiness> subLineBusiness = ModelAssembler.CreateSubLinesBusiness(view.SubLineBusiness);
                List<COMMEN.LineBusiness> entityLineBusiness = view.LineBusiness.Cast<COMMEN.LineBusiness>().ToList();

                foreach (SubLineBusiness subLineBusinessList in subLineBusiness)
                {
                    subLineBusinessList.LineBusinessDescription = entityLineBusiness.First(X => X.LineBusinessCode == subLineBusinessList.LineBusinessId).Description;
                    subLineBusinessList.LineBusinessId = entityLineBusiness.First(X => X.LineBusinessCode == subLineBusinessList.LineBusinessId).LineBusinessCode;
                    sublineBusinessSets.Add(subLineBusinessList);
                }
            }

            return new UTMO.ResultValue<List<SubLineBusiness>, UTMO.ErrorModel>(sublineBusinessSets);
        }

        /// <summary>
        /// Metodo que generar archivo excel
        /// </summary>
        /// <param name="subLineBusiness">Lista de subRamos tecnicos</param>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Retorna Archivo excel</returns>
        public UTMO.Result<string, UTMO.ErrorModel> GenerateFileToSubLineBusiness(List<SubLineBusiness> subLineBusiness, string fileName)
        {
            List<string> listErrors = new List<string>();
            try
            {
                FileProcessValue fileProcessValue = new FileProcessValue()
                {
                    Key1 = (int)FileProcessType.ParametrizationSubLineBusiness
                };
                FileDAO fileDAO = new FileDAO();
                File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    List<Row> rows = new List<Row>();

                    foreach (SubLineBusiness item in subLineBusiness)
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

                        fields[0].Value = item.LineBusinessId.ToString();
                        fields[1].Value = item.LineBusinessDescription.ToString();
                        fields[2].Value = item.Id.ToString();
                        fields[3].Value = item.Description.ToString();
                        fields[4].Value = item.SmallDescription.ToString();

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
                    listErrors.Add(Resources.Errors.ErrorDownloadingExcel);
                    return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, new System.ArgumentException(Resources.Errors.ErrorDownloadingExcel, "original")));
                }
            }
            catch (System.Exception ex)
            {
                listErrors.Add(Resources.Errors.ErrorDownloadingExcel);
                return new UTMO.ResultError<string, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, new System.ArgumentException(Resources.Errors.ErrorDownloadingExcel, "original")));                
            }
        }
    }
}
