// -----------------------------------------------------------------------
// <copyright file="SalePointDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.CommonParamService.EEProvider.DAOs
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using Co.Application.Data;
    using CommonParamService.Assemblers;
    using CommonParamService.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Framework.Queries;
    using Utilities.Error;
    using COMEN = Sistran.Core.Application.Common.Entities;
    using ENUMUT = Sistran.Core.Application.Utilities.Enums;
    using UTMO = Sistran.Core.Application.Utilities.Error;
    using Framework.DAF.Engine;
    using Entities.Views;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;

    /// <summary>
    /// DAO De Sucursal
    /// </summary>
    public class SalePointDAO
    {
        /// <summary>
        /// Actualiza los parametros del modelo de negocio de puntos de venta
        /// </summary>       
        /// <returns>Objeto de ParamParameter</returns>
        public UTMO.Result<List<ParamSalePoint>, ErrorModel> GetSalePointes()
        {
            try
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                SalePointView view = new SalePointView();
                ViewBuilder builder = new ViewBuilder("SalePointView");
                builder.Filter = filter.GetPredicate();

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                var companySalePoints = ModelAssembler.CreateCompanySalePoints(view.SalePoint);
                foreach (var salePoint in companySalePoints)
                {
                    salePoint.Branch.Description = (view.Branch.First(x => ((COMEN.Branch)x).BranchCode == salePoint.Branch.Id) as COMEN.Branch).Description;
                }

                return new UTMO.ResultValue<List<ParamSalePoint>, UTMO.ErrorModel>(companySalePoints);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(CommonParamService.Resources.Errors.ErrorGetParameter);
                return new UTMO.ResultError<List<ParamSalePoint>, UTMO.ErrorModel>(UTMO.ErrorModel.CreateErrorModel(listErrors, ENUMUT.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Obtener Sucursal Por Descripción
        /// </summary>
        /// <param name="description">descripcion de Sucursal</param>
        /// <returns>retorna Sucursal</returns>
        public UTMO.Result<List<ParamSalePoint>, UTMO.ErrorModel> GetSalePointsByDescription(string description)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMEN.SalePoint.Properties.Description, typeof(COMEN.SalePoint).Name).Like().Constant("%" + description + "%");
            filter.Or();
            filter.Property(COMEN.SalePoint.Properties.SmallDescription, typeof(COMEN.SalePoint).Name).Like().Constant("%" + description + "%");

            SalePointView view = new SalePointView();
            ViewBuilder builder = new ViewBuilder("SalePointView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            var companySalePoints = ModelAssembler.CreateCompanySalePoints(view.SalePoint);
            foreach (var salePoint in companySalePoints)
            {
                salePoint.Branch.Description = (view.Branch.First(x => ((COMEN.Branch)x).BranchCode == salePoint.Branch.Id) as COMEN.Branch).Description;
            }

            return new UTMO.ResultValue<List<ParamSalePoint>, UTMO.ErrorModel>(companySalePoints);
        }

        /// <summary>
        /// Obtener Sucursal Por Descripción
        /// </summary>
        /// <param name="description">descripcion de Sucursal</param>
        /// <returns>retorna Sucursal</returns>
        public UTMO.Result<List<ParamSalePoint>, UTMO.ErrorModel> GetSalePointsByBranchCode(int branchId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMEN.SalePoint.Properties.BranchCode, typeof(COMEN.SalePoint).Name).Equal().Constant(branchId);         

            SalePointView view = new SalePointView();
            ViewBuilder builder = new ViewBuilder("SalePointView");
            builder.Filter = filter.GetPredicate();

            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            var companySalePoints = ModelAssembler.CreateCompanySalePoints(view.SalePoint);
            foreach (var salePoint in companySalePoints)
            {
                salePoint.Branch.Description = (view.Branch.First(x => ((COMEN.Branch)x).BranchCode == salePoint.Branch.Id) as COMEN.Branch).Description;
            }

            return new UTMO.ResultValue<List<ParamSalePoint>, UTMO.ErrorModel>(companySalePoints);
        }

        /// <summary>
        /// Genera archivo excel de Sucursal
        /// </summary>
        /// <param name="salePoint">Listado de objectos de seguro</param>
        /// <param name="fileName">Nombre de archivo</param>
        /// <returns>Modelo result</returns>
        public UTMO.Result<string, UTMO.ErrorModel> GenerateFileToSalePoint(List<ParamSalePoint> salePoint, string fileName)
        {
            try
            {
                FileProcessValue fileProcessValue = new FileProcessValue()
                {
                    Key1 = (int)FileProcessType.ParametrizationSalePoint
                };
                FileDAO fileDAO = new FileDAO();
                File file = fileDAO.GetFileByFileProcessValue(fileProcessValue);

                if (file != null && file.IsEnabled)
                {
                    file.Name = fileName;
                    file.Templates[0].Rows = this.AssignValues(salePoint, file);
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
        /// Validación de objetos de seguro 
        /// </summary>
        /// <param name="salePointId">Codigo de objeto de seguro </param>
        /// <returns>valor 1: tiene dependencias, 0: no tiene dependencias</returns> 
        public int ValidateSalePoint(int salePointId, int branchId)
        {
            DataTable result;
            NameValue[] parameters = new NameValue[2];
            parameters[0] = new NameValue("@SALE_POINT_CD", salePointId);
            parameters[1] = new NameValue("@BRANCHID", branchId);
            using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess())
            {
                result = dynamicDataAccess.ExecuteSPDataTable("COMM.VALIDATE_SALE_POINT_PARAMETRIZATION", parameters);
            }

            return (int)result.Rows[0][0];
        }

        /// <summary>
        /// Asigna los valores de las filas
        /// </summary>
        /// <param name="salePoint" >Listado de Sucursales</param>
        /// <param name="file">Configuración archivo</param>
        /// <returns>Listado filas</returns>
        private List<Row> AssignValues(List<ParamSalePoint> salePoint, File file)
        {
            List<Row> rows = new List<Row>();
            foreach (ParamSalePoint item in salePoint)
            {
                var fields = file.Templates[0].Rows[0].Fields.OrderBy(x => x.Order).Select(x => new Field
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

                fields[0].Value = item.Branch.Description;   
                fields[1].Value = item.Branch.Id.ToString();
                fields[2].Value = item.Description;
                fields[3].Value = item.SmallDescription;

                rows.Add(new Row
                {
                    Fields = fields
                });

            }

            return rows;
        }
    }
}
