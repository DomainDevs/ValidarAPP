// -----------------------------------------------------------------------
// <copyright file="ParamVehicleVersion.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Andres Gomez Hernandez</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Application.VehicleParamService.Models
{
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Application.VehicleParamService.Resources;
    using System.Collections.Generic;

    /// <summary>
    /// Modelo de negocio Version
    /// </summary>
    public class ParamVehicleVersion
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly int id;
        /// <summary>
        /// 
        /// </summary>
        private readonly string description;
        /// <summary>
        /// 
        /// </summary>
        private readonly ParamVehicleModel paramVehicleModel;
        /// <summary>
        /// 
        /// </summary>
        private readonly int? engineQuantity;
        /// <summary>
        /// 
        /// </summary>
        private readonly ParamVehicleMake paramVehicleMake;
        /// <summary>
        /// 
        /// </summary>
        private readonly int? horsePower;
        /// <summary>
        /// 
        /// </summary>
        private readonly int? weight;
        /// <summary>
        /// 
        /// </summary>
        private readonly int? tonsQuantity;
        /// <summary>
        /// 
        /// </summary>
        private readonly int? passengerQuantity;
        /// <summary>
        /// 
        /// </summary>
        private readonly ParamVehicleFuel paramVehicleFuel;
        /// <summary>
        /// 
        /// </summary>
        private readonly ParamVehicleBody paramVehicleBody;
        /// <summary>
        /// 
        /// </summary>
        private readonly ParamVehicleType paramVehicleType;
        /// <summary>
        /// 
        /// </summary>
        private readonly ParamVehicleTransmissionType paramVehicleTransmissionType;
        /// <summary>
        /// 
        /// </summary>
        private readonly int? maxSpeedQuantity;
        /// <summary>
        /// 
        /// </summary>
        private int? doorQuantity;
        /// <summary>
        /// 
        /// </summary>
        private readonly decimal? price;
        /// <summary>
        /// 
        /// </summary>
        private readonly bool isImported;
        /// <summary>
        /// 
        /// </summary>
        private readonly bool? lastModel;
        /// <summary>
        /// 
        /// </summary>
        private readonly ParamCurrency paramCurrency;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <param name="paramVehicleModel"></param>
        /// <param name="engineQuantity"></param>
        /// <param name="paramVehicleMake"></param>
        /// <param name="horsePower"></param>
        /// <param name="weight"></param>
        /// <param name="tonsQuantity"></param>
        /// <param name="passengerQuantity"></param>
        /// <param name="paramVehicleFuel"></param>
        /// <param name="paramVehicleBody"></param>
        /// <param name="paramVehicleType"></param>
        /// <param name="paramVehicleTransmissionType"></param>
        /// <param name="maxSpeedQuantity"></param>
        /// <param name="doorQuantity"></param>
        /// <param name="price"></param>
        /// <param name="isImported"></param>
        /// <param name="lastModel"></param>
        private ParamVehicleVersion(int id, string description, ParamVehicleModel paramVehicleModel,int? engineQuantity, ParamVehicleMake paramVehicleMake, int? horsePower,int? weight, int? tonsQuantity, int? passengerQuantity, ParamVehicleFuel paramVehicleFuel,ParamVehicleBody paramVehicleBody, ParamVehicleType paramVehicleType, ParamVehicleTransmissionType paramVehicleTransmissionType,int? maxSpeedQuantity, int? doorQuantity, decimal? price, bool isImported, bool? lastModel, ParamCurrency paramCurrency)
        {
            this.id = id;
            this.description = description;
            this.paramVehicleModel = paramVehicleModel;
            this.engineQuantity = engineQuantity;
            this.paramVehicleMake = paramVehicleMake;
            this.horsePower = horsePower;
            this.weight = weight;
            this.tonsQuantity = tonsQuantity;
            this.passengerQuantity = passengerQuantity;
            this.paramVehicleFuel = paramVehicleFuel;
            this.paramVehicleBody = paramVehicleBody;
            this.paramVehicleType = paramVehicleType;
            this.paramVehicleTransmissionType = paramVehicleTransmissionType;
            this.maxSpeedQuantity = maxSpeedQuantity;
            this.doorQuantity = doorQuantity;
            this.price = price;
            this.isImported = isImported;
            this.lastModel = lastModel;
            this.paramCurrency = paramCurrency;
        }

        private ParamVehicleVersion(int id, string description)
        {
            this.id = id;
            this.description = description;            
        }


        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            get
            {
                return this.id;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get
            {
                return this.description;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ParamVehicleModel ParamVehicleModel
        {
            get
            {
                return this.paramVehicleModel;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? EngineQuantity
        {
            get
            {
                return this.engineQuantity;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ParamVehicleMake ParamVehicleMake
        {
            get
            {
                return this.paramVehicleMake;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? HorsePower
        {
            get
            {
                return this.horsePower;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Weight
        {
            get
            {
                return this.weight;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? TonsQuantity
        {
            get
            {
                return this.tonsQuantity;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? PassengerQuantity
        {
            get
            {
                return this.passengerQuantity;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ParamVehicleFuel ParamVehicleFuel
        {
            get
            {
                return this.paramVehicleFuel;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ParamVehicleBody ParamVehicleBody
        {
            get
            {
                return this.paramVehicleBody;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ParamVehicleType ParamVehicleType
        {
            get
            {
                return this.paramVehicleType;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ParamVehicleTransmissionType ParamVehicleTransmissionType
        {
            get
            {
                return this.paramVehicleTransmissionType;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? MaxSpeedQuantity
        {
            get
            {
                return this.maxSpeedQuantity;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DoorQuantity
        {
            get
            {
                return this.doorQuantity;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? Price
        {
            get
            {
                return this.price;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsImported
        {
            get
            {
                return this.isImported;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool? LastModel
        {
            get
            {
                return this.lastModel;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ParamCurrency ParamCurrency
        {
            get
            {
                return this.paramCurrency;
            }
        }

        public static ResultValue<ParamVehicleVersion, ErrorModel> GetParamVehicleVersion(int id, string description, ParamVehicleModel paramVehicleModel, int? engineQuantity, ParamVehicleMake paramVehicleMake, int? horsePower,int? weight, int? tonsQuantity, int? passengerQuantity, ParamVehicleFuel paramVehicleFuel,ParamVehicleBody paramVehicleBody, ParamVehicleType paramVehicleType, ParamVehicleTransmissionType paramVehicleTransmissionType,int? maxSpeedQuantity, int? doorQuantity, decimal? price, bool isImported, bool? lastModel,ParamCurrency paramCurrency)
        {
            return new ResultValue<ParamVehicleVersion, ErrorModel>(new ParamVehicleVersion(id, description, paramVehicleModel, engineQuantity, paramVehicleMake, horsePower, weight, tonsQuantity, passengerQuantity, paramVehicleFuel, paramVehicleBody, paramVehicleType, paramVehicleTransmissionType, maxSpeedQuantity,doorQuantity,price,isImported,lastModel, paramCurrency));
        }

        public static ResultValue<ParamVehicleVersion, ErrorModel> GetParamVehicleVersionIdDescription(int id, string description)
        {
            return new ResultValue<ParamVehicleVersion, ErrorModel>(new ParamVehicleVersion(id, description));
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        /// <param name="paramVehicleModel"></param>
        /// <param name="engineQuantity"></param>
        /// <param name="paramVehicleMake"></param>
        /// <param name="horsePower"></param>
        /// <param name="weight"></param>
        /// <param name="tonsQuantity"></param>
        /// <param name="passengerQuantity"></param>
        /// <param name="paramVehicleFuel"></param>
        /// <param name="paramVehicleBody"></param>
        /// <param name="paramVehicleType"></param>
        /// <param name="paramVehicleTransmissionType"></param>
        /// <param name="maxSpeedQuantity"></param>
        /// <param name="doorQuantity"></param>
        /// <param name="price"></param>
        /// <param name="isImported"></param>
        /// <param name="lastModel"></param>
        ///<param name="paramCurrency"></param>
        /// <returns></returns>
        public static Result<ParamVehicleVersion, ErrorModel> CreateParamVehicleVersion(int id, string description, ParamVehicleModel paramVehicleModel, int? engineQuantity, ParamVehicleMake paramVehicleMake, int? horsePower, int? weight, int? tonsQuantity, int? passengerQuantity,
            ParamVehicleFuel paramVehicleFuel, ParamVehicleBody paramVehicleBody, ParamVehicleType paramVehicleType, ParamVehicleTransmissionType paramVehicleTransmissionType, int? maxSpeedQuantity, int? doorQuantity, decimal? price, bool isImported, bool? lastModel, ParamCurrency paramCurrency)
        {
            List<string> error = new List<string>();
            if (engineQuantity!=null && engineQuantity > 9999)
            {
                error.Add(Errors.ErrorValidaciontEngineQuantity);
            }
            if (horsePower != null && horsePower > 255)
            {
                error.Add(Errors.ErrorValidaciontHorsePower);
            }
            if (maxSpeedQuantity != null && maxSpeedQuantity>255)
            {
                error.Add(Errors.ErrorValidaciontMaxSpeed);
            }
            if (doorQuantity != null && doorQuantity > 255)
            {
                error.Add(Errors.ErrorValidaciontDoorQuantity);
            }
            if (price==null)
            {
                error.Add(Errors.ErrorValidacionPrice);
            }
            if (price != null && price.Value.ToString().Length > 18)
            {
                error.Add(Errors.ErrorValidaciontPrice);
            }
            if (weight != null && weight > 32767)
            {
                error.Add(Errors.ErrorValidaciontWeigth);
            }
            if (passengerQuantity != null && passengerQuantity > 32767)
            {
                error.Add(Errors.ErrorValidaciontPassengerQuantity);
            }
            if (tonsQuantity != null && tonsQuantity > 255)
            {
                error.Add(Errors.ErrorValidaciontTonsQuantity);
            }
            if (description.Length > 50)
            {
                error.Add(Errors.ErrorValidacionDescription);
            }
            if (paramVehicleModel == null)
            {
                error.Add(Errors.ErrorValidacionParamVehicleModel);
            }
            if (paramVehicleMake == null)
            {
                error.Add(Errors.ErrorValidacionParamVehicleMake);
            }
            if (paramVehicleBody == null)
            {
                error.Add(Errors.ErrorValidacionParamVehicleBody);
            }
            if (paramVehicleType == null)
            {
                error.Add(Errors.ErrorValidacionParamVehicleType);
            }
            if (paramCurrency == null)
            {
                error.Add(Errors.ErrorValidacionParamCurrency);
            }
            if (error.Count > 0)
            {
                return new ResultError<ParamVehicleVersion, ErrorModel>(ErrorModel.CreateErrorModel(error, ErrorType.BusinessFault, null));
            }
            else
            {
                return new ResultValue<ParamVehicleVersion, ErrorModel>(new ParamVehicleVersion(id, description, paramVehicleModel, engineQuantity, paramVehicleMake, horsePower, weight, tonsQuantity, passengerQuantity, paramVehicleFuel, paramVehicleBody, paramVehicleType, paramVehicleTransmissionType, maxSpeedQuantity, doorQuantity, price, isImported, lastModel, paramCurrency));
            }
            
        }
    }
}