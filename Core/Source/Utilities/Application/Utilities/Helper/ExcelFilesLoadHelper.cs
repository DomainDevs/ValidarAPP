using Sistran.Core.Framework.BAF;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Net;
using System.Text;
namespace Sistran.Core.Application.Utilities.Helper
{
    public class ExcelFilesLoadHelper
    {
        private Object thisLock = new Object();

        /// <summary>
        /// Obtiene un conexión OleDb contra un archivo de Excel.
        /// </summary>
        /// <param name="excelFilePath">Ruta del archivo de Excel.</param>
        /// <returns>Conexión de tipo OleDb.</returns>      
        public static OleDbConnection GetOleDbConnection(string excelFilePath)
        {

            OleDbConnection conn = new OleDbConnection();
            var excelConnection = ConfigurationManager.ConnectionStrings["ExcelConnection"];
            if (excelConnection == null || excelFilePath == "" || excelConnection.ConnectionString == "")
            {
                throw new ConfigurationErrorsException("La llave [ExcelConnection] o el nombre del archivo no se ha especificado o su conexion");
            }
            else
            {
                conn.ConnectionString = string.Format(ConfigurationManager.ConnectionStrings["ExcelConnection"].ConnectionString, excelFilePath);
            }
            return conn;
        }


        /// <summary>
        /// Obtiene la información de una hoja del archivo de Excel.
        /// </summary>
        /// <param name="oleDbConnection">Conexión de tipo OleDb del archivo de Excel.</param>
        /// <param name="workSheetName">Nombre de la hoja a obtener la info.</param>
        /// <returns>DataTable con la información de la hoja.</returns>
        public static DataTable GetWorkSheetInfo(OleDbConnection oleDbConnection, string cellsRange, string workSheetName)
        {
            try
            {
                OleDbDataAdapter oleDbDataAdapter = null;
                //Variable que almacenará la información de cada hoja del archivo
                //de Excel.
                DataTable workSheetInfo = null;

                //Variable que almacenará el comando a ejecutar contra cada hoja
                //del archivo de Excel.
                StringBuilder command = new StringBuilder();

                if (string.IsNullOrEmpty(workSheetName))
                {
                    DataTable tablaSheetName;

                    //Se obtiene primer nombre de la primer hoja del excel
                    tablaSheetName = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    workSheetName = tablaSheetName.Rows[0].ItemArray[2].ToString().Replace("'", "");
                    workSheetName = tablaSheetName.Rows[0].ItemArray[2].ToString().Replace("$", "");
                }


                //Se especifica el comando a ejecutar sobre la hoja.
                command.Append("Select * From [" + workSheetName.Replace("'", ""));

                //Se especifica el rango, sí se requiere.
                command.Append(string.IsNullOrEmpty(cellsRange) ? "$]" : "$" + cellsRange + "]");

                //Se especifica el comando y la conexión.
                oleDbDataAdapter = new OleDbDataAdapter(command.ToString(), oleDbConnection);

                //Se instancia el DataTable con la info. de la hoja.
                workSheetInfo = new DataTable(workSheetName.ToString());

                //Se define el esquema.
                oleDbDataAdapter.FillSchema(workSheetInfo, SchemaType.Source);

                //Se llena el DataTable con la info.
                oleDbDataAdapter.Fill(workSheetInfo);

                //Se retorna el DataTable con la info. de la hoja especificada.
                return workSheetInfo;
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en GetWorkSheetInfo", ex);
            }
        }

