using AutoMapper;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.Parameters.Entities;
using Sistran.Core.Application.Product.Entities;
using Sistran.Core.Application.Tax.Entities;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Entities.views;
using Sistran.Core.Application.UniquePersonService.Enums;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ModelIndividual = Sistran.Core.Application.UniquePersonServiceIndividual.Models;
using modelsCommon = Sistran.Core.Application.CommonService.Models;
using modelsPerson = Sistran.Core.Application.UniquePersonService.Models;
using modelTax = Sistran.Core.Application.TaxServices.Models;
using PERMODEL = Sistran.Core.Application.UniquePersonService.Models;
using taxEntity = Sistran.Core.Application.Tax.Entities;


namespace Sistran.Core.Application.UniquePersonService.Assemblers
{
    /// <summary>
    /// Conversion de Entidades a Modelos
    /// </summary>
    public class ModelAssembler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ModelAssembler"/> class.
        /// </summary>
        protected ModelAssembler()
        {
        }
        #region Phone

        /// <summary>
        /// Creates the phone.
        /// </summary>
        /// <param name="phone">The phone.</param>
        /// <returns></returns>
        public static Models.Phone CreatePhone(Phone phone)
        {
            return new Models.Phone


            {
                Id = phone.DataId,
                Description = phone.PhoneNumber.ToString(),
                PhoneType = new Models.PhoneType { Id = phone.PhoneTypeCode },
                IsMain = phone.IsMain ?? false,

                CountryCode = phone.CountryCode,
                CityCode = phone.CityCode,
                ScheduleAvailability = phone.ScheduleAvailability,
                Extension = phone.Extension
            };
        }

