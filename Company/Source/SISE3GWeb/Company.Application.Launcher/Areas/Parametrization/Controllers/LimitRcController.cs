
namespace Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Sistran.Core.Framework.UIF.Web.Areas.Parametrization.Models;
    using Sistran.Core.Framework.UIF.Web.Models;
    using Sistran.Core.Framework.UIF.Web.Services;
    using Sistran.Core.Application.EntityServices.Models;
    using Sistran.Core.Application.EntityServices.Enums;
    using Sistran.Core.Application.ModelServices.Models.UnderwritingParam;
    using ENUMUTIL = Sistran.Core.Services.UtilitiesServices.Enums;
    using ENUMSM = Sistran.Core.Application.ModelServices.Enums;
    using Sistran.Core.Application.ModelServices.Models.Param;

    /// <summary>
    /// 
    /// </summary>
    public class LimitRcController : Controller
    {
        /// <summary>
        /// Parametros de entidad 
        /// </summary>
        private PostEntity postEntity = new PostEntity { EntityType = "Sistran.Core.Application.Common.Entities.CoLimitsRc", KeyType = KeyType.None };

        #region Propiedades

        /// <summary>
        /// Modelo Limite Rc
        /// </summary>
        private static List<LimitRcViewModel> ltslimitRcViewModel = new List<LimitRcViewModel>();

        /// <summary>
        /// Ultimo is de limite Rc
        /// </summary>
        private static int codLimitRcLast = 0;

        /// <summary>
        /// Contador  Guardado
        /// </summary>
        int contador = 0; 

        #endregion

        /// <summary>
        /// Abrir pantalla principal
        /// </summary>
        /// <returns>Retorna vista principal</returns>
        public ActionResult LimitRc()
        {
            LimitRcViewModel viewModelLimit = new LimitRcViewModel();
            viewModelLimit.Limit1 = 0;
            viewModelLimit.Limit2 = 0;
            viewModelLimit.Limit3 = 0;
            viewModelLimit.LimitUnique = 0;                        
            return View(viewModelLimit);
        }

        /// <summary>
        /// Obtiene todos los limites rc
        /// </summary>
        /// <returns>Retorna listado de limites rc</returns>
        public ActionResult GetLimitsRc()
        {
            try
            {
                this.GetListLimitRc();
                return new UifJsonResult(true, ltslimitRcViewModel.OrderBy(x => x.LimitRcCd).ToList());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetLimitRc);
            }
        }

        /// <summary>
        /// Busca limite Rc por descripcion
        /// </summary>
        /// <param name="LimitRcSearch">Descripcion de busqueda</param>
        /// <returns>Retorna lista de modelo limite Rc</returns>
        public ActionResult GetLimitRcByDescription(string LimitRcSearch)
        {
            try
            {
                List<LimitRcViewModel> ltslimitRcByDescription = new List<LimitRcViewModel>();
                foreach (LimitRcViewModel item in ltslimitRcViewModel)
                {
                    if (item.Limit1 == Convert.ToDecimal(LimitRcSearch) || item.Limit2 == Convert.ToDecimal(LimitRcSearch) ||
                item.Limit3 == Convert.ToDecimal(LimitRcSearch) || item.LimitUnique == Convert.ToDecimal(LimitRcSearch))
                    {
                        ltslimitRcByDescription.Add(item);
                    }
                }
                return new UifJsonResult(true, ltslimitRcByDescription.OrderBy(x => x.LimitRcCd).ToList());
            }
            catch (Exception ex)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGetLimitRc);
            }
        }

        /// <summary>
        /// Metodo para (Guardar,Eliminar,Editar) los limites rc
        /// </summary>
        /// <param name="limitRcViewModel">Lista de LimitRcViewModel</param>
        /// <returns>Retorna lista de limites rc</returns>
        public ActionResult ExecuteOperations(List<LimitRcViewModel> limitRcViewModel)
        {
            string messageErrors = string.Empty;
            int add = 0;
            int edit = 0;
            int delete = 0;

            List<Field> fields = new List<Field>();
            contador = 0;

            foreach (LimitRcViewModel item in limitRcViewModel)
            {
                try
                {
                    if ((ENUMUTIL.ParametrizationStatus)item.StatusTypeService == ENUMUTIL.ParametrizationStatus.Delete)
                    {
                        int hasDependencies = DelegateService.UnderwritingParamServiceWeb.ValidateLimitc(item.LimitRcCd);

                        if (hasDependencies == 0)
                        {
                            fields = new List<Field>();
                            fields.Add(new Field { Name = "LimitRcCode", Value = item.LimitRcCd.ToString() });
                            this.postEntity.Fields = fields;
                            DelegateService.EntityServices.Delete(this.postEntity);
                            delete += 1;
                        }
                        else
                        {
                            if (messageErrors != string.Empty)
                            {
                                messageErrors += " ";
                            }

                            messageErrors += string.Format(App_GlobalResources.Language.ErrorDeleteWithDependenciesLimitRc, item.LimitRcCd);
                        }
                    }
                    else
                    {
                        this.AssignPostEntity(item);
                        if (this.postEntity.Status == StatusTypeService.Create)
                        {
                            this.postEntity = DelegateService.EntityServices.Create(this.postEntity);
                            Field field = this.postEntity.Fields.First(x => x.Name == "LimitRcCode");
                            if (field.Value != "0")
                            {
                                add += 1;
                            }
                        }
                        else if (this.postEntity.Status == StatusTypeService.Update)
                        {
                            DelegateService.EntityServices.Update(this.postEntity);
                            edit += 1;
                        }
                    }
                }
                catch (Exception ex)
                {
                    messageErrors = messageErrors + " " + string.Format(ex.Message + " " + item.LimitRcCd + "<br/>");
                }
            }

            ltslimitRcViewModel = new List<LimitRcViewModel>();
            this.GetLimitsRc();

            string message = string.Empty;
            if (add > 0)
            {
                message += string.Format(App_GlobalResources.Language.ReturnSaveAdded, add);
            }

            if (edit > 0)
            {
                if (messageErrors != string.Empty)
                {
                    messageErrors += " ";
                }

                message += string.Format(App_GlobalResources.Language.ReturnSaveEdited, edit);
            }

            if (delete > 0)
            {
                if (messageErrors != string.Empty)
                {
                    messageErrors += " ";
                }

                message += string.Format(App_GlobalResources.Language.ReturnSaveDeleted, delete);
            }

            if (messageErrors != string.Empty)
            {
                if (messageErrors != string.Empty)
                {
                    messageErrors += " ";
                }

                message += messageErrors;
            }

            var result = ltslimitRcViewModel.OrderBy(x => x.LimitRcCd).ToList();
            return new UifJsonResult(true, new { message = message, data = result });
        }

        /// <summary>
        /// Asigna los campos a la entidad
        /// </summary>
        /// <param name="item"></param>
        private void AssignPostEntity(LimitRcViewModel item)
        {
            List<Field> fields = new List<Field>();
            
            if (item.LimitRcCd == 0)
            {
                if (contador == 0)
                {
                    fields.Add(new Field
                    {
                        Name = "LimitRcCode",
                        Value = codLimitRcLast.ToString(),
                        Type = new FieldType { Name = "System.Int32" }
                    });
                }
                else
                {
                    codLimitRcLast = codLimitRcLast + 1;
                    fields.Add(new Field
                    {
                        Name = "LimitRcCode",
                        Value = codLimitRcLast.ToString(),
                        Type = new FieldType { Name = "System.Int32" }
                    });
                }
                contador++;
            }
            else
            {
                fields.Add(new Field
                {
                    Name = "LimitRcCode",
                    Value = item.LimitRcCd.ToString(),
                    Type = new FieldType { Name = "System.Int32" }
                });
            }

            fields.Add(new Field { Name = "Limit1", Value = item.Limit1.ToString() });
            fields.Add(new Field { Name = "Limit2", Value = item.Limit2.ToString() });
            fields.Add(new Field { Name = "Limit3", Value = item.Limit3.ToString() });
            fields.Add(new Field { Name = "LimitUnique", Value = item.LimitUnique.ToString() });
            fields.Add(new Field { Name = "Description", Value = item.Description.ToString() });
            fields.Add(new Field { Name = "StatusTypeService", Value = item.StatusTypeService.ToString() });

            this.postEntity.Fields = fields;
            this.postEntity.KeyType = KeyType.None;
            this.postEntity.Status = item.StatusTypeService;
        }

        /// <summary>
        /// Obtiene listado de limites rc
        /// </summary>
        /// <returns>Retorna lista limites rc</returns>
        private List<LimitRcViewModel> GetListLimitRc()
        {
            if (ltslimitRcViewModel.Count == 0)
            {
                LimitsRcServiceModel limitsRcServiceModel = DelegateService.UnderwritingParamServiceWeb.GetLimitsRc();
                if (limitsRcServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    ltslimitRcViewModel = ModelAssembler.CreateLimitRc(limitsRcServiceModel.LimitRcModel);
                    codLimitRcLast = ltslimitRcViewModel.LastOrDefault().LimitRcCd + 1;
                    return ltslimitRcViewModel.OrderBy(x => x.LimitRcCd).ToList();
                }
                else
                {
                    return null;
                }
            }

            return ltslimitRcViewModel;
        }

        /// <summary>
        /// Genera archivo excel de coberturas
        /// </summary>
        /// <returns>Archivo excel</returns>
        public ActionResult GenerateFileToExport()
        {
            try
            {
                this.GetLimitsRc();
                ExcelFileServiceModel excelFileServiceModel = DelegateService.UnderwritingParamServiceWeb.GenerateFileToLimitRc(ModelAssembler.CreateLimitsRc(ltslimitRcViewModel), App_GlobalResources.Language.LabelLimitRC);
                if (excelFileServiceModel.ErrorTypeService == ENUMSM.ErrorTypeService.Ok)
                {
                    var urlFile = excelFileServiceModel.FileData;
                    return new UifJsonResult(true, DelegateService.commonService.GetKeyApplication("TransferProtocol") + urlFile);
                }
                else
                {
                    return new UifJsonResult(false, App_GlobalResources.Language.ErrorFileNotFound);
                }
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorGeneratingFile);
            }
        }
    }
}