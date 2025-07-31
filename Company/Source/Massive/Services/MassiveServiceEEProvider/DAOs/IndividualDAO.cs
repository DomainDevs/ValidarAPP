using Sistran.Company.Application.Utilities.Constants;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Constants;
using Sistran.Core.Application.Utilities.Managers;
using Sistran.Core.Framework;
using Sistran.Core.Framework.BAF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sistran.Core.Application.Utilities.Helper;
using Sistran.Company.Application.Massive.EEProvider.DAOs;
using Sistran.Company.Application.MassiveServices.EEProvider.Resources;
using Sistran.Company.Application.MassiveServices.EEProvider.Business;
using Sistran.Core.Services.UtilitiesServices.Models;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Company.Application.MassiveServices.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.Cache;

namespace Sistran.Company.Application.MassiveServices.EEProvider.DAOs
{
    public class IndividualDAO
    {
        private static List<CompanyBeneficiaryType> companyBeneficiaryTypes = new List<CompanyBeneficiaryType>();
        public IndividualDAO() 
        {
            if (companyBeneficiaryTypes.Count() == 0)
            {
                companyBeneficiaryTypes = DelegateService.underwritingService.GetCompanyBeneficiaryTypes();
            }
            
        } 
        
        /// <summary>
        /// Obtiene el Filter Individual de acuerdo a la Fila y a los property name
        /// </summary>
        /// <param name="row"></param>
        /// <param name="propertyNameInsuredCode"></param>
        /// <param name="propertyNameIndividualType"></param>
        /// <param name="propertyNamePersonDocument"></param>
        /// <param name="propertyNameCompanyDocument"></param>
        /// <returns></returns>
        private FilterIndividual GetFilterIndividualByRow(Row row, string propertyNameInsuredCode, string propertyNameIndividualType, string propertyNamePersonDocument, 
            string propertyNameCompanyDocument, string propertyNamePersonDocumentType, string propertyNameCompanyDocumentType, List<FilterIndividual> filterIndividuals)
        {
            FilterIndividual filterIndividual = null;
            int insuredCode = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(p => p.PropertyName == propertyNameInsuredCode));
            if (insuredCode > 0)
            {
                filterIndividual = filterIndividuals.Find(p => p.InsuredCode == insuredCode);
            }
            else
            {
                IndividualType individualTypes = BusinessIndividual.FindIndividualTypeByRow(row, propertyNameIndividualType, propertyNamePersonDocument, propertyNameCompanyDocument);
                string documentNumber = string.Empty;
                int documentType;
                switch (individualTypes)
                {
                    case IndividualType.Person:
                        documentNumber = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(p => p.PropertyName == propertyNamePersonDocument));
                        documentType = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(p => p.PropertyName == propertyNamePersonDocumentType));
                        filterIndividual = filterIndividuals.Find(p => p.IndividualType == IndividualType.Person && p.Person.DocumentNumber == documentNumber && p.Person.DocumentTypeId == documentType);

                        break;

