using System.ServiceModel;

namespace Sistran.Company.Application.QueueBrokerService
{
    [ServiceContract]
    public interface IQueueBrokerService
    {
        #region FASECOLDA
        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessCollection"></param>
        [OperationContract]
        void CreateFasecoldaCode(string businessCollection);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessCollection"></param>
        [OperationContract]
        void CreateFasecoldaValue(string businessCollection);
        #endregion

        #region
        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessCollection"></param>
        [OperationContract]
        void CreateListRisk(string businessCollection);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessCollection"></param>
        [OperationContract]
        void CreateListRiskOfac(string businessCollection);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessCollection"></param>
        [OperationContract]
        void RecordListRisk(string businessCollection);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="businessCollection"></param>
        [OperationContract]
        void RecordListRiskOfac(string businessCollection);
        #endregion
    }
}