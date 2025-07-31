// -----------------------------------------------------------------------
// <copyright file="IVehicleParamServiceWeb.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Juan Sebastián Cárdenas Leiva</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.VehicleParamService
{
    using System.Collections.Generic;

    using Sistran.Core.Application.ModelServices.Models.VehicleParam;
    using System.ServiceModel;
    using Sistran.Core.Application.ModelServices.Models.Param;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using Sistran.Core.Application.VehicleParamService.Models;
    using Sistran.Core.Application.ModelServices.Models;

    [ServiceContract]
    public interface IVehicleParamServiceWeb
    {
        [OperationContract]
        InfringementGroupsServiceModel GetInfringementGroup();

        [OperationContract]
        InfringementGroupsServiceModel GetInfringementGroupsByDescription(string description);

        [OperationContract]
        List<InfringementGroupServiceModel> ExecuteOperationsInfringementGroups(List<InfringementGroupServiceModel> infringementGroupsServiceModel);

        [OperationContract]
        ExcelFileServiceModel GenerateFileToInfringementGroup(List<InfringementGroupServiceModel> lstInfringementGroup, string fileName);

        [OperationContract]
        InfringementsServiceModel GetInfringement();

        [OperationContract]
        InfringementsServiceModel GetInfringementByDescription(string description, string code, int? group);

        [OperationContract]
        InfringementGroupsTypeServiceModel GetInfringementGroupType();

        [OperationContract]
        InfringementGroupsTypeServiceModel GetInfringementGroupTypeActive();

        [OperationContract]
        List<InfringementServiceModel> ExecuteOperationsInfringement(List<InfringementServiceModel> infrigementsServiceModel);

        [OperationContract]
        ExcelFileServiceModel GenerateFileToInfringement(List<InfringementServiceModel> lstInfringement, string fileName);

        [OperationContract]
        InfringementStatesServiceModel GetInfringementState();

        [OperationContract]
        List<InfringementStateServiceModel> ExecuteOperationsInfringementState(List<InfringementStateServiceModel> infrigementsServiceModel);

        [OperationContract]
        InfringementStatesServiceModel GetInfringementStateByDescription(string description);


        [OperationContract]
        ExcelFileServiceModel GenerateFileToInfringementState(List<InfringementStateServiceModel> lstInfringementState, string fileName);
        
        [OperationContract]
        MakesServiceModel GetMakes();

        [OperationContract]
        ModelsServiceModel GetModelsByMakeId(int makeId);

        [OperationContract]
        VersionsServiceModel GetVersionsByMakeIdModelId(int makeId, int modelId);

        [OperationContract]
        VersionVehicleFasecoldasServiceModel GetVersionVehicleFasecoldaByFasecoldaId(string fasecoldaId);

        [OperationContract]
        VersionVehicleFasecoldasServiceModel GetVersionVehicleFasecoldaByMakeIdByModelIdByVersionId(int? versionId, int? modelId, int? makeId, string fasecoldaMakeId,string fasecoldaModelId);

        [OperationContract]
        List<VersionVehicleFasecoldaServiceModel> ExecuteOperationsFasecolda(List<VersionVehicleFasecoldaServiceModel> versionVehicleFasecolda);

        [OperationContract]
        ExcelFileServiceModel GenerateFileToVehicleType(string fileName);

        [OperationContract]
        ExcelFileServiceModel GenerateFileToFasecolda(string fileName);        
	
        [OperationContract]
        FasecoldasServiceModel GetAllVersionVehicleFasecoldaByMakeIdByModelIdByVersionId(int? versionId, int? modelId, int? makeId, string fasecoldaMakeId, string fasecoldaModelId);

        

        [OperationContract]
        List<VehicleModelServiceModel> ExecuteOperationVehicleModel(List<VehicleModelServiceModel> listVehicleServiceModel);

        [OperationContract]
        VehicleModelsServiceModel GetInVehicleModelByDescription( string descriptionmodel , int makeId);

        [OperationContract]
        List<VehicelMakeServiceQueryModel> GetVehicelMake();

        [OperationContract]
        VehicleModelsServiceModel GetAdvanASearchVehicleModel(VehicleModelServiceModel paramVehicleModel);

        [OperationContract]
        ExcelFileServiceModel GenerateFileToVehicleModel(List<VehicleModelServiceModel> lstVehicleModeltSM, string fileName);

        [OperationContract]
        VehicleModelServiceModel DeleteVehicleModel(VehicleModelServiceModel vehicleModelServiceModel);
        [OperationContract]
        VehicleModelsServiceModel GetInVehicleModels();



        /// <summary>
        /// Obtiene el valor de los vehiculos 
        /// </summary>
        /// <param name="makeId">id de la marca</param>
        /// <param name="modelId">id del modelo</param>
        /// <param name="versionId">id de la version</param>
        /// <param name="year">año</param>
        /// <returns>listado de valor de vehiculos con id de caracteristicas</returns>
        [OperationContract]
        VehicleVersionYearsServiceModel GetVehicleVersionYearsSMByMakeIdModelIdVersionIdYear(int? makeId, int? modelId, int? versionId, int? year);

        /// <summary>
        /// Genera archivo excel de valor de vehiculo por año
        /// </summary>
        /// <param name="makeId">id de marca</param>
        /// <param name="modelId">id de modelo</param>
        /// <param name="versionId">id de version</param>
        /// <returns>ruta de excel a exportar</returns>
        [OperationContract]
        ExcelFileServiceModel GenerateFileToVehicleVersionYear(int makeId, int modelId, int versionId);

       


        #region VehicleVersion
        [OperationContract]
        VehicleVersionServiceModel ExecuteOperationVehicleVersion(VehicleVersionServiceModel VehicleVersionServiceModel);
        [OperationContract]
        VehicleVersionsServiceModel GetVehicleVersionByDescription(string description);
        [OperationContract]
        VehicleVersionServiceModel DeleteVehicleVersion(VehicleVersionServiceModel VehicleVersionServiceModel);
        [OperationContract]
        VehicleMakesServiceQueryModel GetVehicleMake();
        [OperationContract]
        VehicleModelsServiceQueryModel GetVehicleModelByMake(int MakeID);
        [OperationContract]
        VehicleFuelsServiceQueryModel GetVehicleFuel();
        [OperationContract]
        VehicleTypesServiceQueryModel GetVehicleType();
        [OperationContract]
        VehicleBodysServiceQueryModel GetVehicleBody();
        [OperationContract]
        VehicleTransmissionTypesServiceQueryModel GetVehicleTransmissionType();
        [OperationContract]
        CurrenciesServiceQueryModel GetCurreny();
        [OperationContract]
        VehicleModelsServiceQueryModel GetVehicleModel();
        [OperationContract]
        VehicleVersionsServiceModel GetAdvanzedSearchVehicleVersion(VehicleVersionServiceModel VehicleVersionServiceModel);
        //[OperationContract]
        //ExcelFileServiceModel GenerateFileToVehicleVersion(string fileName, int? makeCode, int? modelCode);
        [OperationContract]
        ExcelFileServiceModel GenerateFileToVehicleVersion(string fileName);
        #endregion
    }
}