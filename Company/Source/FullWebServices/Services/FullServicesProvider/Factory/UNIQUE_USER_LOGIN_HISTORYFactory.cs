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
    public class UNIQUE_USER_LOGIN_HISTORYFactory
    {

        #region data Members

        UNIQUE_USER_LOGIN_HISTORY_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public UNIQUE_USER_LOGIN_HISTORYFactory()
        {
            _dataObject = new UNIQUE_USER_LOGIN_HISTORY_ABM();
        }

        public UNIQUE_USER_LOGIN_HISTORYFactory(string Connection)
        {
            _dataObject = new UNIQUE_USER_LOGIN_HISTORY_ABM(Connection);
        }


        public UNIQUE_USER_LOGIN_HISTORYFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new UNIQUE_USER_LOGIN_HISTORY_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new UNIQUE_USER_LOGIN_HISTORY
        /// </summary>
        /// <param name="businessObject">UNIQUE_USER_LOGIN_HISTORY object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(UNIQUE_USER_LOGIN_HISTORY businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing UNIQUE_USER_LOGIN_HISTORY
        /// </summary>
        /// <param name="businessObject">UNIQUE_USER_LOGIN_HISTORY object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(UNIQUE_USER_LOGIN_HISTORY businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(UNIQUE_USER_LOGIN_HISTORY businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get UNIQUE_USER_LOGIN_HISTORY by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public UNIQUE_USER_LOGIN_HISTORY GetByPrimaryKey(UNIQUE_USER_LOGIN_HISTORYKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all UNIQUE_USER_LOGIN_HISTORYs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<UNIQUE_USER_LOGIN_HISTORY> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of UNIQUE_USER_LOGIN_HISTORY by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<UNIQUE_USER_LOGIN_HISTORY> GetAllBy(UNIQUE_USER_LOGIN_HISTORY.UNIQUE_USER_LOGIN_HISTORYFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        

        ///// <summary>
        ///// delete UNIQUE_USER_LOGIN_HISTORY by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(UNIQUE_USER_LOGIN_HISTORY.UNIQUE_USER_LOGIN_HISTORYFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<UNIQUE_USER_LOGIN_HISTORY> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (UNIQUE_USER_LOGIN_HISTORY mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "UNIQUE_USER_LOGIN_HISTORY";
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