        /// <summary>
        /// Obtiene la info. de una hoja de un archivo de Excel.
        /// </summary>
        /// <param name="excelFilePath">Ruta del archivo de Excel.</param>
        /// <param name="errorMessage">Mensaje de error en la carga del archivo de Excel (si se genera).</param>
        /// <param name="workSheetName">Nombre de la hoja a obtener la info.</param>
        /// <param name="cellsRange">Rango de celdas a tomar. Si la cadena es vacía, se retorna la info. de toda la hoja completa.</param>
        /// <returns>DataSet con las hojas del archivo de Excel.</returns>
        public static DataTable GetExcelFileWorkSheetInfo(string excelFilePath, ref string errorMessage, string workSheetName, string cellsRange)
        {
            //Se declara la conexión de tipo OleDb para conectarse con el archivo
            //de Excel.
            OleDbConnection oleDbConnection = null;

            //Variable que almacenará los nombres de cada hoja del archivo
            //de Excel.
            DataTable workSheetInfo = null;

            try
            {
                //Se invoca el método que se encarga de obtener la conexión de tipo
                //OleDb.
                oleDbConnection = ExcelFilesLoadHelper.GetOleDbConnection(excelFilePath);

                //Se abre la conexión co el archivo de Excel.
                oleDbConnection.Open();

                //Se agrega al DataSet que almacena toda la información del
                //archivo de Excel.
                workSheetInfo = ExcelFilesLoadHelper.GetWorkSheetInfo(oleDbConnection, cellsRange, workSheetName);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                //Se invoca el método que se encarga de cerrar y liberar recursos de
                //una conexión OleDb.
                ExcelFilesLoadHelper.CloseAndDisposeOlDbConnection(oleDbConnection);
            }

            //Se retorna el DataTable con la información de la hoja del archivo de Excel.
            return workSheetInfo;
        }

