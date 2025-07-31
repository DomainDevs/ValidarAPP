using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistran.Company.Application.UnderwritingServices.EEProvider.Helpers
{
    static class HolderConvert
    {
        public static Holder ToHolderFullModel(this IssuanceInsured insured)
        {
            return new Holder
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
                },
                EconomicActivity = insured.EconomicActivity?.Id == null? null: new IssuanceEconomicActivity
                {
                    Id = insured.EconomicActivity.Id
                },
                PaymentMethod = insured.PaymentMethod?.PaymentId == null? null : new IssuancePaymentMethod
                {
                    Id = insured.PaymentMethod.Id
                },
                BirthDate = insured.BirthDate,
                Gender = insured.Gender,
                DeclinedDate = insured.DeclinedDate.HasValue ? insured.DeclinedDate : null,
                CompanyName = insured.CompanyName == null? null : new IssuanceCompanyName
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
                },
                AssociationType = insured.AssociationType == null ? null : new IssuanceAssociationType
                {
                    Id = insured.AssociationType.Id
                }
            };

        }

        public static Holder ToHolderModel(this IssuanceInsured insured)
        {
            return new Holder
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
                },
                AssociationType = insured.AssociationType==null?null:new IssuanceAssociationType
                {
                    Id = insured.AssociationType.Id
                }
            };
        }

        public static List<Holder> ToHolderModelList(this List<IssuanceInsured> insureds)
        {
            return insureds.Select(i => i.ToHolderModel()).ToList();
        }

        public static List<Holder> ToHolderFullModelList(this List<IssuanceInsured> insureds)
        {
            return insureds.Select(i => i.ToHolderFullModel()).ToList();
        }
    }
}
