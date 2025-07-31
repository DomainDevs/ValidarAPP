using System.Collections.Generic;
using System.Collections;
using Sistran.Core.Application.Common.Entities;
using ISSEN = Sistran.Core.Application.Issuance.Entities;
using QUOEN = Sistran.Core.Application.Quotation.Entities;
using Sistran.Core.Application.UnderwritingServices.Models;
using CommonModel = Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.TaxServices;
using Sistran.Core.Framework.DAF;
using Sistran.Core.Services.UtilitiesServices.Enums;

namespace Sistran.Core.Application.Vehicles.ThirdPartyLiabilityService.EEProvider.Assemblers
{
    public class ModelAssembler
    {
        #region Shuttle
        /// <summary>
        /// Creacion de trayecto
        /// </summary>
        /// <param name="shuttle">Entidad trayecto</param>
        /// <returns>Modelo trayecto</returns>
        public static Models.Shuttle CreateShuttle(Shuttle shuttle)
        {
            return new Models.Shuttle
            {
                Id = shuttle.ShuttleCode,
                Description = shuttle.Description,
                SmallDescription = shuttle.SmallDescription,
                Enabled = shuttle.Enabled
            };
        }
        /// <summary>
        /// Crear trayectos
        /// </summary>
        /// <param name="shuttles">Lista de trayecto</param>
        /// <returns>Lista de trayectos</returns>
        public static List<Models.Shuttle> CreateShuttles(IList shuttles)
        {
            List<Models.Shuttle> shuttleModels = new List<Models.Shuttle>();
            foreach (Shuttle item in shuttles)
            {
                shuttleModels.Add(CreateShuttle(item));
            }
            return shuttleModels;
        }

        #endregion
        #region Deductible

        public static Deductible CreateCoverageDeductible(ISSEN.RiskCoverDeduct riskCoverDeduct)
        {
            return new Deductible
            {
                Id = riskCoverDeduct.DeductId.GetValueOrDefault(),
                Rate = riskCoverDeduct.Rate,
                RateType = (RateType)riskCoverDeduct.RateTypeCode,
                DeductValue = riskCoverDeduct.DeductValue,
                DeductibleUnit = new DeductibleUnit
                {
                    Id = riskCoverDeduct.DeductUnitCode
                },
                MinDeductValue = riskCoverDeduct.MinDeductValue.GetValueOrDefault(),
                MinDeductibleUnit = new DeductibleUnit
                {
                    Id = riskCoverDeduct.MinDeductUnitCode.GetValueOrDefault()
                },
                MinDeductibleSubject = new DeductibleSubject
                {
                    Id = riskCoverDeduct.MinDeductSubjectCode.GetValueOrDefault()
                },
                MaxDeductValue = riskCoverDeduct.MaxDeductValue.GetValueOrDefault(),
                MaxDeductibleUnit = new DeductibleUnit
                {
                    Id = riskCoverDeduct.MaxDeductUnitCode.GetValueOrDefault()
                },
                MaxDeductibleSubject = new DeductibleSubject
                {
                    Id = riskCoverDeduct.MaxDeductSubjectCode.GetValueOrDefault()
                },
                DeductibleSubject = new DeductibleSubject
                {
                    Id = riskCoverDeduct.DeductSubjectCode.GetValueOrDefault()
                },
                Currency = new CommonModel.Currency
                {
                    Id = riskCoverDeduct.CurrencyCode.GetValueOrDefault()
                }
            };
        }

        public static Deductible CreateDeductible(QUOEN.Deductible deductible)
        {
            return new Deductible
            {
                Id = deductible.DeductId,
                Rate = deductible.Rate,
                RateType = (RateType)deductible.RateTypeCode,
                DeductValue = deductible.DeductValue,
                DeductibleUnit = new DeductibleUnit { Id = deductible.DeductUnitCode },
                MinDeductValue = deductible.MinDeductValue,
                MinDeductibleUnit = new DeductibleUnit { Id = deductible.MinDeductUnitCode.GetValueOrDefault() },
                MinDeductibleSubject = new DeductibleSubject { Id = deductible.MinDeductSubjectCode.GetValueOrDefault() },
                MaxDeductValue = deductible.MaxDeductValue,
                MaxDeductibleUnit = new DeductibleUnit { Id = deductible.MaxDeductUnitCode.GetValueOrDefault() },
                MaxDeductibleSubject = new DeductibleSubject { Id = deductible.MaxDeductSubjectCode.GetValueOrDefault() },
                DeductibleSubject = new DeductibleSubject { Id = deductible.DeductSubjectCode.GetValueOrDefault() },
                Currency = new CommonModel.Currency { Id = deductible.CurrencyCode.GetValueOrDefault() }
            };
        }

        public static List<Deductible> CreateDeductibles(BusinessCollection businessCollection)
        {
            List<Deductible> deductibles = new List<Deductible>();

            foreach (QUOEN.Deductible field in businessCollection)
            {
                deductibles.Add(ModelAssembler.CreateDeductible(field));
            }

            return deductibles;
        }

        #endregion
    }
}
