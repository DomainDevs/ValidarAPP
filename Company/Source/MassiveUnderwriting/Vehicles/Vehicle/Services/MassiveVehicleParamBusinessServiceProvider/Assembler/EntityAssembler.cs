using System;
using COMMEN = Sistran.Core.Application.Common.Entities;
using TMPEN = Sistran.Core.Application.Temporary.Entities;
using Sistran.Company.Application.MassiveVehicleParamBusinessService.Model;

namespace Sistran.Company.Application.MassiveVehicleParamBusinessServiceProvider.Assembler
{
    public class EntityAssembler
    {
        internal static TMPEN.LoadFasecoldaPrice CreateFasecoldaPrice(CompanyFasecoldaPrice fasecoldaPrice)
        {
            TMPEN.LoadFasecoldaPrice entityFasecoldaPrice = new TMPEN.LoadFasecoldaPrice()
            {
                FasecoldaCode = fasecoldaPrice.Codigo,
                Model = fasecoldaPrice.Modelo,
                VehiclePrice = fasecoldaPrice.Valor
            };

            return entityFasecoldaPrice;
        }

        internal static TMPEN.LoadFasecoldaCod CreateFasecoldaCode(CompanyFasecoldaCode fasecoldaCode)
        {
            TMPEN.LoadFasecoldaCod entityFasecoldaCod = new TMPEN.LoadFasecoldaCod()
            {
                NoveltyCode = fasecoldaCode.Novelty,
                VehicleMakeDesc = fasecoldaCode.Brand,
                VehicleTypeDesc = fasecoldaCode.AutoClass,
                FasecoldaCode = fasecoldaCode.Codigo,
                FasecoldaCdHo = fasecoldaCode.HomologoCode,
                Reference1 = fasecoldaCode.Reference1,
                Reference2 = fasecoldaCode.Reference2,
                Reference3 = fasecoldaCode.Reference3,
                Weigth = Convert.ToInt32(fasecoldaCode.Weight),
                ServiceTypeCode = Convert.ToInt32(fasecoldaCode.IdService),
                ServiceDesc = fasecoldaCode.Service,
                Bcpp = Convert.ToInt32(fasecoldaCode.BCPP),
                LastModel = fasecoldaCode.Import == "1" ? true : false,
                Horsepower = Convert.ToInt32(fasecoldaCode.Power),
                TopSpeed = fasecoldaCode.TypeCabin,
                EngineCc = Convert.ToInt32(fasecoldaCode.Cilindraje),
                Nationality = fasecoldaCode.Nationality,
                PassengerQuantity = Convert.ToInt32(fasecoldaCode.CapacityPayers),
                TonsQuantity = Convert.ToInt32(fasecoldaCode.CapacityCharge),
                DoorQuantity = Convert.ToInt32(fasecoldaCode.Doors),
                AirConditioning = fasecoldaCode.AirConditioned == "1" ? true : false,
                VehicleAxleQuantity = Convert.ToInt32(fasecoldaCode.Axes),
                Status = fasecoldaCode.State,
                VehicleFuelDesc = fasecoldaCode.Fuel,
                Transmission = fasecoldaCode.Transmission,
                Um = fasecoldaCode.UM == "1" ? true : false,
                WeigthCategoryCode = Convert.ToInt32(fasecoldaCode.PesoCategory)
            };

            return entityFasecoldaCod;
        }
        
        internal static COMMEN.AsynchronousProcess CreateAsynchronousProcess(CompanyProcessFasecolda companyProcessFasecolda)
        {
            COMMEN.AsynchronousProcess asynchronousProcess = new COMMEN.AsynchronousProcess
            {
                Description = companyProcessFasecolda.Description,
                BeginDate = companyProcessFasecolda.BeginDate,
                EndDate = companyProcessFasecolda.EndDate,
                UserId = companyProcessFasecolda.User.UserId,
                /*EN COMENTARTIO CON PROPOSITOS DELA INTEGRACIÓN
                Status = companyProcessFasecolda.Status, // Falta agregar este campo a la entidad y al diccionario
                */
                HasError = companyProcessFasecolda.HasError,
                ErrorDescription = companyProcessFasecolda.Error_Description,
                //IssuanceStatus = null,
                //Active = companyProcessFasecolda.Active,
                //StatusId = (int)companyProcessFasecolda.ProcessStatus,
            };

            if (companyProcessFasecolda.ProcessId > 0)
            {
                asynchronousProcess.ProcessId = companyProcessFasecolda.ProcessId;
            }

            return asynchronousProcess;
        }

        internal static COMMEN.CiaAsynchronousProcessFasecoldaMassiveLoad CreateProcessFasecoldaMassiveLoad(CompanyProcessFasecolda companyProcessFasecolda)
        {
            COMMEN.CiaAsynchronousProcessFasecoldaMassiveLoad fasecoldaMassiveLoad = new COMMEN.CiaAsynchronousProcessFasecoldaMassiveLoad
            {
                Pendings = companyProcessFasecolda.Pendings,
                Processeds = companyProcessFasecolda.TotalRowsProcesseds,
                WithErrorsProcesseds = companyProcessFasecolda.WithErrorsProcesseds,
                TotalRowsProcesseds = companyProcessFasecolda.TotalRowsProcesseds,
                TotalRowsLoading = companyProcessFasecolda.TotalRowsLoaded,
                TotalRows = companyProcessFasecolda.TotalRows,
                FileName = companyProcessFasecolda.File.Name,
                TypeFile = (int)companyProcessFasecolda.TypeFile
            };

            if (companyProcessFasecolda.ProcessId > 0)
            {
                fasecoldaMassiveLoad.ProcessId = companyProcessFasecolda.ProcessId;
            }

            return fasecoldaMassiveLoad;
        }

        internal static COMMEN.CiaAsynchronousProcessFasecoldaRow CreateProcessFasecoldaRow(CompanyMassiveVehicleFasecoldaRow vehicleFasecoldaRow)
        {
            return new COMMEN.CiaAsynchronousProcessFasecoldaRow
            {
                FasecoldaMassiveLoadId = vehicleFasecoldaRow.ProcessMassiveLoadId,
                ProcessId = vehicleFasecoldaRow.ProcessId,
                RowNumber = vehicleFasecoldaRow.RowNumber,
                HasError = vehicleFasecoldaRow.HasError,
                ErrorDescription = vehicleFasecoldaRow.Error_Description,
                SerializedRow = vehicleFasecoldaRow.SerializedRow
            };
        }

        internal static COMMEN.CiaAsynchronousProcessFasecoldaStatus CreateCiaAsynchronousProcessFasecoldaStatus(CompanyProcessFasecolda processFasecolda)
        {
            return new COMMEN.CiaAsynchronousProcessFasecoldaStatus
            {
                ProcessId = processFasecolda.ProcessId,
                ProcessFasecoldaStatusId = (int)processFasecolda.ProcessStatusType.StatusType,
                //ProcessTypeId = processFasecolda.StatusId.Value,
                BeginDate = processFasecolda.BeginDate,
                EndDate = processFasecolda.EndDate
            };
}
    }
}
