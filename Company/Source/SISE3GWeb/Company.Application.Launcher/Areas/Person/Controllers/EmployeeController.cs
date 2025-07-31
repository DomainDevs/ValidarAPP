using Sistran.Company.Application.ModelServices.Models;
using Sistran.Company.Application.ModelServices.Models.Param;
using Sistran.Company.Application.UniquePersonAplicationServices.DTOs;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.ModelServices.Enums;
using Sistran.Core.Framework.UIF.Web.Areas.Person.Enums;
using Sistran.Core.Framework.UIF.Web.Areas.Underwriting.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Core.Framework.UIF2.Controls.UifSelect;
using Sistran.Core.Framework.UIF2.Controls.UifTable;
using Sistran.Core.Services.UtilitiesServices.Enums;
using Sistran.Core.Services.UtilitiesServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ACM = Sistran.Core.Application.CommonService.Models;
using ASEPER = Sistran.Core.Framework.UIF.Web.Areas.Person.Models;
using CPEMCV1 = Sistran.Company.Application.UniquePersonServices.V1.Models;
using CPEMV1 = Sistran.Core.Application.UniquePersonService.V1.Models;
using TaxModel = Sistran.Core.Application.TaxServices.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using Sistran.Company.Application.UniquePersonServices.V1.Models;

// -----------------------------------------------------------------------
// <copyright file="PersonController.cs" company="SISTRAN">
// Copyright (c) SISTRAN ANDINA. All rights reserved.
// </copyright>
// <author>Desconocido</author>
// -----------------------------------------------------------------------

namespace Sistran.Core.Framework.UIF.Web.Areas.Person.Controllers
{
    /// <summary>
    /// Acciones de person
    /// </summary>
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class EmployeeController : Controller
    {


        public ActionResult Employee()
        {
            return View();
        }

        public ActionResult SaveEmployee(EmployeeDTO employee)
        {

            try
            {
                employee.UserId = SessionHelper.GetUserId();
                EmployeeDTO result = DelegateService.uniquePersonAplicationService.CreateEmployee(employee);

                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetPhoneTypes);
            }
        }

        public ActionResult GetEmployee(int individualId)
        {
            try
            {
                EmployeeDTO result = DelegateService.uniquePersonAplicationService.GetEmployeeIndividualId(individualId);

                return new UifJsonResult(true, result);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetPhoneTypes);
            }
        }

        public ActionResult GetDeclinedType()
        {
            try
            {
                List<CPEMCV1.CompanyThirdDeclinedType> thirdDeclinedTypes = DelegateService.uniquePersonServiceV1.GetAllThirdDeclinedTypes();
                return new UifJsonResult(true, thirdDeclinedTypes.OrderBy(x => x.Description));
            }
            catch
            {

                return new UifJsonResult(false, App_GlobalResources.LanguagePerson.ErrorGetThirdDeclinedTypes);
            }

        }

    }

}

