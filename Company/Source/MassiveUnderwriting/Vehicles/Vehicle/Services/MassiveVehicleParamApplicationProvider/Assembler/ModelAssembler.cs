using System.Linq;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Company.Application.MassiveVehicleParamBusinessService.Model;
using Sistran.Company.Application.Vehicles.MassiveVehicleParamApplicationService.DTOs;

namespace Sistran.Company.Application.MassiveVehicleParamApplicationServiceProvider.Assembler
{
    public class ModelAssembler
    {
        public static CompanyProcessFasecolda CreateProcessFasecolda(ProcessFasecoldaDTO processFasecoldaDTO)
        {
            if(processFasecoldaDTO == null)
            {
                return null;
            }

            return new CompanyProcessFasecolda
            {
                File = new File
                {
                    Name = processFasecoldaDTO.FileName,
                },
                User = processFasecoldaDTO.User,
                BeginDate = processFasecoldaDTO.BeginDate,
                Active = processFasecoldaDTO.Active,
                ProcessStatus = processFasecoldaDTO.ProcessStatus,
                Description = processFasecoldaDTO.Description,
                ProcessId = processFasecoldaDTO.ProcessId,
                TypeFile = processFasecoldaDTO.ProcessMassiveLoad.FirstOrDefault().TypeFile
            };
        }
    }
}
