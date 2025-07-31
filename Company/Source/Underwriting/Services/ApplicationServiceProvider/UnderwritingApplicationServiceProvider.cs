using Sistran.Company.Application.ApplicationServiceProvider;
using Sistran.Company.Application.ApplicationServiceProvider.Assemblers;
using Sistran.Company.Application.UnderwritingApplicationService;
using Sistran.Company.Application.UnderwritingApplicationService.DTOs;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingApplicationServiceProvider
{
    public class UnderwritingApplicationServiceProvider : IUnderwritingApplicationService
    {
        public List<VehicleTypeDTO> ExecuteOperationsVehicleType(List<VehicleTypeDTO> vehicleTypesDTO)
        {
            try
            {
                return DTOAssembler.CreateVehicleTypes(DelegateService.underwritingService.ExecuteOperationsCompanyVehicleType(ModelAssembler.CreateCompanyVehicleTypes(vehicleTypesDTO)));
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error in ExecuteOperationsVehicleType", ex);
            }
        }

        public string GenerateFileToVehicleBody(VehicleTypeDTO vehicleTypeDTO, string fileName)
        {
            try
            {
                return DelegateService.underwritingService.GenerateFileToCompanyVehicleBody(ModelAssembler.CreateCompanyVehicleType(vehicleTypeDTO), fileName);
            }
            catch (Exception ex)
            {

                throw new Exception("Error in GenerateFileToVehicleBody", ex);
            }
        }

        public string GenerateFileToVehicleType(string fileName)
        {
            try
            {
                return DelegateService.underwritingService.GenerateFileToCompanyVehicleType(fileName);
            }
            catch (Exception ex)
            {

                throw new Exception("Error in GenerateFileToVehicleType", ex);
            }
        }

        public List<VehicleTypeDTO> GetVehicleTypes()
        {
            try
            {
                return DTOAssembler.CreateVehicleTypes(DelegateService.underwritingService.GetCompanyVehicleTypes());
            }
            catch (Exception ex)
            {

                throw new Exception("Error in GetVehicleTypes", ex);
            }
        }
    }
}
