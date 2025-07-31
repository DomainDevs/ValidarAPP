using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.UniquePersonService.Assemblers;
using Sistran.Core.Application.UniquePersonService.Enums;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using models = Sistran.Core.Application.UniquePersonService.Models;

namespace Sistran.Core.Application.UniquePersonService.DAOs
{

    /// <summary>
    /// Clase CRUD prospectos
    /// </summary>
    public class ProspectNaturalDAO
    {

        /// <summary>
        /// Creates the prospect natural.
        /// </summary>
        /// <param name="prospectNatural">The prospect natural.</param>
        /// <returns></returns>
        public models.ProspectNatural CreateProspectNatural(models.ProspectNatural prospectNatural)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            //Se agrega directamente a este metodo, debido a q' no se puede alterar la tabla por data existente
            List<models.ProspectNatural> prospectsExist = GetProspectByDocumentNum(prospectNatural.IdCardNo, (int)IndividualSearchType.ProspectusPerson);
            if (prospectsExist.Count > 0)
            {
                return prospectNatural;
            }
            Prospect prospectNaturalentity = EntityAssembler.CreateProspectNatural(prospectNatural);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(prospectNaturalentity);
            if (prospectNaturalentity?.ProspectId != null)
            {
                CoProspect CoprospectNaturalentity = EntityAssembler.CreateCoProspect(prospectNaturalentity.ProspectId);
                DataFacadeManager.Instance.GetDataFacade().InsertObject(CoprospectNaturalentity);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.CreateProspectNatural");
            return ModelAssembler.CreateModelProspect(prospectNaturalentity);
        }

        /// <summary>
        /// Updates the prospect natural.
        /// </summary>
        /// <param name="prospectNatural">The prospect natural.</param>
        /// <returns></returns>
        public Models.ProspectNatural UpdateProspectNatural(Models.ProspectNatural prospectNatural)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Prospect.Properties.IdCardNo, typeof(Prospect).Name);
            filter.Equal();
            filter.Constant(prospectNatural.IdCardNo);
            Prospect prspectNaturalEntiti = (Prospect)DataFacadeManager.Instance.GetDataFacade().List(typeof(Prospect), filter.GetPredicate()).FirstOrDefault();
            prspectNaturalEntiti.IndividualTypeCode = prospectNatural.IndividualTyepCode;
            prspectNaturalEntiti.CountryCode = prospectNatural.CountryCode;
            prspectNaturalEntiti.StateCode = prospectNatural.StateCode;
            prspectNaturalEntiti.Surname = prospectNatural.Surname;
            prspectNaturalEntiti.Name = prospectNatural.Name;
            prspectNaturalEntiti.Gender = prospectNatural.Gender;
            prspectNaturalEntiti.MaritalStatusCode = prospectNatural.MaritalStatus;
            prspectNaturalEntiti.BirthDate = prospectNatural.BirthDate;
            prspectNaturalEntiti.CityCode = prospectNatural.CityCode;
            prspectNaturalEntiti.IdCardTypeCode = prospectNatural.IdCardTypeCode;
            prspectNaturalEntiti.IdCardNo = prospectNatural.IdCardNo;
            prspectNaturalEntiti.AddressTypeCode = prospectNatural.AddressType;
            prspectNaturalEntiti.EmailAddress = prospectNatural.EmailAddress;
            prspectNaturalEntiti.PhoneNumber = prospectNatural.PhoneNumber;
            prspectNaturalEntiti.Street = prospectNatural.Street;
            prspectNaturalEntiti.AdditionalInfo = prospectNatural.AdditionalInfo;