        internal static List<ModelIndividual.IndividualType> CreateIndividualTypes(Parameters.Entities.IndividualType coIndividualType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates the phones.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.Phone> CreatePhones(BusinessCollection businessCollection)
        {
            List<Models.Phone> phones = new List<Models.Phone>();

            foreach (Phone field in businessCollection)
            {
                phones.Add(ModelAssembler.CreatePhone(field));
            }

            return phones;
        }

        #endregion

        #region PhoneType

        /// <summary>
        /// Creates the type of the phone.
        /// </summary>
        /// <param name="phoneType">Type of the phone.</param>
        /// <returns></returns>
        private static Models.PhoneType CreatePhoneType(PhoneType phoneType)
        {
            return new Models.PhoneType
            {
                Id = phoneType.PhoneTypeCode,
                SmallDescription = phoneType.SmallDescription,
                Description = phoneType.Description
            };
        }

        /// <summary>
        /// Obtener lista bitacora del asegurado y garantia
        /// </summary>
        /// <param name="individualId">individualId Asegurado</param>
        /// <param name="guaranteeId">id de garantia Asegurado</param>
        /// <returns>Listado de Bitacora de asegurado y garantia</returns>
        public static List<Models.InsuredGuaranteeLog> CreateInsuredGuaranteeLogs(BusinessCollection businessCollection)
        {
            List<Models.InsuredGuaranteeLog> guaranteeLogs = new List<Models.InsuredGuaranteeLog>();

            foreach (InsuredGuaranteeLog field in businessCollection)
            {
                guaranteeLogs.Add(ModelAssembler.CreateGuaranteeLogType(field));
            }
            return guaranteeLogs.OrderByDescending(x => x.LogDate).ToList();
        }

        private static Models.InsuredGuaranteeLog CreateGuaranteeLogType(InsuredGuaranteeLog x)
        {
            return new Models.InsuredGuaranteeLog
            {
                Description = x.Description,
                GuaranteeId = x.GuaranteeId,
                GuaranteeStatusCode = x.GuaranteeStatusCode,
                IndividualId = x.IndividualId,
                UserId = x.UserId,
                LogDate = x.LogDate
            };
        }

        /// <summary>
        /// Creates the phone types.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.PhoneType> CreatePhoneTypes(BusinessCollection businessCollection)
        {
            List<Models.PhoneType> phoneTypes = new List<Models.PhoneType>();

            foreach (PhoneType field in businessCollection)
            {
                phoneTypes.Add(ModelAssembler.CreatePhoneType(field));
            }

            return phoneTypes;
        }

        #endregion

        #region LegalRepresent

        /// <summary>
        /// Creates the legal represent.
        /// </summary>
        /// <param name="LegalRepresent">The legal represent.</param>
        /// <returns></returns>
        public static Models.LegalRepresentative CreateLegalRepresent(IndividualLegalRepresent LegalRepresent)
        {
            List<Models.Phone> phones = new List<Models.Phone>();


            if (LegalRepresent.Phone != null)
            {
                phones.Add(new Models.Phone { Id = 1, Description = LegalRepresent.Phone.ToString() });
            }

            if (LegalRepresent.CellPhone != null)
            {

                phones.Add(new Models.Phone { Id = 2, Description = LegalRepresent.CellPhone.ToString() });
            }

            List<Models.Address> addresses = new List<Models.Address>();


            addresses.Add(new Models.Address { Description = LegalRepresent.Address });

            List<Models.Email> emails = new List<Models.Email>();

            emails.Add(new Models.Email { Description = LegalRepresent.Email });

            Models.IdentificationDocument identificationDocument = new Models.IdentificationDocument();


            identificationDocument.Number = LegalRepresent.IdCardNo;
            identificationDocument.DocumentType = new Models.DocumentType() { Id = LegalRepresent.IdCardTypeCode };

            modelsCommon.Amount amount = new modelsCommon.Amount();
            if (LegalRepresent.AuthorizationAmount != null)
            {
                amount.Value = Convert.ToDecimal(LegalRepresent.AuthorizationAmount);

            }

            if (LegalRepresent.CurrencyCode != null)
            {
                amount.Currency = new modelsCommon.Currency() { Id = Convert.ToInt32(LegalRepresent.CurrencyCode) };
            }

            return new Models.LegalRepresentative
            {
                Id = LegalRepresent.IndividualId,
                FullName = LegalRepresent.LegalRepresentativeName,
                ExpeditionDate = LegalRepresent.ExpeditionDate,
                ExpeditionPlace = LegalRepresent.ExpeditionPlace,
                BirthDate = LegalRepresent.BirthDate,
                BirthPlace = LegalRepresent.BirthPlace,
                Nationality = LegalRepresent.Nationality,
                City = new modelsCommon.City() { Id = LegalRepresent.CityCode, Description = LegalRepresent.City, State = new modelsCommon.State() { Id = LegalRepresent.StateCode, Country = new modelsCommon.Country() { Id = LegalRepresent.CountryCode } } },
                Phone = LegalRepresent.Phone.ToString(),
                CellPhone = LegalRepresent.CellPhone.ToString(),
                JobTitle = LegalRepresent.JobTitle,
                Email = LegalRepresent.Email,
                Address = LegalRepresent.Address,
                IdentificationDocument = identificationDocument,
                AuthorizationAmount = amount,
                Description = LegalRepresent.Description
            };
        }

        /// <summary>
        /// Creates the legal represents.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.LegalRepresentative> CreateLegalRepresents(BusinessCollection businessCollection)
        {
            List<Models.LegalRepresentative> LegalRepresents = new List<Models.LegalRepresentative>();

            foreach (IndividualLegalRepresent field in businessCollection)
            {
                LegalRepresents.Add(ModelAssembler.CreateLegalRepresent(field));
            }

            return LegalRepresents;
        }

        #endregion

        #region Address

        /// <summary>
        /// Creates the address.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns></returns>
        public static Models.Address CreateAddress(Address addressEntity)
        {

            Models.Address address = new Models.Address
            {
                Id = addressEntity.DataId,
                Description = addressEntity.Street,
                IsMailAddress = addressEntity.IsMailingAddress,
                AddressType = new Models.AddressType() { Id = addressEntity.AddressTypeCode }
            };
            address.City = new modelsCommon.City
            {
                Id = addressEntity.CityCode ?? 0,
                State = new modelsCommon.State
                {
                    Id = addressEntity.StateCode ?? 0,
                    Country = new modelsCommon.Country
                    {
                        Id = addressEntity.CountryCode ?? 0
                    }

                }
            };

            return address;

        }

        /// <summary>
        /// Creates the addresses.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.Address> CreateAddresses(BusinessCollection businessCollection)
        {
            List<Models.Address> address = new List<Models.Address>();

            foreach (Address field in businessCollection)
            {
                address.Add(ModelAssembler.CreateAddress(field));
            }

            return address;
        }

        #endregion

        #region AddressType

        /// <summary>
        /// Creates the type of the address.
        /// </summary>
        /// <param name="addressType">Type of the address.</param>
        /// <returns></returns>
        public static Models.AddressType CreateAddressType(AddressType addressType)
        {
            return new Models.AddressType
            {
                Id = addressType.AddressTypeCode,
                Description = addressType.SmallDescription
            };
        }

        /// <summary>
        /// Creates the address types.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.AddressType> CreateAddressTypes(BusinessCollection businessCollection)
        {
            List<Models.AddressType> addressTypes = new List<Models.AddressType>();

            foreach (AddressType field in businessCollection)
            {
                addressTypes.Add(ModelAssembler.CreateAddressType(field));
            }

            return addressTypes;
        }


        /// <summary>
        /// Creates the address types.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.AddressType> CreateAddressTypes(BusinessCollection businessCollection, bool IsElectronic)
        {
            List<Models.AddressType> addressTypes = new List<Models.AddressType>();

            foreach (AddressType field in businessCollection)
            {
                if ((bool)field.IsElectronicMail == IsElectronic)
                {
                    addressTypes.Add(ModelAssembler.CreateAddressType(field));
                }

            }

            return addressTypes;
        }

        /// <summary>
        /// Creates the address types electronic.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.AddressType> CreateAddressTypesElectronic(BusinessCollection businessCollection)
        {
            List<Models.AddressType> addressTypes = new List<Models.AddressType>();

            foreach (AddressType field in businessCollection)
            {
                addressTypes.Add(ModelAssembler.CreateAddressType(field));
            }

            return addressTypes;
        }
        #endregion

        #region DocumenType
        /// <summary>
        /// Creates the type of the document.
        /// </summary>
        /// <param name="documentType">Type of the document.</param>
        /// <returns></returns>
        public static Models.DocumentType CreateDocumentType(DocumentType documentType)
        {
            return new Models.DocumentType
            {
                Id = documentType.IdDocumentType,
                Description = documentType.Description,
                SmallDescription = documentType.SmallDescription
            };
        }

        /// <summary>
        /// Creates the document types.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.DocumentType> CreateDocumentTypes(BusinessCollection businessCollection)
        {
            List<Models.DocumentType> documentTypes = new List<Models.DocumentType>();

            foreach (DocumentType field in businessCollection)
            {
                documentTypes.Add(ModelAssembler.CreateDocumentType(field));
            }

            return documentTypes;
        }
        #endregion

        #region Partner
        /// <summary>
        /// Creates the individual part ner.
        /// </summary>
        /// <param name="indParner">The ind parner.</param>
        /// <returns></returns>
        private static Models.Partner CreateIndividualPartNer(IndividualPartner indParner)
        {
            Models.IdentificationDocument identificationDocument = new Models.IdentificationDocument();

            identificationDocument.Number = indParner.IdCardNo;
            identificationDocument.DocumentType = new Models.DocumentType() { Id = indParner.IdCardTypeCode };

            return new Models.Partner
            {
                PartnerId = indParner.PartnerId,
                IdentificationDocument = identificationDocument,
                TradeName = indParner.TradeName,
                Active = indParner.Active
            };
        }

        /// <summary>
        /// Creates the individual part ner.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static Models.Partner CreateIndividualPartNer(BusinessCollection businessCollection)
        {
            List<Models.Partner> individualPartNer = new List<Models.Partner>();

            foreach (IndividualPartner field in businessCollection)
            {
                individualPartNer.Add(ModelAssembler.CreateIndividualPartNer(field));
            }

            return individualPartNer.FirstOrDefault();
        }

        /// <summary>
        /// Creates the par ner.
        /// </summary>
        /// <param name="indParner">The ind parner.</param>
        /// <returns></returns>
        public static Models.Partner CreateParNer(IndividualPartner indParner)
        {
            Models.IdentificationDocument identificationDocument = new Models.IdentificationDocument();


            identificationDocument.Number = indParner.IdCardNo;
            identificationDocument.DocumentType = new Models.DocumentType() { Id = indParner.IdCardTypeCode };

            return new Models.Partner
            {
                PartnerId = indParner.PartnerId,
                IdentificationDocument = identificationDocument,
                TradeName = indParner.TradeName,
                Active = indParner.Active
            };
        }

        /// <summary>
        /// Gets the individual part ner.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.Partner> GetIndividualPartNer(BusinessCollection businessCollection)
        {
            List<Models.Partner> individualPartNer = new List<Models.Partner>();

            foreach (IndividualPartner field in businessCollection)
            {
                individualPartNer.Add(ModelAssembler.CreateIndividualPartNer(field));
            }

            return individualPartNer;
        }
        #endregion

        #region Email

        /// <summary>
        /// Creates the email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public static Models.Email CreateEmail(Email email)
        {
            return new Models.Email
            {
                Id = email.DataId,
                Description = email.Address,
                IsMailingAddress = email.IsMailingAddress,
                EmailType = new Models.EmailType() { Id = email.EmailTypeCode }
            };
        }

        /// <summary>
        /// Creates the emails.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.Email> CreateEmails(BusinessCollection businessCollection)
        {
            List<Models.Email> emails = new List<Models.Email>();

            foreach (Email field in businessCollection)
            {
                emails.Add(ModelAssembler.CreateEmail(field));
            }

            return emails;
        }

        #endregion

        #region EmailType

        /// <summary>
        /// Creates the type of the email.
        /// </summary>
        /// <param name="emailType">Type of the email.</param>
        /// <returns></returns>
        private static Models.EmailType CreateEmailType(EmailType emailType)
        {
            return new Models.EmailType
            {
                Id = emailType.EmailTypeCode,
                SmallDescription = emailType.SmallDescription,
                Description = emailType.Description
            };
        }

        /// <summary>
        /// Creates the email types.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.EmailType> CreateEmailTypes(BusinessCollection businessCollection)
        {
            List<Models.EmailType> emailTypes = new List<Models.EmailType>();

            foreach (EmailType field in businessCollection)
            {
                emailTypes.Add(ModelAssembler.CreateEmailType(field));
            }

            return emailTypes;
        }

        #endregion

        #region AdressEmailType

        /// <summary>
        /// Creates the type of the email.
        /// </summary>
        /// <param name="emailType">Type of the email.</param>
        /// <returns></returns>
        private static Models.EmailType CreateAdressEmailType(AddressType adressType)
        {
            return new Models.EmailType
            {
                Id = adressType.AddressTypeCode,
                SmallDescription = adressType.TinyDescription,
                Description = adressType.SmallDescription
            };
        }

        /// <summary>
        /// Creates the email types.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.EmailType> CreateAdressEmailTypes(BusinessCollection businessCollection, bool IsElectronic)
        {
            List<Models.EmailType> emailTypes = new List<Models.EmailType>();

            foreach (AddressType field in businessCollection)
            {
                if ((bool)field.IsElectronicMail == IsElectronic)
                {
                    emailTypes.Add(ModelAssembler.CreateAdressEmailType(field));
                }
            }

            return emailTypes;
        }

        public static List<Models.AddressType> CreateAddresesElectronics(BusinessCollection businessCollection)
        {
            List<Models.AddressType> emailTypes = new List<Models.AddressType>();

            foreach (AddressType field in businessCollection)
            {
                if ((bool)field.IsElectronicMail)
                {
                    emailTypes.Add(ModelAssembler.CreateAddresElectronic(field));
                }
            }

            return emailTypes;

        }

        public static Models.AddressType CreateAddresElectronic(AddressType adressType)
        {
            return new Models.AddressType
            {
                Id = adressType.AddressTypeCode,
                Description = adressType.SmallDescription
            };
        }

        #endregion
        #region EducativeLevel

        /// <summary>
        /// Creates the educative level.
        /// </summary>
        /// <param name="educativeLevel">The educative level.</param>
        /// <returns></returns>
        private static Models.EducativeLevel CreateEducativeLevel(EducativeLevel educativeLevel)
        {
            return new Models.EducativeLevel
            {
                Id = educativeLevel.EducativeLevelCode,
                Description = educativeLevel.Description,
                SmallDescription = educativeLevel.SmallDescription
            };

        }

        /// <summary>
        /// Creates the educativelevels.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.EducativeLevel> CreateEducativelevels(BusinessCollection businessCollection)
        {
            List<Models.EducativeLevel> educativeLevel = new List<Models.EducativeLevel>();

            foreach (EducativeLevel field in businessCollection)
            {
                educativeLevel.Add(ModelAssembler.CreateEducativeLevel(field));
            }

            return educativeLevel;
        }

        #endregion

        #region Stratum
        /// <summary>
        /// Creates the social layer.
        /// </summary>
        /// <param name="socialLayer">The social layer.</param>
        /// <returns></returns>
        private static Models.SocialLayer CreateSocialLayer(SocialLayer socialLayer)
        {
            return new Models.SocialLayer
            {
                Id = socialLayer.SocialLayerCode,
                Description = socialLayer.Description,
                SmallDescription = socialLayer.SmallDescription
            };
        }
        /// <summary>
        /// Creates the social layers.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.SocialLayer> CreateSocialLayers(BusinessCollection businessCollection)
        {
            List<Models.SocialLayer> socialLayer = new List<Models.SocialLayer>();

            foreach (SocialLayer field in businessCollection)
            {
                socialLayer.Add(ModelAssembler.CreateSocialLayer(field));
            }

            return socialLayer;
        }


        #endregion

        #region Occupation
        /// <summary>
        /// Creates the occupation.
        /// </summary>
        /// <param name="occupation">The occupation.</param>
        /// <returns></returns>
        private static Models.Occupation CreateOccupation(Occupation occupation)
        {
            return new Models.Occupation
            {
                Id = occupation.OccupationCode,
                Description = occupation.Description,
                SmallDescription = occupation.SmallDescription
            };
        }
        /// <summary>
        /// Creates the occupations.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.Occupation> CreateOccupations(BusinessCollection businessCollection)
        {
            List<Models.Occupation> occupation = new List<Models.Occupation>();

            foreach (Occupation field in businessCollection)
            {
                occupation.Add(ModelAssembler.CreateOccupation(field));
            }

            return occupation;
        }
        #endregion

        #region PersonType
        /// <summary>
        /// Creates the type of the person.
        /// </summary>
        /// <param name="personType">Type of the person.</param>
        /// <returns></returns>
        private static Models.PersonType CreatePersonType(UniquePerson.Entities.PersonType personType)
        {
            return new Models.PersonType
            {
                Id = personType.PersonTypeCode,
                Description = personType.Description
            };


        }
        /// <summary>
        /// Creates the person types.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.PersonType> CreatePersonTypes(BusinessCollection businessCollection)
        {
            List<Models.PersonType> personTypes = new List<Models.PersonType>();
            foreach (UniquePerson.Entities.PersonType field in businessCollection)
            {
                personTypes.Add(ModelAssembler.CreatePersonType(field));

            }
            return personTypes;

        }
        #endregion

        #region Speciality
        /// <summary>
        /// Creates the speciality.
        /// </summary>
        /// <param name="speciality">The speciality.</param>
        /// <returns></returns>
        private static Models.Speciality CreateSpeciality(Speciality speciality)
        {
            return new Models.Speciality
            {
                Id = speciality.SpecialityCode,
                Description = speciality.Description,
                SmallDescription = speciality.SmallDescription
            };
        }
        /// <summary>
        /// Creates the specialties.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.Speciality> CreateSpecialties(BusinessCollection businessCollection)
        {
            List<Models.Speciality> specialties = new List<Models.Speciality>();

            foreach (Speciality field in businessCollection)
            {
                specialties.Add(ModelAssembler.CreateSpeciality(field));
            }

            return specialties;
        }
        #endregion

        #region IncomeLevel
        /// <summary>
        /// Creates the income level.
        /// </summary>
        /// <param name="incomeLevel">The income level.</param>
        /// <returns></returns>
        private static Models.IncomeLevel CreateIncomeLevel(IncomeLevel incomeLevel)
        {
            return new Models.IncomeLevel
            {
                Id = incomeLevel.IncomeLevelCode,
                Description = incomeLevel.Description,
                SmallDescription = incomeLevel.SmallDescription
            };
        }
        /// <summary>
        /// Creates the income levels.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.IncomeLevel> CreateIncomeLevels(BusinessCollection businessCollection)
        {
            List<Models.IncomeLevel> incomeLevels = new List<Models.IncomeLevel>();

            foreach (IncomeLevel field in businessCollection)
            {
                incomeLevels.Add(ModelAssembler.CreateIncomeLevel(field));
            }

            return incomeLevels;
        }
        #endregion

        #region PersonJob

        /// <summary>
        /// Creates the person job.
        /// </summary>
        /// <param name="personJob">The person job.</param>
        /// <returns></returns>
        public static Models.LaborPerson CreatePersonJob(PersonJob personJob)
        {
            Models.Occupation Occupation = new Models.Occupation();
            if (personJob.OtherOccupationCode != null)
            {
                Occupation.Id = Convert.ToInt32(personJob.OtherOccupationCode);
            }

            Models.IncomeLevel incomeLevel = new Models.IncomeLevel();

            if (personJob.IncomeLevelCode != null)
            {
                incomeLevel.Id = Convert.ToInt32(personJob.IncomeLevelCode);
            }

            Models.Phone phones = new Models.Phone();


            if (personJob.CompanyPhone != null)
            {
                phones.Id = (int)personJob.CompanyPhone;
            }

            Models.Speciality speciality = new Models.Speciality();

            if (personJob.SpecialityCode != null)
            {

                speciality.Id = Convert.ToInt32(personJob.SpecialityCode);
            }

            return new Models.LaborPerson
            {

                Id = personJob.IndividualId,
                Occupation = new Models.Occupation() { Id = personJob.OccupationCode },
                IncomeLevel = incomeLevel,
                CompanyName = personJob.CompanyName,
                JobSector = personJob.JobSector,
                Position = personJob.Position,
                Contact = personJob.Contact,
                CompanyPhone = phones,
                Speciality = speciality,
                OtherOccupation = Occupation
            };

        }

        /// <summary>
        /// Creates the person jobs.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.LaborPerson> CreatePersonJobs(BusinessCollection businessCollection)
        {
            List<Models.LaborPerson> personJobs = new List<Models.LaborPerson>();

            foreach (PersonJob field in businessCollection)
            {
                personJobs.Add(ModelAssembler.CreatePersonJob(field));
            }

            return personJobs;
        }


        #endregion

        #region Person
        /// <summary>
        /// Creates the person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        public static Models.Person CreatePerson(Person person)
        {

            int EducativeLevelCode = 0;
            int HouseTypeCode = 0;
            int SocialLayerCode = 0;

            if (person.EducativeLevelCode != null)
            {
                EducativeLevelCode = (int)person.EducativeLevelCode;
            }
            if (person.HouseTypeCode != null)
            {
                HouseTypeCode = (int)person.HouseTypeCode;
            }
            if (person.SocialLayerCode != null)
            {
                SocialLayerCode = (int)person.SocialLayerCode;
            }

            return new Models.Person
            {
                IndividualId = person.IndividualId,
                Gender = person.Gender,
                BirthDate = person.BirthDate,
                BirthPlace = person.BirthPlace,
                Children = person.Children,
                Names = person.Name,
                IdentificationDocument = new Models.IdentificationDocument() { DocumentType = new Models.DocumentType() { Id = person.IdCardTypeCode }, Number = person.IdCardNo },
                Surname = person.Surname,
                MotherLastName = person.MotherLastName,
                Name = person.Surname + " " + person.MotherLastName + " " + person.Name,
                EducativeLevel = new Models.EducativeLevel() { Id = EducativeLevelCode },
                SpouseName = person.SpouseName,
                Nationality = person.BirthCountryCode.ToString(),
                HouseType = new modelsPerson.HouseType { Id = HouseTypeCode },
                SocialLayer = new Models.SocialLayer() { Id = SocialLayerCode },
                MaritalStatus = new Models.MaritalStatus() { Id = person.MaritalStatusCode },
                IndividualType = Enums.IndividualType.Person,
                DataProtection = person.DataProtection,
                PersonCode = Convert.ToInt32(person.PersonTypeCode)
            };
        }

        /// <summary>
        /// Creates the persons.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.Person> CreatePersons(BusinessCollection businessCollection)
        {
            List<Models.Person> persons = new List<Models.Person>();

            foreach (Person field in businessCollection)
            {
                persons.Add(ModelAssembler.CreatePerson(field));
            }

            return persons;
        }

        #endregion

        #region AccountType

        /// <summary>
        /// Creates the type of the account.
        /// </summary>
        /// <param name="accountType">Type of the account.</param>
        /// <returns></returns>
        public static Models.PaymentAccountType CreateAccountType(PaymentAccountType accountType)
        {
            return new Models.PaymentAccountType
            {
                Id = accountType.PaymentAccountTypeCode,
                Description = accountType.Description,
            };

        }

        /// <summary>
        /// Creates the account types.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.PaymentAccountType> CreateAccountTypes(BusinessCollection businessCollection)
        {
            List<Models.PaymentAccountType> accountTypes = new List<Models.PaymentAccountType>();

            foreach (PaymentAccountType field in businessCollection)
            {
                accountTypes.Add(ModelAssembler.CreateAccountType(field));
            }

            return accountTypes;

        }
        #endregion

        #region PaymentMethodAccount
        /// <summary>
        /// Creates the payment method account.
        /// </summary>
        /// <param name="paymentMethodAccount">The payment method account.</param>
        /// <returns></returns>
        public static Models.PaymentMethodAccount CreatePaymentMethodAccount(PaymentMethodAccount paymentMethodAccount)
        {
            return new Models.PaymentMethodAccount
            {

                Id = paymentMethodAccount.PaymentId,
                PaymentMethod = new modelsCommon.PaymentMethod { Id = 0 },
                Bank = new modelsCommon.Bank()
                {
                    Id = Convert.ToInt32(paymentMethodAccount.BankCode),
                    BankBranches = new List<modelsCommon.BankBranch>
                { new modelsCommon.BankBranch { Id = paymentMethodAccount.BankBranchNumber == null ? 0: paymentMethodAccount.BankBranchNumber.Value } }
                },
                AccountType = new Models.PaymentAccountType() { Id = paymentMethodAccount.PaymentAccountTypeCode },
                AccountNumber = paymentMethodAccount.AccountNumber
            };

        }

        /// <summary>
        /// Creates the payment method accounts.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.PaymentMethodAccount> CreatePaymentMethodAccounts(BusinessCollection businessCollection)
        {
            List<Models.PaymentMethodAccount> paymentMethodAccounts = new List<Models.PaymentMethodAccount>();

            foreach (PaymentMethodAccount field in businessCollection)
            {
                paymentMethodAccounts.Add(ModelAssembler.CreatePaymentMethodAccount(field));
            }

            return paymentMethodAccounts;
        }
        /// <summary>
        /// Creates the payment method by payment account.
        /// </summary>
        /// <param name="paymentMethodAccount">The payment method account.</param>
        /// <returns></returns>
        public static Models.IndividualPaymentMethod CreatePaymentMethodByPaymentAccount(Models.PaymentMethodAccount paymentMethodAccount)
        {
            return new Models.IndividualPaymentMethod
            {
                Id = paymentMethodAccount.PaymentMethod.Id,
                RoleId = (int)RolesType.Insured,
                Enabled = true
            };
        }
        #endregion

        #region AgentDeclinedType

        /// <summary>
        /// Creates the type of the agent declined.
        /// </summary>
        /// <param name="agentDeclinedType">Type of the agent declined.</param>
        /// <returns></returns>
        private static Models.AgentDeclinedType CreateAgentDeclinedType(AgentDeclinedType agentDeclinedType)
        {
            return new Models.AgentDeclinedType
            {
                Id = agentDeclinedType.AgentDeclinedTypeCode,
                Description = agentDeclinedType.Description,
                SmallDescription = agentDeclinedType.SmallDescription
            };

        }

        /// <summary>
        /// Creates the agent declined types.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.AgentDeclinedType> CreateAgentDeclinedTypes(BusinessCollection businessCollection)
        {
            List<Models.AgentDeclinedType> agentDeclinedType = new List<Models.AgentDeclinedType>();

            foreach (AgentDeclinedType field in businessCollection)
            {
                agentDeclinedType.Add(ModelAssembler.CreateAgentDeclinedType(field));
            }

            return agentDeclinedType;
        }
        #endregion

        #region Agent

        /// <summary>
        /// Creates the agent.
        /// </summary>
        /// <param name="agent">The agent.</param>
        /// <returns></returns>
        public static Models.Agent CreateAgent(Agent agent)
        {
            return new Models.Agent
            {
                IndividualId = agent.IndividualId,
                AgentType = new Models.AgentType() { Id = agent.AgentTypeCode },
                FullName = agent.CheckPayableTo,
                AgentDeclinedType = new Models.AgentDeclinedType() { Id = agent.AgentDeclinedTypeCode },
                DateCurrent = agent.EnteredDate,
                DateDeclined = agent.DeclinedDate,
                Annotations = agent.Annotations,
                DateModification = agent.ModifyDate,
                GroupAgent = new Models.Base.BaseGroupAgent() { Id = agent.AgentGroupCode },
                SalesChannel = new Models.Base.BaseSalesChannel() { Id = agent.SalesChannelCode },
                Locker = agent.Locker,
            };
        }
        public static List<Models.Agent> CreateAgents(List<Agent> agents)
        {
            var imaper = CreateMapAgent();
            return imaper.Map<List<Agent>, List<Models.Agent>>(agents);
        }

        /// <summary>
        /// Creates the agents.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.Agent> CreateAgents(BusinessCollection businessCollection)
        {
            List<Models.Agent> agents = new List<Models.Agent>();

            foreach (Agent field in businessCollection)
            {
                agents.Add(ModelAssembler.CreateAgent(field));
            }

            return agents;
        }

        #endregion

        #region Company

        public static Models.Company CreateCompany(Company company)
        {
            Models.IdentificationDocument identificationDocument = new Models.IdentificationDocument
            {
                DocumentType = new Models.DocumentType { Id = company.TributaryIdTypeCode },
                Number = company.TributaryIdNo
            };

            modelsCommon.Country country = new modelsCommon.Country
            {
                Id = company.CountryCode
            };

            Models.CompanyType companyType = new Models.CompanyType
            {
                Id = company.CompanyTypeCode
            };

            return new Models.Company
            {
                IndividualId = company.IndividualId,
                Name = company.TradeName,
                IdentificationDocument = identificationDocument,
                CountryOrigin = country,
                CompanyType = companyType,
                IndividualType = Enums.IndividualType.LegalPerson,
                LegalRepresentative = new Models.LegalRepresentative()
                {
                    ContactName = company.ContactName,
                    ManagerName = company.ManagerName,
                    GeneralManagerName = company.GeneralManagerName,
                    ContactAdditionalInfo = company.ContactAdditionalInfo
                }
            };
        }

        public static List<Models.Company> CreateCompanies(BusinessCollection businessCollection)
        {
            List<Models.Company> companies = new List<Models.Company>();

            foreach (Company field in businessCollection)
            {
                companies.Add(ModelAssembler.CreateCompany(field));
            }

            return companies;
        }

        #endregion

        #region Agency

        public static Models.Agency CreateAgency(AgentAgency agency)
        {
            return new Models.Agency
            {
                Id = agency.AgentAgencyId,
                Code = agency.AgentCode,
                FullName = agency.Description,
                DateDeclined = agency.DeclinedDate,
                Branch = new modelsCommon.Branch
                {
                    Id = agency.BranchCode
                },
                Agent = new Models.Agent
                {
                    IndividualId = agency.IndividualId
                },
                AgentType = new Models.AgentType
                {
                    Id = agency.AgentTypeCode,
                    Description = agency.Description
                },
                AgentDeclinedType = new Models.AgentDeclinedType
                {
                    Id = agency.AgentDeclinedTypeCode
                }
            };
        }

        public static List<Models.Agency> CreateAgencies(BusinessCollection businessCollection)
        {
            List<Models.Agency> agencies = new List<Models.Agency>();
            foreach (AgentAgency field in businessCollection)
            {
                agencies.Add(ModelAssembler.CreateAgency(field));

            }
            return agencies;
        }
        public static List<Models.Agency> CreateAgentcies(List<AgentAgency> agenciesEntities)
        {
            var imaper = CreateMapAgencies();
            return imaper.Map<List<AgentAgency>, List<Models.Agency>>(agenciesEntities);
        }

        #endregion

        #region Insured

        public static Models.Insured CreateInsured(Insured insured)
        {
            var imaper = CreateMapInsured();
            return imaper.Map<Insured, Models.Insured>(insured);
        }

        /// <summary>
        /// Creates the insureds.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.Insured> CreateInsureds(BusinessCollection businessCollection)
        {
            if (businessCollection != null && businessCollection.Any())
            {
                var imaper = CreateMapInsured();
                var insureds = businessCollection.Cast<Insured>().ToList();
                return imaper.Map<List<Insured>, List<Models.Insured>>(insureds);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region MaritalStatus

        public static Models.MaritalStatus CreateMaritalState(MaritalStatus maritalStatus)
        {
            return new Models.MaritalStatus
            {
                Id = maritalStatus.MaritalStatusCode,
                Description = maritalStatus.Description,
                SmallDescription = maritalStatus.SmallDescription
            };
        }

        public static List<Models.MaritalStatus> CreateMaritalStatus(BusinessCollection businessCollection)
        {
            List<Models.MaritalStatus> maritalStatus = new List<Models.MaritalStatus>();
            foreach (MaritalStatus field in businessCollection)
            {
                maritalStatus.Add(CreateMaritalState(field));
            }
            return maritalStatus;
        }

        #endregion

        #region Individual

        public static Models.Individual CreateIndividual(Individual individual)
        {
            modelsPerson.EconomicActivity activity = new modelsPerson.EconomicActivity();


            activity.Id = Convert.ToInt32(individual.EconomicActivityCode);


            return new Models.Individual
            {
                IndividualId = individual.IndividualId,
                IndividualType = (Enums.IndividualType)individual.IndividualTypeCode,
                EconomicActivity = activity


            };
        }

        public static List<Models.Individual> CreateIndividuals(BusinessCollection businessCollection)
        {
            List<Models.Individual> individuals = new List<Models.Individual>();
            foreach (Individual field in businessCollection)
            {
                individuals.Add(ModelAssembler.CreateIndividual(field));
            }
            return individuals;
        }

        #endregion

        #region IdentityCardType

        public static Models.DocumentType CreateIdentityCardType(IdentityCardType identityCardType)
        {
            return new Models.DocumentType
            {
                Id = identityCardType.IdCardTypeCode,
                Description = identityCardType.Description,
                SmallDescription = identityCardType.SmallDescription
            };
        }

        public static List<Models.DocumentType> CreateIdentityCardTypes(BusinessCollection businessCollection)
        {
            List<Models.DocumentType> documentTypes = new List<Models.DocumentType>();
            foreach (IdentityCardType field in businessCollection)
            {
                documentTypes.Add(ModelAssembler.CreateIdentityCardType(field));
            }
            return documentTypes;
        }

        #endregion

        #region TributaryIdentityType

        public static Models.DocumentType CreateTributaryIdentityType(TributaryIdentityType tributaryIdentityType)
        {
            return new Models.DocumentType
            {
                Id = tributaryIdentityType.TributaryIdTypeCode,
                Description = tributaryIdentityType.Description,
                SmallDescription = tributaryIdentityType.SmallDescription
            };
        }

        public static List<Models.DocumentType> CreateTributaryIdentityTypes(BusinessCollection businessCollection)
        {
            List<Models.DocumentType> documentTypes = new List<Models.DocumentType>();
            foreach (TributaryIdentityType field in businessCollection)
            {
                documentTypes.Add(ModelAssembler.CreateTributaryIdentityType(field));
            }
            return documentTypes;
        }

        #endregion

        #region CoAssociationType

        public static Models.AssociationType CreateAssociationType(CoAssociationType coAssociationType)
        {
            return new Models.AssociationType
            {
                Id = coAssociationType.AssociationTypeCode,
                Description = coAssociationType.Description
            };
        }

        public static List<Models.AssociationType> CreateAssociationTypes(BusinessCollection businessCollection)
        {
            List<Models.AssociationType> associationTypes = new List<Models.AssociationType>();

            foreach (CoAssociationType field in businessCollection)
            {
                associationTypes.Add(ModelAssembler.CreateAssociationType(field));
            }

            return associationTypes;
        }

        #endregion

        #region CompanyType

        public static Models.CompanyType CreateCompanyType(CompanyType companyType)
        {
            return new Models.CompanyType
            {
                Id = companyType.CompanyTypeCode,
                Description = companyType.Description
            };
        }

        public static List<Models.CompanyType> CreateCompanyTypes(BusinessCollection businessCollection)
        {
            List<Models.CompanyType> companyTypes = new List<Models.CompanyType>();
            foreach (CompanyType field in businessCollection)
            {
                companyTypes.Add(ModelAssembler.CreateCompanyType(field));
            }
            return companyTypes;
        }

        #endregion

        #region AGencyPrefix

        public static Models.AgentPrefix CreateAgentPrefix(AgentPrefix prefix)
        {
            return new Models.AgentPrefix
            {
                Id = prefix.IndividualId,
                prefix = new modelsCommon.Prefix() { Id = prefix.PrefixCode }

            };
        }

        public static List<Models.AgentPrefix> CreateAgentPrefixs(BusinessCollection businessCollection)
        {
            List<Models.AgentPrefix> agentPrefix = new List<Models.AgentPrefix>();

            foreach (AgentPrefix field in businessCollection)
            {
                agentPrefix.Add(ModelAssembler.CreateAgentPrefix(field));
            }

            return agentPrefix;
        }
        public static modelsCommon.Prefix CreatePrefix(AgentPrefix prefix)
        {
            return new modelsCommon.Prefix
            {
                Id = prefix.PrefixCode
            };
        }
        #endregion

        #region InsuredDeclinedType
        public static Models.InsuredDeclinedType CreateInsuredDeclinedType(InsuredDeclinedType insuredDeclinedType)
        {
            return new Models.InsuredDeclinedType
            {
                Id = insuredDeclinedType.InsDeclinedTypeCode,
                Description = insuredDeclinedType.Description,
                SmallDescription = insuredDeclinedType.SmallDescription,
            };
        }
        public static List<Models.InsuredDeclinedType> CreateInsuredDeclinedTypes(BusinessCollection businessCollection)
        {
            List<Models.InsuredDeclinedType> insuredDeclinedTypes = new List<Models.InsuredDeclinedType>();
            foreach (InsuredDeclinedType field in businessCollection)
            {
                insuredDeclinedTypes.Add(ModelAssembler.CreateInsuredDeclinedType(field));
            }
            return insuredDeclinedTypes;
        }
        #endregion

        #region ProspectusNatural
        public static Models.Company CreatePersonProspectlegal(Prospect prospectLegal)
        {
            Models.IdentificationDocument identificationDocument = new Models.IdentificationDocument
            {
                DocumentType = new Models.DocumentType { Id = Convert.ToInt32(prospectLegal.TributaryIdTypeCode) },
                Number = prospectLegal.TributaryIdNo,
            };

            modelsCommon.City city = new modelsCommon.City();
            if (prospectLegal.CityCode != null)
            {
                modelsCommon.Country country = new modelsCommon.Country();
                country.Id = prospectLegal.CountryCode.Value;

                modelsCommon.State state = new modelsCommon.State();
                state.Id = prospectLegal.StateCode.Value;
                state.Country = country;

                city.Id = prospectLegal.CityCode.Value;
                city.State = state;
            }


            Models.CompanyType companyType = new Models.CompanyType
            {
                Id = Convert.ToInt32(prospectLegal.CompanyTypeCode)
            };

            List<Models.Address> address = new List<Models.Address>();
            address.Add(new Models.Address { Id = Convert.ToInt32(prospectLegal.AddressTypeCode), Description = prospectLegal.Street });
            address[0].City = city;

            List<Models.Email> emails = new List<Models.Email>();
            emails.Add(new Models.Email { Description = prospectLegal.EmailAddress });

            List<Models.Phone> phones = new List<Models.Phone>();
            phones.Add(new Models.Phone { Description = prospectLegal.PhoneNumber.ToString() });

            return new Models.Company
            {
                IndividualId = prospectLegal.ProspectId,
                Name = prospectLegal.TradeName,
                IdentificationDocument = identificationDocument,
                CompanyType = companyType,
                Addresses = address,
                Phones = phones,
                Emails = emails
            };
        }

        public static List<Models.Company> CreatePersonProspectsLegals(BusinessCollection businessCollection)
        {
            List<Models.Company> prospects = new List<Models.Company>();
            foreach (Prospect item in businessCollection)
            {
                prospects.Add(ModelAssembler.CreatePersonProspectlegal(item));
            }
            return prospects;
        }
        public static Models.Person CreatePersonProspect(Prospect prospectNatural)
        {

            List<Models.Email> emails = new List<Models.Email>();
            emails.Add(new Models.Email { Description = prospectNatural.EmailAddress });
            List<Models.Phone> phones = new List<Models.Phone>();
            phones.Add(new Models.Phone { Description = prospectNatural.PhoneNumber.ToString() });
            List<Models.Address> address = new List<Models.Address>();
            address.Add(new Models.Address { Id = Convert.ToInt32(prospectNatural.AddressTypeCode), Description = prospectNatural.Street });

            modelsCommon.City city = new modelsCommon.City();
            if (prospectNatural.CityCode != null)
            {
                modelsCommon.Country country = new modelsCommon.Country();
                country.Id = prospectNatural.CountryCode.Value;

                modelsCommon.State state = new modelsCommon.State();
                state.Id = prospectNatural.StateCode.Value;
                state.Country = country;

                city.Id = prospectNatural.CityCode.Value;
                city.State = state;
            }

            return new Models.Person
            {
                PersonCode = Convert.ToInt32(prospectNatural.ProspectId),
                Gender = prospectNatural.Gender,
                BirthDate = Convert.ToDateTime(prospectNatural.BirthDate),
                Names = prospectNatural.Name,
                IdentificationDocument = new Models.IdentificationDocument() { DocumentType = new Models.DocumentType() { Id = Convert.ToInt32(prospectNatural.IdCardTypeCode) }, Number = prospectNatural.IdCardNo },
                Surname = prospectNatural.Surname,
                MotherLastName = prospectNatural.MotherLastName,
                Name = prospectNatural.Surname + " " + prospectNatural.MotherLastName + " " + prospectNatural.Name,
                MaritalStatus = new Models.MaritalStatus() { Id = Convert.ToInt32(prospectNatural.MaritalStatusCode) },
                City = city,
                Phones = phones,
                Emails = emails,
                Addresses = address
            };

        }
        public static List<Models.Person> CreatePersonProspects(BusinessCollection businessCollection)
        {
            List<Models.Person> prospects = new List<Models.Person>();
            foreach (Prospect item in businessCollection)
            {
                prospects.Add(ModelAssembler.CreatePersonProspect(item));
            }
            return prospects;
        }

        public static Models.ProspectNatural CreateModelProspect(Prospect prospectNatural)
        {
            modelsCommon.City city = null;
            if (prospectNatural.CityCode != null)
            {
                city = new modelsCommon.City();
                city.Id = prospectNatural.CityCode.Value;
                city.State = new modelsCommon.State
                {
                    Country = new modelsCommon.Country { Id = prospectNatural.CountryCode.Value },
                    Id = (int)prospectNatural.StateCode
                };

                city = DelegateService.commonServiceCore.GetCityByCity(city);
            }
            return new Models.ProspectNatural
            {
                AdditionalInfo = prospectNatural.AdditionalInfo,
                ProspectCode = prospectNatural.ProspectId,
                IndividualTyepCode = prospectNatural.IndividualTypeCode,
                CountryCode = Convert.ToInt32(prospectNatural.CountryCode),
                StateCode = Convert.ToInt32(prospectNatural.StateCode),
                Surname = prospectNatural.Surname,
                Name = prospectNatural.TradeName != null ? prospectNatural.TradeName : prospectNatural.Name,
                Gender = prospectNatural.Gender,
                MaritalStatus = Convert.ToInt32(prospectNatural.MaritalStatusCode),
                BirthDate = Convert.ToDateTime(prospectNatural.BirthDate),
                CityCode = Convert.ToInt32(prospectNatural.CityCode),
                IdCardTypeCode = Convert.ToInt32(prospectNatural.IdCardTypeCode),
                IdCardNo = prospectNatural.IdCardNo,
                AddressType = Convert.ToInt32(prospectNatural.AddressTypeCode),
                EmailAddress = prospectNatural.EmailAddress,
                PhoneNumber = Convert.ToInt64(prospectNatural.PhoneNumber),
                MotherLastName = prospectNatural.MotherLastName,
                TributaryIdTypeCode = prospectNatural.TributaryIdTypeCode,
                TributaryIdNumber = prospectNatural.TributaryIdNo,
                Street = prospectNatural.Street,
                City = city
            };



        }

        public static List<Models.ProspectNatural> CreateNaturalProspects(BusinessCollection businessCollection)
        {
            List<Models.ProspectNatural> prospectusNaturals = new List<Models.ProspectNatural>();
            foreach (Prospect field in businessCollection)
            {
                prospectusNaturals.Add(ModelAssembler.CreateModelProspect(field));
            }
            return prospectusNaturals;
        }

        public static Models.ProspectNatural CreateProspectNaturalPerson(Models.Person person)
        {

            return new Models.ProspectNatural
            {
                IndividualId = person.IndividualId,
                IndividualTypePerson = (int)person.IndividualType,
                Surname = person.Surname,
                Name = person.Surname + person.Name,
                MotherLastName = person.MotherLastName,
                IdCardNo = person.IdentificationDocument.Number
            };
        }

        public static Models.ProspectNatural CreateProspectNaturalCompany(Models.Company company)
        {

            return new Models.ProspectNatural
            {
                IdCardNo = company.IdentificationDocument.Number,
                IndividualId = company.IndividualId,
                IndividualTypePerson = (int)company.IndividualType,
                Name = company.Name,

            };
        }

        public static List<Models.ProspectNatural> CreateProspectsPersonCompany(List<Models.Person> persons, List<Models.Company> companies)
        {
            List<Models.ProspectNatural> prospectLegals = new List<Models.ProspectNatural>();
            foreach (Models.Person field in persons)
            {
                prospectLegals.Add(ModelAssembler.CreateProspectNaturalPerson(field));
            }
            foreach (Models.Company field in companies)
            {
                prospectLegals.Add(ModelAssembler.CreateProspectNaturalCompany(field));
            }

            return prospectLegals;
        }

        #endregion

        #region AgenType
        public static Models.AgentType CreateAgentType(AgentType agentType)
        {
            return new Models.AgentType
            {
                Id = agentType.AgentTypeCode,
                Description = agentType.Description,
                SmallDescription = agentType.SmallDescription
            };

        }
        public static List<Models.AgentType> CreateAgentTypes(BusinessCollection businessCollection)
        {
            List<Models.AgentType> agentType = new List<Models.AgentType>();

            foreach (AgentType field in businessCollection)
            {
                agentType.Add(ModelAssembler.CreateAgentType(field));
            }

            return agentType;

        }
        #endregion

        #region CoCompany
        public static Models.CompanyExtended CreateCoCompany(CoCompany coCompany)
        {
            return new Models.CompanyExtended
            {
                VerifyDigit = coCompany.VerifyDigit,
                AssociationType = new Models.AssociationType { Id = coCompany.AssociationTypeCode }
            };
        }
        public static List<Models.CompanyExtended> CreateCoCompanies(BusinessCollection businessCollection)
        {
            List<Models.CompanyExtended> coCompanies = new List<Models.CompanyExtended>();
            foreach (CoCompany field in businessCollection)
            {
                coCompanies.Add(ModelAssembler.CreateCoCompany(field));

            }
            return coCompanies;
        }
        #endregion
        #region PersonIndividualType
        public static Models.PersonIndividualType CreatePersonIndividualType(PersonIndividualType personIndividualType)
        {
            return new Models.PersonIndividualType
            {
                PersonTypeCode = personIndividualType.PersonTypeCode
            };

        }
        public static List<Models.PersonIndividualType> CreatePersonIndividualTypes(BusinessCollection businessCollection)
        {
            List<Models.PersonIndividualType> personIndividualTypes = new List<Models.PersonIndividualType>();
            foreach (PersonIndividualType field in businessCollection)
            {
                personIndividualTypes.Add(ModelAssembler.CreatePersonIndividualType(field));
            }
            return personIndividualTypes;
        }

        #endregion

        #region CompanyName
        public static Models.CompanyName CreateCompanyName(CoCompanyName coCompanyName)
        {
            return new Models.CompanyName
            {
                IndividualId = coCompanyName.IndividualId,
                NameNum = coCompanyName.NameNum,
                TradeName = coCompanyName.TradeName,
                Address = new Models.Address { Id = coCompanyName.AddressDataCode == null ? 0 : (Int32)coCompanyName.AddressDataCode },
                Phone = new Models.Phone { Id = coCompanyName.PhoneDataCode == null ? 0 : (Int32)coCompanyName.PhoneDataCode },
                Email = new Models.Email { Id = coCompanyName.EmailDataCode == null ? 0 : (Int32)coCompanyName.EmailDataCode },
                IsMain = coCompanyName.IsMain
            };
        }
        public static List<Models.CompanyName> CreateCompaniesName(BusinessCollection businessCollection)
        {
            List<Models.CompanyName> coCompaniesName = new List<Models.CompanyName>();
            foreach (CoCompanyName field in businessCollection)
            {
                coCompaniesName.Add(ModelAssembler.CreateCompanyName(field));

            }
            return coCompaniesName;
        }
        #endregion

        #region InsuredPersonsAndCompanies

        public static Models.Insured CreateInsuredPerson(Person person)
        {
            Models.Insured insured = new Models.Insured
            {
                IndividualId = person.IndividualId,
                Name = person.Surname,
                EconomicActivity = new modelsPerson.EconomicActivity { Id = person.EconomicActivityCode.Value },
                BirthDate = person.BirthDate,
                CustomerType = Enums.CustomerType.Individual
            };
            if (person.MotherLastName != null)
            {
                insured.Name += " " + person.MotherLastName;
            }
            insured.Name += " " + person.Name;

            return insured;
        }

        public static Models.Insured CreateInsuredCompany(Company company)
        {
            return new Models.Insured
            {
                IndividualId = company.IndividualId,
                Name = company.TradeName,
                EconomicActivity = new modelsPerson.EconomicActivity { Id = company.EconomicActivityCode.Value },
                CustomerType = Enums.CustomerType.Individual
            };
        }

        public static List<Models.Insured> CreateInsuredPersonsAndCompanies(BusinessCollection collectionPersons, BusinessCollection collectionCompanies)
        {
            List<Models.Insured> insureds = new List<Models.Insured>();
            foreach (Person item in collectionPersons)
            {
                insureds.Add(ModelAssembler.CreateInsuredPerson(item));
            }
            foreach (Company item in collectionCompanies)
            {
                insureds.Add(ModelAssembler.CreateInsuredCompany(item));
            }

            return insureds;
        }

        #endregion

        #region InsuredConcept
        public static Models.InsuredConcept CreateInsuredConcept(InsuredConcept insuredConcept)
        {
            return new Models.InsuredConcept
            {
                IsInsured = insuredConcept.IsInsured,
                IsHolder = insuredConcept.IsHolder,
                IsBeneficiary = insuredConcept.IsBeneficiary,
                IsConsortium = insuredConcept.IsConsortium == null ? false : insuredConcept.IsConsortium,
                IsPayer = insuredConcept.IsPayer,
                IsRepresentative = insuredConcept.IsRepresentative == null ? false : insuredConcept.IsRepresentative,
                IsSurety = insuredConcept.IsSurety == null ? false : insuredConcept.IsSurety

            };
        }
        public static List<Models.InsuredConcept> CreateInsuredConcepts(BusinessCollection businessCollection)
        {
            List<Models.InsuredConcept> insuredConcept = new List<Models.InsuredConcept>();
            foreach (InsuredConcept field in businessCollection)
            {
                insuredConcept.Add(ModelAssembler.CreateInsuredConcept(field));

            }
            return insuredConcept;
        }
        #endregion

        #region PaymentMethod

        public static Models.IndividualPaymentMethod CreatePaymentMethod(IndividualPaymentMethod paymentMethod)
        {
            return new Models.IndividualPaymentMethod
            {
                Id = paymentMethod.PaymentMethodCode,
                PaymentId = paymentMethod.PaymentId,
                IndividualId = paymentMethod.IndividualId,
                RoleId = paymentMethod.RoleCode,
                Enabled = paymentMethod.Enabled
            };
        }
        public static List<Models.IndividualPaymentMethod> CreatePaymentMethods(BusinessCollection businessCollection)
        {
            List<Models.IndividualPaymentMethod> individualPaymentMethod = new List<Models.IndividualPaymentMethod>();

            foreach (IndividualPaymentMethod field in businessCollection)
            {
                individualPaymentMethod.Add(ModelAssembler.CreatePaymentMethod(field));
            }

            return individualPaymentMethod;
        }
        #endregion


        #region Guarantee

        public static Models.Guarantee CreateGuarantee(Guarantee guarantee, InsuredGuaranteeView insuredGuaranteeView)
        {
            Models.GuaranteeType guaranteeType = null;
            Models.InsuredGuarantee insuredGuarantee = null;
            Models.GuaranteeStatus guaranteeStatus = null;

            foreach (Common.Entities.GuaranteeType gt in insuredGuaranteeView.GuaranteeTypes)
            {
                if (gt.GuaranteeTypeCode == guarantee.GuaranteeTypeCode)
                {
                    guaranteeType = new Models.GuaranteeType();
                    guaranteeType = CreateGuaranteeType(gt);
                    break;
                }
            }

            foreach (InsuredGuarantee ig in insuredGuaranteeView.InsuredGuarantees)
            {
                if (ig.GuaranteeCode == guarantee.GuaranteeCode)
                {
                    insuredGuarantee = new Models.InsuredGuarantee();
                    foreach (GuaranteeStatus gs in insuredGuaranteeView.GuaranteeStates)
                    {
                        if (gs.GuaranteeStatusCode == ig.GuaranteeStatusCode)
                        {
                            guaranteeStatus = new Models.GuaranteeStatus { Code = gs.GuaranteeStatusCode, Description = gs.Description, IsEnabledInd = gs.EnabledInd, IsRemoveInd = gs.RemoveInd, IsValidateDocument = gs.ValidateDocument };
                            insuredGuarantee = CreateInsuredGuarantee(ig, guaranteeStatus);
                            break;
                        }
                    }
                    break;
                }
            }

            return new Models.Guarantee
            {
                Code = guarantee.GuaranteeCode,
                Description = guarantee.Description,
                Apostille = guarantee.Apostille,
                GuaranteeType = guaranteeType,
                InsuredGuarantee = insuredGuarantee
            };
        }

        public static List<Models.Guarantee> CreateGuarantees(InsuredGuaranteeView businessCollection)
        {
            List<Models.Guarantee> guarantees = new List<Models.Guarantee>();

            foreach (Guarantee field in businessCollection.Guarantees)
            {
                guarantees.Add(ModelAssembler.CreateGuarantee(field, businessCollection));
            }

            return guarantees;
        }

        public static Models.Guarantee CreateGuarantee(Guarantee guarantee, GuaranteeView guaranteeView)
        {
            Models.GuaranteeType guaranteeType = null;

            foreach (Common.Entities.GuaranteeType gt in guaranteeView.GuaranteeTypes)
            {
                if (gt.GuaranteeTypeCode == guarantee.GuaranteeTypeCode)
                {
                    guaranteeType = new Models.GuaranteeType();
                    guaranteeType = CreateGuaranteeType(gt);
                    break;
                }
            }

            return new Models.Guarantee
            {
                Code = guarantee.GuaranteeCode,
                Description = guarantee.Description,
                Apostille = guarantee.Apostille,
                GuaranteeType = guaranteeType
            };

        }

        public static List<Models.Guarantee> CreateGuarantees(GuaranteeView businessCollection)
        {
            List<Models.Guarantee> guarantees = new List<Models.Guarantee>();

            foreach (Guarantee field in businessCollection.Guarantees)
            {
                guarantees.Add(ModelAssembler.CreateGuarantee(field, businessCollection));
            }

            return guarantees;
        }

        public static Models.GuaranteeType CreateGuaranteeType(Common.Entities.GuaranteeType guaranteeType)
        {
            return new Models.GuaranteeType
            {
                Code = guaranteeType.GuaranteeTypeCode,
                Description = guaranteeType.Description
            };
        }

        public static List<Models.GuaranteeType> CreateGuaranteeTypes(BusinessCollection businessCollection)
        {
            List<Models.GuaranteeType> guaranteeTypes = new List<Models.GuaranteeType>();

            foreach (Common.Entities.GuaranteeType field in businessCollection)
            {
                guaranteeTypes.Add(ModelAssembler.CreateGuaranteeType(field));
            }
            return guaranteeTypes;
        }

        public static Models.InsuredGuarantee CreateInsuredGuarantee(InsuredGuarantee insuredGuarantee, Models.GuaranteeStatus guaranteeStatus)
        {
            return new Models.InsuredGuarantee
            {
                Id = insuredGuarantee.GuaranteeId,
                IndividualId = insuredGuarantee.IndividualId,
                ValueAmount = insuredGuarantee.InsuranceValueAmount,
                GuaranteeStatus = guaranteeStatus,
                Address = insuredGuarantee.Address,
                Code = insuredGuarantee.GuaranteeCode,
                AppraisalAmount = insuredGuarantee.AppraisalAmount,
                BuiltArea = insuredGuarantee.BuiltAreaQuantity,
                DeedNumber = insuredGuarantee.DeedNumber,
                DescriptionOthers = insuredGuarantee.GuaranteeDescriptionOthers,
                DocumentValueAmount = insuredGuarantee.DocumentValueAmount,
                MeasureArea = insuredGuarantee.MeasureAreaQuantity,
                RegistrationDate = insuredGuarantee.RegistrationDate,
                MortgagerName = insuredGuarantee.MortgagerName,
                DepositEntity = insuredGuarantee.DepositEntity,
                DepositDate = insuredGuarantee.DepositDate,
                Depositor = insuredGuarantee.Depositor,
                Constituent = insuredGuarantee.Constituent,
                Description = insuredGuarantee.Description,
                AppraisalDate = insuredGuarantee.AppraisalDate,
                ExpertName = insuredGuarantee.ExpertName,
                InsuranceAmount = insuredGuarantee.InsuranceValueAmount,
                PolicyNumber = insuredGuarantee.GuaranteePolicyNumber,
                Apostille = insuredGuarantee.Apostille,
                IssuerName = insuredGuarantee.IssuerName,
                DocumentNumber = insuredGuarantee.DocumentNumber,
                ExpirationDate = insuredGuarantee.ExpDate,
                BusinessLineCode = insuredGuarantee.LineBusinessCode,
                RegistrationNumber = insuredGuarantee.RegistrationNumber,
                LicensePlate = insuredGuarantee.LicensePlate,
                EngineNro = insuredGuarantee.EngineSerNro,
                ChassisNro = insuredGuarantee.ChassisSerNo,
                SignatoriesNumber = insuredGuarantee.SignatoriesNum,
                GuaranteeAmount = insuredGuarantee.GuaranteeAmount,
                LastChangeDate = insuredGuarantee.LastChangeDate,
                VehicleMake = (short?)insuredGuarantee.VehicleMakeCode,
                VehicleModel = insuredGuarantee.VehicleModelCode,
                VehicleVersion = (short?)insuredGuarantee.VehicleVersionCode,
                AssetTypeCode = insuredGuarantee.AssetTypeCode,
                RealstateMatriculation = insuredGuarantee.RealstateMatriculation,
                ConstitutionDate = insuredGuarantee.ConstitutionDate,
            };
        }

        #endregion

        #region InsuredGuaranteePrefix
        public static Models.InsuredGuaranteePrefix CreateInsuredGuaranteePrefix(InsuredGuaranteePrefix insuredGuaranteePrefix)
        {
            return new Models.InsuredGuaranteePrefix
            {
                IndividualId = insuredGuaranteePrefix.IndividualId,
                GuaranteeId = insuredGuaranteePrefix.GuaranteeId,
                PrefixCode = insuredGuaranteePrefix.PrefixCode
            };
        }

        public static List<Models.InsuredGuaranteePrefix> CreateInsuredGuaranteePrefixes(BusinessCollection businessCollection)
        {
            List<Models.InsuredGuaranteePrefix> insuredGuaranteePrefixes = new List<Models.InsuredGuaranteePrefix>();
            foreach (InsuredGuaranteePrefix field in businessCollection)
            {
                insuredGuaranteePrefixes.Add(ModelAssembler.CreateInsuredGuaranteePrefix(field));

            }
            return insuredGuaranteePrefixes;
        }
        #endregion

        #region InsuredGuaranteeDocumentation
        public static Models.InsuredGuaranteeDocumentation CreateInsuredGuaranteeDocumentation(InsuredGuaranteeDocumentation insuredGuaranteeDocumentation)
        {
            return new Models.InsuredGuaranteeDocumentation
            {
                IndividualId = insuredGuaranteeDocumentation.IndividualId,
                GuaranteeId = insuredGuaranteeDocumentation.GuaranteeId,
                GuaranteeCode = insuredGuaranteeDocumentation.GuaranteeCode,
                DocumentCode = insuredGuaranteeDocumentation.DocumentCode
            };
        }

        public static List<Models.InsuredGuaranteeDocumentation> CreateInsuredGuaranteeDocumentations(BusinessCollection businessCollection)
        {
            List<Models.InsuredGuaranteeDocumentation> InsuredGuaranteeDocumentations = new List<Models.InsuredGuaranteeDocumentation>();
            foreach (InsuredGuaranteeDocumentation field in businessCollection)
            {
                InsuredGuaranteeDocumentations.Add(ModelAssembler.CreateInsuredGuaranteeDocumentation(field));

            }
            return InsuredGuaranteeDocumentations;
        }
        #endregion

        #region GuaranteeStatus

        private static Models.GuaranteeStatus CreateGuaranteeStatus(GuaranteeStatus guaranteeStauts)
        {
            return new Models.GuaranteeStatus
            {
                Code = guaranteeStauts.GuaranteeStatusCode,
                Description = guaranteeStauts.Description
            };
        }

        public static List<Models.GuaranteeStatus> CreateGuaranteesStatus(BusinessCollection businessCollection)
        {
            List<Models.GuaranteeStatus> guaranteeStauts = new List<Models.GuaranteeStatus>();

            foreach (GuaranteeStatus field in businessCollection)
            {
                guaranteeStauts.Add(ModelAssembler.CreateGuaranteeStatus(field));
            }

            return guaranteeStauts;
        }

        #endregion

        #region PromissoryNoteType

        /// <summary>
        /// Creates the type of the promissory note.
        /// </summary>
        /// <param name="promissoryNoteType">Type of the promissory note.</param>
        /// <returns></returns>
        private static Models.PromissoryNoteType CreatePromissoryNoteType(PromissoryNoteType promissoryNoteType)
        {
            return new Models.PromissoryNoteType
            {
                Id = promissoryNoteType.PromissoryNoteTypeCode,
                Description = promissoryNoteType.Description
            };
        }

        /// <summary>
        /// Creates the promissory note types.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.PromissoryNoteType> CreatePromissoryNoteTypes(BusinessCollection businessCollection)
        {
            List<Models.PromissoryNoteType> promissoryNoteType = new List<Models.PromissoryNoteType>();

            foreach (PromissoryNoteType field in businessCollection)
            {
                promissoryNoteType.Add(ModelAssembler.CreatePromissoryNoteType(field));
            }

            return promissoryNoteType;
        }

        #endregion

        #region MeasurementType

        /// <summary>
        /// Creates the type of the measurement.
        /// </summary>
        /// <param name="measurementType">Type of the measurement.</param>
        /// <returns></returns>
        private static Models.MeasurementType CreateMeasurementType(MeasurementType measurementType)
        {
            return new Models.MeasurementType
            {
                Code = measurementType.MeasurementTypeCode,
                Description = measurementType.SmallDescription
            };
        }

        /// <summary>
        /// Creates the measurement types.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.MeasurementType> CreateMeasurementTypes(BusinessCollection businessCollection)
        {
            List<Models.MeasurementType> guaranteeStauts = new List<Models.MeasurementType>();

            foreach (MeasurementType field in businessCollection)
            {
                guaranteeStauts.Add(ModelAssembler.CreateMeasurementType(field));
            }

            return guaranteeStauts;
        }

        #endregion     

        #region Prospect

        /// <summary>
        /// Creates the prospect by person.
        /// </summary>
        /// <param name="person">The person.</param>
        /// <returns></returns>
        public static Models.Prospect CreateProspectByPerson(Person person)
        {
            return new Models.Prospect
            {
                Id = person.IndividualId,
                IndividualType = Enums.IndividualType.Person,
                CustomerType = Enums.CustomerType.Individual,
                Surname = person.Surname,
                SecondSurname = person.MotherLastName,
                Name = person.Name,
                Gender = person.Gender,
                IdentificationDocument = new Models.IdentificationDocument
                {
                    Number = person.IdCardNo,
                    DocumentType = new Models.DocumentType
                    {
                        Id = person.IdCardTypeCode
                    }
                },
                BirthDate = person.BirthDate
            };
        }

        /// <summary>
        /// Creates the prospect by company.
        /// </summary>
        /// <param name="company">The company.</param>
        /// <returns></returns>
        public static Models.Prospect CreateProspectByCompany(Company company)
        {
            return new Models.Prospect
            {
                Id = company.IndividualId,
                IndividualType = Enums.IndividualType.LegalPerson,
                CustomerType = Enums.CustomerType.Individual,
                TradeName = company.TradeName,
                IdentificationDocument = new Models.IdentificationDocument
                {
                    Number = company.TributaryIdNo,
                    DocumentType = new Models.DocumentType
                    {
                        Id = company.TributaryIdTypeCode
                    }
                }
            };
        }

        /// <summary>
        /// Creates the prospect.
        /// </summary>
        /// <param name="entityProspect">The entity prospect.</param>
        /// <returns></returns>
        public static Models.Prospect CreateProspect(Prospect entityProspect)
        {
            Models.Prospect prospect = new Models.Prospect
            {
                Id = entityProspect.ProspectId,
                IndividualType = (Enums.IndividualType)entityProspect.IndividualTypeCode,
                CustomerType = Enums.CustomerType.Prospect,
                CompanyName = new Models.CompanyName
                {
                    Address = new Models.Address
                    {
                        AddressType = new Models.AddressType
                        {
                            Id = entityProspect.AddressTypeCode.GetValueOrDefault()
                        },
                        Description = entityProspect.Street
                    },
                    Phone = new Models.Phone
                    {
                        Description = entityProspect.PhoneNumber.HasValue ? entityProspect.PhoneNumber.Value.ToString() : ""
                    },
                    Email = new Models.Email
                    {
                        Description = entityProspect.EmailAddress
                    }
                }
            };

            if (entityProspect.CityCode.HasValue)
            {
                prospect.CompanyName.Address.City = new modelsCommon.City
                {
                    Id = entityProspect.CityCode.GetValueOrDefault(),
                    State = new modelsCommon.State
                    {
                        Id = entityProspect.StateCode.GetValueOrDefault(),
                        Country = new modelsCommon.Country
                        {
                            Id = entityProspect.CountryCode.GetValueOrDefault()
                        }
                    }
                };
            }

            if (prospect.IndividualType == Enums.IndividualType.Person)
            {
                prospect.Surname = entityProspect.Surname;
                prospect.SecondSurname = entityProspect.MotherLastName;
                prospect.Name = entityProspect.Name;
                prospect.Gender = entityProspect.Gender;
                prospect.BirthDate = entityProspect.BirthDate.HasValue ? entityProspect.BirthDate.Value : DateTime.MinValue;
                prospect.MaritalStatus = entityProspect.MaritalStatusCode.GetValueOrDefault();
                prospect.IdentificationDocument = new Models.IdentificationDocument
                {
                    Number = entityProspect.IdCardNo,
                    DocumentType = new Models.DocumentType
                    {
                        Id = entityProspect.IdCardTypeCode.GetValueOrDefault()
                    }
                };
            }
            else if (prospect.IndividualType == Enums.IndividualType.LegalPerson)
            {
                prospect.TradeName = entityProspect.TradeName;
                prospect.IdentificationDocument = new Models.IdentificationDocument
                {
                    Number = entityProspect.TributaryIdNo,
                    DocumentType = new Models.DocumentType
                    {
                        Id = entityProspect.TributaryIdTypeCode.GetValueOrDefault()
                    }
                };
            }

            return prospect;
        }

        public static List<Models.Prospect> CreateProspects(BusinessCollection businessCollection)
        {
            List<Models.Prospect> prospects = new List<Models.Prospect>();

            foreach (Prospect entityProspect in businessCollection)
            {
                prospects.Add(ModelAssembler.CreateProspect(entityProspect));
            }

            return prospects;
        }
        /// <summary>
        /// CreateDistinctProspects
        /// El distinct se debe implementar desde la consulta del DAF, se realiza de este modo por que no se ha implementado la funcionalidad.
        /// </summary>
        /// <param name="businessCollection">businessCollection.</param>
        /// <returns>List Prospect </returns>
        public static List<Models.Prospect> CreateDistinctProspects(BusinessCollection businessCollection)
        {
            List<Models.Prospect> prospects = new List<Models.Prospect>();

            foreach (Prospect entityProspect in businessCollection)
            {
                string document = entityProspect.IdCardNo == "" ? entityProspect.TributaryIdNo : entityProspect.IdCardNo;
                Models.Prospect prospect = prospects.Where(x => x.IdentificationDocument.Number == document).FirstOrDefault();
                if (prospect == null)
                {
                    prospects.Add(ModelAssembler.CreateProspect(entityProspect));
                }
            }

            return prospects;
        }
        public static List<Models.Prospect> CreateDistinctProspectsByPerson(BusinessCollection businessCollection)
        {
            List<Models.Prospect> prospects = new List<Models.Prospect>();

            foreach (Person entityPerson in businessCollection)
            {
                int document = entityPerson.IndividualId;
                Models.Prospect prospect = prospects.Where(x => x.Id == document).FirstOrDefault();
                if (prospect == null)
                {
                    prospects.Add(ModelAssembler.CreateProspectByPerson(entityPerson));
                }
            }

            return prospects;
        }
        public static List<Models.Prospect> CreateDistinctProspectsByCompany(BusinessCollection businessCollection)
        {
            List<Models.Prospect> prospects = new List<Models.Prospect>();

            foreach (Company entityCompany in businessCollection)
            {
                int document = entityCompany.IndividualId;
                Models.Prospect prospect = prospects.Where(x => x.Id == document).FirstOrDefault();
                if (prospect == null)
                {
                    prospects.Add(ModelAssembler.CreateProspectByCompany(entityCompany));
                }
            }

            return prospects;
        }
        public static Models.IdentificationDocument CreateCompanyIdentificationDocument(Company company)
        {
            return new Models.IdentificationDocument
            {
                Number = company.TributaryIdNo,
                DocumentType = new Models.DocumentType
                {
                    Id = company.TributaryIdTypeCode
                }
            };
        }
        public static Models.IdentificationDocument CreatePersonIdentificationDocument(Person person)
        {

            return new Models.IdentificationDocument
            {
                Number = person.IdCardNo,
                DocumentType = new Models.DocumentType
                {
                    Id = person.IdCardTypeCode
                }
            };
        }
        #endregion

        #region OperatingQuota

        /// <summary>
        /// Creates the operating quota.
        /// </summary>
        /// <param name="operatingQuota">The operating quota.</param>
        /// <returns></returns>
        private static Models.OperatingQuota CreateOperatingQuota(OperatingQuota operatingQuota)
        {
            return new Models.OperatingQuota
            {
                IndividualId = operatingQuota.IndividualId,
                Amount = new Amount
                {
                    Value = operatingQuota.OperatingQuotaAmount,
                    Currency = new Sistran.Core.Application.CommonService.Models.Currency() { Id = operatingQuota.CurrencyCode, }
                },
                LineBusiness = new modelsCommon.LineBusiness { Id = operatingQuota.LineBusinessCode, Description = null },
                CurrentTo = operatingQuota.CurrentTo,
                StrCurrentTo = operatingQuota.CurrentTo.ToString("dd/MM/yyyy")

            };
        }

        /// <summary>
        /// Creates the operating quotas.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.OperatingQuota> CreateOperatingQuotas(BusinessCollection businessCollection)
        {
            List<Models.OperatingQuota> operatingQuotas = new List<Models.OperatingQuota>();
            foreach (OperatingQuota field in businessCollection)
            {
                operatingQuotas.Add(ModelAssembler.CreateOperatingQuota(field));

            }
            return operatingQuotas;
        }


        /// <summary>
        /// Updates the operating quota.
        /// </summary>
        /// <param name="operatingQuotas">The operating quotas.</param>
        /// <returns></returns>
        public static Models.OperatingQuota UpdateOperatingQuota(OperatingQuota operatingQuotas)
        {
            Models.OperatingQuota operatingQuota = new Models.OperatingQuota();

            operatingQuota = ModelAssembler.CreateOperatingQuota(operatingQuotas);


            return operatingQuota;
        }
        #endregion

        #region Provider

        /// <summary>
        /// Creates the provider.
        /// </summary>
        /// <param name="providerType">Type of the Provider.</param>
        public static Models.Provider CreateProvider(Provider provider)
        {
            return new Models.Provider
            {
                Id = provider.ProviderCode,
                IndividualId = provider.IndividualId,
                ProviderTypeId = provider.ProviderTypeCode,
                OriginTypeId = provider.OriginTypeCode,
                ProviderDeclinedTypeId = provider.ProviderDeclinedTypeCode,
                CreationDate = provider.CreationDate,
                ModificationDate = provider.ModificationDate,
                DeclinationDate = provider.DeclinationDate,
                Observation = provider.Observation,
                SpecialityDefault = provider.SpecialityDefault
            };
        }

        /// <summary>
        /// Creates the providers.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.Provider> CreateProviders(BusinessCollection businessCollection)
        {
            List<Models.Provider> provider = new List<Models.Provider>();

            foreach (Provider field in businessCollection)
            {
                provider.Add(ModelAssembler.CreateProvider(field));
            }

            return provider;
        }

        /// <summary>
        /// Creates the type of the provider.
        /// </summary>
        /// <param name="providerType">Type of the Provider.</param>
        private static Models.ProviderType CreateProviderType(ProviderType providerType)
        {
            return new Models.ProviderType
            {
                Id = providerType.ProviderTypeCode,
                Description = providerType.Description,
                SmallDescription = providerType.SmallDescription
            };
        }

        /// <summary>
        /// Creates the provider types.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.ProviderType> CreateProviderTypes(BusinessCollection businessCollection)
        {
            List<Models.ProviderType> providerTypes = new List<Models.ProviderType>();

            foreach (ProviderType field in businessCollection)
            {
                providerTypes.Add(ModelAssembler.CreateProviderType(field));
            }

            return providerTypes;
        }

        /// <summary>
        /// Creates the type of the provider declined.
        /// </summary>
        /// <param name="providerDeclinedType">Type of the Provider Declinado.</param>
        private static Models.ProviderDeclinedType CreateProviderDeclinedType(ProviderDeclinedType providerDeclinedType)
        {
            return new Models.ProviderDeclinedType
            {
                Id = providerDeclinedType.ProviderDeclinedTypeCode,
                Description = providerDeclinedType.Description,
                SmallDescription = providerDeclinedType.SmallDescription
            };
        }

        /// <summary>
        /// Creates the provider declineds types.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.ProviderDeclinedType> CreateProviderDeclinedTypes(BusinessCollection businessCollection)
        {
            List<Models.ProviderDeclinedType> providerDeclinedType = new List<Models.ProviderDeclinedType>();

            foreach (ProviderDeclinedType field in businessCollection)
            {
                providerDeclinedType.Add(ModelAssembler.CreateProviderDeclinedType(field));
            }

            return providerDeclinedType;
        }

        /// <summary>
        /// Creates the type of the provider speciality.
        /// </summary>
        public static Models.ProviderSpeciality CreateProviderSpeciality(ProviderSpeciality providerSpeciality)
        {
            return new Models.ProviderSpeciality
            {
                Id = providerSpeciality.ProviderSpecialityCode,
                ProviderId = providerSpeciality.ProviderCode,
                SpecialityId = providerSpeciality.SpecialityCode
            };
        }

        /// <summary>
        /// Creates the type of the provider speciality.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.ProviderSpeciality> CreateProviderSpecialitys(BusinessCollection businessCollection)
        {
            List<Models.ProviderSpeciality> providerSpeciality = new List<Models.ProviderSpeciality>();

            foreach (ProviderSpeciality field in businessCollection)
            {
                providerSpeciality.Add(ModelAssembler.CreateProviderSpeciality(field));
            }

            return providerSpeciality;
        }

        /// <summary>
        /// Creates the type of the provider concepto de pago.
        /// </summary>
        public static Models.ProviderPaymentConcept CreateProviderPaymentConceptByDescription(ProviderPaymentConcept providerPaymentConcept, string descriptionConceptPayment)
        {
            return new Models.ProviderPaymentConcept
            {
                Id = providerPaymentConcept.ProviderPaymentConceptId,
                ProviderId = providerPaymentConcept.ProviderCode,
                PaymentConcept = new modelsCommon.PaymentConcept { Id = providerPaymentConcept.PaymentConceptCode, Description = descriptionConceptPayment }
            };
        }

        /// <summary>
        /// Creates the type of the provider concepto de pago.
        /// </summary>
        public static Models.ProviderPaymentConcept CreateProviderPaymentConcept(ProviderPaymentConcept providerPaymentConcept)
        {
            return new Models.ProviderPaymentConcept
            {
                Id = providerPaymentConcept.ProviderPaymentConceptId,
                ProviderId = providerPaymentConcept.ProviderCode,
                PaymentConcept = new modelsCommon.PaymentConcept { Id = providerPaymentConcept.PaymentConceptCode }
            };
        }

        /// <summary>
        /// Creates the type of the provider concepto de pago.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.ProviderPaymentConcept> CreateProviderPaymentConcepts(ProviderPaymentConceptView businessCollection)
        {
            List<Models.ProviderPaymentConcept> providerPaymentConcept = new List<Models.ProviderPaymentConcept>();

            foreach (ProviderPaymentConcept field in businessCollection.ProviderPaymentConcepts)
            {
                providerPaymentConcept.Add(ModelAssembler.CreateProviderPaymentConcept(field, businessCollection));
            }

            return providerPaymentConcept;
        }

        /// <summary>
        /// Creates the type of the provider concepto de pago.
        /// </summary>
        public static Models.ProviderPaymentConcept CreateProviderPaymentConcept(ProviderPaymentConcept providerPaymentConcept, ProviderPaymentConceptView businessCollection)
        {
            modelsCommon.PaymentConcept paymentConcept = new modelsCommon.PaymentConcept();
            foreach (Parameters.Entities.PaymentConcept field in businessCollection.PaymentConcepts)
            {
                if (field.PaymentConceptCode == providerPaymentConcept.PaymentConceptCode)
                {
                    paymentConcept.Id = field.PaymentConceptCode;
                    paymentConcept.Description = field.Description;
                }
            }

            return new Models.ProviderPaymentConcept
            {
                Id = providerPaymentConcept.ProviderPaymentConceptId,
                ProviderId = providerPaymentConcept.ProviderCode,
                PaymentConcept = paymentConcept
            };
        }

        /// <summary>
        /// Creates the type of the provider concepto de pago.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.ProviderPaymentConcept> CreateProviderPaymentConcepts(BusinessCollection businessCollection)
        {
            List<Models.ProviderPaymentConcept> providerPaymentConcept = new List<Models.ProviderPaymentConcept>();

            foreach (ProviderPaymentConcept field in businessCollection)
            {
                providerPaymentConcept.Add(ModelAssembler.CreateProviderPaymentConcept(field));
            }

            return providerPaymentConcept;
        }

        #endregion

        #region Origin

        /// <summary>
        /// Creates the type of origin.
        /// </summary>
        /// <param name="providerType">Type of the Provider.</param>
        public static Models.OriginType CreateOriginType(OriginType originType)
        {
            return new Models.OriginType
            {
                Id = originType.OriginTypeCode,
                Description = originType.Description,
                SmallDescription = originType.SmallDescription
            };
        }

        /// <summary>
        /// Creates the type of origin.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.OriginType> CreateOriginTypes(BusinessCollection businessCollection)
        {
            List<Models.OriginType> originTypes = new List<Models.OriginType>();

            foreach (OriginType field in businessCollection)
            {
                originTypes.Add(ModelAssembler.CreateOriginType(field));
            }

            return originTypes;
        }

        #endregion

        #region Tax

        /// <summary>
        /// Creates the tax de la persona.
        /// </summary>
        /// <param name="providerType">Type of the Provider.</param>
        public static Models.IndividualTax CreateIndividualTax(UniquePerson.Entities.IndividualTax individualTax)
        {
            return new Models.IndividualTax
            {
                Id = individualTax.IndividualTaxId,
                Tax = new modelTax.Tax { Id = individualTax.TaxCode },
                TaxCondition = new modelTax.TaxCondition { Id = individualTax.TaxConditionCode },
                IndividualId = individualTax.IndividualId

            };
        }



        public static Models.IndividualTaxExeption CreateIndividualTaxExemption(taxEntity.IndividualTaxExemption individualTaxExemption)
        {
            return new Models.IndividualTaxExeption
            {
                IndividualTaxExemptionId = individualTaxExemption.IndTaxExemptionId,
                Tax = new modelTax.Tax { Id = individualTaxExemption.TaxCode },
                ExtentPercentage = Convert.ToInt32(individualTaxExemption.ExemptionPercentage),
                Datefrom = individualTaxExemption.CurrentFrom,
                DateUntil = individualTaxExemption.CurrentTo,
                StateCode = new modelsCommon.State { Id = individualTaxExemption.StateCode.Value },
                CountryCode = Convert.ToInt32(individualTaxExemption.CountryCode),
                OfficialBulletinDate = individualTaxExemption.BulletinDate,
                ResolutionNumber = individualTaxExemption.ResolutionNumber,
                TotalRetention = individualTaxExemption.HasFullRetention,
                TaxCategory = new modelTax.TaxCategory { Id = individualTaxExemption.TaxCategoryCode.Value }
            };
        }




        /// <summary>
        /// Mapea modelo IndividualTaxExeption a IndividualTax
        /// </summary>
        /// <param name="individualTaxExeption">Objeto de tipo IndividualTaxExeption</param>
        /// <returns>Objeto de tipo IndividualTax</returns>
        public static Models.IndividualTax MappIndividualTaxFromIndividualTaxExeption(Models.IndividualTaxExeption individualTaxExeption)
        {
            return new Models.IndividualTax()
            {
                Id = individualTaxExeption.Id,
                IndividualId = individualTaxExeption.IndividualId,
                Tax = individualTaxExeption.Tax,
                TaxCondition = individualTaxExeption.TaxCondition
            };
        }

        public static Models.IndividualTaxExeption MappIndividualTaxExeptionFromIndividualTaxExeption(Models.IndividualTaxExeption individualTaxExemption)
        {
            return new Models.IndividualTaxExeption()
            {
                IndividualId = individualTaxExemption.IndividualId,
                IndividualTaxExemptionId = individualTaxExemption.IndividualTaxExemptionId,
                Tax = new modelTax.Tax { Id = individualTaxExemption.Tax.Id },
                ExtentPercentage = Convert.ToInt32(individualTaxExemption.ExtentPercentage),
                Datefrom = individualTaxExemption.Datefrom,
                DateUntil = individualTaxExemption.DateUntil,
                StateCode = new modelsCommon.State { Id = individualTaxExemption.StateCode.Id },
                CountryCode = Convert.ToInt32(individualTaxExemption.CountryCode),
                OfficialBulletinDate = individualTaxExemption.OfficialBulletinDate,
                ResolutionNumber = individualTaxExemption.ResolutionNumber,
                TotalRetention = individualTaxExemption.TotalRetention,
                TaxCategory = new modelTax.TaxCategory { Id = individualTaxExemption.TaxCategory.Id }
            };
        }

        /// <summary>
        /// Mapea modelo IndividualTaxExeption a IndividualTax
        /// </summary>
        /// <param name="individualTaxExeption">Objeto de tipo IndividualTaxExeption</param>
        /// <returns>Objeto de tipo IndividualTax</returns>




        /// <summary>
        /// Creates the taxs de la persona.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.IndividualTax> CreateIndividualTaxs(BusinessCollection businessCollection)
        {
            List<Models.IndividualTax> individualTax = new List<Models.IndividualTax>();

            foreach (UniquePerson.Entities.IndividualTax field in businessCollection)
            {
                individualTax.Add(ModelAssembler.CreateIndividualTax(field));
            }

            return individualTax;
        }

        public static List<modelTax.Tax> CreateTaxs(BusinessCollection businessCollection)
        {
            List<modelTax.Tax> tax = new List<modelTax.Tax>();

            foreach (Tax.Entities.Tax field in businessCollection)
            {
                tax.Add(ModelAssembler.CreateTax(field));
            }

            return tax;
        }
        public static modelTax.Tax CreateTax(Tax.Entities.Tax tax)
        {
            return new modelTax.Tax
            {
                Id = tax.TaxCode,
                Description = tax.Description

            };
        }


        public static List<modelTax.TaxCondition> CreateConditionTaxs(BusinessCollection businessCollection)
        {
            List<modelTax.TaxCondition> taxCondition = new List<modelTax.TaxCondition>();

            foreach (Tax.Entities.TaxCondition field in businessCollection)
            {
                taxCondition.Add(ModelAssembler.CreateConditionTax(field));
            }

            return taxCondition;
        }
        public static modelTax.TaxCondition CreateConditionTax(Tax.Entities.TaxCondition taxCondition)
        {
            return new modelTax.TaxCondition
            {
                Id = taxCondition.TaxConditionCode,
                Description = taxCondition.Description,



            };
        }

        /// <summary>
        /// Creates the taxs de la persona.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.IndividualTax> CreateIndividualTaxs(PersonTaxIndividualTaxView taxIndividualTaxView)
        {
            List<Models.IndividualTax> individualTax = new List<Models.IndividualTax>();

            foreach (UniquePerson.Entities.IndividualTax field in taxIndividualTaxView.IndividualTaxs)
            {
                individualTax.Add(ModelAssembler.CreateIndividualTax(field, taxIndividualTaxView));
            }

            return individualTax;
        }

        public static List<Models.IndividualTaxExeption> CreateIndividualTaxExemptions(TaxIndividualTaxExemptionView taxIndividualTaxExemptionView)
        {
            List<Models.IndividualTaxExeption> individualTax = new List<Models.IndividualTaxExeption>();
            foreach (Tax.Entities.IndividualTaxExemption field in taxIndividualTaxExemptionView.IndividualTaxExemption)
            {
                individualTax.Add(ModelAssembler.CreateIndividualTaxExemption(field, taxIndividualTaxExemptionView));
            }
            return individualTax;

        }

        /// <summary>
        /// Creates the tax de la persona.
        /// </summary>
        /// <param name="providerType">Type of the Provider.</param>
        public static Models.IndividualTaxExeption CreateIndividualTaxExemption(Tax.Entities.IndividualTaxExemption individualTax, TaxIndividualTaxExemptionView taxIndividualTaxView)
        {
            Models.IndividualTaxExeption model1 = new PERMODEL.IndividualTaxExeption();
            if (taxIndividualTaxView.IndividualTaxExemption.Count > 0)
            {
                foreach (Tax.Entities.IndividualTaxExemption item in taxIndividualTaxView.IndividualTaxExemption)
                {
                    model1.IndividualTaxExemptionId = item.IndTaxExemptionId;

                }

            }

            Models.IndividualTaxExeption model = new PERMODEL.IndividualTaxExeption();
            foreach (UniquePerson.Entities.IndividualTax item in taxIndividualTaxView.IndividualTax)
            {
                model.Id = item.IndividualTaxId;

            }


            modelTax.Tax tax = new modelTax.Tax();
            foreach (Tax.Entities.Tax field in taxIndividualTaxView.Tax)
            {
                if (field.TaxCode == individualTax.TaxCode)
                {
                    tax.Description = field.Description;
                    tax.Id = field.TaxCode;
                }
            }

            modelTax.TaxCondition taxCondition = new modelTax.TaxCondition();
            foreach (TaxCondition field in taxIndividualTaxView.TaxCondition)
            {
                taxCondition.Description = field.Description;
                taxCondition.Id = field.TaxConditionCode;

            }

            modelTax.TaxCategory taxCategory = new modelTax.TaxCategory();
            foreach (TaxCategory field in taxIndividualTaxView.TaxCategory)
            {
                taxCategory.Description = field.Description;
                taxCategory.Id = field.TaxCategoryCode;

            }
            modelsCommon.State stateCode = new modelsCommon.State();
            foreach (COMMEN.State field in taxIndividualTaxView.State)
            {
                stateCode.Description = field.Description;
                stateCode.Id = field.StateCode;

            }


            return new Models.IndividualTaxExeption
            {

                Id = model.Id,
                IndividualTaxExemptionId = model1.IndividualTaxExemptionId,
                Tax = tax,
                TaxCondition = taxCondition,
                IndividualId = individualTax.IndividualId,
                TaxCategory = taxCategory,
                StateCode = stateCode,
                CountryCode = Convert.ToInt32(individualTax.CountryCode),
                Datefrom = individualTax.CurrentFrom,
                DateUntil = individualTax.CurrentTo,
                ExtentPercentage = Convert.ToInt32(individualTax.ExemptionPercentage),
                OfficialBulletinDate = individualTax.BulletinDate,
                ResolutionNumber = individualTax.ResolutionNumber,
                TotalRetention = individualTax.HasFullRetention
            };
        }

        //public static Models.IndividualTax CreateIndividualTaxExemptionIndividual(Tax.Entities.IndividualTaxExemption individualTax, TaxIndividualTaxExemptionView taxIndividualTaxView)
        //{

        //    modelTax.Tax tax = new modelTax.Tax();
        //    foreach (Tax.Entities.Tax field in taxIndividualTaxView.Tax)
        //    {
        //        if (field.TaxCode == individualTax.TaxCode)
        //        {
        //            tax.Description = field.Description;
        //            tax.Id = field.TaxCode;
        //        }
        //    }

        //    modelTax.TaxCondition taxCondition = new modelTax.TaxCondition();
        //    foreach (TaxCondition field in taxIndividualTaxView.Tax)
        //    {
        //        if (field.TaxCode == individualTax.TaxCode && field.TaxConditionCode == individualTax.TaxConditionCode)
        //        {
        //            taxCondition.Description = field.Description;
        //            taxCondition.Id = field.TaxConditionCode;
        //        }
        //    }

        //    return new Models.IndividualTax
        //    {
        //        Id = individualTax.IndividualTaxId,
        //        Tax = tax,
        //        TaxCondition = taxCondition,
        //        IndividualId = individualTax.IndividualId
        //    };
        //}


        /// <summary>
        /// /
        /// </summary>
        /// <param name="individualRelationApp"></param>
        /// <returns></returns>
        public static Models.IndividualTax CreateIndividualTax(UniquePerson.Entities.IndividualTax individualTax, PersonTaxIndividualTaxView taxIndividualTaxView)
        {

            modelTax.Tax tax = new modelTax.Tax();
            foreach (Tax.Entities.Tax field in taxIndividualTaxView.Taxs)
            {
                if (field.TaxCode == individualTax.TaxCode)
                {
                    tax.Description = field.Description;
                    tax.Id = field.TaxCode;
                }
            }

            modelTax.TaxCondition taxCondition = new modelTax.TaxCondition();
            foreach (TaxCondition field in taxIndividualTaxView.TaxConditions)
            {
                if (field.TaxCode == individualTax.TaxCode && field.TaxConditionCode == individualTax.TaxConditionCode)
                {
                    taxCondition.Description = field.Description;
                    taxCondition.Id = field.TaxConditionCode;
                }
            }

            return new Models.IndividualTax
            {
                Id = individualTax.IndividualTaxId,
                Tax = tax,
                TaxCondition = taxCondition,
                IndividualId = individualTax.IndividualId
            };
        }


        #endregion

        #region IndividualRelationApp
        public static Models.IndividualRelationApp CreateIndividualRelationApp(IndividualRelationApp individualRelationApp)
        {
            return new Models.IndividualRelationApp()
            {
                Agency = new Models.Agency { Id = individualRelationApp.AgentAgencyId },
                ChildIndividual = new Models.Agent { IndividualId = individualRelationApp.ChildIndividualId },
                ParentIndividualId = individualRelationApp.ParentIndividualId,
                RelationTypeCd = individualRelationApp.RelationTypeCode,
                IndividualRelationAppId = individualRelationApp.IndividualRelationAppId
            };
        }


        /// <summary>
        /// Creates the user person.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.IndividualRelationApp> CreateIndividualsRelationApp(BusinessCollection businessCollection)
        {
            List<Models.IndividualRelationApp> individualsRelationApp = new List<Models.IndividualRelationApp>();

            foreach (IndividualRelationApp field in businessCollection)
            {
                individualsRelationApp.Add(ModelAssembler.CreateIndividualRelationApp(field));
            }

            return individualsRelationApp;
        }
        #endregion

        #region Prefix

        public static modelsCommon.Prefix CreatePrefix(Common.Entities.Prefix entityPrefix)
        {
            var imaper = CreateMapPrefix();
            return imaper.Map<COMMEN.Prefix, modelsCommon.Prefix>(entityPrefix);
        }

        public static List<modelsCommon.Base.BasePrefix> CreatePrefixes(BusinessCollection businessCollection)
        {
            var prefixEntity = businessCollection.Cast<COMMEN.Prefix>().ToList();
            var imaper = CreateMapPrefix();
            return imaper.Map<List<COMMEN.Prefix>, List<modelsCommon.Base.BasePrefix>>(prefixEntity);
        }

        #endregion

        #region ReInsurer
        public static modelsPerson.ReInsurer CreateReinsurer(Reinsurer entityReinsurer)
        {
            return new modelsPerson.ReInsurer
            {
                Annotations = entityReinsurer.Annotations,
                DeclinedDate = entityReinsurer.DeclinedDate,
                DeclaredTypeCD = entityReinsurer.DeclinedTypeCode,
                EnteredDate = entityReinsurer.EnteredDate,
                IndividualId = entityReinsurer.IndividualId,
                ModifyDate = entityReinsurer.ModifyDate,
                ReinsuredCD = entityReinsurer.ReinsurerCode
            };
        }

        public static List<modelsPerson.ReInsurer> CreateReinsurer(BusinessCollection businessCollection)
        {
            List<modelsPerson.ReInsurer> reinsurers = new List<modelsPerson.ReInsurer>();

            foreach (Reinsurer entity in businessCollection)
            {
                reinsurers.Add(ModelAssembler.CreateReinsurer(entity));
            }

            return reinsurers;
        }
        #endregion

        public static List<modelsPerson.Nomenclature> CreateNomenclatures(BusinessCollection businessCollection)
        {
            List<modelsPerson.Nomenclature> nomenlcatures = new List<modelsPerson.Nomenclature>();
            foreach (CoNomenclatures item in businessCollection)
            {
                nomenlcatures.Add(CreateNomenclature(item));
            }
            return nomenlcatures;
        }

        public static modelsPerson.Nomenclature CreateNomenclature(CoNomenclatures coNomenclatures)
        {
            return new modelsPerson.Nomenclature()
            {
                Id = coNomenclatures.Id,
                Description = coNomenclatures.Nomenclatura,
                SmallDescription = coNomenclatures.Abreviatura
            };
        }

        #region IndividualType

        public static ModelIndividual.IndividualType CreateIndividualType(Parameters.Entities.IndividualType entityIndividualType)
        {
            return new ModelIndividual.IndividualType
            {
                Id = entityIndividualType.IndividualTypeCode,
                Description = entityIndividualType.Description,
                SmallDescription = entityIndividualType.SmallDescription
            };
        }

        public static List<ModelIndividual.IndividualType> CreateIndividualTypes(BusinessCollection businessCollection)
        {
            List<ModelIndividual.IndividualType> individualType = new List<ModelIndividual.IndividualType>();

            foreach (Parameters.Entities.IndividualType entity in businessCollection)
            {
                individualType.Add(ModelAssembler.CreateIndividualType(entity));
            }

            return individualType;
        }

        #endregion

        #region CoInsuranceCompany

        public static Models.CoInsuranceCompany CreateCoInsuranceCompany(CoInsuranceCompany entityCoInsuranceCompany)
        {
            return new Models.CoInsuranceCompany
            {
                Id = entityCoInsuranceCompany.InsuranceCompanyId,
                Description = entityCoInsuranceCompany.Description,
                TributaryIdCardNo = entityCoInsuranceCompany.TributaryIdNo
            };
        }

        public static List<Models.CoInsuranceCompany> CreateCoInsuranceCompanies(BusinessCollection businessCollection)
        {
            List<Models.CoInsuranceCompany> coInsuranceCompanies = new List<Models.CoInsuranceCompany>();

            foreach (COMMEN.CoInsuranceCompany entity in businessCollection)
            {
                coInsuranceCompanies.Add(ModelAssembler.CreateCoInsuranceCompany(entity));
            }

            return coInsuranceCompanies;
        }

        #endregion


        #region ExonerationType

        public static Models.ExonerationType CreateExonerationType(COMMEN.ExonerationType entityExonerationType)
        {
            return new Models.ExonerationType
            {
                ExonerationTypeCode = entityExonerationType.ExonerationTypeCode,
                Description = entityExonerationType.Description,
                SmallDescription = entityExonerationType.SmallDescription,
                IndividualTypeCode = entityExonerationType.IndividualTypeCode,
                Enabled = entityExonerationType.Enabled
            };
        }

        public static List<Models.ExonerationType> CreateExonerationtypes(BusinessCollection businessCollection)
        {
            List<Models.ExonerationType> exonerationtypes = new List<Models.ExonerationType>();

            foreach (COMMEN.ExonerationType entity in businessCollection)
            {
                exonerationtypes.Add(ModelAssembler.CreateExonerationType(entity));
            }

            return exonerationtypes;
        }

        #endregion

        #region InsuredProfile
        /// <summary>
        /// Mapeo de la entidad InsuredProfile al modelo InsuredProfile
        /// </summary>
        /// <param name="insuredProfile"> Entidad InsuredProfile</param>
        /// <returns> Modelo InsuredProfile</returns>
        public static Models.InsuredProfile CreateInsuredProfile(InsuredProfile insuredProfile)
        {
            return new Models.InsuredProfile()
            {
                ShortDescription = insuredProfile.SmallDescription,
                LongDescription = insuredProfile.Description,
                Id = insuredProfile.InsProfileCode
            };
        }

        /// <summary>
        /// Mapeo lista de objeto businessCollection a lista modelo InsuredProfile
        /// </summary>
        /// <param name="businessCollection">Objeto businessCollection</param>
        /// <returns>Lista de Modelos InsuredProfile</returns>
        public static List<Models.InsuredProfile> CreateInsuredProfiles(BusinessCollection businessCollection)
        {
            List<Models.InsuredProfile> insuredProfiles = new List<Models.InsuredProfile>();

            foreach (InsuredProfile entity in businessCollection)
            {
                insuredProfiles.Add(ModelAssembler.CreateInsuredProfile(entity));
            }

            return insuredProfiles;
        }
        #endregion

        /// <summary>
        /// Mapeo de la entidad InsuredSegment al modelo InsuredSegment
        /// </summary>
        /// <param name="insuredSegment">Entidad InsuredSegment</param>
        /// <returns>Modelo InsuredSegment</returns>
        #region InsuredSegment
        public static Models.InsuredSegment CreateInsuredSegment(InsuredSegment insuredSegment)
        {
            return new Models.InsuredSegment()
            {
                ShortDescription = insuredSegment.SmallDescription,
                LongDescription = insuredSegment.Description,
                Id = insuredSegment.InsSegmentCode
            };
        }

        public static List<Models.InsuredSegment> CreateInsuredSegments(BusinessCollection businessCollection)
        {
            List<Models.InsuredSegment> insuredSegments = new List<Models.InsuredSegment>();

            foreach (InsuredSegment entity in businessCollection)
            {
                insuredSegments.Add(ModelAssembler.CreateInsuredSegment(entity));
            }

            return insuredSegments;
        }
        #endregion

        #region Company.PhoneType
        /// <summary>
        /// Convierte entidad a modelo del servicio
        /// </summary>
        /// <param name="companyPhoneType">Entidad del tipo teléfono</param>
        /// <returns>Modelo del tipo teléfono</returns>
        public static Models.CompanyPhoneType CreateCompanyPhoneType(CompanyPhoneType companyPhoneType)
        {
            Models.CompanyPhoneType companyPhoneTypeId = new Models.CompanyPhoneType();
            companyPhoneTypeId.PhoneTypeCode = companyPhoneType.PhoneTypeCode;
            return new Models.CompanyPhoneType
            {
                PhoneTypeCode = companyPhoneType.PhoneTypeCode,
                Description = companyPhoneType.Description,
                SmallDescription = companyPhoneType.SmallDescription,
                IsCellphone = Convert.ToBoolean(companyPhoneType.IsCellphone),
            };
        }

        /// <summary>
        /// Convierte lista de entidades a lista de modelos del servicio
        /// </summary>
        /// <param name="businessCollection">Lista de entidades del tipo teléfono</param>
        /// <returns>Lista de modelos del tipo teléfono</returns>	
        public static List<Models.CompanyPhoneType> CreateCompanyPhoneType(BusinessCollection businessCollection)
        {
            List<Models.CompanyPhoneType> companyPhoneType = new List<Models.CompanyPhoneType>();
            foreach (CompanyPhoneType field in businessCollection)
            {
                companyPhoneType.Add(ModelAssembler.CreateCompanyPhoneType(field));
            }

            return companyPhoneType;
        }
        #endregion
        #region Company.AddressType
        /// <summary>
        /// Convierte entidad a modelo del servicio
        /// </summary>
        /// <param name="companyAddressType">Entidad del tipo dirección</param>
        /// <returns>Modelo del tipo dirección</returns>
        public static Models.CompanyAddressType CreateCompanyAddressType(CompanyAddressType companyAddressType)
        {
            Models.CompanyAddressType companyAddressTypeId = new Models.CompanyAddressType();
            companyAddressTypeId.AddressTypeCode = companyAddressType.AddressTypeCode;
            return new Models.CompanyAddressType
            {
                AddressTypeCode = companyAddressType.AddressTypeCode,
                SmallDescription = companyAddressType.SmallDescription,
                TinyDescription = companyAddressType.TinyDescription,
                IsElectronicMail = Convert.ToBoolean(companyAddressType.IsElectronicMail)
            };
        }

        /// <summary>
        /// Convierte lista de entidades a lista de modelos del servicio
        /// </summary>
        /// <param name="businessCollection">Lista de entidades del tipo dirección</param>
        /// <returns>Lista de modelos del tipo dirección</returns>	
        public static List<Models.CompanyAddressType> CreateCompanyAddressType(BusinessCollection businessCollection)
        {
            List<Models.CompanyAddressType> companyAddressType = new List<Models.CompanyAddressType>();
            foreach (CompanyAddressType field in businessCollection)
            {
                companyAddressType.Add(ModelAssembler.CreateCompanyAddressType(field));
            }

            return companyAddressType;
        }
        #endregion

        #region AgentCommission

        public static Models.CommissionAgent CreateAgentCommission(AgencyCommissRate agentCommission)
        {
            var imaper = CreateMapAgentCommission();
            return imaper.Map<AgencyCommissRate, Models.CommissionAgent>(agentCommission);
        }
        public static List<Models.CommissionAgent> CreateAgentCommissions(BusinessCollection businessCollection)
        {
            var imaper = CreateMapAgentCommission();
            var agentCommissions = businessCollection.Cast<AgencyCommissRate>().ToList();
            return imaper.Map<List<AgencyCommissRate>, List<Models.CommissionAgent>>(agentCommissions);
        }


        #endregion

        #region LineBusiness
        public static modelsCommon.LineBusiness CreateLineBusiness(Common.Entities.LineBusiness entityLineBusiness)
        {
            var imaper = CreateMapLineBusiness();
            return imaper.Map<Common.Entities.LineBusiness, modelsCommon.LineBusiness>(entityLineBusiness);
        }

        public static List<modelsCommon.LineBusiness> CreateLineBusinesses(BusinessCollection businessCollection)
        {
            var imaper = CreateMapLineBusiness();
            var entityLineBusiness = businessCollection.Cast<Common.Entities.LineBusiness>().ToList();
            return imaper.Map<List<Common.Entities.LineBusiness>, List<modelsCommon.LineBusiness>>(entityLineBusiness);
        }
        #endregion


        #region SubLineBusiness
        public static modelsCommon.SubLineBusiness CreateSubLineBusiness(Common.Entities.SubLineBusiness entitySubLineBusiness)
        {
            var imaper = CreateMapSubLineBusiness();
            return imaper.Map<Common.Entities.SubLineBusiness, modelsCommon.SubLineBusiness>(entitySubLineBusiness);
        }

        public static List<modelsCommon.SubLineBusiness> CreateSubLineBusinesses(BusinessCollection businessCollection)
        {
            var imaper = CreateMapSubLineBusiness();
            var entitySubLineBusiness = businessCollection.Cast<Common.Entities.SubLineBusiness>().ToList();
            return imaper.Map<List<Common.Entities.SubLineBusiness>, List<modelsCommon.SubLineBusiness>>(entitySubLineBusiness);
        }
        #endregion


        #region EconomicActivity

        private static PERMODEL.EconomicActivity CreateEconomicActivity(COMMEN.EconomicActivity economicActivity)
        {
            return new PERMODEL.EconomicActivity
            {
                Id = economicActivity.EconomicActivityCode,
                Description = economicActivity.Description,
            };
        }

        public static List<PERMODEL.EconomicActivity> CreateEconomicActivities(BusinessCollection businessCollection)
        {
            List<PERMODEL.EconomicActivity> economicActivities = new List<PERMODEL.EconomicActivity>();

            foreach (COMMEN.EconomicActivity entity in businessCollection)
            {
                economicActivities.Add(ModelAssembler.CreateEconomicActivity(entity));
            }

            return economicActivities;
        }

        #endregion

        #region AssetType

        private static PERMODEL.AssetType CreateAssetType(COMMEN.AssetType entityAssetType)
        {
            return new PERMODEL.AssetType
            {
                Code = entityAssetType.AssetTypeCode,
                Description = entityAssetType.Description,
                IsVehicle = entityAssetType.IsVehicle
            };
        }

        public static List<PERMODEL.AssetType> CreateAssetTypes(BusinessCollection businessCollection)
        {
            List<PERMODEL.AssetType> assetTypes = new List<PERMODEL.AssetType>();

            foreach (COMMEN.AssetType entity in businessCollection)
            {
                assetTypes.Add(ModelAssembler.CreateAssetType(entity));
            }

            return assetTypes;
        }

        #endregion      

        #region AgentGroup

        /// <summary>
        /// Creates the group the Agent.
        /// </summary>
        /// <param name="agentGroup">Type of group the agent </param>
        /// <returns></returns>
        private static Models.Base.BaseGroupAgent CreateAgentGroup(AgentGroup agentGroup)
        {
            return new Models.Base.BaseGroupAgent
            {
                Id = agentGroup.AgentGroupCode,
                Description = agentGroup.Description

            };

        }

        /// <summary>
        /// Creates the group the Agent.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.Base.BaseGroupAgent> CreateAgentGroups(BusinessCollection businessCollection)
        {
            List<Models.Base.BaseGroupAgent> agentGroup = new List<Models.Base.BaseGroupAgent>();

            foreach (AgentGroup field in businessCollection)
            {
                agentGroup.Add(ModelAssembler.CreateAgentGroup(field));
            }

            return agentGroup;
        }
        #endregion

        #region AgentSalesChanel 

        /// <summary>
        /// Creates the group the Agent.
        /// </summary>
        /// <param name="agentGroup">Type of group the agent </param>
        /// <returns></returns>
        private static Models.Base.BaseSalesChannel CreateAgentoSalesChannel(SalesChannel agentGroup)
        {
            return new Models.Base.BaseSalesChannel
            {
                Id = agentGroup.SalesChannelCode,
                Description = agentGroup.Description
            };

        }

        /// <summary>
        /// Creates the group the Agent.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.Base.BaseSalesChannel> CreateAgentoSalesChannels(BusinessCollection businessCollection)
        {
            List<Models.Base.BaseSalesChannel> agentSalesChannel = new List<Models.Base.BaseSalesChannel>();

            foreach (SalesChannel field in businessCollection)
            {
                agentSalesChannel.Add(ModelAssembler.CreateAgentoSalesChannel(field));
            }

            return agentSalesChannel;
        }
        #endregion

        #region AgentAccountExecutive

        public static PERMODEL.Person CreateExecutive(Person entityPerson)
        {
            return new PERMODEL.Person
            {
                IndividualId = entityPerson.IndividualId,
                Name = entityPerson.Name,
                MotherLastName = entityPerson.MotherLastName,
                PersonCode = Convert.ToInt32(entityPerson.IdCardNo)
            };
        }
        public static List<PERMODEL.Person> CreateExecutives(BusinessCollection businessCollection)
        {
            List<PERMODEL.Person> persons = new List<PERMODEL.Person>();

            foreach (Person entity in businessCollection)
            {
                persons.Add(ModelAssembler.CreateExecutive(entity));
            }

            return persons;
        }

        public static PERMODEL.Base.BaseEmployeePerson CreateEmployee(Employee entityEmployee)
        {
            return new PERMODEL.Base.BaseEmployeePerson
            {
                Id = entityEmployee.IndividualId,
            };
        }

        public static List<PERMODEL.Base.BaseEmployeePerson> CreateEmployees(BusinessCollection businessCollection)
        {
            List<PERMODEL.Base.BaseEmployeePerson> employees = new List<PERMODEL.Base.BaseEmployeePerson>();

            foreach (Employee entity in businessCollection)
            {
                employees.Add(ModelAssembler.CreateEmployee(entity));
            }
            return employees;
        }

        #endregion

        #region "PersonInterest"

        private static Models.InterestGroupsType CreateInterestGroup(InterestGroupType interestGroupType)
        {
            return new Models.InterestGroupsType()
            {
                InterestGroupTypeId = interestGroupType.InterestGroupTypeCode,
                Description = interestGroupType.Description
            };
        }


        public static List<Models.InterestGroupsType> CreatePersonInterests(BusinessCollection businessCollection)
        {
            List<Models.InterestGroupsType> interestGroupsTypes = new List<Models.InterestGroupsType>();

            foreach (InterestGroupType field in businessCollection)
            {
                interestGroupsTypes.Add(ModelAssembler.CreateInterestGroup(field));
            }

            return interestGroupsTypes;
        }

        public static Models.PersonInterestGroup CreatePersonInterestGroup(PersonInterestGroup personInterestGroup)
        {
            return new Models.PersonInterestGroup()
            {
                IndividualId = personInterestGroup.IndividualId,
                InterestGroupTypeId = personInterestGroup.InterestGroupTypeCode
            };
        }

        public static List<Models.PersonInterestGroup> CreatePersonInterestGroups(BusinessCollection businessCollection)
        {
            List<Models.PersonInterestGroup> personInterestGroups = new List<Models.PersonInterestGroup>();
            foreach (PersonInterestGroup item in businessCollection)
            {
                personInterestGroups.Add(CreatePersonInterestGroup(item));
            }

            return personInterestGroups;
        }

        #endregion
        public static List<Models.AgentPrefix> CreateAgentsPrefixies(List<AgentPrefix> agentPrefixies)
        {
            var immaper = CreateMapAgentPrefix();
            return immaper.Map<List<AgentPrefix>, List<Models.AgentPrefix>>(agentPrefixies);
        }
        #region automapper
        public static IMapper CreateMapAgentPrefix()
        {

            var config = MapperCache.GetMapper<AgentPrefix, Models.AgentPrefix>(cfg =>
           {
               cfg.CreateMap<AgentPrefix, Models.AgentPrefix>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IndividualId))
               .ForMember(dest => dest.prefix, opt => opt.MapFrom(src => new modelsCommon.Prefix() { Id = src.PrefixCode }));
           });
            return config;
        }
        #region prefix
        public static IMapper CreateMapPrefix()
        {
            var config = MapperCache.GetMapper<COMMEN.Prefix, modelsCommon.Prefix>(cfg =>
            {
                cfg.CreateMap<COMMEN.Prefix, modelsCommon.Prefix>();
                cfg.CreateMap<COMMEN.Prefix, modelsCommon.Base.BasePrefix>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PrefixCode));
            });
            return config;
        }
        #endregion
        #region LineBusiness Mapper
        public static IMapper CreateMapLineBusiness()
        {
            var config = MapperCache.GetMapper<Common.Entities.LineBusiness, modelsCommon.LineBusiness>(cfg =>
            {
                cfg.CreateMap<Common.Entities.LineBusiness, modelsCommon.LineBusiness>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.LineBusinessCode))
           .ForMember(dest => dest.ShortDescription, opt => opt.MapFrom(src => src.SmallDescription));
            });
            return config;
        }
        #endregion  LineBusiness Mapper
        #region LineBusiness Mapper
        /// <summary>
        /// Creates the map sub line business.
        /// </summary>
        public static IMapper CreateMapSubLineBusiness()
        {
            var config = MapperCache.GetMapper<Common.Entities.SubLineBusiness, modelsCommon.SubLineBusiness>(cfg =>
            {
                cfg.CreateMap<Common.Entities.SubLineBusiness, modelsCommon.SubLineBusiness>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SubLineBusinessCode))
           .ForMember(dest => dest.LineBusinessId, opt => opt.MapFrom(src => src.LineBusinessCode));
            });
            return config;
        }
        #endregion LineBusiness Mapper
        /// <summary>
        /// Creates the agent.
        /// </summary>
        /// <param name="agent">The agent.</param>
        /// <returns></returns>
        public static IMapper CreateMapAgent()
        {
            var config = MapperCache.GetMapper<Agent, Models.Agent>(cfg =>
            {
                cfg.CreateMap<Agent, Models.Agent>()
            .ForMember(dest => dest.AgentType, opt => opt.MapFrom(src => new Models.AgentType() { Id = src.AgentTypeCode }))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.CheckPayableTo))
            .ForMember(dest => dest.AgentDeclinedType, opt => opt.MapFrom(src => new Models.AgentDeclinedType() { Id = src.AgentDeclinedTypeCode }))
            .ForMember(dest => dest.DateCurrent, opt => opt.MapFrom(src => src.EnteredDate))
            .ForMember(dest => dest.DateDeclined, opt => opt.MapFrom(src => src.DeclinedDate))
            .ForMember(dest => dest.DateModification, opt => opt.MapFrom(src => src.ModifyDate));
            });
            return config;
        }
        public static IMapper CreateMapAgencies()
        {
            var config = MapperCache.GetMapper<AgentAgency, Models.Agency>(cfg =>
            {
                cfg.CreateMap<AgentAgency, Models.Agency>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AgentAgencyId))
               .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.AgentCode))
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Description))
               .ForMember(dest => dest.DateDeclined, opt => opt.MapFrom(src => src.DeclinedDate))
               .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => new modelsCommon.Branch { Id = src.BranchCode }))
               .ForMember(dest => dest.Agent, opt => opt.MapFrom(src => new Models.Agent { IndividualId = src.IndividualId }))
               .ForMember(dest => dest.AgentType, opt => opt.MapFrom(src => new Models.AgentType { Id = src.AgentTypeCode }))
               .ForMember(dest => dest.AgentDeclinedType, opt => opt.MapFrom(src => new Models.AgentDeclinedType { Id = src.AgentDeclinedTypeCode }));
            });
            return config;
        }
        #region Comision Agente
        public static IMapper CreateMapAgentCommission()
        {
            var config = MapperCache.GetMapper<AgencyCommissRate, Models.CommissionAgent>(cfg =>
            {
                cfg.CreateMap<AgencyCommissRate, Models.CommissionAgent>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AgencyCommissRateId))
               .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
               .ForMember(dest => dest.Agency, opt => opt.MapFrom(src => new Models.Agency { Id = src.AgentAgencyId }))
               .ForMember(dest => dest.Prefix, opt => opt.MapFrom(src => new modelsCommon.Prefix { Id = src.PrefixCode }))
               .ForMember(dest => dest.LineBusiness, opt => opt.MapFrom(src => new modelsCommon.LineBusiness { Id = (int)src.LineBusinessCode }))
               .ForMember(dest => dest.SubLineBusiness, opt => opt.MapFrom(src => new modelsCommon.SubLineBusiness { Id = (int)src.SubLineBusinessCode }))
               .ForMember(dest => dest.PercentageCommission, opt => opt.MapFrom(src => (decimal)src.StCommissPercentage))
               .ForMember(dest => dest.PercentageAdditional, opt => opt.MapFrom(src => (decimal)src.AdditCommissPercentage));
            });
            return config;
        }
        #endregion
        #endregion automapper


        public static List<Models.Rol> CreateIndividualRoles(BusinessCollection businessCollection)
        {
            List<Models.Rol> individualRoles = new List<Models.Rol>();

            foreach (IndividualRole field in businessCollection)
            {
                individualRoles.Add(ModelAssembler.CreateIndividualRol(field));
            }

            return individualRoles;
        }
        public static Models.Rol CreateIndividualRol(IndividualRole individualRole)
        {
            return new Models.Rol()
            {
                Id = individualRole.RoleCode,
                SubRoleId = individualRole.SubRoleCode
            };
        }
        public static IndividualRole CreateIndividualRol(Models.Rol modelRole, int individualId)
        {
            return new IndividualRole(individualId, modelRole.Id)
            {
                RoleCode = modelRole.Id,
                SubRoleCode = modelRole.SubRoleId
            };
        }
        #region Mapper
        #region Asegurados
        /// <summary>
        /// Creates the map insured.
        /// </summary>
        public static IMapper CreateMapInsured()
        {
            var config = MapperCache.GetMapper<Insured, Models.Insured>(cfg =>
            {
                cfg.CreateMap<Insured, Models.Insured>()
            .ForMember(dest => dest.Id, scr => scr.MapFrom(ori => ori.IndividualId))
            .ForMember(dest => dest.Name, scr => scr.MapFrom(ori => ori.CheckPayableTo))
            .ForMember(dest => dest.Profile, scr => scr.MapFrom(ori => ori.InsProfileCode))
            .ForMember(dest => dest.IsComercialClient, scr => scr.MapFrom(ori => (ori.IsCommercialClient == 1) ? true : false))
            .ForMember(dest => dest.IsMailAddress, scr => scr.MapFrom(ori => (ori.IsMailAddress == 1) ? true : false))
            .ForMember(dest => dest.IsSMS, scr => scr.MapFrom(ori => (ori.IsSms == 1) ? true : false))
            .ForMember(dest => dest.ReferedBy, scr => scr.MapFrom(ori => ori.ReferredBy))
            .ForMember(dest => dest.InsuredId, scr => scr.MapFrom(ori => ori.InsuredCode > 0 ? ori.InsuredCode : ori.PrimaryKey["InsuredCode"] != null ? (int)ori.PrimaryKey["InsuredCode"] : 0))
            .ForMember(dest => dest.InsuredProfile, scr => scr.MapFrom(ori => new PERMODEL.Base.BaseInsuredProfile
            {
                IndividualId = ori.InsProfileCode
            }))
            .ForMember(dest => dest.InsuredSegment, scr => scr.MapFrom(ori => new PERMODEL.Base.BaseInsuredSegment()
            {
                IndividualId = ori.InsSegmentCode
            }))
            .ForMember(dest => dest.InsuredMain, scr => scr.MapFrom(ori => new PERMODEL.Base.BaseInsuredMain()
            {
                IndividualId = (ori.MainInsuredIndId == null) ? 0 : Convert.ToInt32(ori.MainInsuredIndId)
            }));
            });
            return config;
        }
        #endregion Asegurados
        #endregion Autommaper

        #region DocumentsTypeRange
        /// <summary>
        /// Convierte entidad a modelo del servicio
        /// </summary>
        /// <param name="DocumentsTypeRange"></param>
        /// <returns></returns>
        public static Models.DocumentTypeRange CreateDocumentTypeRange(DocumentsTypeRange documentTypeRange)
        {
            DAOs.DocumentTypeDAO DocumentTypeDAO = new DAOs.DocumentTypeDAO();

            return new Models.DocumentTypeRange
            {
                Id = documentTypeRange.DocumentsTypeRangeCode,
                CardTypeCode = DocumentTypeDAO.GetDocumentTypeById(documentTypeRange.IdCardTypeCode),
                Gender = documentTypeRange.Gender,
                CardNumberFrom = documentTypeRange.IdCardNoFrom,
                CardNumberTo = documentTypeRange.IdCardNoTo
            };
        }

        public static List<Models.DocumentTypeRange> CreateDocumentsTypeRange(BusinessCollection businessCollection)
        {
            List<Models.DocumentTypeRange> DocumentTypeRange = new List<Models.DocumentTypeRange>();

            foreach (DocumentsTypeRange DocumentTypeRangeEntity in businessCollection)
            {
                DocumentTypeRange.Add(CreateDocumentTypeRange(DocumentTypeRangeEntity));
            }

            return DocumentTypeRange;
        }

        #endregion

    }
}