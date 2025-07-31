// -----------------------------------------------------------------------
// <copyright file="CoOnuListDAO.cs" company="Sistran">
// Copyright (c). All rights reserved.
// </copyright>
// <author>Robinson Castro Londoño</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.DAOs
{
    using System.Collections.Generic;
    using System.Linq;
    using Assemblers;
    using Core.Application.Utilities.DataFacade;
    using Core.Framework.DAF;
    using Core.Framework.Queries;

    public class CoOnuListDAO
    {
        public List<Models.CompanyCoOnuList> GetListOnuByDocumentNumberFullName(string documentNumber, string fullName)
        {
            List<Models.CompanyCoOnuList> coOnuLists = new List<Models.CompanyCoOnuList>();

            if (string.IsNullOrEmpty(documentNumber) || string.IsNullOrEmpty(fullName))
            { return coOnuLists; }

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.OpenParenthesis();
            filter.Property(UniquePerson.Entities.CoOnuList.Properties.IdentificationNro, typeof(UniquePerson.Entities.CoOnuList).Name).Like().Constant($"%{documentNumber}%");
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
                filter.Property(UniquePerson.Entities.CoOnuList.Properties.Descripction, typeof(UniquePerson.Entities.CoOnuList).Name).Like().Constant($"%{item}%");
                first = false;
            }
            filter.CloseParenthesis();

            IEnumerable<UniquePerson.Entities.CoOnuList> entityCoOnuLists = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePerson.Entities.CoOnuList), filter.GetPredicate())).Cast<UniquePerson.Entities.CoOnuList>();
            coOnuLists.AddRange(ModelAssembler.CreateCoOnuList(entityCoOnuLists));

            return coOnuLists;
        }
    }
}
