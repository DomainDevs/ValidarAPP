using Sistran.Core.Application.UnderwritingServices.EEProvider.Resources;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.UniquePersonService.V1.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model = Sistran.Core.Application.UnderwritingServices.Models;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class HolderDAO
    {
        /// <summary>
        /// Obtener tomadores por Id, Documento o Descripción
        /// </summary>
        /// <param name="description">Parametro de Busqueda</param>
        /// <param name="insuredSearchType">Tipo de Busqueda</param>
        /// <param name="customerType">Tipo de Cliente</param>
        /// <returns>Tomadores</returns>
        public List<Model.Holder> GetHoldersByDescriptionInsuredSearchTypeCustomerType(string description, InsuredSearchType insuredSearchType, CustomerType? customerType)
        {
            ConcurrentBag<Holder> holdersAll = new ConcurrentBag<Holder>();
            List<Holder> holders = new List<Holder>();

            InsuredDAO insuredDAO = new InsuredDAO();
            List<IssuanceInsured> insureds = insuredDAO.GetInsuredsByDescriptionInsuredSearchTypeCustomerType(description, insuredSearchType, customerType);
            if (insureds != null && insureds.Count > 0)
            {
                insureds.AsParallel().ForAll(insured =>
                {
                    Holder newHolder = new Holder
                    {
                        CustomerType = insured.CustomerType,
                        IndividualId = insured.IndividualId,
                        IndividualType = insured.IndividualType,
                        InsuredId = insured.InsuredId,
                        Surname = insured.Surname,
                        SecondSurname = insured.SecondSurname,
                        Name = insured.IndividualType == IndividualType.Person ? (
                            insured.Surname + " " + (string.IsNullOrEmpty(insured.SecondSurname) ? "" : insured.SecondSurname + " ") + insured.Name
                            ) : insured.Name,
                        IdentificationDocument = new IssuanceIdentificationDocument
                        {
                            Number = insured.IdentificationDocument.Number,
                            DocumentType = new IssuanceDocumentType
                            {
                                Id = insured.IdentificationDocument.DocumentType.Id,
                                Description = insured.IdentificationDocument.DocumentType.Description
                            }
                        }
                    };
                    holdersAll.Add(newHolder);
                });
                holders = holdersAll.ToList();
                if (insureds.Count == 1 && insureds[0].CustomerType != CustomerType.Prospect)
                {
                    holders[0].EconomicActivity = new IssuanceEconomicActivity
                    {
                        Id = insureds[0].EconomicActivity.Id
                    };
                    holders[0].PaymentMethod = new IssuancePaymentMethod
                    {
                        Id = insureds[0].PaymentMethod.Id,
                        PaymentId = insureds[0].PaymentMethod.PaymentId,
                        Description = insureds[0].PaymentMethod.Description

                    };
                    holders[0].BirthDate = insureds[0].BirthDate;
                    holders[0].Gender = insureds[0].Gender;
                    holders[0].IdentificationDocument = new IssuanceIdentificationDocument
                    {
                        Number = insureds[0].IdentificationDocument.Number,
                        DocumentType = new IssuanceDocumentType
                        {
                            Id = insureds[0].IdentificationDocument.DocumentType.Id,
                            Description = insureds[0].IdentificationDocument.DocumentType.Description
                        }
                    };

                    if (insureds[0].DeclinedDate.HasValue)
                    {
                        holders[0].DeclinedDate = insureds[0].DeclinedDate.Value;
                    }
                    if (insureds[0].CompanyName != null)
                    {
                        holders[0].CompanyName = new IssuanceCompanyName
                        {
                            NameNum = insureds[0].CompanyName.NameNum,
                            TradeName = insureds[0].CompanyName.TradeName,
                            Address = insureds[0].CompanyName?.Address == null ? null : new IssuanceAddress
                            {
                                Id = insureds[0].CompanyName.Address.Id,
                                Description = insureds[0].CompanyName.Address.Description
                            },
                            Phone = insureds[0].CompanyName?.Phone == null ? null : new IssuancePhone
                            {
                                Id = insureds[0].CompanyName.Phone.Id,
                                Description = insureds[0].CompanyName.Phone.Description
                            },
                            Email = insureds[0].CompanyName?.Email == null ? null : new IssuanceEmail
                            {
                                Id = insureds[0].CompanyName.Email.Id,
                                Description = insureds[0].CompanyName.Email.Description
                            }
                        };
                    }
                    else
                    {
                        throw new System.Exception(Errors.ErrorHolderWithoutAddress);
                    }

                    List<Email> emails = DelegateService.uniquePersonServiceCoreV1.GetEmailsByIndividualId(holders[0].IndividualId);
                    if (emails.Count > 0)
                    {
                        foreach (Email email in emails)
                        {
                            if (email.EmailType.Id == 23)
                            {
                                holders[0].Email = email.Description;
                            }
                        }
                    }

                    List<InsuredFiscalResponsibility> fiscal = DelegateService.uniquePersonServiceCoreV1.GetFiscalResponsibilityByIndividualId(holders[0].IndividualId);
                    if (fiscal.Count > 0)
                    {
                        holders[0].FiscalResponsibility = fiscal;
                    }
                    
                }

                return holders;
            }
            else
            {
                return null;
            }

        }
        public Holder GetHolderByInsuredCode(int insuredCode)
        {
            InsuredDAO insuredDAO = new InsuredDAO();
            IssuanceInsured insured = insuredDAO.GetInsuredByInsuredCode(insuredCode);

            if (insured != null)
            {
                return new Holder
                {
                    CustomerType = insured.CustomerType,
                    IndividualType = insured.IndividualType,
                    IndividualId = insured.IndividualId,
                    Name = insured.Name + " " + insured.Surname + " " + insured.SecondSurname,
                    IdentificationDocument = new IssuanceIdentificationDocument
                    {
                        Number = insured.IdentificationDocument.Number,
                        DocumentType = new IssuanceDocumentType
                        {
                            Id = insured.IdentificationDocument.DocumentType.Id
                        }
                    },
                    EconomicActivity = new IssuanceEconomicActivity
                    {
                        Id = insured.EconomicActivity.Id
                    },
                    InsuredId = insured.InsuredId,
                    PaymentMethod = new IssuancePaymentMethod
                    {
                        Id = insured.PaymentMethod.Id
                    },
                    BirthDate = insured.BirthDate,
                    Gender = insured.Gender,
                    DeclinedDate = insured.DeclinedDate.GetValueOrDefault(),
                    CompanyName = new IssuanceCompanyName
                    {
                        NameNum = insured.CompanyName.NameNum,
                        TradeName = insured.CompanyName.TradeName,
                        Address = insured.CompanyName?.Address == null ? null : new IssuanceAddress
                        {
                            Id = insured.CompanyName.Address.Id,
                            Description = insured.CompanyName.Address.Description
                        },
                        Phone = insured.CompanyName?.Phone == null ? null : new IssuancePhone
                        {
                            Id = insured.CompanyName.Phone.Id,
                            Description = insured.CompanyName.Phone.Description
                        },
                        Email = insured.CompanyName?.Email == null ? null : new IssuanceEmail
                        {
                            Id = insured.CompanyName.Email.Id,
                            Description = insured.CompanyName.Email.Description
                        }
                    }
                };
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Tomador        
        /// </summary>
        /// <param name="individualId"></param>
        /// <returns></returns>
        public IssuanceInsured GetHolderByIndividualId(int individualId)
        {
            IssuanceInsured insured = new IssuanceInsured();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePerson.Entities.Insured.Properties.IndividualId, typeof(UniquePerson.Entities.Insured).Name);
            filter.Equal();
            filter.Constant(individualId);

            SelectQuery selectQuery = new SelectQuery();
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.InsuredCode, typeof(UniquePerson.Entities.Insured).Name)));
            selectQuery.AddSelectValue(new SelectValue(new Column(UniquePerson.Entities.Insured.Properties.DeclinedDate, typeof(UniquePerson.Entities.Insured).Name)));
            selectQuery.Table = new ClassNameTable(typeof(UniquePerson.Entities.Insured), typeof(UniquePerson.Entities.Insured).Name);
            selectQuery.Where = filter.GetPredicate();
            using (IDataReader reader = DataFacadeManager.Instance.GetDataFacade().Select(selectQuery))
            {
                while (reader.Read())
                {
                    insured = new IssuanceInsured
                    {
                        CustomerType = CustomerType.Individual,
                        IndividualId = individualId,
                        InsuredId = Convert.ToInt32(reader["InsuredCode"]),
                        DeclinedDate = (DateTime?)reader["DeclinedDate"]
                    };
                    break;
                }
            }
            return insured;
        }
    }
}