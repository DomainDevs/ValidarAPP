using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Text;
using Sistran.Core.Framework.UIF.Web.Models;
using Sistran.Core.Application.CommonService.Enums;
using Sistran.Core.Framework.UIF.Web.Services;
using System.Configuration;
using System.IO;

namespace Sistran.Core.Framework.UIF.Web.Areas.Common.Controllers
{
    [Authorize]
    public class MainController : Controller
    {
        private static List<Sistran.Core.Application.CommonService.Models.Parameter> parameterType = new List<Sistran.Core.Application.CommonService.Models.Parameter>();
        [NoDirectAccess]
        public ActionResult Main()
        {
            return View();
        }

        public ActionResult GetParameters()
        {
            try
            {
                if (parameterType.Count == 0)
                {
                    parameterType.Add(new Sistran.Core.Application.CommonService.Models.Parameter { Description = "PersonType", Value = DelegateService.commonService.GetParameterByParameterId((int)ParametersTypes.PersonType).NumberParameter.Value });
                }

                return new UifJsonResult(true, parameterType);
            }
            catch (Exception)
            {
                return new UifJsonResult(false, App_GlobalResources.Language.ErrorSearchParameter);
            }
        }

        /// <summary>
        /// Realiza la descarga de un archivo
        /// </summary>
        /// <param name="fileName">Nombre del archivo en el servidor</param>
        /// <param name="fileNameDownload">nombre de archivo a descargar</param>
        /// <returns></returns>
        [HttpGet]
        public FileResult DownloadFile(string fileName, string fileNameDownload)
        {
            var externalPath = string.Empty;
            byte[] fileBytes;
            string dateNow = DateTime.Now.ToString("dd/MM/yyyy");
            string fileExtension = Path.GetExtension(fileName);

            Response.AddHeader("content-disposition", "attachement;filename =" + fileNameDownload + "(" + dateNow + ")" + Path.GetExtension(fileName));

            try
            {
                externalPath = string.Format("{0}\\{1}", DelegateService.commonService.GetKeyApplication("ExternalFolderFiles"), fileName);

                switch (fileExtension)
                {
                    case ".csv":
                        fileBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(System.IO.File.ReadAllText(externalPath));
                        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet);
                    case ".xlsx":
                        MemoryStream memoryStream = new MemoryStream(); 
                        using (FileStream fileStream = System.IO.File.OpenRead(externalPath))
                        {
                            memoryStream.SetLength(fileStream.Length);
                            fileStream.Read(memoryStream.GetBuffer(), 0, (int)fileStream.Length);
                        }
                        return File(memoryStream, "Excel");
                    default:
                        fileBytes = new byte[0];
                        return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet);
                }
            }
            catch (Exception)
            {
                var bytes = new byte[0];
                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet);
            }
            finally
            {
                if (System.IO.File.Exists(externalPath))
                {
                    System.IO.File.Delete(externalPath);
                }
            }
        }
    }
}