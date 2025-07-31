using System;
using Newtonsoft.Json;
using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using Sistran.Core.Services.UtilitiesServices.Models;
using COMMEN = Sistran.Core.Application.Common.Entities;
using Sistran.Company.Application.MassiveVehicleParamBusinessService.Model;
using Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.Enum;

namespace Sistran.Company.Application.MassiveVehicleParamBusinessServiceProvider.Assembler
{
    public class ModelAssembler
    {
        public static String CreateSerializeObjectCode(Row row)
        {
            string novelty = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "Novedad")).ToString();
            string brand = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "Marca")).ToString();
            string autoClass = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "Clase")).ToString();
            string codigo = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "Codigo")).ToString();
            string homologoCode = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "HomologoCodigo")).ToString();
            string reference1 = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "Referencia1")).ToString();
            string reference2 = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "Referencia2")).ToString();
            string reference3 = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "Referencia3")).ToString();
            string weight = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "Peso")).ToString();
            string idService = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "IdServicio")).ToString();
            string service = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "Servicio")).ToString();
            string bcpp = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "Bcpp")).ToString();
            string import = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "Importado")).ToString();
            string power = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "Potencia")).ToString();
            string typeCabin = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "TipoCaja")).ToString();
            string cilindraje = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "Cilindraje")).ToString();
            string nationality = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "Nacionalidad")).ToString();
            string capacityPayers = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "CapacidadPasajeros")).ToString();
            string capacityCharge = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "CapacidadCarga")).ToString();
            string doors = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "Puertas")).ToString();
            string airConditioned = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "AireAcondicionado")).ToString();
            string axes = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "Ejes")).ToString();
            string state = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "Estado")).ToString();
            string fuel = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "Combustible")).ToString();
            string transmission = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "Transmision")).ToString();
            string um = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "Um")).ToString();
            string pesoCategory = DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == "PesoCategoria")).ToString();

            CompanyFasecoldaCode fasecoldaCode = new CompanyFasecoldaCode
            {
                Novelty = novelty,
                Brand = brand,
                AutoClass = autoClass,
                Codigo = codigo,
                HomologoCode = homologoCode,
                Reference1 = reference1,
                Reference2 = reference2,
                Reference3 = reference3,
                Weight = weight,
                IdService = idService,
                Service = service,
                BCPP = bcpp,
                Import = import,
                Power = power,
                TypeCabin = typeCabin,
                Cilindraje = cilindraje,
                Nationality = nationality,
                CapacityPayers = capacityPayers,
                CapacityCharge = capacityCharge,
                Doors = doors,
                AirConditioned = airConditioned,
                Axes = axes,
                State = state,
                Fuel = fuel,
                Transmission = transmission,
                UM = um,
                PesoCategory = pesoCategory
            };

            return JsonConvert.SerializeObject(fasecoldaCode);
        }

        public static CompanyProcessFasecolda CreateProcessFasecolda(COMMEN.CiaAsynchronousProcessFasecoldaMassiveLoad fasecoldaMassiveLoad)
        {
            return new CompanyProcessFasecolda
            {
                Id = fasecoldaMassiveLoad.Id,
                ProcessId = fasecoldaMassiveLoad.ProcessId,
                TotalRows = fasecoldaMassiveLoad.TotalRows,
                File = new File
                {
                    Name = fasecoldaMassiveLoad.FileName
                },
                ProcessStatusType = new CompanyVehicleFasecoldaStatusType
                {
                },
                TypeFile = (FileTypeFasecoldaEnum)fasecoldaMassiveLoad.TypeFile
            };
        }

        public static List<CompanyMassiveVehicleFasecoldaRow> CreateMassiveFasecoldaRow(BusinessCollection businessCollection)
        {
            List<CompanyMassiveVehicleFasecoldaRow> vehicleFasecoldaRows = new List<CompanyMassiveVehicleFasecoldaRow>();

            foreach (COMMEN.CiaAsynchronousProcessFasecoldaRow entityCollectiveEmissionRow in businessCollection)
            {
                vehicleFasecoldaRows.Add(CreateVehicleFasecoldaRow(entityCollectiveEmissionRow));
            }

            return vehicleFasecoldaRows;
        }

        public static CompanyMassiveVehicleFasecoldaRow CreateVehicleFasecoldaRow(COMMEN.CiaAsynchronousProcessFasecoldaRow ciaAsynchronousProcessFasecoldaRow)
        {
            return new CompanyMassiveVehicleFasecoldaRow()
            {
                Id = ciaAsynchronousProcessFasecoldaRow.Id,
                ProcessMassiveLoadId = ciaAsynchronousProcessFasecoldaRow.Id,
                ProcessId = ciaAsynchronousProcessFasecoldaRow.ProcessId,
                RowNumber = ciaAsynchronousProcessFasecoldaRow.RowNumber,
                HasError = ciaAsynchronousProcessFasecoldaRow.HasError.Value,
                Error_Description = ciaAsynchronousProcessFasecoldaRow.ErrorDescription,
                SerializedRow = ciaAsynchronousProcessFasecoldaRow.SerializedRow
            };
        }

    }
}
