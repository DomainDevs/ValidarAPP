using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.CommonServices.EEProvider.DAOs
{
    public static class CountryRatingZoneDAO
    {
        public static IList List(Predicate filter, string[] sort)
        {
            IList list = (IList)DataFacadeManager.Instance.GetDataFacade().List(typeof(COMMEN.CountryRatingZone), filter, sort);
            return list;
        }

        /// <summary>
        /// Recuperación de objetos de la entidad <see cref="Sistran.Core.Application.Common.Entities.CountryRatingZone">CountryRatingZone</see>.
        /// </summary>
        /// <param name="prefixCode">The prefix code.</param>
        /// <param name="countryCode">The country code.</param>
        /// <param name="stateCode">The state code.</param>
        /// <param name="cityCode">The city code.</param>
        /// <returns></returns>
        /// <exception cref="Sistran.Core.Framework.BAF.BusinessException">NO_RATING_ZONE_FOR_COUNTRY_PREFIX</exception>
        public static IList ListByPrefixCodeCountryCodeStateCode(int prefixCode, int? countryCode, int? stateCode, int? cityCode)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Predicate(StateFilter(prefixCode, countryCode, stateCode));

            filter.And();
            filter.Property(COMMEN.CountryRatingZone.Properties.CityCode);

            if (cityCode != null)
            {
                filter.Equal();
                filter.Constant(cityCode);
            }
            else
            {
                filter.IsNull();
            }

            BusinessCollection<COMMEN.CountryRatingZone> countyRatingZoneList = (BusinessCollection<COMMEN.CountryRatingZone>)DataFacadeManager.Instance.GetDataFacade().List<COMMEN.CountryRatingZone>(filter.GetPredicate());

            if (countyRatingZoneList.Count == 0)
            {
                filter.Clear();
                filter.Predicate(StateFilter(prefixCode, countryCode, stateCode));

                filter.And();
                filter.Property(COMMEN.CountryRatingZone.Properties.CityCode);
                filter.IsNull();

                countyRatingZoneList = (BusinessCollection<COMMEN.CountryRatingZone>)DataFacadeManager.Instance.GetDataFacade().List<COMMEN.CountryRatingZone>(filter.GetPredicate());

                if (countyRatingZoneList.Count == 0)
                {
                    filter.Clear();
                    filter.Predicate(CountryPrefixFilter(prefixCode, countryCode));

                    filter.And();
                    filter.Property(COMMEN.CountryRatingZone.Properties.StateCode);
                    filter.IsNull();

                    filter.And();
                    filter.Property(COMMEN.CountryRatingZone.Properties.CityCode);
                    filter.IsNull();

                    countyRatingZoneList = (BusinessCollection<COMMEN.CountryRatingZone>)DataFacadeManager.Instance.GetDataFacade().List<COMMEN.CountryRatingZone>(filter.GetPredicate());

                    if (countyRatingZoneList.Count == 0)
                    {
                        throw new BusinessException("NO_RATING_ZONE_FOR_COUNTRY_PREFIX", new string[] { countryCode.ToString(), prefixCode.ToString() });
                    }

                }
            }

            return (IList)countyRatingZoneList;

        }

        /// <summary>
        /// Filtrar por Ramo Comercial y Pais
        /// </summary>
        /// <param name="prefixCode">The prefix code.</param>
        /// <param name="countryCode">The country code.</param>
        /// <returns></returns>
        private static Predicate CountryPrefixFilter(int prefixCode, int? countryCode)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();

            filter.Property(COMMEN.CountryRatingZone.Properties.PrefixCode);
            filter.Equal();
            filter.Constant(prefixCode);

            filter.And();
            filter.Property(COMMEN.CountryRatingZone.Properties.CountryCode);
            filter.Equal();
            filter.Constant(countryCode);

            return filter.GetPredicate();
        }

        /// <summary>
        /// Filtrar por Ramo Comercial, Pais y Provincia (Estado)
        /// </summary>
        /// <param name="stateCode">The state code.</param>
        /// <param name="prefixCode">The prefix code.</param>
        /// <param name="countryCode">The country code.</param>
        /// <returns></returns>
        private static Predicate StateFilter(int prefixCode, int? countryCode, int? stateCode)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Predicate(CountryPrefixFilter(prefixCode, countryCode));

            filter.And();
            filter.Property(COMMEN.CountryRatingZone.Properties.StateCode);

            if (stateCode != null)
            {
                filter.Equal();
                filter.Constant(stateCode);
            }
            else
            {
                filter.IsNull();
            }

            return filter.GetPredicate();
        }
    }
}
