using System.Collections.Generic;
using Sistran.Company.Application.MassiveVehicleParamBusinessService.Model;
using Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.DTOs;

namespace Sistran.Company.Application.MassiveVehicleParamApplicationServiceProvider.Assembler
{
    public class DTOAssembler
    {
        public static List<ProcessFasecoldaMassiveLoadDTO> CreateProcessMasives(List<CompanyProcessFasecoldaMassiveLoad> companyProcessFasecoldaMassiveLoads)
        {
            List<ProcessFasecoldaMassiveLoadDTO> ProcessFasecoldaMassiveLoadDTOs = new List<ProcessFasecoldaMassiveLoadDTO>();
            foreach (CompanyProcessFasecoldaMassiveLoad fasecoldaMassiveLoad in companyProcessFasecoldaMassiveLoads)
            {
                ProcessFasecoldaMassiveLoadDTOs.Add(CreateProcessMasive(fasecoldaMassiveLoad));
            }

            return ProcessFasecoldaMassiveLoadDTOs;
        }

        public static ProcessFasecoldaMassiveLoadDTO CreateProcessMasive(CompanyProcessFasecoldaMassiveLoad processFasecoldaMassiveLoad)
        {
            if(processFasecoldaMassiveLoad == null)
            {
                return null;
            }

            return new ProcessFasecoldaMassiveLoadDTO
            {
                ProcessId = processFasecoldaMassiveLoad.ProcessId,
                TotalRows = processFasecoldaMassiveLoad.TotalRows,
                TotalRowsProcesseds = processFasecoldaMassiveLoad.TotalRowsProcesseds,
                Pendings = processFasecoldaMassiveLoad.Pendings,
                TotalRowsLoaded = processFasecoldaMassiveLoad.WithErrorsLoaded,
                FileName = processFasecoldaMassiveLoad.File?.Name,
                TypeFile = processFasecoldaMassiveLoad.TypeFile,
                Status = processFasecoldaMassiveLoad.StatusId,
                EnableProcessing = processFasecoldaMassiveLoad.EnableProcessing
            };
        }

        public static ProcessFasecoldaDTO CreateProcessFasecolda(CompanyProcessFasecolda companyProcessFasecolda)
        {
            if(companyProcessFasecolda == null)
            {
                return null;
            }

            return new ProcessFasecoldaDTO
            {
                ProcessId = companyProcessFasecolda.ProcessId,
                HasError = companyProcessFasecolda.HasError,
                ProcessStatus = companyProcessFasecolda.ProcessStatus,
                ProcessStatusType = new VehicleFasecoldaStatusTypeDTO
                {
                    StatusType = companyProcessFasecolda.ProcessStatusType.StatusType 
                }
            };
        }
    }
}
