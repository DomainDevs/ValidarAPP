using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    /// <summary>
    /// Tipo Individuo
    /// </summary>
    public class PersonIndividualTypeDAO
    {
        /// <summary>
        /// Creates the type of the person individual.
        /// </summary>
        /// <param name="personIndividualType">Type of the person individual.</param>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public Models.PersonIndividualType CreatePersonIndividualType(Models.PersonIndividualType personIndividualType, int individualId)
        {
            personIndividualType.IndividualId = individualId;
            PersonIndividualType personIndividualTypeEntity = EntityAssembler.CreatePersonIndividualType(personIndividualType);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(personIndividualTypeEntity);
            return ModelAssembler.CreatePersonIndividualType(personIndividualTypeEntity);
        }

        /// <summary>
        /// Gets the person individual type individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public Models.PersonIndividualType GetPersonIndividualTypeIndividualId(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PersonIndividualType.Properties.IndividualId, typeof(PersonIndividualType).Name);
            filter.Equal();
            filter.Constant(individualId);
            BusinessCollection businessCollection = new BusinessCollection();
            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(PersonIndividualType), filter.GetPredicate()));
            return ModelAssembler.CreatePersonIndividualTypes(businessCollection).FirstOrDefault();

        }

        /// <summary>
        /// Updates the type of the person individual.
        /// </summary>
        /// <param name="personIndividualType">Type of the person individual.</param>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public Models.PersonIndividualType UpdatePersonIndividualType(Models.PersonIndividualType personIndividualType, int individualId)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(PersonIndividualType.Properties.IndividualId, typeof(PersonIndividualType).Name);
            filter.Equal();
            filter.Constant(individualId);
            PersonIndividualType personIndividualTypeEntity = null;
            personIndividualTypeEntity = (PersonIndividualType)DataFacadeManager.Instance.GetDataFacade().List(typeof(PersonIndividualType), filter.GetPredicate()).FirstOrDefault();
            if (personIndividualTypeEntity == null)
            {
                return CreatePersonIndividualType(personIndividualType, individualId);

            }
            else
            {
                personIndividualTypeEntity.PersonTypeCode = personIndividualType.PersonTypeCode;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(personIndividualTypeEntity);
                return ModelAssembler.CreatePersonIndividualType(personIndividualTypeEntity);
            }

        }
    }
}