                    case IndividualType.Company:
                        documentNumber = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(p => p.PropertyName == propertyNameCompanyDocument));
                        documentType = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(p => p.PropertyName == propertyNameCompanyDocumentType));
                        filterIndividual = filterIndividuals.Find(p => p.IndividualType == IndividualType.Company && p.Company.DocumentNumber == documentNumber && p.Company.DocumentTypeId == documentType);
                        break;
                    default:
                        row.HasError = true;
                        row.ErrorDescription += StringHelper.ConcatenateString("Tipo de individuo no encontrado en", propertyNameIndividualType);
                        break;
                }
            }
            return filterIndividual;
        }

        public CompanyIssuanceInsured CreateInsured(Row row, Holder holder, List<FilterIndividual> filtersIndividuals)
        {
            CompanyIssuanceInsured insured = new CompanyIssuanceInsured();
            bool InsuredEqualHolder = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(p => p.PropertyName == CompanyFieldPropertyName.InsuredEqualHolder));
            if (InsuredEqualHolder)
            {
                insured.CustomerType = holder.CustomerType;
                insured.IndividualType = holder.IndividualType;
                insured.IndividualId = holder.IndividualId;
                insured.Name = holder.Name;
                insured.IdentificationDocument = holder.IdentificationDocument;
                insured.EconomicActivity = holder.EconomicActivity;
                insured.InsuredId = holder.InsuredId;
                insured.PaymentMethod = holder.PaymentMethod;
                insured.BirthDate = holder.BirthDate;
                insured.Gender = holder.Gender;
                insured.DeclinedDate = holder.DeclinedDate;
                insured.CompanyName = holder.CompanyName;
                insured.Surname = holder.Surname;
            }
            else
            {
                FilterIndividual filterIndividual = GetFilterIndividualByRow(row,
                    FieldPropertyName.InsuredCode,
                    FieldPropertyName.InsuredIndividualType,
                    FieldPropertyName.InsuredPersonDocumentNumber,
                    FieldPropertyName.InsuredCompanyDocumentNumber,
                    FieldPropertyName.InsuredPersonDocumentType,
                    FieldPropertyName.InsuredCompanyDocumentType, filtersIndividuals);

                if (filterIndividual != null)
                {
                    if (!string.IsNullOrEmpty(filterIndividual.Error))
                    {
                        row.HasError = true;
                        row.ErrorDescription += String.Format("{0} {1}", Errors.ErrorInsuredDataIncorrect, filterIndividual.Error);
                        throw new ValidationException(row.ErrorDescription);
                    }
                    insured = CreateInsuredByFilterIndividual(filterIndividual);
                }
            }
            return insured;
        }

        public CompanyBeneficiary CreateBeneficiary(Row row, CompanyIssuanceInsured insured, List<FilterIndividual> filtersIndividuals)
        {
            CompanyBeneficiary beneficiary = new CompanyBeneficiary();
            bool beneficiaryEqualInsured = (bool)DelegateService.utilitiesService.GetValueByField<bool>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BeneficiaryEqualInsured));
            if (beneficiaryEqualInsured)
            {
                beneficiary.IndividualId = insured.IndividualId;
                beneficiary.IdentificationDocument = insured.IdentificationDocument;
                beneficiary.CompanyName = insured.CompanyName;
                beneficiary.Name = insured.Name;
                beneficiary.BeneficiaryType = new CompanyBeneficiaryType
                {
                    Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(p => p.PropertyName == FieldPropertyName.BeneficiaryBeneficiaryType))
                };
                beneficiary.CustomerType = insured.CustomerType;
                beneficiary.Participation = 100;
                beneficiary.DeclinedDate = insured.DeclinedDate;
            }
            else
            {
                FilterIndividual filterIndividual = GetFilterIndividualByRow(row, FieldPropertyName.BeneficiaryInsuredCode, FieldPropertyName.BeneficiaryIndividualType, FieldPropertyName.BeneficiaryPersonDocumentNumber, FieldPropertyName.BeneficiaryCompanyDocumentNumber, FieldPropertyName.BeneficiaryPersonDocumentType,
                    FieldPropertyName.BeneficiaryCompanyDocumentType, filtersIndividuals);

                if (filterIndividual != null)
                {
                    if (!string.IsNullOrEmpty(filterIndividual.Error))
                    {
                        row.HasError = true;
                        row.ErrorDescription += String.Format("{0} {1}", Errors.ErrorBeneficiaryDataIncorrect, filterIndividual.Error);
                        throw new ValidationException(row.ErrorDescription);
                    }
                    beneficiary = CreateBeneficiaryByFilterIndividual(row, filterIndividual);
                    beneficiary.Participation = 100;
                }
                else
                {
                    throw new ValidationException(Errors.BeneficiaryNotExists);
                }
            }
            
            if (!companyBeneficiaryTypes.Any(x => x.Id == beneficiary.BeneficiaryType.Id))
            {
                row.HasError = true;
                row.ErrorDescription += String.Format(Errors.BeneficiaryType);
                throw new ValidationException(row.ErrorDescription);
               
            }

            return beneficiary;
        }

        /// <summary>
        /// Crea los beneficiarios adicionales
        /// </summary>
        /// <param name="beneficiariesTemplate"></param>
        /// <returns></returns>
        public List<CompanyBeneficiary> CreateAdditionalBeneficiaries(Template beneficiariesTemplate, List<FilterIndividual> filtersIndividuals)
        {
            List<CompanyBeneficiary> beneficiaries = new List<CompanyBeneficiary>();
            if (beneficiariesTemplate != null)
            {
                ParallelHelper.ForEach(beneficiariesTemplate.Rows, row =>
                {
                    FilterIndividual filterIndividual = GetFilterIndividualByRow(row,
                        FieldPropertyName.BeneficiaryInsuredCode,
                        FieldPropertyName.BeneficiaryIndividualType,
                        FieldPropertyName.BeneficiaryPersonDocumentNumber,
                        FieldPropertyName.BeneficiaryCompanyDocumentNumber,
                        FieldPropertyName.BeneficiaryPersonDocumentType,
                        FieldPropertyName.BeneficiaryCompanyDocumentType, filtersIndividuals);

                    if (filterIndividual != null)
                    {
                        if (!string.IsNullOrEmpty(filterIndividual.Error))
                        {
                            row.HasError = true;
                            row.ErrorDescription += String.Format("{0} {1}", Errors.ErrorBeneficiaryDataIncorrect, filterIndividual.Error);
                            throw new ValidationException(row.ErrorDescription);
                        }

                        CompanyBeneficiary beneficiary = CreateBeneficiaryByFilterIndividual(row, filterIndividual);
                        beneficiary.Participation = (decimal)DelegateService.utilitiesService.GetValueByField<decimal>(row.Fields.Find(p => p.PropertyName == FieldPropertyName.BeneficiaryParticipation));
                        beneficiaries.Add(beneficiary);
                    }
                });
            }

            return beneficiaries;
        }

        /// <summary>
        /// Crea el tomador
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public Holder CreateHolder(Row row, List<FilterIndividual> filtersIndividuals)
        {
            FilterIndividual filterIndividual = null;
            //Si el individuo existe en el excel, lo busca por CodigoAsegurad y/o documento
            if (FilterIndividualDAO.ExistsHolderInExcel(row))
            {
                filterIndividual = GetFilterIndividualByRow(row,
                    FieldPropertyName.HolderInsuredCode,
                    FieldPropertyName.HolderIndividualType,
                    FieldPropertyName.HolderPersonDocumentNumber,
                    FieldPropertyName.HolderCompanyDocumentNumber,
                    FieldPropertyName.HolderPersonDocumentType,
                    FieldPropertyName.HolderCompanyDocumentType, 
                    filtersIndividuals);
            }
            else //Si no existe en el excel y es solicitud agrupadaroa lo busca por Codigo Agrupador y Numero de solicitud
            {
                bool withRequestGrouping = false;
                if (row.Fields.Any(y => y.PropertyName == FieldPropertyName.RequestGroup))
                    withRequestGrouping = true;
                if (withRequestGrouping)
                {
                    int billingGroup = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.BillingGroup));
                    string requestGroup = (string)DelegateService.utilitiesService.GetValueByField<string>(row.Fields.Find(x => x.PropertyName == FieldPropertyName.RequestGroup));
                }
            }
            if (filterIndividual == null)
                throw new ValidationException(Errors.HolderNotExists);
            if (!string.IsNullOrEmpty(filterIndividual.Error.Trim()))
            {
                row.HasError = true;
                row.ErrorDescription += String.Format("{0} {1}", Errors.ErrorHolderDataIncorrect, filterIndividual.Error);
                throw new ValidationException(row.ErrorDescription);

            }
            return CreateHolderByFilterIndividual(filterIndividual);
        }

        private CompanyIssuanceInsured CreateInsuredByFilterIndividual(FilterIndividual filterIndividual)
        {
            CompanyIssuanceInsured insured = new CompanyIssuanceInsured();

            if (filterIndividual.Person != null)
            {
                insured = new CompanyIssuanceInsured()
                {
                    IndividualType = IndividualType.Person,
                    InsuredId = filterIndividual.InsuredCode.GetValueOrDefault(),
                    IndividualId = filterIndividual.Person.IndividualId,
                    IdentificationDocument = new IssuanceIdentificationDocument
                    {
                        Number = filterIndividual.Person.DocumentNumber,
                        DocumentType = new IssuanceDocumentType
                        {
                            Id = filterIndividual.Person.DocumentTypeId
                        }
                    },
                    Name = filterIndividual.Person.FullName,
                    EnteredDate = DateTime.Now,
                    Profile = 2,
                    CompanyName = new IssuanceCompanyName()
                    {
                        Address = new IssuanceAddress
                        {
                            Id = filterIndividual.Person.AddressId,
                            Description = filterIndividual.Person.AddressDescription,
                            City = filterIndividual.Person.AddressCity
                        },
                        Phone = new IssuancePhone
                        {
                            Id = filterIndividual.Person.PhoneId,
                            Description = filterIndividual.Person.PhoneDescription
                        },
                        Email = new IssuanceEmail
                        {
                            Id = filterIndividual.Person.EmailId,
                            Description = filterIndividual.Person.EmailDescription
                        }
                    },
                    BirthDate = filterIndividual.Person.BirthDate,
                    Gender = filterIndividual.Person.Gender,
                    CustomerType = filterIndividual.Person.CustomerType,
                    EconomicActivity = new IssuanceEconomicActivity
                    {
                        Id = filterIndividual.Person.EconomicActivityId
                    },
                    PaymentMethod = new IssuancePaymentMethod
                    {
                        PaymentId = filterIndividual.Person.PaymentId,
                        Id = filterIndividual.Person.PaymentMethodId
                    },
                    //CustomerTypeDescription = filterIndividual.Person.CustomerTypeDescription,
                    //OwnerRoleCode = filterIndividual.Person.OwnerRoleCode,
                    //SecondSurname = filterIndividual.Person.MotherLastName,
                    Surname = filterIndividual.Person.Surname,
                };
            }
            else if (filterIndividual.Company != null)
            {
                insured = new CompanyIssuanceInsured()
                {
                    IndividualType = IndividualType.Company,
                    InsuredId = filterIndividual.InsuredCode.GetHashCode(),
                    IndividualId = filterIndividual.Company.IndividualId,
                    IdentificationDocument = new IssuanceIdentificationDocument
                    {
                        Number = filterIndividual.Company.DocumentNumber,
                        DocumentType = new IssuanceDocumentType
                        {
                            Id = filterIndividual.Company.DocumentTypeId
                        }
                    },
                    Name = filterIndividual.Company.FullName,
                    EnteredDate = DateTime.Now,
                    Profile = 2,
                    //BranchCode = 1,
                    //IsCommercialClient = filterIndividual.IsCommercialClient,
                    //IsMailAddress = filterIndividual.IsMailAddress,
                    //IsSms = filterIndividual.IsSms,
                    CustomerType = filterIndividual.Company.CustomerType,
                    EconomicActivity = new IssuanceEconomicActivity
                    {
                        Id = filterIndividual.Company.EconomicActivityId
                    },
                    PaymentMethod = new IssuancePaymentMethod
                    {
                        PaymentId = filterIndividual.Company.PaymentId,
                        Id = filterIndividual.Company.PaymentMethodId
                    },
                    CompanyName = new IssuanceCompanyName()
                    {
                        Address = new IssuanceAddress
                        {
                            Id = filterIndividual.Company.AddressId,
                            Description = filterIndividual.Company.AddressDescription,
                            City = filterIndividual.Company.AddressCity
                        },
                        Phone = new IssuancePhone
                        {
                            Id = filterIndividual.Company.PhoneId,
                            Description = filterIndividual.Company.PhoneDescription
                        },
                        Email = new IssuanceEmail
                        {
                            Id = filterIndividual.Company.EmailId,
                            Description = filterIndividual.Company.EmailDescription
                        }
                    }
                };
            }
            return insured;
        }

        private CompanyBeneficiary CreateBeneficiaryByFilterIndividual(Row row, FilterIndividual filterIndividual)
        {
            CompanyBeneficiary beneficiary = new CompanyBeneficiary();
            if (filterIndividual.Person != null)
            {
                beneficiary = new CompanyBeneficiary()
                {
                    IndividualId = filterIndividual.Person.IndividualId,
                    IdentificationDocument = new IssuanceIdentificationDocument
                    {
                        Number = filterIndividual.Person.DocumentNumber,
                        DocumentType = new IssuanceDocumentType
                        {
                            Id = filterIndividual.Person.DocumentTypeId
                        }
                    },
                    Name = filterIndividual.Person.FullName,
                    CustomerType = filterIndividual.Person.CustomerType,
                    BeneficiaryType = new CompanyBeneficiaryType
                    {
                        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(p => p.PropertyName == FieldPropertyName.BeneficiaryBeneficiaryType))
                    },
                    CompanyName = new IssuanceCompanyName()
                    {
                        Address = new IssuanceAddress
                        {
                            Id = filterIndividual.Person.AddressId,
                            Description = filterIndividual.Person.AddressDescription,
                            City = filterIndividual.Person.AddressCity
                        },
                        Phone = new IssuancePhone
                        {
                            Id = filterIndividual.Person.PhoneId,
                            Description = filterIndividual.Person.PhoneDescription
                        },
                        Email = new IssuanceEmail
                        {
                            Id = filterIndividual.Person.EmailId,
                            Description = filterIndividual.Person.EmailDescription
                        }
                    },
                    DeclinedDate = filterIndividual.DeclinedDate
                };
            }
            else if (filterIndividual.Company != null)
            {
                beneficiary = new CompanyBeneficiary()
                {
                    IndividualId = filterIndividual.Company.IndividualId,
                    IdentificationDocument = new IssuanceIdentificationDocument
                    {
                        Number = filterIndividual.Company.DocumentNumber,
                        DocumentType = new IssuanceDocumentType
                        {
                            Id = filterIndividual.Company.DocumentTypeId
                        }
                    },
                    Name = filterIndividual.Company.FullName,
                    CustomerType = filterIndividual.Company.CustomerType,
                    CompanyName = new IssuanceCompanyName()
                    {
                        Address = new IssuanceAddress
                        {
                            Id = filterIndividual.Company.AddressId,
                            Description = filterIndividual.Company.AddressDescription,
                            City = filterIndividual.Company.AddressCity
                        },
                        Phone = new IssuancePhone
                        {
                            Id = filterIndividual.Company.PhoneId,
                            Description = filterIndividual.Company.PhoneDescription
                        },
                        Email = new IssuanceEmail
                        {
                            Id = filterIndividual.Company.EmailId,
                            Description = filterIndividual.Company.EmailDescription
                        }
                    },
                    BeneficiaryType = new CompanyBeneficiaryType
                    {
                        Id = (int)DelegateService.utilitiesService.GetValueByField<int>(row.Fields.Find(p => p.PropertyName == FieldPropertyName.BeneficiaryBeneficiaryType))
                    },
                    DeclinedDate = filterIndividual.DeclinedDate
                };
            }
            return beneficiary;
        }

        private Holder CreateHolderByFilterIndividual(FilterIndividual filterIndividual)
        {
            Holder holder = new Holder();
            if (filterIndividual.Person != null)
            {
                holder = new Holder()
                {
                    IndividualType = IndividualType.Person,
                    IndividualId = filterIndividual.Person.IndividualId,
                    InsuredId = filterIndividual.InsuredCode.GetValueOrDefault(),
                    IdentificationDocument = new IssuanceIdentificationDocument
                    {
                        Number = filterIndividual.Person.DocumentNumber,
                        DocumentType = new IssuanceDocumentType
                        {
                            Id = filterIndividual.Person.DocumentTypeId
                        }
                    },
                    Name = filterIndividual.Person.FullName,
                    CustomerType = filterIndividual.Person.CustomerType,
                    EconomicActivity = new IssuanceEconomicActivity
                    {
                        Id = filterIndividual.Person.EconomicActivityId
                    },
                    PaymentMethod = new IssuancePaymentMethod
                    {
                        PaymentId = filterIndividual.Person.PaymentId,
                        Id = filterIndividual.Person.PaymentMethodId
                    },
                    CompanyName = new IssuanceCompanyName()
                    {
                        Address = new IssuanceAddress
                        {
                            Id = filterIndividual.Person.AddressId,
                            Description = filterIndividual.Person.AddressDescription,
                            City = filterIndividual.Person.AddressCity
                        },
                        Phone = new IssuancePhone
                        {
                            Id = filterIndividual.Person.PhoneId,
                            Description = filterIndividual.Person.PhoneDescription
                        },
                        Email = new IssuanceEmail
                        {
                            Id = filterIndividual.Person.EmailId,
                            Description = filterIndividual.Person.EmailDescription
                        }
                    },
                    BirthDate = filterIndividual.Person.BirthDate,
                    Gender = filterIndividual.Person.Gender
                };

            }
            else if (filterIndividual.Company != null)
            {
                holder = new Holder()
                {
                    IndividualType = IndividualType.Company,
                    IndividualId = filterIndividual.Company.IndividualId,
                    InsuredId = filterIndividual.InsuredCode.GetValueOrDefault(),
                    IdentificationDocument = new IssuanceIdentificationDocument
                    {
                        Number = filterIndividual.Company.DocumentNumber,
                        DocumentType = new IssuanceDocumentType
                        {
                            Id = filterIndividual.Company.DocumentTypeId
                        }
                    },
                    Name = filterIndividual.Company.FullName,
                    CustomerType = filterIndividual.Company.CustomerType,
                    EconomicActivity = new IssuanceEconomicActivity
                    {
                        Id = filterIndividual.Company.EconomicActivityId
                    },
                    PaymentMethod = new IssuancePaymentMethod
                    {
                        PaymentId = filterIndividual.Company.PaymentId,
                        Id = filterIndividual.Company.PaymentMethodId
                    },
                    CompanyName = new IssuanceCompanyName()
                    {
                        Address = new IssuanceAddress
                        {
                            Id = filterIndividual.Company.AddressId,
                            Description = filterIndividual.Company.AddressDescription,
                            City = filterIndividual.Company.AddressCity
                        },
                        Phone = new IssuancePhone
                        {
                            Id = filterIndividual.Company.PhoneId,
                            Description = filterIndividual.Company.PhoneDescription
                        },
                        Email = new IssuanceEmail
                        {
                            Id = filterIndividual.Company.EmailId,
                            Description = filterIndividual.Company.EmailDescription
                        }
                    }
                };
            }

            return holder;
        }
    }
}
