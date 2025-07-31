using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class InsuredDAO
    {
        public List<IssuanceInsured> GetPersonOrCompanyByByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType searchType, CustomerType? customerType)
        {
            List<IssuanceInsured> insureds = new List<IssuanceInsured>();
            int maxRows = 50;
            if (customerType.HasValue)
            {
                if (customerType.Value == CustomerType.Individual)
                {
                    insureds.AddRange(GetPersonsByDescriptionSearchTypeMaxRows(description, searchType, maxRows));
                    insureds.AddRange(GetCompaniesByDescriptionSearchTypeMaxRows(description, searchType, maxRows));
                }
            }
            return insureds;
        }

        public List<IssuanceInsured> GetInsuredsByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            List<IssuanceInsured> insureds = new List<IssuanceInsured>();
            int maxRows = 50;

            if (customerType.HasValue)
            {
                if (customerType.Value == CustomerType.Individual)
                {
                    insureds.AddRange(GetPersonsByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));
                    insureds.AddRange(GetCompaniesByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));
                }
                else
                {
                    insureds.AddRange(GetProspectsByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));
                }
            }
            else
            {
                insureds.AddRange(GetPersonsByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));
                insureds.AddRange(GetCompaniesByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));
                insureds.AddRange(GetProspectsByDescriptionInsuredSearchTypeMaxRows(description, insuredSearchType, maxRows));
            }

            if (insureds.Count == 1 && insureds[0].CustomerType == CustomerType.Individual)
            {
                CompanyNameDAO companyNameDAO = new CompanyNameDAO();
                insureds[0].CompanyName = companyNameDAO.GetNotificationAddressesByIndividualId(insureds[0].IndividualId, insureds[0].CustomerType).FirstOrDefault();

                PaymentMethodDAO paymentMethodDAO = new PaymentMethodDAO();
                insureds[0].PaymentMethod = paymentMethodDAO.GetPaymentMethodByIndividualId(insureds[0].IndividualId);
            }
            return insureds;
        }
        public List<IssuanceInsured> GetPersonsByDescriptionSearchTypeMaxRows(string description, InsuredSearchType insuredSearchType, int maxRows)
        {
            List<IssuanceInsured> insureds = new List<IssuanceInsured>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Int64 identificationNumber = 0;
            Int64.TryParse(description, out identificationNumber);
            if (identificationNumber > 0 && description.Trim().Equals(identificationNumber))
            {
                switch (insuredSearchType)
                {
                    case InsuredSearchType.IndividualId:
                        filter.Property(UniquePerson.Entities.Person.Properties.IndividualId, "p");
                        filter.Like();
                        filter.Constant($"{description.Trim()}%");
                        break;
                    case InsuredSearchType.DocumentNumber:
                        filter.Property(UniquePerson.Entities.Person.Properties.IdCardNo, "p");
                        filter.Like();
                        filter.Constant($"{description.Trim()}%");
                        break;
                    case InsuredSearchType.CodeIndividual:
                        filter.Property(UniquePerson.Entities.Insured.Properties.InsuredCode, "i");
                        filter.Like();
                        filter.Constant($"{description.Trim()}%");
                        break;
                }
            }
            else
            {
                IEnumerable<string> fullName = description.Trim().Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim());

                if (fullName.Any())
                {
                    List<string> properties = new List<string> {
                        UniquePerson.Entities.Person.Properties.Name,
                        UniquePerson.Entities.Person.Properties.Surname,
                        UniquePerson.Entities.Person.Properties.MotherLastName
                    };

                    filter.OpenParenthesis();
                    for (int j = 0; j < fullName.Count(); j++)
                    {
                        string item = fullName.ElementAt(j);
                        if (j != 0) { filter.And(); }

                        for (int i = 0; i < properties.Count(); i++)
                        {
                            string property = properties.ElementAt(i);
                            if (i != 0) { filter.Or(); }

                            filter.Property(property, "p");
                            filter.Like();
                            filter.Constant($"%{item}%");
                        }
                    }
                    filter.CloseParenthesis();

                    filter.Or();
                }

                filter.OpenParenthesis();
                filter.Property(UniquePerson.Entities.Person.Properties.IdCardNo, "p");
                filter.Like();
                filter.Constant($"{description.Trim()}%");
                filter.CloseParenthesis();
            }

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.MaxRows = maxRows;

            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.IndividualId, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.Surname, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.MotherLastName, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.Name, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.EconomicActivityCode, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.IdCardNo, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.IdCardTypeCode, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.BirthDate, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.Gender, "p")));

            selectQuery.Table = new ClassNameTable(typeof(UniquePerson.Entities.Person), "p");
            selectQuery.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    IssuanceInsured insured = new IssuanceInsured
                    {
                        CustomerType = CustomerType.Individual,
                        IndividualType = IndividualType.Person,
                        IndividualId = Convert.ToInt32(reader["IndividualId"]),
                        Surname = reader["Surname"].ToString(),
                        SecondSurname = reader["MotherLastName"] == null ? "" : reader["MotherLastName"].ToString(),
                        Name = reader["Name"].ToString(),
                        IdentificationDocument = new IssuanceIdentificationDocument
                        {
                            Number = reader["IdCardNo"].ToString(),
                            DocumentType = new IssuanceDocumentType
                            {
                                Id = Convert.ToInt32(reader["IdCardTypeCode"])
                            }
                        },
                        EconomicActivity = new IssuanceEconomicActivity
                        {
                            Id = Convert.ToInt32(reader["EconomicActivityCode"])
                        },
                        BirthDate = (DateTime?)reader["BirthDate"],
                        Gender = reader["Gender"].ToString(),
                        AssociationType = new IssuanceAssociationType
                        {
                            Id = 1
                        }
                    };
                    insureds.Add(insured);
                };
            }
            foreach (IssuanceInsured insured in insureds)
            {
                filter = new ObjectCriteriaBuilder();
                selectQuery = new SelectQuery();
                selectQuery.MaxRows = maxRows;

                filter.Property(UniquePerson.Entities.IdentityCardType.Properties.IdCardTypeCode, "i");
                filter.Equal();
                filter.Constant(insured.IdentificationDocument.DocumentType.Id);
                selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.IdentityCardType.Properties.Description, "i")));
                selectQuery.Table = new ClassNameTable(typeof(UniquePerson.Entities.IdentityCardType), "i");

                selectQuery.Where = filter.GetPredicate();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        insured.IdentificationDocument.DocumentType.Description = reader["Description"].ToString();
                    }
                }
            }

            return insureds;
        }

        public List<IssuanceInsured> GetPersonsByDescriptionInsuredSearchTypeMaxRows(string description, InsuredSearchType insuredSearchType, int maxRows)
        {
            List<IssuanceInsured> insureds = new List<IssuanceInsured>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Int64 identificationNumber = 0;
            Int64.TryParse(description, out identificationNumber);

            if (identificationNumber > 0 && description.Trim().Equals(identificationNumber.ToString()))
            {
                switch (insuredSearchType)
                {
                    case InsuredSearchType.IndividualId:
                        filter.Property(UniquePerson.Entities.Person.Properties.IndividualId, "p");
                        filter.Equal();
                        filter.Constant(description);
                        break;
                    case InsuredSearchType.DocumentNumber:
                        filter.Property(UniquePerson.Entities.Person.Properties.IdCardNo, "p");
                        filter.Like();
                        filter.Constant($"{description.Trim()}%");
                        break;
                    case InsuredSearchType.CodeIndividual:
                        filter.Property(UniquePerson.Entities.Insured.Properties.InsuredCode, "i");
                        filter.Equal();
                        filter.Constant(description);
                        break;
                }
            }
            else
            {
                IEnumerable<string> fullName = description.Trim().Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim());

                if (fullName.Any())
                {
                    List<string> properties = new List<string> {
                        UniquePerson.Entities.Person.Properties.Name,
                        UniquePerson.Entities.Person.Properties.Surname,
                        UniquePerson.Entities.Person.Properties.MotherLastName
                    };

                    filter.OpenParenthesis();
                    for (int j = 0; j < fullName.Count(); j++)
                    {
                        string item = fullName.ElementAt(j);
                        if (j != 0) { filter.And(); }

                        for (int i = 0; i < properties.Count(); i++)
                        {
                            string property = properties.ElementAt(i);
                            if (i != 0) { filter.Or(); }

                            filter.Property(property, "p");
                            filter.Like();
                            filter.Constant($"%{item}%");
                        }
                    }
                    filter.CloseParenthesis();

                    filter.Or();
                }

                filter.OpenParenthesis();
                filter.Property(UniquePerson.Entities.Person.Properties.IdCardNo, "p");
                filter.Like();
                filter.Constant($"{description.Trim()}%");
                filter.CloseParenthesis();
            }

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.MaxRows = maxRows;

            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.IndividualId, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.Surname, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.MotherLastName, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.Name, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.EconomicActivityCode, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.IdCardNo, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.IdCardTypeCode, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.BirthDate, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.Gender, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.CheckPayableTo, "i")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.InsuredCode, "i")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.EnteredDate, "i")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.DeclinedDate, "i")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.InsDeclinedTypeCode, "i")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.Annotations, "i")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.InsProfileCode, "i")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.BranchCode, "i")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.ModifyDate, "i")));

            Join join = new Join(new ClassNameTable(typeof(UniquePerson.Entities.Person), "p"), new ClassNameTable(typeof(UniquePerson.Entities.Insured), "i"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UniquePerson.Entities.Person.Properties.IndividualId, "p")
                .Equal()
                .Property(UniquePerson.Entities.Insured.Properties.IndividualId, "i").GetPredicate());

            selectQuery.Table = join;
            selectQuery.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    IssuanceInsured insured = new IssuanceInsured
                    {
                        CustomerType = CustomerType.Individual,
                        IndividualType = IndividualType.Person,
                        IndividualId = Convert.ToInt32(reader["IndividualId"]),
                        Surname = reader["Surname"].ToString(),
                        SecondSurname = reader["MotherLastName"] == null ? "" : reader["MotherLastName"].ToString(),
                        Name = reader["Name"].ToString(),
                        IdentificationDocument = new IssuanceIdentificationDocument
                        {
                            Number = reader["IdCardNo"].ToString(),
                            DocumentType = new IssuanceDocumentType
                            {
                                Id = Convert.ToInt32(reader["IdCardTypeCode"])
                            }
                        },
                        EconomicActivity = new IssuanceEconomicActivity
                        {
                            Id = Convert.ToInt32(reader["EconomicActivityCode"])
                        },
                        BirthDate = (DateTime?)reader["BirthDate"],
                        Gender = reader["Gender"].ToString(),
                        AssociationType = new IssuanceAssociationType
                        {
                            Id = 1
                        }
                    };

                    if (reader["InsuredCode"] != null)
                    {
                        insured.InsuredId = Convert.ToInt32(reader["InsuredCode"]);
                        insured.DeclinedDate = (DateTime?)reader["DeclinedDate"];
                    }

                    insureds.Add(insured);
                }
            }

            foreach (IssuanceInsured insured in insureds)
            {
                filter = new ObjectCriteriaBuilder();
                selectQuery = new SelectQuery();
                selectQuery.MaxRows = maxRows;

                filter.Property(UniquePerson.Entities.IdentityCardType.Properties.IdCardTypeCode, "i");
                filter.Equal();
                filter.Constant(insured.IdentificationDocument.DocumentType.Id);
                selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.IdentityCardType.Properties.Description, "i")));
                selectQuery.Table = new ClassNameTable(typeof(UniquePerson.Entities.IdentityCardType), "i");

                selectQuery.Where = filter.GetPredicate();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        insured.IdentificationDocument.DocumentType.Description = reader["Description"].ToString();
                    }
                }
            }

            return insureds;
        }

        public List<IssuanceInsured> GetCompaniesByDescriptionSearchTypeMaxRows(string description, InsuredSearchType insuredSearchType, int maxRows)
        {
            List<IssuanceInsured> insureds = new List<IssuanceInsured>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Int64 identificationNumber = 0;
            Int64.TryParse(description, out identificationNumber);

            if (identificationNumber > 0 && description.Trim().Equals(identificationNumber))
            {
                switch (insuredSearchType)
                {
                    case InsuredSearchType.IndividualId:
                        filter.Property(UniquePerson.Entities.Company.Properties.IndividualId, "c");
                        filter.Like();
                        filter.Constant($"{description.Trim()}%");
                        break;
                    case InsuredSearchType.DocumentNumber:
                        filter.Property(UniquePerson.Entities.Company.Properties.TributaryIdNo, "c");
                        filter.Like();
                        filter.Constant($"{description.Trim()}%");
                        break;
                    case InsuredSearchType.CodeIndividual:
                        filter.Property(UniquePerson.Entities.Insured.Properties.InsuredCode, "i");
                        filter.Like();
                        filter.Constant($"{description.Trim()}%");
                        break;
                }
            }
            else
            {
                filter.Property(UniquePerson.Entities.Company.Properties.TradeName, "c");
                filter.Like();
                filter.Constant($"%{description.Trim()}%");
                filter.Or();
                filter.Property(UniquePerson.Entities.Company.Properties.TributaryIdNo, "c");
                filter.Like();
                filter.Constant($"{description.Trim()}%");
            }

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.MaxRows = maxRows;

            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.CoCompany.Properties.AssociationTypeCode, "cc")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Company.Properties.IndividualId, "c")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Company.Properties.TradeName, "c")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Company.Properties.EconomicActivityCode, "c")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Company.Properties.TributaryIdNo, "c")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Company.Properties.TributaryIdTypeCode, "c")));


            Join join = new Join(new ClassNameTable(typeof(UniquePerson.Entities.Company), "c"), new ClassNameTable(typeof(UniquePerson.Entities.CoCompany), "cc"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UniquePerson.Entities.Company.Properties.IndividualId, "c")
                .Equal().Property(UniquePerson.Entities.CoCompany.Properties.IndividualId, "cc")
                .GetPredicate());

            selectQuery.Table = join;
            selectQuery.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    IssuanceInsured insured = new IssuanceInsured
                    {
                        CustomerType = CustomerType.Individual,
                        IndividualType = IndividualType.Company,
                        IndividualId = Convert.ToInt32(reader["IndividualId"]),
                        Name = reader["TradeName"].ToString(),
                        IdentificationDocument = new IssuanceIdentificationDocument
                        {
                            Number = reader["TributaryIdNo"].ToString(),
                            DocumentType = new IssuanceDocumentType
                            {
                                Id = Convert.ToInt32(reader["TributaryIdTypeCode"])
                            }
                        },
                        EconomicActivity = new IssuanceEconomicActivity
                        {
                            Id = Convert.ToInt32(reader["EconomicActivityCode"])
                        },
                        AssociationType = new IssuanceAssociationType
                        {
                            Id = Convert.ToInt32(reader["AssociationTypeCode"])
                        }
                    };

                    insureds.Add(insured);
                }
            }
            foreach (IssuanceInsured insured in insureds)
            {
                filter = new ObjectCriteriaBuilder();
                selectQuery = new SelectQuery();
                selectQuery.MaxRows = maxRows;

                filter.Property(UniquePerson.Entities.TributaryIdentityType.Properties.TributaryIdTypeCode, "i");
                filter.Equal();
                filter.Constant(insured.IdentificationDocument.DocumentType.Id);
                selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.TributaryIdentityType.Properties.Description, "i")));
                selectQuery.Table = new ClassNameTable(typeof(UniquePerson.Entities.TributaryIdentityType), "i");

                selectQuery.Where = filter.GetPredicate();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        insured.IdentificationDocument.DocumentType.Description = reader["Description"].ToString();
                    }
                }
            }
            return insureds;
        }

        public List<IssuanceInsured> GetCompaniesByDescriptionInsuredSearchTypeMaxRows(string description, InsuredSearchType insuredSearchType, int maxRows)
        {
            List<IssuanceInsured> insureds = new List<IssuanceInsured>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Int64 identificationNumber = 0;
            Int64.TryParse(description, out identificationNumber);

            if (identificationNumber > 0 && description.Trim().Equals(identificationNumber.ToString()))
            {
                switch (insuredSearchType)
                {
                    case InsuredSearchType.IndividualId:
                        filter.Property(UniquePerson.Entities.Company.Properties.IndividualId, "c");
                        filter.Equal();
                        filter.Constant(description);
                        break;
                    case InsuredSearchType.DocumentNumber:
                        filter.Property(UniquePerson.Entities.Company.Properties.TributaryIdNo, "c");
                        filter.Like();
                        filter.Constant($"{description.Trim()}%");
                        break;
                    case InsuredSearchType.CodeIndividual:
                        filter.Property(UniquePerson.Entities.Insured.Properties.InsuredCode, "i");
                        filter.Equal();
                        filter.Constant(description);
                        break;
                }
            }
            else
            {
                filter.Property(UniquePerson.Entities.Company.Properties.TradeName, "c");
                filter.Like();
                filter.Constant($"%{description.Trim()}%");
                filter.Or();
                filter.Property(UniquePerson.Entities.Company.Properties.TributaryIdNo, "c");
                filter.Like();
                filter.Constant($"{description.Trim()}%");
            }

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.MaxRows = maxRows;

            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.CoCompany.Properties.AssociationTypeCode, "cc")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Company.Properties.IndividualId, "c")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Company.Properties.TradeName, "c")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Company.Properties.EconomicActivityCode, "c")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Company.Properties.TributaryIdNo, "c")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Company.Properties.TributaryIdTypeCode, "c")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.CheckPayableTo, "i")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.InsuredCode, "i")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.EnteredDate, "i")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.DeclinedDate, "i")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.InsDeclinedTypeCode, "i")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.Annotations, "i")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.InsProfileCode, "i")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.BranchCode, "i")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.ModifyDate, "i")));

            Join join = new Join(new ClassNameTable(typeof(UniquePerson.Entities.Company), "c"), new ClassNameTable(typeof(UniquePerson.Entities.Insured), "i"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder().Property(UniquePerson.Entities.Company.Properties.IndividualId, "c").Equal().Property(UniquePerson.Entities.Insured.Properties.IndividualId, "i").GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(UniquePerson.Entities.CoCompany), "cc"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UniquePerson.Entities.Company.Properties.IndividualId, "c")
                .Equal().Property(UniquePerson.Entities.CoCompany.Properties.IndividualId, "cc")
                .GetPredicate());

            selectQuery.Table = join;
            selectQuery.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    IssuanceInsured insured = new IssuanceInsured
                    {
                        CustomerType = CustomerType.Individual,
                        IndividualType = IndividualType.Company,
                        IndividualId = Convert.ToInt32(reader["IndividualId"]),
                        Name = reader["TradeName"].ToString(),
                        IdentificationDocument = new IssuanceIdentificationDocument
                        {
                            Number = reader["TributaryIdNo"].ToString(),
                            DocumentType = new IssuanceDocumentType
                            {
                                Id = Convert.ToInt32(reader["TributaryIdTypeCode"])
                            }
                        },
                        EconomicActivity = new IssuanceEconomicActivity
                        {
                            Id = Convert.ToInt32(reader["EconomicActivityCode"])
                        },
                        AssociationType = new IssuanceAssociationType
                        {
                            Id = Convert.ToInt32(reader["AssociationTypeCode"])
                        }
                    };

                    if (reader["InsuredCode"] != null)
                    {
                        insured.InsuredId = Convert.ToInt32(reader["InsuredCode"]);
                        insured.DeclinedDate = (DateTime?)reader["DeclinedDate"];
                    }

                    insureds.Add(insured);
                }
            }

            foreach (IssuanceInsured insured in insureds)
            {
                filter = new ObjectCriteriaBuilder();
                selectQuery = new SelectQuery();
                selectQuery.MaxRows = maxRows;

                filter.Property(UniquePerson.Entities.TributaryIdentityType.Properties.TributaryIdTypeCode, "i");
                filter.Equal();
                filter.Constant(insured.IdentificationDocument.DocumentType.Id);
                selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.TributaryIdentityType.Properties.Description, "i")));
                selectQuery.Table = new ClassNameTable(typeof(UniquePerson.Entities.TributaryIdentityType), "i");

                selectQuery.Where = filter.GetPredicate();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        insured.IdentificationDocument.DocumentType.Description = reader["Description"].ToString();
                    }
                }
            }

            return insureds;
        }

        public List<IssuanceInsured> GetProspectsByDescriptionInsuredSearchTypeMaxRows(string description, InsuredSearchType insuredSearchType, int maxRows)
        {
            List<IssuanceInsured> insureds = new List<IssuanceInsured>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            Int64 identificationNumber = 0;
            Int64.TryParse(description, out identificationNumber);

            if (identificationNumber > 0 && description.Trim().Equals(identificationNumber.ToString()))
            {
                switch (insuredSearchType)
                {
                    case InsuredSearchType.IndividualId:
                        filter.Property(UniquePerson.Entities.Prospect.Properties.ProspectId, "p");
                        filter.Like();
                        filter.Constant($"{description.Trim()}%");
                        break;
                    case InsuredSearchType.DocumentNumber:
                        filter.Property(UniquePerson.Entities.Prospect.Properties.IdCardNo, "p");
                        filter.Like();
                        filter.Constant($"{description.Trim()}%");
                        filter.Or();
                        filter.Property(UniquePerson.Entities.Prospect.Properties.TributaryIdNo, "p");
                        filter.Like();
                        filter.Constant($"{description.Trim()}%");
                        break;
                }
            }
            else
            {
                IEnumerable<string> fullName = description.Trim().Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim());

                if (fullName.Any())
                {
                    List<string> properties = new List<string> {
                        UniquePerson.Entities.Prospect.Properties.TradeName,
                        UniquePerson.Entities.Prospect.Properties.Name,
                        UniquePerson.Entities.Prospect.Properties.Surname,
                        UniquePerson.Entities.Prospect.Properties.MotherLastName
                    };

                    filter.OpenParenthesis();
                    for (int j = 0; j < fullName.Count(); j++)
                    {
                        string item = fullName.ElementAt(j);
                        if (j != 0) { filter.And(); }

                        for (int i = 0; i < properties.Count(); i++)
                        {
                            string property = properties.ElementAt(i);
                            if (i != 0) { filter.Or(); }

                            filter.Property(property, "p");
                            filter.Like();
                            filter.Constant($"%{item}%");
                        }
                    }
                    filter.CloseParenthesis();

                    filter.Or();
                }

                filter.OpenParenthesis();
                filter.Property(UniquePerson.Entities.Prospect.Properties.IdCardNo, "p");
                filter.Like();
                filter.Constant($"{description.Trim()}%");

                filter.Or();

                filter.Property(UniquePerson.Entities.Prospect.Properties.TributaryIdNo, "p");
                filter.Like();
                filter.Constant($"{description.Trim()}%");
                filter.CloseParenthesis();
            }

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.MaxRows = maxRows;

            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Prospect.Properties.ProspectId, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Prospect.Properties.IndividualTypeCode, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Prospect.Properties.TradeName, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Prospect.Properties.TributaryIdNo, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Prospect.Properties.TributaryIdTypeCode, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Prospect.Properties.Surname, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Prospect.Properties.MotherLastName, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Prospect.Properties.Name, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Prospect.Properties.IdCardNo, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Prospect.Properties.IdCardTypeCode, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Prospect.Properties.BirthDate, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Prospect.Properties.Gender, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Prospect.Properties.Street, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Prospect.Properties.PhoneNumber, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Prospect.Properties.EmailAddress, "p")));

            selectQuery.Table = new ClassNameTable(typeof(UniquePerson.Entities.Prospect), "p");
            selectQuery.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    IssuanceInsured insured = new IssuanceInsured
                    {
                        CustomerType = CustomerType.Prospect,
                        IndividualType = (IndividualType)Convert.ToInt32(reader["IndividualTypeCode"]),
                        IndividualId = Convert.ToInt32(reader["ProspectId"]),
                    };

                    insured.CompanyName = new IssuanceCompanyName
                    {
                        Address = new IssuanceAddress
                        {
                            Description = reader["Street"] == null ? "" : reader["Street"].ToString()
                        },
                        Phone = new IssuancePhone
                        {
                            Description = reader["PhoneNumber"] == null ? "" : reader["PhoneNumber"].ToString()
                        },
                        Email = new IssuanceEmail
                        {
                            Description = reader["EmailAddress"] == null ? "" : reader["EmailAddress"].ToString()
                        }
                    };

                    if (insured.IndividualType == IndividualType.Person)
                    {
                        insured.Surname = reader["Surname"].ToString();
                        insured.SecondSurname = reader["MotherLastName"] == null ? "" : reader["MotherLastName"].ToString();
                        insured.Name = reader["Name"].ToString();

                        insured.IdentificationDocument = new IssuanceIdentificationDocument
                        {
                            Number = reader["IdCardNo"] == null ? "" : reader["IdCardNo"].ToString(),
                            DocumentType = new IssuanceDocumentType
                            {
                                Id = Convert.ToInt32(reader["IdCardTypeCode"])
                            }
                        };
                        insured.BirthDate = (DateTime?)reader["BirthDate"];
                        insured.Gender = reader["Gender"] == null ? "" : reader["Gender"].ToString();
                    }
                    else
                    {
                        insured.Name = reader["TradeName"] == null ? "" : reader["TradeName"].ToString();
                        insured.IdentificationDocument = new IssuanceIdentificationDocument
                        {
                            Number = reader["TributaryIdNo"].ToString(),
                            DocumentType = new IssuanceDocumentType
                            {
                                Id = Convert.ToInt32(reader["TributaryIdTypeCode"])
                            }
                        };
                    }

                    insureds.Add(insured);
                }
            }

            foreach (IssuanceInsured insured in insureds)
            {
                filter = new ObjectCriteriaBuilder();
                selectQuery = new SelectQuery();
                selectQuery.MaxRows = maxRows;

                if (insured.IndividualType == IndividualType.Person)
                {
                    filter.Property(UniquePerson.Entities.IdentityCardType.Properties.IdCardTypeCode, "i");
                    filter.Equal();
                    filter.Constant(insured.IdentificationDocument.DocumentType.Id);
                    selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.IdentityCardType.Properties.Description, "i")));
                    selectQuery.Table = new ClassNameTable(typeof(UniquePerson.Entities.IdentityCardType), "i");
                }
                else
                {
                    filter.Property(UniquePerson.Entities.TributaryIdentityType.Properties.TributaryIdTypeCode, "i");
                    filter.Equal();
                    filter.Constant(insured.IdentificationDocument.DocumentType.Id);
                    selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.TributaryIdentityType.Properties.Description, "i")));
                    selectQuery.Table = new ClassNameTable(typeof(UniquePerson.Entities.TributaryIdentityType), "i");
                }

                selectQuery.Where = filter.GetPredicate();
                using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
                {
                    while (reader.Read())
                    {
                        insured.IdentificationDocument.DocumentType.Description = reader["Description"].ToString();
                    }
                }
            }
            return insureds;
        }

        public IssuanceInsured GetInsuredByInsuredCode(int insuredCode)
        {
            IssuanceInsured insured = new IssuanceInsured();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePerson.Entities.Insured.Properties.InsuredCode, "i");
            filter.Equal();
            filter.Constant(insuredCode);

            SelectQuery selectQuery = new SelectQuery();

            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.InsuredCode, "i")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.DeclinedDate, "i")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Individual.Properties.IndividualId, "in")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Individual.Properties.EconomicActivityCode, "in")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Individual.Properties.IndividualTypeCode, "in")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Company.Properties.TradeName, "c")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Company.Properties.TributaryIdNo, "c")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Company.Properties.TributaryIdTypeCode, "c")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.Surname, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.MotherLastName, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.Name, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.IdCardNo, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.IdCardTypeCode, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.BirthDate, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.Gender, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.IndividualPaymentMethod.Properties.PaymentMethodCode, "pma")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.IndividualPaymentMethod.Properties.PaymentId, "pma")));

            Join join = new Join(new ClassNameTable(typeof(UniquePerson.Entities.Insured), "i")
                , new ClassNameTable(typeof(UniquePerson.Entities.Individual), "in"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UniquePerson.Entities.Insured.Properties.IndividualId, "i")
                .Equal().Property(UniquePerson.Entities.Company.Properties.IndividualId, "in")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(UniquePerson.Entities.Company), "c"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UniquePerson.Entities.Insured.Properties.IndividualId, "i")
                .Equal().Property(UniquePerson.Entities.Company.Properties.IndividualId, "c")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(UniquePerson.Entities.Person), "p"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UniquePerson.Entities.Insured.Properties.IndividualId, "i")
                .Equal().Property(UniquePerson.Entities.Person.Properties.IndividualId, "p")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(UniquePerson.Entities.IndividualPaymentMethod), "pma"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UniquePerson.Entities.Insured.Properties.IndividualId, "i")
                .Equal().Property(UniquePerson.Entities.IndividualPaymentMethod.Properties.IndividualId, "pma")
                .GetPredicate());

            selectQuery.Table = join;
            selectQuery.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {

                    if (Convert.ToInt32(reader["IndividualTypeCode"].ToString()) == Convert.ToInt32(IndividualType.Company))
                    {
                        insured = new IssuanceInsured
                        {
                            CustomerType = CustomerType.Individual,
                            IndividualType = IndividualType.Company,
                            IndividualId = Convert.ToInt32(reader["IndividualId"]),
                            Name = reader["TradeName"].ToString(),
                            IdentificationDocument = new IssuanceIdentificationDocument
                            {
                                Number = reader["TributaryIdNo"].ToString(),
                                DocumentType = new IssuanceDocumentType
                                {
                                    Id = Convert.ToInt32(reader["TributaryIdTypeCode"])
                                }
                            },
                            EconomicActivity = new IssuanceEconomicActivity
                            {
                                Id = Convert.ToInt32(reader["EconomicActivityCode"])
                            },

                            InsuredId = Convert.ToInt32(reader["InsuredCode"]),
                            DeclinedDate = (DateTime?)reader["DeclinedDate"],
                            PaymentMethod = new IssuancePaymentMethod
                            {
                                Id = reader["PaymentMethodCode"] == null ? 0 : Convert.ToInt32(reader["PaymentMethodCode"]),
                                PaymentId = reader["PaymentId"] == null ? 0 : Convert.ToInt32(reader["PaymentId"])
                            }
                        };
                    }
                    else
                    {
                        insured = new IssuanceInsured
                        {
                            CustomerType = CustomerType.Individual,
                            IndividualType = IndividualType.Person,
                            IndividualId = Convert.ToInt32(reader["IndividualId"]),
                            Surname = reader["Surname"].ToString(),
                            SecondSurname = reader["MotherLastName"] == null ? "" : reader["MotherLastName"].ToString(),
                            Name = reader["Name"].ToString(),
                            IdentificationDocument = new IssuanceIdentificationDocument
                            {
                                Number = reader["IdCardNo"].ToString(),
                                DocumentType = new IssuanceDocumentType
                                {
                                    Id = Convert.ToInt32(reader["IdCardTypeCode"])
                                }
                            },
                            EconomicActivity = new IssuanceEconomicActivity
                            {
                                Id = Convert.ToInt32(reader["EconomicActivityCode"])
                            },
                            BirthDate = (DateTime?)reader["BirthDate"],
                            Gender = reader["Gender"].ToString(),

                            InsuredId = Convert.ToInt32(reader["InsuredCode"]),
                            DeclinedDate = (DateTime?)reader["DeclinedDate"],
                            PaymentMethod = new IssuancePaymentMethod
                            {
                                Id = reader["PaymentMethodCode"] == null ? 0 : Convert.ToInt32(reader["PaymentMethodCode"]),
                                PaymentId = reader["PaymentId"] == null ? 0 : Convert.ToInt32(reader["PaymentId"])
                            }
                        };
                        insured.Name = insured.Name + " " + insured.SecondSurname + " " + insured.Surname;
                    }
                }
            }

            return insured;
        }

        public IssuanceInsured GetInsuredByIndividualId(int individualId)
        {
            IssuanceInsured insured = new IssuanceInsured();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePerson.Entities.Insured.Properties.IndividualId, "i");
            filter.Equal();
            filter.Constant(individualId);

            SelectQuery selectQuery = new SelectQuery();

            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.InsuredCode, "i")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.DeclinedDate, "i")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Individual.Properties.IndividualId, "in")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Individual.Properties.EconomicActivityCode, "in")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Individual.Properties.IndividualTypeCode, "in")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Company.Properties.TradeName, "c")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Company.Properties.TributaryIdNo, "c")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Company.Properties.TributaryIdTypeCode, "c")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.Surname, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.MotherLastName, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.Name, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.IdCardNo, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.IdCardTypeCode, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.BirthDate, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Person.Properties.Gender, "p")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.IndividualPaymentMethod.Properties.PaymentMethodCode, "pma")));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.IndividualPaymentMethod.Properties.PaymentId, "pma")));

            Join join = new Join(new ClassNameTable(typeof(UniquePerson.Entities.Insured), "i")
                , new ClassNameTable(typeof(UniquePerson.Entities.Individual), "in"), JoinType.Inner);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UniquePerson.Entities.Insured.Properties.IndividualId, "i")
                .Equal().Property(UniquePerson.Entities.Company.Properties.IndividualId, "in")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(UniquePerson.Entities.Company), "c"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UniquePerson.Entities.Insured.Properties.IndividualId, "i")
                .Equal().Property(UniquePerson.Entities.Company.Properties.IndividualId, "c")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(UniquePerson.Entities.Person), "p"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UniquePerson.Entities.Insured.Properties.IndividualId, "i")
                .Equal().Property(UniquePerson.Entities.Person.Properties.IndividualId, "p")
                .GetPredicate());

            join = new Join(join, new ClassNameTable(typeof(UniquePerson.Entities.IndividualPaymentMethod), "pma"), JoinType.Left);
            join.Criteria = (new ObjectCriteriaBuilder()
                .Property(UniquePerson.Entities.Insured.Properties.IndividualId, "i")
                .Equal().Property(UniquePerson.Entities.IndividualPaymentMethod.Properties.IndividualId, "pma")
                .GetPredicate());

            selectQuery.Table = join;
            selectQuery.Where = filter.GetPredicate();

            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {

                    if (Convert.ToInt32(reader["IndividualTypeCode"].ToString()) == Convert.ToInt32(IndividualType.Company))
                    {
                        insured = new IssuanceInsured
                        {
                            CustomerType = CustomerType.Individual,
                            IndividualType = IndividualType.Company,
                            IndividualId = Convert.ToInt32(reader["IndividualId"]),
                            Name = reader["TradeName"].ToString(),
                            IdentificationDocument = new IssuanceIdentificationDocument
                            {
                                Number = reader["TributaryIdNo"].ToString(),
                                DocumentType = new IssuanceDocumentType
                                {
                                    Id = Convert.ToInt32(reader["TributaryIdTypeCode"])
                                }
                            },
                            EconomicActivity = new IssuanceEconomicActivity
                            {
                                Id = Convert.ToInt32(reader["EconomicActivityCode"])
                            },

                            InsuredId = Convert.ToInt32(reader["InsuredCode"]),
                            DeclinedDate = (DateTime?)reader["DeclinedDate"],
                            PaymentMethod = new IssuancePaymentMethod
                            {
                                Id = reader["PaymentMethodCode"] == null ? 0 : Convert.ToInt32(reader["PaymentMethodCode"]),
                                PaymentId = reader["PaymentId"] == null ? 0 : Convert.ToInt32(reader["PaymentId"])
                            }
                        };
                    }
                    else
                    {
                        insured = new IssuanceInsured
                        {
                            CustomerType = CustomerType.Individual,
                            IndividualType = IndividualType.Person,
                            IndividualId = Convert.ToInt32(reader["IndividualId"]),
                            Surname = reader["Surname"].ToString(),
                            SecondSurname = reader["MotherLastName"] == null ? "" : reader["MotherLastName"].ToString(),
                            Name = reader["Name"].ToString(),
                            IdentificationDocument = new IssuanceIdentificationDocument
                            {
                                Number = reader["IdCardNo"].ToString(),
                                DocumentType = new IssuanceDocumentType
                                {
                                    Id = Convert.ToInt32(reader["IdCardTypeCode"])
                                }
                            },
                            EconomicActivity = new IssuanceEconomicActivity
                            {
                                Id = Convert.ToInt32(reader["EconomicActivityCode"])
                            },
                            BirthDate = (DateTime?)reader["BirthDate"],
                            Gender = reader["Gender"].ToString(),

                            InsuredId = Convert.ToInt32(reader["InsuredCode"]),
                            DeclinedDate = (DateTime?)reader["DeclinedDate"],
                            PaymentMethod = new IssuancePaymentMethod
                            {
                                Id = reader["PaymentMethodCode"] == null ? 0 : Convert.ToInt32(reader["PaymentMethodCode"]),
                                PaymentId = reader["PaymentId"] == null ? 0 : Convert.ToInt32(reader["PaymentId"])
                            }
                        };
                        insured.Name = insured.Name + " " + insured.SecondSurname + " " + insured.Surname;
                    }
                }
            }

            return insured;
        }
    }
}