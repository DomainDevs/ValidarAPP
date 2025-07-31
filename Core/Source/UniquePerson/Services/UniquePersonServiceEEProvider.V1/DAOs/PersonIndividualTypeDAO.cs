using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
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
            //PersonIndividualType personIndividualTypeEntity = EntityAssembler.CreatePersonIndividualType(personIndividualType);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(null);
            return ModelAssembler.CreatePersonIndividualType(null);
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
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(PersonIndividualType), filter.GetPredicate()));
            }

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
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                personIndividualTypeEntity = (PersonIndividualType)daf.List(typeof(PersonIndividualType), filter.GetPredicate()).FirstOrDefault();
            }
            if (personIndividualTypeEntity == null)
            {
                return CreatePersonIndividualType(personIndividualType, individualId);

            }
            else
            {
                personIndividualTypeEntity.PersonTypeCode = personIndividualType.PersonTypeCode;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    daf.UpdateObject(personIndividualTypeEntity);
                }
                return ModelAssembler.CreatePersonIndividualType(personIndividualTypeEntity);
            }

        }
    }
}
