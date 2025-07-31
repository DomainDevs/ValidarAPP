using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.UniquePersonService.Enums;
using Sistran.Core.Application.UniquePersonService.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{
    public class ProspectDAO
    {
        /// <summary>
        /// Obtener Prospecto
        /// </summary>
        /// <param name="individualType">Tipo de individuo</param>
        /// <param name="documentTypeId">Id tipo de documento</param>
        /// <param name="document">Documento</param>
        /// <returns>Prospecto</returns>
        public Prospect GetProspectByIndividualTypeDocumentTypeIdDocument(Enums.IndividualType individualType, int documentTypeId, string document)
        {
            Prospect prospect = null;

            if (individualType == Enums.IndividualType.Person)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(UniquePerson.Entities.Person.Properties.IdCardTypeCode, "p", documentTypeId);
                filter.And();
                filter.PropertyEquals(UniquePerson.Entities.Person.Properties.IdCardNo, "p", document);

                BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List<UniquePerson.Entities.Person>(filter.GetPredicate());

                if (businessCollection.Count > 0)
                {
                    prospect = ModelAssembler.CreateProspectByPerson((UniquePerson.Entities.Person)businessCollection[0]);

                    AddressDAO AddressDAO = new AddressDAO();
                    List<Address> addresses = AddressDAO.GetAddresses(prospect.Id);

                    if (addresses.Count > 0)
                    {
                        prospect.CompanyName = new CompanyName
                        {
                            Address = addresses.FirstOrDefault()
                        };

                        PhoneDAO phoneDAO = new PhoneDAO();
                        List<Phone> phones = phoneDAO.GetPhonesByIndividualId(prospect.Id);

                        if (phones.Count > 0)
                        {
                            prospect.CompanyName.Phone = phones.FirstOrDefault();
                        }

                        EmailDAO emailDAO = new EmailDAO();
                        List<Email> emails = emailDAO.GetEmailsByIndividualId(prospect.Id);

                        if (emails.Count > 0)
                        {
                            prospect.CompanyName.Email = emails.FirstOrDefault();
                        }
                    }
                }
                else
                {
                    filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(UniquePerson.Entities.Prospect.Properties.IdCardTypeCode, "p", documentTypeId);
                    filter.And();
                    filter.PropertyEquals(UniquePerson.Entities.Prospect.Properties.IdCardNo, "p", document);

                    businessCollection = DataFacadeManager.Instance.GetDataFacade().List<UniquePerson.Entities.Prospect>(filter.GetPredicate());

                    if (businessCollection.Count > 0)
                    {
                        prospect = ModelAssembler.CreateProspect((UniquePerson.Entities.Prospect)businessCollection[0]);
                    }
                }
            }
            else if (individualType == Enums.IndividualType.LegalPerson)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.PropertyEquals(UniquePerson.Entities.Company.Properties.TributaryIdTypeCode, "p", documentTypeId);
                filter.And();
                filter.PropertyEquals(UniquePerson.Entities.Company.Properties.TributaryIdNo, "p", document);

                BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List<UniquePerson.Entities.Company>(filter.GetPredicate());

                if (businessCollection.Count > 0)
                {
                    prospect = ModelAssembler.CreateProspectByCompany((UniquePerson.Entities.Company)businessCollection[0]);

                    AddressDAO AddressDAO = new AddressDAO();
                    List<Address> addresses = AddressDAO.GetAddresses(prospect.Id);

                    if (addresses.Count > 0)
                    {
                        prospect.CompanyName = new CompanyName
                        {
                            Address = addresses.FirstOrDefault()
                        };

                        PhoneDAO phoneDAO = new PhoneDAO();
                        List<Phone> phones = phoneDAO.GetPhonesByIndividualId(prospect.Id);

                        if (phones.Count > 0)
                        {
                            prospect.CompanyName.Phone = phones.FirstOrDefault();
                        }

                        EmailDAO emailDAO = new EmailDAO();
                        List<Email> emails = emailDAO.GetEmailsByIndividualId(prospect.Id);

                        if (emails.Count > 0)
                        {
                            prospect.CompanyName.Email = emails.FirstOrDefault();
                        }
                    }
                }
                else
                {
                    filter = new ObjectCriteriaBuilder();
                    filter.PropertyEquals(UniquePerson.Entities.Prospect.Properties.TributaryIdTypeCode, "p", documentTypeId);
                    filter.And();
                    filter.PropertyEquals(UniquePerson.Entities.Prospect.Properties.TributaryIdNo, "p", document);

                    businessCollection = DataFacadeManager.Instance.GetDataFacade().List<UniquePerson.Entities.Prospect>(filter.GetPredicate());

                    if (businessCollection.Count > 0)
                    {
                        prospect = ModelAssembler.CreateProspect((UniquePerson.Entities.Prospect)businessCollection[0]);
                    }
                }
            }

            return prospect;
        }

        /// <summary>
        /// Obtener Prospectos Por Id, Número De Documento O Nombre
        /// </summary>
        /// <param name="description">Id, Número De Documento O Nombre</param>
        /// <param name="insuredSearchType">Tipo De Busqueda</param>
        /// <returns>Prospectos</returns>
        public List<Prospect> GetProspectByDescription(string description, InsuredSearchType insuredSearchType)
        {
            List<Prospect> prospects = new List<Prospect>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            Int64 documentNumber = 0;
            Int64.TryParse(description, out documentNumber);

            if (insuredSearchType == InsuredSearchType.DocumentNumber)
            {
                if (documentNumber > 0)
                {
                    filter.PropertyEquals(UniquePerson.Entities.Prospect.Properties.IdCardNo, "p", documentNumber.ToString());
                    filter.Or();
                    filter.PropertyEquals(UniquePerson.Entities.Prospect.Properties.TributaryIdNo, "p", documentNumber.ToString());
                }
                else
                {
                    filter.PropertyEquals(UniquePerson.Entities.Prospect.Properties.TradeName, "p", description);
                    filter.Or();
                    filter.PropertyEquals(UniquePerson.Entities.Prospect.Properties.Name, "p", description);
                }
            }
            else
            {
                filter.PropertyEquals(UniquePerson.Entities.Prospect.Properties.ProspectId, "p", documentNumber);
            }

            BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List<UniquePerson.Entities.Prospect>(filter.GetPredicate());

            if (businessCollection.Count > 0)
            {
                prospects = ModelAssembler.CreateProspects(businessCollection);
            }

            return prospects;
        }

        /// <summary>
        /// Obtener Prospecto Por Identificador
        /// </summary>
        /// <param name="prospectId"></param>
        /// <returns></returns>
        public Prospect GetProspectByProspectId(int prospectId)
        {
            PrimaryKey key = UniquePerson.Entities.Prospect.CreatePrimaryKey(prospectId);
            UniquePerson.Entities.Prospect entityProspect = (UniquePerson.Entities.Prospect)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            if (entityProspect != null)
            {
                return ModelAssembler.CreateProspect(entityProspect);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Crear prospecto
        /// </summary>
        /// <param name="prospect">Datos prospecto</param>
        /// <returns>Prospecto</returns>
        public Prospect CreateProspect(Prospect prospect)
        {
            UniquePerson.Entities.Prospect entityProspect = EntityAssembler.CreateProspect(prospect);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(entityProspect);
            prospect.Id = entityProspect.ProspectId;

            return prospect;
        }

        /// <summary>
        /// Actualizar prospecto
        /// </summary>
        /// <param name="prospect">Datos prospecto</param>
        /// <returns>Prospecto</returns>
        public Prospect UpdateProspect(Prospect prospect)
        {
            PrimaryKey key = UniquePerson.Entities.Prospect.CreatePrimaryKey(prospect.Id);
            UniquePerson.Entities.Prospect entityProspect = (UniquePerson.Entities.Prospect)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

            if (entityProspect != null)
            {
                entityProspect.IndividualTypeCode = (int)prospect.IndividualType;
                entityProspect.Street = prospect.CompanyName.Address.Description;
                if (prospect.CompanyName.Phone.Description != null)
                {
                    entityProspect.PhoneNumber = prospect.CompanyName.Phone.Description.Length > 0 ? Convert.ToInt64(prospect.CompanyName.Phone.Description) : (long?)null;
                }
                else
                {
                    entityProspect.PhoneNumber = (long?)null;
                }
                entityProspect.EmailAddress = prospect.CompanyName.Email.Description;

                if (prospect.IndividualType == Enums.IndividualType.Person)
                {
                    entityProspect.Surname = prospect.Surname;
                    entityProspect.MotherLastName = prospect.SecondSurname;
                    entityProspect.Name = prospect.Name;
                    entityProspect.Gender = prospect.Gender;
                    entityProspect.BirthDate = prospect.BirthDate;
                    entityProspect.IdCardTypeCode = prospect.IdentificationDocument.DocumentType.Id;
                    entityProspect.IdCardNo = prospect.IdentificationDocument.Number;
                    entityProspect.MaritalStatusCode = prospect.MaritalStatus;
                }
                else if (prospect.IndividualType == Enums.IndividualType.LegalPerson)
                {
                    entityProspect.TradeName = prospect.TradeName;
                    entityProspect.TributaryIdTypeCode = prospect.IdentificationDocument.DocumentType.Id;
                    entityProspect.TributaryIdNo = prospect.IdentificationDocument.Number;
                }

                if (prospect.CompanyName.Address.City != null && prospect.CompanyName.Address.City.Id > 0)
                {
                    entityProspect.CityCode = prospect.CompanyName.Address.City.Id;

                    if (prospect.CompanyName.Address.City.State != null && prospect.CompanyName.Address.City.State.Id > 0)
                    {
                        entityProspect.StateCode = prospect.CompanyName.Address.City.State.Id;

                        if (prospect.CompanyName.Address.City.State.Country != null && prospect.CompanyName.Address.City.State.Country.Id > 0)
                        {
                            entityProspect.CountryCode = prospect.CompanyName.Address.City.State.Country.Id;
                        }
                    }
                }

                if (prospect.CompanyName.Address.AddressType != null && prospect.CompanyName.Address.AddressType.Id > 0)
                {
                    entityProspect.AddressTypeCode = prospect.CompanyName.Address.AddressType.Id;
                }

                DataFacadeManager.Instance.GetDataFacade().UpdateObject(entityProspect);

                return prospect;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener Prospectos Por Id, Número De Documento O Nombre
        /// </summary>
        /// <param name="description">Id, Número De Documento O Nombre</param>
        /// <returns>Prospectos</returns>
        public List<Prospect> GetAdvancedProspectByDescription(string description)
        {
            List<Prospect> prospects = new List<Prospect>();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            Int64 documentNumber = 0;
            Int64.TryParse(description, out documentNumber);

            if (documentNumber > 0)
            {
                filter.PropertyEquals(UniquePerson.Entities.Person.Properties.IdCardNo, "p", documentNumber.ToString());
                filter.Or();
                filter.PropertyEquals(UniquePerson.Entities.Person.Properties.TributaryIdNo, "p", documentNumber.ToString());
            }
            else
            {
                filter.Property(UniquePerson.Entities.Person.Properties.Name, "p");
                filter.Like();
                filter.Constant("%" + description + "%");
                filter.Or();
                filter.Property(UniquePerson.Entities.Person.Properties.Surname, "p");
                filter.Like();
                filter.Constant("%" + description + "%");
            }
            BusinessCollection businessCollection = DataFacadeManager.Instance.GetDataFacade().List<UniquePerson.Entities.Person>(filter.GetPredicate());

            if (businessCollection.Count > 0)
            {
                prospects = ModelAssembler.CreateDistinctProspectsByPerson(businessCollection);
            }
            else
            {
                filter = new ObjectCriteriaBuilder();
                if (documentNumber > 0)
                {
                    filter.PropertyEquals(UniquePerson.Entities.Prospect.Properties.IdCardNo, "p", documentNumber.ToString());
                    filter.Or();
                    filter.PropertyEquals(UniquePerson.Entities.Prospect.Properties.TributaryIdNo, "p", documentNumber.ToString());
                }
                else
                {
                    filter.Property(UniquePerson.Entities.Prospect.Properties.TradeName, "p");
                    filter.Like();
                    filter.Constant("%" + description + "%");
                    filter.Or();
                    filter.Property(UniquePerson.Entities.Prospect.Properties.Name, "p");
                    filter.Like();
                    filter.Constant("%" + description + "%");
                    filter.Or();
                    filter.Property(UniquePerson.Entities.Prospect.Properties.Surname, "p");
                    filter.Like();
                    filter.Constant("%" + description + "%");
                }
                businessCollection = DataFacadeManager.Instance.GetDataFacade().List<UniquePerson.Entities.Prospect>(filter.GetPredicate());

                if (businessCollection.Count > 0)
                {
                    prospects = ModelAssembler.CreateDistinctProspects(businessCollection);
                }
                else
                {
                    filter = new ObjectCriteriaBuilder();
                    if (documentNumber > 0)
                    {
                        filter.PropertyEquals(UniquePerson.Entities.Company.Properties.TributaryIdNo, "p", documentNumber.ToString());
                    }
                    else
                    {
                        filter.Property(UniquePerson.Entities.Company.Properties.TradeName, "p");
                        filter.Like();
                        filter.Constant("%" + description + "%");
                    }
                    businessCollection = DataFacadeManager.Instance.GetDataFacade().List<UniquePerson.Entities.Company>(filter.GetPredicate());
                    if (businessCollection.Count > 0)
                    {
                        prospects = ModelAssembler.CreateDistinctProspectsByCompany(businessCollection);
                    }
                }
            }
            return prospects;
        }
    }
}