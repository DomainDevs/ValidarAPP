using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Entities.views;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.UniquePersonService.V1.Business
{
    public class EmployeePersonBusiness
    {
        public List<Models.EmployeePerson> GetEmployeePersons()
        {
            try
            {
                AgentExecutiveViewV1 view = new AgentExecutiveViewV1();
                ViewBuilder builder = new ViewBuilder("AgentExecutiveViewV1");

                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

                builder.Filter = filter.GetPredicate();
                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                List<Models.Person> persons = ModelAssembler.CreateExecutives(view.Persons);
                List<Models.EmployeePerson> employess = ModelAssembler.CreateEmployees(view.Employees);
                List<Models.EmployeePerson> result = new List<Models.EmployeePerson>();

                foreach (Models.EmployeePerson item in employess)
                {
                    item.Id = persons.First(x => x.IndividualId == item.Id).IndividualId;
                    item.Name = persons.First(x => x.IndividualId == item.Id).Name;
                    item.MotherLastName = persons.First(x => x.IndividualId == item.Id).SecondSurName;
                    item.IdCardNo = persons.First(x => x.IndividualId == item.Id).IdentificationDocument.Number.ToString();

                    result.Add(item);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, ex);
            }
        }
    }
}