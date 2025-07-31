using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.UniquePersonService.Entities.views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    public class EmployeeDAO
    {
        public Models.Base.BaseEmployeePerson GetEmployeePersonByIndividualId(int IndividualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            AgentExecutiveView view = new AgentExecutiveView();
            ViewBuilder builder = new ViewBuilder("AgentExecutiveView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Person.Properties.IndividualId, typeof(Person).Name);
            filter.Equal();
            filter.Constant(IndividualId);
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            List<Models.Person> persons = ModelAssembler.CreateExecutives(view.Persons);
            List<Models.Base.BaseEmployeePerson> employess = ModelAssembler.CreateEmployees(view.Employees);
            List<Models.Base.BaseEmployeePerson> result = new List<Models.Base.BaseEmployeePerson>();


            foreach (Models.Base.BaseEmployeePerson item in employess)
            {
                item.Description = persons.First(x => x.IndividualId == item.Id).Name;
                item.IdCardNo = persons.First(x => x.IndividualId == item.Id).PersonCode.ToString();
                item.MotherLastName = persons.First(x => x.IndividualId == item.Id).MotherLastName;
                result.Add(item);
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetEmployeePersonByIndividualId");
            return result.First();

        }


        public List<Models.Base.BaseEmployeePerson> GetEmployeePersons()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            AgentExecutiveView view = new AgentExecutiveView();
            ViewBuilder builder = new ViewBuilder("AgentExecutiveView");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            List<Models.Person> persons = ModelAssembler.CreateExecutives(view.Persons);
            List<Models.Base.BaseEmployeePerson> employess = ModelAssembler.CreateEmployees(view.Employees);
           List<Models.Base.BaseEmployeePerson> result = new List<Models.Base.BaseEmployeePerson>();


            foreach (Models.Base.BaseEmployeePerson item in employess)
            {
                item.Id= persons.First(x => x.IndividualId == item.Id).IndividualId;
                item.Name = persons.First(x => x.IndividualId == item.Id).Name;
                item.MotherLastName= persons.First(x => x.IndividualId == item.Id).MotherLastName;
                item.IdCardNo= persons.First(x => x.IndividualId == item.Id).PersonCode.ToString();
                result.Add(item);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetEmployeePersonByIndividualId");
            return result;

        }

    }

}
