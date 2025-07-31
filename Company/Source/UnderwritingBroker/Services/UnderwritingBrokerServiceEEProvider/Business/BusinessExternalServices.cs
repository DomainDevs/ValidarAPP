using Newtonsoft.Json.Linq;
using Sistran.Company.Application.PendingOperationEntityServiceEEProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Sistran.Company.Application.UnderwritingBrokerServiceEEProvider.Business
{
    public class BusinessExternalServices
    {
        public void ProcessResponseFromExperienceServiceHistoricPolicies(string businessCollection)
        {
            try
            {
                //DelegateService.massiveVehicleService.ProcessResponseFromExperienceServiceHistoricPolicies(businessCollection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ProcessResponseFromExperienceServiceHistoricSinister(string businessCollection)
        {
            try
            {
                //DelegateService.massiveVehicleService.ProcessResponseFromExperienceServiceHistoricSinister(businessCollection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ProcessResponseFromScoreService(string businessCollection)
        {
            try
            {
                if (businessCollection != null)
                {
                    try
                    {
                        JObject jObject = JObject.Parse(businessCollection);
                        var idCorrelaccion = jObject["HEADER"]["idCorrelacionConsumidor"].ToString();


                        string[] header = idCorrelaccion.Split('.');

                        int subCoveredRiskType = System.Convert.ToInt32(header[3]);

                        switch (subCoveredRiskType)
                        {
                            case 1: // Autos
                                //DelegateService.massiveVehicleService.ProcessResponseFromScoreService(businessCollection);
                                break;
                            case 3: // Hogar
                                //DelegateService.massivePropertyService.ProcessResponseFromScoreService(businessCollection);
                                break;
                        }

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ProcessResponseFromSimitService(string businessCollection)
        {
            try
            {
                //DelegateService.massiveVehicleService.ProcessResponseFromSimitService(businessCollection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
