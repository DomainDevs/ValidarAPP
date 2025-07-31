using FuzzySharp.Extractor;
using ListRiskMatchingProcess.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;

namespace ListRiskMatchingProcess.Helper
{
    public class ValidateListRiskPersonHelper
    {
        private readonly ConcurrentBag<RiskListMatch> riskListMatches;
        private readonly IEnumerable<UPEN.ViewListRiskPerson> entityViewListRisk;
        private readonly string documentNumber;
        private readonly string fullName;
        private int? riskListType;
        private Dictionary<string, int> umbrals;

        public ValidateListRiskPersonHelper(IEnumerable<UPEN.ViewListRiskPerson> entityViewListRisk, string documentNumber, string fullName, int? riskListType, Dictionary<string, int> umbrals)
        {
            this.riskListMatches = new ConcurrentBag<RiskListMatch>();
            this.entityViewListRisk = entityViewListRisk;
            this.documentNumber = documentNumber;
            this.fullName = fullName;
            this.riskListType = riskListType;
            this.umbrals = umbrals;
        }

        public IEnumerable<RiskListMatch> ExecuteFuzzyMatching()
        {
            IEnumerable<IGrouping<dynamic, UPEN.ViewListRiskPerson>> groupViewLists = this.entityViewListRisk.GroupBy(x => new { x.Description, x.RiskListType });

            CancellationTokenSource cts = new CancellationTokenSource();
            ParallelOptions options = new ParallelOptions()
            {
                CancellationToken = cts.Token,
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            try
            {
                Parallel.ForEach(groupViewLists, options, group =>
                {
                    int umbral = this.umbrals[group.Key.RiskListType];

                    IEnumerable<string> listNames = group.Select(x => x.FullName);
                    IEnumerable<string> listDocuments = group.Select(x => x.DocumentNumber);

                    Task<ExtractedResult<string>> taskSimilarName = Task.Run(() => FuzzySharp.Process.ExtractOne(fullName, listNames, s => s.ToLowerInvariant().Trim(), cutoff: umbral));
                    Task<ExtractedResult<string>> taskSimilarDocument = Task.Run(() => FuzzySharp.Process.ExtractOne(documentNumber, listDocuments, s => s.ToLowerInvariant().Trim(), cutoff: umbral));

                    Task.WaitAll(taskSimilarName, taskSimilarDocument);

                    ExtractedResult<string> similarName = taskSimilarName.Result;
                    ExtractedResult<string> similarDocument = taskSimilarDocument.Result;

                    RiskListMatch riskListMatch = new RiskListMatch();
                    if (similarName != null)
                    {
                        riskListMatch.listType = group.First().Description;
                        riskListMatch.listVersion = group.First().ProcessId;
                        riskListMatch.registrationDate = group.First().RegistrationDate;
                        riskListMatch.jModel = similarName.Value;
                        riskListMatch.percentage = similarName.Score;

                        riskListMatches.Add(riskListMatch);
                        cts?.Cancel();
                    }
                    else if (similarDocument != null)
                    {
                        riskListMatch.listType = group.First().Description;
                        riskListMatch.listVersion = group.First().ProcessId;
                        riskListMatch.registrationDate = group.First().RegistrationDate;
                        riskListMatch.jModel = similarDocument.Value;
                        riskListMatch.percentage = similarDocument.Score;

                        riskListMatches.Add(riskListMatch);
                        cts?.Cancel();
                    }
                });
            }
            catch (OperationCanceledException e)
            {
                //this._logger.LogInformation(e.Message);
            }
            finally
            {
                cts.Dispose();
            }

            return this.riskListMatches;
        }
    }
}
