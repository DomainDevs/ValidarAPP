using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    /// <summary>
    /// Estado Civil
    /// </summary>
    public class MaritalStatusDAO
    {
        /// <summary>
        /// Gets the marital state by identifier.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        public Models.MaritalStatus GetMaritalStateById(int Id)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(MaritalStatus.Properties.MaritalStatusCode, typeof(MaritalStatus).Name);
            filter.Equal();
            filter.Constant(Id);
            MaritalStatus maritalStatus = null;
            maritalStatus = (MaritalStatus)DataFacadeManager.Instance.GetDataFacade().List(typeof(MaritalStatus), filter.GetPredicate()).FirstOrDefault();

            return ModelAssembler.CreateMaritalState(maritalStatus);
        }


        /// <summary>
        /// Gets the marital status.
        /// </summary>
        /// <returns></returns>
        public List<Models.MaritalStatus> GetMaritalStatus()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(MaritalStatus)));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetMaritalStatus");
            return ModelAssembler.CreateMaritalStatus(businessCollection);
        }

        /// <summary>
        /// Finds the specified marital status code.
        /// </summary>
        /// <param name="maritalStatusCode">The marital status code.</param>
        /// <returns></returns>
        public static MaritalStatus Find(int maritalStatusCode)
        {
            PrimaryKey key = MaritalStatus.CreatePrimaryKey(maritalStatusCode);
            return (MaritalStatus)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        }
    }
}
