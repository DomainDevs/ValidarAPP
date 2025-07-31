using System;
using Newtonsoft.Json;

namespace Sistran.Company.Application.QueueBrokerServiceEEProvider.Business
{
    public class QueueBrokerFasecoldaBusiness
    {
        public void CreateFasecoldaValue(string businessCollection)
        {
            if (businessCollection != null)
            {
                try
                {
                    //DelegateService.paraMassiveUnderwriting.CreateFasecoldaValue(businessCollection);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public void CreateFasecoldaCode(string businessCollection)
        {
            if (businessCollection != null)
            {
                try
                {
                    //DelegateService.paraMassiveUnderwriting.CreateFasecoldaCode(businessCollection);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void CreateListRisk(string businessCollection)
        {
            if (businessCollection != null)
            {
                try
                {
                    DelegateService._UniquePersonListRiskBusinessService.CreateListRiskTemporal(businessCollection);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public void CreateListRiskOfac(string businessCollection)
        {
            if (businessCollection != null)
            {
                try
                {
                    DelegateService._UniquePersonListRiskBusinessService.CreateListRiskOfacTemporal(businessCollection);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public void RecordListRisk(string businessCollection)
        {
            if (businessCollection != null)
            {
                try
                {
                    DelegateService._UniquePersonListRiskBusinessService.IssueRecordListRisk(businessCollection);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public void RecordListRiskOfac(string businessCollection)
        {
            if (businessCollection != null)
            {
                try
                {
                    DelegateService._UniquePersonListRiskBusinessService.IssueRecordListRiskOfac(businessCollection);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}