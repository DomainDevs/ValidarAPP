// -----------------------------------------------------------------------
// <copyright file="CountryStateCityDAO.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Oscar Camacho</author>
// -----------------------------------------------------------------------

namespace Sistran.Company.Application.UniquePersonParamService.EEProvider.DAOs
{
    using Sistran.Company.Application.UniquePersonParamService.EEProvider.Assemblers;
    using Sistran.Company.Application.UniquePersonParamService.EEProvider.Entities.views;
    using Sistran.Company.Application.UniquePersonParamService.Models;
    using Sistran.Core.Application.Common.Entities;
    using Sistran.Core.Application.Utilities.DataFacade;
    using Sistran.Core.Application.Utilities.Enums;
    using Sistran.Core.Application.Utilities.Error;
    using Sistran.Core.Framework.DAF;
    using Sistran.Core.Framework.DAF.Engine;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using UNENT = Sistran.Core.Application.UniquePerson.Entities;

    public class CountryStateCityDAO
    {
        public Result<List<ParamCountryStateCity>, ErrorModel> GetCountryStateCity()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();          
            try
            {
                List<ParamCountryStateCity> paramCountryStateCity = new List<ParamCountryStateCity>();
                CountryStateCityView view = new CountryStateCityView();
                ViewBuilder builder = new ViewBuilder("CountryStateCityView");

                DataFacadeManager.Instance.GetDataFacade().FillView(builder, view);

                foreach (City city in view.City.Cast<City>())
                {

                    State state = view.State.Cast<State>().First(x => x.CountryCode == city.CountryCode && x.StateCode == city.StateCode);
                    Country country = view.Country.Cast<Country>().First(x => x.CountryCode == city.CountryCode);

                    paramCountryStateCity.Add(ModelAssembler.GenerateCountryStateCity(city, state, country));

                }

                return new ResultValue<List<ParamCountryStateCity>, ErrorModel>(paramCountryStateCity);
            }
            catch (System.Exception ex)
            {
                List<string> listErrors = new List<string>();
                listErrors.Add(Resources.Errors.NoTypeOfBranchWasFound);
                return new ResultError<List<ParamCountryStateCity>, ErrorModel>(ErrorModel.CreateErrorModel(listErrors, ErrorType.TechnicalFault, ex));
            }
        }
    }
}
