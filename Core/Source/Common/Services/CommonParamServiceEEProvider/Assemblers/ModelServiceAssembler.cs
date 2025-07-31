// -----------------------------------------------------------------------
// <copyright file="ModelServiceAssembler.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Jeison Rodriguez</author>
// -----------------------------------------------------------------------
namespace Sistran.Core.Application.CommonParamService.Assemblers
{
    using System;
    using System.Collections.Generic;
    using CommonService.Models;
    using ModelServices.Models.CommonParam;
    using Sistran.Core.Application.CommonParamService.Models;
    using Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.ModelServices.Models.Common;       
    using UNIEN = UniquePerson.Entities;

    /// <summary>
    /// Covnierte de modelos de negocio a modelos de servicio
    /// </summary>
    public class ModelServiceAssembler
    {
        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ModelServiceAssembler"/> class.
        /// </summary>
        protected ModelServiceAssembler()
        {
        }

        /// <summary>
        /// Mapeo del modelo ParamParameter al modelo ParametersServiceModel
        /// </summary>
        /// <param name="paramParameter">Lista de ParamParameter</param>
        /// <returns>Modelo ParametersServiceModel</returns>
        public static ParametersServiceModel MappParameters(List<ParamParameter> paramParameter)
        {
            ParametersServiceModel paramParametersServiceModel = new ParametersServiceModel();
            List<ParameterServiceModel> listParameterServiceModel = new List<ParameterServiceModel>();
            foreach (ParamParameter paramParameterServiceModel in paramParameter)
            {
                ParameterServiceModel itemParameterServiceModel = new ParameterServiceModel();
                itemParameterServiceModel.ParameterId = paramParameterServiceModel.ParameterId;
                itemParameterServiceModel.Description = paramParameterServiceModel.Description;
                itemParameterServiceModel.Value = paramParameterServiceModel.Value;

                listParameterServiceModel.Add(itemParameterServiceModel);
            }

            paramParametersServiceModel.ErrorDescription = new List<string>();
            paramParametersServiceModel.ErrorTypeService = ErrorTypeService.Ok;
            paramParametersServiceModel.ParameterServiceModel = listParameterServiceModel;

            return paramParametersServiceModel;
        }

        /// <summary>
        /// Mapeo de la modelo ParamParameter al modelo ParameterServiceModel
        /// </summary>
        /// <param name="paramParameter">Lista de ParamParameter</param>
        /// <returns>Modelo ParameterServiceModel</returns>
        public static ParameterServiceModel MappParameter(ParamParameter paramParameter)
        {
            ParameterServiceModel parameterServiceModel = new ParameterServiceModel();
            parameterServiceModel.ParameterId = paramParameter.ParameterId;
            parameterServiceModel.Description = paramParameter.Description;
            parameterServiceModel.Value = paramParameter.Value;
            parameterServiceModel.ParametricServiceModel = new ModelServices.Models.Param.ParametricServiceModel();
            parameterServiceModel.ParametricServiceModel.ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel();
            parameterServiceModel.ParametricServiceModel.ErrorServiceModel.ErrorDescription = new List<string>();
            parameterServiceModel.ParametricServiceModel.ErrorServiceModel.ErrorTypeService = ErrorTypeService.Ok;

            parameterServiceModel.DiscontinuityLogServiceModel = new DiscontinuityLogServiceModel();
            parameterServiceModel.DiscontinuityLogServiceModel.ParametricServiceModel = new ModelServices.Models.Param.ParametricServiceModel();
            parameterServiceModel.DiscontinuityLogServiceModel.ParametricServiceModel.ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel();
            parameterServiceModel.DiscontinuityLogServiceModel.ParametricServiceModel.ErrorServiceModel.ErrorDescription = new List<string>();
            parameterServiceModel.DiscontinuityLogServiceModel.ParametricServiceModel.ErrorServiceModel.ErrorTypeService = ErrorTypeService.Ok;

            parameterServiceModel.InfringementLogServiceModel = new InfringementLogServiceModel();
            parameterServiceModel.InfringementLogServiceModel.ParametricServiceModel = new ModelServices.Models.Param.ParametricServiceModel();
            parameterServiceModel.InfringementLogServiceModel.ParametricServiceModel.ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel();
            parameterServiceModel.InfringementLogServiceModel.ParametricServiceModel.ErrorServiceModel.ErrorDescription = new List<string>();
            parameterServiceModel.InfringementLogServiceModel.ParametricServiceModel.ErrorServiceModel.ErrorTypeService = ErrorTypeService.Ok;

            return parameterServiceModel;
        }

