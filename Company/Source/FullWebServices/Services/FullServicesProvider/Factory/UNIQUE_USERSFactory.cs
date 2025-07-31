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
    public class UNIQUE_USERSFactory
    {

        #region data Members

        UNIQUE_USERS_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public UNIQUE_USERSFactory()
        {
            _dataObject = new UNIQUE_USERS_ABM();
        }

        public UNIQUE_USERSFactory(string Connection)
        {
            _dataObject = new UNIQUE_USERS_ABM(Connection);
        }


        public UNIQUE_USERSFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new UNIQUE_USERS_ABM(Connection, userId, AppId,Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new UNIQUE_USERS
        /// </summary>
        /// <param name="businessObject">UNIQUE_USERS object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(UNIQUE_USERS businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing UNIQUE_USERS
        /// </summary>
        /// <param name="businessObject">UNIQUE_USERS object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(UNIQUE_USERS businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(UNIQUE_USERS businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get UNIQUE_USERS by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public UNIQUE_USERS GetByPrimaryKey(UNIQUE_USERSKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all UNIQUE_USERSs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<UNIQUE_USERS> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of UNIQUE_USERS by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<UNIQUE_USERS> GetAllBy(UNIQUE_USERS.UNIQUE_USERSFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}

        

        ///// <summary>
        ///// delete UNIQUE_USERS by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(UNIQUE_USERS.UNIQUE_USERSFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<UNIQUE_USERS> businessObject, List<TableMessage> ListTableMessage)
        {            
            foreach (UNIQUE_USERS mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
tMessage.Message = "True";
                tMessage.NameTable = "UNIQUE_USERS";
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
