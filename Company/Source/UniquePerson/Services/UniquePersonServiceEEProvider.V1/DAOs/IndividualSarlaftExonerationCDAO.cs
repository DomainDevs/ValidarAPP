using Sistran.Company.Application.UniquePerson.Entities;
using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Linq;

namespace Sistran.Company.Application.UniquePersonServices.V1.DAOs
{
    /// <summary>
    /// Sarlaft Exoneraciones
    /// </summary>
    public class IndividualSarlaftExonerationDAO
    {
        /// <summary>
        /// Obtener exoneracion asociada a un individuo
        /// </summary>
        /// <param name="individualId">Id del individuo</param>
        /// <returns></returns>
        public Models.CompanySarlaftExoneration GetSarlaftExonerationByIndividualId(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(IndividualSarlaftExoneration.Properties.IndividualId, typeof(IndividualSarlaftExoneration).Name);
            filter.Equal();
            filter.Constant(individualId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(IndividualSarlaftExoneration), filter.GetPredicate()));
            Models.CompanySarlaftExoneration sarlaftExoneration = ModelAssembler.CreateSarlaftExonerations(businessCollection).FirstOrDefault();

            return sarlaftExoneration;
        }



        /// <summary>
        /// Crear nuevo email
        /// </summary>
        /// <param name="email">Modelo email</param>
        /// <returns></returns>
        public Models.CompanySarlaftExoneration CreateSarlaftExoneration(Models.CompanySarlaftExoneration sarlaftExoneration, int individualId)
        {
            IndividualSarlaftExoneration sarlaftExonerationEntity = EntityAssembler.CreateSarlaftExoneration(sarlaftExoneration);
            sarlaftExonerationEntity.IndividualId = individualId;
            DataFacadeManager.Instance.GetDataFacade().InsertObject(sarlaftExonerationEntity);
            return ModelAssembler.CreateSarlaftExoneration(sarlaftExonerationEntity);

        }

        /// <summary>
        /// Actualizar sarlaftexoneration
        /// </summary>
        /// <param name="email">Modelo sarlaftexoneration</param>
        /// <returns></returns>
        public Models.CompanySarlaftExoneration UpdateSarlaftExoneration(Models.CompanySarlaftExoneration sarlaftExoneration, int individualId)
        {
            PrimaryKey key = IndividualSarlaftExoneration.CreatePrimaryKey(individualId);
            IndividualSarlaftExoneration sarlaftExonerationEntity = (IndividualSarlaftExoneration)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
            if (sarlaftExonerationEntity != null)
            {
                sarlaftExonerationEntity.ExonerationTypeCode = sarlaftExoneration.ExonerationType.Id;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(sarlaftExonerationEntity);
                return ModelAssembler.CreateSarlaftExoneration(sarlaftExonerationEntity);
            }
            else
            {
                return CreateSarlaftExoneration(sarlaftExoneration, individualId);

            }
        }
    }
}
