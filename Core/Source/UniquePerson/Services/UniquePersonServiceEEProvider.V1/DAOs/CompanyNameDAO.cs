using Sistran.Core.Application.UniquePersonService.V1.Assemblers;
using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Core.Application.UniquePersonService.V1.Resources;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TP = Sistran.Core.Application.Utilities.Utility;

namespace Sistran.Core.Application.UniquePersonService.V1.DAOs
{
    /// <summary>
    /// Nombre Compañia
    /// </summary>
    public class CompanyNameDAO
    {
        /// <summary>
        /// Creates the name of the companies.
        /// </summary>
        /// <param name="coCompanyName">Name of the co company.</param>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public Models.CompanyName CreateCompaniesName(Models.CompanyName coCompanyName, int individualId)
        {
            UniquePersonV1.Entities.CoCompanyName coCompanyNameEntity = EntityAssembler.CreateCoCompanyName(coCompanyName);
            coCompanyNameEntity.IndividualId = individualId;
            DataFacadeManager.Instance.GetDataFacade().InsertObject(coCompanyNameEntity);
            return ModelAssembler.CreateCompanyName(coCompanyNameEntity);
        }
        /// <summary>
        /// Updates the name of the companies.
        /// </summary>
        /// <param name="coCompannyName">Name of the co companny.</param>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public Models.CompanyName UpdateCompaniesName(Models.CompanyName coCompannyName, int individualId)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePersonV1.Entities.CoCompanyName.Properties.IndividualId, typeof(UniquePersonV1.Entities.CoCompanyName).Name);
            filter.Equal();
            filter.Constant(individualId);
            filter.And();
            filter.Property(UniquePersonV1.Entities.CoCompanyName.Properties.NameNum, typeof(UniquePersonV1.Entities.CoCompanyName).Name);
            filter.Equal();
            filter.Constant(coCompannyName.NameNum);

            List<Models.CompanyName> coCompaniesNameList = GetCompanyNameByFilter(filter);
            if (coCompaniesNameList.Count > 0)
            {
                PrimaryKey key = UniquePersonV1.Entities.CoCompanyName.CreatePrimaryKey(individualId, coCompannyName.NameNum);
                UniquePersonV1.Entities.CoCompanyName coCompanyNameEntity = (UniquePersonV1.Entities.CoCompanyName)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);

