using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Sistran.Co.Previsora.Application.FullServicesProvider.DataLayer;
using Sistran.Co.Previsora.Application.FullServicesProvider.Helpers;
using Sistran.Co.Previsora.Application.FullServices.Models.DataLayer;
using Sistran.Co.Previsora.Application.FullServices.Models;
using Sybase.Data.AseClient;


namespace Sistran.Co.Previsora.Application.FullServicesProvider.BussinesLayer
{
    public class FINANCIAL_STATEMENTSFactory
    {

        #region data Members

        FINANCIAL_STATEMENTS_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public FINANCIAL_STATEMENTSFactory()
        {
            _dataObject = new FINANCIAL_STATEMENTS_ABM();
        }

        public FINANCIAL_STATEMENTSFactory(string Connection)
        {
            _dataObject = new FINANCIAL_STATEMENTS_ABM(Connection);
        }


        public FINANCIAL_STATEMENTSFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new FINANCIAL_STATEMENTS_ABM(Connection, userId, AppId, Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new FINANCIAL_STATEMENTS
        /// </summary>
        /// <param name="businessObject">FINANCIAL_STATEMENTS object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(FINANCIAL_STATEMENTS businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing FINANCIAL_STATEMENTS
        /// </summary>
        /// <param name="businessObject">FINANCIAL_STATEMENTS object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(FINANCIAL_STATEMENTS businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(FINANCIAL_STATEMENTS businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get FINANCIAL_STATEMENTS by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public FINANCIAL_STATEMENTS GetByPrimaryKey(FINANCIAL_STATEMENTSKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all FINANCIAL_STATEMENTSs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<FINANCIAL_STATEMENTS> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of FINANCIAL_STATEMENTS by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<FINANCIAL_STATEMENTS> GetAllBy(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}



        ///// <summary>
        ///// delete FINANCIAL_STATEMENTS by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(FINANCIAL_STATEMENTS.FINANCIAL_STATEMENTSFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<FINANCIAL_STATEMENTS> businessObject, List<TableMessage> ListTableMessage)
        {
            foreach (FINANCIAL_STATEMENTS mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
                tMessage.Message = "True";
                tMessage.NameTable = "FINANCIAL_STATEMENTS";
                try
                {
                    if (mp.State.Equals('C'))
                        tMessage.Message = Insert(mp).ToString();
                    else if (mp.State.Equals('U'))
                        tMessage.Message = Update(mp).ToString();
                    else if (mp.State.Equals('D'))
                        tMessage.Message = Delete(mp).ToString();
                }
                catch (SupException ex)
                {
                    tMessage.Message = ex.Source;
                    continue;
                }
                finally
                {
                    ListTableMessage.Add(tMessage);
                }

            }
            return ListTableMessage;
        }
        #endregion

    }
}
