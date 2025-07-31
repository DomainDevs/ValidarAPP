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
    public class VehicleTypeDAO
    {
        VehicleBodyDAO vehicleBodyDAO = new VehicleBodyDAO();

        public VehicleType CreateVehicleType(VehicleType vehicleType)
        {

            //convertir de model a entity
            COMMEN.VehicleType entityVehicleType = EntityAssembler.CreateVehicleType(vehicleType);

            SelectQuery selectQuery = new SelectQuery();
            //Function funtion = new Function(FunctionType.Max);

            //funtion.AddParameter(new Column(COMMEN.VehicleType.Properties.VehicleTypeCode));
            selectQuery.AddSelectValue(new SelectValue(new Column(COMMEN.VehicleType.Properties.VehicleTypeCode)));
            selectQuery.Table = new ClassNameTable(typeof(COMMEN.VehicleType), "v");
            //selectQuery.AddSelectValue(new SelectValue(funtion));


            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    entityVehicleType.VehicleTypeCode = (Convert.ToInt32(reader[0]) + 1);
                }
            }

            //realizar las operaciones con los entities utilizando DAF
            DataFacadeManager.Instance.GetDataFacade().InsertObject(entityVehicleType);

            //Falta luego de crear el VehicleType asociarlo con VehicleBody

            if (vehicleType.VehicleBodies.Count > 0)
            {
                foreach (var vehicleBody in vehicleType.VehicleBodies)
                {
                    vehicleBodyDAO.InsertVehicleTypeBodies(entityVehicleType.VehicleTypeCode, vehicleBody.Id);
                }
            }

            //return del model
            return ModelAssembler.CreateVehicleType(entityVehicleType);
        }

        public VehicleType UpdateVehicleType(VehicleType vehicleType)
        {
            COMMEN.VehicleType vehicleTypeEntity = EntityAssembler.CreateVehicleType(vehicleType);

            vehicleBodyDAO.DeleteVehicleTypeBodies(vehicleType.Id);

            DataFacadeManager.Update(vehicleTypeEntity);

            if (vehicleType.VehicleBodies.Count > 0)
            {
                foreach (var vehicleBody in vehicleType.VehicleBodies)
                {
                    vehicleBodyDAO.InsertVehicleTypeBodies(vehicleType.Id, vehicleBody.Id);
                }
            }

            return ModelAssembler.CreateVehicleType(vehicleTypeEntity);
        }

        public void DeleteVehicleType(VehicleType vehicleType)
        {
            var filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(COMMEN.VehicleType.Properties.VehicleTypeCode, vehicleType.Id);
            DataFacadeManager.Instance.GetDataFacade().DeleteObjects(typeof(COMMEN.VehicleType), filter.GetPredicate());
        }

        public List<VehicleType> GetVehicleTypes()
        {
            //Asignamos BusinessCollection a una Lista
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(
                typeof(COMMEN.VehicleType)));

            //return del model
            return ModelAssembler.CreateVehicleTypes(businessCollection);

        }

        public string GenerateFileToVehicleType(string fileName)
        {
            try
            {
                UTILMO.FileProcessValue fileProcessValue = new UTILMO.FileProcessValue();
                fileProcessValue.Key1 = (int)UTILEN.FileProcessType.ParametrizationVehicleType;

                UTILMO.File file = DelegateService.utilitiesServiceCore.GetFileByFileProcessValue(fileProcessValue);

                if ((file == null) || (!file.IsEnabled))
                {
                    return string.Empty;
                }

                file.Name = fileName;
                List<UTILMO.Row> rows = new List<UTILMO.Row>();

                List<VehicleType> vehicleTypesResult = this.GetVehicleTypes();

                if (!(vehicleTypesResult is List<VehicleType>))
                {
                    return "Error in GenerateFileToExportVehicleType";
                }
                else
                {
                    foreach (VehicleType vehicleType in vehicleTypesResult)
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

                        if (fields.Count < 5)
                        {
                            return "ErrorTemplateColumnsNotEqual";
                        }

                        string isTruck = string.Empty;
                        string isEnable = string.Empty;

                        fields[0].Value = vehicleType.Id.ToString();
                        fields[1].Value = vehicleType.Description;
                        fields[2].Value = vehicleType.SmallDescription;
                        fields[3].Value = vehicleType.IsTruck.ToStringFieldExcel();
                        fields[4].Value = vehicleType.IsActive.ToStringFieldExcel();

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
                throw new Exception("Error in GenerateFileToExportVehicleType", ex);
            }
        }
    }

}
