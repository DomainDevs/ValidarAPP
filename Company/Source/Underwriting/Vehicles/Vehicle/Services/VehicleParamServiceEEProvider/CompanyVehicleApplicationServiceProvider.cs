
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.Vehicles.VehicleApplicationService.DTOs;
using Sistran.Company.Application.Vehicles.VehicleApplicationService.EEProvider.Assemblers;
using Sistran.Company.Application.Vehicles.VehicleApplicationService.EEProvider.Resources;
using Sistran.Company.Application.Vehicles.VehicleServices.Models;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.ModelServices.Enums;
using Sistran.Core.Application.ModelServices.Models.VehicleParam;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Application.Vehicles.Models;
using Sistran.Core.Framework.BAF;

namespace Sistran.Company.Application.Vehicles.VehicleApplicationService.EEProvider
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class CompanyVehicleApplicationServiceProvider : ICompanyVehicleApplicationService
    {
        public List<SelectDTO> GetVehiclesMake()
        {
            try
            {
                List<Make> ListMakeModel = DelegateService.vehicleService.GetMakes();
                var result = AplicationAssembler.CreateMakeDTOs(ListMakeModel);
                return result;
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetVehicleMake), ex);
            }
        }

        public List<SelectDTO> GetModelsByMake(int makeID)
        {
            try
            {
                List<Model> ListModelModel = DelegateService.vehicleService.GetModelsByMakeId(makeID);
                var result = AplicationAssembler.CreateModelDTOs(ListModelModel);
                return result;
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetModelByMake), ex);
            }
        }

        public List<SelectDTO> GetFuelsType()
        {
            try
            {
                List<Fuel> ListFuelModel = DelegateService.vehicleService.GetFuels();
                var result = AplicationAssembler.CreateFuelDTOs(ListFuelModel);
                return result;
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetFuelType), ex);
            }
        }

        public List<SelectDTO> GetBodies()
        {
            try
            {
                List<Body> ListBodiesModel = DelegateService.vehicleService.GetBodies();
                var result = AplicationAssembler.CreateBodiesDTOs(ListBodiesModel);
                return result;
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetBody), ex);
            }
        }

        public List<SelectDTO> GetVehicleNotInsuredCauses()
        {
            try
            {
                List<CompanyNotInsurableCause> ListCauseModel = DelegateService.vehicleService.GetNotInsurableCauses();
                var result = AplicationAssembler.CreateCauseDTOs(ListCauseModel);
                return result;
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetVehicleMake), ex);
            }
        }

        public List<SelectDTO> GetTypesVehicle()
        {
            try
            {
                List<Core.Application.Vehicles.Models.Type> ListTypesModel = DelegateService.vehicleService.GetTypes();
                var result = AplicationAssembler.CreateTypeDTOs(ListTypesModel);
                return result;
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetTypeVehicle), ex);
            }
        }

        public List<SelectDTO> GetCurrencies()
        {
            try
            {
                List<Currency> ListCurrenciesModel = DelegateService.commonService.GetCurrencies();
                var result = AplicationAssembler.CreateCurrenciesDTOs(ListCurrenciesModel);
                return result;
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetCurrency), ex);
            }
        }

        public List<SelectDTO> GetTransmissionsType()
        {
            try
            {
                List<TransmissionType> ListTransmissionTypes = DelegateService.vehicleService.GetVehicleTransmissionType();
                var result = AplicationAssembler.CreateTransmissionTypeDTOs(ListTransmissionTypes);
                return result;
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetTransmissionType), ex);
            }
        }

        public VehicleVersionDTO CreateVehicleVersion(VehicleVersionDTO vehicleVersionDTO)
        {
            try
            {

                return AplicationAssembler.CreateVehicleVersionDTO(DelegateService.vehicleService.CreateCompanyVersion(ModelAssembler.CreateVehicleVersonModel(vehicleVersionDTO)));
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorCreateVehicleVersion), ex);
            }
        }

        public VehicleVersionDTO UpdateVehicleVersion(VehicleVersionDTO vehicleVersionDTO)
        {
            try
            {
                return AplicationAssembler.CreateVehicleVersionDTO(DelegateService.vehicleService.UpdateCompanyVersion(ModelAssembler.CreateVehicleVersonModel(vehicleVersionDTO)));
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorUpdateVehicleVersion), ex);
            }
        }
        public List<SelectDTO> GetVersionByMakeIdModelId(int makeId, int modelId)
        {
            try
            {
                VersionsServiceModel versionServiceModelList = DelegateService.VehicleParamServices.GetVersionsByMakeIdModelId(makeId, modelId);
                List<SelectDTO> versionList = AplicationAssembler.CreateVersionsDTOs(versionServiceModelList);
                return versionList;
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetVehicleMake), ex);
            }
        }

        public FasecoldaDTO GetFasecoldaCodeByMakeIdModelIdVersionId(int makeId, int modelId, int versionId)
        {
            try
            {
                CompanyVehicle vehicle = DelegateService.vehicleService.GetVehicleByMakeIdModelIdVersionId(makeId, modelId, versionId);
                FasecoldaDTO Fasecolda = AplicationAssembler.CreateFasecoldaDTO(vehicle);
                return Fasecolda;
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetVehicleMake), ex);
            }
        }
        public List<VehicleVersionDTO> GetVehicleVersionsByDescription(string description)
        {
            try
            {

                return AplicationAssembler.CreateVehicleVersionDTOs(DelegateService.vehicleService.GetCompanyVersionsByDescription(description));
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSearchVehicleVersion), ex);
            }
        }

        public FasecoldaDTO GetVehicleByFasecoldaCode(string code, int year)
        {
            try
            {
                CompanyVehicle vehicle = DelegateService.vehicleService.GetVehicleByFasecoldaCode(code, year);
                FasecoldaDTO Fasecolda = AplicationAssembler.CreateFasecoldaDTO(vehicle);
                return Fasecolda;
            }
            catch (System.Exception ex)
            {
                throw new BusinessException(ExceptionManager.GetMessage(ex, "Error consultando fasecolda"), ex);
            }
        }

        public List<ValidationPlateDTO> GetValidationPlate()
        {
            try
            {
                List<ValidationPlateServiceModel> limitsRcServiceModel = DelegateService.vehicleService.GetValidationPlate();
                var result = AplicationAssembler.CreateValidationPlatesDTO(limitsRcServiceModel);
                return result;
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorGetVehicleMake), ex);
            }
        }

        public List<VehicleVersionDTO> GetAdvanzedSearchVehiclesVersion(int? makeCode, int? modelCode, string description)
        {
            try
            {
                return AplicationAssembler.CreateVehicleVersionDTOs(DelegateService.vehicleService.GetCompanyVersionsByMakeModelVersion(makeCode, modelCode, description));
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorSearchVehicleVersion), ex);
            }
        }

        public void DeleteVehicleVersion(int id, int makeId, int modelId)
        {
            try
            {
                DelegateService.vehicleService.DeleteCompanytVehicle(id, makeId, modelId);
            }
            catch (System.Exception ex)
            {

                throw new BusinessException(ExceptionManager.GetMessage(ex, Errors.ErrorDeletingVehicleVersion), ex);
            }
        }


    }
}
