using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Sistran.Company.Application.UnderwritingApplicationService.DTOs;

namespace Sistran.Company.Application.UnderwritingApplicationService
{
    [ServiceContract]
    public interface IUnderwritingApplicationService
    {
        [OperationContract]
        List<VehicleTypeDTO> ExecuteOperationsVehicleType(List<VehicleTypeDTO> vehicleTypesDTO);

        [OperationContract]
        List<VehicleTypeDTO> GetVehicleTypes();

        [OperationContract]
        string GenerateFileToVehicleType(string fileName);

        [OperationContract]
        string GenerateFileToVehicleBody(VehicleTypeDTO vehicleTypeDTO, string fileName);


        #region CiaRatingZoneBranch

        //[OperationContract]
        //CiaRatingZoneBranchDTO CreateCiaRatingZoneBranch(CiaRatingZoneBranchDTO ciaRatingZoneBranch);

        //[OperationContract]
        //void DeleteCiaBranchRatingZone(int ratingZoneCode, int branchCode);

        //[OperationContract]
        //CiaRatingZoneBranchDTO GetRatingZoneBranch(int ratingZoneCode, int branchCode);

        //[OperationContract]
        //List<CiaRatingZoneBranchDTO> GetRatingZonesBranchs();

        //[OperationContract]
        //List<CompanyRatingZoneDTO> GetRatingZonesByPrefixIdAndBranchId(int prefixId, int branchId);


        //[OperationContract]
        //List<CompanyRatingZoneDTO> GetRatingZonesByPrefixId(int prefixId);

        #endregion
    }
}