        /// <summary>
        /// Convierte modelos de sucursal en Modelos de servicio de sucursal
        /// </summary>
        /// <param name="paramBranchs">Lista de sucursales</param>
        /// <returns>Lista de modelos de servicio de sucursal</returns>
        public static List<BranchServiceQueryModel> CreateBranchServiceQueryModels(List<ParamBranch> paramBranchs)
        {
            List<BranchServiceQueryModel> branchServiceQueryModels = new List<BranchServiceQueryModel>();

            foreach (ParamBranch paramBranch in paramBranchs)
            {
                branchServiceQueryModels.Add(CreateBranchServiceQueryModel(paramBranch));
            }

            return branchServiceQueryModels;
        }

        /// <summary>
        /// Convierte modelo de sucursal en Modelo de servicio de sucursal
        /// </summary>
        /// <param name="paramBranch">Modelo de sucursal</param>
        /// <returns>Modelo de servicio de sucursal</returns>
        public static BranchServiceQueryModel CreateBranchServiceQueryModel(ParamBranch paramBranch)
        {
            return new BranchServiceQueryModel()
            {
                Id = paramBranch.Id,
                Description = paramBranch.Description,
                SmallDescription = paramBranch.SmallDescription,
                Is_issue = paramBranch.Is_issue
            };
        }

        /// <summary>
        /// Convierte lista de modelos de puntos de venta en lista de Modelos de servicio de punto de venta
        /// </summary>
        /// <param name="salepoint">Lista de puntos de venta</param>
        /// <returns>Lista de modelos de servicio de puntos de venta</returns>
        public static List<SalePointServiceModel> CreateSalePointServiceModel(List<ParamSalePoint> salepoint)
        {
            List<SalePointServiceModel> salePointServiceModel = new List<SalePointServiceModel>();
            foreach (var item in salepoint)
            {
                salePointServiceModel.Add(CreateSalePointServiceModel(item));
            }

            return salePointServiceModel;
        }

        /// <summary>
        /// convierte lista de modelo de negocio a lista modelo de servicio
        /// </summary>
        /// <param name="branch">lista de modelo de negocio</param>
        /// <returns>lista de modelo de servicio</returns>
        public static List<BranchServiceModel> CreateBranchesServicesModel(List<ParamCoBranch> branch)
        {
            List<BranchServiceModel> branchServiceModel = new List<BranchServiceModel>();

            foreach (var item in branch)
            {
                branchServiceModel.Add(CreateBranchServiceModel(item));
            }

            return branchServiceModel;
        }

        /// <summary>
        /// convierte de modelo de servicio a modelo de negocio
        /// </summary>
        /// <param name="paramCoBranch">modelo de negocio</param>
        /// <returns>modelo de servicio</returns>
        public static BranchServiceModel CreateBranchServiceModel(ParamCoBranch paramCoBranch)
        {
            BranchServiceModel branchServiceModel = new BranchServiceModel();
            branchServiceModel.AddressType = CreateAddressType(paramCoBranch.AddressType);
            branchServiceModel.Address = paramCoBranch.Address;
            branchServiceModel.Branch = CreateBranchServiceQueryModel(paramCoBranch.Branch);
            branchServiceModel.City = new CityServiceRelationModel { Id = paramCoBranch.City.Id };
            branchServiceModel.PhoneNumber = paramCoBranch.PhoneNumber;
            branchServiceModel.PhoneType = new PhoneTypeServiceQueryModel { Id = paramCoBranch.PhoneType.Id };
            branchServiceModel.IsIssue = paramCoBranch.IsIssue;
            branchServiceModel.State = new StateServiceQueryModel { Id = paramCoBranch.State.Id };
            branchServiceModel.Id = paramCoBranch.Id;
            branchServiceModel.Country = new CountryServiceQueryModel { Id = paramCoBranch.Country.Id };
            branchServiceModel.StatusTypeService = ModelServices.Enums.StatusTypeService.Original;
            branchServiceModel.ErrorServiceModel = new ModelServices.Models.Param.ErrorServiceModel()
            {
                ErrorTypeService = ErrorTypeService.Ok
            };

            return branchServiceModel;
        }

        /// <summary>
        /// Convierte modelos de puntos de venta en Modelos de servicio de punto de venta
        /// </summary>
        /// <param name="item">puntos de venta</param>
        /// <returns>modelos de servicio de puntos de venta</returns>
        private static SalePointServiceModel CreateSalePointServiceModel(ParamSalePoint item) => new SalePointServiceModel
        {
            Description = item.Description,
            Id = item.Id,
            SmallDescription = item.SmallDescription,
            Branch = new BranchServiceQueryModel
            {
                Description = item.Branch.Description,
                Id = item.Branch.Id
            },
            Enabled = item.Enabled
        };

