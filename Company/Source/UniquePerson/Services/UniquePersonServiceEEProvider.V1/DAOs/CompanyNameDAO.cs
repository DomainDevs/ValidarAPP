using Sistran.Core.Application.UniquePersonService.V1.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using Sistran.Core.Services.UtilitiesServices.Enums;
using System.Collections.Generic;
using System.Linq;
using UPDAO = Sistran.Core.Application.UniquePersonService.V1.DAOs;
using UPEN = Sistran.Core.Application.UniquePersonV1.Entities;
using UPMO = Sistran.Core.Application.UniquePersonService.V1.Models;
using UPPCORE = Sistran.Core.Application.UniquePersonService.V1;
namespace Sistran.Company.Application.UniquePersonServices.V1.EEProvider.DAOs
{
    public class CompanyNameDAO
    {
        /// <summary>
        /// Obtener Direcciones de Notificación del Individuo Company
        /// </summary>
        /// <param name="individualId">Id Individuo</param>
        /// <returns>Direcciones de Notificación</returns>
        public List<UPMO.CompanyName> CompanyGetNotificationAddressesByIndividualId(int individualId, CustomerType customerType)
        {
            List<UPMO.CompanyName> companyNames = new List<UPMO.CompanyName>();

            if (customerType == CustomerType.Individual)
            {
                ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
                filter.Property(UPEN.CoCompanyName.Properties.IndividualId, typeof(UPEN.CoCompanyName).Name);
                filter.Equal();
                filter.Constant(individualId);

                BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(UPEN.CoCompanyName), filter.GetPredicate()));

                UPDAO.AddressDAO addressDAO = new UPDAO.AddressDAO();
                List<UPMO.Address> addresses = addressDAO.GetAddresses(individualId);
                addresses = addresses.OrderByDescending(x => x.IsPrincipal).ToList();

                UPDAO.PhoneDAO phoneDAO = new UPDAO.PhoneDAO();
                List<UPMO.Phone> phones = phoneDAO.GetPhonesByIndividualId(individualId);
                phones = phones.OrderByDescending(x => x.IsMain).ToList();

                UPDAO.EmailDAO emailDAO = new UPDAO.EmailDAO();
                List<UPMO.Email> emails = emailDAO.GetEmailsByIndividualId(individualId);
                emails = emails.OrderByDescending(x => x.IsPrincipal).ToList();

                if (businessCollection.Count > 0)
                {
                    companyNames = UPPCORE.Assemblers.ModelAssembler.CreateCompaniesName(businessCollection);

                    foreach (UPMO.CompanyName item in companyNames)
                    {
                        if (item.Address != null && item.Address.Id > 0)
                        {
                            item.Address = addresses.FirstOrDefault(x => x.Id == item.Address.Id);
                        }
                        else
                        {
                            item.Address = addresses.FirstOrDefault();
                        }
                        if (item.Phone != null && item.Phone.Id > 0)
                        {
                            item.Phone = phones.FirstOrDefault(x => x.Id == item.Phone.Id);
                        }
                        else
                        {
                            item.Phone = phones.FirstOrDefault();
                        }

                        if (item.Email != null && item.Email.Id > 0)
                        {
                            item.Email = emails.FirstOrDefault(x => x.Id == item.Email.Id);
                        }
                        else
                        {
                            item.Email = emails.FirstOrDefault();
                        }
                    }
                }
                else if (addresses.Count > 0)
                {
                    foreach (UPMO.Address address in addresses)
                    {
                        UPMO.CompanyName companyName = new UPMO.CompanyName
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
                    UPDAO.CompanyNameDAO companyNameDao = new UPDAO.CompanyNameDAO();
                    companyNameDao.CreateCompaniesName(companyNames[0], individualId);
                }
            }
            else
            {
                PrimaryKey primaryKey = UPEN.Prospect.CreatePrimaryKey(individualId);
                UPEN.Prospect entityProspect = (UPEN.Prospect)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(primaryKey);

                companyNames.Add(new UPMO.CompanyName
                {
                    Address = new UPMO.Address
                    {
                        Description = entityProspect.Street
                    },
                    Phone = new UPMO.Phone
                    {
                        Description = entityProspect.PhoneNumber.HasValue ? entityProspect.PhoneNumber.ToString() : ""
                    },
                    Email = new UPMO.Email
                    {
                        Description = entityProspect.EmailAddress
                    }
                });
            }

            return companyNames;
        }
    }
}
