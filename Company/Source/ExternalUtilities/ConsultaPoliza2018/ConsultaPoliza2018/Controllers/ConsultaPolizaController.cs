using ConsultaPoliza2018.Models.Entities;
using Domain.Models.Entities;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ConsultaPoliza2018.Controllers
{
    public class ConsultaPolizaController : Controller
    {
        //ContextAxa db = new ContextAxa();
        // GET: ConsultaPoliza
        public async Task<ActionResult> Index()
        {
            try
            {
                CustomResult custom = await Task.FromResult(GetResult(1001059, 94, 10));
                CustomResult customTwo = await Task.FromResult(GetResult(1001059, 94, 10));


            }
            catch (Exception exc)
            {
                throw;
            }


            return View();
        }




        // GET: ConsultaPoliza/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ConsultaPoliza/Create
        public ActionResult Create()
        {
            return View(new SearchModel());
        }

        // POST: ConsultaPoliza/Create
        [HttpPost]
        public async Task<ActionResult> Create(SearchModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    CustomResult custom =
                        await Task.FromResult(GetResult(model.DOCUMENT_NUM, model.BRANCH_CD, model.PREFIX_CD));
                    CustomResult customTwo =
                        await Task.FromResult(GetResult(model.DOCUMENT_NUM_CompareTo, model.BRANCH_CD, model.PREFIX_CD));

                    ViewBag.customOne = custom;
                    ViewBag.customTwo = customTwo;
                    model.Itero = true;
                }
                else
                {
                    return View();
                }
                return View(model);
            }
            catch (Exception exc)
            {
                throw;
            }


        }

        // GET: ConsultaPoliza/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ConsultaPoliza/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ConsultaPoliza/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ConsultaPoliza/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public CustomResult GetResult(long DOCUMENT_NUM, int BRANCH_CD, int PREFIX_CD)
        {
            DbContext bd = new DbContext("DBContext");
            var command = bd.Database.Connection.CreateCommand();

            command.CommandText = "dbo.SP_ComparePoliza2018_2";

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter
            {
                Value = DOCUMENT_NUM,
                DbType = DbType.Decimal,
                ParameterName = "@DOCUMENT_NUM"
            });
            command.Parameters.Add(new SqlParameter
            {
                Value = BRANCH_CD,
                DbType = DbType.Int16,
                ParameterName = "@BRANCH_CD"
            });
            command.Parameters.Add(new SqlParameter
            {
                Value = PREFIX_CD,
                DbType = DbType.Int16,
                ParameterName = "@PREFIX_CD"
            });
            command.Parameters.Add(new SqlParameter
            {
                Value = 0,
                DbType = DbType.Int16,
                ParameterName = "@END_DOCUMENT_NUM"
            });

            CustomResult custom = new CustomResult();
            try
            {
                bd.Database.Connection.Open();
                var reader = command.ExecuteReader();

                //string noprop = "";
                //while (reader.NextResult())
                //{
                //    Type type = custom.GetType();
                //    PropertyInfo[] fields = type.GetProperties()
                //        .Where(x => !noprop.Split(' ').Contains(x.Name))
                //        .ToArray();
                //    if (reader.HasRows)
                //    {
                //        foreach (var prop in fields)
                //        {
                //            custom.GetType().GetProperty(prop.Name)
                //                .SetValue(custom,
                //                ((IObjectContextAdapter)bd).ObjectContext.Translate<TypeOf(prop)>(reader).ToList());
                //            noprop += $"{prop.Name} ";
                //            break;
                //        }
                //    }


                //}
                /*
                var dt = new DataTable();
                dt.Load(reader);*/


                var ds = new DataSet();
                using (var r = command.ExecuteReader())
                {
                    ds.Load(r, LoadOption.OverwriteChanges, new string[] { "COMM.BRANCH", "COMM.PREFIX", "ISS.POLICY", "ISS.CO_POLICY", "ISS.ENDORSEMENT", "ISS.CO_ENDORSEMENT", /*"ISS.CPT_ENDORSEMENT",*/ "ISS.GROUP_ENDORSEMENT", "ISS.COINSURANCE_ACCEPTED", "ISS.COINSURANCE_ASSIGNED", "ISS.POLICY_AGENT", "ISS.COMMISS_AGENT", "ISS.POLICY_CLAUSE", "ISS.ENDORSEMENT_PAYER", "ISS.FIRST_PAY_COMP", "ISS.PAYER_PAYMENT", "ISS.CO_PAYER_PAYMENT_COMP", "", "ISS.RISK", "ISS.CO_RISK", /*"ISS.CPT_RISK",*/ "ISS.ENDORSEMENT_RISK", "ISS.RISK_VEHICLE", "ISS.CO_RISK_VEHICLE", "ISS.RISK_VEHICLE_DRIVER", "ISS.RISK_BENEFICIARY", "ISS.RISK_CLAUSE", "ISS.RISK_SURETY", "ISS.CO_RISK_SURETY", "ISS.RISK_LOCATION", "ISS.RISK_SURETY_GUARANTEE", "ISS.ENDO_RISK_COVERAGE", "ISS.RISK_COVERAGE", "ISS.RISK_COVERAGE_2", "ISS.RISK_COVER_DEDUCT", "ISS.RISK_COVER_CLAUSE", "ISS.PAYER_COMP", "ISS.PAYER_COMP_2", "ISS.RISK_COVER_DETAIL", "ISS.RISK_DETAIL_ACCESSORY", "ISS.RISK_DETAIL", "ISS.RISK_COVER_DETAIL_DEDUCT", "ISS.RISK_DETAIL_DESCRIPTION" });
                }

                if (reader.HasRows)
                {
                    custom.COMMBRANCH = ((IObjectContextAdapter)bd).ObjectContext.Translate<COMMBRANCH>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                    custom.COMMPREFIX = ((IObjectContextAdapter)bd).ObjectContext.Translate<COMMPREFIX>(reader).ToList();
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSPolicy = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSPolicy>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    //custom.ISSCOPolice = ((IObjectContextAdapter)bd)
                    //    .ObjectContext.Translate<ISSCOPolice>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSENDORSEMENT = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSENDORSEMENT>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSCO_ENDORSEMENT = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSCO_ENDORSEMENT>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSCPT_ENDORSEMENT = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSCPT_ENDORSEMENT>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSGROUP_ENDORSEMENT = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSGROUP_ENDORSEMENT>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSCOINSURANCE_ACCEPT_ACCEPTED = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSCOINSURANCE_ACCEPT_ACCEPTED>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSCOINSURANCE_ASSIGNED = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSCOINSURANCE_ASSIGNED>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSPOLICY_AGENT = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSPOLICY_AGENT>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSCOMMISS_AGENT = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSCOMMISS_AGENT>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSPOLICY_CLAUSE = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSPOLICY_CLAUSE>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSENDORSEMENT_PAYER = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSENDORSEMENT_PAYER>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSFIRST_PAY_COMP = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSFIRST_PAY_COMP>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSPAYER_PAYMENT = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSPAYER_PAYMENT>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSCO_PAYER_PAYMENT_COMP = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSCO_PAYER_PAYMENT_COMP>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSCO_PAYER_PAYMENT_COMPSum = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSCO_PAYER_PAYMENT_COMPSum>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSRISK = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSRISK>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSCO_RISK = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSCO_RISK>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSCPT_RISK = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSCPT_RISK>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSCO_CPT_RISK_INFRINGEMENT = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSCO_CPT_RISK_INFRINGEMENT>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSRISK_VEHICLE = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSRISK_VEHICLE>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSCO_RISK_VEHICLE = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSCO_RISK_VEHICLE>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSRISK_VEHICLE_DRIVE = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSRISK_VEHICLE_DRIVE>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSRISK_BENEFICIARY = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSRISK_BENEFICIARY>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSRISK_CLAUSE = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSRISK_CLAUSE>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSRISK_SURETY = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSRISK_SURETY>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSCO_RISK_SURETY = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSCO_RISK_SURETY>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSRISK_LOCATION = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSRISK_LOCATION>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSRISK_SURETY_GUARANTEE = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSRISK_SURETY_GUARANTEE>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSENDO_RISK_COVERAGE = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSENDO_RISK_COVERAGE>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSRISK_COVERAGESum = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSRISK_COVERAGESum>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSRISK_COVERAGE = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSRISK_COVERAGE>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSRISK_COVER_DEDUCT = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSRISK_COVER_DEDUCT>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSRISK_COVER_CLAUSE = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSRISK_COVER_CLAUSE>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.COMPONENTES = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<COMPONENTES>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSPAYER_COMP = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSPAYER_COMP>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSRISK_COVER_DETAIL = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSRISK_COVER_DETAIL>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSRISK_DETAIL_ACCESSORY = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSRISK_DETAIL_ACCESSORY>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSRISK_DETAIL = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSRISK_DETAIL>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSRISK_COVER_DETAIL_DEDUCT = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSRISK_COVER_DETAIL_DEDUCT>(reader).ToList();
                }
                reader.NextResult();

                if (reader.HasRows)
                {
                    custom.ISSRISK_DETAIL_DESCRIPTION = ((IObjectContextAdapter)bd)
                        .ObjectContext.Translate<ISSRISK_DETAIL_DESCRIPTION>(reader).ToList();
                }
            }
            catch (Exception e)
            {
                throw;
            }
            finally
            {
                bd.Database.Connection.Close();
            }

            return custom;
        }
                

    }

}
