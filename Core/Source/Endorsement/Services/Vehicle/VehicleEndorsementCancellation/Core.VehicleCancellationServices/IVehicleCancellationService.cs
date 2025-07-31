using Sistran.Core.Application.CancellationEndorsement;
using System.ServiceModel;

namespace Sistran.Core.Application.VehicleEndorsementCancellationService
{
    [ServiceContract]
    public interface IVehicleCancellationService : ICancellationEndorsement
    {
        [OperationContract]
        object GetVehicleEndorsementCancellation(int idVehicleEndorsementCancellation);


        /// <summary>
        /// Cancela una póliza a partir del inicio de vigencia
        /// </summary>
        /// <param name="documentNumber"> Número de documento </param>
        /// <param name="branchCode"> Sucursal </param>
        /// <param name="prefixCode"> Ramo comercial </param>
        /// <param name="conditionText"> Texto </param>
        /// <param name="endorsementReason">  </param>
        /// <param name="userId"></param>
        /// <param name="annotations"></param>
        /// <param name="isNominative"></param>
        void CancellationPolicy(int documentNumber, int branchCode, int prefixCode, string conditionText, int endorsementReason, int userId, string annotations, bool isNominative);


        /// <summary>
        /// Crea el temporal de cnacelación de una póliza
        /// </summary>
        /// <param name="policyId"> Identificador de la póliza </param>
        /// <param name="userId"> Identificador del usuario que está realizando la operación </param>
        /// <param name="conditions">Texto de las condiciones </param>
        /// <param name="endorsementReason">Razón del endoso</param>
        /// <param name="annotations">  </param>
        void CreateTemporalCancelEndorsement(int policyId, int userId, string conditions, int endorsementReason, string annotations);

        /// <summary>
        /// Guarda el endoso de cancelación de un temporal
        /// </summary>
        /// <param name="tempId"> Identificador del temporal </param>
        void SaveEndorsementCancel(int tempId);
    }
}
