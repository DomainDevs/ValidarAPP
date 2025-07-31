using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    /// <summary>
    /// Individuo
    /// </summary>
    public class IndividualDAO
    {
        /// <summary>
        /// Gets the individual by individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public Models.Individual GetIndividualByIndividualId(int individualId)
        {

            PrimaryKey key = Individual.CreatePrimaryKey(individualId);
            Individual individulaEntity = new Individual();
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                individulaEntity = (Individual)daf.GetObjectByPrimaryKey(key);
            }
            return ModelAssembler.CreateIndividual(individulaEntity);

        }

        /// <summary>
        /// Updates the individual by individual identifier.
        /// </summary>
        /// <param name="individual">The individual.</param>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public Models.Individual UpdateIndividualByIndividualId(Models.Individual individual, int individualId)
        {
            ObjectCriteriaBuilder filterDocument = new ObjectCriteriaBuilder();
            filterDocument.Property(Individual.Properties.IndividualId, typeof(Individual).Name);
            filterDocument.Equal();
            filterDocument.Constant(individualId);

            Individual individualEntity = (Individual)DataFacadeManager.Instance.GetDataFacade().List(typeof(Individual), filterDocument.GetPredicate()).FirstOrDefault();
            //todo ricardo
            //individualEntity.OwnerRoleCode = individual.OwnerRoleCode;
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(individualEntity);
            return ModelAssembler.CreateIndividual(individualEntity);
        }

        /// <summary>
        /// Gets the individual by filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public Individual GetIndividualByFilter(ObjectCriteriaBuilder filter)
        {

            Individual individual = (Individual)DataFacadeManager.Instance.GetDataFacade().List(typeof(Individual), filter.GetPredicate()).FirstOrDefault();
            return individual;
        }

        /// <summary>
        /// Obtiene el Individual segun el identificador
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public static Individual Find(int individualId)
        {
            PrimaryKey key = Individual.CreatePrimaryKey(individualId);
            Individual individual = (Individual)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            return individual;
        }
    }
}
