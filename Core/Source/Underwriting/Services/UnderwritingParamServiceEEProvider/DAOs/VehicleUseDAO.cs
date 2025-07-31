// -----------------------------------------------------------------------
// <copyright file="VehicleUseDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres Gonzalez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UnderwritingParamService.EEProvider.DAOs
{
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Assemblers;
    using Sistran.Core.Application.UnderwritingParamService.EEProvider.Resources;
    using Sistran.Core.Application.UnderwritingParamService.Models;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.Queries;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using UTILEN = Sistran.Core.Services.UtilitiesServices.Enums;
    using UTILMO = Sistran.Core.Services.UtilitiesServices.Models;
    /// <summary>
    /// Dao para uso
    /// </summary>
    public class VehicleUseDAO
    {
        /// <summary>
        /// Inserta los tipos de uso a la Carrocería de vehículo
        /// </summary>
        /// <param name="vehicleBodyCode">Codigo Carrocería de vehículo</param>
        /// <param name="vehicleBodyUse">Listado de uso</param>
        /// <returns>Listado de usos</returns>
        public Result<List<ParamVehicleUse>, ErrorModel> InsertVehicleBodyUses(int vehicleBodyCode, List<ParamVehicleUse> vehicleBodyUse)
        {
            try
            {
                List<ParamVehicleUse> paramVehicleUses = new List<ParamVehicleUse>();
                if (vehicleBodyUse.Count > 0)
                {
                    List<VehicleBodyUse> vehicleBodyUses = EntityAssembler.CreateVehicleBodyUse(vehicleBodyCode, vehicleBodyUse);
                    BusinessCollection businessCollection = new BusinessCollection();
                    businessCollection.AddRange(vehicleBodyUses);
                    //TODO: Se debe realizar el seguimiento y verificación de por que el metodo
                    //InserObjects no se ejecuta correctamente 
                    //DataFacadeManager.Instance.GetDataFacade().InsertObjects(businessCollection);

                    foreach (var vehicleBoduUse in businessCollection)
                        DataFacadeManager.Instance.GetDataFacade().InsertObject(vehicleBoduUse);
                    

                    foreach (VehicleBodyUse item in businessCollection)
                    {
                        paramVehicleUses.Add(((ResultValue<ParamVehicleUse, ErrorModel>)ParamVehicleUse.CreateParamVehicleUse(item.VehicleUseCode, string.Empty)).Value);
                    }
                }

                return new ResultValue<List<ParamVehicleUse>, ErrorModel>(paramVehicleUses);
            }
            catch (Exception ex)
            {
                return new ResultError<List<ParamVehicleUse>, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorInsertUses }, Utilities.Enums.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Elimina las usos de una Carrocería de vehículo
        /// </summary>
        /// <param name="vehicleBodyCode">Codigo Carrocería de vehículo</param>
        public void DeleteVehicleBodyUses(int vehicleBodyCode)
        {
            var filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(VehicleBodyUse.Properties.VehicleBodyCode, vehicleBodyCode);
            DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(VehicleBodyUse), filter.GetPredicate());
        }

        /// <summary>
        /// Obtiene los tipos de usos por id
        /// </summary>
        /// <param name="idsVehicleUse">Listado de id</param>
        /// <returns>Listado de usos</returns>
        public Result<List<ParamVehicleUse>, ErrorModel> GetVehicleUsesByIds(List<int> idsVehicleUse)
        {
            try
            {
                List<ParamVehicleUse> vehicleUses = new List<ParamVehicleUse>();
                if (idsVehicleUse != null)
                {
                    var filter = new ObjectCriteriaBuilder();
                    filter.Property(VehicleUse.Properties.VehicleUseCode, typeof(VehicleUse).Name);
                    filter.In();
                    filter.ListValue();
                    foreach (int id in idsVehicleUse)
                    {
                        filter.Constant(id);
                    }

                    filter.EndList();
                    BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(VehicleUse), filter.GetPredicate()));
                    foreach (VehicleUse vehicleUse in businessCollection)
                    {
                        ParamVehicleUse paramVehicleUse = ((ResultValue<ParamVehicleUse, ErrorModel>)ParamVehicleUse.GetParamVehicleUse(vehicleUse.VehicleUseCode, vehicleUse.SmallDescription)).Value;
                        vehicleUses.Add(paramVehicleUse);
                    }
                }

                return new ResultValue<List<ParamVehicleUse>, ErrorModel>(vehicleUses);
            }
            catch (Exception ex)
            {
                return new ResultError<List<ParamVehicleUse>, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorGetUses }, Utilities.Enums.ErrorType.TechnicalFault, ex));
            }
        }

        /// <summary>
        /// Genera el archivo de excel
        /// </summary>
        /// <param name="vehicleBodyUse">Modelo de uso</param>
        /// <param name="fileName">Nombre del archivo</param>
        /// <returns>Error o archivo generado</returns>
        public Result<string, ErrorModel> GenerateFileToVehicleUse(ParamVehicleBodyUse vehicleBodyUse, string fileName)
        {
            try
            {
                UTILMO.FileProcessValue fileProcessValue = new UTILMO.FileProcessValue();
                fileProcessValue.Key1 = (int)UTILEN.FileProcessType.ParametrizationVehicleBodyUse;

                UTILMO.File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);
                if (file == null)
                {
                    return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorNotExistsTemplate }, Utilities.Enums.ErrorType.BusinessFault, null));
                }

                if (!file.IsEnabled)
                {
                    return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string>() { Errors.ErrorTempleteIsNotEnable }, Utilities.Enums.ErrorType.BusinessFault, null));
                }

                file.Name = fileName;
                List<UTILMO.Row> rows = new List<UTILMO.Row>();

                foreach (Models.ParamVehicleUse paramVehicleUse in vehicleBodyUse.VehicleUses)
                {
                    List<UTILMO.Field> fields = file.Templates[0].Rows[0].Fields.Select(p => new UTILMO.Field()
                    {
                        ColumnSpan = p.ColumnSpan,
                        Description = p.Description,
                        FieldType = p.FieldType,
                        Id = p.Id,
                        IsEnabled = p.IsEnabled,
                        IsMandatory = p.IsMandatory,
                        Order = p.Order,
                        RowPosition = p.RowPosition,
                        SmallDescription = p.SmallDescription
                    }).ToList();

                    if (fields.Count < 3)
                    {
                        return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string> { Errors.ErrorTemplateColumnsNotEqual }, Utilities.Enums.ErrorType.BusinessFault, null));
                    }

                    fields[0].Value = vehicleBodyUse.VehicleBody.Id.ToString();
                    fields[1].Value = vehicleBodyUse.VehicleBody.Description;
                    fields[2].Value = paramVehicleUse.Description;

                    rows.Add(new UTILMO.Row
                    {
                        Fields = fields
                    });
                }

                file.Templates[0].Rows = rows;
                file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                string generateFile = DelegateService.utilitiesServiceCore.GenerateFile(file);
                return new ResultValue<string, ErrorModel>(generateFile);
            }
            catch (Exception ex)
            {
                return new ResultError<string, ErrorModel>(ErrorModel.CreateErrorModel(new List<string> { Errors.ErrorGenerateFile }, Utilities.Enums.ErrorType.TechnicalFault, ex));
            }
        }
    }
}
