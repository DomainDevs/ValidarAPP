using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CModelsV1 = Sistran.Core.Application.UniquePersonService.V1.Models;
using ComModelsV1 = Sistran.Company.Application.UniquePersonServices.V1.Models;
using UPV1 = Sistran.Core.Application.UniquePersonService.V1Individual.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    public class DocumentTypeRangeController : Controller
    {
        // GET: Parametrization/DocumentTypeRange
        public ActionResult DocumentTypeRange()
        {
            return View();
        }

        public JsonResult GetDocumentTypeRange()
        {
            try
            {
                List<CModelsV1.DocumentTypeRange> DocumentTypeRange = DelegateService.uniquePersonServiceV1.GetDocumentTypeRange();
  
                ComModelsV1.CiaDocumentTypeRange ciaDocumentTypeRanges = new ComModelsV1.CiaDocumentTypeRange();

                foreach (CModelsV1.DocumentTypeRange item in DocumentTypeRange)
                {
                    ciaDocumentTypeRanges =  DelegateService.uniquePersonServiceV1.GetCiaDocumentTypeRangeId(item.Id);
                    UPV1.IndividualType IndividualType = DelegateService.uniquePersonServiceV1.GetIndividualTypeById(ciaDocumentTypeRanges.IndividualTypeId);

                    item.DescriptionIndividualType = IndividualType.Description;
                }

                return new UifJsonResult(true, DocumentTypeRange);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, "GetDocumentTypeRange");
            }

        }

        public JsonResult SaveDocumentTypeRange(DocumentTypeRangeViewModel ListDocumentTypeRange)
        {
            try
            {
                DelegateService.uniquePersonServiceV1.CreateDocumentTypeRange(new CModelsV1.DocumentTypeRange()
                {
                    CardTypeCode = new CModelsV1.DocumentType() { Id = ListDocumentTypeRange.TypeDocumentId },
                    Gender = ListDocumentTypeRange.GenderId == 1 ? "M" : "F",
                    CardNumberFrom = ListDocumentTypeRange.Inicial,
                    CardNumberTo = ListDocumentTypeRange.Final
                });

                //Tabla extendida
                DelegateService.uniquePersonServiceV1.CreateCiaDocumentTypeRange(new ComModelsV1.CiaDocumentTypeRange()
                {
                    IndividualTypeId = ListDocumentTypeRange.PersonTypeId
                });

                return new UifJsonResult(true, App_GlobalResources.Language.CreateDocumentTypeRange);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, "Error SaveDocumentTypeRange");
            }   
        }

        public ActionResult GetIndividualTypes()
        {
            try
            {
                List<UPV1.IndividualType> individualType = DelegateService.uniquePersonServiceV1.GetIndividualTypes();
                return new UifJsonResult(true, individualType);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, "Error GetIndividualTypes");
            }
        }

        public JsonResult GetDocumentTypeRangeById(int IndividualTypeId)
        {
            try
            {
                List<CModelsV1.DocumentTypeRange> DocumentTypeRange = DelegateService.uniquePersonServiceV1.GetDocumentTypeRange();

                ComModelsV1.CiaDocumentTypeRange ciaDocumentTypeRanges = new ComModelsV1.CiaDocumentTypeRange();

                foreach (CModelsV1.DocumentTypeRange item in DocumentTypeRange)
                {
                    ciaDocumentTypeRanges = DelegateService.uniquePersonServiceV1.GetCiaDocumentTypeRangeId(item.Id);
                    UPV1.IndividualType IndividualType = DelegateService.uniquePersonServiceV1.GetIndividualTypeById(ciaDocumentTypeRanges.IndividualTypeId);
                    item.DescriptionIndividualType = IndividualType.Description;
                    item.IndividualTypeId = IndividualType.Id;
                }

                DocumentTypeRange.RemoveAll(x => x.IndividualTypeId != IndividualTypeId); 

                return new UifJsonResult(true, DocumentTypeRange);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, "Error GetIndividualTypes");
            }
        }



        public JsonResult UpdateDocumentTypeRange(DocumentTypeRangeViewModel ListDocumentTypeRange, int documentTypeRangeId)
        {
            try
            {
                DelegateService.uniquePersonServiceV1.UpdateDocumentTypeRange(new ComModelsV1.DocumentTypeRange()
                {
                    Id = documentTypeRangeId,
                    CardTypeCode = new ComModelsV1.CompanyDocumentType() { Id = ListDocumentTypeRange.TypeDocumentId },
                    Gender = ListDocumentTypeRange.GenderId == 1 ? "M" : "F",
                    CardNumberFrom = ListDocumentTypeRange.Inicial,
                    CardNumberTo = ListDocumentTypeRange.Final
                });
                
                //Tabla extendida
                DelegateService.uniquePersonServiceV1.UpdateCiaDocumentTypeRange(new ComModelsV1.CiaDocumentTypeRange()
                {
                    IndividualTypeId = ListDocumentTypeRange.PersonTypeId,
                    DocumentTypeRange = documentTypeRangeId
                });


                return new UifJsonResult(true, App_GlobalResources.Language.UpdateDocumentTypeRange);
            }
            catch (Exception ex)
            {

                return new UifJsonResult(false, "Error UpdateDocumentTypeRange" + ex.Message);
            }
        }


        public JsonResult GetDocumentTypeRangeId(int documentTypeRangeId)
        {
            try
            {
                List<CModelsV1.DocumentTypeRange> DocumentTypeRange = DelegateService.uniquePersonServiceV1.GetDocumentsTypeRangeId(documentTypeRangeId);

                ComModelsV1.CiaDocumentTypeRange ciaDocumentTypeRanges = new ComModelsV1.CiaDocumentTypeRange();

                foreach (CModelsV1.DocumentTypeRange item in DocumentTypeRange)
                {
                    ciaDocumentTypeRanges = DelegateService.uniquePersonServiceV1.GetCiaDocumentTypeRangeId(item.Id);
                    UPV1.IndividualType IndividualType = DelegateService.uniquePersonServiceV1.GetIndividualTypeById(ciaDocumentTypeRanges.IndividualTypeId);
                    item.DescriptionIndividualType = IndividualType.Description;
                    item.IndividualTypeId = IndividualType.Id;
                }
              
                return new UifJsonResult(true, DocumentTypeRange);
            }
            catch (Exception)
            {

                return new UifJsonResult(false, "Error GetIndividualTypes");
            }
        }


    }
}