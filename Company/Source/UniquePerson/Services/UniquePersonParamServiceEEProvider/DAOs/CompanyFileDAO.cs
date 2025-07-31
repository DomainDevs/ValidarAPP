// -----------------------------------------------------------------------
// <copyright file="FileDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Desconocido</author>
// -----------------------------------------------------------------------
namespace Sistran.Company.Application.UniquePersonParamService.EEProvider.DAOs
{
    using Sistran.Company.Application.UniquePersonParamService.Models;
    using Sistran.Core.Application.UtilitiesServicesEEProvider.DAOs;
    using Sistran.Core.Services.UtilitiesServices.Enums;
    using Sistran.Core.Services.UtilitiesServices.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Clase para generar archivos a exportar.
    /// </summary>
    public class CompanyFileDAO
    {
        /// <summary>
        /// Genera archivo excel para aliados
        /// </summary>
        /// <param name="alliancesList">Lista de todos los aliados</param>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Url de desacarga del archivo</returns>
        public string GenerateFileToAlliance(List<Alliance> alliancesList, string fileName)
        {
            FileDAO companyFileDao = new FileDAO();
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.ParametrizationAlliance;

            File file = companyFileDao.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (Alliance alliance in alliancesList)
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

                    fields[0].Value = alliance.AllianceId.ToString();
                    fields[1].Value = alliance.Description;
                    fields[2].Value = alliance.IsScore == true ? "SI" : "NO";
                    fields[3].Value = alliance.IsFine == true ? "SI" : "NO";

                    rows.Add(new Row
                    {
                        Fields = fields
                    });
                }

                file.Templates[0].Rows = rows;
                file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");
                
                return companyFileDao.GenerateFile(file);
                
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Genera archivo excel para sucursal aliados
        /// </summary>
        /// <param name="branchAlliancesList">Lista de todas las sucursales de aliados</param>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Url de desacarga del archivo</returns>
        public string GenerateFileToBranchAlliance(List<BranchAlliance> branchAlliancesList, string fileName)
        {
            
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.ParametrizationBranchAlliance;
            FileDAO companyFileDao = new FileDAO();
            File file = companyFileDao.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (BranchAlliance branch in branchAlliancesList)
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

                    fields[0].Value = branch.BranchId.ToString();
                    fields[1].Value = branch.BranchDescription;
                    fields[2].Value = branch.CityName;
                    fields[3].Value = branch.StateName;

                    rows.Add(new Row
                    {
                        Fields = fields
                    });
                }

                file.Templates[0].Rows = rows;
                file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");
                
                return companyFileDao.GenerateFile(file);
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Genera archivo excel para puntos de venta de aliados
        /// </summary>
        /// <param name="salePointsAlliancesList">Lista de todos los puntos de venta de aliados</param>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Url de desacarga del archivo</returns>
        public string GenerateFileToSalePointsAlliance(List<AllianceBranchSalePonit> salePointsAlliancesList, string fileName)
        {
            FileProcessValue fileProcessValue = new FileProcessValue();
            fileProcessValue.Key1 = (int)FileProcessType.ParametrizationSalePointsAlliance;
            FileDAO companyFileDao = new FileDAO();
            File file = companyFileDao.GetFileByFileProcessValue(fileProcessValue);

            if (file != null && file.IsEnabled)
            {
                file.Name = fileName;
                List<Row> rows = new List<Row>();

                foreach (AllianceBranchSalePonit salePoint in salePointsAlliancesList)
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

                    fields[0].Value = salePoint.AllianceId.ToString();
                    fields[1].Value = salePoint.AllianceDescription.ToString();
                    fields[2].Value = salePoint.BranchId.ToString();
                    fields[3].Value = salePoint.BranchDescription.ToString();
                    fields[4].Value = salePoint.SalePointId.ToString();
                    fields[5].Value = salePoint.SalePointDescription;

                    rows.Add(new Row
                    {
                        Fields = fields
                    });
                }

                file.Templates[0].Rows = rows;
                file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");
                return companyFileDao.GenerateFile(file);
            }
            else
            {
                return "";
            }
        }
    }
}
