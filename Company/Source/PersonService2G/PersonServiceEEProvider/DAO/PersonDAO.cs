using PersonService.Model;
using Sistran.Co.Application.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace PersonServiceEEProvider.DAO
{
    internal class PersonDAO
    {
        public List<Person> GetPerson2gByDocumentNumber(string documentNumber, bool company)
        {
            try
            {
                DataTable result = new DataTable();
                List<Person> people = new List<Person>();
                NameValue[] parameters = new NameValue[2];
                parameters[0] = new NameValue("@NRO_DOC", documentNumber);
                parameters[1] = new NameValue("@COMPANY", company);
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess("Sise2G"))
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("INT.GET_PERSON_2G_BY_DOCUMENT_NUMBER", parameters);
                }

                if (result != null)
                {
                    foreach (DataRow dataRow in result.Rows)
                    {
                        people.Add(new Person
                        {
                            Id = Convert.ToInt32(dataRow["ID_PERSONA"]),
                            DocumentNumber = Convert.ToString(dataRow["NRO_DOC"]),
                            FullName = Convert.ToString(dataRow["FULLNAME"]),
                            Role = Convert.ToString(dataRow["ROL"]),
                        });
                    };

                    return people;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Person> GetPerson2gByPersonId(int personId, bool company)
        {
            try
            {
                DataSet result = new DataSet();
                List<Person> people = new List<Person>();
                List<Address> addresses = new List<Address>();
                List<Email> emails = new List<Email>();
                List<Phone> phones = new List<Phone>();
                NameValue[] parameters = new NameValue[2];
                parameters[0] = new NameValue("@PERSON_ID", personId);
                parameters[1] = new NameValue("@COMPANY", company);

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess("Sise2G"))
                {
                    result = dynamicDataAccess.ExecuteSPDataSet("INT.GET_PERSON_2G_BY_PERSON_ID", parameters);
                }

                if (result != null)
                {
                    foreach (DataRow dataRow in result.Tables[1].Rows)
                    {
                        addresses.Add(new Address
                        {
                            Id = Convert.ToInt32(dataRow["COD_TIPO_DIR"]),
                            TipoDireccion = Convert.ToString(dataRow["TDIRECCION"]),
                            Description = Convert.ToString(dataRow["TXT_DIRECCION"]),
                            CountryId = Convert.ToInt32(dataRow["COD_PAIS"]),
                            CountryDescription = Convert.ToString(dataRow["PAIS"]),
                            StateId = Convert.ToInt32(dataRow["COD_DPTO"]),
                            CityId = Convert.ToInt32(dataRow["COD_MUNICIPIO"])
                        });
                    }

                    foreach (DataRow dataRow in result.Tables[2].Rows)
                    {
                        phones.Add(new Phone
                        {
                            Id = Convert.ToInt32(dataRow["COD_TIPO_TELEF"]),
                            SmallDescription = Convert.ToString(dataRow["TIPO_TELEFONO"]),
                            Description = Convert.ToString(dataRow["TXT_TELEFONO"]),
                        });
                    }

                    foreach (DataRow dataRow in result.Tables[0].Rows)
                    {
                        people.Add(new Person
                        {
                            DocumentNumber = Convert.ToString(dataRow["NRO_DOC"]),
                            IdentificationDocument = new IdentificationDocument
                            {
                                DocumentType = new DocumentType
                                {
                                    Id = Convert.ToInt32(dataRow["COD_TIPO_DOC"]),
                                    SmallDescription = Convert.ToString(dataRow["TXT_DESC_REDU"]),
                                    Description = Convert.ToString(dataRow["TDOC"])
                                }
                            },
                            Surname = Convert.ToString(dataRow["TXT_APELLIDO1"]),
                            SecondSurname = Convert.ToString(dataRow["TXT_APELLIDO2"]),
                            Names = Convert.ToString(dataRow["TXT_NOMBRE"]),
                            FullName = Convert.ToString(dataRow["FULLNAME"]),
                            Gender = Convert.ToString(dataRow["TXT_SEXO"]),
                            EconomicActivityId = Convert.ToInt32(dataRow["COD_CIIU"].ToString() != string.Empty ? dataRow["COD_CIIU"] : 0),
                            MaritalStatusId = Convert.ToInt32(dataRow["COD_EST_CIVIL"]),
                            BirthDate = Convert.ToDateTime(dataRow["FEC_NAC"]),
                            BirthPlace = Convert.ToString(dataRow["TXT_LUGAR_NAC"]),
                            Addresses = addresses,
                            Phones = phones
                        });
                    };

                    return people;
                }
                return null;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Company> GetCompany2gByCompanyId(int companyId, bool company)
        {
            try
            {
                DataSet result = new DataSet();
                List<Company> companies = new List<Company>();
                List<Address> addresses = new List<Address>();
                List<Email> emails = new List<Email>();
                List<Phone> phones = new List<Phone>();
                NameValue[] parameters = new NameValue[2];
                parameters[0] = new NameValue("@PERSON_ID", companyId);
                parameters[1] = new NameValue("@COMPANY", company);

                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess("Sise2G"))
                {
                    result = dynamicDataAccess.ExecuteSPDataSet("INT.GET_PERSON_2G_BY_PERSON_ID", parameters);
                }

                if (result != null)
                {
                    foreach (DataRow dataRow in result.Tables[1].Rows)
                    {
                        addresses.Add(new Address
                        {
                            Id = Convert.ToInt32(dataRow["COD_TIPO_DIR"]),
                            TipoDireccion = Convert.ToString(dataRow["TDIRECCION"]),
                            Description = Convert.ToString(dataRow["TXT_DIRECCION"]),
                            CountryId = Convert.ToInt32(dataRow["COD_PAIS"]),
                            CountryDescription = Convert.ToString(dataRow["PAIS"]),
                            StateId = Convert.ToInt32(dataRow["COD_DPTO"]),
                            CityId = Convert.ToInt32(dataRow["COD_MUNICIPIO"])
                        });
                    }

                    foreach (DataRow dataRow in result.Tables[2].Rows)
                    {
                        phones.Add(new Phone
                        {
                            Id = Convert.ToInt32(dataRow["COD_TIPO_TELEF"]),
                            SmallDescription = Convert.ToString(dataRow["TIPO_TELEFONO"]),
                            Description = Convert.ToString(dataRow["TXT_TELEFONO"]),
                        });
                    }

                    foreach (DataRow dataRow in result.Tables[0].Rows)
                    {
                        companies.Add(new Company
                        {
                            DocumentNumber = Convert.ToString(dataRow["NRO_DOC"]),
                            IdentificationDocument = new IdentificationDocument
                            {
                                DocumentType = new DocumentType
                                {
                                    Id = Convert.ToInt32(dataRow["COD_TIPO_DOC"]),
                                    SmallDescription = Convert.ToString(dataRow["TXT_DESC_REDU"]),
                                    Description = Convert.ToString(dataRow["TDOC"])
                                }
                            },
                            FullName = Convert.ToString(dataRow["FULLNAME"]),
                            VerifyDigit = Convert.ToInt32(dataRow["NRO_NIT_VERIF"]),
                            AssociationType = Convert.ToInt32(dataRow["COD_TASOCIACION"].ToString() != string.Empty ? dataRow["COD_TASOCIACION"] : 0),
                            EconomicActivityId = Convert.ToInt32(dataRow["COD_CIIU"].ToString() != string.Empty ? dataRow["COD_CIIU"] : 0),
                            CompanyType = Convert.ToInt32(dataRow["COD_TTIPO_EMPRESA"].ToString() != string.Empty ? dataRow["COD_TTIPO_EMPRESA"] : 0),
                            CountryId = Convert.ToInt32(dataRow["COD_PAIS"].ToString() != string.Empty ? dataRow["COD_PAIS"] : 0),
                            Addresses = addresses,
                            Phones = phones
                        });
                    };

                    return companies;
                }
                return null;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Provider GetProvider2g(int personId)
        {
            try
            {
                DataSet result = new DataSet();
                Provider provider = new Provider();
                NameValue[] parameters = new NameValue[1];
                parameters[0] = new NameValue("@PERSON_ID", personId);
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess("Sise2G"))
                {
                    result = dynamicDataAccess.ExecuteSPDataSet("INT.GET_PROVIDER_2G_BY_PERSON_ID", parameters);
                }

                if (result != null)
                {
                    provider = new Provider
                    {
                        SupplierProfileId = Convert.ToInt32(result.Tables[0].Rows[0]["SUPPLIER_PROFILE_CD"].ToString() != string.Empty ? result.Tables[0].Rows[0]["SUPPLIER_PROFILE_CD"] : 0),
                        ProviderTypeId = Convert.ToInt32(result.Tables[0].Rows[0]["COD_TIPO_PRES"]),
                        CreationDate = Convert.ToDateTime(result.Tables[0].Rows[0]["FEC_ALTA"]),
                        ProviderDeclinedTypeId = Convert.ToInt32(result.Tables[0].Rows[0]["COD_BAJA"].ToString() != string.Empty ? result.Tables[0].Rows[0]["COD_BAJA"] : 0),
                    };
                    if (result.Tables[0].Rows[0]["FEC_BAJA"].ToString() != string.Empty)
                    {
                        provider.DeclinationDate = Convert.ToDateTime(result.Tables[0].Rows[0]["FEC_BAJA"]);
                    }
                    if (result.Tables[1].Rows.Count > 0)
                    {
                        provider.providerPaymentConcepts = new List<ProviderPaymentConcept>();
                        foreach (DataRow dataRow in result.Tables[1].Rows)
                        {
                            provider.providerPaymentConcepts.Add(new ProviderPaymentConcept
                            {
                                PaymentConceptId = Convert.ToInt32(dataRow["COD_CPTO"])
                            });
                        }
                    }

                    return provider;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<BankTransfer> GetBankTransfer2g(int transferPersonId)
        {
            try
            {
                DataTable result = new DataTable();
                List<BankTransfer> bankTransfers = new List<BankTransfer>();
                NameValue[] parameters = new NameValue[1];
                parameters[0] = new NameValue("@PERSON_ID", transferPersonId);
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess("Sise2G"))
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("INT.GET_BANKTRANSFER_2G_BY_PERSON_ID", parameters);
                }

                if (result != null)
                {
                    foreach (DataRow dataRow in result.Rows)
                    {
                        bankTransfers.Add(new BankTransfer
                        {
                            BankId = Convert.ToInt32(dataRow["COD_BANCO"]),
                            BankSquare = Convert.ToString(dataRow["TXT_COD_PLAZA"]),
                            BankBranchId = Convert.ToString(dataRow["COD_SUC_BANCO"]),
                            CityId = Convert.ToInt32(dataRow["COD_PLAZA"]),
                            CountryId = Convert.ToInt32(dataRow["COD_PAIS"]),
                            AccountTypeId = Convert.ToInt32(dataRow["COD_TIPO_CTA_BCO"]),
                            CurrencyId = Convert.ToInt32(dataRow["COD_MONEDA"]),
                            PaymentBeneficiary = Convert.ToString(dataRow["TXT_NOMBRE_BENEFICIARIO"]),
                            AccountNumber = Convert.ToString(dataRow["TXT_NUMERO_CUENTA"]),
                            ActiveAccount = dataRow["SN_STATUS_CUENTA"].ToString() == "-1" ? true : false,
                            DefaultAccount = dataRow["SN_DEFAULT"].ToString() == "-1" ? true : false
                        });
                    };

                    return bankTransfers;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Tax> GetTax2g(int taxPersonId)
        {
            try
            {
                DataTable result = new DataTable();
                List<Tax> Taxes = new List<Tax>();
                NameValue[] parameters = new NameValue[1];
                parameters[0] = new NameValue("@PERSON_ID", taxPersonId);
                using (DynamicDataAccess dynamicDataAccess = new DynamicDataAccess("Sise2G"))
                {
                    result = dynamicDataAccess.ExecuteSPDataTable("INT.GET_TAX_2G_BY_PERSON_ID", parameters);
                }

                if (result != null)
                {
                    foreach (DataRow dataRow in result.Rows)
                    {
                        Taxes.Add(new Tax
                        {
                            CountryId = Convert.ToInt32(dataRow["COUNTRY_CD"].ToString() != string.Empty ? dataRow["COUNTRY_CD"] : 0),
                            StateCode = Convert.ToInt32(dataRow["STATE_CD"].ToString() != string.Empty ? dataRow["STATE_CD"] : 0),
                            TaxId = Convert.ToInt32(dataRow["TAX_CD"]),
                            TaxCategoryId = Convert.ToInt32(dataRow["TAX_CATEGORY_CD"].ToString() != string.Empty ? dataRow["TAX_CATEGORY_CD"] : 0),
                            TaxCondition = Convert.ToInt32(dataRow["TAX_CONDITION_CD"]),
                            RoleId = Convert.ToInt32(dataRow["ROLE_CD"])
                        });
                    };

                    return Taxes;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
