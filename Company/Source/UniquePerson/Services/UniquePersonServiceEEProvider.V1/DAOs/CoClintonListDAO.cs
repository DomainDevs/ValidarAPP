namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.DAOs
{
    using System.Collections.Generic;
    using System.Linq;
    using Assemblers;
    using Core.Application.Utilities.DataFacade;
    using Core.Framework.DAF;
    using Core.Framework.Queries;
    using UPCIAEN = UniquePerson.Entities;

    public class CoClintonListDAO
    {
        public List<Models.CompanyCoClintonList> GetListClintonByDocumentNumberFullName(string documentNumber, string fullName)
        {
            List<Models.CompanyCoClintonList> clintonLists = new List<Models.CompanyCoClintonList>();

            if (string.IsNullOrEmpty(documentNumber) || string.IsNullOrEmpty(fullName))
            { return clintonLists; }

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.OpenParenthesis();
            filter.Property(UPCIAEN.CoClintonList.Properties.IdentificationNro, typeof(UPCIAEN.CoClintonList).Name).Like().Constant($"%{documentNumber}%");
            filter.CloseParenthesis();
            filter.And();
            filter.OpenParenthesis();

            bool first = true;
            foreach (string item in fullName.Split(' '))
            {
                if (!first)
                {
                    filter.Or();
                }
                filter.Property(UPCIAEN.CoClintonList.Properties.Descripction, typeof(UPCIAEN.CoClintonList).Name).Like().Constant($"%{item}%");
                first = false;
            }
            filter.CloseParenthesis();

            IEnumerable<UPCIAEN.CoClintonList> entityCoClintonList = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPCIAEN.CoClintonList), filter.GetPredicate())).Cast<UPCIAEN.CoClintonList>();
            clintonLists.AddRange(ModelAssembler.CreateCoClintonList(entityCoClintonList));

            return clintonLists;
        }
    }
}