                coCompanyNameEntity.TradeName = coCompannyName.TradeName;
                coCompanyNameEntity.IsMain = coCompannyName.IsMain;
                if (coCompannyName.Address.Id != 0)
                {
                    coCompanyNameEntity.AddressDataCode = coCompannyName.Address.Id;
                }
                if (coCompannyName.Phone.Id != 0)
                {
                    coCompanyNameEntity.PhoneDataCode = coCompannyName.Phone.Id;
                }
                if (coCompannyName.Email.Id != 0)
                {
                    coCompanyNameEntity.EmailDataCode = coCompannyName.Email.Id;
                }
                DataFacadeManager.Instance.GetDataFacade().UpdateObject(coCompanyNameEntity);
                return ModelAssembler.CreateCompanyName(coCompanyNameEntity);
            }
            else
            {
                return CreateCompaniesName(coCompannyName, individualId);

            }

        }

        /// <summary>
        /// Gets the company name by filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        public List<Models.CompanyName> GetCompanyNameByFilter(ObjectCriteriaBuilder filter)
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.CoCompanyName), filter.GetPredicate()));
            return ModelAssembler.CreateCompaniesName(businessCollection);
        }

        /// <summary>
        /// Gets the company name by individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public List<Models.CompanyName> GetCompanyNamesByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(UniquePersonV1.Entities.CoCompanyName.Properties.IndividualId, typeof(UniquePersonV1.Entities.CoCompanyName).Name);
            filter.Equal();
            filter.Constant(individualId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.CoCompanyName), filter.GetPredicate()));
            List<Models.CompanyName> businessName = ModelAssembler.CreateCompaniesName(businessCollection);

            ObjectCriteriaBuilder filterAddress = new ObjectCriteriaBuilder();
            filterAddress.Property(UniquePersonV1.Entities.Address.Properties.IndividualId, typeof(UniquePersonV1.Entities.Address).Name);
            filterAddress.Equal();
            filterAddress.Constant(individualId);
            AddressDAO addressDAO = new AddressDAO();
            List<Models.Address> address = addressDAO.GetAddressesByfilter(filterAddress);

            ObjectCriteriaBuilder filterPhone = new ObjectCriteriaBuilder();
            filterPhone.Property(UniquePersonV1.Entities.Phone.Properties.IndividualId, typeof(UniquePersonV1.Entities.Phone).Name);
            filterPhone.Equal();
            filterPhone.Constant(individualId);
            PhoneDAO PhoneDAO = new PhoneDAO();
            List<Models.Phone> phone = PhoneDAO.GetPhonesByFilter(filterPhone);

            ObjectCriteriaBuilder filterEmail = new ObjectCriteriaBuilder();
            filterEmail.Property(UniquePersonV1.Entities.Email.Properties.IndividualId, typeof(UniquePersonV1.Entities.Email).Name);
            filterEmail.Equal();
            filterEmail.Constant(individualId);
            EmailDAO EmailDAO = new EmailDAO();
            List<Models.Email> email = EmailDAO.GetEmailsByFilter(filterEmail);

            businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UniquePersonV1.Entities.EmailType)));
            List<Models.EmailType> emailTypes = ModelAssembler.CreateEmailTypes(businessCollection);
            foreach (Models.CompanyName item in businessName)
            {
                if (item.Address != null && address != null && address.Count > 0 && item.Address.Id != 0)
                {
                    item.Address = address.First(x => x.Id == item.Address.Id);
                }
                if (item.Phone != null && phone != null && phone.Count > 0 && item.Phone.Id != 0)
                {
                    item.Phone = phone.First(x => x.Id == item.Phone.Id);
                }
                if (item.Email != null && email != null && email.Count > 0 && item.Email.Id != 0)
                {
                    item.Email = email.First(x => x.Id == item.Email.Id);
                }
                if (item.Email != null && item.Email.EmailType != null && item.Email.EmailType.Id != 0)
                {
                    item.Email.EmailType = emailTypes.First(x => x.Id == item.Email.EmailType.Id);
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.V1.DAOs.GetCompanyNamesByIndividualId");
            return businessName;
        }

        /// <summary>
        /// Obtener Direcciones de Notificación del Individuo
        /// </summary>
        /// <param name="individualId">Id Individuo</param>
        /// <returns>Direcciones de Notificación</returns>
        public List<Models.CompanyName> GetNotificationAddressesByIndividualId(int individualId, CustomerType customerType)
        {
            List<Models.CompanyName> companyNames = new List<Models.CompanyName>();

            if (customerType == CustomerType.Individual)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(UniquePersonV1.Entities.CoCompanyName.Properties.IndividualId, typeof(UniquePersonV1.Entities.CoCompanyName).Name);
                filter.Equal();
                filter.Constant(individualId);
                BusinessCollection businessCollection = new BusinessCollection();
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    businessCollection = new BusinessCollection(daf.SelectObjects(typeof(UniquePersonV1.Entities.CoCompanyName), filter.GetPredicate()));
                }
                AddressDAO addressDAO = new AddressDAO();
                List<Models.Address> addresses = addressDAO.GetAddresses(individualId);
                addresses = addresses.OrderByDescending(x => x.IsPrincipal).ToList();

                PhoneDAO phoneDAO = new PhoneDAO();
                List<Models.Phone> phones = phoneDAO.GetPhonesByIndividualId(individualId);
                phones = phones.OrderByDescending(x => x.IsMain).ToList();

                EmailDAO emailDAO = new EmailDAO();
                List<Models.Email> emails = emailDAO.GetEmailsByIndividualId(individualId);
                emails = emails.OrderByDescending(x => x.IsPrincipal).ToList();

                if (businessCollection.Count > 0)
                {
                    companyNames = ModelAssembler.CreateCompaniesName(businessCollection);
                    if (companyNames.Count > 0)
                    {
                        

                        foreach (Models.CompanyName companyName in companyNames)
                        {
                            if(companyName.Address != null && companyName.Address?.Id != null && companyName.Address?.Id >0)
                            companyName.Address = addresses.Where(x => x.Id == companyName.Address?.Id).FirstOrDefault();
                            else
                            companyName.Address = addresses.FirstOrDefault();

                            if(companyName.Phone !=null && companyName.Phone?.Id != null && companyName.Phone?.Id > 0)
                            companyName.Phone = phones.Where(x => x.Id == companyName.Phone?.Id).FirstOrDefault();
                            else
                            companyName.Phone = phones.FirstOrDefault();

                            companyName.Email = emails.Where(x => x.Id == companyName.Email?.Id).FirstOrDefault();
                           
                        }

                        //companyNames[0].NameNum = 1;
                        //companyNames[0].IsMain = true;
                        //companyNames[0].TradeName = "Dirección Principal";
                        //CreateCompaniesName(companyNames[0], individualId);
                    }
                }
                else
                {
                    if (addresses.Count > 0)
                    {   foreach(Models.Address address in addresses)
                        {
                            Models.CompanyName companyName = new Models.CompanyName
                            {
                                Address = address
                            };

                            if (phones.Count > 0)
                            {
                                companyName.Phone = phones[0];
                                phones.RemoveAt(0);
                            }

                            if (emails.Count > 0)
                            {
                                companyName.Email = emails[0];
                                emails.RemoveAt(0);
                            }
                            companyNames.Add(companyName);
                        }
                        companyNames[0].NameNum = 1;
                        companyNames[0].IsMain = true;
                        companyNames[0].TradeName = "Dirección Principal";
                       // CreateCompaniesName(companyNames[0], individualId);
                    }
                }

                
            }
            else
            {
                PrimaryKey primaryKey = UniquePersonV1.Entities.Prospect.CreatePrimaryKey(individualId);
                UniquePersonV1.Entities.Prospect entityProspect = (UniquePersonV1.Entities.Prospect)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

                companyNames.Add(new Models.CompanyName
                {
                    Address = new Models.Address
                    {
                        Description = entityProspect.Street
                    },
                    Phone = new Models.Phone
                    {
                        Description = entityProspect.PhoneNumber.HasValue ? entityProspect.PhoneNumber.ToString() : ""
                    },
                    Email = new Models.Email
                    {
                        Description = entityProspect.EmailAddress
                    }
                });
            }

            return companyNames;
        }
        
        /// <summary>
        /// Obtener Direcciones de Notificación del Individuo
        /// </summary>
        /// <param name="individualId">Id Individuo</param>
        /// <param name="customerType">Type of the customer.</param>
        /// <returns>
        /// Direcciones de Notificación
        /// </returns>
        public List<Models.CompanyName> GetHolderNotificationAddressesByIndividualId(int individualId, CustomerType customerType)
        {
            ConcurrentBag<Models.CompanyName> companyNames = new ConcurrentBag<Models.CompanyName>();

            if (customerType == CustomerType.Individual)
            {
                AddressDAO addressDAO = new AddressDAO();
                var addresses = TP.Task.Run(() =>
                {
                    var result = addressDAO.GetAddresses(individualId)?.OrderByDescending(x => x.IsPrincipal).ToList();
                    DataFacadeManager.Dispose();
                    return result;
                }
                );


                PhoneDAO phoneDAO = new PhoneDAO();
                var phones = TP.Task.Run(() =>
                {
                    var result = phoneDAO.GetPhonesByIndividualId(individualId)?.OrderByDescending(x => x.IsMain).ToList();
                    DataFacadeManager.Dispose();
                    return result;
                }
                );

                EmailDAO emailDAO = new EmailDAO();
                var emails = TP.Task.Run(() =>
                {
                    var result = emailDAO.GetEmailsByIndividualId(individualId)?.OrderByDescending(x => x.IsPrincipal).ToList();
                    DataFacadeManager.Dispose();
                    return result;
                }
                );
                addresses.Wait();
                phones.Wait();
                emails.Wait();
                object obj = new object();
                if (addresses.Result != null && addresses.Result.Count > 0)
                {
                    TP.Parallel.For(0, 1, addressRow =>
                    {
                        var companyName = new Models.CompanyName
                        {
                            Address = addresses.Result[addressRow]
                        };
                        if (phones.Result != null && phones.Result.Count > 0)
                        {
                            companyName.Phone = phones.Result[0];
                            lock (obj)
                            {
                                phones.Result.RemoveAt(0);
                            }
                        }
                        if (emails.Result.Count > 0)
                        {
                            companyName.Email = emails.Result[0];
                            lock (obj)
                            {
                                emails.Result.RemoveAt(0);
                            }
                        }
                        companyNames.Add(companyName);
                    });

                    companyNames.ToList()[0].NameNum = 1;
                    companyNames.ToList()[0].IsMain = true;
                    companyNames.ToList()[0].TradeName = Errors.PrincipalAddress;
                }
            }
            else
            {
                PrimaryKey primaryKey = UniquePersonV1.Entities.Prospect.CreatePrimaryKey(individualId);
                UniquePersonV1.Entities.Prospect entityProspect = null;
                using (var daf = DataFacadeManager.Instance.GetDataFacade())
                {
                    entityProspect = (UniquePersonV1.Entities.Prospect)daf.GetObjectByPrimaryKey(primaryKey);
                }
                if (entityProspect != null)
                {
                    companyNames.Add(new Models.CompanyName
                    {
                        Address = new Models.Address
                        {
                            Description = entityProspect.Street
                        },
                        Phone = new Models.Phone
                        {
                            Description = entityProspect.PhoneNumber.HasValue ? entityProspect.PhoneNumber.ToString() : ""
                        },
                        Email = new Models.Email
                        {
                            Description = entityProspect.EmailAddress
                        }
                    });
                }
            }

            return companyNames.ToList();
        }
    }
}
