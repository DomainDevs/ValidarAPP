using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Application.UnderwritingServices.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
using Sistran.Core.Framework.UIF.Web.Areas.Printing.Models;
using Sistran.Core.Framework.UIF.Web.Helpers;
using Sistran.Core.Framework.UIF.Web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using COMModel = Sistran.Core.Application.CommonService.Models;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Company.Application.UnderwritingServices.Models;
using Sistran.Core.Services.UtilitiesServices.Models;

namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    public class TechnicalBranchController : Controller
    {
        #region Variables y Constantes
        List<LineBusiness> lineBussines = new List<LineBusiness>();
        private static List<TechnicalBranchViewModel> linebusinessModel = new List<TechnicalBranchViewModel>();
        //private static List<COM_MODELS.LineBusiness> lineModelService = new List<COM_MODELS.LineBusiness>();

        #endregion

        #region Technical Brnach Main

        // GET: Parametrization/TechnicalBranch
        /// <summary>
        /// llamado a vista de ramo tecnico
        /// </summary>
        /// <returns></returns>
        public ActionResult TechnicalBranch()
        {
            return View();
        }

        public ActionResult GetTechnicalBranchById(int id)
        {
            try
            {
                ActionResult ar = null;
                var obj = DelegateService.commonService.GetLinesBusinessByPrefixId(id);


                if (obj == null)
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.MessageNotFoundProcess);
                }
                else
                {
                    return ar;
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetProcess);
            }
        }

        public ActionResult SaveTechnicalBranch(TechnicalBranchViewModel linebusinesModel)
        {
            try
            {
                if (linebusinesModel.Id != 0)
                {
                    LineBusiness modelservice = new LineBusiness();
                    modelservice.Id = linebusinesModel.Id;
                    modelservice.Description = linebusinesModel.LongDescription;
                    modelservice.ShortDescription = linebusinesModel.ShortDescription;
                    modelservice.TyniDescription = linebusinesModel.TyniDescription;
                    modelservice.ReportLineBusiness = linebusinesModel.Id;
                    modelservice.IdLineBusinessbyRiskType = linebusinesModel.RiskTypeId;
                    modelservice.ListInsurectObjects = linebusinesModel.ListIsnsuranceObjects;

                    //lista de tipos de riesgos del ramo tecnico
                    //if (linebusinesModel.ListLineBusinessCoveredrisktype != null)
                    //{
                    //    List<LineBusinessCoveredRiskType> objeto = new List<LineBusinessCoveredRiskType>();
                    //    for (int i = 0; i < linebusinesModel.ListLineBusinessCoveredrisktype.Count; i++)
                    //    {
                    //        modelservice.ListLineBusinessCoveredrisktype = new List<LineBusinessCoveredRiskType>(linebusinesModel.ListLineBusinessCoveredrisktype.Count);
                    //        modelservice.ListLineBusinessCoveredrisktype.Add(new LineBusinessCoveredRiskType());
                    //        modelservice.ListLineBusinessCoveredrisktype[0].IdRiskType = linebusinesModel.ListLineBusinessCoveredrisktype[i].IdLineBusiness;
                    //        modelservice.ListLineBusinessCoveredrisktype[0].IdLineBusiness = linebusinesModel.Id;
                    //        objeto.Add(modelservice.ListLineBusinessCoveredrisktype[0]);
                    //    }
                    //    modelservice.ListLineBusinessCoveredrisktype = objeto;
                    //}
                    string message;
                    //lista de Amparos
                    //if (linebusinesModel.ProtectionAssigned != null)
                    //{
                    //    List<PerilLineBusiness> object1 = new List<PerilLineBusiness>();
                    //    for (int i = 0; i < linebusinesModel.ProtectionAssigned.Count; i++)
                    //    {
                    //        //new List<QUOModel.PerilLineBusiness>(linebusinesModel.ProtectionAssigned.Count);
                    //        modelservice.ListProtections = new List<PerilLineBusiness>(linebusinesModel.ProtectionAssigned.Count);
                    //        modelservice.ListProtections.Add(new PerilLineBusiness());
                    //        modelservice.ListProtections[0].IdPeril = linebusinesModel.ProtectionAssigned[i].Id;
                    //        modelservice.ListProtections[0].IdLineBusiness = linebusinesModel.Id;
                    //        object1.Add(modelservice.ListProtections[0]);
                    //    }

                    //    modelservice.ListProtections = object1;
                    //}                   

                    LineBusiness response = DelegateService.commonService.CreateLineBussiness(modelservice);
                    message = "Los datos se modificaron correctamente";
                    
                    return new UifJsonResult(true, new { message = message });
                }

                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSendLineBusiness);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorCreateLineBusiness);//pendiente colocar mensaje para ramo tecnico
            }
        }
        #endregion

        #region Search Advanced Technical Brnach

        /// <summary>
        /// llamado a vista de busqueda avanzada de ramo tecnico
        /// </summary>
        /// <returns></returns>
        public PartialViewResult SearchAdvancedTechnicalBranch()
        {
            return PartialView();
        }
        #endregion
   
        #region Insurance Objects Technical Brnach

        /// <summary>
        /// llamado a vista de objetos de seguro para ramo tecnico
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Insuranceobjects()
        {
            return PartialView();
        }

        public ActionResult GetAllInsuranceObjects()
        {
            try
            {
                List<CompanyInsuredObject> companyInsuredObjects = DelegateService.underwritingService.GetCompanyInsuredObjects();
                List<InsurencesObjectsViewModel> insurencesObjectsResult = new List<InsurencesObjectsViewModel>();
                insurencesObjectsResult.ForEach(x => x.DeclarativeDescription = x.IsDeraclarative == true ? App_GlobalResources.Language.LabelEnabled : String.Empty);
                foreach (CompanyInsuredObject companyInsuredObject in companyInsuredObjects)
                {
                    insurencesObjectsResult.Add(Models.ModelAssembler.CreateInsurencesObjects(companyInsuredObject));
                }
                if (insurencesObjectsResult.Count != 0)
                {
                    return new UifJsonResult(true, insurencesObjectsResult.OrderBy(x => x.Description).ToList());
                }
                return new UifJsonResult(false, App_GlobalResources.Language.ErrMsgRangeEntityNotFound);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }

        public ActionResult GetObjectByLineBusinessId(int idLineBusiness)
        {
            try
            {
                List<CompanyInsuredObject> companyInsuredObjects = DelegateService.underwritingService.GetCompanyInsuredObjectsByLineBusinessId(idLineBusiness);
                List<InsurencesObjectsViewModel> insurencesObjectsResult = new List<InsurencesObjectsViewModel>();
                
                foreach (CompanyInsuredObject item in companyInsuredObjects)
                {
                    insurencesObjectsResult.Add(Models.ModelAssembler.CreateInsurencesObjects(item));
                }
                if (insurencesObjectsResult.Count != 0)
                {
                    insurencesObjectsResult.ForEach(x => x.DeclarativeDescription = x.IsDeraclarative == true ? App_GlobalResources.Language.LabelEnabled : String.Empty);
                    return new UifJsonResult(true, insurencesObjectsResult.OrderBy(x => x.Description).ToList());
                }
                return new UifJsonResult(false, App_GlobalResources.Language.ErrMsgRangeEntityNotFound);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }
        #endregion
        

        public ActionResult GetPerilByLineBusinessId(int idLineBusiness)
        {
            try
            {
                List<Peril> insurencesObjectsEntity = DelegateService.quotationService.GetPerilsByLineBusinessId(idLineBusiness);
                List<ProtectionViewModel> protectionResult = new List<ProtectionViewModel>();

                foreach (Peril item in insurencesObjectsEntity)
                {
                    protectionResult.Add(Models.ModelAssembler.CreateProtection(item));
                }
                if (protectionResult.Count != 0)
                {
                   
                    return new UifJsonResult(true, protectionResult.OrderBy(x => x.DescriptionLong).ToList());
                }
                return new UifJsonResult(false, App_GlobalResources.Language.ErrMsgRangeEntityNotFound);
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, ex.Message);
            }
        }






        #region Protections Technical Brnach

        /// <summary>
        /// llamado a vista de Amparos para ramo tecnico
        /// </summary>
        /// <returns></returns>
        public PartialViewResult Protection()
        {
            return PartialView();
        }
        #endregion

        #region RiskType Technical Brnach

        /// <summary>
        /// Llamdo a vista de Tipos de riesgo
        /// </summary>
        /// <returns></returns>
        public PartialViewResult RiskTypeTechnicalBranch()
        {
            return PartialView();
        }


        /// <summary>
        /// action que carga los tipos de riesgo para el ramo tecnico
        /// </summary>
        /// <returns></returns>
        public ActionResult GetRiskTypeTechnicalBranch()
        {
            try
            {
                //var obj =  EnumsHelper.GetItems<CoveredRisk>();
                //IEnumerable<CoveredRisk> obj1 = new IEnumerable<CoveredRisk>();              
                List<GroupCoverage> groupCoverage = DelegateService.underwritingService.GetAllGroupCoverages();
                if (groupCoverage.Count != 0)
                {
                    //return new UifSelectResult(groupCoverage.OrderBy(x => x.Description));
                    return new UifJsonResult(true, groupCoverage.OrderBy(x => x.Description));
                }
                else
                {
                    return new UifJsonResult(false, null);
                    //return new UifSelectResult("");
                }
                //

                //ActionResult ar = null;
                ////var obj = DelegateService.UnderwritingService.GetAllGroupCoverages();
                ////var obj = DelegateService.UnderwritingService.GetRiskCommercialClass();
                //var obj = DelegateService.QuotationService.GetType();
                //return ar;
                ////return new UifJsonResult(true, obj.OrderBy(x => x.Description).ToList());
            }
            catch (System.Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetCovered_Risks_Types);
            }
        }

        public ActionResult GetRiskType()
        {
            try
            {
                return new UifJsonResult(true, EnumsHelper.GetItems<CoveredRiskType>().OrderBy(x => x.Text));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorQueryAccessType);
            }
        }
        //public UifSelectResult GetRiskTypeByPrefixId(int prefixId)
        //{
        //    try
        //    {
        //        var obj= DelegateService.
        //        List<RiskType> riskType = DelegateService.CommonService.GetRiskTypeByPrefixId(prefixId);
        //        return new UifSelectResult(riskType.OrderBy(x => x.Description));
        //    }
        //    catch (System.Exception)
        //    {

        //        return new UifSelectResult("");
        //    }
        //}
        #endregion
        /// <summary>
        /// Lista de ramo técnico con tipos de riesgo
        /// </summary>
        public void GetListLinesBusiness()
        {
            if (lineBussines.Count == 0)
            {
                lineBussines = DelegateService.commonService.GetLinesBusiness().ToList();
                lineBussines = GetRiskTypeByLineBusiness(lineBussines);
            }
        }

        /// <summary>
        /// Tipos de riesgo
        /// </summary>
        /// <param name="bussines"></param>
        /// <returns></returns>
        public List<LineBusiness> GetRiskTypeByLineBusiness(List<LineBusiness> bussines)
        {
            LineBusinessCoveredRiskType modelservice = new LineBusinessCoveredRiskType();
            List<LineBusinessCoveredRiskType> listModelService = new List<LineBusinessCoveredRiskType>();
            List<LineBusiness> bussinesService = new List<LineBusiness>();
            bussinesService = DelegateService.commonService.GetRiskTypeByLineBusinessId();
            for (int i = 0; i < bussines.Count; i++)
            {
               // bussines[i].ListLineBusinessCoveredrisktype = new List<LineBusinessCoveredRiskType>();
                foreach (var item2 in bussinesService)
                {
                    modelservice = new LineBusinessCoveredRiskType();
                    if (bussines[i].Id == item2.Id)
                    {
                        modelservice.IdLineBusiness = item2.Id;
                        modelservice.IdRiskType = item2.IdLineBusinessbyRiskType;
                     //   bussines[i].ListLineBusinessCoveredrisktype.Add(modelservice);
                    }
                }
            }
            return bussines;
        }
        /// <summary>
        /// Búsqueda avanzada, filtra por ramo técnico y tipo de riesgo
        /// </summary>
        /// <param name="LineBusinessView"></param>
        /// <returns></returns>
        public ActionResult GetLineBusinessAdvancedSearch(TechnicalBranchViewModel LineBusinessView)
        {
            try
            {
                List<LineBusiness> coveredRiskTypeSearch = new List<LineBusiness>();
                GetListLinesBusiness();
                foreach (var item in lineBussines)
                {
                    if (LineBusinessView != null)
                    {
                        if (LineBusinessView.RiskTypeId > 0)
                        {
                            //for (int i = 0; i < item.ListLineBusinessCoveredrisktype.Count; i++)
                            //{
                            //    if (item.ListLineBusinessCoveredrisktype[i].IdRiskType == LineBusinessView.RiskTypeId)
                            //    {
                            //        coveredRiskTypeSearch.Add(item);
                            //    }
                            //}
                        }
                        if (LineBusinessView.Description != null && LineBusinessView.Description != "")
                        {
                            if (item.Description.Contains(LineBusinessView.Description))
                            {
                                coveredRiskTypeSearch.Add(item);
                            }
                        }
                    }

                }
                return new UifJsonResult(true, coveredRiskTypeSearch.ToList().OrderBy(x => x.Id));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchTechnicalBranch);
            }
        }

        public ActionResult GetPerilsAssign(int Id)
        {
            try
            {
                var returnPerils = DelegateService.quotationService.GetPerilsByLineBusinessAssigned(Id);
                return new UifJsonResult(true, Models.ModelAssembler.CreateTechicalBranchProtection(returnPerils));
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchTechnicalBranch);
            }
        }

        /// <summary>
        /// Genera archivo excel ramo técnico
        /// </summary>
        /// <returns></returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                GetListLinesBusiness();
                string urlFile = DelegateService.commonService.GenerateFileLinebusiness(lineBussines, App_GlobalResources.Language.FileNameTechnicalBranch);

                if (string.IsNullOrEmpty(urlFile))
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
                else
                {
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
            }

            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
    }
}