        /// <summary>
        /// Cierra y libera recursos de una conexión OleDb abierta.
        /// </summary>
        /// <param name="oleDbConnection">Conexión OleDb.</param>
        public static void CloseAndDisposeOlDbConnection(OleDbConnection oleDbConnection)
        {
            try
            {
                // Se valida si la conexión es nula o no para cerrarla.
                if (oleDbConnection != null)
                {
                    //Se cierra la conexión.
                    oleDbConnection.Close();

                    //Se liberan recursos de la conexión.
                    oleDbConnection.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw new BusinessException("Error en CloseAndDisposeOlDbConnection", ex);
            }
        }


        /// <summary>
        /// Obtiene los nombres de las hoja de un archivo de Excel.
        /// </summary>
        /// <param name="excelFilePath">Ruta del archivo de Excel.</param>
        /// <param name="errorMessage">Mensaje de error en la obtención de nombres de las hojas del archivo de Excel (si se genera).</param>
        /// <returns>DataTable con los nombres de las hoja del archivo de Excel (Para obtener el nombre de la hoja: DataTable.DataRow["TABLE_NAME"]).</returns>
        public static string GetExcelFileFirstWorkSheetName(string excelFilePath)
        {
            //Se declara la conexión de tipo OleDb para conectarse con el archivo
            //de Excel.
            OleDbConnection oleDbConnection = null;

            //Variable que almacenará los nombres de cada hoja del archivo
            //de Excel.
            DataTable workSheetsNames = null;
            string workSheetName = ConfigurationManager.AppSettings["WorkSheetName"];

            try
            {
                //Se invoca el método que se encarga de obtener la conexión de tipo
                //OleDb.
                oleDbConnection = ExcelFilesLoadHelper.GetOleDbConnection(excelFilePath);

                //Se abre la conexión co el archivo de Excel.
                oleDbConnection.Open();

                //Se obtienen los nombres de cada hoja del archivo de Excel.
                workSheetsNames = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                workSheetName = workSheetsNames.Rows[0].ItemArray[2].ToString();
                workSheetName = workSheetName.Replace("$", "");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                //Se invoca el método que se encarga de cerrar y liberar recursos de
                //una conexión OleDb.
                ExcelFilesLoadHelper.CloseAndDisposeOlDbConnection(oleDbConnection);
            }

            //Se retorna el DataTable con los nombres de las hojas del archivo de Excel.
            return workSheetName;
        }


        /// <summary>
        /// Obtener Archivo Excel Comple enviando columna y fila inicial.
        /// </summary>
        /// <param name="excelFileName">Nombre  de Excel.</param>
        /// <param name="errorMessage">Error Generado</param>
        /// <param name="workSheetName">Nombre Hoja Excel</param>
        ///  <param name="RowIni">Fila Inicial Excel</param>
        ///  <param name="BeginColumn">Columna Inicial Excel</param>
        ///  <param name="EndColumn">Columna final Excel</param>             
        /// <returns>Datatabel Cargue Excel</returns>  
        public DataTable GetRemoteExcelFileWorkSheetInfoExcel(string excelFileName, ref string errorMessage, int RowIni, string BeginColumn, string EndColumn, bool html)
        {
            int maxRetries = Convert.ToInt32(ConfigurationManager.AppSettings["GetFileRetries"]);
            string user = ConfigurationManager.AppSettings["UserDomain"].ToString();
            string password = ConfigurationManager.AppSettings["DomainPassword"].ToString();
            string domain = ConfigurationManager.AppSettings["Domain"].ToString();
            string destFileDir = ConfigurationManager.AppSettings["LocalMassiveLoadPath"];
            string excelRemotePaht = ConfigurationManager.AppSettings["ExternalFolderFiles"];
            string sourceFilePath = excelRemotePaht + @"\" + excelFileName;
            string destFilePath = destFileDir + @"\" + excelFileName;
            string CellsRange = "";
            Uri uri = new Uri(excelRemotePaht, UriKind.Absolute);

            int retries = 0;
            while (retries < maxRetries)
            {
                try
                {
                    using (NetworkConnection networkCon = new NetworkConnection(uri.LocalPath, new NetworkCredential(user, password, domain)))
                    {
                        if (networkCon._resultConnection == 1219 || networkCon._resultConnection == 0)
                        {
                            File.Copy(sourceFilePath, destFilePath, true);
                            break;
                        }
                        else
                        {
                            throw new Win32Exception(networkCon._resultConnection);
                        }
                    }
                }
                catch (Exception ex)
                {
                    retries++;
                    if (retries == maxRetries)
                    {
                        throw new BusinessException("Error en GetRemoteExcelFileWorkSheetInfoExcel", ex);
                    }
                }
            }
            //Se declara la conexión de tipo OleDb para conectarse con el archivo
            //de Excel.
            OleDbConnection oleDbConnection = null;

            //Variable que almacenará los nombres de cada hoja del archivo
            //de Excel.
            DataTable workSheetInfo = null;

            try
            {
                //Se invoca el método que se encarga de obtener la conexión de tipo
                //OleDb.
                oleDbConnection = ExcelFilesLoadHelper.GetOleDbConnection(destFilePath);

                //Se abre la conexión co el archivo de Excel.
                oleDbConnection.Open();

                //Se agrega al DataSet que almacena toda la información del
                //archivo de Excel.
                workSheetInfo = ExcelFilesLoadHelper.GetWorkSheetInfo(oleDbConnection, null, null);
                CellsRange = BeginColumn + RowIni.ToString() + ":" + EndColumn + workSheetInfo.Rows.Count.ToString();
                workSheetInfo = ExcelFilesLoadHelper.GetWorkSheetInfo(oleDbConnection, CellsRange, null);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                //Se invoca el método que se encarga de cerrar y liberar recursos de
                //una conexión OleDb.
                ExcelFilesLoadHelper.CloseAndDisposeOlDbConnection(oleDbConnection);
            }

            //Se retorna el DataTable con la información de la hoja del archivo de Excel.
            return workSheetInfo;
        }

        /// <summary>
        /// Obtener Archivo Excel Comple enviando columna y fila inicial.
        /// </summary>
        /// <param name="excelFileName">Nombre  de Excel.</param>
        /// <param name="errorMessage">Error Generado</param>
        /// <param name="workSheetName">Nombre Hoja Excel</param>
        ///  <param name="RowIni">Fila Inicial Excel</param>
        ///  <param name="BeginColumn">Columna Inicial Excel</param>
        ///  <param name="EndColumn">Columna final Excel</param>             
        ///  <param name="userName">Nombre de Usuario</param>   
        /// <returns>Datatabel Cargue Excel</returns>  
        public DataTable GetRemoteExcelFileWorkSheetInfoExcelUser(string excelFileName, ref string errorMessage, int RowIni, string BeginColumn, string EndColumn, string userName)
        {
            int maxRetries = Convert.ToInt32(ConfigurationManager.AppSettings["GetFileRetries"]);
            string user = ConfigurationManager.AppSettings["UserDomain"].ToString();
            string password = ConfigurationManager.AppSettings["DomainPassword"].ToString();
            string domain = ConfigurationManager.AppSettings["Domain"].ToString();
            string destFileDir = ConfigurationManager.AppSettings["LocalMassiveLoadPath"];
            string excelRemotePaht = ConfigurationManager.AppSettings["ExternalFolderFiles"];
            string sourceFilePath = excelRemotePaht + @"\" + userName + @"\" + excelFileName;

            if (!Directory.Exists(destFileDir + @"\" + userName))
            {
                Directory.CreateDirectory(destFileDir + @"\" + userName);
            }

            string destFilePath = destFileDir + @"\" + userName + @"\" + excelFileName;

            string CellsRange = "";
            Uri uri = new Uri(excelRemotePaht, UriKind.Absolute);

            int retries = 0;
            while (retries < maxRetries)
            {
                try
                {
                    using (NetworkConnection networkCon = new NetworkConnection(uri.LocalPath, new NetworkCredential(user, password, domain)))
                    {
                        if (networkCon._resultConnection == 1219 || networkCon._resultConnection == 0)
                        {
                            File.Copy(sourceFilePath, destFilePath, true);
                            break;
                        }
                        else
                        {
                            throw new Win32Exception(networkCon._resultConnection);
                        }
                    }
                }
                catch (Exception ex)
                {
                    retries++;
                    if (retries == maxRetries)
                        throw ex;
                }
            }
            //Se declara la conexión de tipo OleDb para conectarse con el archivo
            //de Excel.
            OleDbConnection oleDbConnection = null;

            //Variable que almacenará los nombres de cada hoja del archivo
            //de Excel.
            DataTable workSheetInfo = null;

            try
            {
                //Se invoca el método que se encarga de obtener la conexión de tipo
                //OleDb.
                oleDbConnection = ExcelFilesLoadHelper.GetOleDbConnection(destFilePath);

                //Se abre la conexión co el archivo de Excel.
                oleDbConnection.Open();

                workSheetInfo = ExcelFilesLoadHelper.GetWorkSheetInfo(oleDbConnection, null, null);
                CellsRange = BeginColumn + RowIni.ToString() + ":" + EndColumn + workSheetInfo.Rows.Count.ToString();
                workSheetInfo = ExcelFilesLoadHelper.GetWorkSheetInfo(oleDbConnection, CellsRange, null);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                ExcelFilesLoadHelper.CloseAndDisposeOlDbConnection(oleDbConnection);
            }

            //Se retorna el DataTable con la información de la hoja del archivo de Excel.
            return workSheetInfo;
        }


        /// <summary>
        /// Obtiene la info. de una hoja de un archivo de Excel.
        /// </summary>
        /// <param name="pathExcelFile">Ruta del archivo de Excel, Carpeta de Usuario y Nombre de Excel</param>
        public DataTable GetExcelFileWorkSheetInfoByRemotePath(string pathExcelFile)
        {
            OleDbConnection oleDbConnection = null;
            DataTable workSheetInfo = null;
            try
            {
                if (pathExcelFile != "")
                {
                    lock (thisLock)
                    {
                        oleDbConnection = ExcelFilesLoadHelper.GetOleDbConnection(pathExcelFile);
                        oleDbConnection.Open();
                        workSheetInfo = ExcelFilesLoadHelper.GetWorkSheetInfo(oleDbConnection, null, null);
                    }
                }
                else
                {
                    throw new Exception("No existe la Ruta del Archivo");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                if (oleDbConnection != null && oleDbConnection.State != ConnectionState.Closed)
                {
                    ExcelFilesLoadHelper.CloseAndDisposeOlDbConnection(oleDbConnection);
                }
            }

            return workSheetInfo;
        }

    }
}