        /// <summary>
        /// convierte de modelo de negocio a entidad
        /// </summary>
        /// <param name="adrressType">modelo de entidad</param>
        /// <returns>modelo de servicio </returns>
        private static AddressTypeServiceQueryModel CreateAddressType(ParamAddressType adrressType) => new AddressTypeServiceQueryModel
        {
            Description = adrressType?.Description,
            Id = adrressType.Id
        };

        /// <summary>
        /// Convierte el MOD-B de las paises al MOD-S
        /// </summary>
        /// <param name="countries">MOD-B de las paises</param>
        /// <returns>MOD-S de las paises</returns>
        public static List<CountryServiceQueryModel> CreateCountryServiceQueryModel(List<Country> countries)
        {
            List<CountryServiceQueryModel> countryServiceQueryModels = new List<CountryServiceQueryModel>();
            foreach (Country country in countries)
            {
                countryServiceQueryModels.Add(new CountryServiceQueryModel
                {
                    Id = country.Id,
                    Description = country.Description,
                });
            }
            return countryServiceQueryModels;
        }

        /// <summary>
        /// Convierte el MOD-B de las estados al MOD-S
        /// </summary>
        /// <param name="states">MOD-B de las estados</param>
        /// <returns>MOD-S de las estados</returns>
        public static List<StateServiceQueryModel> CreateStatesServiceQueryModel(List<State> states)
        {
            List<StateServiceQueryModel> stateServiceQueryModels = new List<StateServiceQueryModel>();
            foreach (State state in states)
            {
                stateServiceQueryModels.Add(new StateServiceQueryModel
                {
                    Id = state.Id,
                    Description = state.Description,
                    Country = new CountryServiceQueryModel
                    {
                        Id = state.Country.Id
                    }
                });
            }
            return stateServiceQueryModels;
        }

        /// <summary>
        /// Convierte el MOD-B de las ciudades al MOD-S
        /// </summary>
        /// <param name="cities">MOD-B de las ciudades</param>
        /// <returns>MOD-S de las ciudades</returns>
        public static List<CityServiceRelationModel> CreateCityServiceRelationModels(List<City> cities)
        {
            List<CityServiceRelationModel> cityServiceRelationModels = new List<CityServiceRelationModel>();
            foreach (City city in cities)
            {
                cityServiceRelationModels.Add(new CityServiceRelationModel
                {
                    Id = city.Id,
                    Description = city.Description,
                    State = new StateServiceQueryModel
                    {
                        Id = city.State.Id,
                        Description = city.State.Description,
                        Country = new CountryServiceQueryModel
                        {
                            Id = city.State.Country.Id,
                            Description = city.State.Country.Description
                        }
                    }
                });
            }
            return cityServiceRelationModels;
        }

        /// <summary>
        /// convierte de entidad a modelo de servicio 
        /// </summary>
        /// <param name="phoneType">entidad</param>
        /// <returns>modelo de servicio</returns>
        public static List<PhoneTypeServiceQueryModel> CreatePhoneTypeServiceQueryModel(List<UNIEN.PhoneType> phoneType)
        {
            List<PhoneTypeServiceQueryModel> phoneTypeServiceQueryModel = new List<PhoneTypeServiceQueryModel>();

            foreach (var item in phoneType)
            {
                phoneTypeServiceQueryModel.Add(CreatePhone(item));
            }

            return phoneTypeServiceQueryModel;
        }

        /// <summary>
        /// convierte de entidad a modelo de servicio 
        /// </summary>
        /// <param name="item">entidad</param>
        /// <returns>modelo de servicio</returns>
        private static PhoneTypeServiceQueryModel CreatePhone(UNIEN.PhoneType item) => new PhoneTypeServiceQueryModel
        {
            Description = item.Description,
            Id = item.PhoneTypeCode
        };

        /// <summary>
        /// convierte de entidad a modelo de servicio 
        /// </summary>
        /// <param name="addressType">entidad</param>
        /// <returns>modelo de servicio</returns>
        public static List<AddressTypeServiceQueryModel> CreateAddressTypeServiceQueryModel(List<UNIEN.AddressType> addressType)
        {
            List<AddressTypeServiceQueryModel> addressTypeServiceQueryModel = new List<AddressTypeServiceQueryModel>();

            foreach (var item in addressType)
            {
                addressTypeServiceQueryModel.Add(CreateAddressTypes(item));
            }

            return addressTypeServiceQueryModel;
        }

        /// <summary>
        /// convierte de entidad a modelo de servicio 
        /// </summary>
        /// <param name="item">entidad</param>
        /// <returns>modelo de servicio</returns>
        private static AddressTypeServiceQueryModel CreateAddressTypes(UNIEN.AddressType item) => new AddressTypeServiceQueryModel
        {
            Description = item.SmallDescription,
            Id = item.AddressTypeCode
        };

    }
}
