using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMMEN = Sistran.Core.Application.Common.Entities;
using UTILMO = Sistran.Core.Services.UtilitiesServices.Models;
using UTILEN = Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class VehicleBodyDAO
    {

        public List<VehicleBody> GetVehicleTypeBodiesByVehicleType(int vehicleTypeCode)
        {

            List<VehicleBody> vehicleBodies = new List<VehicleBody>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(COMMEN.VehicleTypeBody.Properties.VehicleTypeCode, vehicleTypeCode);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.VehicleTypeBody), filter.GetPredicate()));

            foreach (COMMEN.VehicleTypeBody item in businessCollection)
            {
                vehicleBodies.Add(GetVehicleBodyByVehicleTypeCode(item.VehicleBodyCode));
            }

            return vehicleBodies;
        }

        public VehicleBody GetVehicleBodyByVehicleTypeCode(int vehicleBodyCode)
        {
            PrimaryKey primaryKey = COMMEN.VehicleBody.CreatePrimaryKey(vehicleBodyCode);

            COMMEN.VehicleBody vehicleBody = (COMMEN.VehicleBody)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

            return ModelAssembler.CreateVehicleBody(vehicleBody);
        }

        public List<VehicleBody> GetVehicleBodies()
        {

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.VehicleBody)));

            return ModelAssembler.CreateVehicleBodies(businessCollection);
        }

        public void DeleteVehicleTypeBodies(int vehicleTypeCode)
        {
            var filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(COMMEN.VehicleTypeBody.Properties.VehicleTypeCode, vehicleTypeCode);
            DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(COMMEN.VehicleTypeBody), filter.GetPredicate());
        }

        public List<VehicleBody> InsertVehicleTypeBodies(int vehicleTypeCode, int vehicleBodyCode)
        {
            try
            {
                List<VehicleBody> vehicleBodies = new List<VehicleBody>();

                COMMEN.VehicleTypeBody vehicleTypeBody = new COMMEN.VehicleTypeBody(vehicleTypeCode, vehicleBodyCode);

                DataFacadeManager.Insert(vehicleTypeBody);

                return vehicleBodies;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in InsertVehicleTypeBodies", ex);
            }
        }

        public string GenerateFileToExportVehicleBody(VehicleType vehicleType, string fileName)
        {
            try
            {
                UTILMO.FileProcessValue fileProcessValue = new UTILMO.FileProcessValue();
                fileProcessValue.Key1 = (int)UTILEN.FileProcessType.ParametrizationVehicleTypeBody;

                UTILMO.File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);
                if ((file == null) || (!file.IsEnabled))
                {
                    return string.Empty;
                }

                file.Name = fileName;
                List<UTILMO.Row> rows = new List<UTILMO.Row>();

                List<Models.VehicleBody> vehicleBodiesResult = GetVehicleTypeBodiesByVehicleType(vehicleType.Id);

                if (!(vehicleBodiesResult is List<Models.VehicleBody>))
                {
                    return "Error in GenerateFileToExportVehicleBody";
                }
                else
                {
                    foreach (Models.VehicleBody vehicleBody in vehicleBodiesResult)
                    {
                        List<UTILMO.Field> fields = file.Templates[0].Rows[0].Fields.OrderBy(x => x.Order).Select(p => new UTILMO.Field()
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
                            return "ErrorTemplateColumnsNotEqual";
                        }

                        string isTruck = string.Empty;
                        string isEnable = string.Empty;

                        fields[0].Value = vehicleType.Id.ToString();
                        fields[1].Value = vehicleType.Description;
                        fields[2].Value = vehicleBody.SmallDescription;

                        rows.Add(new UTILMO.Row
                        {
                            Fields = fields
                        });
                    }

                    file.Templates[0].Rows = rows;
                    file.Name = fileName + "_" + DateTime.Now.ToString("dd_MM_yyyy");

                    string generateFile = DelegateService.utilitiesServiceCore.GenerateFile(file);
                    return generateFile;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GenerateFileToExportVehicleBody", ex);
            }
        }
    }
}
