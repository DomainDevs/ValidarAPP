using Domain.Extensions;
using Domain.Models.Entities;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace ConsultaPoliza2018.Controllers
{
    public class PolizaController : Controller
    {
        [HttpGet]
        public ActionResult Index() //SearchPolicies modelo)
        {
            ViewBag.Title = "Comparar Pólizas";

            this.llenarCombos();

            return View(new SearchPolicies()); //View(modelo);
        }

        [HttpPost]
        public ActionResult Index(SearchPolicies modelo, string valor)
        {
            if (!ModelState.IsValid) {
                //this.llenarCombos();
                return this.Index();
            }               
            return RedirectToAction("Compare", new RouteValueDictionary(modelo));
        }

        public async Task<ActionResult> Compare(SearchPolicies modelo)
        {
            var result = new ResultCompare();
            var poliza = new CustomResult();
            var poliza2 = new CustomResult();

            if (ModelState.IsValid)
            {
                result.polizaNum = modelo.PolizaNum;

                var tipoConexion =  (modelo.tipoConexion1.Equals(1) ? Domain.DataAccess.BaseDeDatos.SqlServer : Domain.DataAccess.BaseDeDatos.Sybase);                
                await poliza.GetResultAsync(modelo.PolizaNum, modelo.Ramo, modelo.Sucursal, new DBInfo() {  basededatos = modelo.basededatos1, password = modelo.password1, servidor = modelo.servidor1, usuario = modelo.usuario1, baseDeDatos = tipoConexion } );
                result.poliza = poliza.ToJSON();

                if (modelo.PolizaNum2.HasValue)
                {
                    result.polizaNum2 = modelo.PolizaNum2.Value;
                    tipoConexion = (modelo.tipoConexion2.Equals(1) ? Domain.DataAccess.BaseDeDatos.SqlServer : Domain.DataAccess.BaseDeDatos.Sybase);
                    await poliza2.GetResultAsync(modelo.PolizaNum2.Value, modelo.Ramo, modelo.Sucursal, new DBInfo() { basededatos = modelo.basededatos2, password = modelo.password2, servidor = modelo.servidor2, usuario = modelo.usuario2, baseDeDatos = tipoConexion });
                    result.poliza2 = poliza2.ToJSON();

                    result.resume = poliza.PublicInstancePropertiesEqual(poliza2, new string[] { "DOCUMENT_NUM", "POLICY_ID", "ENDORSEMENT_ID", "PAYER_ID" });
                }                
            }
            else
            {
                //return RedirectToAction("Index", new RouteValueDictionary(modelo));
                return RedirectToAction("Index");
            }
            return View(result);

        }

        public async Task<ActionResult> Temporal()
        {

            var poliza = new Models.Entities.Poliza();
            poliza.GetResultAsync();
            return View(poliza);
        }

        private void llenarCombos()
        {
            var items = new Items[] {
                new Items() { Value = 1, Text = "SQL Server" },
                new Items() { Value = 2, Text = "SyBase" } };
            var list = new SelectList(items, "Value", "Text");

            ViewData["lista"] = list;
        }
    }
}