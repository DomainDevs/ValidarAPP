using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sistran.Co.Application.Data;
using Sistran.Co.Cardif.Application.IntegrationServices.Models;

namespace Sistran.Co.Cardif.Application.IntegrationProvider.Provider
{
    public class Estationary
    {
        public Estationary() { }
        /// <summary>
        /// Executa el procedimiento Papeleria_borrado_tablas_papeleria_pivot
        /// </summary>
        public void ETLPapeleria_borrado_tablas_papeleria_pivot(DynamicDataAccess pdb) 
        {
            try
            {
                pdb.ExecuteSPSerDataSetNoTransaction("Papeleria_borrado_tablas_papeleria_pivot");
            }
            catch (Exception ex) 
            {
                throw new Exception(String.Concat("Error en el método ETLPapeleria_borrado_tablas_papeleria_pivot ",
                    ex.Message));
            }
        }
        /// <summary>
        /// Executa el procedimiento papeleria_seleccionar_talmacensoat
        /// </summary>
        public EntityETLPapeleriaAlmacenSoatResponse ETLPapeleria_seleccionar_talmacensoatt(DynamicDataAccess pdb)
        {
            try
            {
                EntityETLPapeleriaAlmacenSoatResponse response = new EntityETLPapeleriaAlmacenSoatResponse();
                response.lstEntityETLPapeleriaAlmacenSoat =
                    ConvertidorListas<EntityETLPapeleriaAlmacenSoatRequest>.DataTableToList(
                pdb.ExecuteSPSerDataSetNoTransaction("papeleria_seleccionar_talmacensoat").Tables[0]);

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Concat("Error en el método ETLPapeleria_seleccionar_talmacensoatt ",
                    ex.Message));
            }
        }
        /// <summary>
        /// Executa el procedimiento papeleria_insertar_talmacen_soat_wkf
        /// </summary>
        public void ETLPapeleria_insertar_talmacen_soat_wkf(EntityETLPapeleriaAlmacenSoatResponse response, DynamicDataAccess pdb)
        {
            try
            {
                ETLHelper etl = new ETLHelper();
                foreach (EntityETLPapeleriaAlmacenSoatRequest request in response.lstEntityETLPapeleriaAlmacenSoat) 
                {
                    etl.EjecutarProcedimiento(pdb, request, "papeleria_insertar_talmacen_soat_wkf");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Concat("Error en el método ETLPapeleria_borrado_tablas_papeleria_pivot ",
                    ex.Message));
            }
        }
        /// <summary>
        /// Executa el procedimiento papeleria_seleccionar_tproc_soat_wkf
        /// </summary>
        public EntityETLProcSoatResponse ETLPapeleria_seleccionar_tproc_soat_wkf(DynamicDataAccess pdb)
        {
            try
            {
                EntityETLProcSoatResponse response = new EntityETLProcSoatResponse();
                response.lstEntityETLProcSoat =
                    ConvertidorListas<EntityETLProcSoatRequest>.DataTableToList(
                pdb.ExecuteSPSerDataSetNoTransaction("papeleria_seleccionar_tproc_soat_wkf").Tables[0]);

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Concat("Error en el método ETLpapeleria_seleccionar_tproc_soat_wkf ",
                    ex.Message));
            }
        }
        /// <summary>
        /// Executa el procedimiento papeleria_insertar_talmacen_soat_wkf
        /// </summary>
        public void ETLpapeleria_insertar_tproc_soat_wkf_sise(EntityETLProcSoatResponse response, DynamicDataAccess pdb)
        {
            try
            {
                ETLHelper etl = new ETLHelper();
                foreach (EntityETLProcSoatRequest request in response.lstEntityETLProcSoat)
                {
                    etl.EjecutarProcedimiento(pdb, request, "papeleria_insertar_tproc_soat_wkf_sise");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Concat("Error en el método ETLPapeleria_insertar_tproc_soat_wkf_sise ",
                    ex.Message));
            }
        }
        /// <summary>
        /// Executa el procedimiento sp_ejecuta_integra_papeleria
        /// </summary>
        public void ETLsp_ejecuta_integra_papeleria(DynamicDataAccess pdb)
        {
            try
            {
                pdb.ExecuteSPSerDataSetNoTransaction("sp_ejecuta_integra_papeleria");
            }
            catch (Exception ex)
            {
                throw new Exception(String.Concat("Error en el método ETLsp_ejecuta_integra_papeleria ",
                    ex.Message));
            }
        }




    }
}
