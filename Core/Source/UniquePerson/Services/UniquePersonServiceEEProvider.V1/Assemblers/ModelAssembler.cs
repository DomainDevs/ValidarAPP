using AutoMapper;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Parameters.Entities;
using Sistran.Core.Application.Tax.Entities;
using Sistran.Core.Application.UniquePersonService.V1.Entities.views;
using Sistran.Core.Application.UniquePersonV1.Entities;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Framework.DAF;
using System;
using System.Collections.Generic;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;
using ENTV1 = Sistran.Core.Application.UniquePersonV1.Entities;
using modelsTax = Sistran.Core.Application.TaxServices.Models;
using ModelIndividual = Sistran.Core.Application.UniquePersonService.V1Individual.Models;
using modelsCommon = Sistran.Core.Application.CommonService.Models;
using modelsPerson = Sistran.Core.Application.UniquePersonService.V1.Models;
using PERMODEL = Sistran.Core.Application.UniquePersonService.V1.Models;
using TAXEN = Sistran.Core.Application.Tax.Entities;
using PRODENT = Sistran.Core.Application.Product.Entities;
using INTEN = Sistran.Core.Application.Integration.Entities;

namespace Sistran.Core.Application.UniquePersonService.V1.Assemblers
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
                CityCode = phone.CityCode,
                CountryCode = phone.CountryCode,
                Extension = phone.Extension,
                ScheduleAvailability = phone.ScheduleAvailability
            };
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

        internal static List<Models.GuaranteeStatusRoute> CreateGuaranteeStatusRoutes(BusinessCollection statusRoutes)
        {
            List<Models.GuaranteeStatusRoute> guaranteeStatusRoutes = new List<Models.GuaranteeStatusRoute>();

            foreach (GuaranteeStatus guaranteeStatus in statusRoutes)
            {
                guaranteeStatusRoutes.Add(ModelAssembler.CreateGuaranteeStatusRoute(guaranteeStatus));
            }

            return guaranteeStatusRoutes;
        }

        public static Models.GuaranteeStatusRoute CreateGuaranteeStatusRoute(GuaranteeStatus guaranteeStatus)
        {
            return new Models.GuaranteeStatusRoute
            {
                Id = guaranteeStatus.GuaranteeStatusCode,
                Description = guaranteeStatus.Description,

            };
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
            if (LegalRepresent != null)
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
                    City = new modelsCommon.City()
                    {
                        Id = LegalRepresent.CityCode,
                        Description = LegalRepresent.City,
                        State = new modelsCommon.State()
                        {
                            Id = LegalRepresent.StateCode,

                            Country = new modelsCommon.Country()
                            {
                                Id = LegalRepresent.CountryCode
                            }
                        }
                    },
                    Phone = LegalRepresent.Phone.ToString(),
                    CellPhone = LegalRepresent.CellPhone.ToString(),
                    JobTitle = LegalRepresent.JobTitle,
                    Email = LegalRepresent.Email,
                    Address = LegalRepresent.Address,
                    AuthorizationAmount = amount,
                    Description = LegalRepresent.Description,
                    IdentificationDocument = new Models.IdentificationDocument()
                    {
                        Number = LegalRepresent.IdCardNo,
                        DocumentType = new Models.DocumentType()
                        {
                            Id = LegalRepresent.IdCardTypeCode,

                        }
                    },
                };
            }
            else
            {
                return null;
            }
        }

        internal static List<PERMODEL.UserAssignedConsortium> CreateUserAssignedConsortiums(BusinessCollection businessObjects)
        {
            List<PERMODEL.UserAssignedConsortium> userAssignedConsortiums = new List<PERMODEL.UserAssignedConsortium>();

            foreach (UserAssignedConsortium entityUserAssignedConsortium in businessObjects)
            {
                userAssignedConsortiums.Add(ModelAssembler.CreateUserAssignedConsortium(entityUserAssignedConsortium));
            }

            return userAssignedConsortiums;
        }

        internal static PERMODEL.UserAssignedConsortium CreateUserAssignedConsortium(UserAssignedConsortium entityUserAssignedConsortium)
        {
            return new PERMODEL.UserAssignedConsortium
            {
                UserAssignedConsortiumId = entityUserAssignedConsortium.UserAssignedConsortiumCode,
                UserId = entityUserAssignedConsortium.UserId,
                NitAssignedConsortium = entityUserAssignedConsortium.NitAssociationType
            };
        }

        internal static List<PERMODEL.Consortium> CreateCoConsortiums(BusinessCollection businessObjects)
        {
            List<Models.Consortium> consortiums = new List<Models.Consortium>();

            foreach (CoConsortium coConsortium in businessObjects)
            {
                consortiums.Add(ModelAssembler.Createconsortiums(coConsortium));
            }

            return consortiums;
        }

        internal static PERMODEL.Consortium Createconsortiums(CoConsortium coConsortium)
        {
            return new PERMODEL.Consortium
            {
                InsuredCode = coConsortium.InsuredCode,
                IndividualId = coConsortium.IndividualId,
                ConsortiumId = coConsortium.ConsortiumId,
                Ismain = coConsortium.IsMain,
                ParticipationRate = coConsortium.ParticipationRate,
                StartDate = coConsortium.StartDate,
                Enabled = coConsortium.Enabled
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
                IsPrincipal = addressEntity.IsMailingAddress,
                AddressType = new Models.AddressType() { Id = addressEntity.AddressTypeCode },
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
                SmallDescription = documentType.SmallDescription,
                IsAlphanumeric = documentType.IsAlphanumeric
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
        public static Models.Partner CreateIndividualPartNer(IndividualPartner indParner)
        {
            Models.IdentificationDocument identificationDocument = new Models.IdentificationDocument();

            identificationDocument.Number = indParner.IdCardNo;
            identificationDocument.DocumentType = new Models.DocumentType() { Id = indParner.IdCardTypeCode };

            return new Models.Partner
            {

                PartnerId = indParner.PartnerId,
                IndividualId = indParner.IndividualId,
                TradeName = indParner.TradeName,
                Active = indParner.Active,
                IdentificationDocument = identificationDocument
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
                IsPrincipal = email.IsMailingAddress,
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
        private static Models.PersonType CreatePersonType(UniquePersonV1.Entities.PersonType personType)
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
            foreach (UniquePersonV1.Entities.PersonType field in businessCollection)
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

        #region LabourPerson

        /// <summary>
        /// Creates the person job.
        /// </summary>
        /// <param name="personJob">The person job.</param>
        /// <returns></returns>
        public static Models.LabourPerson CreatePersonJob(PersonJob personJob)
        {
            PERMODEL.Occupation Occupation = new Models.Occupation();
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

            Models.EducativeLevel educativeLevel = new Models.EducativeLevel();
            if (educativeLevel != null)
            {
                educativeLevel.Id = Convert.ToInt32(educativeLevel.Id);
            }

            Models.HouseType housetype = new Models.HouseType();
            if (housetype != null)
            {
                housetype.Id = Convert.ToInt32(housetype.Id);
            }

            Models.SocialLayer sociallayer = new Models.SocialLayer();
            if (sociallayer != null)
            {
                sociallayer.Id = Convert.ToInt32(sociallayer.Id);
            }

            Models.PersonType persontype = new Models.PersonType();
            if (persontype != null)
            {
                persontype.Id = Convert.ToInt32(persontype.Id);
            }


            return new Models.LabourPerson
            {
                IndividualType = Services.UtilitiesServices.Enums.IndividualType.Person,
                CustomerType = Services.UtilitiesServices.Enums.CustomerType.Individual,
                Id = personJob.IndividualId,
                Occupation = new Models.Occupation() { Id = personJob.OccupationCode },
                IncomeLevel = incomeLevel,
                CompanyName = personJob.CompanyName,
                JobSector = personJob.JobSector,
                Position = personJob.Position,
                Contact = personJob.Contact,
                CompanyPhone = phones,
                Speciality = speciality,
                OtherOccupation = Occupation,
                EducativeLevel = new Models.EducativeLevel { Id = educativeLevel.Id },
                HouseType = new Models.HouseType { Id = housetype.Id },
                SocialLayer = new Models.SocialLayer { Id = sociallayer.Id },
                PersonType = new Models.PersonType { Id = persontype.Id }

            };

        }

        /// <summary>
        /// Creates the person job.
        /// </summary>
        /// <param name="personJob">The person job.</param>
        /// <returns></returns>
        public static Models.LabourPerson CreatePersonJob(PersonJob personJob, Person person, List<Models.PersonInterestGroup> personInterestGroups)
        {

            int EducativeLevelCode = 0;
            int HouseTypeCode = 0;
            int SocialLayerCode = 0;
            int PersonTypeCode = 0;
            int BirthCountryCode = 0;

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
            if (person.PersonTypeCode != null)
            {
                PersonTypeCode = (int)person.PersonTypeCode;
            }
            if (person.BirthCountryCode != null)
            {
                BirthCountryCode = (int)person.BirthCountryCode;
            }

            PERMODEL.Occupation Occupation = new Models.Occupation();
            if (personJob?.OtherOccupationCode != null)
            {
                Occupation.Id = (int)personJob.OtherOccupationCode;
            }
            return new Models.LabourPerson
            {
                Id = personJob?.IndividualId,
                IndividualId = person.IndividualId,
                IndividualType = Services.UtilitiesServices.Enums.IndividualType.Person,
                CustomerType = Services.UtilitiesServices.Enums.CustomerType.Individual,
                Occupation = personJob?.OccupationCode == null ? null : new Models.Occupation() { Id = personJob.OccupationCode },
                IncomeLevel = personJob?.IncomeLevelCode == null ? null : new modelsPerson.IncomeLevel { Id = (int)personJob.IncomeLevelCode },
                CompanyName = personJob?.CompanyName,
                JobSector = personJob?.JobSector,
                Position = personJob?.Position,
                Contact = personJob?.Contact,
                CompanyPhone = personJob?.CompanyPhone == null ? null : new modelsPerson.Phone { Id = (int)personJob.CompanyPhone },
                Speciality = personJob?.SpecialityCode == null ? null : new modelsPerson.Speciality { Id = (int)personJob.SpecialityCode },
                OtherOccupation = Occupation,
                EducativeLevel = person.EducativeLevelCode == null ? null : new Models.EducativeLevel() { Id = EducativeLevelCode },
                HouseType = person.HouseTypeCode == null ? null : new Models.HouseType { Id = HouseTypeCode },
                SocialLayer = person.SocialLayerCode == null ? null : new Models.SocialLayer { Id = SocialLayerCode },
                PersonType = person.PersonTypeCode == null ? null : new Models.PersonType { Id = PersonTypeCode },
                Children = person.Children,
                SpouseName = person.SpouseName,
                BirthCountryId = BirthCountryCode,
                PersonInterestGroup = personInterestGroups
            };

        }

        /// <summary>
        /// Creates the person jobs.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.LabourPerson> CreatePersonJobs(BusinessCollection businessCollection)
        {
            List<Models.LabourPerson> personJobs = new List<Models.LabourPerson>();

            foreach (PersonJob field in businessCollection)
            {
                personJobs.Add(ModelAssembler.CreatePersonJob(field));
            }

            return personJobs;
        }


        #endregion LabourPerson

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
            if (person == null)
            {
                return new Models.Person(); ;
            }
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
                CustomerType = Services.UtilitiesServices.Enums.CustomerType.Individual,
                IndividualId = person.IndividualId,
                Gender = person.Gender,
                BirthDate = person.BirthDate,
                BirthPlace = person.BirthPlace,
                Children = person.Children,
                FullName = person.Name,
                IdentificationDocument = new Models.IdentificationDocument() { DocumentType = new Models.DocumentType() { Id = person.IdCardTypeCode }, Number = person.IdCardNo },
                SurName = person.Surname,
                SecondSurName = person.MotherLastName,
                Name = person.Surname + " " + person.MotherLastName + " " + person.Name,
                EducativeLevel = new Models.EducativeLevel() { Id = EducativeLevelCode },
                SpouseName = person.SpouseName,
                HouseType = new modelsPerson.HouseType { Id = HouseTypeCode },
                SocialLayer = new Models.SocialLayer() { Id = SocialLayerCode },
                MaritalStatus = new Models.MaritalStatus() { Id = person.MaritalStatusCode },
                EconomicActivity = new modelsPerson.EconomicActivity { Id = person.EconomicActivityCode == null ? 0 : (int)person.EconomicActivityCode },
                IndividualType = Services.UtilitiesServices.Enums.IndividualType.Person,
                CheckPayable = person.CheckPayable,
                //todo ricardo
                DataProtection = person.DataProtection
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
        public static Models.PaymentAccount CreatePaymentMethodAccount(PaymentMethodAccount paymentMethodAccount)
        {
            return new Models.PaymentAccount
            {
                Id = paymentMethodAccount.PaymentId,
                Type = new Models.PaymentAccountType() { Id = paymentMethodAccount.PaymentAccountTypeCode },
                Number = paymentMethodAccount.AccountNumber,
                BankBranch = new modelsCommon.BankBranch
                {
                    Id = paymentMethodAccount.BankBranchNumber == null ? 0 : paymentMethodAccount.BankBranchNumber.Value,
                    Bank = new modelsCommon.Bank()
                    {
                        Id = paymentMethodAccount.BankCode == null ? 0 : paymentMethodAccount.BankCode.Value
                    }
                },
                Currency = new modelsCommon.Currency() { Id = paymentMethodAccount.CurrencyCode == null ? 0 : paymentMethodAccount.CurrencyCode }
            };

        }

        /// <summary>
        /// Creates the payment method accounts.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.PaymentAccount> CreatePaymentMethodAccounts(BusinessCollection businessCollection)
        {
            List<Models.PaymentAccount> paymentMethodAccounts = new List<Models.PaymentAccount>();

            foreach (PaymentMethodAccount field in businessCollection)
            {
                paymentMethodAccounts.Add(ModelAssembler.CreatePaymentMethodAccount(field));
            }

            return paymentMethodAccounts;
        }

        internal static List<Models.Role> CreateRoles(BusinessCollection businessCollection)
        {
            List<Models.Role> roles = new List<modelsPerson.Role>();
            foreach (ENTV1.Role rol in businessCollection)
            {
                roles.Add(CreateRole(rol));
            }
            return roles;
        }

        internal static Models.Role CreateRole(UniquePersonV1.Entities.Role rol)
        {
            return new modelsPerson.Role()
            {
                Id = rol.RoleCode,
                Description = rol.Description
            };
        }


        #endregion

        #region Role
        public static Models.IndividualPaymentMethod CreatePaymentMethod(IndividualPaymentMethod paymentMethod)
        {
            return new Models.IndividualPaymentMethod
            {
                Id = paymentMethod.PaymentId,
                Method = new modelsPerson.PaymentMethod() { Id = paymentMethod.PaymentMethodCode },
                Account = new modelsPerson.PaymentAccount(),
                Rol = new modelsPerson.Role() { Id = paymentMethod.RoleCode },
                IsEnabled = paymentMethod.Enabled
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
                AgentDeclinedType = agent.AgentDeclinedTypeCode == null ? null : new Models.AgentDeclinedType() { Id = (int)agent.AgentDeclinedTypeCode },
                DateCurrent = agent.EnteredDate,
                DateDeclined = agent.DeclinedDate,
                Annotations = agent.Annotations,
                DateModification = agent.ModifyDate,
                GroupAgent = agent.AgentGroupCode == null ? null : new Models.GroupAgent() { Id = (int)agent.AgentGroupCode },
                SalesChannel = agent.SalesChannelCode == null ? null : new Models.SalesChannel() { Id = (int)agent.SalesChannelCode },
                EmployeePerson = agent.AccExecutiveIndId == null ? null : new Models.EmployeePerson() { Id = (int)agent.AccExecutiveIndId },
                Locker = agent.Locker,
                CommissionDiscountAgreement = Convert.ToBoolean(agent.CommissionDiscountAgreement == null ? 0 : agent.CommissionDiscountAgreement)
            };
        }
        public static List<Models.Agent> CreateAgents(List<Agent> agents)
        {
            var immaper = CreateMapAgent();
            return immaper.Map<List<Agent>, List<Models.Agent>>(agents);
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
                Number = company.TributaryIdNo,

            };

            Models.CompanyType companyType = new Models.CompanyType
            {
                Id = company.CompanyTypeCode
            };

            return new Models.Company
            {
                IndividualId = company.IndividualId,
                FullName = company.TradeName,
                IdentificationDocument = identificationDocument,
                CountryId = company.CountryCode,
                CompanyType = companyType,
                IndividualType = Services.UtilitiesServices.Enums.IndividualType.Company,
                CustomerType = Services.UtilitiesServices.Enums.CustomerType.Individual,
                CheckPayable = company.CheckPayable,
                EconomicActivity = new modelsPerson.EconomicActivity { Id = company.EconomicActivityCode == null ? 0 : (int)company.EconomicActivityCode }

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
                AgentDeclinedType = agency.AgentDeclinedTypeCode == null ? null : new Models.AgentDeclinedType
                {
                    Id = (int)agency.AgentDeclinedTypeCode
                },
                Annotations = agency.Annotations,


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
            var immaper = CreateMapAgencies();
            return immaper.Map<List<AgentAgency>, List<Models.Agency>>(agenciesEntities);
        }

        #endregion

        #region Insured

        public static Models.Insured CreateInsured(Insured insured)
        {
            return new Models.Insured
            {
                IndividualId = insured.IndividualId,
                FullName = insured.CheckPayableTo,
                InsuredCode = insured.InsuredCode > 0 ? insured.InsuredCode : insured.PrimaryKey["InsuredCode"] != null ? (int)insured.PrimaryKey["InsuredCode"] : 0,
                EnteredDate = insured.EnteredDate,
                DeclinedDate = insured.DeclinedDate,
                DeclinedType = insured.InsDeclinedTypeCode == null ? null : new Models.InsuredDeclinedType { Id = (int)insured.InsDeclinedTypeCode },
                Annotations = insured.Annotations,
                Profile = new Models.InsuredProfile { Id = insured.InsProfileCode },
                Segment = new Models.InsuredSegment { Id = insured.InsSegmentCode },
                Branch = new modelsCommon.Branch { Id = insured.BranchCode },
                ModifyDate = insured.ModifyDate,
                IsMailAddress = (insured.IsMailAddress == 1) ? true : false,
                IsSMS = (insured.IsSms == 1) ? true : false,
                ReferedBy = insured.ReferredBy,
                ElectronicBiller = insured.ElectronicBiller,
                RegimeType = insured.RegimeType,
            };
        }

        public static List<Models.Insured> CreateInsureds(BusinessCollection businessCollection)
        {
            List<Models.Insured> insureds = new List<Models.Insured>();

            foreach (Insured field in businessCollection)
            {
                insureds.Add(ModelAssembler.CreateInsured(field));

            }

            return insureds;
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
                IndividualType = (Services.UtilitiesServices.Enums.IndividualType)individual.IndividualTypeCode,
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
                SmallDescription = identityCardType.SmallDescription,
                IsAlphanumeric = identityCardType.IsAlphanumeric
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
                SmallDescription = tributaryIdentityType.SmallDescription,
                IsAlphanumeric = tributaryIdentityType.IsAlphanumeric
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


        #region FiscalResponsibility

        public static Models.FiscalResponsibility CreateFiscalresponsibility(FiscalResponsibility entityFiscalResponsibility)
        {
            return new Models.FiscalResponsibility
            {
                Id = entityFiscalResponsibility.Id,
                Code = entityFiscalResponsibility.Code,
                Description = entityFiscalResponsibility.Description
            };
        }

        public static List<Models.FiscalResponsibility> CreateFiscalresponsibilities(BusinessCollection businessCollection)
        {
            List<Models.FiscalResponsibility> fiscalResponsibilities = new List<Models.FiscalResponsibility>();
            foreach (FiscalResponsibility field in businessCollection)
            {
                fiscalResponsibilities.Add(ModelAssembler.CreateFiscalresponsibility(field));
            }
            return fiscalResponsibilities;
        }




        public static Models.InsuredFiscalResponsibility CreateInsuredFiscalResponsibility(
           InsuredFiscalResponsibility entityFiscalResponsibility)
        {
            return new Models.InsuredFiscalResponsibility
            {
                Id = entityFiscalResponsibility.Id,
                IndividualId = entityFiscalResponsibility.IndividualId,
                InsuredId = entityFiscalResponsibility.InsuredCode,
                FiscalResponsabilityId = entityFiscalResponsibility.FiscalResponsibilityId
            };
        }

        public static List<Models.InsuredFiscalResponsibility> CreateListFiscalResponsibilities(
            List<InsuredFiscalResponsibility> fiscalResponsibilities)
        {
            var result = new List<Models.InsuredFiscalResponsibility>();

            foreach (InsuredFiscalResponsibility fiscal in fiscalResponsibilities)
            {
                result.Add(CreateInsuredFiscalResponsibility(fiscal));
            }
            return result;
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
                FullName = prospectLegal.TradeName,
                IdentificationDocument = identificationDocument,
                CompanyType = companyType,
                //todo ricardo
                //Addresses = address,
                //Phones = phones,
                //Emails = emails
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
                //todo Ricardo
                //PersonCode = Convert.ToInt32(prospectNatural.ProspectId),
                Gender = prospectNatural.Gender,
                BirthDate = Convert.ToDateTime(prospectNatural.BirthDate),
                FullName = prospectNatural.Name,
                IdentificationDocument = new Models.IdentificationDocument() { DocumentType = new Models.DocumentType() { Id = Convert.ToInt32(prospectNatural.IdCardTypeCode) }, Number = prospectNatural.IdCardNo },
                SurName = prospectNatural.Surname,
                SecondSurName = prospectNatural.MotherLastName,
                Name = prospectNatural.Surname + " " + prospectNatural.MotherLastName + " " + prospectNatural.Name,
                MaritalStatus = new Models.MaritalStatus() { Id = Convert.ToInt32(prospectNatural.MaritalStatusCode) },
                //todo ricardo
                //City = city,
                //Phones = phones,
                //Emails = emails,
                //Addresses = address
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

        public static Models.Company CreateCompanyProspect(Prospect prospectNatural)
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

            return new Models.Company
            {
                //todo Ricardo
                //PersonCode = Convert.ToInt32(prospectNatural.ProspectId),
                //Gender = prospectNatural.Gender,
                //BirthDate = Convert.ToDateTime(prospectNatural.BirthDate),
                //FullName = prospectNatural.Name,
                //IdentificationDocument = new Models.IdentificationDocument() { DocumentType = new Models.DocumentType() { Id = Convert.ToInt32(prospectNatural.IdCardTypeCode) }, Number = prospectNatural.IdCardNo },
                //SurName = prospectNatural.Surname,
                //SecondSurName = prospectNatural.MotherLastName,
                //Name = prospectNatural.Surname + " " + prospectNatural.MotherLastName + " " + prospectNatural.Name,
                //MaritalStatus = new Models.MaritalStatus() { Id = Convert.ToInt32(prospectNatural.MaritalStatusCode) },
                //todo ricardo
                //City = city,
                //Phones = phones,
                //Emails = emails,
                //Addresses = address
            };

        }
        public static List<Models.Company> CreateCompanyProspects(BusinessCollection businessCollection)
        {
            List<Models.Company> prospects = new List<Models.Company>();
            foreach (Prospect item in businessCollection)
            {
                prospects.Add(ModelAssembler.CreateCompanyProspect(item));
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
                //city.State = new modelsCommon.State
                //{
                //    Country = new modelsCommon.Country { Id = prospectNatural.CountryCode.Value },
                //    Id = (int)prospectNatural.City.State.Country.Id
                //};

                //city = DelegateService.commonServiceCore.GetCityByCity(city);
            }
            return new Models.ProspectNatural
            {
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
                City = city,
                AdditionalInfo = prospectNatural.AdditionalInfo
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
                Surname = person.SurName,
                Name = person.SurName + person.Name,
                MotherLastName = person.SecondSurName,
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
                Name = company.FullName,

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
                AssociationType = new Models.AssociationType { Id = coCompany.AssociationTypeCode, NitAssociationType = coCompany.NitAssociationType }

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
            if (coCompanyName == null)
            {
                return null;
            }
            return new Models.CompanyName
            {
                IndividualId = coCompanyName.IndividualId,
                NameNum = coCompanyName.NameNum,
                TradeName = coCompanyName.TradeName,
                Address = new Models.Address { Id = coCompanyName.AddressDataCode == null ? 0 : (Int32)coCompanyName.AddressDataCode },
                Phone = new Models.Phone { Id = coCompanyName.PhoneDataCode == null ? 0 : (Int32)coCompanyName.PhoneDataCode },
                Email = new Models.Email { Id = coCompanyName.EmailDataCode == null ? 0 : (Int32)coCompanyName.EmailDataCode },
                IsMain = coCompanyName.IsMain,
                Enabled = coCompanyName.Enabled
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
                //IndividualId = person.IndividualId,
                //FullName = person.Surname,
                //EconomicActivity = new modelsPerson.EconomicActivity { Id = person.EconomicActivityCode.Value },
                //BirthDate = person.BirthDate,
                //CustomerType = Services.UtilitiesServices.Enums.CustomerType.Individual
            };
            //if (person.MotherLastName != null)
            //{
            //    insured.Name += " " + person.MotherLastName;
            //}
            //insured.Name += " " + person.Name;

            return insured;
        }

        public static Models.Insured CreateInsuredCompany(Company company)
        {
            return new Models.Insured
            {
                //IndividualId = company.IndividualId,
                //FullName = company.TradeName,
                //EconomicActivity = new modelsPerson.EconomicActivity { Id = company.EconomicActivityCode.Value },
                //CustomerType = Services.UtilitiesServices.Enums.CustomerType.Individual
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
                IsConsortium = insuredConcept.IsConsortium,
                IsPayer = insuredConcept.IsPayer,
                IsRepresentative = insuredConcept.IsRepresentative,
                IsSurety = insuredConcept.IsSurety

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

        #region InsuredAgent

        public static Models.InsuredAgent CreateInsuredAgent(UniquePersonV1.Entities.InsuredAgent insuredAgent)
        {
            return new Models.InsuredAgent
            {
                InsuredIndId = new Models.Insured { IndividualId = insuredAgent.InsuredIndId },
                AgentIndId = new Models.Agent { IndividualId = insuredAgent.AgentIndId },
                AgentAgencyId = new Models.Agency { Id = insuredAgent.AgentAgencyId },
                IsMain = insuredAgent.IsMain,

            };
        }
        public static List<Models.InsuredAgent> CreateInsuredAgents(BusinessCollection businessCollection)
        {
            List<Models.InsuredAgent> insuredAgent = new List<Models.InsuredAgent>();
            foreach (InsuredAgent field in businessCollection)
            {
                insuredAgent.Add(ModelAssembler.CreateInsuredAgent(field));

            }
            return insuredAgent;
        }

        #endregion InsuredAgent
        #region PaymentMethod
        public static List<Models.IndividualPaymentMethod> CreatePaymentMethods(BusinessCollection businessCollection)
        {
            List<Models.IndividualPaymentMethod> individualPaymentMethod = new List<Models.IndividualPaymentMethod>();

            foreach (IndividualPaymentMethod field in businessCollection)
            {
                individualPaymentMethod.Add(CreatePaymentMethod(field));
            }

            return individualPaymentMethod;
        }
        #endregion


        #region Guarantee

        public static Models.Guarantee CreateGuarantee(Guarantee guarantee, InsuredGuaranteeViewV1 insuredGuaranteeView)
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
                            guaranteeStatus = new Models.GuaranteeStatus
                            {
                                Id = gs.GuaranteeStatusCode,
                                Description = gs.Description,
                                IsEnabledInd = gs.EnabledInd,
                                IsRemoveInd = gs.RemoveInd,
                                IsValidateDocument = gs.ValidateDocument
                            };
                            insuredGuarantee = CreateInsuredGuarantee(ig, guaranteeStatus);
                            break;
                        }
                    }
                    break;
                }
            }

            return new Models.Guarantee
            {
                Id = guarantee.GuaranteeCode,
                Description = guarantee.Description,
                HasApostille = guarantee.Apostille,
                Type = guaranteeType,
            };
        }

        public static List<Models.Guarantee> CreateGuarantees(InsuredGuaranteeViewV1 businessCollection)
        {
            List<Models.Guarantee> guarantees = new List<Models.Guarantee>();

            foreach (Guarantee field in businessCollection.Guarantees)
            {
                guarantees.Add(ModelAssembler.CreateGuarantee(field, businessCollection));
            }

            return guarantees;
        }

        public static Models.Guarantee CreateGuarantee(Guarantee guarantee, GuaranteeViewV1 guaranteeView)
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
                Id = guarantee.GuaranteeCode,
                Description = guarantee.Description,
                HasApostille = guarantee.Apostille,
                Type = guaranteeType,
                HasPromissoryNote = guarantee.PromissoryNoteTypeInd
            };

        }

        public static List<Models.Guarantee> CreateGuarantees(GuaranteeViewV1 businessCollection)
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
                IsDocument = guaranteeType.IsDocument,
                Class = new modelsPerson.GuaranteeClass()
                {
                    Id = guaranteeType.GuaranteeClassCode
                },
                IsOthers = guaranteeType.IsOthers,
                IsPromissoryNote = guaranteeType.IsPromissoryNote,
                IsVehicle = guaranteeType.IsVehicle,
                Description = guaranteeType.Description,
                GuaranteeTypeId = guaranteeType.GuaranteeTypeCode

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

        public static Models.InsuredGuaranteeMortgage CreateInsuredGuaranteeMortgage(InsuredGuarantee insuredGuarantee)
        {
            return new Models.InsuredGuaranteeMortgage()
            {
                Id = insuredGuarantee.GuaranteeId,
                Description = insuredGuarantee.Description,
                IndividualId = insuredGuarantee.IndividualId,

                AppraisalAmount = Convert.ToDecimal(insuredGuarantee.AppraisalAmount),
                AppraisalDate = Convert.ToDateTime(insuredGuarantee.AppraisalDate),
                ExpertName = insuredGuarantee.ExpertName,
                AssetType = new modelsPerson.AssetType()
                {
                    Code = Convert.ToInt32(insuredGuarantee.AssetTypeCode)
                },
                RegistrationNumber = insuredGuarantee.RegistrationNumber,
                InsuranceValueAmount = Convert.ToDecimal(insuredGuarantee.InsuranceValueAmount),
                MeasureAreaQuantity = Convert.ToDecimal(insuredGuarantee.MeasureAreaQuantity),
                BuiltAreaQuantity = Convert.ToDecimal(insuredGuarantee.MeasureAreaQuantity),
                MeasurementType = new modelsPerson.MeasurementType()
                {
                    Id = Convert.ToInt32(insuredGuarantee.MeasurementTypeCode)
                },
                InsuranceCompany = insuredGuarantee.InsuranceCompany,
                InsuranceCompanyId = Convert.ToDecimal(insuredGuarantee.InsuranceCompanyId),
                PolicyNumber = insuredGuarantee.GuaranteePolicyNumber,
                SignatoriesNumber = Convert.ToInt32(insuredGuarantee.SignatoriesNum),

                Address = insuredGuarantee.Address,
                Guarantee = new modelsPerson.Guarantee()
                {
                    Id = Convert.ToInt32(insuredGuarantee.GuaranteeCode),
                    HasApostille = insuredGuarantee.Apostille
                },
                Branch = new modelsCommon.Branch()
                {
                    Id = Convert.ToInt32(insuredGuarantee.BranchCode)
                },
                City = new modelsCommon.City()
                {
                    Id = Convert.ToInt32(insuredGuarantee.CityCode),
                    State = new modelsCommon.State()
                    {
                        Id = Convert.ToInt32(insuredGuarantee.StateCode),
                        Country = new modelsCommon.Country()
                        {
                            Id = Convert.ToInt32(insuredGuarantee.CountryCode)
                        }
                    }
                },
                Currency = new modelsCommon.Currency()
                {
                    Id = Convert.ToInt32(insuredGuarantee.CountryCode)
                },
                ClosedInd = insuredGuarantee.ClosedInd,
                IsCloseInd = insuredGuarantee.ClosedInd,
                LastChangeDate = insuredGuarantee.LastChangeDate,
                RegistrationDate = insuredGuarantee.RegistrationDate,
                Status = new modelsPerson.GuaranteeStatus()
                {
                    Id = Convert.ToInt32(insuredGuarantee.GuaranteeStatusCode)
                }

            };

        }

        public static Models.InsuredGuaranteePledge CreateInsuredGuaranteePledge(InsuredGuarantee insuredGuarantee)
        {
            return new modelsPerson.InsuredGuaranteePledge()
            {
                Id = insuredGuarantee.GuaranteeId,
                Description = insuredGuarantee.Description,

                IndividualId = insuredGuarantee.IndividualId,
                Address = insuredGuarantee.Address,
                Guarantee = new modelsPerson.Guarantee()
                {
                    Id = Convert.ToInt32(insuredGuarantee.GuaranteeCode)
                },
                Branch = new modelsCommon.Branch()
                {
                    Id = Convert.ToInt32(insuredGuarantee.BranchCode)
                },
                City = new modelsCommon.City()
                {
                    Id = Convert.ToInt32(insuredGuarantee.CityCode),
                    State = new modelsCommon.State()
                    {
                        Id = Convert.ToInt32(insuredGuarantee.StateCode),
                        Country = new modelsCommon.Country()
                        {
                            Id = Convert.ToInt32(insuredGuarantee.CountryCode)

                        }
                    },
                },
                Currency = new modelsCommon.Currency()
                {
                    Id = Convert.ToInt32(insuredGuarantee.CurrencyCode)
                },
                IsCloseInd = insuredGuarantee.ClosedInd,
                ClosedInd = insuredGuarantee.ClosedInd,
                RegistrationDate = insuredGuarantee.RegistrationDate,
                LastChangeDate = insuredGuarantee.LastChangeDate,
                Status = new modelsPerson.GuaranteeStatus()
                {
                    Id = Convert.ToInt32(insuredGuarantee.GuaranteeStatusCode)
                },

                AppraisalAmount = Convert.ToDecimal(insuredGuarantee.AppraisalAmount),
                AppraisalDate = Convert.ToDateTime(insuredGuarantee.AppraisalDate),
                LicensePlate = insuredGuarantee.LicensePlate,
                EngineNumer = insuredGuarantee.EngineSerNro,
                ChassisNumer = insuredGuarantee.ChassisSerNo,
                InsuranceCompanyId = Convert.ToDecimal(insuredGuarantee.InsuranceCompanyId),
                InsuranceCompany = insuredGuarantee.InsuranceCompany,
                PolicyNumber = insuredGuarantee.GuaranteePolicyNumber,
                InsuranceValueAmount = Convert.ToDecimal(insuredGuarantee.InsuranceValueAmount)
            };
        }

        public static Models.InsuredGuaranteePromissoryNote CreateInsuredGuaranteePromissoryNote(InsuredGuarantee insuredGuarantee)
        {
            return new modelsPerson.InsuredGuaranteePromissoryNote()
            {
                Id = insuredGuarantee.GuaranteeId,
                Description = insuredGuarantee.Description,

                IndividualId = insuredGuarantee.IndividualId,
                Address = insuredGuarantee.Address,
                Guarantee = new modelsPerson.Guarantee()
                {
                    Id = Convert.ToInt32(insuredGuarantee.GuaranteeCode)
                },
                Branch = new modelsCommon.Branch()
                {
                    Id = Convert.ToInt32(insuredGuarantee.BranchCode)
                },
                City = new modelsCommon.City()
                {
                    Id = Convert.ToInt32(insuredGuarantee.CityCode),
                    State = new modelsCommon.State()
                    {
                        Id = Convert.ToInt32(insuredGuarantee.StateCode),
                        Country = new modelsCommon.Country()
                        {
                            Id = Convert.ToInt32(insuredGuarantee.CountryCode)

                        }
                    },
                },
                Currency = new modelsCommon.Currency()
                {
                    Id = Convert.ToInt32(insuredGuarantee.CurrencyCode)
                },
                IsCloseInd = insuredGuarantee.ClosedInd,
                ClosedInd = insuredGuarantee.ClosedInd,
                RegistrationDate = insuredGuarantee.RegistrationDate,
                LastChangeDate = insuredGuarantee.LastChangeDate,
                Status = new modelsPerson.GuaranteeStatus()
                {
                    Id = Convert.ToInt32(insuredGuarantee.GuaranteeStatusCode)
                },

                DocumentNumber = insuredGuarantee.DocumentNumber,
                DocumentValueAmount = Convert.ToDecimal(insuredGuarantee.DocumentValueAmount),
                ExtDate = Convert.ToDateTime(insuredGuarantee.ExpDate),
                PromissoryNoteType = new modelsPerson.PromissoryNoteType()
                {
                    Id = Convert.ToInt32(insuredGuarantee.PromissoryNoteTypeCode)
                },
                SignatoriesNumber = Convert.ToInt32(insuredGuarantee.SignatoriesNum),
                ConstitutionDate = Convert.ToDateTime(insuredGuarantee.ConstitutionDate)

            };
        }

        public static Models.InsuredGuaranteeFixedTermDeposit CreateInsuredGuaranteeFixedTermDeposit(InsuredGuarantee insuredGuarantee)
        {
            return new modelsPerson.InsuredGuaranteeFixedTermDeposit()
            {
                Id = insuredGuarantee.GuaranteeId,
                Description = insuredGuarantee.Description,

                IndividualId = insuredGuarantee.IndividualId,
                Address = insuredGuarantee.Address,
                Guarantee = new modelsPerson.Guarantee()
                {
                    Id = Convert.ToInt32(insuredGuarantee.GuaranteeCode)
                },
                Branch = new modelsCommon.Branch()
                {
                    Id = Convert.ToInt32(insuredGuarantee.BranchCode)
                },
                City = new modelsCommon.City()
                {
                    Id = Convert.ToInt32(insuredGuarantee.CityCode),
                    State = new modelsCommon.State()
                    {
                        Id = Convert.ToInt32(insuredGuarantee.StateCode),
                        Country = new modelsCommon.Country()
                        {
                            Id = Convert.ToInt32(insuredGuarantee.CountryCode)

                        }
                    },
                },
                Currency = new modelsCommon.Currency()
                {
                    Id = Convert.ToInt32(insuredGuarantee.CurrencyCode)
                },
                IsCloseInd = insuredGuarantee.ClosedInd,
                ClosedInd = insuredGuarantee.ClosedInd,
                RegistrationDate = insuredGuarantee.RegistrationDate,
                LastChangeDate = insuredGuarantee.LastChangeDate,
                Status = new modelsPerson.GuaranteeStatus()
                {
                    Id = Convert.ToInt32(insuredGuarantee.GuaranteeStatusCode)
                },

                DocumentNumber = insuredGuarantee.DocumentNumber,
                DocumentValueAmount = Convert.ToDecimal(insuredGuarantee.DocumentValueAmount),
                ExtDate = Convert.ToDateTime(insuredGuarantee.ExpDate),
                ConstitutionDate = Convert.ToDateTime(insuredGuarantee.ConstitutionDate),
                IssuerName = insuredGuarantee.IssuerName

            };
        }

        internal static Models.InsuredGuaranteeOthers CreateInsuredGuaranteeOthers(InsuredGuarantee insuredGuaranteeEntity)
        {
            return new Models.InsuredGuaranteeOthers()
            {

                Id = insuredGuaranteeEntity.GuaranteeId,
                DescriptionOthers = insuredGuaranteeEntity.GuaranteeDescriptionOthers,
                IndividualId = insuredGuaranteeEntity.IndividualId,

                Guarantee = new modelsPerson.Guarantee()
                {
                    Id = Convert.ToInt32(insuredGuaranteeEntity.GuaranteeCode)
                },
                Branch = new modelsCommon.Branch()
                {
                    Id = Convert.ToInt32(insuredGuaranteeEntity.BranchCode)
                },
                IsCloseInd = insuredGuaranteeEntity.ClosedInd,
                ClosedInd = insuredGuaranteeEntity.ClosedInd,
                RegistrationDate = insuredGuaranteeEntity.RegistrationDate,
                LastChangeDate = insuredGuaranteeEntity.LastChangeDate,
                Status = new modelsPerson.GuaranteeStatus()
                {
                    Id = Convert.ToInt32(insuredGuaranteeEntity.GuaranteeStatusCode)
                }
            };
        }


        public static Models.InsuredGuarantee CreateInsuredGuarantee(InsuredGuarantee insuredGuarantee, Models.GuaranteeStatus guaranteeStatus)
        {
            return new Models.InsuredGuarantee
            {
                //Id = insuredGuarantee.GuaranteeId,
                //IndividualId = insuredGuarantee.IndividualId,
                City = new modelsCommon.City()
                {
                    Id = Convert.ToInt32(insuredGuarantee.CityCode)
                },
                ClosedInd = insuredGuarantee.ClosedInd,
                IsCloseInd = insuredGuarantee.ClosedInd,
                Currency = new modelsCommon.Currency()
                {
                    Id = Convert.ToInt32(insuredGuarantee.CurrencyCode)
                },
                Guarantee = new modelsPerson.Guarantee()
                {
                    Id = insuredGuarantee.GuaranteeId,
                    Description = insuredGuarantee.GuaranteeDescriptionOthers,
                    HasApostille = insuredGuarantee.Apostille

                },
                Address = insuredGuarantee.Address,
                Description = insuredGuarantee.Description,
                //  AppraisalDate = insuredGuarantee.AppraisalDate,
                // ExpertName = insuredGuarantee.ExpertName,
                RegistrationDate = insuredGuarantee.RegistrationDate,
                LastChangeDate = insuredGuarantee.LastChangeDate,
                Status = new modelsPerson.GuaranteeStatus()
                {
                    Id = guaranteeStatus.Id,
                    Description = guaranteeStatus.Description,
                    IsEnabledInd = guaranteeStatus.IsEnabledInd,
                    IsEnabledSubscription = guaranteeStatus.IsEnabledSubscription,
                    IsRemoveInd = guaranteeStatus.IsRemoveInd,
                    IsValidateDocument = guaranteeStatus.IsValidateDocument,
                    SmallDescription = guaranteeStatus.SmallDescription
                }


                //ValueAmount = insuredGuarantee.InsuranceValueAmount,
                //GuaranteeStatus = guaranteeStatus,
                //Code = insuredGuarantee.GuaranteeCode,
                //AppraisalAmount = insuredGuarantee.AppraisalAmount,
                //BuiltArea = insuredGuarantee.BuiltAreaQuantity,
                //DeedNumber = insuredGuarantee.DeedNumber,
                //DescriptionOthers = insuredGuarantee.GuaranteeDescriptionOthers,
                //DocumentValueAmount = insuredGuarantee.DocumentValueAmount,
                //MeasureArea = insuredGuarantee.MeasureAreaQuantity,
                //MortgagerName = insuredGuarantee.MortgagerName,
                //DepositEntity = insuredGuarantee.DepositEntity,
                //DepositDate = insuredGuarantee.DepositDate,
                //Depositor = insuredGuarantee.Depositor,
                //Constituent = insuredGuarantee.Constituent,

                //InsuranceAmount = insuredGuarantee.InsuranceValueAmount,
                //PolicyNumber = insuredGuarantee.GuaranteePolicyNumber,
                //Apostille = insuredGuarantee.Apostille,
                //IssuerName = insuredGuarantee.IssuerName,
                //DocumentNumber = insuredGuarantee.DocumentNumber,
                //ExpirationDate = insuredGuarantee.ExpDate,
                //BusinessLineCode = insuredGuarantee.LineBusinessCode,
                //RegistrationNumber = insuredGuarantee.RegistrationNumber,
                //LicensePlate = insuredGuarantee.LicensePlate,
                //EngineNro = insuredGuarantee.EngineSerNro,
                //ChassisNro = insuredGuarantee.ChassisSerNo,
                //SignatoriesNumber = insuredGuarantee.SignatoriesNum,
                //GuaranteeAmount = insuredGuarantee.GuaranteeAmount,

                //VehicleMake = (short?)insuredGuarantee.VehicleMakeCode,
                //VehicleModel = insuredGuarantee.VehicleModelCode,
                //VehicleVersion = (short?)insuredGuarantee.VehicleVersionCode,
                //AssetTypeCode = insuredGuarantee.AssetTypeCode,
                //RealstateMatriculation = insuredGuarantee.RealstateMatriculation,
                //ConstitutionDate = insuredGuarantee.ConstitutionDate,
            };
        }

        public static List<Models.InsuredGuarantee> CreateInsuredGuarantees(BusinessCollection insuredGuaranteesEntities)
        {
            List<Models.InsuredGuarantee> insuredGuarantees = new List<modelsPerson.InsuredGuarantee>();

            foreach (InsuredGuarantee item in insuredGuaranteesEntities)
            {
                insuredGuarantees.Add(CreateInsuredGuarantee(item));
            }
            return insuredGuarantees;
        }

        public static Models.InsuredGuarantee CreateInsuredGuarantee(InsuredGuarantee insuredGuarantee)
        {
            return new modelsPerson.InsuredGuarantee()
            {
                Id = insuredGuarantee.GuaranteeId,
                IndividualId = insuredGuarantee.IndividualId,
                Address = insuredGuarantee.Address,
                Branch = new modelsCommon.Branch()
                {
                    Id = Convert.ToInt32(insuredGuarantee.BranchCode)
                },
                City = new modelsCommon.City()
                {
                    Id = Convert.ToInt32(insuredGuarantee.CityCode),
                    State = new modelsCommon.State()
                    {
                        Id = Convert.ToInt32(insuredGuarantee.StateCode),
                        Country = new modelsCommon.Country()
                        {
                            Id = Convert.ToInt32(insuredGuarantee.CountryCode)
                        }
                    }
                },
                ClosedInd = insuredGuarantee.ClosedInd,
                IsCloseInd = insuredGuarantee.ClosedInd,
                Currency = new modelsCommon.Currency()
                {
                    Id = Convert.ToInt32(insuredGuarantee.CurrencyCode)
                },
                Description = insuredGuarantee.Description,
                Status = new modelsPerson.GuaranteeStatus()
                {
                    Id = Convert.ToInt32(insuredGuarantee.StateCode)
                },
                LastChangeDate = insuredGuarantee.LastChangeDate,
                RegistrationDate = insuredGuarantee.RegistrationDate,
                Guarantee = new modelsPerson.Guarantee()
                {
                    Id = Convert.ToInt32(insuredGuarantee.GuaranteeCode)
                }

            };
        }

        public static List<Models.Guarantee> CreateGuarantees(BusinessCollection businessCollection)
        {
            List<Models.Guarantee> guarantees = new List<Models.Guarantee>();

            foreach (Guarantee item in businessCollection)
            {
                guarantees.Add(ModelAssembler.CreateGuarantee(item));
            }

            return guarantees;
        }

        public static Models.Guarantee CreateGuarantee(Guarantee guaranteeEntities)
        {
            return new modelsPerson.Guarantee()
            {
                Id = guaranteeEntities.GuaranteeCode,
                Description = guaranteeEntities.Description,
                HasApostille = guaranteeEntities.Apostille,
                HasPromissoryNote = guaranteeEntities.PromissoryNoteTypeInd,
                Type = new modelsPerson.GuaranteeType()
                {
                    Id = guaranteeEntities.GuaranteeTypeCode
                }
            };
        }

        public static List<Models.Guarantor> CreateGuarantors(BusinessCollection businessCollection)
        {
            List<Models.Guarantor> Modelguarantors = new List<modelsPerson.Guarantor>();
            foreach (Guarantor item in businessCollection)
            {
                Modelguarantors.Add(CreateGuarantor(item));
            }
            return Modelguarantors;
        }


        public static Models.Guarantor CreateGuarantor(Guarantor guarantor)
        {
            return new Models.Guarantor()
            {
                Adrress = guarantor.Adrress,
                CardNro = guarantor.IdCardNo,
                CityText = guarantor.CityText,
                GuaranteeId = guarantor.GuaranteeId,
                IndividualId = guarantor.IndividualId,
                GuarantorId = guarantor.GuarantorId,
                PhoneNumber = guarantor.PhoneNumber,
                Name = guarantor.GuarantorName,
                TradeName = guarantor.TradeName,
                TributaryIdNo = guarantor.TributaryIdNo
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
                Id = guaranteeStauts.GuaranteeStatusCode,
                Description = guaranteeStauts.Description,
            };
        }

        public static Models.GuaranteeStatusRoute CreateStatusRoute(int AssignedGuaranteeStatusCd, int GuaranteeStatusCd)
        {
            return new Models.GuaranteeStatusRoute
            {
                Id = GuaranteeStatusCd,
                AssignedGuaranteeStatusCd = AssignedGuaranteeStatusCd,
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

        public static List<Models.GuaranteeStatusUser> CreateGuaranteesStatusUser(BusinessCollection businessCollection)
        {
            List<Models.GuaranteeStatusUser> guaranteeStauts = new List<Models.GuaranteeStatusUser>();

            foreach (GuaranteeStatusUser field in businessCollection)
            {
                guaranteeStauts.Add(ModelAssembler.CreateGuaranteeStatusUser(field));
            }

            return guaranteeStauts;
        }

        private static Models.GuaranteeStatusUser CreateGuaranteeStatusUser(GuaranteeStatusUser guaranteeStatusUser)
        {
            return new Models.GuaranteeStatusUser
            {
                GuaranteeStatusCode = guaranteeStatusUser.GuaranteeStatusCode,
                UserId = guaranteeStatusUser.UserId,
            };
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
                Id = measurementType.MeasurementTypeCode,
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
                IndividualType = Services.UtilitiesServices.Enums.IndividualType.Person,
                CustomerType = Services.UtilitiesServices.Enums.CustomerType.Individual,
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
                IndividualType = Services.UtilitiesServices.Enums.IndividualType.Company,
                CustomerType = Services.UtilitiesServices.Enums.CustomerType.Individual,
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
                IndividualType = (Services.UtilitiesServices.Enums.IndividualType)entityProspect.IndividualTypeCode,
                CustomerType = Services.UtilitiesServices.Enums.CustomerType.Prospect,
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

            if (prospect.IndividualType == Services.UtilitiesServices.Enums.IndividualType.Person)
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
            else if (prospect.IndividualType == Services.UtilitiesServices.Enums.IndividualType.Company)
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

        #region OperatingQuota V1

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
                LineBusinessId = operatingQuota.LineBusinessCode,
                CurrencyId = operatingQuota.CurrencyCode,
                Amount = operatingQuota.OperatingQuotaAmount,
                CurrentTo = operatingQuota.CurrentTo,
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

        #region Suppliers

        /// <summary>
        /// Creates the CreateSupplierAccountingConcept.
        /// </summary>
        /// <param name="providerType">Type of the Provider.</param>
        public static Models.SupplierAccountingConcept CreateSupplierAccountingConcept(SupplierAccountingConcept supplierAccountingConcept)
        {
            return new Models.SupplierAccountingConcept
            {
                Id = supplierAccountingConcept.SupplierAccountingConceptId,
                Supplier = new Models.Supplier() { Id = supplierAccountingConcept.SupplierCode },
                AccountingConcept = new Models.AccountingConcept() { Id = supplierAccountingConcept.AccountingConceptCode }
            };
        }

        /// <summary>
        /// Creates the CreateSupplierAccountingConcepts.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.SupplierAccountingConcept> CreateSupplierAccountingConcepts(BusinessCollection businessCollection)
        {
            List<Models.SupplierAccountingConcept> accountingConcept = new List<Models.SupplierAccountingConcept>();

            foreach (SupplierAccountingConcept field in businessCollection)
            {
                accountingConcept.Add(ModelAssembler.CreateSupplierAccountingConcept(field));
            }

            return accountingConcept;
        }

        /// <summary>
        /// Creates the CreateSupplierAccountingConcept.
        /// </summary>
        /// <param name="providerType">Type of the Provider.</param>
        public static Models.SupplierGroupSupplier CreateSupplierGroupSupplier(SupplierGroupSupplier supplierGroupSupplier)
        {
            return new Models.SupplierGroupSupplier
            {
                GroupSupplierCd = supplierGroupSupplier.GroupSupplierCode,
                SupplierCd = supplierGroupSupplier.SupplierCode,
                SupplierGroupSupplierCd = supplierGroupSupplier.SupplierGroupSupplierCode
            };
        }

        /// <summary>
        /// Creates the CreateGroupsSupplier.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.SupplierGroupSupplier> CreateSupplierGroupSupplier(BusinessCollection businessCollection)
        {
            List<Models.SupplierGroupSupplier> groupSupplier = new List<Models.SupplierGroupSupplier>();

            foreach (SupplierGroupSupplier field in businessCollection)
            {
                groupSupplier.Add(ModelAssembler.CreateSupplierGroupSupplier(field));
            }

            return groupSupplier;
        }


        /// <summary>
        /// Creates the CreateAccountingConcept.
        /// </summary>
        /// <param name="providerType">Type of the Provider.</param>
        public static Models.AccountingConcept CreateAccountingConcept(AccountingConcept accountingConcept)
        {
            return new Models.AccountingConcept
            {
                Id = accountingConcept.AccountingConceptCode,
                Description = accountingConcept.Description
            };
        }

        /// <summary>
        /// Creates the CreateAccountingConcepts.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.AccountingConcept> CreateAccountingConcepts(BusinessCollection businessCollection)
        {
            List<Models.AccountingConcept> accountingConcept = new List<Models.AccountingConcept>();

            foreach (AccountingConcept field in businessCollection)
            {
                accountingConcept.Add(ModelAssembler.CreateAccountingConcept(field));
            }

            return accountingConcept;
        }



        /// <summary>
        /// Creates the SUPPLIER_PROFILE.
        /// </summary>
        /// <param name="providerType">Type of the Provider.</param>
        public static Models.SupplierProfile CreateSupplierProfile(UniquePersonV1.Entities.SupplierProfile supplierProfile)
        {
            return new Models.SupplierProfile
            {
                Id = supplierProfile.SupplierProfileCode,
                Description = supplierProfile.Description,
                IsEnabled = (bool)supplierProfile.Enabled
            };
        }

        /// <summary>
        /// Creates the providers.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.SupplierProfile> CreateSupplierProfiles(BusinessCollection businessCollection)
        {
            List<Models.SupplierProfile> supplierProfile = new List<Models.SupplierProfile>();

            foreach (UniquePersonV1.Entities.SupplierProfile field in businessCollection)
            {
                supplierProfile.Add(ModelAssembler.CreateSupplierProfile(field));
            }

            return supplierProfile;
        }

        /// <summary>
        /// Creates the providers.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.SupplierProfile> CreateSupplierTypeProfiles(List<UniquePersonV1.Entities.SupplierTypeProfile> supplierTypeProfiles)
        {
            List<Models.SupplierProfile> supplierProfile = new List<Models.SupplierProfile>();

            foreach (UniquePersonV1.Entities.SupplierTypeProfile field in supplierTypeProfiles)
            {
                supplierProfile.Add(ModelAssembler.CreateSupplierTypeProfile(field));
            }

            return supplierProfile;
        }

        /// <summary>
        /// Creates the SUPPLIER_PROFILE.
        /// </summary>
        /// <param name="providerType">Type of the Provider.</param>
        public static Models.SupplierProfile CreateSupplierTypeProfile(UniquePersonV1.Entities.SupplierTypeProfile supplierProfile)
        {
            return new Models.SupplierProfile
            {
                Id = supplierProfile.SupplierProfileCode,
                Description = supplierProfile.Description,
                IsEnabled = supplierProfile.Enabled
            };
        }

        /// <summary>
        /// Creates the provider.
        /// </summary>       
        public static Models.Supplier CreateSupplier(Supplier supplier)
        {
            var ModelSupplier = new Models.Supplier
            {

                Id = supplier.SupplierCode,
                Profile = new Models.SupplierProfile { Id = (int)supplier.SupplierProfileCode },
                Type = new Models.SupplierType { Id = (int)supplier.SupplierTypeCode },
                EnteredDate = supplier.EnteredDate,
                DeclinedDate = supplier.DeclinedDate,
                DeclinedReason = supplier.DeclinedReason,
                Enabled = supplier.Enabled,
                CheckPayableTo = supplier.CheckPayableTo,
                IndividualId = supplier.IndividualId,
                Name = supplier.Name,
                ModificationDate = supplier.ModificationDate,
                Observation = supplier.Observation,

            };

            if (supplier.SupplierDeclinedTypeCode != null)
            {
                ModelSupplier.DeclinedType = new Models.SupplierDeclinedType { Id = (int)supplier.SupplierDeclinedTypeCode };
            }
            return ModelSupplier;

        }

        /// <summary>
        /// Creates the providers.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.Supplier> CreateSuppliers(BusinessCollection businessCollection)
        {
            List<Models.Supplier> provider = new List<Models.Supplier>();

            foreach (Supplier field in businessCollection)
            {
                provider.Add(ModelAssembler.CreateSupplier(field));
            }

            return provider;
        }

        /// <summary>
        /// Creates the type of the provider.
        /// </summary>
        /// <param name="providerType">Type of the Provider.</param>
        private static Models.SupplierType CreateSupplierType(SupplierType supplierType)
        {
            return new Models.SupplierType
            {
                Id = supplierType.SupplierTypeCode,
                Description = supplierType.Description,
                Enable = supplierType.Enabled
            };
        }

        /// <summary>
        /// Creates the provider types.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.SupplierType> CreateSupplierTypes(BusinessCollection businessCollection)
        {
            List<Models.SupplierType> supplierTypes = new List<Models.SupplierType>();

            foreach (SupplierType field in businessCollection)
            {
                supplierTypes.Add(ModelAssembler.CreateSupplierType(field));
            }

            return supplierTypes;
        }

        /// <summary>
        /// Creates the type of the provider declined.
        /// </summary>
        /// <param name="providerDeclinedType">Type of the Provider Declinado.</param>
        private static Models.SupplierDeclinedType CreateSupplierDeclinedType(SupplierDeclinedType supplierDeclinedType)
        {
            return new Models.SupplierDeclinedType
            {
                Id = supplierDeclinedType.SupplierDeclinedTypeCode,
                Description = supplierDeclinedType.Description,
                SmallDescription = supplierDeclinedType.SmallDescription
            };
        }

        /// <summary>
        /// Creates the provider declineds types.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.SupplierDeclinedType> CreateSupplierDeclinedTypes(BusinessCollection businessCollection)
        {
            List<Models.SupplierDeclinedType> supplierDeclinedType = new List<Models.SupplierDeclinedType>();

            foreach (SupplierDeclinedType field in businessCollection)
            {
                supplierDeclinedType.Add(ModelAssembler.CreateSupplierDeclinedType(field));
            }

            return supplierDeclinedType;
        }

        /// <summary>
        /// Creates the GroupSupplier.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.GroupSupplier> CreateGroupsSupplier(BusinessCollection businessCollection)
        {
            List<Models.GroupSupplier> groupSupplier = new List<Models.GroupSupplier>();

            foreach (GroupSupplier field in businessCollection)
            {
                groupSupplier.Add(ModelAssembler.CreateGroupSupplier(field));
            }

            return groupSupplier;
        }

        /// <summary>
        /// Creates the type of the provider declined.
        /// </summary>
        /// <param name="providerDeclinedType">Type of the Provider Declinado.</param>
        private static Models.GroupSupplier CreateGroupSupplier(GroupSupplier groupSupplier)
        {
            return new Models.GroupSupplier
            {
                Id = groupSupplier.GroupSupplierCode,
                Description = groupSupplier.Description,
            };
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
        public static Models.SupplierPaymentConcept CreateProviderPaymentConceptByDescription(ProviderPaymentConcept providerPaymentConcept, string descriptionConceptPayment)
        {
            return new Models.SupplierPaymentConcept
            {
                Id = providerPaymentConcept.ProviderPaymentConceptId,
                ProviderId = providerPaymentConcept.ProviderCode,
                PaymentConcept = new modelsCommon.PaymentConcept { Id = providerPaymentConcept.PaymentConceptCode, Description = descriptionConceptPayment }
            };
        }

        /// <summary>
        /// Creates the type of the provider concepto de pago.
        /// </summary>
        public static Models.SupplierPaymentConcept CreateProviderPaymentConcept(ProviderPaymentConcept providerPaymentConcept)
        {
            return new Models.SupplierPaymentConcept
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
        public static List<Models.SupplierPaymentConcept> CreateProviderPaymentConcepts(BusinessCollection businessCollection)
        {
            List<Models.SupplierPaymentConcept> providerPaymentConcept = new List<Models.SupplierPaymentConcept>();

            foreach (ProviderPaymentConcept field in businessCollection)
            {
                providerPaymentConcept.Add(ModelAssembler.CreateProviderPaymentConcept(field));
            }

            return providerPaymentConcept;
        }

        #endregion Suppliers

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
        public static Models.IndividualTax CreateIndividualTax(TAXEN.IndividualTax individualTax)
        {
            return new Models.IndividualTax
            {
                Id = individualTax.IndividualTaxId,
                TaxRate = new modelsTax.TaxRate
                {
                    Id = individualTax.TaxRateCode,
                },
                IndividualId = individualTax.IndividualId,
                Role = new Models.Role
                {
                    Id = individualTax.RoleCode != null ? (int)individualTax.RoleCode : 0
                }
            };
        }

        //public static Models.IndividualTax CreateTaxRate(List<Models.IndividualTax> individualTaxes, TAXEN.TaxRate taxRate)
        //{
        //
        //}

        public static List<Models.IndividualTaxExeption> CreateIndividualTaxExeptions(BusinessCollection individualTaxExeptions, BusinessCollection state, BusinessCollection category)
        {
            List<Models.IndividualTaxExeption> result = new List<Models.IndividualTaxExeption>();
            foreach (Tax.Entities.IndividualTaxExemption field in individualTaxExeptions)
            {
                var exception = ModelAssembler.CreateIndividualTaxExeption(field);
                foreach (COMMEN.State item in state)
                {
                    if (exception.StateCode?.Id == item.StateCode)
                    {
                        exception.StateCode.Description = item.Description;
                    }
                }
                foreach (TaxCategory item in category)
                {
                    if (exception.TaxCategory?.Id == item.TaxCategoryCode)
                    {
                        exception.TaxCategory.Description = item.Description;
                    }
                }
                result.Add(exception);
            }
            return result;

        }

        public static Models.IndividualTaxExeption CreateIndividualTaxExeption(TAXEN.IndividualTaxExemption individualTaxExemption)
        {
            return individualTaxExemption == null ? new Models.IndividualTaxExeption() : new Models.IndividualTaxExeption
            {
                TaxCode = individualTaxExemption.TaxCode,
                IndividualId = individualTaxExemption.IndividualId,
                IndividualTaxExemptionId = individualTaxExemption.IndTaxExemptionId,
                ExtentPercentage = Convert.ToInt32(individualTaxExemption.ExemptionPercentage),
                Datefrom = individualTaxExemption.CurrentFrom.GetValueOrDefault(),
                DateUntil = individualTaxExemption.CurrentTo,
                StateCode = new modelsCommon.State { Id = individualTaxExemption.StateCode.Value },
                CountryCode = Convert.ToInt32(individualTaxExemption.CountryCode),
                OfficialBulletinDate = individualTaxExemption.BulletinDate,
                ResolutionNumber = individualTaxExemption.ResolutionNumber,
                TotalRetention = individualTaxExemption.HasFullRetention,
                TaxCategory = new Models.TaxCategory { Id = individualTaxExemption.TaxCategoryCode.Value }
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
                //Id = individualTaxExeption.Id,
                //IndividualId = individualTaxExeption.IndividualId,
                //Tax = individualTaxExeption.Tax,
                //TaxCondition = individualTaxExeption.TaxCondition
            };
        }

        public static Models.IndividualTaxExeption MappIndividualTaxExeptionFromIndividualTaxExeption(Models.IndividualTaxExeption individualTaxExemption)
        {
            return new Models.IndividualTaxExeption()
            {
                //IndividualId = individualTaxExemption.IndividualId,
                IndividualTaxExemptionId = individualTaxExemption.IndividualTaxExemptionId,
                //Tax = new Models.Tax { Id = individualTaxExemption.Tax.Id },
                ExtentPercentage = Convert.ToInt32(individualTaxExemption.ExtentPercentage),
                Datefrom = individualTaxExemption.Datefrom,
                DateUntil = individualTaxExemption.DateUntil,
                StateCode = new modelsCommon.State { Id = individualTaxExemption.StateCode.Id },
                CountryCode = Convert.ToInt32(individualTaxExemption.CountryCode),
                OfficialBulletinDate = individualTaxExemption.OfficialBulletinDate,
                ResolutionNumber = individualTaxExemption.ResolutionNumber,
                TotalRetention = individualTaxExemption.TotalRetention,
                TaxCategory = new Models.TaxCategory { Id = individualTaxExemption.TaxCategory.Id }
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

            foreach (TAXEN.IndividualTax field in businessCollection)
            {
                individualTax.Add(ModelAssembler.CreateIndividualTax(field));
            }

            return individualTax;
        }

        public static List<Models.IndividualTax> CreateIndividualTaxTaxRates(List<modelsTax.TaxRate> taxRates, List<Models.IndividualTax> individualTaxes)
        {
            foreach (Models.IndividualTax individualTax in individualTaxes)
            {
                individualTax.TaxRate = taxRates.Where(x => x.Id == individualTax.TaxRate.Id).FirstOrDefault();
                //individualTax.Add(ModelAssembler.CreateTaxRate(individualTaxes, field));
            }

            return individualTaxes;
        }

        public static List<Models.IndividualTax> CreateIndividualTaxRoles(List<Models.Role> roles, List<Models.IndividualTax> individualTaxes)
        {
            foreach (Models.IndividualTax individualTax in individualTaxes)
            {
                individualTax.Role = roles.Where(x => x.Id == individualTax.Role.Id).FirstOrDefault();
            }
            return individualTaxes;
        }

        public static List<modelsTax.TaxRate> createTaxRates(BusinessCollection businessCollection, BusinessCollection condition)
        {
            List<modelsTax.TaxRate> modeltaxRate = new List<modelsTax.TaxRate>();
            foreach (Tax.Entities.TaxRate field in businessCollection)
            {
                modeltaxRate.Add(ModelAssembler.createTaxRate(field));
            }
            if (condition != null && condition.Count > 0 && modeltaxRate.Count > 0)
            {
                foreach (modelsTax.TaxRate taxRate in modeltaxRate)
                {
                    taxRate.TaxCondition.Description = condition.Cast<TAXEN.TaxCondition>()
                        .FirstOrDefault(x => x.TaxConditionCode == taxRate.TaxCondition.Id && x.TaxCode == taxRate.Tax.Id).Description;
                }
            }
            return modeltaxRate;
        }

        public static List<Models.Tax> CreateTaxs(BusinessCollection businessCollection)
        {
            List<Models.Tax> tax = new List<Models.Tax>();

            foreach (Tax.Entities.Tax field in businessCollection)
            {
                tax.Add(ModelAssembler.CreateTax(field));
            }

            return tax;
        }
        public static Models.Tax CreateTax(Tax.Entities.Tax tax)
        {
            return new Models.Tax
            {
                Id = tax.TaxCode,
                Description = tax.Description

            };
        }

        public static modelsTax.TaxRate createTaxRate(Tax.Entities.TaxRate taxRate)
        {
            return new modelsTax.TaxRate
            {
                Id = taxRate.TaxRateId,
                Tax = new modelsTax.Tax
                {
                    Id = taxRate.TaxCode
                },
                TaxCategory = new modelsTax.TaxCategory
                {
                    Id = taxRate.TaxCategoryCode != null ? (int)taxRate.TaxCategoryCode : 0,
                },
                TaxCondition = new modelsTax.TaxCondition
                {
                    Id = taxRate.TaxConditionCode != null ? (int)taxRate.TaxConditionCode : 0,
                }
            };
        }


        public static List<Models.TaxCondition> CreateConditionTaxs(BusinessCollection businessCollection)
        {
            List<Models.TaxCondition> taxCondition = new List<Models.TaxCondition>();

            foreach (Tax.Entities.TaxCondition field in businessCollection)
            {
                taxCondition.Add(ModelAssembler.CreateConditionTax(field));
            }

            return taxCondition;
        }
        public static Models.TaxCondition CreateConditionTax(Tax.Entities.TaxCondition taxCondition)
        {
            return new Models.TaxCondition
            {
                Id = taxCondition.TaxConditionCode,
                Description = taxCondition.Description,



            };
        }


        /// <summary>
        /// Creates the tax de la persona.
        /// </summary>
        /// <param name="providerType">Type of the Provider.</param>
        public static Models.IndividualTaxExeption CreateIndividualTaxExemption(Tax.Entities.IndividualTaxExemption individualTax, TaxIndividualTaxExemptionViewV1 taxIndividualTaxView)
        {
            //Models.IndividualTaxExeption model1 = new PERMODEL.IndividualTaxExeption();
            //if (taxIndividualTaxView.IndividualTaxExemption.Count > 0)
            //{
            //    foreach (Tax.Entities.IndividualTaxExemption item in taxIndividualTaxView.IndividualTaxExemption)
            //    {
            //        model1.IndividualTaxExemptionId = item.IndTaxExemptionId;

            //    }

            //}

            //Models.IndividualTaxExeption model = new PERMODEL.IndividualTaxExeption();
            //foreach (UniquePersonV1.Entities.IndividualTax item in taxIndividualTaxView.IndividualTax)
            //{
            //    model.Id = item.IndividualTaxId;

            //}


            //Models.Tax tax = new Models.Tax();
            //foreach (Tax.Entities.Tax field in taxIndividualTaxView.Tax)
            //{
            //    if (field.TaxCode == individualTax.TaxCode)
            //    {
            //        tax.Description = field.Description;
            //        tax.Id = field.TaxCode;
            //    }
            //}

            //Models.TaxCondition taxCondition = new Models.TaxCondition();
            //foreach (TaxCondition field in taxIndividualTaxView.TaxCondition)
            //{
            //    taxCondition.Description = field.Description;
            //    taxCondition.Id = field.TaxConditionCode;

            //}

            //Models.TaxCategory taxCategory = new Models.TaxCategory();
            //foreach (TaxCategory field in taxIndividualTaxView.TaxCategory)
            //{
            //    taxCategory.Description = field.Description;
            //    taxCategory.Id = field.TaxCategoryCode;

            //}
            //modelsCommon.State stateCode = new modelsCommon.State();
            //foreach (COMMEN.State field in taxIndividualTaxView.State)
            //{
            //    stateCode.Description = field.Description;
            //    stateCode.Id = field.StateCode;

            //}


            //return new Models.IndividualTaxExeption
            //{

            //    Id = model.Id,
            //    IndividualTaxExemptionId = model1.IndividualTaxExemptionId,
            //    Tax = tax,
            //    TaxCondition = taxCondition,
            //    IndividualId = individualTax.IndividualId,
            //    TaxCategory = taxCategory,
            //    StateCode = stateCode,
            //    CountryCode = Convert.ToInt32(individualTax.CountryCode),
            //    Datefrom = individualTax.CurrentFrom,
            //    DateUntil = individualTax.CurrentTo,
            //    ExtentPercentage = Convert.ToInt32(individualTax.ExemptionPercentage),
            //    OfficialBulletinDate = individualTax.BulletinDate,
            //    ResolutionNumber = individualTax.ResolutionNumber,
            //    TotalRetention = individualTax.HasFullRetention
            //};
            return null;
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
        public static Models.IndividualTax CreateIndividualTax(TAXEN.IndividualTax individualTax, TaxIndividualTaxViewV1 taxIndividualTaxView)
        {

            Models.Tax tax = new Models.Tax();
            //foreach (Tax.Entities.Tax field in taxIndividualTaxView.Taxs)
            //{
            //    if (field.TaxCode == individualTax.TaxCode)
            //    {
            //        tax.Description = field.Description;
            //        tax.Id = field.TaxCode;
            //    }
            //}

            Models.TaxCondition taxCondition = new Models.TaxCondition();
            //foreach (TaxCondition field in taxIndividualTaxView.TaxConditions)
            //{
            //    if (field.TaxCode == individualTax.TaxCode && field.TaxConditionCode == individualTax.TaxConditionCode)
            //    {
            //        taxCondition.Description = field.Description;
            //        taxCondition.Id = field.TaxConditionCode;
            //    }
            //}

            return new Models.IndividualTax
            {
                Id = individualTax.IndividualTaxId,
                Tax = tax,
                TaxRate = new modelsTax.TaxRate
                {
                    Id = individualTax.TaxRateCode
                },
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
            var immaper = CreateMapPrefix();
            return immaper.Map<COMMEN.Prefix, modelsCommon.Prefix>(entityPrefix);
        }

        public static List<modelsCommon.Base.BasePrefix> CreatePrefixes(BusinessCollection businessCollection)
        {
            var prefixEntity = businessCollection.Cast<COMMEN.Prefix>().ToList();
            var immaper = CreateMapPrefix();
            return immaper.Map<List<COMMEN.Prefix>, List<modelsCommon.Base.BasePrefix>>(prefixEntity);
        }

        #endregion

        #region ReInsurer V1
        public static modelsPerson.ReInsurer CreateReinsurer(Reinsurer entityReinsurer)
        {
            var codInsured = entityReinsurer
                .PrimaryKey?
                .GetKeys()?
                .Values?
                .Cast<int>()?
                .FirstOrDefault(x => x != entityReinsurer.IndividualId)
                ?? entityReinsurer.ReinsurerCode;

            return new modelsPerson.ReInsurer
            {
                Annotations = entityReinsurer.Annotations,
                DeclinedDate = entityReinsurer.DeclinedDate,
                DeclaredTypeCD = entityReinsurer.DeclinedTypeCode,
                EnteredDate = entityReinsurer.EnteredDate,
                IndividualId = entityReinsurer.IndividualId,
                ModifyDate = entityReinsurer.ModifyDate,
                ReinsuredCD = codInsured
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

        #region IndividualRole

        /// <summary>
        /// Creates the Individual Role.
        /// </summary>
        /// <param name="operatingQuota">The operating quota.</param>
        /// <returns></returns>
        private static Models.IndividualRole CreateIndividualRole(IndividualRole individualRole)
        {
            return new Models.IndividualRole
            {
                IndividualId = individualRole.IndividualId,
                RoleId = individualRole.RoleCode
            };
        }


        /// <summary>
        /// Creates the Individual Role.
        /// </summary>
        /// <param name="businessCollection">The business collection.</param>
        /// <returns></returns>
        public static List<Models.IndividualRole> CreateIndividualRoles(BusinessCollection businessCollection)
        {
            List<Models.IndividualRole> IndividualRole = new List<Models.IndividualRole>();
            foreach (IndividualRole field in businessCollection)
            {
                IndividualRole.Add(ModelAssembler.CreateIndividualRole(field));

            }
            return IndividualRole;
        }

        #endregion IndividualRole

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




        #region ExonerationType

        public static Models.ExonerationType CreateExonerationType(COMMEN.ExonerationType entityExonerationType)
        {
            return new Models.ExonerationType
            {
                Id = entityExonerationType.ExonerationTypeCode,
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

        #region AgentCommission

        public static Models.Commission CreateAgentCommission(ENTV1.AgencyCommissRate agentCommission)
        {
            var immaper = CreateMapAgentCommission();
            return immaper.Map<ENTV1.AgencyCommissRate, Models.Commission>(agentCommission);
        }
        public static List<Models.Commission> CreateAgentCommissions(BusinessCollection businessCollection)
        {
            var immaper = CreateMapAgentCommission();
            var agentCommissions = businessCollection.Cast<ENTV1.AgencyCommissRate>().ToList();
            return immaper.Map<List<ENTV1.AgencyCommissRate>, List<Models.Commission>>(agentCommissions);
        }

        public static List<Models.CommissionAgent> CreateAgentCommissionsAgents(BusinessCollection businessCollection)
        {
            var imaper = CreateMapAgentCommission();
            var agentCommissions = businessCollection.Cast<PRODENT.AgencyCommissRate>().ToList();
            return imaper.Map<List<PRODENT.AgencyCommissRate>, List<Models.CommissionAgent>>(agentCommissions);
        }


        #endregion

        #region LineBusiness
        public static modelsCommon.LineBusiness CreateLineBusiness(Common.Entities.LineBusiness entityLineBusiness)
        {
            var immaper = CreateMapLineBusiness();
            return immaper.Map<Common.Entities.LineBusiness, modelsCommon.LineBusiness>(entityLineBusiness);
        }

        public static List<modelsCommon.LineBusiness> CreateLineBusinesses(BusinessCollection businessCollection)
        {
            var immaper = CreateMapLineBusiness();
            var entityLineBusiness = businessCollection.Cast<Common.Entities.LineBusiness>().ToList();
            return immaper.Map<List<Common.Entities.LineBusiness>, List<modelsCommon.LineBusiness>>(entityLineBusiness);
        }
        #endregion


        #region SubLineBusiness
        public static modelsCommon.SubLineBusiness CreateSubLineBusiness(Common.Entities.SubLineBusiness entitySubLineBusiness)
        {
            var immaper = CreateMapSubLineBusiness();
            return immaper.Map<Common.Entities.SubLineBusiness, modelsCommon.SubLineBusiness>(entitySubLineBusiness);
        }

        public static List<modelsCommon.SubLineBusiness> CreateSubLineBusinesses(BusinessCollection businessCollection)
        {
            var immaper = CreateMapSubLineBusiness();
            var entitySubLineBusiness = businessCollection.Cast<Common.Entities.SubLineBusiness>().ToList();
            return immaper.Map<List<Common.Entities.SubLineBusiness>, List<modelsCommon.SubLineBusiness>>(entitySubLineBusiness);
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
        private static Models.GroupAgent CreateAgentGroup(AgentGroup agentGroup)
        {
            return new Models.GroupAgent
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
        public static List<Models.GroupAgent> CreateAgentGroups(BusinessCollection businessCollection)
        {
            List<Models.GroupAgent> agentGroup = new List<Models.GroupAgent>();

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
        private static Models.SalesChannel CreateAgentoSalesChannel(SalesChannel agentGroup)
        {
            return new Models.SalesChannel
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
        public static List<Models.SalesChannel> CreateAgentoSalesChannels(BusinessCollection businessCollection)
        {
            List<Models.SalesChannel> agentSalesChannel = new List<Models.SalesChannel>();

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
                SecondSurName = entityPerson.MotherLastName,
                IdentificationDocument = new modelsPerson.IdentificationDocument { Number = entityPerson.IdCardNo },
                //todo ricardo
                //PersonCode = Convert.ToInt32(entityPerson.IdCardNo)
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

        public static PERMODEL.EmployeePerson CreateEmployee(Employee entityEmployee)
        {
            return new PERMODEL.EmployeePerson
            {
                Id = entityEmployee.IndividualId,
            };
        }

        public static List<PERMODEL.EmployeePerson> CreateEmployees(BusinessCollection businessCollection)
        {
            List<PERMODEL.EmployeePerson> employees = new List<PERMODEL.EmployeePerson>();

            foreach (Employee entity in businessCollection)
            {
                employees.Add(ModelAssembler.CreateEmployee(entity));
            }
            return employees;
        }
        internal static List<PERMODEL.HouseType> CreateHouseTypes(BusinessCollection businessCollection)
        {
            List<PERMODEL.HouseType> houseTypes = new List<PERMODEL.HouseType>();

            foreach (HouseType entityHouseType in businessCollection)
            {
                houseTypes.Add(CreateHouseType(entityHouseType));
            }

            return houseTypes;
        }

        internal static PERMODEL.HouseType CreateHouseType(HouseType entityHouseType)
        {
            return new PERMODEL.HouseType
            {
                Id = entityHouseType.HouseTypeCode,
                Description = entityHouseType.Description,
                SmallDescription = entityHouseType.SmallDescription
            };
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
            var config = MapperCache.GetMapper<COMMEN.Prefix, modelsCommon.Base.BasePrefix>(cfg =>
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
              .ForMember(dest => dest.AgentDeclinedType, opt => opt.MapFrom(src => src.AgentDeclinedTypeCode == null ? null : new Models.AgentDeclinedType() { Id = (int)src.AgentDeclinedTypeCode }))
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
            .ForMember(dest => dest.AgentDeclinedType, opt => opt.MapFrom(src => src.AgentDeclinedTypeCode == null ? null : new Models.AgentDeclinedType { Id = (int)src.AgentDeclinedTypeCode }));
            });
            return config;
        }
        #region Comision Agente
        public static IMapper CreateMapAgentCommission()
        {
            var config = MapperCache.GetMapper<ENTV1.AgencyCommissRate, Models.Commission>(cfg =>
            {
                cfg.CreateMap<ENTV1.AgencyCommissRate, Models.Commission>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AgencyCommissRateId))
            .ForMember(dest => dest.IndividualId, opt => opt.MapFrom(src => src.IndividualId))
            .ForMember(dest => dest.Prefix, opt => opt.MapFrom(src => new modelsCommon.Prefix { Id = src.PrefixCode }))
            .ForMember(dest => dest.LineBusiness, opt => opt.MapFrom(src => new modelsCommon.LineBusiness { Id = (int)src.LineBusinessCode }))
            .ForMember(dest => dest.SubLineBusiness, opt => opt.MapFrom(src => new modelsCommon.SubLineBusiness { Id = (int)src.SubLineBusinessCode }))
            .ForMember(dest => dest.PercentageCommission, opt => opt.MapFrom(src => src.StCommissPercentage))
            .ForMember(dest => dest.PercentageAdditional, opt => opt.MapFrom(src => (decimal)src.AdditCommissPercentage))
            .ForMember(dest => dest.AgentAgencyId, opt => opt.MapFrom(src => src.AgentAgencyId));
            });
            return config;
        }
        #endregion
        #endregion automapper

        #region Consortium
        public static Models.Consortium CreateConsortiums(CoConsortium consortium)
        {
            return new Models.Consortium()
            {
                InsuredCode = consortium.InsuredCode,
                ConsortiumId = consortium.ConsortiumId,
                Enabled = consortium.Enabled,
                IndividualId = consortium.IndividualId,
                Ismain = consortium.IsMain,
                ParticipationRate = consortium.ParticipationRate,
                StartDate = consortium.StartDate,
            };
        }

        public static List<Models.Consortium> CreateConsortium(List<CoConsortium> consortium)
        {
            var result = new List<Models.Consortium>();
            foreach (var item in consortium)
            {
                result.Add(ModelAssembler.CreateConsortiums(item));
            }
            return result;
        }

        /// <summary>
        /// Creates the consortiums.
        /// </summary>
        /// <param name="coConsortium">The co consortium.</param>
        /// <param name="coConsortiumView">The co consortium view.</param>
        /// <returns></returns>
        public static List<Models.Consortium> CreateCoConsortiums(Entities.views.ConsorcioViewCoV1 businessCollection)
        {
            var persons = businessCollection.PersonConsortium.Select(x => (Person)x).ToList();
            var companies = businessCollection.CompanyConsortium.Select(x => (Company)x).ToList();
            List<CoConsortium> entitiesConsortiums = businessCollection.CoConsortiumList.Cast<CoConsortium>().ToList();
            var immaper = AutoMapperAssembler.CreateMapConsortium();
            List<Models.Consortium> consortiums = immaper.Map<List<CoConsortium>, List<Models.Consortium>>(entitiesConsortiums);
            consortiums.AsParallel().ForAll(z =>
            {
                Person person = persons?.FirstOrDefault(p => p.IndividualId == z.IndividualId);
                Company company = companies?.FirstOrDefault(c => c.IndividualId == z.IndividualId);



                z.FullName = person != null ? $"{person.Name ?? ""} {person.Surname ?? ""} {person.MotherLastName ?? ""}" : (company.TradeName ?? "");
                z.IdentificationDocument = new modelsPerson.IdentificationDocument
                {
                    Number = person?.IdCardNo ?? company?.TributaryIdNo ?? ""
                };
                z.IdentificationDocument.DocumentType = new modelsPerson.DocumentType
                {
                    Id = person?.IdCardTypeCode ?? company?.TributaryIdTypeCode ?? 0
                };
            });
            return consortiums;
        }


        #endregion

        #region CompanyCoInsured
        /// <summary>
        /// Mapea La Informacion guardad en la base de Datos
        /// </summary>
        /// <param name="entidad">Información desde la base de Datos.</param>
        /// <returns></returns>
        public static Models.CompanyCoInsured CreateCoInsured(COMMEN.CoInsuranceCompany entityCoInsured)
        {
            return new Models.CompanyCoInsured
            {
                AddressTypeCode = entityCoInsured.AddressTypeCode,
                Annotations = entityCoInsured.Annotations,
                CityCode = entityCoInsured.CityCode,
                CountryCode = entityCoInsured.CountryCode,
                Description = entityCoInsured.Description,
                EnsureInd = entityCoInsured.EnsureInd,
                EnteredDate = entityCoInsured.EnteredDate,
                ModifyDate = entityCoInsured.ModifyDate,
                PhoneNumber = entityCoInsured.PhoneNumber,
                PhoneTypeCode = entityCoInsured.PhoneTypeCode,
                StateCode = entityCoInsured.StateCode,
                Street = entityCoInsured.Street,
                TributaryIdNo = entityCoInsured.TributaryIdNo,
                ComDeclinedTypeCode = entityCoInsured.ComDeclinedTypeCode,
                DeclinedDate = entityCoInsured.DeclinedDate,
                InsuraceCompanyId = entityCoInsured.InsuranceCompanyId,
                IndividualId = Convert.ToInt32(entityCoInsured.IndividualId)
            };
        }
        #endregion

        #region OthersDeclinedType
        public static Models.AllOthersDeclinedType CreateAllOthersDeclinedType(OthersDeclinedType othersDeclinedType)
        {
            return new Models.AllOthersDeclinedType
            {
                Id = othersDeclinedType.OtherDeclinedTypeCode,
                Description = othersDeclinedType.Description,
                SmallDescription = othersDeclinedType.SmallDescription,
                RoleCd = othersDeclinedType.RoleCode
            };

        }
        public static List<Models.AllOthersDeclinedType> CreateAllOthersDeclinedTypes(BusinessCollection businessCollection)
        {
            List<Models.AllOthersDeclinedType> othersDeclinedType = new List<Models.AllOthersDeclinedType>();

            foreach (OthersDeclinedType field in businessCollection)
            {
                othersDeclinedType.Add(ModelAssembler.CreateAllOthersDeclinedType(field));
            }

            return othersDeclinedType;

        }

        internal static PERMODEL.InsuredGuaranteeLog CreateInsuredGuaranteeLog(InsuredGuaranteeLog entityInsuredGuaranteeLog)
        {
            return new modelsPerson.InsuredGuaranteeLog()
            {
                Description = entityInsuredGuaranteeLog.Description,
                GuaranteeId = entityInsuredGuaranteeLog.GuaranteeId,
                GuaranteeStatusCode = entityInsuredGuaranteeLog.GuaranteeStatusCode,
                IndividualId = entityInsuredGuaranteeLog.IndividualId,
                LogDate = entityInsuredGuaranteeLog.LogDate,
                UserId = entityInsuredGuaranteeLog.UserId
            };
        }


        internal static List<PERMODEL.InsuredGuaranteeLog> CreateInsuredGuaranteeLogs(BusinessCollection businessCollectio)
        {
            List<PERMODEL.InsuredGuaranteeLog> insuredGuaranteeLogs = new List<modelsPerson.InsuredGuaranteeLog>();
            foreach (InsuredGuaranteeLog entityinsuredGuaranteeLog in businessCollectio)
            {
                insuredGuaranteeLogs.Add(CreateInsuredGuaranteeLog(entityinsuredGuaranteeLog));
            }
            return insuredGuaranteeLogs;
        }
        #endregion

        #region CompanyInsurance

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

        #region InsuredSegment
        public static Models.InsuredSegmentV1 CreateInsuredSegment(InsuredSegment insuredSegment)
        {
            return new Models.InsuredSegmentV1()
            {
                ShortDescription = insuredSegment.SmallDescription,
                LongDescription = insuredSegment.Description,
                Id = insuredSegment.InsSegmentCode
            };
        }

        public static List<Models.InsuredSegmentV1> CreateInsuredSegments(BusinessCollection businessCollection)
        {
            List<Models.InsuredSegmentV1> insuredSegments = new List<Models.InsuredSegmentV1>();

            foreach (InsuredSegment entity in businessCollection)
            {
                insuredSegments.Add(ModelAssembler.CreateInsuredSegment(entity));
            }

            return insuredSegments;
        }
        #endregion

        #region InsuredProfiles

        /// <summary>
        /// Mapeo de la entidad InsuredProfile al modelo InsuredProfile
        /// </summary>
        /// <param name="insuredProfile"> Entidad InsuredProfile</param>
        /// <returns> Modelo InsuredProfile</returns>
        public static Models.InsuredProfile CreateInsuredProfile(InsuredProfile insuredProfile)
        {
            return new Models.InsuredProfile()
            {
                SmallDescription = insuredProfile.SmallDescription,
                Description = insuredProfile.Description,
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

        #endregion InsuredProfiles

        #region CompanyName
        public static Models.BankTransfers CreateBankTransfers(PersonAccountBank personAccountBank)
        {
            return new Models.BankTransfers
            {
                Id = personAccountBank.AccountBankCode,
                AccountNumber = personAccountBank.Number,
                AccountType = new PERMODEL.AccountType { Code = personAccountBank.AccountTypeCode == 0 ? 0 : personAccountBank.AccountTypeCode },
                ActiveAccount = personAccountBank.Active,
                Bank = new modelsCommon.Bank { Id = personAccountBank.BankCode == 0 ? 0 : personAccountBank.BankCode },
                BankBranch = personAccountBank.BankBranch,
                BankSquare = personAccountBank.BankSquare,
                Currency = new modelsCommon.Currency { Id = personAccountBank.CurrencyCode == 0 ? 0 : personAccountBank.CurrencyCode },
                DefaultAccount = personAccountBank.DefaultAccount,
                Individual = personAccountBank.IndividualId,
                IntermediaryBank = personAccountBank.BankIntermediary,
                PaymentBeneficiary = personAccountBank.Beneficiary,
                InscriptionDate = personAccountBank.InscriptionDate
            };
        }
        public static List<Models.BankTransfers> CreateBanksTransfers(BusinessCollection businessCollection)
        {
            List<Models.BankTransfers> bankTransfers = new List<Models.BankTransfers>();
            foreach (PersonAccountBank field in businessCollection)
            {
                bankTransfers.Add(ModelAssembler.CreateBankTransfers(field));

            }
            return bankTransfers;
        }
        #endregion

        public static List<modelsPerson.AccountType> CreateAccountTypesV1(BusinessCollection businessCollection)
        {
            List<modelsPerson.AccountType> accounTypes = new List<modelsPerson.AccountType>();
            foreach (ENTV1.AccountType field in businessCollection)
            {
                accounTypes.Add(ModelAssembler.CreateAccountTypeV1(field));

            }
            return accounTypes;
        }
        public static Models.AccountType CreateAccountTypeV1(ENTV1.AccountType accountType)
        {
            return new Models.AccountType
            {
                Code = accountType.AccountTypeCode,
                Description = accountType.Description,
                SmallDescription = accountType.SmallDescription
            };

        }


        #region EconomiGroup
        /// <summary>
        /// Mapeo de la entidad InsuredProfile al modelo InsuredProfile
        /// </summary>
        /// <param name="insuredProfile"> Entidad InsuredProfile</param>
        /// <returns> Modelo InsuredProfile</returns>
        public static Models.EconomicGroup CreateEconomicGroup(ENTV1.EconomicGroup economicGroup)
        {
            return new Models.EconomicGroup()
            {
                EconomicGroupId = economicGroup.EconomicGroupId,
                EconomicGroupName = economicGroup.EconomicGroupName,
                DeclinedDate = economicGroup.DeclinedDate,
                Enabled = economicGroup.EnabledInd,
                EnteredDate = economicGroup.EnteredDate,
                TributaryIdNo = economicGroup.TrubutaryIdNo,
                TributaryIdType = economicGroup.TributaryIdTypeCode,
                OperationQuoteAmount = economicGroup.OperatingQuotaAmount,
                UserId = economicGroup.UserId,
                VerifyDigit = economicGroup.VerifyDigit
            };
        }

        /// <summary>
        /// Mapeo lista de objeto businessCollection a lista modelo InsuredProfile
        /// </summary>
        /// <param name="businessCollection">Objeto businessCollection</param>
        /// <returns>Lista de Modelos InsuredProfile</returns>
        public static List<Models.EconomicGroup> CreateEconomicGroups(BusinessCollection businessCollection)
        {
            List<Models.EconomicGroup> insuredProfiles = new List<Models.EconomicGroup>();

            foreach (EconomicGroup entity in businessCollection)
            {
                insuredProfiles.Add(ModelAssembler.CreateEconomicGroup(entity));
            }

            return insuredProfiles;
        }

        /// <summary>
        /// Mapeo de la entidad InsuredProfile al modelo InsuredProfile
        /// </summary>
        /// <param name="insuredProfile"> Entidad InsuredProfile</param>
        /// <returns> Modelo InsuredProfile</returns>
        public static Models.EconomicGroupDetail CreateEconomicGroupDetail(ENTV1.EconomicGroupDetail economicGroupDetail)
        {
            return new Models.EconomicGroupDetail()
            {
                EconomicGroupId = economicGroupDetail.EconomicGroupId,
                Enabled = economicGroupDetail.EnabledInd,
                DeclinedDate = economicGroupDetail.DeclinedDate,
                IndividualId = economicGroupDetail.IndividualId
            };
        }

        /// <summary>
        /// Mapeo lista de objeto businessCollection a lista modelo InsuredProfile
        /// </summary>
        /// <param name="businessCollection">Objeto businessCollection</param>
        /// <returns>Lista de Modelos InsuredProfile</returns>
        public static List<Models.EconomicGroupDetail> CreateEconomicGroupDetails(BusinessCollection businessCollection)
        {
            List<Models.EconomicGroupDetail> economicGroupDetail = new List<Models.EconomicGroupDetail>();

            foreach (EconomicGroupDetail entity in businessCollection)
            {
                economicGroupDetail.Add(ModelAssembler.CreateEconomicGroupDetail(entity));
            }

            return economicGroupDetail;
        }

        /// <summary>
        /// Mapeo de la entidad InsuredProfile al modelo InsuredProfile
        /// </summary>
        /// <param name="insuredProfile"> Entidad InsuredProfile</param>
        /// <returns> Modelo InsuredProfile</returns>
        public static Models.TributaryIdentityType CreateTributaryType(ENTV1.TributaryIdentityType tributaryType)
        {
            return new Models.TributaryIdentityType()
            {
                Id = tributaryType.TributaryIdTypeCode,
                Description = tributaryType.Description,
                SmallDescription = tributaryType.SmallDescription,
            };
        }

        /// <summary>
        /// Mapeo lista de objeto businessCollection a lista modelo InsuredProfile
        /// </summary>
        /// <param name="businessCollection">Objeto businessCollection</param>
        /// <returns>Lista de Modelos InsuredProfile</returns>
        public static List<Models.TributaryIdentityType> CreateTributaryTypes(BusinessCollection businessCollection)
        {
            List<Models.TributaryIdentityType> listTributaryType = new List<Models.TributaryIdentityType>();

            foreach (TributaryIdentityType entity in businessCollection)
            {
                listTributaryType.Add(ModelAssembler.CreateTributaryType(entity));
            }

            return listTributaryType;
        }
        #endregion

        #region FiscalResponsibility
        public static Models.FiscalResponsibility CreateFiscalResponsibility(FiscalResponsibility entityFiscalResponsibility)
        {
            return new Models.FiscalResponsibility
            {
                Id = entityFiscalResponsibility.Id,
                Code = entityFiscalResponsibility.Code,
                Description = entityFiscalResponsibility.Description
            };
        }
        public static List<Models.FiscalResponsibility> CreateListFiscalResponsibility(BusinessCollection entityBusinessCollection)
        {
            List<Models.FiscalResponsibility> fiscalResponsibilities = new List<Models.FiscalResponsibility>();
            foreach (FiscalResponsibility field in entityBusinessCollection)
            {
                fiscalResponsibilities.Add(ModelAssembler.CreateFiscalResponsibility(field));

            }
            return fiscalResponsibilities;
        }
        #endregion

        #region Politicas

        /// <summary>
        /// Crear politicas ejecutadas para personas.
        /// </summary>
        /// <returns></returns>
        public static Models.PersonOperation CreatePersonOperation(TmpPersonOperation personOperation)
        {
            return new Models.PersonOperation
            {
                OperationId = personOperation.OperationId,
                IndividualId = personOperation.IndividualId,
                Operation = personOperation.Operation,
                Process = personOperation.Proccess,
                ProcessType = personOperation.TypeProccess,
                FunctionId = personOperation.FunctionId

            };


        }
        /// <summary>
        /// Crear politicas ejecutadas para personas.
        /// </summary>
        /// <returns></returns>
        public static List<Models.PersonOperation> CreatePersonOperations(BusinessCollection businessCollection)
        {
            List<Models.PersonOperation> personOperation = new List<Models.PersonOperation>();
            foreach (TmpPersonOperation field in businessCollection)
            {
                personOperation.Add(ModelAssembler.CreatePersonOperation(field));

            }
            return personOperation;

        }

        #endregion Politicas

        #region Punto de Control (Integracion)
        public static Models.PersonAccountBankControl CreatePersonAccountBankControl(INTEN.UpPersonAccountBankControl entityUpPersonAccountBankControl)
        {
            return new Models.PersonAccountBankControl()
            {
                IndividualId = entityUpPersonAccountBankControl.IndividualId,
                Action = entityUpPersonAccountBankControl.Action
            };
        }

        public static Models.IndividualControl CreateIndividualControl(INTEN.UpIndividualControl entityIndividualControl)
        {
            return new Models.IndividualControl()
            {
                IndividualId = entityIndividualControl.IndividualId,
                Action = entityIndividualControl.Action
            };
        }
        public static Models.InsuredControl CreateInsuredControl(INTEN.UpInsuredControl entityInsuredControl)
        {
            return new Models.InsuredControl()
            {
                IndividualId = entityInsuredControl.IndividualId,
                InsuredCode = entityInsuredControl.InsuredCode,
                Action = entityInsuredControl.Action
            };
        }
        #endregion
    }
}