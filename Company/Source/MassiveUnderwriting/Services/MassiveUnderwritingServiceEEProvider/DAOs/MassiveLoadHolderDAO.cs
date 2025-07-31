using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UPENUM = Sistran.Core.Application.UniquePersonService.Enums;
using UPMO = Sistran.Company.Application.UniquePersonServices.Models;

namespace Sistran.Company.Application.MassiveUnderwritingServices.EEProvider.DAOs
{
    public class MassiveLoadHolderDAO
    {
        public Holder CreateHolder(Row row)
        {
            Holder holder = new Holder();

            holder.IndividualType = (UPENUM.IndividualType)(int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderIndividualType));

            switch (holder.IndividualType)
            {
                case UPENUM.IndividualType.LegalPerson:
                    holder = CreateHolderCompany(row);
                    break;
                case UPENUM.IndividualType.Person:
                    holder = CreateHolderPerson(row);
                    break;
                default:
                    throw new ValidationException("ErrorCreateHolder|" + row.Number);
            }

            return holder;
        }

        private Holder CreateHolderCompany(Row row)
        {
            UPMO.Company company = new UPMO.Company();
            Individual individual = new Individual();

            int IndividualId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderInsuredCode));
            if (IndividualId > 0)
            {
                company = DelegateService.uniquePersonService.GetCompanyByIndividualId(IndividualId);

                if (company == null)
                {
                    company = CreateCompany(row);
                }
            }
            else
            {
                company = DelegateService.uniquePersonService.GetCompanyByDocumentNumber((string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyDocumentNumber)));
                if (company == null)
                {
                    company = CreateCompany(row);
                }
            }

            //ARMAR EL MODELO HOLDER
            Holder holder = new Holder
            {
                IdentificationDocument = new IdentificationDocument
                {
                    DocumentType = new DocumentType()
                }
            };
            holder.CustomerType = UPENUM.CustomerType.Individual; ;
            holder.IdentificationDocument.DocumentType.Id = company.IdentificationDocument.DocumentType.Id;
            holder.IdentificationDocument.Number = company.IdentificationDocument.Number;
            holder.PaymentMethod = company.PaymentMethod;
            holder.CompanyName = DelegateService.uniquePersonService.CompanyGetNotificationAddressesByIndividualId(company.IndividualId, UPENUM.CustomerType.Individual).FirstOrDefault();

            return holder;
        }

        private Holder CreateHolderPerson(Row row)
        {

            UPMO.Person person = new UPMO.Person();
            Individual individual = new Individual();

            int IndividualId = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderInsuredCode));
            if (IndividualId > 0)
            {
                person = DelegateService.uniquePersonService.GetPersonByIndividualId(IndividualId);

                if (person == null)
                {
                    person = CreatePerson(row);
                }

            }
            else
            {
                person = DelegateService.uniquePersonService.GetPersonByDocumentNumber((string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonDocumentNumber)));
                if (person == null)
                {
                    person = CreatePerson(row);
                }
            }

            //ARMAR EL MODELO HOLDER

            Holder holder = new Holder
            {
                IdentificationDocument = new IdentificationDocument
                {
                    DocumentType = new DocumentType()
                }
            };
            holder.IndividualId = person.IndividualId;
            holder.CustomerType = UPENUM.CustomerType.Individual;
            holder.IdentificationDocument.DocumentType.Id = person.IdentificationDocument.DocumentType.Id;
            holder.IdentificationDocument.Number = person.IdentificationDocument.Number;
            holder.PaymentMethod = person.PaymentMethod;
            holder.CompanyName = DelegateService.uniquePersonService.CompanyGetNotificationAddressesByIndividualId(person.IndividualId, UPENUM.CustomerType.Individual).FirstOrDefault();

            return holder;
        }

        private UPMO.Company CreateCompany(Row row)
        {
            UPMO.Company company = new UPMO.Company();

            Country country = new Country()
            {
                Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCountry))
            };

            company.Addresses = new List<Address>();
            company.Addresses.Add(new Address()
            {
                AddressType = new AddressType()
                {
                    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressType))
                },

                Description = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressDescription)),
                City = new City()
                {
                    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCity)),
                    State = new State()
                    {
                        Country = country,
                        Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressState))
                    }
                }
            });
            company.CountryOrigin = country;

            company.CompanyType = new CompanyType()
            {
                Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyType))
            };
            company.IdentificationDocument = new IdentificationDocument()
            {
                DocumentType = new DocumentType()
                {
                    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyDocumentType))
                },
                Number = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyDocumentNumber))
            };
            company.EconomicActivity = new EconomicActivity()
            {
                Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEconomicActivity))
            };
            company.Name = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderCompanyTradeName));
            //exonerationType
            company.Phones = new List<Phone>();
            company.Phones.Add(new Phone()
            {
                PhoneType = new PhoneType()
                {
                    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneType))

                },
                Description = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneDescription))
            });
            company.Emails = new List<Email>();
            company.Emails.Add(new Email()
            {
                EmailType = new EmailType()
                {
                    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEmailType))
                },
                Description = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEmailDescription))
            });
            company.PaymentMethod = new IndividualPaymentMethod()
            {
                Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPaymentPlan))
            };

            company = DelegateService.uniquePersonService.CreateCompany(company);
            return company;
        }

        private UPMO.Person CreatePerson(Row row)
        {
            Company.Application.UniquePersonServices.Models.Person person = new UniquePersonServices.Models.Person();
            person.Addresses = new List<Address>();
            person.Addresses.Add(new Address()
            {
                AddressType = new AddressType()
                {
                    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressType))
                },

                Description = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressDescription)),
                City = new City()
                {
                    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCity)),
                    State = new State()
                    {
                        Country = new Country()
                        {
                            Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressCountry))
                        },
                        Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderAddressState))
                    }
                }
            });

            person.Surname = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonSurname));
            person.SecondSurname = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonSecondSurname));
            person.Names = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonName));
            person.Gender = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonGender));
            person.MaritalStatus = new MaritalStatus()
            {
                Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonMaritalStatus))
            };
            person.BirthDate = (DateTime)DelegateService.commonService.GetValueByField<DateTime>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonBirthDate));

            person.IdentificationDocument = new IdentificationDocument()
            {
                DocumentType = new DocumentType()
                {
                    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonDocumentType))
                },
                Number = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPersonDocumentNumber))
            };
            person.EconomicActivity = new EconomicActivity()
            {
                Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEconomicActivity))
            };
            person.Phones = new List<Phone>();
            person.Phones.Add(new Phone()
            {
                PhoneType = new PhoneType()
                {
                    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneType))

                },
                Description = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPhoneDescription))
            });
            person.Emails = new List<Email>();
            person.Emails.Add(new Email()
            {
                EmailType = new EmailType()
                {
                    Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEmailType))
                },
                Description = (string)DelegateService.commonService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderEmailDescription))
            });
            person.PaymentMethod = new IndividualPaymentMethod()
            {
                Id = (int)DelegateService.commonService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.HolderPaymentPlan))
            };
            person.EducativeLevel = new EducativeLevel();
            person.HouseType = new HouseType { Id = 1 };
            person.SocialLayer = new SocialLayer { Id = 1 };
            person.LaborPerson = new LaborPerson { Id = 1, Occupation = new Occupation { Id = 0 } };
            person = DelegateService.uniquePersonService.CreatePerson(person, null);

            return person;
        }
    }
}
