//using Sistran.Company.Application.UnderwritingServices;
//using Sistran.Company.Application.UnderwritingServices.Models;
//using Sistran.Core.Application.UnderwritingServices.Enums;
//using Sistran.Core.Application.VehicleEndorsementModificationService;
//using System.ServiceModel;
//using VEM = Sistran.Company.Application.Vehicles.VehicleServices.Models;

//namespace Sistran.Company.Application.VehicleModificationService
//{
//    [ServiceContract]
//    public interface ITransportModificationServiceCia : ITransportModificationService
//    {

//        /// <summary>
//        /// Creacion Temporal, endoso Modificacion
//        /// </summary>
//        /// <param name="companyEndorsement">The company endorsement.</param>
//        /// <returns></returns>
//        [OperationContract]
//        CompanyPolicy CreateTemporal(CompanyEndorsement companyEndorsement, bool isMassive);

//        /// <summary>
//        /// Gets the data modification.
//        /// </summary>
//        /// <param name="risk">The risk.</param>
//        /// <param name="vehiclePolicy">The vehicle policy.</param>
//        /// <param name="coverageStatusType">Type of the coverage status.</param>
//        /// <returns></returns>
//        [OperationContract]
//        VEM.CompanyVehicle GetDataModification(VEM.CompanyVehicle risk, CoverageStatusType coverageStatusType);


//    }
//}
