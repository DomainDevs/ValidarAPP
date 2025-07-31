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
    /// Contragarantias Asegurado
    /// </summary>
    public class InsuredGuaranteePrefixDAO
    {
        /// <summary>
        /// Consulta ramos asociados a una contragarantía
        /// </summary>
        /// <param name="individualId"> Id Individuo</param>
        /// <param name="guaranteeId"> Id de la contragarantía</param>
        /// <returns> Ramos asociados a una contragarantía </returns>
        public List<Models.InsuredGuaranteePrefix> GetInsuredGuaranteePrefix(int individualId, int guaranteeId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePerson.Entities.InsuredGuaranteePrefix.Properties.IndividualId, typeof(UniquePerson.Entities.InsuredGuaranteePrefix).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(UniquePerson.Entities.InsuredGuaranteePrefix.Properties.GuaranteeId, typeof(UniquePerson.Entities.InsuredGuaranteePrefix).Name);
            filter.Equal();
            filter.Constant(guaranteeId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePerson.Entities.InsuredGuaranteePrefix), filter.GetPredicate()));

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetInsuredGuaranteePrefix");
            return ModelAssembler.CreateInsuredGuaranteePrefixes(businessCollection);
        }

        /// <summary>
        /// Saves the guarantee prefix.
        /// </summary>
        /// <param name="listPrefix">The list prefix.</param>
        /// <param name="guaranteeId">The guarantee identifier.</param>
        public void SaveGuaranteePrefix(List<Models.InsuredGuaranteePrefix> listPrefix, int guaranteeId)
        {
            if (listPrefix != null && listPrefix.Count > 0)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(UniquePerson.Entities.InsuredGuaranteePrefix.Properties.IndividualId, typeof(UniquePerson.Entities.InsuredGuarantee.Properties).Name);
                filter.Equal();
                filter.Constant(listPrefix.FirstOrDefault().IndividualId);
                filter.And();
                filter.Property(UniquePerson.Entities.InsuredGuaranteePrefix.Properties.GuaranteeId, typeof(UniquePerson.Entities.InsuredGuarantee.Properties).Name);
                filter.Equal();
                filter.Constant(guaranteeId);
                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePerson.Entities.InsuredGuaranteePrefix), filter.GetPredicate()));
                FacadeDAO.deleteCollection(businessCollection);
                foreach (Models.InsuredGuaranteePrefix prefix in listPrefix)
                {
                    prefix.GuaranteeId = guaranteeId;
                    UniquePerson.Entities.InsuredGuaranteePrefix insuredGuaranteePrefixEntity = EntityAssembler.CreateInsuredGuaranteePrefix(prefix);
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(insuredGuaranteePrefixEntity);
                }
            }
        }
    }
}

