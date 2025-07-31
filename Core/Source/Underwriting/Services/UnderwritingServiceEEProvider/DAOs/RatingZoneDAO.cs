
using Sistran.Core.Application.UnderwritingServices.EEProvider.Assemblers;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Application.Utilities.DataFacade;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Framework.Queries;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using COMMEN = Sistran.Core.Application.Common.Entities;

namespace Sistran.Core.Application.UnderwritingServices.EEProvider.DAOs
{
    public class RatingZoneDAO
    {
        /// <summary>
        /// Finds the specified rating zone code.
        /// </summary>
        /// <param name="ratingZoneCode">The rating zone code.</param>
        /// <returns></returns>
        public static COMMEN.RatingZone Find(int ratingZoneCode)
        {
            PrimaryKey key = COMMEN.RatingZone.CreatePrimaryKey(ratingZoneCode);
            return (COMMEN.RatingZone)DataFacadeManager.Instance.GetDataFacade().GetObjectByPrimaryKey(key);
        }

        /// <summary>
        /// Finds all rating zone code.
        /// </summary>
        /// <returns></returns>
        public List<RatingZone> GetRatingZones()
        {
            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.RatingZone)));
            return ModelAssembler.CreateRatingZones(businessCollection);
        }

        /// <summary>
        /// Obtener lista de zonas de tarifación por ramo comercial
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <returns></returns>
        public List<RatingZone> GetRatingZonesByPrefixId(int prefixId)
        {
            BusinessCollection businessCollection = null;
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.RatingZone.Properties.PrefixCode, typeof(COMMEN.RatingZone).Name);
            filter.Equal();
            filter.Constant(prefixId);
            using (var daf = DataFacadeManager.Instance.GetDataFacade())
            {
                businessCollection = new BusinessCollection(daf.SelectObjects(typeof(COMMEN.RatingZone), filter.GetPredicate()));
            }
            
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetRatingZonesByPrefixId");
            return ModelAssembler.CreateRatingZones(businessCollection);
        }

        /// <summary>
        /// Finds the specified rating zone code.
        /// </summary>
        /// <param name="ratingZoneCode">The rating zone code.</param>
        /// <returns></returns>
        public RatingZone RatingZoneByRatingZoneCode(int ratingZoneCode)
        {
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.RatingZone.Properties.RatingZoneCode, typeof(COMMEN.RatingZone).Name);
            filter.Equal();
            filter.Constant(ratingZoneCode);

            BusinessCollection businessCollection = new BusinessCollection(DataFacadeManager.Instance.GetDataFacade().SelectObjects(typeof(COMMEN.RatingZone), filter.GetPredicate()));
            return ModelAssembler.CreateRatingZones(businessCollection).FirstOrDefault();

        }


        /// <summary>
        /// Obtener entidad zona de tarifacion por pais
        /// </summary>
        /// <param name="prefixId">Id ramo comercial</param>
        /// <param name="countryId">Id de pais</param>
        /// <param name="stateId">Id departamento</param>
        /// <returns></returns>
        public RatingZone GetRatingZonesByPrefixCodeCountryCodeStateCode(int prefixId, int countryId, int stateId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            ObjectCriteriaBuilder filter = new ObjectCriteriaBuilder();
            filter.Property(COMMEN.CountryRatingZone.Properties.PrefixCode, typeof(COMMEN.CountryRatingZone).Name);
            filter.Equal();
            filter.Constant(prefixId);
            filter.And();
            filter.Property(COMMEN.CountryRatingZone.Properties.CountryCode, typeof(COMMEN.CountryRatingZone).Name);
            filter.Equal();
            filter.Constant(countryId);
            filter.And();
            filter.Property(COMMEN.CountryRatingZone.Properties.StateCode, typeof(COMMEN.CountryRatingZone).Name);
            filter.Equal();
            filter.Constant(stateId);
            COMMEN.CountryRatingZone countryRatingZone = (COMMEN.CountryRatingZone)DataFacadeManager.Instance.GetDataFacade().List(typeof(COMMEN.CountryRatingZone), filter.GetPredicate()).FirstOrDefault();
            if (countryRatingZone != null)
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetRatingZonesByPrefixCodeCountryCodeStateCode");
                return RatingZoneByRatingZoneCode(countryRatingZone.RatingZoneCode);
            }
            else
            {
                stopWatch.Stop();
                Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Core.Application.CommonService.Providers.GetRatingZonesByPrefixCodeCountryCodeStateCode");
                return new RatingZone();
            }
        }
    }
}
