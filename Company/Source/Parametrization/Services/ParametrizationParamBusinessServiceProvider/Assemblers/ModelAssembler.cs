// -----------------------------------------------------------------------
// <copyright file="ModelAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jaime Trujillo</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.ParametrizationParamBusinessServiceProvider.Assemblers
{
    using Sistran.Company.Application.ParametrizationParamBusinessService.Model;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using COMMEN = Sistran.Core.Application.Parameters.Entities;
    using ModelEntities = Sistran.Company.Application.Common.Entities;
    using Sistran.Core.Framework.DAF;
    using Sistran.Company.Application.Common.Entities;
    using Sistran.Core.Application.Common.Entities;
    using TP = Sistran.Core.Application.Utilities.Utility;

    /// <summary>
    /// convierte de Entidad a modelo Company
    /// </summary>
    public class ModelAssembler
    {

        #region BillingPeriod

        /// <summary>
        /// CreateBillingPeriod. mapaea de entity a modelo de negocio company.
        /// </summary>
        /// <param name="entityBillingPeriod"></param>
        /// <returns>CompanyParamBillingPeriod</returns>
        public static CompanyParamBillingPeriod CreateBillingPeriod(COMMEN.BillingPeriod entityBillingPeriod)
        {
            return new CompanyParamBillingPeriod
            {
                BILLING_PERIOD_CD = entityBillingPeriod.BillingPeriodCode,
                Description = entityBillingPeriod.Description

            };
        }

        /// <summary>
        /// CreateBillingPeriods. mapea de un BusinessCollection a una lista de modelos de Negocio company.
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<CompanyParamBillingPeriod></returns>
        public static List<CompanyParamBillingPeriod> CreateBillingPeriods(BusinessCollection businessCollection)
        {
            var sync = new object();
            List<CompanyParamBillingPeriod> BillingPeriod = new List<CompanyParamBillingPeriod>();
            TP.Parallel.ForEach(businessCollection, (entity) =>
            {
                lock (sync)
                {
                    BillingPeriod.Add(ModelAssembler.CreateBillingPeriod((COMMEN.BillingPeriod)entity));
                }
            }
            );

            return BillingPeriod;
        }

        #endregion

        #region BusinessType

        /// <summary>
        /// CreateBusinessType. mapaea de entity a modelo de negocio company.
        /// </summary>
        /// <param name="entityBusinessType"></param>
        /// <returns>CompanyParamBusinessType</returns>
        public static CompanyParamBusinessType CreateBusinessType(COMMEN.BusinessType entityBusinessType)
        {
            return new CompanyParamBusinessType
            {
                BUSINESS_TYPE_CD = entityBusinessType.BusinessTypeCode,
                SMALL_DESCRIPTION = entityBusinessType.SmallDescription

            };
        }

        /// <summary>
        /// CreateBusinessTypes. mapea de un BusinessCollection a una lista de modelos de Negocio company.
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>List<CompanyParamBusinessType></returns>
        public static List<CompanyParamBusinessType> CreateBusinessTypes(BusinessCollection businessCollection)
        {
            var sync = new object();
            List<CompanyParamBusinessType> BusinessType = new List<CompanyParamBusinessType>();
            TP.Parallel.ForEach(businessCollection, (entity) =>
            {
                lock (sync)
                {
                    BusinessType.Add(ModelAssembler.CreateBusinessType((COMMEN.BusinessType)entity));
                }
            }
            );

            return BusinessType;
        }

        #endregion

        #region City

        /// <summary>
        /// MappParamCity mpaea de entity a mdelo de negocio company, recibe los datos de las tablas de state y country
        /// </summary>
        /// <param name="city"></param>
        /// <param name="country"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static CompanyParamCity MappParamCity(ModelEntities.City city, ModelEntities.Country country, ModelEntities.State state)
        {
            CompanyParamCity paramCity = new CompanyParamCity
            {
                Id = city.CityCode,
                Description = city.Description,
                SmallDescription = city.SmallDescription,
                Country = new CompanyParamCountry { Id = country.CountryCode, Description = country.Description },
                State = new CompanyParamState { Id = state.StateCode, Description = state.Description }

            };
            return paramCity;
        }

        /// <summary>
        /// MappParamCities mapea de entity city a modelo de negocio company
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        public static CompanyParamCity MappParamCities(ModelEntities.City city)
        {
            CompanyParamCity paramCity = new CompanyParamCity
            {
                Id = city.CityCode,
                Description = city.Description,
                SmallDescription = city.SmallDescription,
                Country = new CompanyParamCountry { Id = city.CountryCode },
                State = new CompanyParamState { Id = city.StateCode }
            };

            return paramCity;
        }

        /// <summary>
        /// ConvertToModelCities mapea de un list a una lista de modelos de egocio company de city
        /// </summary>
        /// <param name="cityList"></param>
        /// <returns></returns>
        public static List<CompanyParamCity> ConvertToModelCities(System.Collections.IList cityList)
        {
            List<CompanyParamCity> cities = new List<CompanyParamCity>();
            foreach (ModelEntities.City city in cityList)
            {
                cities.Add(MappParamCities(city));
            }
            return cities;
        }

        #endregion
        #region Parameter


        public static CompanyParameters CreateParameter(Parameter entityParameter)
        {
            return new CompanyParameters
            {
                Id = entityParameter.ParameterId,
                Description = entityParameter.Description,
                BoolParameter = entityParameter.BoolParameter,
                NumberParameter = entityParameter.NumberParameter,
                DateParameter = entityParameter.DateParameter,
                TextParameter = entityParameter.TextParameter,
                PercentageParameter = entityParameter.PercentageParameter,
                AmountParameter = entityParameter.AmountParameter
            };
      }

            #endregion

        }
}


