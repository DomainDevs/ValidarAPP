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
    public class AGENCY_COMMISS_CURRENCYFactory
    {

        #region data Members

        AGENCY_COMMISS_CURRENCY_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public AGENCY_COMMISS_CURRENCYFactory()
        {
            _dataObject = new AGENCY_COMMISS_CURRENCY_ABM();
        }

        public AGENCY_COMMISS_CURRENCYFactory(string Connection)
        {
            _dataObject = new AGENCY_COMMISS_CURRENCY_ABM(Connection);
        }


        public AGENCY_COMMISS_CURRENCYFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new AGENCY_COMMISS_CURRENCY_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new AGENCY_COMMISS_CURRENCY
        /// </summary>
        /// <param name="businessObject">AGENCY_COMMISS_CURRENCY object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(AGENCY_COMMISS_CURRENCY businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing AGENCY_COMMISS_CURRENCY
        /// </summary>
        /// <param name="businessObject">AGENCY_COMMISS_CURRENCY object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(AGENCY_COMMISS_CURRENCY businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(AGENCY_COMMISS_CURRENCY businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get AGENCY_COMMISS_CURRENCY by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public AGENCY_COMMISS_CURRENCY GetByPrimaryKey(AGENCY_COMMISS_CURRENCYKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all AGENCY_COMMISS_CURRENCYs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<AGENCY_COMMISS_CURRENCY> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of AGENCY_COMMISS_CURRENCY by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<AGENCY_COMMISS_CURRENCY> GetAllBy(AGENCY_COMMISS_CURRENCY.AGENCY_COMMISS_CURRENCYFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        

        ///// <summary>
        ///// delete AGENCY_COMMISS_CURRENCY by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(AGENCY_COMMISS_CURRENCY.AGENCY_COMMISS_CURRENCYFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<AGENCY_COMMISS_CURRENCY> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (AGENCY_COMMISS_CURRENCY mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "AGENCY_COMMISS_CURRENCY";
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
