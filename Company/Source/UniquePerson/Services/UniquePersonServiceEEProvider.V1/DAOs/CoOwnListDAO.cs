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

    public class CoOwnListDAO
    {
        public List<Models.CompanyCoOwnList> GetListOwnByDocumentNumberFullName(string documentNumber, string fullName)
        {
            List<Models.CompanyCoOwnList> coOwnLists = new List<Models.CompanyCoOwnList>();

            if (string.IsNullOrEmpty(documentNumber) || string.IsNullOrEmpty(fullName))
            { return coOwnLists; }

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.OpenParenthesis();
            filter.Property(UniquePerson.Entities.CoOwnList.Properties.IdentificationNro, typeof(UniquePerson.Entities.CoOwnList).Name).Like().Constant($"%{documentNumber}%");
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
                filter.Property(UniquePerson.Entities.CoOwnList.Properties.Descripction, typeof(UniquePerson.Entities.CoOwnList).Name).Like().Constant($"%{item}%");
                first = false;
            }
            filter.CloseParenthesis();

            IEnumerable<UniquePerson.Entities.CoOwnList> entityCoOnuLists = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePerson.Entities.CoOwnList), filter.GetPredicate())).Cast<UniquePerson.Entities.CoOwnList>();
            coOwnLists.AddRange(ModelAssembler.CreateCoOwnList(entityCoOnuLists));

            return coOwnLists;
        }
    }
}
