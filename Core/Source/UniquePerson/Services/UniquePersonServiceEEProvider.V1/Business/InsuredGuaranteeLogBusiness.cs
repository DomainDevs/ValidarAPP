using Sistran.Core.Framework.DAF;
using Sistran.Core.Application.UniquePersonV1.Entities;
using System.Collections.Generic;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class InsuredGuaranteeLogBusiness
    {

        internal Models.InsuredGuaranteeLog CreateInsuredGuaranteeLog(Models.InsuredGuaranteeLog InsuredGuaranteeLog)
        {
            InsuredGuaranteeLog entityInsuredGuaranteeLog = Assemblers.EntityAssembler.CreateInsuredGuaranteeLog(InsuredGuaranteeLog);
            DataFacadeManager.Insert(entityInsuredGuaranteeLog);
            return ModelAssembler.CreateInsuredGuaranteeLog(entityInsuredGuaranteeLog);
        }

        internal Models.InsuredGuaranteeLog UpdateInsuredGuaranteeLog(Models.InsuredGuaranteeLog InsuredGuaranteeLog)
        {
            InsuredGuaranteeLog entityInsuredGuaranteeLog = EntityAssembler.CreateInsuredGuaranteeLog(InsuredGuaranteeLog);
            DataFacadeManager.Update(entityInsuredGuaranteeLog);
            return ModelAssembler.CreateInsuredGuaranteeLog(entityInsuredGuaranteeLog);
        }

        internal void DeleteInsuredGuaranteeLog(Models.InsuredGuaranteeLog InsuredGuaranteeLogd)
        {
            PrimaryKey primaryKey = InsuredGuaranteeLog.CreatePrimaryKey(InsuredGuaranteeLogd.IndividualId, InsuredGuaranteeLogd.GuaranteeId, InsuredGuaranteeLogd.GuaranteeStatusCode, InsuredGuaranteeLogd.UserId, InsuredGuaranteeLogd.LogDate);
            DataFacadeManager.Delete(primaryKey);
        }

        internal Models.InsuredGuaranteeLog GetInsuredGuaranteeLogById(Models.InsuredGuaranteeLog InsuredGuaranteeLogd)
        {
            PrimaryKey primaryKey = InsuredGuaranteeLog.CreatePrimaryKey(InsuredGuaranteeLogd.IndividualId, InsuredGuaranteeLogd.GuaranteeId, InsuredGuaranteeLogd.GuaranteeStatusCode, InsuredGuaranteeLogd.UserId, InsuredGuaranteeLogd.LogDate);
            InsuredGuaranteeLog entityInsuredGuaranteeLog = (InsuredGuaranteeLog)DataFacadeManager.GetObject(primaryKey);
            return ModelAssembler.CreateInsuredGuaranteeLog(entityInsuredGuaranteeLog);
        }

        internal List<Models.InsuredGuaranteeLog> GetInsuredGuaranteeLogByIndividualIdByGuaranteeId(int individualId, int guaranteeId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.PropertyEquals(InsuredGuaranteeLog.Properties.IndividualId, typeof(InsuredGuaranteeLog).Name, individualId);
            filter.And();
            filter.PropertyEquals(InsuredGuaranteeLog.Properties.GuaranteeId, typeof(InsuredGuaranteeLog).Name, guaranteeId);

            BusinessCollection businessObjects = DataFacadeManager.GetObjects(typeof(InsuredGuaranteeLog), filter.GetPredicate());
            List<Models.InsuredGuaranteeLog> insuredGuaranteeLogs = new List<Models.InsuredGuaranteeLog>();
            if (businessObjects != null && businessObjects.Count > 0)
            {
                foreach (InsuredGuaranteeLog item in businessObjects)
                {
                    Models.InsuredGuaranteeLog insuredGuaranteeLog = ModelAssembler.CreateInsuredGuaranteeLog(item);
                    insuredGuaranteeLog.UserName = null;// DelegateService.uniqueUserServiceCore.GetUserById(insuredGuaranteeLog.UserId).Name;
                    insuredGuaranteeLogs.Add(insuredGuaranteeLog);
                }

                return insuredGuaranteeLogs;
            }
            else
            {
                return new List<Models.InsuredGuaranteeLog>();
            }
        }

    }

}
