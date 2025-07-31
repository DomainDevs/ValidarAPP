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
    public class AffectedDAO
    {
        public List<Affected> GetAffectedsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            List<Affected> affecteds = new List<Affected>();

            int maxRows = 50;

            affecteds.AddRange(GetPersonsByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));
            affecteds.AddRange(GetCompaniesByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));

            return affecteds;
        }

        public List<Affected> GetPersonsByDescriptionInsuredSearchTypeMaxRows(string description, InsuredSearchType insuredSearchType, int maxRows)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Int64 identificationNumber = 0;
            Int64.TryParse(description, out identificationNumber);

            if (identificationNumber > 0)
            {
                switch (insuredSearchType)
                {
                    case InsuredSearchType.IndividualId:
                        filter.Property(UPEN.Person.Properties.IndividualId, "p");
                        filter.Equal();
                        filter.Constant(identificationNumber);
                        break;
                    case InsuredSearchType.DocumentNumber:
                        filter.Property(UPEN.Person.Properties.IdCardNo, "p");
                        filter.Equal();
                        filter.Constant(identificationNumber.ToString());
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
                    filter.Property(UPEN.Person.Properties.MotherLastName, "p");
                    filter.Like();
                    filter.Constant(fullName[1] + "%");

                    filter.Or();
                    filter.Property(UPEN.Person.Properties.Name, "p");
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

            return ModelAssembler.CreateAffectedsByPersons(DataFacadeManager.GetObjects(typeof(UPEN.Person), filter.GetPredicate(), null, 1, maxRows, false));
        }

        public List<Affected> GetCompaniesByDescriptionInsuredSearchTypeMaxRows(string description, InsuredSearchType insuredSearchType, int maxRows)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Int64 identificationNumber = 0;
            Int64.TryParse(description, out identificationNumber);

            if (identificationNumber > 0)
            {
                switch (insuredSearchType)
                {
                    case InsuredSearchType.IndividualId:
                        filter.Property(UPEN.Company.Properties.IndividualId, "c");
                        filter.Equal();
                        filter.Constant(identificationNumber);
                        break;
                    case InsuredSearchType.DocumentNumber:
                        filter.Property(UPEN.Company.Properties.TributaryIdNo, "c");
                        filter.Equal();
                        filter.Constant(identificationNumber.ToString());
                        break;
                }
            }
            else
            {
                filter.Property(UPEN.Company.Properties.TradeName, "c");
                filter.Like();
                filter.Constant(description.Trim() + "%");
            }

            return ModelAssembler.CreateAffectedsByCompanies(DataFacadeManager.GetObjects(typeof(UPEN.Company), filter.GetPredicate(), null, 1, maxRows, false));
        }
    }
}
