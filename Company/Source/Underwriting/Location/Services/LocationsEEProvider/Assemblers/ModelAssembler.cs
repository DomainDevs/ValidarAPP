using Sistran.Core.Framework.DAF;
using System.Collections.Generic;
using LOMO = Sistran.Core.Application.Locations.Models;
using UniquePersonEntities = Sistran.Core.Application.UniquePerson.Entities;
using System.Linq;

namespace Sistran.Company.Application.Locations.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region Sufix

        public static LOMO.Suffix CreateSuffix(UniquePersonEntities.StreetType sufix)
        {
            var mapper = AutoMapperAssembler.CreateMapSuffix();
            return mapper.Map<UniquePersonEntities.StreetType, LOMO.Suffix>(sufix);

            //return new LOMO.Suffix
            //{
            //    Id = sufix.StreetTypeCode,
            //    Description = sufix.SmallDescription
            //};
        }

        public static List<LOMO.Suffix> CreateSuffixes(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapSuffix();
            return mapper.Map<List<UniquePersonEntities.StreetType>, List<LOMO.Suffix>>(businessCollection.Cast<UniquePersonEntities.StreetType>().ToList());

            //List<LOMO.Suffix> sufixs = new List<LOMO.Suffix>();

            //foreach (UniquePersonEntities.StreetType field in businessCollection)
            //{
            //    sufixs.Add(ModelAssembler.CreateSuffix(field));
            //}

            //return sufixs;
        }

        #endregion

        #region AparmentOrOffice

        public static LOMO.ApartmentOrOffice CreateApartmentOrOffice(UniquePersonEntities.StreetType sufix)
        {
            var mapper = AutoMapperAssembler.CreateMapApartmentOrOffice();
            return mapper.Map<UniquePersonEntities.StreetType, LOMO.ApartmentOrOffice>(sufix);

            //return new LOMO.ApartmentOrOffice
            //{
            //    Id = sufix.StreetTypeCode,
            //    Description = sufix.SmallDescription
            //};
        }

        public static List<LOMO.ApartmentOrOffice> CreateApartmentsOrOffices(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapApartmentOrOffice();
            return mapper.Map<List<UniquePersonEntities.StreetType>, List<LOMO.ApartmentOrOffice>>(businessCollection.Cast<UniquePersonEntities.StreetType>().ToList());

            //List<LOMO.ApartmentOrOffice> aparmentOrOffices = new List<LOMO.ApartmentOrOffice>();

            //foreach (UniquePersonEntities.StreetType field in businessCollection)
            //{
            //    aparmentOrOffices.Add(ModelAssembler.CreateApartmentOrOffice(field));
            //}

            //return aparmentOrOffices;
        }

        #endregion

        #region StreetType

        public static LOMO.RouteType CreateRouteType(UniquePersonEntities.StreetType sufix)
        {
            var mapper = AutoMapperAssembler.CreateMapRouteType();
            return mapper.Map<UniquePersonEntities.StreetType, LOMO.RouteType>(sufix);

            //return new LOMO.RouteType
            //{
            //    Id = sufix.StreetTypeCode,
            //    Description = sufix.SmallDescription
            //};
        }

        public static List<LOMO.RouteType> CreateRouteTypes(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapRouteType();
            return mapper.Map<List<UniquePersonEntities.StreetType>, List<LOMO.RouteType>>(businessCollection.Cast<UniquePersonEntities.StreetType>().ToList());

            //List<LOMO.RouteType> sufixs = new List<LOMO.RouteType>();

            //foreach (UniquePersonEntities.StreetType field in businessCollection)
            //{
            //    sufixs.Add(ModelAssembler.CreateRouteType(field));
            //}

            //return sufixs;
        }

        #endregion



    }
}

