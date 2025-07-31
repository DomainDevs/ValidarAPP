using Sistran.Company.Application.UniquePersonServices.V1.EEProvider.Assemblers;
using Sistran.Company.Application.UniquePersonServices.V1.Models;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.UniquePersonService.V1.DAOs;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using entities = Sistran.Core.Application.UniquePersonV1.Entities;

namespace Sistran.Company.Application.UniquePersonServices.V1.DAOs
{
    public class CoInsurerDAO : Sistran.Core.Application.UniquePersonService.V1.DAOs.CoInsurerDAO
    {
        public CoInsurerCompany GetCoInsurerByIndividualId(int individualId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (individualId != 0)
            {
                filter.Property(CoInsuranceCompany.Properties.IndividualId);
                filter.Equal();
                filter.Constant(individualId);
            }
            BusinessCollection<CoInsuranceCompany> businessCollection = new BusinessCollection<CoInsuranceCompany>();
            businessCollection = DataFacadeManager.Instance.GetDataFacade().List<CoInsuranceCompany>(filter.GetPredicate());
            CoInsurerCompany coInsurerCompanyModel = null;
            if (businessCollection != null)
            {
                List<CoInsurerCompany> lstCoInsurerCompanyModel = ModelAssembler.CreateCoInsurer(businessCollection);
                if (lstCoInsurerCompanyModel != null)
                {
                    coInsurerCompanyModel = lstCoInsurerCompanyModel[0];
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.UniquePersonServices.V1.DAOs.GetCoInsurerByIndividualId");

            return coInsurerCompanyModel;
        }
        public CoInsuranceCompany GetCoInsurerEntityByIndividualId(int individualId)
        {

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (individualId != 0)
            {
                filter.Property(CoInsuranceCompany.Properties.IndividualId);
                filter.Equal();
                filter.Constant(individualId);
            }
            BusinessCollection<CoInsuranceCompany> businessCollection = new BusinessCollection<CoInsuranceCompany>();
            businessCollection = DataFacadeManager.Instance.GetDataFacade().List<CoInsuranceCompany>(filter.GetPredicate());


            if (businessCollection.Count > 0)
            {
                return businessCollection[0];
            }
            else
            {
                return null;
            }
        }

        public CoInsurerCompany GetCoInsurerByTributaryNo(string tributaryNo)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (tributaryNo != null && tributaryNo != String.Empty)
            {
                filter.Property(CoInsuranceCompany.Properties.TributaryIdNo);
                filter.Like();
                filter.Constant(tributaryNo + "%");
            }
            BusinessCollection<CoInsuranceCompany> businessCollection = new BusinessCollection<CoInsuranceCompany>();
            businessCollection = DataFacadeManager.Instance.GetDataFacade().List<CoInsuranceCompany>(filter.GetPredicate());
            CoInsurerCompany coInsurerCompanyModel = null;
            if (businessCollection != null)
            {
                List<CoInsurerCompany> lstCoInsurerCompanyModel = ModelAssembler.CreateCoInsurer(businessCollection);
                if (lstCoInsurerCompanyModel != null)
                {
                    coInsurerCompanyModel = lstCoInsurerCompanyModel[0];
                }
            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.UniquePersonServices.V1.DAOs.GetCoInsurerByTributaryNo");

            return coInsurerCompanyModel;
        }

        public CoInsurerCompany CreateInsurerByExistingCompany(CoInsurerCompany coinsurer)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            if (coinsurer.IndividualId != 0)
            {
                filter.Property(CoInsuranceCompany.Properties.IndividualId);
                filter.Equal();
                filter.Constant(coinsurer.IndividualId);
            }
            BusinessCollection<entities.Company> existingCompany = DataFacadeManager.Instance.GetDataFacade().List<entities.Company>(filter.GetPredicate());
            if (existingCompany != null)
            {
                AddressDAO addressDAO = new AddressDAO();
                var addresses = addressDAO.GetAddresses(coinsurer.IndividualId).First();

                PhoneDAO phoneDAO = new PhoneDAO();
                var phones = phoneDAO.GetPhonesByIndividualId(coinsurer.IndividualId).Find(p => p.IsMain == true);

                ModelAssembler.CopyCompanyToCoInsurer(existingCompany[0], coinsurer, addresses, phones);

                CoInsuranceCompany coinsurerAux = GetCoInsurerEntityByIndividualId(coinsurer.IndividualId);



                if (coinsurerAux == null)
                {


                    coinsurerAux = new CoInsuranceCompany();
                    EntityAssembler.CreateCoInsurer(coinsurerAux, coinsurer);
                    coinsurerAux.InsuranceCompanyId = GetIdCoInsurer();
                    DataFacadeManager.Instance.GetDataFacade().InsertObject(coinsurerAux);
                }
                else
                {
                    //  Crear
                    EntityAssembler.CreateCoInsurer(coinsurerAux, coinsurer);
                    DataFacadeManager.Instance.GetDataFacade().UpdateObject(coinsurerAux);

                }
                CoInsurerCompany coInsurerCompanyModel = null;
                coInsurerCompanyModel = ModelAssembler.CreateCoInsurer(coinsurerAux);


                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Application.UniquePersonServices.V1.DAOs.CreateInsurerByExistingCompany");

                return coInsurerCompanyModel;
            }
            else
            {
                return null;
            }


        }

        public int GetIdCoInsurer()
        {
            int maxCoInsurer = (int)new BusinessCollection(DataFacadeManager.Instance.GetDataFacade()
                            .SelectObjects(typeof(CoInsuranceCompany)))
                            .Cast<CoInsuranceCompany>().Max(x => x.InsuranceCompanyId);
            maxCoInsurer++;
            return maxCoInsurer;
        }

    }
}