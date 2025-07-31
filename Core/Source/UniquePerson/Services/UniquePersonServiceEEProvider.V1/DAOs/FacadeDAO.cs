using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    /// <summary>
    /// 
    /// </summary>
    public static class FacadeDAO
    {
        /// <summary>
        /// Deletes the collection.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        public static void deleteCollection(BusinessCollection businessCollection)
        {
            foreach (var collection in businessCollection)
            {
                DataFacadeManager.Instance.GetDataFacade().DeleteObject(collection);
            }
        }

        /// <summary>
        /// Gets the parameter by identifier.
        /// </summary>
        /// <param name="parameterId">The parameter identifier.</param>
        /// <returns></returns>
        public static Parameter GetParameterById(int parameterId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Parameter.Properties.ParameterId, typeof(Parameter.Properties).Name);
            filter.Equal();
            filter.Constant(parameterId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Parameter), filter.GetPredicate()));
            return (Parameter)(businessCollection).FirstOrDefault();
        }
    }
}
