using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class RecuperatorDAO
    {
        public List<Recuperator> GetRecuperatorsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            List<Recuperator> recuperators = new List<Recuperator>();
            int maxRows = 50;

            recuperators.AddRange(GetPersonsByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));
            recuperators.AddRange(GetCompaniesByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));

            return recuperators;
        }

        public List<Recuperator> GetPersonsByDescriptionInsuredSearchTypeMaxRows(string description, InsuredSearchType insuredSearchType, int maxRows)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Int64 identificationNumber = 0;
            Int64.TryParse(description, out identificationNumber);

            if (identificationNumber > 0)
            {
                switch (insuredSearchType)
                {
                    case InsuredSearchType.IndividualId:
                        filter.Property(UniquePerson.Entities.Person.Properties.IndividualId, "p");
                        filter.Equal();
                        filter.Constant(identificationNumber);
                        break;
                    case InsuredSearchType.DocumentNumber:
                        filter.Property(UniquePerson.Entities.Person.Properties.IdCardNo, "p");
                        filter.Like();
                        filter.Constant(description + '%');
                        break;
                }
            }
            else
            {
                string[] fullName = description.Trim().Split(' ');
                fullName = fullName.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                filter.Property(UniquePerson.Entities.Person.Properties.Surname, "p");
                filter.Like();
                filter.Constant(fullName[0] + "%");

                if (fullName.Length > 1)
                {
                    filter.And();
                    filter.OpenParenthesis();
                    filter.Property(UniquePerson.Entities.Person.Properties.MotherLastName, "p");
                    filter.Like();
                    filter.Constant(fullName[1] + "%");

                    filter.Or();
                    filter.Property(UniquePerson.Entities.Person.Properties.Name, "p");
                    filter.Like();
                    filter.Constant(fullName[1] + "%");
                    filter.CloseParenthesis();

                    if (fullName.Length > 2)
                    {
                        string name = "";
                        int cont = 0;
                        string space = "";
                        for (int i = 2; i < fullName.Length; i++)
                        {
                            if (cont > 0)
                            {
                                space = " ";
                            }
                            name += space + fullName[i];
                            cont++;
                        }

                        filter.And();
                        filter.Property(UniquePerson.Entities.Person.Properties.Name, "p");
                        filter.Like();
                        filter.Constant(name + "%");
                    }
                }
            }

            return ModelAssembler.CreateRecuperatorsByPersons(DataFacadeManager.GetObjects(typeof(UPEN.Person), filter.GetPredicate(), null, 1, maxRows, false));
        }


        public List<Recuperator> GetCompaniesByDescriptionInsuredSearchTypeMaxRows(string description, InsuredSearchType insuredSearchType, int maxRows)
        {
            List<Recuperator> Recuperator = new List<Recuperator>();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Int64 identificationNumber = 0;
            Int64.TryParse(description, out identificationNumber);

            if (identificationNumber > 0)
            {
                switch (insuredSearchType)
                {
                    case InsuredSearchType.IndividualId:
                        filter.Property(UniquePerson.Entities.Company.Properties.IndividualId, "c");
                        filter.Equal();
                        filter.Constant(identificationNumber);
                        break;
                    case InsuredSearchType.DocumentNumber:
                        filter.Property(UniquePerson.Entities.Company.Properties.TributaryIdNo, "c");
                        filter.Like();
                        filter.Constant(description + '%');
                        break;
                }
            }
            else
            {
                filter.Property(UniquePerson.Entities.Company.Properties.TradeName, "c");
                filter.Like();
                filter.Constant(description.Trim() + "%");
            }

            return ModelAssembler.CreateRecuperatorsByCompanies(DataFacadeManager.GetObjects(typeof(UPEN.Company), filter.GetPredicate(), null, 1, maxRows, false));
        }
    }
}
