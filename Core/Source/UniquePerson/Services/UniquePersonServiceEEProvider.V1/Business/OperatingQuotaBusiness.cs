using Sistran.Core.Application.UniquePersonService.V1.Enums;
using MOUP= Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using System.Diagnostics;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class OperatingQuotaBusiness
    {
        /// <summary>
        /// Gets the operating quota by individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public List<Models.OperatingQuota> GetOperatingQuotaByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<Models.OperatingQuota> listOperatingQuota = new List<Models.OperatingQuota>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(OperatingQuota.Properties.IndividualId, typeof(OperatingQuota).Name);
            filter.Equal();
            filter.Constant(individualId);

            var business = DataFacadeManager.GetObjects(typeof(OperatingQuota), filter.GetPredicate());
            listOperatingQuota = ModelAssembler.CreateOperatingQuotas(business);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.GetOperatingQuotaByIndividualId");
            return listOperatingQuota;
        }

        /// <summary>
        /// Creates the operating quota.
        /// </summary>
        /// <param name="listOperatingQuota">The list operating quota.</param>
        /// <returns></returns>
        public List<Models.OperatingQuota> CreateOperatingQuota(List<Models.OperatingQuota> listOperatingQuota)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            if (listOperatingQuota != null && listOperatingQuota.Count > 0)
            {
                foreach (Models.OperatingQuota item in listOperatingQuota)
                {
                    if (item != null)
                    {
                        PrimaryKey primaryKey = OperatingQuota.CreatePrimaryKey(item.IndividualId, item.LineBusinessId, item.CurrencyId);
                        OperatingQuota entityOperatingQuota = (OperatingQuota)DataFacadeManager.GetObject(primaryKey);

                        if (entityOperatingQuota != null)
                        {
                            entityOperatingQuota.OperatingQuotaAmount = item.Amount;
                            entityOperatingQuota.CurrentTo = item.CurrentTo;
                            DataFacadeManager.Update(entityOperatingQuota);
                        }
                        else
                        {
                            OperatingQuota OperatingQuotaEntity = EntityAssembler.CreateOperatingQuota(item);
                            DataFacadeManager.Insert(OperatingQuotaEntity);
                        }
                    }
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.CreateOperatingQuota");
            return listOperatingQuota;
        }

        /// <summary>
        /// Updates the operating quota.
        /// </summary>
        /// <param name="OperatingQuota">The operating quota.</param>
        /// <returns></returns>
        public Models.OperatingQuota UpdateOperatingQuota(Models.OperatingQuota OperatingQuota)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            if (OperatingQuota != null)
            {
               OperatingQuota OperatingQuotaEntity = EntityAssembler.CreateOperatingQuota(OperatingQuota);

                if (OperatingQuotaEntity != null)
                {
                    OperatingQuotaEntity.OperatingQuotaAmount = OperatingQuota.Amount;
                    OperatingQuotaEntity.CurrentTo = OperatingQuota.CurrentTo;

                    DataFacadeManager.Update(OperatingQuotaEntity);
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.UpdateOperatingQuota");
            return OperatingQuota;
        }

        /// <summary>
        /// Deletes the operating quota.
        /// </summary>
        /// <param name="OperatingQuota">The operating quota.</param>
        /// <returns></returns>
        public bool DeleteOperatingQuota(Models.OperatingQuota OperatingQuota)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            PrimaryKey primaryKey = UniquePersonV1.Entities.OperatingQuota.CreatePrimaryKey(OperatingQuota.IndividualId, OperatingQuota.LineBusinessId, OperatingQuota.CurrencyId);
            DataFacadeManager.Delete(primaryKey);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.DeleteOperatingQuota");
            return true; 
        }

        public bool DeleteOperatingQuotaEvent(int individualId, int lineBusinessId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(OperatingQuotaEvent.Properties.IdentificationId, typeof(OperatingQuotaEvent).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(OperatingQuotaEvent.Properties.LineBusinessCode, typeof(OperatingQuotaEvent).Name);
            filter.Equal();
            filter.Constant(lineBusinessId);
            
            OperatingQuotaEvent operatingQuotaEvent = DataFacadeManager.GetObjects(typeof(OperatingQuotaEvent), filter.GetPredicate()).Cast<OperatingQuotaEvent>().FirstOrDefault();           
            PrimaryKey primaryKey = UniquePersonV1.Entities.OperatingQuotaEvent.CreatePrimaryKey(operatingQuotaEvent.OperatingQuotaEventCode);
            DataFacadeManager.Delete(primaryKey);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.Business.DeleteOperatingQuotaEvent");
            return true;
        }

    }
}