            DataFacadeManager.Instance.GetDataFacade().UpdateObject(prspectNaturalEntiti);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.UpdateProspectNatural");
            return ModelAssembler.CreateModelProspect(prspectNaturalEntiti);
        }

        /// <summary>
        /// Creates the prospect legal.
        /// </summary>
        /// <param name="prospectLegal">The prospect legal.</param>
        /// <returns></returns>
        public models.ProspectNatural CreateProspectLegal(models.ProspectNatural prospectLegal)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<models.ProspectNatural> prospectsExist = GetProspectByDocumentNum(prospectLegal.TributaryIdNumber, (int)IndividualSearchType.ProspectusCompany);
            if (prospectsExist.Count > 0)
            {
                return prospectLegal;
            }
            Prospect prospectNaturalentity = EntityAssembler.CreateProspectLegal(prospectLegal);
            DataFacadeManager.Instance.GetDataFacade().InsertObject(prospectNaturalentity);

            if (prospectNaturalentity?.ProspectId != null)
            {
                CoProspect CoprospectNaturalentity = EntityAssembler.CreateCoProspect(prospectNaturalentity.ProspectId, CalculateDigitVerify(prospectNaturalentity.TributaryIdNo.Trim()));
                DataFacadeManager.Instance.GetDataFacade().InsertObject(CoprospectNaturalentity);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.CreateProspectLegal");
            return ModelAssembler.CreateModelProspect(prospectNaturalentity);

        }

        public int CalculateDigitVerify(string documentNumber) 
        {
            int [] vpri =  new int[16];
            int  x, y, z, i,  dv1;
            string nit1 = documentNumber;

            try
            {
                z = documentNumber.Length;

                if (nit1.Length>0)
                {

                    x = 0; y = 0; z = documentNumber.Length;
                    vpri[1] = 3;
                    vpri[2] = 7;
                    vpri[3] = 13;
                    vpri[4] = 17;
                    vpri[5] = 19;
                    vpri[6] = 23;
                    vpri[7] = 29;
                    vpri[8] = 37;
                    vpri[9] = 41;
                    vpri[10] = 43;
                    vpri[11] = 47;
                    vpri[12] = 53;
                    vpri[13] = 59;
                    vpri[14] = 67;
                    vpri[15] = 71;
                    for (i = 0; i < z; i++)
                    {
                        y = (int.Parse(nit1.Substring(i, 1)));
                        x += (y * vpri[z - i]);
                    }

                    y = x % 11;

                    if (y > 1)
                    {
                        dv1 = 11 - y;
                    }
                    else
                    {
                        dv1 = y;
                    }
                    return (int)dv1;
                }
                return -1;
            }catch(System.Exception)
            {
                return -1;
            }
        }

        /// <summary>
        /// Updates the prospect legal.
        /// </summary>
        /// <param name="prospectLegal">The prospect legal.</param>
        /// <returns></returns>
        public Models.ProspectNatural UpdateProspectLegal(Models.ProspectNatural prospectLegal)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Prospect.Properties.ProspectId, typeof(Prospect).Name);
            filter.Equal();
            filter.Constant(prospectLegal.ProspectCode);

            Prospect prspectLegalEntiti = (Prospect)DataFacadeManager.Instance.GetDataFacade().List(typeof(Prospect), filter.GetPredicate()).FirstOrDefault();

            prspectLegalEntiti.IndividualTypeCode = prospectLegal.IndividualTyepCode;
            prspectLegalEntiti.TradeName = prospectLegal.Name;
            prspectLegalEntiti.TributaryIdTypeCode = prospectLegal.TributaryIdTypeCode;
            prspectLegalEntiti.TributaryIdNo = prospectLegal.TributaryIdNumber;
            prspectLegalEntiti.CityCode = prospectLegal.CityCode;
            prspectLegalEntiti.StateCode = prospectLegal.StateCode;
            prspectLegalEntiti.CountryCode = prospectLegal.CountryCode;
            prspectLegalEntiti.CompanyTypeCode = prospectLegal.CompanyTypeCode;
            prspectLegalEntiti.Street = prospectLegal.Street;
            prspectLegalEntiti.AddressTypeCode = prospectLegal.AddressType;
            prspectLegalEntiti.PhoneNumber = prospectLegal.PhoneNumber;
            prspectLegalEntiti.EmailAddress = prospectLegal.EmailAddress;
            prspectLegalEntiti.AdditionalInfo = prospectLegal.AdditionalInfo;
            DataFacadeManager.Instance.GetDataFacade().UpdateObject(prspectLegalEntiti);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.UpdateProspectLegal");
            return ModelAssembler.CreateModelProspect(prspectLegalEntiti);
        }

        /// <summary>
        /// Gets the prospect natural by individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public Models.ProspectNatural GetProspectNaturalByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Prospect.Properties.ProspectId, typeof(Prospect).Name);
            filter.Equal();
            filter.Constant(individualId);
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Prospect), filter.GetPredicate()));
            Models.ProspectNatural prospect = ModelAssembler.CreateNaturalProspects(businessCollection).FirstOrDefault();

            if (prospect != null && prospect.CityCode != null && prospect.CityCode != 0)
            {
                City city = new City();
                Country country = new Country();
                country.Id = prospect.CountryCode.Value;

                State state = new State();
                state.Id = prospect.StateCode.Value;
                state.Country = country;

                city.Id = prospect.CityCode.Value;
                city.State = state;

                prospect.City = DelegateService.commonServiceCore.GetCityByCity(city);

            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetProspectNaturalByIndividualId");
            return prospect;
        }

        /// <summary>
        /// Gets the prospect legal by individual identifier.
        /// </summary>
        /// <param name="individualId">The individual identifier.</param>
        /// <returns></returns>
        public Models.ProspectNatural GetProspectLegalByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(Prospect.Properties.ProspectId, typeof(Prospect).Name);
            filter.Equal();
            filter.Constant(individualId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Prospect), filter.GetPredicate()));
            Models.ProspectNatural prospect = ModelAssembler.CreateNaturalProspects(businessCollection).FirstOrDefault();

            if (prospect != null && prospect.CityCode != null && prospect.CityCode != 0)
            {

                City city = new City();
                Country country = new Country();
                country.Id = prospect.CountryCode.Value;

                State state = new State();
                state.Id = prospect.StateCode.Value;
                state.Country = country;

                city.Id = prospect.CityCode.Value;
                city.State = state;
                prospect.City = DelegateService.commonServiceCore.GetCityByCity(city);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetProspectLegalByIndividualId");
            return prospect;
        }

        /// <summary>
        /// Gets the name of all prospect by document number surname mother last name trade.
        /// </summary>
        /// <param name="documentNumber">The document number.</param>
        /// <param name="surname">The surname.</param>
        /// <param name="motherLastName">Last name of the mother.</param>
        /// <param name="name">The name.</param>
        /// <param name="tradeName">Name of the trade.</param>
        /// <param name="searchType">Type of the search.</param>
        /// <returns></returns>
        public List<Models.ProspectNatural> GetAllProspectByDocumentNumberSurnameMotherLastNameTradeName(string documentNumber, string surname, string motherLastName, string name, string tradeName, int searchType, int? documentType, int? individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            List<Models.ProspectNatural> prospects = new List<Models.ProspectNatural>();
            PersonDAO personDAO = new PersonDAO();
            CompanyDAO companyDAO = new CompanyDAO();
            List<Models.Person> persons = new List<models.Person>();
            List<Models.Company> companies = new List<models.Company>();
            if ((documentNumber != "" && documentNumber != null) || ((!string.IsNullOrEmpty(surname) || (!string.IsNullOrEmpty(motherLastName)) || (!string.IsNullOrEmpty(name)))))
            {
                persons = personDAO.GetPersonByDocumentNumberSurnameMotherLastName(documentNumber, surname, motherLastName, name, searchType, documentType, individualId);
            }
            if ((documentNumber != "" && documentNumber != null) || (!string.IsNullOrEmpty(tradeName)))
            {
                companies = companyDAO.GetCompaniesByDocumentNumberNameSearchType(documentNumber, tradeName, searchType, documentType, individualId);
            }
            prospects = ModelAssembler.CreateProspectsPersonCompany(persons, companies);

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetAllProspectByDocumentNumberSurnameMotherLastNameTradeName");
            return prospects;
        }


        /// <summary>
        /// Gets the prospect by document number.
        /// </summary>
        /// <param name="documentNum">The document number.</param>
        /// <param name="searchType">Type of the search.</param>
        /// <returns></returns>
        public List<Models.ProspectNatural> GetProspectByDocumentNum(string documentNum, int searchType)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (searchType == 3)
            {
                filter.Property(Prospect.Properties.IdCardNo, typeof(Prospect).Name);
                filter.Equal();
                filter.Constant(documentNum);

            }
            else
            {
                filter.Property(Prospect.Properties.TributaryIdNo, typeof(Prospect).Name);
                filter.Equal();
                filter.Constant(documentNum);

            }
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Prospect), filter.GetPredicate()));
            List<Models.ProspectNatural> prospects = new List<models.ProspectNatural>();
            if (businessCollection.Count > 0)
            {
                prospects = ModelAssembler.CreateNaturalProspects(businessCollection);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetProspectByDocumentNum");
            return prospects;
        }

        /// <summary>
        /// Gets the prospect by individual ID .
        /// </summary>
        /// <param name="individualId">The Individual Id.</param>        
        /// <returns></returns>
        public List<Models.ProspectNatural> GetProspectByIndividualId(string individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(Prospect.Properties.ProspectId, typeof(Prospect).Name);
            filter.Equal();
            filter.Constant(individualId);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(Prospect), filter.GetPredicate()));
            List<Models.ProspectNatural> prospects = new List<models.ProspectNatural>();
            if (businessCollection.Count > 0)
            {
                prospects = ModelAssembler.CreateNaturalProspects(businessCollection);
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.UniquePersonService.DAOs.GetProspectByIndividualId");
            return prospects;
        }
    }
}
