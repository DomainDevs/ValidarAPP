using Sistran.Company.Application.UniquePersonListRiskBusinessServiceProvider;
using Sistran.Core.Application.UniquePersonListRiskBusinessService.Model;
using Sistran.Core.Application.UniquePersonListRiskBusinessServiceProvider.Helper;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;

namespace Sistran.Core.Application.UniquePersonListRiskBusinessServiceProvider.DAO
{
    public class ListRiskLoadDAO
    {
        public IEnumerable<UPEN.ViewListRiskPerson> GetViewListRiskPerson(int? listRiskType)
        {
            Stopwatch timer = new Stopwatch();
            timer.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UPEN.ViewListRiskPerson.Properties.RiskListCode, typeof(UPEN.ViewListRiskPerson).Name).IsNotNull();

            if (listRiskType.HasValue)
            {
                filter.And().Property(UPEN.ViewListRiskPerson.Properties.RiskListCode, typeof(UPEN.ViewListRiskPerson).Name).Equal().Constant(listRiskType);
            }

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPEN.ViewListRiskPerson), filter.GetPredicate()));

            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            Debug.WriteLine($"EXECUTED GET_LISTS => {new { count = businessCollection.Count(), time = timeTaken.ToString("m\\:ss\\.ffff") }}");

            return businessCollection.Cast<UPEN.ViewListRiskPerson>();
        }

        public Dictionary<string, int> GetUmbral()
        {
            Dictionary<string, int> umbral = new Dictionary<string, int>();
            umbral.Add("ONU", DelegateServices.commonServiceCore.GetParameterByDescription("RISK_LIST_UMBRAL_ONU")?.NumberParameter ?? 60);
            umbral.Add("OFAC", DelegateServices.commonServiceCore.GetParameterByDescription("RISK_LIST_UMBRAL_OFAC")?.NumberParameter ?? 60);
            umbral.Add("PROPIA", DelegateServices.commonServiceCore.GetParameterByDescription("RISK_LIST_UMBRAL_OWN")?.NumberParameter ?? 60);

            return umbral;
        }

        public IDictionary<int, DateTime> GetListRiskLastVersion()
        {
            IDictionary<int, DateTime> valuePairs = new Dictionary<int, DateTime>();

            SelectQuery select = new SelectQuery();
            Function functionMax1 = new Function(FunctionType.Max);
            Function functionMax2 = new Function(FunctionType.Max);

            select.AddSelectValue(new SelectValue(new Column(UPEN.RiskListPerson.Properties.RiskListDescriptionCode, "rlp")));

            functionMax1.AddParameter(new Column(UPEN.RiskListPerson.Properties.ProcessId, "rlp"));
            select.AddSelectValue(new SelectValue(functionMax1, "Proceso"));

            functionMax2.AddParameter(new Column(UPEN.RiskListPerson.Properties.RegistrationDate, "rlp"));
            select.AddSelectValue(new SelectValue(functionMax2, "FechaMovimiento"));

            select.AddGroupValue(new Column(UPEN.RiskListPerson.Properties.RiskListDescriptionCode, "rlp"));

            select.Table = new ClassNameTable(typeof(UPEN.RiskListPerson), "rlp");

            IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select);
            while (reader.Read())
            {
                valuePairs.Add(int.Parse(reader["RiskListDescriptionCode"].ToString()), DateTime.Parse(reader["FechaMovimiento"].ToString()));
            }

            reader.Dispose();

            return valuePairs;
        }

        public List<RiskListMatch> ValidateListRiskPerson(string documentNumber, string fullName, int? riskListType)
        {
            List<RiskListMatch> riskListMatches = new List<RiskListMatch>();

            if (string.IsNullOrEmpty(documentNumber) && string.IsNullOrEmpty(fullName))
            { return riskListMatches; }

            if (string.IsNullOrEmpty(documentNumber))
            {
                documentNumber = string.Empty;
            }

            if (string.IsNullOrEmpty(fullName))
            {
                fullName = string.Empty;
            }

            IEnumerable<UPEN.ViewListRiskPerson> viewLists = CacheManager.entityViewListRisks.AsParallel()
                .Where(x => x.RiskListCode == (riskListType ?? x.RiskListCode));

            ValidateListRiskPersonHelper validateHelper = new ValidateListRiskPersonHelper(viewLists, documentNumber, fullName, riskListType, CacheManager.umbrals);
            return validateHelper.ExecuteFuzzyMatching().ToList();
        }
    }
}
