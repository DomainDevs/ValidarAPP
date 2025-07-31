using Sistran.Company.Application.UploadFileServices.Models;
using Sistran.Company.Services.CollectiveUnderwritingServices.EEProvider.Providers;
using Sistran.Company.Services.UploadFileServices.Enums;
using Sistran.Core.Application.LoggerUp;
using Sistran.Core.Framework.BAF;
using Sistran.Core.Framework.BAF.Application;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Sistran.Company.Services.UploadFileServices.EEProvider.DAOs
{

    public class ExecFinishEventArgs : EventArgs
    {
        /// <summary>
        /// Obtiene o establece un valor que indica si se presentó error.
        /// </summary>
        /// <value>
        ///   <c>Verdadero si hubo error; de lo contrario, falso.
        /// </value>
        public bool Error { get; set; }

        /// <summary>
        /// Obtiene o establece el texto del mensaje.
        /// </summary>
        /// <value>
        /// Texto del mensaje.
        /// </value>
        public string Message { get; set; }


        /// <summary>
        /// Instancia para la clase ExecFinishEventArgs.
        /// </summary>
        public ExecFinishEventArgs()
        {
            Error = false;
            Message = string.Empty;
        }
    }

    public class countRecordsByDelegate
    {
        public delegate void ProcespolDelegate(int registros, out int registrosCount);
        private ProcespolDelegate procespolDelegate;
        public int threadId { get; set; }
        public int recordsNumber { get; set; }
        public ProcespolDelegate delegado
        {
            get
            {

                return procespolDelegate;
            }
        }
        public countRecordsByDelegate(ProcespolDelegate p)
        {
            procespolDelegate = p;
        }
    }


    public class MassiveLoadThreadDAO : Provider
    {
        public delegate void ExecFinishHandler(object sender, ExecFinishEventArgs e);
        private volatile int countMassiveLoadThreads = 0;
        private Logger logger = new Logger();
        private bool writeLog = false;
        public System.Threading.Thread esteThread;
        private IList Treas = new System.Collections.Generic.List<countRecordsByDelegate>();
        private Stack stack = new Stack();
        public event ExecFinishHandler ExecFinish;
        private DataTable Table;
        private static object objLock = new object();
        private volatile int totalRows;
        private volatile int recordsCount;
        private volatile static bool stopping = false;
        private volatile bool shouldStop;
        private volatile int countRowAux;
        private volatile int Fin;
        private volatile bool processInit = false;
        private volatile int threadIni;
        private volatile int packageId = 0;
        private volatile int packageInit = 0;
        private volatile int CountThead = 0;
        private object packageLock = new object();
        private int ConReg;
        private List<MassiveLoadVehicleReception> Mle = new List<MassiveLoadVehicleReception>();

        public MassiveLoadThreadDAO(IRequestProcessor requestProcessor)
            : base(requestProcessor)
        {

        }

        public void setMassiveLoadProcess(MassiveLoadProcess massiveLoadProcess)
        {
            massivelp = massiveLoadProcess;
        }

        public void RequestStop()
        {
            shouldStop = true;
        }

        private MassiveLoadProcess massivelp = new MassiveLoadProcess();
        public ActionContext Context;


        /// <summary>
        /// Initializa el proceso de excel.
        /// Recorre la información del excel y genera un string con los valores
        /// </summary>
        /// <param name="userName">Nombre de usuario.</param>
        /// <param name="fieldSetId">Id del ramo.</param>
        /// <returns></returns>
        public List<MassiveLoadVehicleReception> InitExcelProcess(string userName, int fieldSetId, int tempId)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string pattern = ConfigurationManager.AppSettings["pattern"];

            try
            {
                string errorMessage1 = string.Empty;

                bool writeLogEvents = false;
                Boolean.TryParse(ConfigurationManager.AppSettings.Get("WriteLogEvents"), out writeLogEvents);
                writeLog = writeLogEvents;
                DataTable MassLoad;
                totalRows = 0;
                recordsCount = 0;
                Fin = 0;
                massivelp.MaxThread = Convert.ToInt32(ConfigurationManager.AppSettings["MaxThreadCollective"]);
                massivelp.MaxRowsPerThread = Convert.ToInt32(ConfigurationManager.AppSettings["MaxRowsPerThread"]);
                int initialRowExcel = Convert.ToInt32(ConfigurationManager.AppSettings["InitialRowExcel"]);
                int characterPerString = Convert.ToInt32(ConfigurationManager.AppSettings["CharacterPerString"]);
                MassiveDAO massiveDAO = new MassiveDAO();
                MassiveLoadFieldSet massiveLoadFieldSet = massiveDAO.GetMassiveLoadFieldSetByFieldSetIdPrefixCd(fieldSetId, 0);

                massivelp.BeginColumn = massiveLoadFieldSet.BeginColumn;
                massivelp.EndColumn = massiveLoadFieldSet.EndColumn;
                string CellsRange = string.Empty;


                int processId = massiveDAO.SaveAsynchronousProcess(massivelp.UserId, massivelp.MassiveLoadId);
                massiveDAO.SaveMassiveCollectiveRelation(massivelp.TemporalId, massivelp.MassiveLoadId, processId, massivelp.UserId, 0);

                MassLoad = null;

                int packageRowLast = initialRowExcel - 1 + massivelp.MaxRowsPerThread;

                totalRows = MassLoad.Rows.Count;
                StringBuilder sbQryValues = new StringBuilder();
                StringBuilder auxSbQryValues = new StringBuilder();
                int countRow = initialRowExcel;
                int countNew = countRow;
                int n = MassLoad.Columns.Count;
                int r = MassLoad.Rows.Count;
                int i = 0;



                string valueColumn = "";
                int fieldLength;
                int maxFieldLength;

                countRowAux = 0;

                threadIni = Thread.CurrentThread.ManagedThreadId;
                sbQryValues.Append("VALUES ");
                Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
                ConReg = countRow;

                List<MassiveLoadFields> lstFields = massiveDAO.GetMassiveLoadFields();

                List<MassiveLoadFields> lstFieldsMap = massiveDAO.GetMassiveLoadFieldsByFieldSetId(fieldSetId);


                // 18/12/2015 - Carlos Rebeiz Quiceno Becerra - Colpatria : Configuracion validacion errores
                string errorExists = "";
                string errorCharacter = "";
                string errorLenght = "";
                string msjErrorExists = "";
                string msjErrorCharacter = "";
                string msjErrorLenght = "";
                bool hasErrorExists = false;
                bool hasErrorCharacter = false;
                bool hasErrorLenght = false;
                int numberError = 0;
                string msjTotalErrors = "";
                string errorField = "";

                errorLenght = ConfigurationManager.AppSettings["ErrorLenght"];
                errorCharacter = ConfigurationManager.AppSettings["errorCharacter"];
                errorExists = ConfigurationManager.AppSettings["errorExists"];
                // 18/12/2015 - Carlos Rebeiz Quiceno Becerra

                foreach (DataRow dr in MassLoad.Rows)
                {
                    CountThead = CountThead + 1;
                    if (!(Convert.IsDBNull(dr[0]) || Convert.ToString(dr[0]) == ""))
                    {

                        if (sbQryValues.Length <= characterPerString)
                        {
                            if ((int)FieldSet.EXCLUSIONRIESGOS == fieldSetId)
                            {
                                auxSbQryValues.Append(string.Format("({0},{1},", massivelp.MassiveLoadId, countRow));
                            }
                            else
                            {
                                auxSbQryValues.Append(string.Format("({0},{1},{2},", massivelp.MassiveLoadId, countRow, 1));
                            }

                            hasErrorLenght = false;
                            hasErrorCharacter = false;
                            msjErrorLenght = "";
                            msjErrorCharacter = "";
                            numberError = 0;

                            for (i = 0; i < n; i++)
                            {
                                // 18/12/2015 - Carlos Rebeiz Quiceno Becerra - Colpatria : Quitamos el caracter invalido del campo
                                valueColumn = dr[i].ToString();
                                fieldLength = valueColumn.Length;
                                // 18/12/2015 - Carlos Rebeiz Quiceno Becerra

                                if ((int)FieldSet.EXCLUSIONRIESGOS == fieldSetId)
                                {
                                    errorField = lstFieldsMap[i + 2].MassiveFieldDescription;
                                    maxFieldLength = lstFields.Where(x => x.MassiveFieldName == lstFieldsMap[i + 2].MassiveFieldName).Select(x => x.FieldLong.GetValueOrDefault(0)).FirstOrDefault();
                                }
                                else
                                {
                                    errorField = lstFieldsMap[i + 3].MassiveFieldDescription;
                                    maxFieldLength = lstFields.Where(x => x.MassiveFieldName == lstFieldsMap[i + 3].MassiveFieldName).Select(x => x.FieldLong.GetValueOrDefault(0)).FirstOrDefault();
                                }

                                // 18/12/2015 - Carlos Rebeiz Quiceno Becerra - Colpatria : Quitamos el caracter invalido del campo
                                if (valueColumn.Contains('|'))
                                {
                                    valueColumn = valueColumn.Substring(1, valueColumn.Length - 1);
                                }
                                // 18/12/2015 - Carlos Rebeiz Quiceno Becerra

                                // validacion tamaño maximo de la columna
                                if (fieldLength > maxFieldLength)
                                {
                                    hasErrorLenght = true;
                                    msjErrorLenght += errorLenght + " " + errorField + ", ";
                                    valueColumn = dr[i].ToString().Substring(0, maxFieldLength);
                                    numberError = 1;
                                }

                                // validacion caracter invalido de la columna
                                if (reg.IsMatch(valueColumn))
                                {
                                    hasErrorCharacter = true;
                                    msjErrorCharacter += errorCharacter + " " + errorField + ", ";
                                    valueColumn = "";
                                    numberError = 1;
                                }

                                // validacion existencia
                                if (((int)FieldSet.EXCLUSIONRIESGOS == fieldSetId) && (massiveDAO.GetRiskExcludeCountByMassiveId(massivelp.MassiveLoadId, valueColumn, massivelp.TemporalId) > 0))
                                {
                                    hasErrorExists = true;
                                    numberError = 1;
                                }

                                auxSbQryValues.Append("'" + valueColumn + "',");
                            }

                            if (hasErrorLenght && hasErrorCharacter)
                            {
                                msjTotalErrors = msjErrorLenght + " - " + msjErrorCharacter;
                            }
                            else if (hasErrorLenght)
                            {
                                msjTotalErrors = msjErrorLenght;
                            }
                            else if (hasErrorCharacter)
                            {
                                msjTotalErrors = msjErrorCharacter;
                            }
                            else if (hasErrorExists)
                            {
                                msjTotalErrors = msjErrorExists;
                            }

                            auxSbQryValues.Append("'" + numberError + "','" + msjTotalErrors + "','processId')");
                            msjTotalErrors = "";
                            // FIN VALIDACION ESTRUCTURA Y FORMATOS ERRORES EN EL CARGUE



                            if (countRow > 0 && (countRow % massivelp.MaxRowsPerThread) == 0)
                            {
                                processId = massiveDAO.SaveAsynchronousProcess(massivelp.UserId, massivelp.MassiveLoadId);
                                massiveDAO.SaveMassiveCollectiveRelation(massivelp.TemporalId, massivelp.MassiveLoadId, processId, massivelp.UserId, 0);

                            }

                            if (auxSbQryValues.Length + sbQryValues.Length + 1 <= characterPerString)
                            {
                                sbQryValues.Append(auxSbQryValues.ToString());
                                sbQryValues.Append(",");
                                if (sbQryValues.Length > characterPerString)
                                {
                                    string values = sbQryValues.ToString();
                                    values = (values.EndsWith(",")) ? values.Trim(new char[] { ',' }) : values;
                                    packageRowLast = initialRowExcel + countRow;
                                    packageId = packageId + 1;
                                    InitThread(values, countRow, packageRowLast, countRowAux, processId);
                                    initialRowExcel = packageRowLast + 1;
                                    countRowAux = 0;
                                    sbQryValues = new StringBuilder();
                                    sbQryValues.Append("VALUES ");
                                }
                            }
                            else
                            {
                                string values = sbQryValues.ToString();
                                values = (values.EndsWith(",")) ? values.Trim(new char[] { ',' }) : values;
                                packageRowLast = initialRowExcel + countRow;
                                packageId = packageId + 1;
                                InitThread(values, countRow, packageRowLast, countRowAux, processId);
                                initialRowExcel = packageRowLast + 1;
                                countRowAux = 0;
                                sbQryValues = new StringBuilder();
                                sbQryValues.Append("VALUES ");
                                sbQryValues.Append(auxSbQryValues.ToString());
                                sbQryValues.Append(",");
                            }
                            auxSbQryValues = new StringBuilder();
                        }
                        else
                        {
                            if (sbQryValues.Length > 0 && sbQryValues.Length >= characterPerString)
                            {
                                string values = sbQryValues.ToString();
                                values = (values.EndsWith(",")) ? values.Trim(new char[] { ',' }) : values;
                                packageId = packageId + 1;
                                InitThread(values, countRow, packageRowLast, countRowAux, processId);
                                initialRowExcel = packageRowLast + 1;
                                countNew = countRow;
                                sbQryValues = new StringBuilder();
                                sbQryValues.Append("VALUES ");
                                sbQryValues.Append(auxSbQryValues.ToString());
                                if (auxSbQryValues != null)
                                {
                                    if (auxSbQryValues.Length > 0)
                                    {
                                        sbQryValues.Append(",");
                                    }
                                }
                            }
                        }
                    }

                    auxSbQryValues = new StringBuilder();
                    countRow++;
                    countRowAux++;
                    ConReg++;
                }

                if (sbQryValues.Length > 0)
                {
                    string values = sbQryValues.ToString();
                    values = (values.EndsWith(",")) ? values.Trim(new char[] { ',' }) : values;
                    packageId = packageId + 1;
                    InitThread(values, countRow, packageRowLast, countRowAux, processId);
                    countNew = countRow;
                }
            }
            catch (Exception ex)
            {
                logger.WriteEntry(ex.Message, writeLog);

            }

            stopWatch.Stop();
            Debug.WriteLine(stopWatch.Elapsed.ToString(), "Sistran.Company.Services.UploadFileServices.EEProvider.DAOs.InitExcelProcess");

            return Mle;
        }

        /// <summary>
        /// Initializes the thread.
        /// </summary>
        /// <param name="values">Valores del registro.</param>
        /// <param name="first">Fila inicial.</param>
        /// <param name="packageRowLast">Fila final.</param>
        /// <param name="countRowAux">Numero de registros.</param>
        private void InitThread(string values, int first, int packageRowLast, int countRowAux, int processId)
        {


            int registros;
            int aux = countRowAux;
            MassiveLoadProcess localMassiveLoadProcess = new MassiveLoadProcess();
            localMassiveLoadProcess.MassiveLoadId = massivelp.MassiveLoadId;
            localMassiveLoadProcess.FieldSet = massivelp.FieldSet;
            localMassiveLoadProcess.UserId = massivelp.UserId;
            localMassiveLoadProcess.RowFirst = first;
            localMassiveLoadProcess.Values = values.Replace("processId", Convert.ToString(processId));

            MassiveLoadThreadDAO localMassiveLoadThread = new MassiveLoadThreadDAO(this.RequestProcessor);
            localMassiveLoadThread.setMassiveLoadProcess(localMassiveLoadProcess);

            localMassiveLoadThread.writeLog = this.writeLog;

            countRecordsByDelegate.ProcespolDelegate caller = new countRecordsByDelegate.ProcespolDelegate(localMassiveLoadThread.ProcessThread);

            if (countMassiveLoadThreads < massivelp.MaxThread)
            {
                IAsyncResult result = caller.BeginInvoke(aux, out registros, new AsyncCallback(finishThread), null);
                countMassiveLoadThreads++;
            }
            else
            {
                countRecordsByDelegate ct = new countRecordsByDelegate(caller);
                ct.recordsNumber = countRowAux;
                ct.threadId = Thread.CurrentThread.ManagedThreadId;
                stack.Push(ct);
            }
        }

        /// <summary>
        /// Proceso del hilo.
        /// Envia la información al provider. 
        /// </summary>
        /// <param name="records">Registros.</param>
        /// <param name="recordsCont">Registros cont.</param>
        private void ProcessThread(int records, out int recordsCont)
        {
            UploadFileDAO provider = new UploadFileDAO();
            ExecFinishEventArgs eventArgs = new ExecFinishEventArgs();
            recordsCont = records;
            try
            {
                provider.InsertMassiveLoad(this.massivelp.UserId, this.massivelp.MassiveLoadId, this.massivelp.FieldSet, this.massivelp.Values, this.massivelp.TemporalId);
            }
            catch (Exception ex)
            {
                logger.WriteEntry(ex.Message, writeLog);
            }
        }


        /// <summary>
        /// Finalización del hilo aux.
        /// </summary>
        private void FinishThreadAux()
        {
            int intRespuesta = 0;
            UploadFileDAO provider = new UploadFileDAO();
            lock (objLock)
            {
                intRespuesta = provider.FindMassiveLoad(massivelp.MassiveLoadId);
            }

            if (intRespuesta != 0)
            {
                //Se Actualiza en el Store
                if (!this.processInit)
                {
                    this.processInit = true;
                    try
                    {
                        provider.ValidateMassiveLoadByMassiveId(massivelp.MassiveLoadId, massivelp.FieldSet);
                    }
                    catch (Exception ex)
                    {
                        logger.WriteEntry(ex.Message, writeLog);
                    }
                }

            }
        }

        /// <summary>
        /// Finalización del hilo.
        /// </summary>
        /// <param name="ias">Estado de la operación.</param>
        private void finishThread(IAsyncResult ias)
        {

            int MaxThreadAux;
            int registros = 0;
            int regAux = 0;
            int contregAux = 0;
            int countMassiveLoadThreadsAux = 0;
            Stack stk;
            countRecordsByDelegate th2;
            countRecordsByDelegate.ProcespolDelegate p;
            p = null;
            lock (packageLock)
            {
                packageInit = packageInit + 1;
            }
            AsyncResult pExtendedReturn = (AsyncResult)ias;
            countRecordsByDelegate.ProcespolDelegate delegateInfo = (countRecordsByDelegate.ProcespolDelegate)pExtendedReturn.AsyncDelegate;
            delegateInfo.EndInvoke(out registros, ias);

            recordsCount = recordsCount + registros;
            if (countMassiveLoadThreads > 0)
            {
                countMassiveLoadThreads--;
            }
            countMassiveLoadThreadsAux = countMassiveLoadThreads;
            MaxThreadAux = massivelp.MaxThread;
            stk = stack;
            contregAux = recordsCount;

            if (countMassiveLoadThreadsAux < MaxThreadAux && stk.Count > 0)
            {
                lock (objLock)
                {
                    if (stk.Count > 0)
                    {
                        th2 = (countRecordsByDelegate)stk.Pop();
                        p = th2.delegado;
                        regAux = th2.recordsNumber;
                        stack = stk;
                    }
                }
                if (p != null)
                {
                    IAsyncResult result = p.BeginInvoke(regAux, out registros, new AsyncCallback(finishThread), null);
                    lock (objLock)
                    {
                        this.countMassiveLoadThreads++;
                    }
                }
            }
            if (packageInit >= packageId && CountThead >= totalRows)
            {
                FinishThreadAux();
            }
        }


        /// <summary>
        /// Update Assincronuos Process.
        /// </summary>
        /// <param name="ProcessId">Identificador del proceso.</param>
        /// <param name="ErrorMessage">Mensaje de error.</param>
        private void UpdateAssincronuos(int ProcessId, string ErrorMessage)
        {
            try
            {
                UploadFileDAO provider = new UploadFileDAO();
                provider.UpdateAsynchronousProcessByProcessId(ProcessId, ErrorMessage);
            }
            catch (Exception ex)
            {
                logger.WriteEntry(ex.Message, writeLog);
            }
        }


    }
}
