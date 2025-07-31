using Sistran.Co.Application.Data;

namespace Sistran.Core.Application.VehicleEndorsementCancellationService3GProvider.DAOs
{
    public class CancellationDAO
    {
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
        public void CancellationPolicy(int documentNumber, int branchCode, int prefixCode, string conditionText, int endorsementReason, int userId, string annotations, bool isNominative)
        {
            NameValue[] parameters = new NameValue[2];
            parameters[0] = new NameValue("@DOCUMENT_NUM", documentNumber);
            parameters[1] = new NameValue("@BRANCH_CD", branchCode);
            parameters[2] = new NameValue("@PREFIX_CD", prefixCode);
            parameters[3] = new NameValue("@CONDITION_TEXT", conditionText);
            parameters[4] = new NameValue("@ENDO_REASON_CD", endorsementReason);
            parameters[5] = new NameValue("@USER_ID", userId);
            parameters[6] = new NameValue("@ANNOTATIONS", annotations);

            if (isNominative)
            {
                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    pdb.ExecuteSPNonQuery("MSV.CANCELLATION_POLICY_NOMINATIVE", parameters);
                }
            }
            else
            {
                using (DynamicDataAccess pdb = new DynamicDataAccess())
                {
                    pdb.ExecuteSPNonQuery("MSV.CANCELLATION_POLICY", parameters);
                }
            }
        }

        /// <summary>
        /// Crea el temporal de cnacelación de una póliza
        /// </summary>
        /// <param name="policyId"> Identificador de la póliza </param>
        /// <param name="userId"> Identificador del usuario que está realizando la operación </param>
        /// <param name="conditions">Texto de las condiciones </param>
        /// <param name="endorsementReason">Razón del endoso</param>
        /// <param name="annotations">  </param>
        public void CreateTemporalCancelEndorsement(int policyId, int userId, string conditions, int endorsementReason, string annotations)
        {
            NameValue[] parameters = new NameValue[2];
            parameters[0] = new NameValue("@POLICY_ID", policyId);
            parameters[1] = new NameValue("@USER_ID", userId);
            parameters[2] = new NameValue("@CONDITION_TEXT", conditions);
            parameters[3] = new NameValue("@ENDO_REASON_CD", endorsementReason);
            parameters[4] = new NameValue("@ANNOTATIONS", annotations);
            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                pdb.ExecuteSPNonQuery("MSV.CREATE_TEMPORAL_CANCEL_ENDORSEMENT", parameters);
            }
        }

        /// <summary>
        /// Guarda el endoso de cancelación de un temporal
        /// </summary>
        /// <param name="tempId"> Identificador del temporal </param>
        public void SaveEndorsementCancel(int tempId)
        {
            NameValue[] parameters = new NameValue[2];
            parameters[0] = new NameValue("@TEMP_ID", tempId);

            using (DynamicDataAccess pdb = new DynamicDataAccess())
            {
                pdb.ExecuteSPNonQuery("MSV.SAVE_ENDORSEMENT_CANCEL", parameters);
            }
        }
    }
}
