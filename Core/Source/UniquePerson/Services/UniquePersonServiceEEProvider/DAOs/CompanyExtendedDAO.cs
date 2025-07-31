using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    /// <summary>
    /// Compañia
    /// </summary>
    public class CompanyExtendedDAO
    {
        /// <summary>
        /// Creates the co company.
        /// </summary>
        /// <param name="coCompany">The co company.</param>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public Models.CompanyExtended CreateCoCompany(Models.CompanyExtended coCompany, int individualId)
        {
            CoCompany coCompanyEntity = EntityAssembler.CreateCoCompany(coCompany, individualId);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(coCompanyEntity);
            return ModelAssembler.CreateCoCompany(coCompanyEntity);
        }

        /// <summary>
        /// Updates the co company.
        /// </summary>
        /// <param name="coCompany">The co company.</param>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public Models.CompanyExtended UpdateCoCompany(Models.CompanyExtended coCompany, int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CoCompany.Properties.IndividualId, typeof(CoCompany).Name);
            filter.Equal();
            filter.Constant(individualId);


            CoCompany coCompanyEntity = (CoCompany)DataFacadeManager.Instance.GetDataFacade().List(typeof(CoCompany), filter.GetPredicate()).FirstOrDefault();
            if (coCompanyEntity != null)
            {
                coCompanyEntity.VerifyDigit = coCompany.VerifyDigit;
                coCompanyEntity.AssociationTypeCode = coCompany.AssociationType.Id;
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(coCompanyEntity);
                return ModelAssembler.CreateCoCompany(coCompanyEntity);
            }

            return CreateCoCompany(coCompany, individualId);

        }

        /// <summary>
        /// Gets the co company by individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public Models.CompanyExtended GetCoCompanyByIndividualId(int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(CoCompany.Properties.IndividualId, typeof(CoCompany).Name);
            filter.Equal();
            filter.Constant(individualId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(CoCompany), filter.GetPredicate()));
            return ModelAssembler.CreateCoCompanies(businessCollection).FirstOrDefault();
        }

    }
}
