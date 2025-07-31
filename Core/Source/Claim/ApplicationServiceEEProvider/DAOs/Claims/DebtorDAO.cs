using Sistran.Core.Application.ClaimServices.EEProvider.Assemblers;
using Sistran.Core.Application.ClaimServices.EEProvider.Models.Claim;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;
using CLMEN = Sistran.Core.Application.Claims.Entities;

namespace Sistran.Core.Application.ClaimServices.EEProvider.DAOs.Claims
{
    public class DebtorDAO
    {
        public List<Debtor> GetDebtorByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            List<Debtor> debtors = new List<Debtor>();
            int maxRows = 50;

            debtors.AddRange(GetPersonsByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));
            debtors.AddRange(GetCompaniesByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));            

            return debtors;
        }

        public List<Debtor> GetPersonsByDescriptionInsuredSearchTypeMaxRows(string description, InsuredSearchType insuredSearchType, int maxRows)
        {
            List<Debtor> buyers = new List<Debtor>();
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
                        filter.Constant(identificationNumber.ToString() + '%');
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

                filter.Or();
                filter.Property(UniquePerson.Entities.Person.Properties.IdCardNo, "p");
                filter.Like();
                filter.Constant(fullName[0].ToString() + "%");
            }

            return ModelAssembler.CreateDebtorByPersons(DataFacadeManager.GetObjects(typeof(UPEN.Person), filter.GetPredicate(), null, 1, maxRows, false));
        }

        public List<Debtor> GetCompaniesByDescriptionInsuredSearchTypeMaxRows(string description, InsuredSearchType insuredSearchType, int maxRows)
        {
            List<Debtor> buyers = new List<Debtor>();

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
                        filter.Constant(identificationNumber.ToString() + '%');
                        break;
                }
            }
            else
            {
                filter.Property(UniquePerson.Entities.Company.Properties.TradeName, "c");
                filter.Like();
                filter.Constant(description.Trim() + "%");
                filter.Or();
                filter.Property(UniquePerson.Entities.Company.Properties.TributaryIdNo, "c");
                filter.Like();
                filter.Constant(description.Trim() + "%");
            }

            return ModelAssembler.CreateDebtorByCompanies(DataFacadeManager.GetObjects(typeof(UPEN.Company), filter.GetPredicate(), null, 1, maxRows, false));
        }

        public List<Debtor> GetParticipantsByDescriptionInsuredSearchTypeMaxRows(string description, InsuredSearchType insuredSearchType, int maxRows)
        {
            List<Debtor> debtors = new List<Debtor>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Int64 identificationNumber = 0;
            Int64.TryParse(description, out identificationNumber);

            if (identificationNumber > 0)
            {
                switch (insuredSearchType)
                {
                    case InsuredSearchType.IndividualId:
                        filter.Property(CLMEN.Participant.Properties.ParticipantCode, "p");
                        filter.Equal();
                        filter.Constant(identificationNumber);
                        break;
                    case InsuredSearchType.DocumentNumber:
                        filter.Property(CLMEN.Participant.Properties.DocumentNumber, "p");
                        filter.Like();
                        filter.Constant(identificationNumber.ToString() + '%');
                        break;
                }
            }
            else
            {
                filter.Property(CLMEN.Participant.Properties.Fullname, "p");
                filter.Like();
                filter.Constant(description + "%");
            }

            return ModelAssembler.CreateDebtorsByParticipants(DataFacadeManager.GetObjects(typeof(CLMEN.Participant), filter.GetPredicate(), null, 1, 50, false));
        }
    }
}
