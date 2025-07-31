// -----------------------------------------------------------------------
// <copyright file="PersonDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Gina Gomez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.UniquePersonParamServices.EEProvider.DAOs
{
    using System;
    using System.Collections.Generic;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.UniquePersonParamService.Models;
    using Sistran.Core.Framework.Queries;
    using entities = Sistran.Core.Application.UniquePerson.Entities;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.UniquePersonParamService.EEProviders.EEProvider.Assemblers;

    /// <summary>
    /// Contiene las propiedades de Persona
    /// </summary>
    public class PersonDAO
    {
        /// <summary>
        /// Busca datos de una persona
        /// </summary>
        /// <param name="individualId">IndividualId</param>
        /// <returns></returns>
        public Result<List<ParamPerson>, ErrorModel> GetPersonByIndividualId(List<int> individualId)
        {
            try
            {
                List<ParamPerson> personsModel = new List<ParamPerson>();
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(entities.Person.Properties.IndividualId, typeof(entities.Person).Name);
                filter.In();
                filter.ListValue();
                foreach (int id in individualId)
                {
                    filter.Constant(id);
                }
                filter.EndList();

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(entities.Person), filter.GetPredicate()));

                if (businessCollection.Count > 0)
                {
                    personsModel = ModelAssembler.CreatePersons(businessCollection);
                }
                return new ResultValue<List<ParamPerson>, ErrorModel>(personsModel);
            }
            catch (Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add("Error al obtener personas");
                return new ResultError<List<ParamPerson>, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
            }

        }
    }
}
