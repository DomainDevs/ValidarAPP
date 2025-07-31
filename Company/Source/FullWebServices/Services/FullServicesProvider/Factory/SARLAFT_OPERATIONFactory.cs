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
    public class SARLAFT_OPERATIONFactory
    {

        #region data Members

        SARLAFT_OPERATION_ABM _dataObject = null;

        #endregion

        #region Constructor

        /// <summary>
        /// Class constructor
        /// </summary>
        /// 
        public SARLAFT_OPERATIONFactory()
        {
            _dataObject = new SARLAFT_OPERATION_ABM();
        }

        public SARLAFT_OPERATIONFactory(string Connection)
        {
            _dataObject = new SARLAFT_OPERATION_ABM(Connection);
        }


        public SARLAFT_OPERATIONFactory(string Connection, string userId, int AppId, AseCommand Command)
        {
            _dataObject = new SARLAFT_OPERATION_ABM(Connection, userId, AppId, Command);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Insert new SARLAFT_OPERATION
        /// </summary>
        /// <param name="businessObject">SARLAFT_OPERATION object</param>
        /// <returns>true for successfully saved</returns>
        public bool Insert(SARLAFT_OPERATION businessObject)
        {
            return _dataObject.Insert(businessObject);
        }

        /// <summary>
        /// Update existing SARLAFT_OPERATION
        /// </summary>
        /// <param name="businessObject">SARLAFT_OPERATION object</param>
        /// <returns>true for successfully saved</returns>
        public bool Update(SARLAFT_OPERATION businessObject)
        {
            return _dataObject.Update(businessObject);
        }

        /// <summary>
        /// delete by primary key
        /// </summary>
        /// <param name="keys">primary key</param>
        /// <returns>true for succesfully deleted</returns>
        public bool Delete(SARLAFT_OPERATION businessObject)
        {
            return _dataObject.Delete(businessObject);
        }

        #region Metodos Comentados

        ///// <summary>
        ///// get SARLAFT_OPERATION by primary key.
        ///// </summary>
        ///// <param name="keys">primary key</param>
        ///// <returns>Student</returns>
        //public SARLAFT_OPERATION GetByPrimaryKey(SARLAFT_OPERATIONKeys keys)
        //{
        //    return _dataObject.SelectByPrimaryKey(keys); 
        //}

        ///// <summary>
        ///// get list of all SARLAFT_OPERATIONs
        ///// </summary>
        ///// <returns>list</returns>
        //public List<SARLAFT_OPERATION> GetAll()
        //{
        //    return _dataObject.SelectAll(); 
        //}

        ///// <summary>
        ///// get list of SARLAFT_OPERATION by field
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>list</returns>
        //public List<SARLAFT_OPERATION> GetAllBy(SARLAFT_OPERATION.SARLAFT_OPERATIONFields fieldName, object value)
        //{
        //    return _dataObject.SelectByField(fieldName.ToString(), value);  
        //}



        ///// <summary>
        ///// delete SARLAFT_OPERATION by field.
        ///// </summary>
        ///// <param name="fieldName">field name</param>
        ///// <param name="value">value</param>
        ///// <returns>true for successfully deleted</returns>
        //public bool Delete(SARLAFT_OPERATION.SARLAFT_OPERATIONFields fieldName, object value)
        //{
        //    return _dataObject.DeleteByField(fieldName.ToString(), value); 
        //}

        #endregion

        /// <summary>
        /// Process Data in Mpersona_telef
        /// </summary>
        /// <param name="businessObject">Mpersona_telef object</param>
        /// <returns>true for successfully saved</returns>
        public List<TableMessage> Process(List<SARLAFT_OPERATION> businessObject, List<TableMessage> ListTableMessage)
        {
            foreach (SARLAFT_OPERATION mp in businessObject)
            {
                TableMessage tMessage = new TableMessage();
                tMessage.Message = "True";
                tMessage.NameTable = "SARLAFT_OPERATION";
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
