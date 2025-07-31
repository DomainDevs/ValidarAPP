using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Integration.UndewritingIntegrationServices.DTOs.Person;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Data;
using System.Linq;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;
namespace Sistran.Core.Integration.UnderwritingServices.EEProvider.DAOs
{
    public class PersonDAO
    {
        public static string GetNameByIndividualId(int individualId)
        {
            String name = string.Empty;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UPEN.Individual.Properties.IndividualId, typeof(UPEN.Individual).Name);
            filter.Equal();
            filter.Constant(individualId);

            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(UPEN.Individual.Properties.IndividualTypeCode, typeof(UPEN.Individual).Name), UPEN.Individual.Properties.IndividualTypeCode));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.IdCardNo, typeof(UPEN.Person).Name), UPEN.Person.Properties.IdCardNo));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Company.Properties.TributaryIdNo, typeof(UPEN.Company).Name), UPEN.Company.Properties.TributaryIdNo));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.Surname, typeof(UPEN.Person).Name), UPEN.Person.Properties.Surname));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.MotherLastName ?? "", typeof(UPEN.Person).Name), UPEN.Person.Properties.MotherLastName));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.Name, typeof(UPEN.Person).Name), UPEN.Person.Properties.Name));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Company.Properties.TradeName, typeof(UPEN.Company).Name), UPEN.Company.Properties.TradeName));

            Join join = new Join(new ClassNameTable(typeof(UPEN.Individual), typeof(UPEN.Individual).Name), new ClassNameTable(typeof(UPEN.Company), typeof(UPEN.Company).Name), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.Individual.Properties.IndividualId, typeof(UPEN.Individual).Name)
                .Equal()
                .Property(UPEN.Company.Properties.IndividualId, typeof(UPEN.Company).Name)
                .GetPredicate());
            join = new Join(join, new ClassNameTable(typeof(UPEN.Person), typeof(UPEN.Person).Name), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.Person.Properties.IndividualId, typeof(UPEN.Person).Name)
                .Equal()
                .Property(UPEN.Individual.Properties.IndividualId, typeof(UPEN.Individual).Name)
                .GetPredicate());
            select.Table = join;
            select.Where = filter.GetPredicate();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                name = reader.SelectReader(r =>
                {
                    if (Convert.ToInt32(r[UPEN.Individual.Properties.IndividualTypeCode]) == (Int32)IndividualType.Person)
                    {
                        return Convert.ToString(r[UPEN.Person.Properties.Surname]) + " " + Convert.ToString(r[UPEN.Person.Properties.MotherLastName]) + " " + Convert.ToString(r[UPEN.Person.Properties.Name]);
                    }
                    else
                    {
                        return Convert.ToString(r[UPEN.Company.Properties.TradeName]);
                    }

                }).ToList().FirstOrDefault();
            }
            return name;
        }
        public static PersonDataDTO GetPersonByFilter(PersonRequestDTO personRequestDTO)
        {
            String name = string.Empty;
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UPEN.Person.Properties.IdCardNo, typeof(UPEN.Person).Name);
            filter.Equal();
            filter.Constant(personRequestDTO.DocumentNumber);
            filter.And();
            filter.Property(UPEN.Individual.Properties.IndividualTypeCode, typeof(UPEN.Individual).Name);
            filter.Equal();
            filter.Constant(personRequestDTO.DocumentTypeId);
            filter.Or();
            filter.Property(UPEN.Company.Properties.TributaryIdNo, typeof(UPEN.Company).Name);
            filter.Equal();
            filter.Constant(personRequestDTO.DocumentNumber);

            SelectQuery select = new SelectQuery();
            select.AddSelectValue(new SelectValue(new Column(UPEN.Individual.Properties.IndividualId, typeof(UPEN.Individual).Name), UPEN.Individual.Properties.IndividualId));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Individual.Properties.IndividualTypeCode, typeof(UPEN.Individual).Name), UPEN.Individual.Properties.IndividualTypeCode));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.Surname, typeof(UPEN.Person).Name), UPEN.Person.Properties.Surname));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.MotherLastName ?? "", typeof(UPEN.Person).Name), UPEN.Person.Properties.MotherLastName));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Person.Properties.Name, typeof(UPEN.Person).Name), UPEN.Person.Properties.Name));
            select.AddSelectValue(new SelectValue(new Column(UPEN.Company.Properties.TradeName, typeof(UPEN.Company).Name), UPEN.Company.Properties.TradeName));

            Join join = new Join(new ClassNameTable(typeof(UPEN.Individual), typeof(UPEN.Individual).Name), new ClassNameTable(typeof(UPEN.Company), typeof(UPEN.Company).Name), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.Individual.Properties.IndividualId, typeof(UPEN.Individual).Name)
                .Equal()
                .Property(UPEN.Company.Properties.IndividualId, typeof(UPEN.Company).Name)
                .GetPredicate());
            join = new Join(join, new ClassNameTable(typeof(UPEN.Person), typeof(UPEN.Person).Name), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UPEN.Person.Properties.IndividualId, typeof(UPEN.Person).Name)
                .Equal()
                .Property(UPEN.Individual.Properties.IndividualId, typeof(UPEN.Individual).Name)
                .GetPredicate());
            select.Table = join;
            select.Where = filter.GetPredicate();
            PersonDataDTO personDTO = null;
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(select))
            {
                personDTO = reader.SelectReader(r =>
                {
                    string FullName = "";
                    if (Convert.ToInt32(r[UPEN.Individual.Properties.IndividualTypeCode]) == (Int32)IndividualType.Person)
                    {
                        FullName = Convert.ToString(r[UPEN.Person.Properties.Surname]) + " " + Convert.ToString(r[UPEN.Person.Properties.MotherLastName]) + " " + Convert.ToString(r[UPEN.Person.Properties.Name]);
                    }
                    else
                    {
                        FullName = Convert.ToString(r[UPEN.Company.Properties.TradeName]);
                    }
                    return new PersonDataDTO
                    {
                        FullName = FullName,
                        IndividualId = Convert.ToInt32(r[UPEN.Individual.Properties.IndividualId])
                    };

                }).ToList().FirstOrDefault();
            }
            return personDTO;
        }

    }
}
