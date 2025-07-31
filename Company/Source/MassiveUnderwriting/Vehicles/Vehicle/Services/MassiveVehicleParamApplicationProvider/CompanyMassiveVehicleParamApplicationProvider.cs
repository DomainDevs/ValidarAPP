using System.Collections.Generic;
using Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService;
using Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.DTOs;
using Sistran.Company.Application.MassiveVehicleParamApplicationServiceProvider.Assembler;

namespace Sistran.Company.Application.MassiveVehicleParamApplicationServiceProvider
{
    public class CompanyMassiveVehicleParamApplicationProvider : ICompanyMassiveVehicleParamApplicationService
    {
        public string GetErrorExcelProcessVehicleFasecolda(int loadProcessId)
        {
            return DelegateService.vehicleParamBusinessService.GetErrorExcelProcessVehicleFasecolda(loadProcessId);
        }

        public List<ProcessFasecoldaMassiveLoadDTO> GetProcessMassiveVehiclefasecolda(int loadProcessId)
        {
            return DTOAssembler.CreateProcessMasives(DelegateService.vehicleParamBusinessService.GetProcessMassiveVehiclefasecolda(loadProcessId));
        }

        public ProcessFasecoldaDTO GenerateProccessMassiveVehicleFasecolda(int processId)
        {
            return DTOAssembler.CreateProcessFasecolda(DelegateService.vehicleParamBusinessService.GenerateProcessMassiveVehicleFasecolda(processId));
        }

        public ProcessFasecoldaDTO GenerateLoadMassiveVehicleFasecolda(ProcessFasecoldaDTO processFasecoldaDTO)
        {
            return DTOAssembler.CreateProcessFasecolda(DelegateService.vehicleParamBusinessService.GenerateLoadMassiveVehicleFasecolda(ModelAssembler.CreateProcessFasecolda(processFasecoldaDTO)));
        }
    }
}
