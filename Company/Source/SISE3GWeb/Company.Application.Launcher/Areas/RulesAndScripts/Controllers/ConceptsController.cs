using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Sistran.Core.Framework.UIF.Web.Areas.Common.Controllers;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using RulesScriptsEnum = Sistran.Core.Application.RulesScriptsServices.Enums;

namespace Sistran.Core.Framework.UIF.Web.Areas.RulesAndScripts.Controllers
{
    [Authorize]
    public class ConceptsController : Controller
    {
        /// <summary>
        /// Obtiene los conceptos segun el filtro 
        /// </summary>
        /// <param name="listEntities">lista de id de entidades</param>
        /// <param name="filter">like de la descripcion</param>
        /// <returns></returns>
        public JsonResult GetConceptsByFilter(List<int> listEntities, string filter)
        {
            try
            {
                var concepts = DelegateService.conceptsService.GetConceptsByFilter(listEntities, filter);
                return new UifJsonResult(true, concepts);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }

        /// <summary>
        /// Obtiene los conceptos por id concept y idEntity
        /// </summary>
        /// <param name="idConcept">id del concepto</param>
        /// <param name="idEntity">id de la entidad</param>
        /// <returns></returns>
        public JsonResult GetConceptByIdConceptIdEntity(int idConcept, int idEntity)
        {
            try
            {
                var concept = DelegateService.conceptsService.GetConceptByIdConceptIdEntity(idConcept, idEntity);
                return new UifJsonResult(true, concept);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, e.Message);
            }
        }

        /// <summary>
        /// Obtiene el concepto especifico con sus respectivos valores
        /// </summary>
        /// <param name="idEntity">id de la entidad</param>
        /// <param name="idConcept">id del concepto</param>
        /// <param name="conceptType">tipo de concepto</param>
        /// <returns></returns>
        public JsonResult GetSpecificConceptWithVales(int idConcept, int idEntity, string[] dependency, RulesScriptsEnum.ConceptType conceptType) {
            try
            {
                var concept = DelegateService.conceptsService.GetSpecificConceptWithVales(idConcept, idEntity, dependency, conceptType);
                return new UifJsonResult(true, concept);
            }
            catch (Exception e)
            {
                return new UifJsonResult(false, ExceptionHelper.GetMessage(e.Message));
            }
        }
    }
}