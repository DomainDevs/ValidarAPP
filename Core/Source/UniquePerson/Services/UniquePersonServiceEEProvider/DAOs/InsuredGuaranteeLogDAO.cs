using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    /// <summary>
    ///  bitacora del asegurado y garantia
    /// </summary>
    public class InsuredGuaranteeLogDAO
    {


        /// <summary>
        /// Crea bitacora del asegurado y garantia
        /// </summary>
        /// <param name="newBinacleGuarantee">Nueva bitacora garantia</param>
        /// <returns>Listado de Bitacora de asegurado y garantia</returns>
        public Models.InsuredGuaranteeLog SaveInsuredGuaranteLog(Models.InsuredGuaranteeLog insuredGuaranteLog, int guaranteeId)
        {
            try
            {
                if (insuredGuaranteLog != null)
                {
                    insuredGuaranteLog.GuaranteeId = guaranteeId;
                    UniquePerson.Entities.InsuredGuaranteeLog insuredGuaranteeLogEntity = EntityAssembler.CreateInsuredGuaranteeLog(insuredGuaranteLog);

                    PrimaryKey primaryKey = UniquePerson.Entities.InsuredGuaranteeLog.CreatePrimaryKey(insuredGuaranteeLogEntity.IndividualId, guaranteeId, insuredGuaranteeLogEntity.GuaranteeStatusCode, insuredGuaranteeLogEntity.UserId, insuredGuaranteeLogEntity.LogDate);
                    UniquePerson.Entities.InsuredGuaranteeLog entityInsuredGuaranteeLog = (UniquePerson.Entities.InsuredGuaranteeLog)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

                    if (entityInsuredGuaranteeLog == null)
                        DataFacadeManager.Instance.GetDataFacade().InsertObject(insuredGuaranteeLogEntity);

                    else
                        DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityInsuredGuaranteeLog);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return insuredGuaranteLog;
        }

        /// <summary>
        /// Obtener lista bitacora del asegurado y garantia
        /// </summary>
        /// <param name="individualId">individualId Asegurado</param>
        /// <param name="guaranteeId">id de garantia Asegurado</param>
        /// <returns>Listado de Bitacora de asegurado y garantia</returns>
        public List<Models.InsuredGuaranteeLog> GetInsuredGuaranteeLogs(int individualId, int guaranteeId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(InsuredGuaranteeLog.Properties.IndividualId, typeof(InsuredGuaranteeLog).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(InsuredGuaranteeLog.Properties.GuaranteeId, typeof(InsuredGuaranteeLog).Name);
            filter.Equal();
            filter.Constant(guaranteeId);

            BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List(typeof(InsuredGuaranteeLog), filter.GetPredicate());

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetInsuredGuaranteeLogs");
            return Assemblers.ModelAssembler.CreateInsuredGuaranteeLogs(businessCollection);//OrderByDescending(x => x.LogDate).ToList();
        }
    }
}

