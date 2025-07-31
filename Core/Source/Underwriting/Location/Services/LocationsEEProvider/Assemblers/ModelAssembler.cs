using AutoMapper;
using Sistran.Core.Application.Common.Entities;
using Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Application.UniquePerson.Entities;
using Sistran.Core.Application.Utilities.Cache;
using Sistran.Core.Framework.DAF;
using System.Linq;
using System.Collections.Generic;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using UNDModel = Sistran.Core.Application.UnderwritingServices.Models;
using UNDTO = Sistran.Core.Application.UnderwritingServices.DTOs;

namespace Sistran.Core.Application.Locations.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region Sufix

         /// <summary>
        /// Creacion de modelo subfijos
        /// </summary>
        /// <param name="sufix">Entidad StreetType</param>
        /// <returns>Modelo subfijo</returns>
        public static Models.Suffix CreateSuffix(StreetType sufix)
        {
            var mapper = AutoMapperAssembler.CreateMapSuffix();
            return mapper.Map<StreetType, Models.Suffix>(sufix);
/*            return new Models.Suffix
            {
                Id = sufix.StreetTypeCode,
                Description = sufix.SmallDescription
            };*/
        }
        /// <summary>
        /// Creacion de lista de subfijos
        /// </summary>
        /// <param name="businessCollection"></param>
        /// <returns>Lista de subfijos</returns>
        public static List<Models.Suffix> CreateSuffixes(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapSuffix();
            return mapper.Map<List<StreetType>, List<Models.Suffix>>(businessCollection.Cast<StreetType>().ToList());

            //List<Models.Suffix> sufixs = new List<Models.Suffix>();

            //foreach (StreetType field in businessCollection)
            //{
            //    sufixs.Add(ModelAssembler.CreateSuffix(field));
            //}

            //return sufixs;
        }

        #endregion

        #region AparmentOrOffice
        /// <summary>
        /// Creacion de Apartamento/Oficina
        /// </summary>
        /// <param name="sufix">Entidad StreetType</param>
        /// <returns>Modelo Apartamento/Oficina</returns>
        public static Models.ApartmentOrOffice CreateApartmentOrOffice(StreetType sufix)
        {
            var mapper = AutoMapperAssembler.CreateMapApartmentsOrOffices();
            return mapper.Map<StreetType, Models.ApartmentOrOffice>(sufix);

            //return new Models.ApartmentOrOffice
            //{
            //    Id = sufix.StreetTypeCode,
            //    Description = sufix.SmallDescription
            //};
        }
        /// <summary>
        /// Creacion de Lista de Apartamento/Oficina
        /// </summary>
        /// <param name="businessCollection">coleccion de entidades</param>
        /// <returns>Modelo Apartamento/Oficina</returns>
        public static List<Models.ApartmentOrOffice> CreateApartmentsOrOffices(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapApartmentsOrOffices();
            return mapper.Map<List<StreetType>, List<Models.ApartmentOrOffice>>(businessCollection.Cast<StreetType>().ToList());

            //List<Models.ApartmentOrOffice> aparmentOrOffices = new List<Models.ApartmentOrOffice>();

            //foreach (StreetType field in businessCollection)
            //{
            //    aparmentOrOffices.Add(ModelAssembler.CreateApartmentOrOffice(field));
            //}

            //return aparmentOrOffices;
        }

        #endregion

        #region StreetType
        /// <summary>
        /// Crear tipo de rutas
        /// </summary>
        /// <param name="sufix">entidad StreetType</param>
        /// <returns>Modelo RouteType</returns>
        public static Models.RouteType CreateRouteType(StreetType sufix)
        {
            var mapper = AutoMapperAssembler.CreateMapRouteType();
            return mapper.Map<StreetType, Models.RouteType>(sufix);

            //return new Models.RouteType
            //{
            //    Id = sufix.StreetTypeCode,
            //    Description = sufix.SmallDescription,
            //    SimilarStreetTypeCd = sufix.SimilarStreetTypeCode
            //};
        }
        /// <summary>
        /// Crear lista de tipos de rutas
        /// </summary>
        /// <param name="businessCollection">coleccion de entidades</param>
        /// <returns>Lista de tipos de rutas</returns>
        public static List<Models.RouteType> CreateRouteTypes(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapRouteType();
            return mapper.Map<List<StreetType>, List<Models.RouteType>>(businessCollection.Cast<StreetType>().ToList());

            //List<Models.RouteType> sufixs = new List<Models.RouteType>();

            //foreach (StreetType field in businessCollection)
            //{
            //    sufixs.Add(ModelAssembler.CreateRouteType(field));
            //}

            //return sufixs;
        }

        #endregion

        #region ConstructionType
        /// <summary>
        /// Creacion de Tipo de construccion
        /// </summary>
        /// <param name="constructionCategory">Entidad ConstructionCategory</param>
        /// <returns>Modelo tipo de construccion</returns>
        public static Models.ConstructionType CreateConstructionType(ConstructionCategory constructionCategory)
        {
            var mapper = AutoMapperAssembler.CreateMapConstructionType();
            return mapper.Map<ConstructionCategory, Models.ConstructionType>(constructionCategory);

            //return new Models.ConstructionType
            //{
            //    Id = constructionCategory.ConstructionCategoryCode,
            //    Description = constructionCategory.SmallDescription
            //};
        }
        /// <summary>
        /// Creacion de lista de tipos de construccion
        /// </summary>
        /// <param name="businessCollection">coleccion de entidades</param>
        /// <returns>Lista de tipos de construccion</returns>
        public static List<Models.ConstructionType> CreateConstructionTypes(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapConstructionType();
            return mapper.Map<List<ConstructionCategory>, List<Models.ConstructionType>>(businessCollection.Cast<ConstructionCategory>().ToList());

            //List<Models.ConstructionType> constructionTypes = new List<Models.ConstructionType>();

            //foreach (ConstructionCategory field in businessCollection)
            //{
            //    constructionTypes.Add(ModelAssembler.CreateConstructionType(field));
            //}

            //return constructionTypes;
        }

        #endregion

        #region RiskType
        /// <summary>
        /// Creacion de tipo de riesgo
        /// </summary>
        /// <param name="riskTypeLocation">entidad RiskTypeLocation </param>
        /// <returns>Modelo de tipo de riesgo</returns>
        public static UNDModel.RiskType CreateRiskType(RiskTypeLocation riskTypeLocation)
        {
            var mapper = AutoMapperAssembler.CreateMapRiskTypes();
            return mapper.Map<RiskTypeLocation, UNDModel.RiskType>(riskTypeLocation);

            //return new UNDModel.RiskType
            //{
            //    Id = riskTypeLocation.RiskTypeLocationCode,
            //    Description = riskTypeLocation.SmallDescription
            //};
        }
        /// <summary>
        /// Creacion de lista de tipos de riesgos
        /// </summary>
        /// <param name="businessCollection">coleccion de entidades</param>
        /// <returns>Lista de tipos de riesgos</returns>
        public static List<UNDModel.RiskType> CreateRiskTypes(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapRiskTypes();
            return mapper.Map<List<RiskTypeLocation>, List<UNDModel.RiskType>>(businessCollection.Cast<RiskTypeLocation>().ToList());

            //List<UNDModel.RiskType> riskTypes = new List<UNDModel.RiskType>();

            //foreach (RiskTypeLocation field in businessCollection)
            //{
            //    riskTypes.Add(ModelAssembler.CreateRiskType(field));
            //}

            //return riskTypes;
        }

        #endregion

        #region RiskUse
        /// <summary>
        /// Creacion de uso de riesgo
        /// </summary>
        /// <param name="riskUse">Entidad RiskUseEarthquake </param>
        /// <returns>Modelo de uso de riesgo</returns>
        public static Models.RiskUse CreateRiskUse(RiskUseEarthquake riskUse)
        {
            var mapper = AutoMapperAssembler.CreateMapRiskUse();
            return mapper.Map<RiskUseEarthquake, Models.RiskUse>(riskUse);

            //return new Models.RiskUse
            //{
            //    Id =(int) riskUse.RiskUseCode,
            //    Description = riskUse.Description
            //};
        }
        /// <summary>
        /// Creacion de lista de uso de riesgo
        /// </summary>
        /// <param name="businessCollection">coleccion de entidades</param>
        /// <returns>Lista de uso del riesgo</returns>
        public static List<Models.RiskUse> CreateRiskUses(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapRiskUse();
            return mapper.Map<List<RiskUseEarthquake>, List<Models.RiskUse>>(businessCollection.Cast<RiskUseEarthquake>().ToList());

            //List<Models.RiskUse> riskUses = new List<Models.RiskUse>();

            //foreach (RiskUseEarthquake field in businessCollection)
            //{
            //    riskUses.Add(ModelAssembler.CreateRiskUse(field));
            //}

            //return riskUses;
        }

        #endregion

        #region PolicyRiskDTO
        public static UNDTO.PolicyRiskDTO CreatePolicyRiskDTOs(ISSEN.Policy policy)
        {
            var mapper = AutoMapperAssembler.CreateMapPolicyRiskDTOs();
            return mapper.Map<ISSEN.Policy, UNDTO.PolicyRiskDTO>(policy);

            //return new UNDTO.PolicyRiskDTO
            //{
            //    DocumentNumber = policy.DocumentNumber,
            //    PrefixId = policy.PrefixCode,
            //    BranchId = policy.BranchCode
            //};
        }
        public static List<UNDTO.PolicyRiskDTO> CreatePolicyRiskDTOs(BusinessCollection businessCollection)
        {
            var mapper = AutoMapperAssembler.CreateMapPolicyRiskDTOs();
            return mapper.Map<List<ISSEN.Policy>, List<UNDTO.PolicyRiskDTO>>(businessCollection.Cast<ISSEN.Policy>().ToList());

            //List<UNDTO.PolicyRiskDTO> policyRiskDTOs = new List<UNDTO.PolicyRiskDTO>();
            //foreach (ISSEN.Policy field in businessCollection)
            //{
            //    policyRiskDTOs.Add(CreatePolicyRiskDTOs(field));
            //}
            //return policyRiskDTOs;
        }
        #endregion

    }
}

