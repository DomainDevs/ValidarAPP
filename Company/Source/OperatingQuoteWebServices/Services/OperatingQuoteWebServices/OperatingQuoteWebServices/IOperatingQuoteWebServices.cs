using OperatingQuoteWebServices.Models;
using Sistran.Co.Previsora.Application.FullServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace OperatingQuoteWebServices
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IOperatingQuoteWebServices" en el código y en el archivo de configuración a la vez.
    [ServiceContract]
    [XmlSerializerFormat]
    
    public interface IOperatingQuoteWebServices
    {

        [OperationContract]
        double GetExchangeRateDate(DateTime OperatingQuotaExchangeRateDate, int currencyCd);

        [OperationContract]
        List<OperatingQuotaIndividual> GetIndividualOperatingQuota(int identification_type, string identification_id);

        [OperationContract]
        bool GetStatusAplication(StatusAplicationDTO statusaplication);

        [OperationContract]
        List<TableMessage> ModifyOperatingQuota(List<OPERATING_QUOTA> OperatingQuota, List<TableMessage> ListTableMessage);

        [OperationContract]
        OperatingQuotaResponse RegisterOperativeQuota(WSOperatingQuota operatingQuota);
    }
}
