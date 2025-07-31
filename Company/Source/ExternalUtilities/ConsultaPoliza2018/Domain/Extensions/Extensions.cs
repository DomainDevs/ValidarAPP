using Domain.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Domain.Models.Entities;
using Sybase.Data.AseClient;
using Newtonsoft.Json;

namespace Domain.Extensions
{
    public static class Extensions
    {
        public static void LoadDataForDataSet(this CustomResult objeto, DataSet ds)
        {
            var iteratorTables = ds.Tables.GetEnumerator();
            while (iteratorTables.MoveNext())
            {
                var dt = (DataTable)iteratorTables.Current;
                if (dt.Rows.Count > 0 && dt.Columns.Contains("TableName"))
                {
                    var iteratorRows = dt.Rows.GetEnumerator();
                    var nombreEntidad = string.Empty;
                    while (iteratorRows.MoveNext())
                    {
                        var dr = (DataRow)iteratorRows.Current;
                        if (nombreEntidad.Equals(string.Empty))
                        {
                            nombreEntidad = dr["TableName"].ToString();
                            nombreEntidad = "Domain.Models.Entities." + nombreEntidad.ToUpper().Replace(".", "");
                        }

                        if (nombreEntidad != null)
                        {
                            System.Diagnostics.Debug.WriteLine(nombreEntidad);
                            var tipo = Type.GetType(nombreEntidad);
                            if (tipo != null)
                            {

                                var entidad = (IEntityBasic)Activator.CreateInstance(tipo);
                                entidad.Load(dr);

                                objeto.CargaEnLista(entidad);

                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("NO RESOLVIO EL TIPO: " + nombreEntidad);
                            }

                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("ERROR EN NOMBRE DE ENTIDAD ");
                        }
                    }
                }
            }
        }
        private static void CargaEnLista(this CustomResult objeto, IEntityBasic obj)
        {
            var nombre = obj.GetType().ToString();

            nombre = nombre.Substring(nombre.LastIndexOf('.') + 1);

            switch (nombre)
            {
                case "COMMBRANCH": if (objeto.COMMBRANCH == null) { objeto.COMMBRANCH = new List<COMMBRANCH>(); } objeto.COMMBRANCH.Add((COMMBRANCH)obj); break;
                case "COMMPREFIX": if (objeto.COMMPREFIX == null) { objeto.COMMPREFIX = new List<COMMPREFIX>(); } objeto.COMMPREFIX.Add((COMMPREFIX)obj); break;
                case "ISSPOLICY": if (objeto.ISSPolicy == null) { objeto.ISSPolicy = new List<ISSPolicy>(); } objeto.ISSPolicy.Add((ISSPolicy)obj); break;
                case "ISSCOPOLICY": if (objeto.ISSCOPolicy == null) { objeto.ISSCOPolicy = new List<ISSCOPolicy>(); } objeto.ISSCOPolicy.Add((ISSCOPolicy)obj); break;
                case "ISSENDORSEMENT": if (objeto.ISSENDORSEMENT == null) { objeto.ISSENDORSEMENT = new List<ISSENDORSEMENT>(); } objeto.ISSENDORSEMENT.Add((ISSENDORSEMENT)obj); break;
                case "ISSCO_ENDORSEMENT": if (objeto.ISSCO_ENDORSEMENT == null) { objeto.ISSCO_ENDORSEMENT = new List<ISSCO_ENDORSEMENT>(); } objeto.ISSCO_ENDORSEMENT.Add((ISSCO_ENDORSEMENT)obj); break;
                case "ISSCPT_ENDORSEMENT": if (objeto.ISSCPT_ENDORSEMENT == null) { objeto.ISSCPT_ENDORSEMENT = new List<ISSCPT_ENDORSEMENT>(); } objeto.ISSCPT_ENDORSEMENT.Add((ISSCPT_ENDORSEMENT)obj); break;
                case "ISSGROUP_ENDORSEMENT": if (objeto.ISSGROUP_ENDORSEMENT == null) { objeto.ISSGROUP_ENDORSEMENT = new List<ISSGROUP_ENDORSEMENT>(); } objeto.ISSGROUP_ENDORSEMENT.Add((ISSGROUP_ENDORSEMENT)obj); break;
                case "ISSCOINSURANCE_ACCEPT_ACCEPTED": if (objeto.ISSCOINSURANCE_ACCEPT_ACCEPTED == null) { objeto.ISSCOINSURANCE_ACCEPT_ACCEPTED = new List<ISSCOINSURANCE_ACCEPT_ACCEPTED>(); } objeto.ISSCOINSURANCE_ACCEPT_ACCEPTED.Add((ISSCOINSURANCE_ACCEPT_ACCEPTED)obj); break;
                case "ISSCOINSURANCE_ASSIGNED": if (objeto.ISSCOINSURANCE_ASSIGNED == null) { objeto.ISSCOINSURANCE_ASSIGNED = new List<ISSCOINSURANCE_ASSIGNED>(); } objeto.ISSCOINSURANCE_ASSIGNED.Add((ISSCOINSURANCE_ASSIGNED)obj); break;
                case "ISSPOLICY_AGENT": if (objeto.ISSPOLICY_AGENT == null) { objeto.ISSPOLICY_AGENT = new List<ISSPOLICY_AGENT>(); } objeto.ISSPOLICY_AGENT.Add((ISSPOLICY_AGENT)obj); break;
                case "ISSCOMMISS_AGENT": if (objeto.ISSCOMMISS_AGENT == null) { objeto.ISSCOMMISS_AGENT = new List<ISSCOMMISS_AGENT>(); } objeto.ISSCOMMISS_AGENT.Add((ISSCOMMISS_AGENT)obj); break;
                case "ISSPOLICY_CLAUSE": if (objeto.ISSPOLICY_CLAUSE == null) { objeto.ISSPOLICY_CLAUSE = new List<ISSPOLICY_CLAUSE>(); } objeto.ISSPOLICY_CLAUSE.Add((ISSPOLICY_CLAUSE)obj); break;
                case "ISSENDORSEMENT_PAYER": if (objeto.ISSENDORSEMENT_PAYER == null) { objeto.ISSENDORSEMENT_PAYER = new List<ISSENDORSEMENT_PAYER>(); } objeto.ISSENDORSEMENT_PAYER.Add((ISSENDORSEMENT_PAYER)obj); break;
                case "ISSFIRST_PAY_COMP": if (objeto.ISSFIRST_PAY_COMP == null) { objeto.ISSFIRST_PAY_COMP = new List<ISSFIRST_PAY_COMP>(); } objeto.ISSFIRST_PAY_COMP.Add((ISSFIRST_PAY_COMP)obj); break;
                case "ISSPAYER_PAYMENT": if (objeto.ISSPAYER_PAYMENT == null) { objeto.ISSPAYER_PAYMENT = new List<ISSPAYER_PAYMENT>(); } objeto.ISSPAYER_PAYMENT.Add((ISSPAYER_PAYMENT)obj); break;
                case "ISSCO_PAYER_PAYMENT_COMP": if (objeto.ISSCO_PAYER_PAYMENT_COMP == null) { objeto.ISSCO_PAYER_PAYMENT_COMP = new List<ISSCO_PAYER_PAYMENT_COMP>(); } objeto.ISSCO_PAYER_PAYMENT_COMP.Add((ISSCO_PAYER_PAYMENT_COMP)obj); break;
                case "ISSCO_PAYER_PAYMENT_COMPSum": if (objeto.ISSCO_PAYER_PAYMENT_COMPSum == null) { objeto.ISSCO_PAYER_PAYMENT_COMPSum = new List<ISSCO_PAYER_PAYMENT_COMPSum>(); } objeto.ISSCO_PAYER_PAYMENT_COMPSum.Add((ISSCO_PAYER_PAYMENT_COMPSum)obj); break;
                case "ISSRISK": if (objeto.ISSRISK == null) { objeto.ISSRISK = new List<ISSRISK>(); } objeto.ISSRISK.Add((ISSRISK)obj); break;
                case "ISSCO_RISK": if (objeto.ISSCO_RISK == null) { objeto.ISSCO_RISK = new List<ISSCO_RISK>(); } objeto.ISSCO_RISK.Add((ISSCO_RISK)obj); break;
                case "ISSCPT_RISK": if (objeto.ISSCPT_RISK == null) { objeto.ISSCPT_RISK = new List<ISSCPT_RISK>(); } objeto.ISSCPT_RISK.Add((ISSCPT_RISK)obj); break;
                case "ISSCO_CPT_RISK_INFRINGEMENT": if (objeto.ISSCO_CPT_RISK_INFRINGEMENT == null) { objeto.ISSCO_CPT_RISK_INFRINGEMENT = new List<ISSCO_CPT_RISK_INFRINGEMENT>(); } objeto.ISSCO_CPT_RISK_INFRINGEMENT.Add((ISSCO_CPT_RISK_INFRINGEMENT)obj); break;
                case "ISSRISK_VEHICLE": if (objeto.ISSRISK_VEHICLE == null) { objeto.ISSRISK_VEHICLE = new List<ISSRISK_VEHICLE>(); } objeto.ISSRISK_VEHICLE.Add((ISSRISK_VEHICLE)obj); break;
                case "ISSCO_RISK_VEHICLE": if (objeto.ISSCO_RISK_VEHICLE == null) { objeto.ISSCO_RISK_VEHICLE = new List<ISSCO_RISK_VEHICLE>(); } objeto.ISSCO_RISK_VEHICLE.Add((ISSCO_RISK_VEHICLE)obj); break;
                case "ISSRISK_VEHICLE_DRIVE": if (objeto.ISSRISK_VEHICLE_DRIVE == null) { objeto.ISSRISK_VEHICLE_DRIVE = new List<ISSRISK_VEHICLE_DRIVE>(); } objeto.ISSRISK_VEHICLE_DRIVE.Add((ISSRISK_VEHICLE_DRIVE)obj); break;
                case "ISSRISK_BENEFICIARY": if (objeto.ISSRISK_BENEFICIARY == null) { objeto.ISSRISK_BENEFICIARY = new List<ISSRISK_BENEFICIARY>(); } objeto.ISSRISK_BENEFICIARY.Add((ISSRISK_BENEFICIARY)obj); break;
                case "ISSRISK_CLAUSE": if (objeto.ISSRISK_CLAUSE == null) { objeto.ISSRISK_CLAUSE = new List<ISSRISK_CLAUSE>(); } objeto.ISSRISK_CLAUSE.Add((ISSRISK_CLAUSE)obj); break;
                case "ISSRISK_SURETY": if (objeto.ISSRISK_SURETY == null) { objeto.ISSRISK_SURETY = new List<ISSRISK_SURETY>(); } objeto.ISSRISK_SURETY.Add((ISSRISK_SURETY)obj); break;
                case "ISSCO_RISK_SURETY": if (objeto.ISSCO_RISK_SURETY == null) { objeto.ISSCO_RISK_SURETY = new List<ISSCO_RISK_SURETY>(); } objeto.ISSCO_RISK_SURETY.Add((ISSCO_RISK_SURETY)obj); break;
                case "ISSRISK_LOCATION": if (objeto.ISSRISK_LOCATION == null) { objeto.ISSRISK_LOCATION = new List<ISSRISK_LOCATION>(); } objeto.ISSRISK_LOCATION.Add((ISSRISK_LOCATION)obj); break;
                case "ISSRISK_SURETY_GUARANTEE": if (objeto.ISSRISK_SURETY_GUARANTEE == null) { objeto.ISSRISK_SURETY_GUARANTEE = new List<ISSRISK_SURETY_GUARANTEE>(); } objeto.ISSRISK_SURETY_GUARANTEE.Add((ISSRISK_SURETY_GUARANTEE)obj); break;
                case "ISSENDO_RISK_COVERAGE": if (objeto.ISSENDO_RISK_COVERAGE == null) { objeto.ISSENDO_RISK_COVERAGE = new List<ISSENDO_RISK_COVERAGE>(); } objeto.ISSENDO_RISK_COVERAGE.Add((ISSENDO_RISK_COVERAGE)obj); break;
                case "ISSRISK_COVERAGESum": if (objeto.ISSRISK_COVERAGESum == null) { objeto.ISSRISK_COVERAGESum = new List<ISSRISK_COVERAGESum>(); } objeto.ISSRISK_COVERAGESum.Add((ISSRISK_COVERAGESum)obj); break;
                case "ISSRISK_COVERAGE": if (objeto.ISSRISK_COVERAGE == null) { objeto.ISSRISK_COVERAGE = new List<ISSRISK_COVERAGE>(); } objeto.ISSRISK_COVERAGE.Add((ISSRISK_COVERAGE)obj); break;
                case "ISSRISK_COVER_DEDUCT": if (objeto.ISSRISK_COVER_DEDUCT == null) { objeto.ISSRISK_COVER_DEDUCT = new List<ISSRISK_COVER_DEDUCT>(); } objeto.ISSRISK_COVER_DEDUCT.Add((ISSRISK_COVER_DEDUCT)obj); break;
                case "ISSRISK_COVER_CLAUSE": if (objeto.ISSRISK_COVER_CLAUSE == null) { objeto.ISSRISK_COVER_CLAUSE = new List<ISSRISK_COVER_CLAUSE>(); } objeto.ISSRISK_COVER_CLAUSE.Add((ISSRISK_COVER_CLAUSE)obj); break;
                case "COMPONENTES": if (objeto.COMPONENTES == null) { objeto.COMPONENTES = new List<COMPONENTES>(); } objeto.COMPONENTES.Add((COMPONENTES)obj); break;
                case "ISSPAYER_COMP": if (objeto.ISSPAYER_COMP == null) { objeto.ISSPAYER_COMP = new List<ISSPAYER_COMP>(); } objeto.ISSPAYER_COMP.Add((ISSPAYER_COMP)obj); break;
                case "ISSRISK_COVER_DETAIL": if (objeto.ISSRISK_COVER_DETAIL == null) { objeto.ISSRISK_COVER_DETAIL = new List<ISSRISK_COVER_DETAIL>(); } objeto.ISSRISK_COVER_DETAIL.Add((ISSRISK_COVER_DETAIL)obj); break;
                case "ISSRISK_DETAIL_ACCESSORY": if (objeto.ISSRISK_DETAIL_ACCESSORY == null) { objeto.ISSRISK_DETAIL_ACCESSORY = new List<ISSRISK_DETAIL_ACCESSORY>(); } objeto.ISSRISK_DETAIL_ACCESSORY.Add((ISSRISK_DETAIL_ACCESSORY)obj); break;
                case "ISSRISK_DETAIL": if (objeto.ISSRISK_DETAIL == null) { objeto.ISSRISK_DETAIL = new List<ISSRISK_DETAIL>(); } objeto.ISSRISK_DETAIL.Add((ISSRISK_DETAIL)obj); break;
                case "ISSRISK_COVER_DETAIL_DEDUCT": if (objeto.ISSRISK_COVER_DETAIL_DEDUCT == null) { objeto.ISSRISK_COVER_DETAIL_DEDUCT = new List<ISSRISK_COVER_DETAIL_DEDUCT>(); } objeto.ISSRISK_COVER_DETAIL_DEDUCT.Add((ISSRISK_COVER_DETAIL_DEDUCT)obj); break;
                case "ISSRISK_DETAIL_DESCRIPTION": if (objeto.ISSRISK_DETAIL_DESCRIPTION == null) { objeto.ISSRISK_DETAIL_DESCRIPTION = new List<ISSRISK_DETAIL_DESCRIPTION>(); } objeto.ISSRISK_DETAIL_DESCRIPTION.Add((ISSRISK_DETAIL_DESCRIPTION)obj); break;
                default: throw new NotImplementedException();
            }
        }
        public static string ToJSON(this CustomResult objeto)
        {
            return JsonConvert.SerializeObject(objeto, Formatting.Indented);
        }
        public static string PublicInstancePropertiesEqual(this CustomResult self, CustomResult to, params string[] ignore) /*where T : class*/
        {
            var resume = string.Empty;

            if (self != null && to != null)
            {
                Type type = self.GetType();//typeof(T);
                List<string> ignoreList = new List<string>(ignore);
                foreach (System.Reflection.PropertyInfo pi in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    if (!ignoreList.Contains(pi.Name))
                    {

                        var selfValue = convertidor(type.GetProperty(pi.Name).GetValue(self, null));
                        var toValue = convertidor(type.GetProperty(pi.Name).GetValue(to, null));

                        resume += string.Concat("\n", string.Format(" Atributo: [{0}] ", pi.Name).PadRight(80, '=').ToUpper(), "\n");

                        if (selfValue == null && toValue == null)
                        {
                            resume += string.Concat(string.Format(" Falto entidad **{0}** en Póliza 1 y Póliza 2", pi.Name).ToUpper(), "\n");
                        }
                        else if (selfValue == null)
                        {
                            resume += string.Concat(string.Format(" Falto entidad **{0}** en Póliza 1 ", pi.Name).ToUpper(), "\n");
                        }
                        else if (toValue == null)
                        {
                            resume += string.Concat(string.Format(" Falto entidad **{0}** en Póliza 2 ", pi.Name).ToUpper(), "\n");
                        }else if (selfValue != null && toValue != null)
                        {

                            var max = selfValue.Count;
                            if (max < toValue.Count)
                                max = toValue.Count;

                            for (int i = 0; i < max; i++)
                            {
                                var selfObject = (IEntityBasic)(selfValue.Count > i ? selfValue.ToArray()[i] : null); //.PublicInstancePropertiesEqual(toValue.ToArray()[i]);
                                var toObject = (IEntityBasic)(toValue.Count > i ? toValue.ToArray()[i] : null);

                                resume += string.Concat("\n", string.Format(" Comparando las entidades **{0}** en posición {1} ", pi.Name, i).ToUpper(), "\n");

                                if (selfObject == null)
                                {
                                    resume += string.Concat(string.Format(" Falto entidad **{0}** en la Póliza 1, en la posición {1} ", pi.Name, i).ToUpper(), "\n");
                                    selfObject = new EntityBasicClass();
                                }
                                else if (toObject == null)
                                {
                                    resume += string.Concat(string.Format(" Falto entidad **{0}** en la Póliza 2, en la posición {1} ", pi.Name, i).ToUpper(), "\n");
                                }

                                resume += selfObject.PublicInstancePropertiesEqual(toObject, ignore);
                            }
                        }
                    }
                }
            }

            return resume;
        }
        private static IList<IEntityBasic> convertidor(object o)
        {
            IList<IEntityBasic> respuesta;

            if (o != null)
            {

                if (o.GetType().FullName.Contains(".COMMBRANCH,"))
                {
                    respuesta = ((List<COMMBRANCH>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".COMMPREFIX,"))
                {
                    respuesta = ((List<COMMPREFIX>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSPolicy,"))
                {
                    respuesta = ((List<ISSPolicy>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSCOPolicy,"))
                {
                    respuesta = ((List<ISSCOPolicy>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSENDORSEMENT,"))
                {
                    respuesta = ((List<ISSENDORSEMENT>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSCO_ENDORSEMENT,"))
                {
                    respuesta = ((List<ISSCO_ENDORSEMENT>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSGROUP_ENDORSEMENT,"))
                {
                    respuesta = ((List<ISSGROUP_ENDORSEMENT>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSPOLICY_AGENT,"))
                {
                    respuesta = ((List<ISSPOLICY_AGENT>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSCOMMISS_AGENT,"))
                {
                    respuesta = ((List<ISSCOMMISS_AGENT>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSPOLICY_CLAUSE,"))
                {
                    respuesta = ((List<ISSPOLICY_CLAUSE>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSENDORSEMENT_PAYER,"))
                {
                    respuesta = ((List<ISSENDORSEMENT_PAYER>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSPAYER_PAYMENT,"))
                {
                    respuesta = ((List<ISSPAYER_PAYMENT>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSRISK,"))
                {
                    respuesta = ((List<ISSRISK>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSCO_RISK,"))
                {
                    respuesta = ((List<ISSCO_RISK>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSRISK_VEHICLE,"))
                {
                    respuesta = ((List<ISSRISK_VEHICLE>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSCO_RISK_VEHICLE,"))
                {
                    respuesta = ((List<ISSCO_RISK_VEHICLE>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSRISK_BENEFICIARY,"))
                {
                    respuesta = ((List<ISSRISK_BENEFICIARY>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSENDO_RISK_COVERAGE,"))
                {
                    respuesta = ((List<ISSENDO_RISK_COVERAGE>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSRISK_COVERAGE,"))
                {
                    respuesta = ((List<ISSRISK_COVERAGE>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSRISK_COVER_DEDUCT,"))
                {
                    respuesta = ((List<ISSRISK_COVER_DEDUCT>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSPAYER_COMP,"))
                {
                    respuesta = ((List<ISSPAYER_COMP>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSCOINSURANCE_ACCEPT_ACCEPTED,"))
                {
                    respuesta = ((List<ISSCOINSURANCE_ACCEPT_ACCEPTED>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSCOINSURANCE_ASSIGNED,"))
                {
                    respuesta = ((List<ISSCOINSURANCE_ASSIGNED>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSFIRST_PAY_COMP,"))
                {
                    respuesta = ((List<ISSFIRST_PAY_COMP>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSCO_PAYER_PAYMENT_COMP,"))
                {
                    respuesta = ((List<ISSCO_PAYER_PAYMENT_COMP>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSCO_PAYER_PAYMENT_COMPSum,"))
                {
                    respuesta = ((List<ISSCO_PAYER_PAYMENT_COMPSum>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSRISK_VEHICLE_DRIVE,"))
                {
                    respuesta = ((List<ISSRISK_VEHICLE_DRIVE>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSRISK_CLAUSE,"))
                {
                    respuesta = ((List<ISSRISK_CLAUSE>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSRISK_SURETY,"))
                {
                    respuesta = ((List<ISSRISK_SURETY>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSCO_RISK_SURETY,"))
                {
                    respuesta = ((List<ISSCO_RISK_SURETY>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSRISK_LOCATION,"))
                {
                    respuesta = ((List<ISSRISK_LOCATION>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSRISK_SURETY_GUARANTEE,"))
                {
                    respuesta = ((List<ISSRISK_SURETY_GUARANTEE>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSRISK_COVERAGESum,"))
                {
                    respuesta = ((List<ISSRISK_COVERAGESum>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSRISK_COVER_CLAUSE,"))
                {
                    respuesta = ((List<ISSRISK_COVER_CLAUSE>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".COMPONENTES,"))
                {
                    respuesta = ((List<COMPONENTES>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSRISK_COVER_DETAIL,"))
                {
                    respuesta = ((List<ISSRISK_COVER_DETAIL>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSRISK_DETAIL_ACCESSORY,"))
                {
                    respuesta = ((List<ISSRISK_DETAIL_ACCESSORY>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSRISK_DETAIL,"))
                {
                    respuesta = ((List<ISSRISK_DETAIL>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSRISK_COVER_DETAIL_DEDUCT,"))
                {
                    respuesta = ((List<ISSRISK_COVER_DETAIL_DEDUCT>)o).Cast<IEntityBasic>().ToList();
                }
                else if (o.GetType().FullName.Contains(".ISSRISK_DETAIL_DESCRIPTION,"))
                {
                    respuesta = ((List<ISSRISK_DETAIL_DESCRIPTION>)o).Cast<IEntityBasic>().ToList();
                }
                else
                {
                    respuesta = null;
                }
            }
            else
            {
                respuesta = null;
            }

            return respuesta;
        }
        public static string PublicInstancePropertiesEqual(this IEntityBasic self, IEntityBasic to, params string[] ignore) /*where T : class*/
        {
            var resume = string.Empty;

            if (self != null && to != null)
            {
                Type type = self.GetType();
                List<string> ignoreList = new List<string>(ignore);
                foreach (System.Reflection.PropertyInfo pi in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {

                    if (!ignoreList.Contains(pi.Name))
                    {
                        object selfValue = type.GetProperty(pi.Name).GetValue(self, null);
                        object toValue = type.GetProperty(pi.Name).GetValue(to, null);

                        if (selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue)))
                        {
                            resume += string.Format(" {0} son diferentes, En la Póliza 1: {1} vs la Póliza 2: {2} \n", pi.Name, selfValue, toValue);
                        }
                    }
                }
            }
            return resume;
        }


        private static void SetItemFromRow<T>(T item, DataRow row) where T : new()
        {
            // go through each column
            foreach (DataColumn c in row.Table.Columns)
            {
                // find the property for the column
                PropertyInfo p = item.GetType().GetProperty(c.ColumnName);

                // if exists, set the value
                if (p != null && row[c] != DBNull.Value)
                {
                    p.SetValue(item, row[c], null);
                }
            }
        }

        public static AseParameter CreateParameter(this AseCommand o, string nombre, AseDbType tipo, ParameterDirection direccion, Object valor)
        {
            var respuesta = o.CreateParameter();
            respuesta.ParameterName = nombre;
            respuesta.AseDbType = tipo;
            respuesta.Direction = direccion;
            respuesta.Value = valor;
            return respuesta;
        }

        public static DbParameter CreateParameter(this DbCommand o, string nombre, DbType tipo, ParameterDirection direccion, Object valor)
        {
            var respuesta = o.CreateParameter();
            respuesta.ParameterName = nombre;
            respuesta.DbType = tipo;
            respuesta.Direction = direccion;
            respuesta.Value = valor;
            return respuesta;
        }

        //public static AseConnection ConnectionStringSet(this AseConnection o, string ConnectionStringName)
        //{
        //    var respuesta = new AseConnection();
        //    var connectionString = System.Configuration..ConnectionStrings[ConnectionStringName].ConnectionString;
        //    respuesta.ConnectionString = connectionString;
        //    return respuesta;
        //}

        public static async Task GetResultAsync(this CustomResult objeto, long DOCUMENT_NUM, int BRANCH_CD, int PREFIX_CD, DBInfo dbInfo)
        {
            var ds = new DataSet();
            var parametros = new Dictionary<string, object>();

            try
            {
                //var conexion = "HolaMundo";
                var storeProcedure = "INT.GET_INFO_POLICY";
                parametros.Add("DOCUMENT_NUM", DOCUMENT_NUM);
                parametros.Add("BRANCH_CD", BRANCH_CD);
                parametros.Add("PREFIX_CD", PREFIX_CD);

                var dataAccess = new DataAccessClass(dbInfo.servidor, dbInfo.basededatos, dbInfo.usuario, dbInfo.password, dbInfo.baseDeDatos).MethodFactory();

                ds = dataAccess.consultarSP(storeProcedure, parametros);

                objeto.LoadDataForDataSet(ds);

            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Write(e);
            }
        }
    }
}