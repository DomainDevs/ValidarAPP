using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Entities.views;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF.Engine;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    public class EmployeeDAO
    {
        public Models.EmployeePerson GetEmployeePersonByIndividualId(int IndividualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            AgentExecutiveViewV1 view = new AgentExecutiveViewV1();
            ViewBuilder builder = new ViewBuilder("AgentExecutiveViewV1");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Person.Properties.IndividualId, typeof(Person).Name);
            filter.Equal();
            filter.Constant(IndividualId);
            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            List<Models.Person> persons = ModelAssembler.CreateExecutives(view.Persons);
            List<Models.EmployeePerson> employess = ModelAssembler.CreateEmployees(view.Employees);
            List<Models.EmployeePerson> result = new List<Models.EmployeePerson>();


            foreach (Models.EmployeePerson item in employess)
            {
                item.Description = persons.First(x => x.IndividualId == item.Id).Name;
                //todo ricardo
                //item.IdCardNo = persons.First(x => x.IndividualId == item.Id).PersonCode.ToString();
                item.MotherLastName = persons.First(x => x.IndividualId == item.Id).SecondSurName;
                result.Add(item);
            }
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetEmployeePersonByIndividualId");
            return result.First();

        }


        public List<Models.Base.BaseEmployeePerson> GetEmployeePersons()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            AgentExecutiveViewV1 view = new AgentExecutiveViewV1();
            ViewBuilder builder = new ViewBuilder("AgentExecutiveViewV1");

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            builder.Filter = filter.GetPredicate();
            DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

            List<Models.Person> persons = ModelAssembler.CreateExecutives(view.Persons);
            List<Models.EmployeePerson> employess = ModelAssembler.CreateEmployees(view.Employees);
           List<Models.Base.BaseEmployeePerson> result = new List<Models.Base.BaseEmployeePerson>();


            foreach (Models.Base.BaseEmployeePerson item in employess)
            {
                item.Id= persons.First(x => x.IndividualId == item.Id).IndividualId;
                item.Name = persons.First(x => x.IndividualId == item.Id).Name;
                item.MotherLastName= persons.First(x => x.IndividualId == item.Id).SecondSurName;
                item.IdCardNo = persons.First(x => x.IndividualId == item.Id).IdentificationDocument.ToString();
                
                //todo ricardo
                //item.IdCardNo= persons.First(x => x.IndividualId == item.Id).PersonCode.ToString();
                result.Add(item);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetEmployeePersonByIndividualId");
            return result;

        }

    }

}
