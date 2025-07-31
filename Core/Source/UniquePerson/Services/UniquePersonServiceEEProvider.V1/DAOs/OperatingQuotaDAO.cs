using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    /// <summary>
    /// Cupo Operativo Consorcio
    /// </summary>
    public class OperatingQuotaDAO
    {
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
                        UniquePersonV1.Entities.OperatingQuota OperatingQuotaEntity = EntityAssembler.CreateOperatingQuota(item);
                        PrimaryKey primaryKey = UniquePersonV1.Entities.OperatingQuota.CreatePrimaryKey(item.IndividualId, item.LineBusinessId, item.CurrencyId);
                        UniquePersonV1.Entities.OperatingQuota entityOperatingQuota = (UniquePersonV1.Entities.OperatingQuota)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

                        if (entityOperatingQuota != null)
                        {
                            entityOperatingQuota.OperatingQuotaAmount = item.Amount;
                            entityOperatingQuota.CurrentTo = item.CurrentTo;
                            DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityOperatingQuota);
                        }
                        else
                        {
                            DataFacadeManager.Instance.GetDataFacade().InsertObject(OperatingQuotaEntity);
                        }
                    }
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.CreateOperatingQuota");
            return listOperatingQuota;
        }

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
            filter.Property(UniquePersonV1.Entities.OperatingQuota.Properties.IndividualId, typeof(UniquePersonV1.Entities.OperatingQuota).Name);
            filter.Equal();
            filter.Constant(individualId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.OperatingQuota), filter.GetPredicate()));
            listOperatingQuota = ModelAssembler.CreateOperatingQuotas(businessCollection);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetOperatingQuotaByIndividualId");
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
                UniquePersonV1.Entities.OperatingQuota OperatingQuotaEntity = EntityAssembler.CreateOperatingQuota(OperatingQuota);

                PrimaryKey primaryKey = UniquePersonV1.Entities.OperatingQuota.CreatePrimaryKey(OperatingQuota.IndividualId, OperatingQuota.LineBusinessId, OperatingQuota.CurrencyId);
                UniquePersonV1.Entities.OperatingQuota entityOperatingQuota = (UniquePersonV1.Entities.OperatingQuota)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
                if (entityOperatingQuota != null)
                {
                    OperatingQuotaEntity.OperatingQuotaAmount = OperatingQuota.Amount;
                    OperatingQuotaEntity.CurrentTo = OperatingQuota.CurrentTo;

                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(OperatingQuotaEntity);
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.UpdateOperatingQuota");
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
            UniquePersonV1.Entities.OperatingQuota entityOperatingQuota = (UniquePersonV1.Entities.OperatingQuota)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);
            if (entityOperatingQuota != null)
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(entityOperatingQuota);

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.DeleteOperatingQuota");
                return true;
            }
            else
            {

                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.DeleteOperatingQuota");
                return false;
            }
        }
    }
}
